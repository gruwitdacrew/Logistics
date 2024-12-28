using Logistics.Data;
using Logistics.Data.Account.AccountDTOs.Requests;
using Logistics.Data.Account.AccountDTOs.Responses;
using Logistics.Data.Account.Models;
using Logistics.Data.Accounts.DTOs.Requests;
using Logistics.Data.Accounts.DTOs.Responses;
using Logistics.Data.Common;
using Logistics.Data.Common.CommonDTOs.Responses;
using Logistics.Data.Common.DTOs.Responses;
using Logistics.Data.Documents.Models;
using Logistics.Services.Utils.EmailService;
using Logistics.Services.Utils.TokenGenerator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace Logistics.Services
{
    public class UserService
    {
        AppDBContext _context;
        TokenGenerator _tokenGenerator;
        EmailService _emailService;
        public UserService(AppDBContext context, TokenGenerator tokenGenerator, EmailService emailService)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
            _emailService = emailService;
        }

        private User getUserById(Guid userId)
        {
            User? user = _context.Users.Where(x => x.id == userId).FirstOrDefault();
            if (user == null)
            {
                throw new ErrorException(404, "Пользователь с таким id не найден");
            }
            return user;
        }

        private Shipper getShipperById(Guid shipperId)
        {
            Shipper? shipper = _context.Shippers.Where(x => x.id == shipperId).FirstOrDefault();
            if (shipper == null)
            {
                throw new ErrorException(404, "Грузоотправитель с таким id не найден");
            }
            return shipper;
        }

        private Transporter getTransporterById(Guid transporterId)
        {
            Transporter? transporter = _context.Transporters.Where(x => x.id == transporterId).FirstOrDefault();
            if (transporter == null)
            {
                throw new ErrorException(404, "Перевозчик с таким id не найден");
            }
            return transporter;
        }



        public async Task<ActionResult> Logout(Guid userId)
        {
            User user = getUserById(userId);

            user.token = null;

            _context.Update(user);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> Refresh(Guid userId, string lastRefreshToken)
        {
            User user = getUserById(userId);

            if (user.token != lastRefreshToken) return new UnauthorizedObjectResult("");

            var refreshToken = _tokenGenerator.GenerateToken(user, Token.Refresh);
            var accessToken = _tokenGenerator.GenerateAccessToken(user, user.role);

            user.token = refreshToken;

            _context.Update(user);
            _context.SaveChanges();

            return new OkObjectResult(new TokenResponse(user, accessToken, refreshToken));
        }

        public async Task<ActionResult> SetNewPassword(Guid userId, string? oldPassword, string password)
        {
            User user = getUserById(userId);

            if ((user.password == null) != (oldPassword == null)) return new ConflictObjectResult(new ErrorResponse(409, "Старый пароль передается - если не сброшен, не передается - если сброшен"));

            if (oldPassword != null && Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(oldPassword))) != user.password) return new UnauthorizedObjectResult(new ErrorResponse(401, "Старый пароль не подходит"));

            user.password = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password)));

            _context.Update(user);
            _context.SaveChanges();

            return new OkObjectResult("");
        }
        
        public async Task<ActionResult> ResetPasswordRequest(string login)
        {
            User? user = _context.Users.Where(x => x.email == login).FirstOrDefault();
            if (user == null) return new NotFoundObjectResult(new ErrorResponse(404, "Пользователя с такой почтой нет"));

            var resetPasswordToken = _tokenGenerator.GenerateToken(user, Token.ResetPassword);
            await _emailService.SendResetPasswordRequest(user.email, resetPasswordToken);

            return new OkObjectResult("");
        }

        public async Task<ActionResult> ResetPassword(Guid userId)
        {
            User user = getUserById(userId);

            user.password = null;
            user.token = null;

            _context.Users.Update(user);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> ApproveEmail(Guid userId, bool isEmailOwner)
        {
            User user = getUserById(userId);
            PendingEmail? email = _context.PendingEmails.Where(x => x.user == user).FirstOrDefault();

            if (email == null) return new ConflictObjectResult(new ErrorResponse(409, "Эл. почта не может быть подтверждена: либо на нее не регистрировались, либо ее подтвердил другой пользователь."));

            if (isEmailOwner)
            {
                user.email = email.value;

                var refreshToken = _tokenGenerator.GenerateToken(user, Token.Refresh);
                var accessToken = _tokenGenerator.GenerateAccessToken(user, user.role);

                user.token = refreshToken;

                _context.PendingEmails.Where(x => x.value == email.value).ExecuteDelete();
                _context.Update(user);
                _context.SaveChanges();

                return new OkObjectResult(new TokenResponse(user, accessToken, refreshToken));
            }
            else
            {
                _context.PendingEmails.Remove(email);
                _context.SaveChanges();
                return new OkObjectResult("");
            }
        }

        public async Task<ActionResult> Register(Role role, RegisterRequestDTO registerRequest)
        {
            User user;

            ErrorProblemDetails problemDetails = new ErrorProblemDetails(400);

            if (_context.Users.Where(x => x.email == registerRequest.email).FirstOrDefault() != null)
            {
                problemDetails.addError("email", "На эту электронную почту уже зарегистрирован пользователь");
            }
            if (_context.Users.Where(x => x.phone == registerRequest.phone).FirstOrDefault() != null)
            {
                problemDetails.addError("phone", "Пользователь с таким телефоном уже зарегистрирован");
            }
            if (problemDetails.errors.Count > 0) return new BadRequestObjectResult(problemDetails);

            if (role == Role.Shipper)
            {
                user = new Shipper(registerRequest);
                _context.Shippers.Add(user as Shipper);
            }
            else
            {
                user = new Transporter(registerRequest);
                _context.Transporters.Add(user as Transporter);
            }
            PendingEmail email = new PendingEmail(user, registerRequest.email);

            _context.PendingEmails.Add(email);
            _context.SaveChanges();

            var approveEmailToken = _tokenGenerator.GenerateToken(user, Token.ApproveEmail);
            await _emailService.SendApproveEmailRequest(registerRequest.email, approveEmailToken);

            return new OkObjectResult("");
        }

        public async Task<ActionResult> Login(LoginRequestDTO loginRequest)
        {
            User? user = _context.Users.Where(x => x.email == loginRequest.email).FirstOrDefault();

            ErrorProblemDetails problemDetails = new ErrorProblemDetails(400);

            if (user == null)
            {
                problemDetails.addError("email", "Пользователь с таким email не найден");
            }
            else if (user.password != Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(loginRequest.password))))
            {
                problemDetails.addError("password", "Неправильный пароль");
            }
            if (problemDetails.errors.Count > 0) return new BadRequestObjectResult(problemDetails);

            var refreshToken = _tokenGenerator.GenerateToken(user, Token.Refresh);
            var accessToken = _tokenGenerator.GenerateAccessToken(user, user.role);

            user.token = refreshToken;

            _context.Users.Update(user);
            _context.SaveChanges();

            return new OkObjectResult(new TokenResponse(user, accessToken, refreshToken));
        }

        private void EditUser(User user, EditUserRequestDTO editUserRequest)
        {
            ErrorProblemDetails errorProblemDetails = new ErrorProblemDetails(400);

            if (editUserRequest.email != user.email && _context.Users.Where(x => x.email == editUserRequest.email).FirstOrDefault() != null)
            {
                errorProblemDetails.addError("email", "На эту электронную почту уже зарегистрирован пользователь");
            }
            if (editUserRequest.phone != user.phone && _context.Users.Where(x => x.phone == editUserRequest.phone).FirstOrDefault() != null)
            {
                errorProblemDetails.addError("phone", "Пользователь с таким телефоном уже зарегистрирован");
            }
            if (errorProblemDetails.errors.Count > 0) throw new ErrorCollectionException(errorProblemDetails.status, errorProblemDetails.errors);

            user.edit(editUserRequest);

            if (editUserRequest.email != user.email)
            {
                PendingEmail email = new PendingEmail(user, editUserRequest.email);
                _context.PendingEmails.Add(email);

                var approveEmailToken = _tokenGenerator.GenerateToken(user, Token.ApproveEmail);
                _emailService.SendApproveEmailRequest(editUserRequest.email, approveEmailToken);
            }

            _context.Users.Update(user);
        }

        public async Task<ActionResult> EditShipper(Guid shipperId, EditShipperRequestDTO editRequest)
        {
            Shipper shipper = getShipperById(shipperId);

            EditUser(shipper, editRequest);

            _context.Shippers.Update(shipper);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> EditTransporter(Guid transporterId, EditTransporterRequestDTO editRequest)
        {
            Transporter transporter = getTransporterById(transporterId);

            EditUser(transporter, editRequest);

            transporter.permanentResidence = editRequest.permanentResidence;

            _context.Transporters.Update(transporter);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> EditCompany(Guid userId, EditCompanyRequestDTO editRequest)
        {
            User user = getUserById(userId);

            ErrorProblemDetails problemDetails = new ErrorProblemDetails(400);

            if (editRequest.organizationalForm == OrganizationForm.Individual && editRequest.companyName!=null)
            {
                problemDetails.addError("companyName", "У физического лица нет названия компании");
            }
            else if (editRequest.organizationalForm != OrganizationForm.Individual && editRequest.companyName == null)
            {
                problemDetails.addError("companyName", "Укажите название компании");
            }
            if ((editRequest.INN.Length == 10) != (editRequest.organizationalForm != OrganizationForm.Individual))
            {
                problemDetails.addError("INN", "Для физ. лица ИНН составляет 12 цифр, для юр. лица - 10");
            }
            else if (_context.Users.Where(x => x.company.INN == editRequest.INN).FirstOrDefault() != null)
            {
                problemDetails.addError("INN", "Этот ИНН уже есть в системе");
            }
            if (problemDetails.errors.Count > 0) throw new ErrorCollectionException(problemDetails.status, problemDetails.errors);

            user.company.edit(editRequest);

            _context.Users.Update(user);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> ShipperProfile(Guid shipperId)
        {
            Shipper shipper = getShipperById(shipperId);

            return new OkObjectResult(new ShipperProfileResponse(shipper));
        }

        public async Task<ActionResult> TransporterProfile(Guid transporterId)
        {
            Transporter transporter = getTransporterById(transporterId);

            return new OkObjectResult(new TransporterProfileResponse(transporter));
        }

        public async Task<ActionResult> AboutCompany(Guid userId)
        {
            User user = getUserById(userId);

            return new OkObjectResult(user.company);
        }

        public async Task<ActionResult> UploadPhoto(Guid userId, IFormFile file)
        {
            User user = getUserById(userId);

            if (file == null || file.Length == 0)
                return new UnprocessableEntityObjectResult(new ErrorResponse(422, "Файл не выбран"));

            if ("image/png" != file.ContentType)
                return new UnprocessableEntityObjectResult(new ErrorResponse(422, "Файл должен быть фотографией png"));

            byte[] fileData;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileData = memoryStream.ToArray();
            }

            user.photo = fileData;

            _context.Users.Update(user);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> GetPhoto(Guid userId)
        {
            User user = getUserById(userId);

            if (user.photo == null) { return new NotFoundObjectResult(new ErrorResponse(404, "Фотография не указана")); }

            return new OkObjectResult(user.photo);
        }

        public async Task<ActionResult> DeletePhoto(Guid userId)
        {
            User user = getUserById(userId);

            if (user.photo == null) { return new NotFoundObjectResult(new ErrorResponse(404, "Фотография не указана")); }

            user.photo = null;

            _context.Users.Update(user);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> CreateTransporterTruck(Guid transporterId, CreateTruckRequestDTO createRequest)
        {
            Transporter transporter = getTransporterById(transporterId);

            Truck truck = _context.Trucks.Where(x => x.transporterId == transporterId).FirstOrDefault();
            if (truck != null) return new ConflictObjectResult(new ErrorResponse(409, "У вас уже указан транспорт"));

            truck = new Truck(createRequest);
            transporter.truck = truck;

            _context.Trucks.Add(truck);
            _context.Transporters.Update(transporter);
            _context.SaveChanges();

            return new OkObjectResult("");
        }
        
        public async Task<ActionResult> EditTransporterTruck(Guid transporterId, EditTruckRequestDTO editRequest)
        {
            Truck truck = _context.Trucks.Where(x => x.transporterId == transporterId).FirstOrDefault();
            if (truck == null) return new NotFoundObjectResult(new ErrorResponse(404, "У вас не указан транспорт"));

            truck.edit(editRequest);

            _context.Trucks.Update(truck);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> GetTransporterTruck(Guid transporterId)
        {
            Truck truck = _context.Trucks.Where(x => x.transporterId == transporterId).FirstOrDefault();
            if (truck == null) return new NotFoundObjectResult(new ErrorResponse(404, "У вас не указан транспорт"));

            return new OkObjectResult(new TruckResponse(truck));
        }
    }
}
