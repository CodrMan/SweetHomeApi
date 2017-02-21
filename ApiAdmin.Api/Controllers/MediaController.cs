using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;

using ApiAdmin.Core.DTO;
using ApiAdmin.Services.Abstract;
using ApiAdmin.Services.Concrete;
using ApiAdmin.Api.Infrastructure;


namespace ApiAdmin.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [JsonWrapper]
    public class MediaController : BaseController
    {
        public MediaController(AppUserManager userManager, ISettingService settingService) : base(userManager, settingService)
        {
        }

        [HttpPost]
        [Route("uploadImage")]
        public IHttpActionResult UploadImageFile(long userId)
        {
            var files = HttpContext.Current.Request.Files;
            if (files.Count > 0)
            {
                var file = files[0];

                var filenameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                var filenameExtension = Path.GetExtension(file.FileName);
                var newFileName = filenameWithoutExtension + Guid.NewGuid() + filenameExtension;

                string appPath = SettingService.GetSettingParam("AppPath").ParamValue;
                string tempFolder = "Content\\" + SettingService.GetSettingParam("TempFolder").ParamValue;

                var user = UserManager.FindById(userId);
                var newFolder = Path.Combine(appPath, tempFolder, user.UserName);
                var tempPath = Path.Combine(newFolder, newFileName);

                CreateDirectory(newFolder);
                Image image = Image.FromStream(file.InputStream);
                image.Save(tempPath);

                HttpCode(HttpStatusCode.OK);
                return Ok(newFileName);
            }

            HttpCode(HttpStatusCode.NotFound);
            return Ok();
        }

        [HttpPost]
        [Route("confirmUploadImage")]
        public IHttpActionResult ConfirmUploadImageFile(ConfirmUploadImageDTO confirmUpload)
        {
            var user = UserManager.FindById(confirmUpload.UserId);
            string appPath = SettingService.GetSettingParam("AppPath").ParamValue;
            string tempFolder = "Content\\" + SettingService.GetSettingParam("TempFolder").ParamValue;
            string imageFolder = "Content\\" + SettingService.GetSettingParam("UserImageFolder").ParamValue;

            var tempPath = Path.Combine(appPath, tempFolder, user.UserName);
            var targetPath = Path.Combine(appPath, imageFolder, user.UserName);

            DeleteDirectory(targetPath);
            Thread.Sleep(100);
            if (CreateDirectory(targetPath))
            {
                File.Move(Path.Combine(tempPath, confirmUpload.ImageName), Path.Combine(targetPath, confirmUpload.ImageName));

                user.PhotoUri = confirmUpload.ImageName;
                UserManager.Update(user);
                return Ok();
            }

            HttpCode(HttpStatusCode.InternalServerError);
            HttpMessage("Image failed load");
            return Ok();
        }

        [HttpDelete]
        [Route("removeImage")]
        public IHttpActionResult RemoveImageByUserId(long userId)
        {
            var user = UserManager.FindById(userId);
            user.PhotoUri = null;
            var result = UserManager.Update(user);

            if (result.Succeeded)
            {
                var appPath = SettingService.GetSettingParam("AppPath").ParamValue;
                var userImageFolder = "Content\\" + SettingService.GetSettingParam("UserImageFolder").ParamValue;
                var targetPath = Path.Combine(appPath, userImageFolder, user.UserName);
                
                if (DeleteDirectory(targetPath))
                    return Ok();
                
                HttpMessage("Failed to remove the image from a folder.");
                return Ok();
            }

            HttpCode(HttpStatusCode.NotFound);
            HttpMessage(GetErrorResult(result.Errors));
            return Ok();
        }

        private bool DeleteDirectory(string path)
        {
            if (!Directory.Exists(path))
                return false;

            Directory.Delete(path, true);
            return true;
        }

        private bool CreateDirectory(string path)
        {
            if (Directory.Exists(path))
                return false;

            Directory.CreateDirectory(path);
            return true;
        }
    }
}
