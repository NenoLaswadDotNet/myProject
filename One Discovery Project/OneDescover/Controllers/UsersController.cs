using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OneDiscovery.Data;
using OneDiscovery.Model;
using OneDiscovery.ViewModels;
using OneDiscovery.Service;
using System.Configuration;
using Newtonsoft.Json;
using System.Web.Security;
using OneDiscovery.Security;
using Contoso.Filters;

namespace OneDiscovery.Controllers
{
    //Custom Exception Handling Filter
    [OneDiscoveryExceptionFilter]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            if (ShouldRedirectToHomePage())
                return RedirectToAction("Index", "Home");
            else
                return View("Register");
        }

        // POST
        [AllowAnonymous]//Available for any user
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            //Generate random salt with GUID.
            //Use SHA method to hash the password with salt
            //Store the hashed password, salt and other user info into the database after validation
            try
            {
                if (ModelState.IsValid)
                {
                    //First check if the user already exists in the database; if not, let him regist; else show error message
                    var userCheck = _userService.GetUserByUserName(model.Username);
                    if (userCheck == null)
                    {
                        var salt = Password.GenerateSalt();
                        var password = Password.Encode(model.Password, salt);
                        var user = new User
                        {
                            Email = model.Username,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Password = password,
                            Age = model.Age,
                            Phone = model.Phone,
                            Salt = salt,
                            FailedAttempts = 0
                            //After 3 times(in the Web.config) wrong attemp, the account will be locked for a specific period of time(declared in the Web.config)
                        };

                        _userService.CreateUser(user);
                        
                        //After registion, rediret the user to the login page
                        return RedirectToAction("Login", "Users");


                    } else {
                        //showing the error message for existing user
                        ModelState.AddModelError("exist_user_error", "User name already exists!");
                    }
                    
                }
                return View();
                
            }
            catch
            {
                throw;
            }
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            if (ShouldRedirectToHomePage())
                return RedirectToAction("Index", "Home");
            else
                return View("Login");
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //fetch the user data from the database
                    var user = _userService.GetUserByUserName(loginViewModel.Username);

                    if (user != null)
                    {
                        //check if the user is locked; if is locked, check if it fulfill the requirement of unlock
                        if (user.IsLocked)
                        {
                            //check how long the time has past after locked
                            TimeSpan diff = System.DateTime.Now - Convert.ToDateTime(user.LastLockedDateTime);
                            if (diff.TotalSeconds >= Convert.ToInt32(ConfigurationManager.AppSettings["LockedTime"]))
                            {
                                user.IsLocked = false;
                                user.FailedAttempts = 0;
                                _userService.UpdateUser(user);
                            }
                            else
                            {
                                var timeleft = Convert.ToInt32(ConfigurationManager.AppSettings["LockedTime"]) - diff.Seconds;
                                ModelState.AddModelError("", "Exceed Max Attemps, Try " + timeleft + " Seconds Later.");
                            }
                        }

                        if (!user.IsLocked)
                        {
                            //hash the user typed password with the salt from the database
                            //check if the hashed password match the one stored in the databse
                            var password = Security.Password.Encode(loginViewModel.Password, user.Salt);
                            if (user.Password == password)
                            {
                                var personRoles = user.Roles.Select(r => r.RoleName).ToArray();
                                var serializeModel = new OneDiscoveryPrincipleModel
                                {
                                    UserName = user.Email,
                                    FirstName = user.FirstName,
                                    LastName = user.LastName,
                                    Roles = personRoles
                                };
                                var userData = JsonConvert.SerializeObject(serializeModel);

                                //create the auth ticket for that user
                                var authTicket = new FormsAuthenticationTicket(1, user.Email, DateTime.Now,
                                    DateTime.Now.AddMinutes(15), false, userData);
                                var encTicket = FormsAuthentication.Encrypt(authTicket);
                                var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                                Response.Cookies.Add(faCookie);
                                
                                FormsAuthentication.SetAuthCookie(loginViewModel.Username, loginViewModel.RememberMe);

                                //login successfully, redirect to the Index page
                                return RedirectToAction("Index", "Home");

                            }
                            else
                            {
                                ModelState.AddModelError("", "Incorrect username and/or password");
                                //if the user is not locked, increase the fail attempts counter in the database
                                //if the user exceeds the max fail attempts, lock the account
                                if (!user.IsLocked)
                                {
                                    if (++user.FailedAttempts == Convert.ToInt32(ConfigurationManager.AppSettings["PasswordMaxAttempTimes"]))
                                    {
                                        user.LastLockedDateTime = System.DateTime.Now;
                                        user.IsLocked = true;
                                        ModelState.AddModelError("", "Exceed Max Attemps, Account is Locked!");
                                    }
                                    _userService.UpdateUser(user);
                                }

                            }
                        }

                    }
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect username and/or password");
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Users");
        }


        private bool ShouldRedirectToHomePage() {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                return authTicket != null;
            }
            else
            {
                return false;
            }
        }
    }
}
