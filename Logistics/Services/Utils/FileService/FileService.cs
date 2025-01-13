using System.IO;
using System.Security.AccessControl;
using Logistics.Data.Account.Models;
using Logistics.Services.Utils.EmailService;
using Logistics.Services.Utils.TokenGenerator;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;

namespace Logistics.Services
{
    public enum FileType
    {
        photo,
        passport,
        license
    }
    public class FileService
    {
        private IMinioClient _client;

        private FileServiceConfiguration fileServiceConfiguration = new FileServiceConfiguration();

        public FileService(IConfiguration configuration)
        {
            configuration.Bind("FileSettings", fileServiceConfiguration);

            _client = new MinioClient()
                .WithEndpoint(fileServiceConfiguration.Endpoint, 9000)
                .WithCredentials(fileServiceConfiguration.AccessKey, fileServiceConfiguration.SecretKey)
                .Build();

            foreach (FileType fileType in Enum.GetValues(typeof(FileType)))
            {
                if (!_client.BucketExistsAsync(new BucketExistsArgs().WithBucket(fileType.ToString())).Result)
                {
                    _client.MakeBucketAsync(new MakeBucketArgs().WithBucket(fileType.ToString()));
                }
            }
        }

        public async Task put(IFormFile file, FileType fileType, Guid fileId)
        {
            PutObjectArgs poa = new PutObjectArgs()
                .WithBucket(fileType.ToString())
                .WithObject(fileId.ToString())
                .WithStreamData(file.OpenReadStream())
                .WithObjectSize(file.Length)
                .WithContentType(file.ContentType);

            await _client.PutObjectAsync(poa);
        }

        public async Task<FileContentResult> get(FileType fileType, Guid? fileId)
        {
            if (fileId == null) return null;

            using (var memoryStream = new MemoryStream())
            {
                GetObjectArgs goa = new GetObjectArgs()
                    .WithBucket(fileType.ToString())
                    .WithObject(fileId.ToString())
                    .WithCallbackStream((stream) =>
                    {
                        stream.CopyTo(memoryStream);
                    });

                var metadata = await _client.GetObjectAsync(goa);

                return new FileContentResult(memoryStream.ToArray(), metadata.ContentType)
                {
                    FileDownloadName = metadata.ObjectName,
                };
            }
        }

        public async Task delete(FileType fileType, Guid? fileId)
        {
            RemoveObjectArgs roa = new RemoveObjectArgs()
                .WithBucket(fileType.ToString())
                .WithObject(fileId.ToString());

            await _client.RemoveObjectAsync(roa);
        }
    }
}
