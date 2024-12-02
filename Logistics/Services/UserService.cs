using Logistics.Data;
using Logistics.Data.Account.AccountDTOs.Requests;
using Logistics.Data.Account.AccountDTOs.Responses;
using Logistics.Data.Account.Models;
using Logistics.Data.Accounts.DTOs.Requests;
using Logistics.Data.Accounts.DTOs.Responses;
using Logistics.Data.Common.CommonDTOs.Responses;
using Logistics.Data.Common.DTOs.Responses;
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

        public async Task<ActionResult> Logout(Guid userId, string refreshToken)
        {
            User? user = _context.Users.Where(x => x.id == userId).FirstOrDefault();
            if (user != null && user.token == refreshToken)
            {
                user.token = null;

                _context.Update(user);
                _context.SaveChanges();

                return new OkObjectResult("");
            }
            return new UnauthorizedObjectResult("");
        }

        public async Task<ActionResult> Refresh(Guid userId, string lastRefreshToken)
        {
            User? user = _context.Users.Where(x => x.id == userId).FirstOrDefault();
            if (user != null && user.token == lastRefreshToken)
            {
                var refreshToken = _tokenGenerator.GenerateToken(user, Token.Refresh);
                var accessToken = _tokenGenerator.GenerateAccessToken(user, user.role);

                user.token = refreshToken;

                _context.Update(user);
                _context.SaveChanges();

                return new OkObjectResult(new TokenResponse(accessToken, refreshToken));
            }
            return new UnauthorizedObjectResult("");
        }

        public async Task<ActionResult> SetNewPassword(Guid userId, string? oldPassword, string password)
        {
            User? user = _context.Users.Where(x => x.id == userId).FirstOrDefault();

            if (user == null) return new UnauthorizedObjectResult("");

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
            User? user = _context.Users.Where(x => x.id == userId).FirstOrDefault();
            if (user == null) return new UnauthorizedObjectResult("");

            user.password = null;
            user.token = null;

            _context.Users.Update(user);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> ApproveEmail(Guid userId, bool isEmailOwner)
        {
            User user = _context.Users.Where(x => x.id == userId).FirstOrDefault();
            PendingEmail? email = _context.PendingEmails.Where(x => x.user == user).FirstOrDefault();

            if (email == null) return new ConflictObjectResult(new ErrorResponse(409, "Эл. почта не может быть подтверждена: либо на нее не регистрировались, либо ее подтвердил другой пользователь."));

            if (isEmailOwner)
            {
                if (user == null) return new UnauthorizedObjectResult("");

                user.email = email.value;

                var refreshToken = _tokenGenerator.GenerateToken(user, Token.Refresh);
                var accessToken = _tokenGenerator.GenerateAccessToken(user, user.role);

                user.token = refreshToken;

                _context.PendingEmails.Where(x => x.value == email.value).ExecuteDelete();
                _context.Update(user);
                _context.SaveChanges();

                return new OkObjectResult(new TokenResponse(accessToken, refreshToken));
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

            if (_context.Users.Where(x => x.email == registerRequest.email).FirstOrDefault() != null)
            {
                return new ConflictObjectResult(new ErrorResponse(409, "На эту электронную почту уже зарегистрирован пользователь"));
            }
            else if (_context.Users.Where(x => x.phone == registerRequest.phone).FirstOrDefault() != null)
            {
                return new ConflictObjectResult(new ErrorResponse(409, "Пользователь с таким телефоном уже зарегистрирован"));
            }

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
            if (user == null)
            {
                return new NotFoundObjectResult(new ErrorResponse(404, "Пользователь с таким email не найден"));
            }
            else if (user.password != Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(loginRequest.password))))
            {
                return new UnauthorizedObjectResult(new ErrorResponse(401, "Неправильный пароль"));
            }

            var refreshToken = _tokenGenerator.GenerateToken(user, Token.Refresh);
            var accessToken = _tokenGenerator.GenerateAccessToken(user, user.role);

            user.token = refreshToken;

            _context.Users.Update(user);
            _context.SaveChanges();

            return new OkObjectResult(new TokenResponse(accessToken, refreshToken));
        }

        public async Task<ActionResult> Edit(Guid userId, EditRequestDTO editRequest)
        {
            User? user = _context.Users.Where(x => x.id == userId).FirstOrDefault();
            if (user == null)
            {
                return new UnauthorizedObjectResult("");
            }

            if (_context.Users.Where(x => x.email == editRequest.email).FirstOrDefault() != null)
            {
                return new ConflictObjectResult(new ErrorResponse(409, "На эту электронную почту уже зарегистрирован пользователь"));
            }
            if (_context.Users.Where(x => x.phone == editRequest.phone).FirstOrDefault() != null)
            {
                return new ConflictObjectResult(new ErrorResponse(409, "Пользователь с таким телефоном уже зарегистрирован"));
            }

            if (editRequest.fullName != null) user.fullName = editRequest.fullName;
            if (editRequest.phone != null) user.phone = editRequest.phone;
            if (editRequest.email != null)
            {
                PendingEmail email = new PendingEmail(user, editRequest.email);
                _context.PendingEmails.Add(email);

                var approveEmailToken = _tokenGenerator.GenerateToken(user, Token.ApproveEmail);
                await _emailService.SendApproveEmailRequest(editRequest.email, approveEmailToken);
            }

            _context.Users.Update(user);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> EditCompany(Guid userId, EditCompanyRequestDTO editRequest)
        {
            User? user = _context.Users.Where(x => x.id == userId).FirstOrDefault();
            if (user == null)
            {
                return new UnauthorizedObjectResult("");
            }

            if (editRequest.organizationalForm == OrganizationForm.Individual && editRequest.companyName!=null)
            {
                return new BadRequestObjectResult(new ErrorResponse(400, "У физического лица нет названия компании"));
            }
            if ((editRequest.INN.Length == 10) == (editRequest.organizationalForm != OrganizationForm.Individual))
            {
                return new BadRequestObjectResult(new ErrorResponse(400, "Для физ. лица ИНН составляет 12 цифр, для юр. лица - 10"));
            }
            if (_context.Users.Where(x => x.company.INN == editRequest.INN).FirstOrDefault() != null)
            {
                return new ConflictObjectResult(new ErrorResponse(409, "Этот ИНН уже есть в системе"));
            }

            user.company.organizationalForm = editRequest.organizationalForm;
            if (editRequest.companyName != null) user.company.companyName = editRequest.companyName;
            user.company.INN = editRequest.INN;

            _context.Users.Update(user);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> Profile(Guid userId)
        {
            User? user = _context.Users.Where(x => x.id == userId).FirstOrDefault();
            if (user == null)
            {
                return new UnauthorizedObjectResult("");
            }
            return new OkObjectResult(new ProfileResponse(user));
        }

        public async Task<ActionResult> AboutCompany(Guid userId)
        {
            User? user = _context.Users.Where(x => x.id == userId).FirstOrDefault();
            if (user == null)
            {
                return new UnauthorizedObjectResult("");
            }
            return new OkObjectResult(user.company);
        }

        public async Task<ActionResult> CreateTransporterTruck(Guid transporterId, CreateTruckRequestDTO createRequest)
        {
            Transporter? transporter = _context.Transporters.Where(x => x.id == transporterId).FirstOrDefault();
            if (transporter == null)
            {
                return new UnauthorizedObjectResult("");
            }

            Truck truck = new Truck(createRequest);
            transporter.truck = truck;

            _context.Transporters.Update(transporter);
            _context.SaveChanges();

            return new OkObjectResult(null);
        }
        
        public async Task<ActionResult> EditTransporterTruck(Guid transporterId, EditTruckRequestDTO editRequest)
        {
            Transporter? transporter = _context.Transporters.Where(x => x.id == transporterId).FirstOrDefault();
            if (transporter == null)
            {
                return new UnauthorizedObjectResult("");
            }

            transporter.truck.edit(editRequest);

            _context.Transporters.Update(transporter);
            _context.SaveChanges();

            return new OkObjectResult(null);
        }

        public async Task<ActionResult> GetTransporterTruck(Guid transporterId)
        {
            Transporter? transporter = _context.Transporters.Where(x => x.id == transporterId).FirstOrDefault();
            if (transporter == null)
            {
                return new UnauthorizedObjectResult("");
            }

            return new OkObjectResult(new TruckResponse(transporter.truck));
        }
    }
}
