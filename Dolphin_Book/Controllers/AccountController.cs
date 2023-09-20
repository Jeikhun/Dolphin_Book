using Dolphin_Book.Areas.Admin.ViewModels;
using Dolphin_Book.Core.Constants;
using Dolphin_Book.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using static System.Net.Mime.MediaTypeNames;
using Dolphin_Book.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Dolphin_Book.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _env;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
        }
        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Dolphin_Book.ViewModels.AccountLoginVM vm)
        {
            if (!ModelState.IsValid) return View();

            var user = await _userManager.FindByEmailAsync(vm.Email);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Email yaxud şifrə yanlışdır...");
                return View();
            }
            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError(string.Empty, "Email Təsdiqlənməyib...");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email yaxud şifrə yanlışdır...");
                return View();
            }

            return RedirectToAction("index", "home");



        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Dolphin_Book.ViewModels.AccountRegisterVM model)
        {

            if (!ModelState.IsValid) return View();

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                Fullname = model.Fullname,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return View();
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var confirmationLink = Url.Action(nameof(ConfirmEmail), "account", new { token, email = user.Email }, Request.Scheme);
            var confirmationLink = Url.Action(action: "confirmemail", controller: "account", values: new { token = token, email = user.Email }, protocol: Request.Scheme);
            //var message = new Message(new string[] { user.Email }, "Email Confirmation", confirmationLink);
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("jeikhunjalil@gmail.com");
            mail.To.Add(user.Email);
            mail.Subject = "Confirm Email";
            mail.Body = $"<a href='{confirmationLink}'>Confirm Email</a>";
            string body = string.Empty;
            string path = Path.Combine(_env.WebRootPath, "assets", "Templates", "email.html");
            using (StreamReader SourceReader = System.IO.File.OpenText(path))
            {
                body = SourceReader.ReadToEnd();
            }
            body = body.Replace("{{Link}}", confirmationLink);
            body = body.Replace("{{Name}}", user.UserName);
            body = body.Replace("{{Text}}", "Confirm Email");
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            NetworkCredential networkCredential = new NetworkCredential("jeikhunjalil@gmail.com", "fdgxcltipvqqujug");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = networkCredential;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(mail);
            //_emailSender.SendEmail(message);

            await _userManager.AddToRoleAsync(user, UserRoles.User.ToString());
            TempData["register"] = "Please,verify your email";

            return RedirectToAction(nameof(VerifyEmail));
        }
        public IActionResult VerifyEmail()
        {
            return View();
        }
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                NotFound();
            }

            await _userManager.ConfirmEmailAsync(user, token);
            return RedirectToAction(nameof(Login));





        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return View();
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) { return NotFound(); }
            var token = _userManager.GeneratePasswordResetTokenAsync(user);
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Port = 7015;
            var result = Url.Action(action: "resetpassword", controller: "account", values: new { token = token.Result, email = email }, protocol: Request.Scheme);
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("jeikhunjalil@gmail.com");
            mail.To.Add(user.Email);
            mail.Subject = "Reset Password";
            mail.IsBodyHtml = true;
            string body = string.Empty;
            string path = Path.Combine(_env.WebRootPath, "assets", "Templates", "email.html");
            using (StreamReader SourceReader = System.IO.File.OpenText(path))
            {
                body = SourceReader.ReadToEnd();
            }
            body = body.Replace("{{Link}}", result);
            body = body.Replace("{{Name}}", user.UserName);
            body = body.Replace("{{Text}}", "Reset Password");
            mail.Body = body;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            NetworkCredential networkCredential = new NetworkCredential("jeikhunjalil@gmail.com", "fdgxcltipvqqujug");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = networkCredential;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Internet və Email ünvanı yoxlayın..");
                return View();
            }

            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
             if(token == null || email==null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) { return NotFound(); }
            ResetPassVM model = new ResetPassVM
            {
                Token = token,
                Email = email
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPassVM vm)
        {
            if(!ModelState.IsValid) {
                return View();
            
            }
            var user = await _userManager.FindByEmailAsync(vm.Email);
            if (user == null) { return NotFound(); }

            var result = await _userManager.ResetPasswordAsync(user, vm.Token, vm.Password);

            if (!result.Succeeded)
            {
                return Json(result.Errors);
            }



            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
            //return RedirectToAction("index", "home", new { area = "" });

        }
    }
}
