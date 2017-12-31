using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoginDemo.Models;
using System.Net.Mail;
using System.Net;
using System.Web.Security;

namespace LoginDemo.Controllers
{
    public class UserController : Controller
    {
        //Registration Action
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        //Registraton Post Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified,ActivationCode")]User user)
        {
            bool status = false;
            string Message = "";


            //Model validation
            if (ModelState.IsValid)
            {
                #region    //Email is already exsit
                var Isexist = isEmailExist(user.Email);
                if (Isexist)
                {
                    ModelState.AddModelError("EmailError", "Email Already Exist..!");
                    return View(user);
                }
                #endregion


                #region Generate Activation Code
                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region Password Hashing
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                #endregion
                user.IsEmailVerified=false;

                #region Save Data To Database
                using (sampleDBEntities1 DB = new sampleDBEntities1())
                {
                    DB.Users.Add(user);
                    DB.SaveChanges();
                    //Email send
                    sendEmailVerificationCode(user.Email,user.ActivationCode.ToString());
                    Message = "Regestratin successfully Done, Account Activation link has be sent to you email ID"
                        + user.Email;
                    status = true;
                }

                #endregion
            }
            else
            {
                Message = "Invalid Request..!";
            }

            ViewBag.Message = Message;
            ViewBag.Status = status; 

        


            return View(user);
        }

        //Verify Account
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool status = false;
            using (sampleDBEntities1 sdb = new sampleDBEntities1())
            {

                sdb.Configuration.ValidateOnSaveEnabled = false;
                var v = sdb.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();

                if (v != null)
                {
                    v.IsEmailVerified = true;
                    sdb.SaveChanges();
                    status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request :(";
                }
            }
            ViewBag.Status = status;
                return View();
        }

        //Loin
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        //Login Post
        public ActionResult Login(UserLogin login, string ReturnUrl = "")
        {
            string message = "";
            using (sampleDBEntities1 sdb = new Models.sampleDBEntities1())
            {
                var v = sdb.Users.Where(a => a.Email == login.EmailID).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(Crypto.Hash(login.password), v.Password) == 0)

                    {
                        int timeout = login.RememberMe ? 525600 : 20; //525600 minutes==1Year
                        var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("index", "Home");
                        }
                    }
                    else
                    {
                        message = "Invalid credentials entered..! :(";
                    }
                }
                else
                {
                    message = "Invalid Credentials entered..! :(";
                }
            }
            ViewBag.Message = message;
            return View();
        }
        //Logout

        [Authorize]
        [HttpPost]
        public ActionResult logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }
        [NonAction]
        public bool isEmailExist(string email)
        {
            using (sampleDBEntities1 DE = new sampleDBEntities1())
            {
                var v = DE.Users.Where(a => a.Email== email).FirstOrDefault();
                return v!=null;
            }
        }
        [NonAction]
        public void sendEmailVerificationCode(string email, string VerificationCode)
        {
            //var scheme = Request.Url.Scheme;
            //var host = Request.Url.Host;
            //var port = Request.Url.Port;
            var verifyURL = "/User/VerifyAccount/" + VerificationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyURL);
            var fromEmail = new MailAddress("prakashraj2111@gmail.com", "Prakash Buchade");
            var toEmail = new MailAddress(email);
            var Emailpassword = "9270291116";
            string sub = "Your Account is successfully created";
            string body = "<br><BR> CONGRATULATIONS, Your Account is created successfully."
                + "To verify your email please click on below link.<br><br>"
                + "<a href='"+link+"'>"+link+"</a>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port=587,
                EnableSsl=true,
                DeliveryMethod=SmtpDeliveryMethod.Network,
                UseDefaultCredentials=false,
                Credentials=new NetworkCredential(fromEmail.Address,Emailpassword)
            };
            using (var Mail = new MailMessage(fromEmail, toEmail)
            {
                Subject = sub,
                Body = body,
                IsBodyHtml = true

            })
                smtp.Send(Mail);

        }
    }
}