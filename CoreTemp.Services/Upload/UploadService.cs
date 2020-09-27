using CoreTemp.Data.DatabaseContext;
using CoreTemp.Data.DTOs.Identity;
using CoreTemp.Data.DTOs.Upload;
using CoreTemp.Repo.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTemp.Services.Upload
{
    public class UploadService : IUploadService
    {
        private readonly IUnitOfWork<CoreTempDbContext> _db;
        public UploadService(IUnitOfWork<CoreTempDbContext> dbContext)
        {
            _db = dbContext;
        }
        public async Task<FileUploadedDto> UploadFile(IFormFile file, string userId, string WebRootPath, string UrlBegan, string Url)
        {
            return await UploadFileToLocal(file, userId, WebRootPath, UrlBegan, Url);
        }

        public async Task<FileUploadedDto> UploadFileToLocal(IFormFile file, string userId,
            string WebRootPath, string UrlBegan, string Url)
        {
            if (file.Length > 0)
            {
                try
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string fileExtention = Path.GetExtension(fileName);
                    string fileNewName = string.Format("{0}{1}", userId, fileExtention);
                    string path = Path.Combine(WebRootPath, Url);
                    string fullPath = Path.Combine(path, fileNewName);

                    var dirRes = CreateDirectory(WebRootPath, Url);
                    if (!dirRes.Status)
                    {
                        return new FileUploadedDto()
                        {
                            Status = false,
                            Message = dirRes.Message
                        };
                    }

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return new FileUploadedDto()
                    {
                        Status = true,
                        LocalUploaded = true,
                        Message = "با موفقیت در لوکال آپلود شد",
                        PublicId = "0",
                        Url = $"{UrlBegan}/{"wwwroot/" + Url.Split('\\').Aggregate("", (current, str) => current + (str + "/")) + fileNewName}"
                    };
                    //Url = $"{UrlBegan}/{"wwwroot/" + Url.Split('\\').Aggregate("", (current, str) => current + (str + "/")) + fileNewName}"

                }
                catch (Exception ex)
                {
                    return new FileUploadedDto()
                    {
                        Status = false,
                        Message = ex.Message
                    };
                }
            }
            else
            {
                return new FileUploadedDto()
                {
                    Status = false,
                    Message = "فایلی برای اپلود یافت نشد"
                };
            }
        }



        public FileUploadedDto RemoveFileFromLocal(string photoName, string WebRootPath, string filePath)
        {

            string path = Path.Combine(WebRootPath, filePath);
            string fullPath = Path.Combine(path, photoName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return new FileUploadedDto()
                {
                    Status = true,
                    Message = "فایل با موفقیت حذف شد"
                };
            }
            else
            {
                return new FileUploadedDto()
                {
                    Status = true,
                    Message = "فایل وجود نداشت"
                };
            }
        }

        public ApiReturn<string> CreateDirectory(string WebRootPath, string Url)
        {
            try
            {
                var path = Path.Combine(WebRootPath, Url);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return new ApiReturn<string>
                {
                    Status = true
                };

            }
            catch (Exception ex)
            {
                return new ApiReturn<string>
                {
                    Status = false,
                    Message = ex.Message
                };
            }

        }

    }
}
