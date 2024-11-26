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
            User user = _context.Users.Where(x => x.id == userId).FirstOrDefault();

            Passport passport = new Passport(createPassport, user);
            
            _context.Passports.Add(passport);
            _context.SaveChanges();

            return new OkObjectResult(null);
        }

        public async Task<ActionResult> EditPassport(Guid userId, EditPassportDTO editPassport)
        {
            Passport passport = _context.Passports.Where(x => x.user.id == userId).FirstOrDefault();

            if (passport == null)
            {
                return new NotFoundObjectResult(new ErrorResponse(404, "Вы не заносили паспорт в систему"));
            }

            passport.edit(editPassport);

            _context.Passports.Update(passport);
            _context.SaveChanges();

            return new OkObjectResult(null);
        }

        public async Task<ActionResult> GetPassport(Guid userId)
        {
            Passport passport = _context.Passports.Where(x => x.user.id == userId).FirstOrDefault();

            if (passport == null)
            {
                return new NotFoundObjectResult(new ErrorResponse(404, "Вы не заносили паспорт в систему"));
            }

            return new OkObjectResult(new PassportResponse(passport));
        }

        public async Task<ActionResult> DeletePassport(Guid userId)
        {
            Passport passport = _context.Passports.Where(x => x.user.id == userId).FirstOrDefault();
            if (passport == null)
            {
                return new NotFoundObjectResult(new ErrorResponse(404, "Вы не заносили паспорт в систему"));
            }

            _context.Passports.Remove(passport);
            _context.SaveChanges();

            return new OkObjectResult(null);
        }


        public async Task<ActionResult> CreateDriverLicense(Guid userId, CreateDriverLicenseRequestDTO createLicense)
        {
            Transporter transporter = _context.Transporters.Where(x => x.id == userId).FirstOrDefault();

            DriverLicense driverLicense = new DriverLicense(createLicense, transporter);

            _context.Licenses.Add(driverLicense);
            _context.SaveChanges();

            return new OkObjectResult(null);
        }

        public async Task<ActionResult> EditDriverLicense(Guid userId, EditDriverLicenseDTO editLicense)
        {
            DriverLicense license = _context.Licenses.Where(x => x.transporter.id == userId).FirstOrDefault();
            if (license == null)
            {
                return new NotFoundObjectResult(new ErrorResponse(404, "Вы не заносили водительское удостоверение в систему"));
            }

            license.edit(editLicense);

            _context.Licenses.Update(license);
            _context.SaveChanges();

            return new OkObjectResult(null);
        }

        public async Task<ActionResult> GetDriverLicense(Guid userId)
        {
            DriverLicense license = _context.Licenses.Where(x => x.transporter.id == userId).FirstOrDefault();
            if (license == null)
            {
                return new NotFoundObjectResult(new ErrorResponse(404, "Вы не заносили водительское удостоверение в систему"));
            }

            return new OkObjectResult(new DriverLicenseResponse(license));
        }

        public async Task<ActionResult> DeleteDriverLicense(Guid userId)
        {
            DriverLicense license = _context.Licenses.Where(x => x.transporter.id == userId).FirstOrDefault();
            if (license == null)
            {
                return new NotFoundObjectResult(new ErrorResponse(404, "Вы не заносили водительское удостоверение в систему"));
            }

            _context.Licenses.Remove(license);
            _context.SaveChanges();

            return new OkObjectResult(null);
        }


        public async Task<ActionResult> UploadPassportScan(Guid userId, byte[] file)
        {
            Passport passport = _context.Passports.Where(p => p.user.id == userId).FirstOrDefault();

            if (passport == null) return new NotFoundObjectResult(new ErrorResponse(404, "Вы не указывали паспорт"));
            passport.scan = file;

            _context.Passports.Update(passport);
            _context.SaveChangesAsync();

            return new OkObjectResult(null);
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
            _context.SaveChangesAsync();

            return new OkObjectResult(null);
        }


        public async Task<ActionResult> UploadLicenseScan(Guid userId, byte[] file)
        {
            DriverLicense license = _context.Licenses.Where(p => p.transporter.id == userId).FirstOrDefault();

            if (license == null) return new NotFoundObjectResult(new ErrorResponse(404, "Вы не указывали водительское удостоверение"));
            license.scan = file;

            _context.Licenses.Update(license);
            _context.SaveChangesAsync();

            return new OkObjectResult(null);
        }

        public async Task<ActionResult> GetLicenseScan(Guid userId)
        {
            DriverLicense license = _context.Licenses.Where(p => p.transporter.id == userId).FirstOrDefault();

            if (license == null) return new NotFoundObjectResult(new ErrorResponse(404, "Вы не указывали водительское удостоверение"));
            if (license.scan == null) return new NotFoundObjectResult(new ErrorResponse(404, "У вас нет прикрепленного скана водительского удостоверения"));

            return new OkObjectResult(license.scan);
        }

        public async Task<ActionResult> DeleteLicenseScan(Guid userId)
        {
            DriverLicense license = _context.Licenses.Where(p => p.transporter.id == userId).FirstOrDefault();

            if (license == null) return new NotFoundObjectResult(new ErrorResponse(404, "Вы не указывали водительское удостоверение"));
            if (license.scan == null) return new NotFoundObjectResult(new ErrorResponse(404, "У вас нет прикрепленного скана водительского удостоверения"));

            license.scan = null;

            _context.Licenses.Update(license);
            _context.SaveChangesAsync();

            return new OkObjectResult(null);
        }

    }
}
