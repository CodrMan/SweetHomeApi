using System.Net;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using AutoMapper;

using SweetHome.Api.Helpers;
using SweetHome.Api.Models;
using SweetHome.Core.DTO;
using SweetHome.Core.Entities.Identity;
using SweetHome.Services.Abstract;
using SweetHome.Services.Concrete;


namespace SweetHome.Api.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : BaseController
    {
        private readonly IMessageService _messageService;

        public AccountController(AppUserManager userManager, ISettingService settingService, IMessageService messageService)
            : base(userManager, settingService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        [Route("Register")]
        public IHttpActionResult Register(RegisterUserDTO model)
        {
            if (EmailExists(model.Email))
            {
                HttpCode(HttpStatusCode.Forbidden);
                HttpMessage("We already have user with this email address.");

                return Ok();
            }

            if (!ModelState.IsValid || model.Password != model.ConfirmPassword)
            {
                HttpCode(HttpStatusCode.Forbidden);
                HttpMessage("Passwords doesn't match");

                return Ok();
            }
            
            var newUser = Mapper.Map(model, new AppUser());
            var result = UserManager.Create(newUser, model.Password);

            if (result.Succeeded)
            {
                AppUser addedUser = UserManager.Find(model.Email, model.Password);

                if (addedUser != null)
                {
                    string accessToken = TokenGenerator.GenerateToken(UserManager, addedUser);
                    HttpCode(HttpStatusCode.OK);

                    return Ok(new LoginResponseViewModel { Token = accessToken, UserId = addedUser.Id });
                }
            }

            HttpCode(HttpStatusCode.NotFound);
            HttpMessage("Something went wrong"); //TODO return actual error

            return Ok();
        }

        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login(LoginUserDTO model)
        {
            if (!string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(model.Password))
            {
                var user = UserManager.Find(model.Email, model.Password);
                if (user != null)
                {
                    string accessToken = TokenGenerator.GenerateToken(UserManager, user);
                    HttpCode(HttpStatusCode.OK);

                    return Ok(new LoginResponseViewModel() {Token = accessToken, UserId = user.Id});
                }

                HttpCode(HttpStatusCode.NotFound);
                HttpMessage("User does not exist.");

                return Ok();
            }

            HttpCode(HttpStatusCode.BadRequest);
            HttpMessage("Something went wrong");

            return Ok();
        }

        [HttpPost]
        [Route("RestorePassword")]
        public IHttpActionResult RestorePassword(RestorePasswordDTO model)
        {
            var currentUser = UserManager.FindByEmail(model.Email);

            if (currentUser == null)
            {
                HttpCode(HttpStatusCode.Forbidden);
                HttpMessage("User with this email address not finded");

                return Ok();
            }

            var confimationToken = UserManager.GeneratePasswordResetToken(currentUser.Id);
            var newPassword = System.Web.Security.Membership.GeneratePassword(6, 0);

            UserManager.ResetPassword(currentUser.Id, confimationToken, newPassword);
            _messageService.AddRestorePasswordMessage(currentUser, newPassword);

            HttpCode(HttpStatusCode.OK);
            return Ok();
        }

        private bool EmailExists(string email)
        {
            return UserManager.FindByEmail(email) != null;
        }
    }
}
