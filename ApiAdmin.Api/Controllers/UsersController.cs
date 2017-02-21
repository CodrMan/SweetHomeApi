using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using AutoMapper;

using ApiAdmin.Core.Entities.Identity;
using ApiAdmin.Services.Abstract;
using ApiAdmin.Services.Concrete;
using ApiAdmin.Api.Infrastructure;


namespace ApiAdmin.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [JsonWrapper]
    public class UsersController : BaseController
    {
        private readonly IMessageService _messageService;

        public UsersController(
            AppUserManager userManager,
            ISettingService settingService,
            IMessageService messageService) : base(userManager, settingService)
        {
            _messageService = messageService;
        }
        
        public IEnumerable<AppUser> GetUsers()
        {
            return UserManager.Users.ToList();
        }
        
        [ResponseType(typeof(AppUser))]
        public IHttpActionResult GetUser(long id)
        {
            var user = UserManager.FindById(id);
            if (user == null)
                return NotFound();

            user.PhotoUri = GetFullUrlImage(user.PhotoUri, user.UserName);  
            return Ok(user);
        }
        
        [ResponseType(typeof(void))]
        [HttpPut]
        public IHttpActionResult PutUser(AppUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var olduser = UserManager.FindById(user.Id);
            var newUser = Mapper.Map(user, olduser);
            newUser.Gender = CheckGender(user.Gender);
            newUser.PhotoUri = GetImageName(newUser.PhotoUri);
            UserManager.Update(newUser);

            return Ok(newUser);
        }
        
        [ResponseType(typeof(AppUser))]
        [HttpPost]
        public IHttpActionResult PostUser(AppUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var defaultPassword = SettingService.GetSettingParam("DefaultPassword").ParamValue;
            user.UserName = user.Email;
            user.Gender = CheckGender(user.Gender);
            var result = UserManager.Create(user, defaultPassword);

            if (result.Succeeded)
                return Ok();
            
            HttpCode(HttpStatusCode.NotFound);
            HttpMessage(GetErrorResult(result.Errors));
            return Ok();
        }
        
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteUser(long id)
        {
            _messageService.DeleteAllByUserId(id);
            var user = UserManager.FindById(id);
            if (user != null)
            {
                UserManager.Delete(user);
                return Ok();
            }

            HttpCode(HttpStatusCode.NotFound);
            HttpMessage("User not found");
            return Ok();
        }

        private static string CheckGender(string genderName)
        {
            if (genderName != "Male" && genderName != "Female")
                return "";

            return genderName;
        }

        private string GetFullUrlImage(string imgName, string userName)
        {
            if (imgName == null)
                return null;

            var siteUrl = SettingService.GetSettingParam("SiteUrl").ParamValue;
            var userImageFolder = SettingService.GetSettingParam("UserImageFolder").ParamValue;
            return siteUrl + "/Content/" + userImageFolder + "/" + userName + "/" + imgName;
        }

        private string GetImageName(string imgUrl)
        {
            if (imgUrl == null)
                return null;

            var arr = imgUrl.Split('/');
            return arr[arr.Length - 1];
        }
    }
}