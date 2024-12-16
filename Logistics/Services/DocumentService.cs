using Logistics.Data;
using Logistics.Data.Account.Models;
using Logistics.Data.Common.CommonDTOs.Responses;
using Logistics.Data.Documents.DTOs.Requests;
using Logistics.Data.Documents.DTOs.Responses;
using Logistics.Data.Documents.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Logistics.Services
{
    public class DocumentService
    {
        AppDBContext _context;
        public DocumentService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> CreatePassport(Guid userId, CreatePassportDTO createPassport)
        {
            User? user = _context.Users.Where(x => x.id == userId).FirstOrDefault();
            if (user == null) return new UnauthorizedObjectResult(null);

            Passport? passport = _context.Passports.Where(x => x.user == user).FirstOrDefault();

            if (passport != null) return new ConflictObjectResult(new ErrorResponse(409, "У вас уже указан паспорт"));

            passport = new Passport(createPassport, user);

            _context.Passports.Add(passport);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> EditPassport(Guid userId, EditPassportDTO editPassport)
        {
            Passport? passport = _context.Passports.Where(x => x.user.id == userId).FirstOrDefault();
            if (passport == null)
            {
                return new NotFoundObjectResult(new ErrorResponse(404, "Вы не указывали паспорт"));
            }

            passport.edit(editPassport);

            _context.Passports.Update(passport);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> GetPassport(Guid userId)
        {
            Passport? passport = _context.Passports.Where(x => x.user.id == userId).FirstOrDefault();

            if (passport == null)
            {
                return new NotFoundObjectResult(new ErrorResponse(404, "Вы не указывали паспорт"));
            }

            return new OkObjectResult(new PassportResponse(passport));
        }

        public async Task<ActionResult> DeletePassport(Guid userId)
        {
            Passport? passport = _context.Passports.Where(x => x.user.id == userId).FirstOrDefault();
            if (passport == null)
            {
                return new NotFoundObjectResult(new ErrorResponse(404, "Вы не указывали паспорт"));
            }

            _context.Passports.Remove(passport);
            _context.SaveChanges();

            return new OkObjectResult("");
        }


        public async Task<ActionResult> CreateDriverLicense(Guid transporterId, CreateDriverLicenseRequestDTO createLicense)
        {
            Transporter? transporter = _context.Transporters.Where(x => x.id == transporterId).FirstOrDefault();

            DriverLicense? driverLicense = _context.Licenses.Where(x => x.transporter == transporter).FirstOrDefault();

            if (driverLicense != null) return new ConflictObjectResult(new ErrorResponse(409, "У вас уже указано водительское удостоверение"));

            driverLicense = new DriverLicense(createLicense, transporter);

            _context.Licenses.Add(driverLicense);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> EditDriverLicense(Guid transporterId, EditDriverLicenseDTO editLicense)
        {
            DriverLicense license = _context.Licenses.Where(x => x.transporter.id == transporterId).FirstOrDefault();
            if (license == null)
            {
                return new NotFoundObjectResult(new ErrorResponse(404, "Вы не указывали водительское удостоверение"));
            }

            license.edit(editLicense);

            _context.Licenses.Update(license);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> GetDriverLicense(Guid transporterId)
        {
            DriverLicense license = _context.Licenses.Where(x => x.transporter.id == transporterId).FirstOrDefault();
            if (license == null)
            {
                return new NotFoundObjectResult(new ErrorResponse(404, "Вы не указывали водительское удостоверение"));
            }

            return new OkObjectResult(new DriverLicenseResponse(license));
        }

        public async Task<ActionResult> DeleteDriverLicense(Guid transporterId)
        {
            DriverLicense license = _context.Licenses.Where(x => x.transporter.id == transporterId).FirstOrDefault();
            if (license == null)
            {
                return new NotFoundObjectResult(new ErrorResponse(404, "Вы не указывали водительское удостоверение"));
            }

            _context.Licenses.Remove(license);
            _context.SaveChanges();

            return new OkObjectResult("");
        }


        public async Task<ActionResult> UploadPassportScan(Guid userId, IFormFile file)
        {
            Passport passport = _context.Passports.Where(p => p.user.id == userId).FirstOrDefault();

            if (passport == null) return new NotFoundObjectResult(new ErrorResponse(404, "Вы не указывали паспорт"));

            if (file == null || file.Length == 0)
                return new UnprocessableEntityObjectResult(new ErrorResponse(422, "Файл не выбран"));

            if (file.ContentType != "application/pdf")
                return new UnprocessableEntityObjectResult(new ErrorResponse(422, "Файл должен быть в формате pdf"));

            byte[] pdfFileData;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                pdfFileData = memoryStream.ToArray();
            }

            passport.scan = pdfFileData;

            _context.Passports.Update(passport);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> GetPassportScan(Guid userId)
        {
            Passport passport = _context.Passports.Where(p => p.user.id == userId).FirstOrDefault();

            if (passport == null) return new NotFoundObjectResult(new ErrorResponse(404, "Вы не указывали паспорт"));
            if (passport.scan == null) return new NotFoundObjectResult(new ErrorResponse(404, "У вас нет прикрепленного скана паспорта"));

            return new OkObjectResult(passport.scan);
        }

        public async Task<ActionResult> DeletePassportScan(Guid userId)
        {
            Passport passport = _context.Passports.Where(p => p.user.id == userId).FirstOrDefault();

            if (passport == null) return new NotFoundObjectResult(new ErrorResponse(404, "Вы не указывали паспорт"));
            if (passport.scan == null) return new NotFoundObjectResult(new ErrorResponse(404, "У вас нет прикрепленного скана паспорта"));

            passport.scan = null;

            _context.Passports.Update(passport); 
            _context.SaveChanges();

            return new OkObjectResult("");
        }


        public async Task<ActionResult> UploadLicenseScan(Guid transporterId, IFormFile file)
        {
            DriverLicense license = _context.Licenses.Where(p => p.transporter.id == transporterId).FirstOrDefault();

            if (license == null) return new NotFoundObjectResult(new ErrorResponse(404, "Вы не указывали водительское удостоверение"));

            if (file == null || file.Length == 0)
                return new UnprocessableEntityObjectResult(new ErrorResponse(422, "Файл не выбран"));

            if (file.ContentType != "application/pdf")
                return new UnprocessableEntityObjectResult(new ErrorResponse(422, "Файл должен быть в формате pdf"));

            byte[] pdfFileData;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                pdfFileData = memoryStream.ToArray();
            }

            license.scan = pdfFileData;

            _context.Licenses.Update(license);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

        public async Task<ActionResult> GetLicenseScan(Guid transporterId)
        {
            DriverLicense license = _context.Licenses.Where(p => p.transporter.id == transporterId).FirstOrDefault();

            if (license == null) return new NotFoundObjectResult(new ErrorResponse(404, "Вы не указывали водительское удостоверение"));
            if (license.scan == null) return new NotFoundObjectResult(new ErrorResponse(404, "У вас нет прикрепленного скана водительского удостоверения"));

            return new OkObjectResult(license.scan);
        }

        public async Task<ActionResult> DeleteLicenseScan(Guid transporterId)
        {
            DriverLicense license = _context.Licenses.Where(p => p.transporter.id == transporterId).FirstOrDefault();

            if (license == null) return new NotFoundObjectResult(new ErrorResponse(404, "Вы не указывали водительское удостоверение"));
            if (license.scan == null) return new NotFoundObjectResult(new ErrorResponse(404, "У вас нет прикрепленного скана водительского удостоверения"));

            license.scan = null;

            _context.Licenses.Update(license);
            _context.SaveChanges();

            return new OkObjectResult("");
        }

    }
}
