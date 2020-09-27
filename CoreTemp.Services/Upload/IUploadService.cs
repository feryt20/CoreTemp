using CoreTemp.Data.DTOs.Identity;
using CoreTemp.Data.DTOs.Upload;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreTemp.Services.Upload
{
    public interface IUploadService
    {
        Task<FileUploadedDto> UploadFile(IFormFile file, string userId, string WebRootPath, string UrlBegan, string UrlUrl);
        Task<FileUploadedDto> UploadFileToLocal(IFormFile file, string userId, string WebRootPath, string UrlBegan, string UrlUrl);
        FileUploadedDto RemoveFileFromLocal(string photoName, string WebRootPath, string filePath);
        ApiReturn<string> CreateDirectory(string WebRootPath, string UrlUrl);
    }
}
