using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shippery.Models.Database;
using Shippery.Models.Resources;
using Shippery.Models.Responses;
using MimeKit;

namespace Shippery.Controllers
{
    [Route("UserController")]
    public class UserController : Controller
    {
        DatabaseDelegate databaseDelegate;

        public UserController()
        {
            databaseDelegate = new DatabaseDelegate(ConfigurationHelper.GetMySQLConnectionString());
        }

        [HttpPost("CheckUser")]
        public UserResponse CheckUser([FromBody] User u)
        {
            Console.WriteLine(u.Username);
            Console.WriteLine(u.Password);
            if (string.IsNullOrEmpty(u.Username) || string.IsNullOrEmpty(u.Password)) return new UserResponse(false, "Neither username or password can be empty!");
            else if (databaseDelegate.CheckPassword(u)) return new UserResponse(true);
            else return new UserResponse(false, "User does not exist!");          
        }

        [HttpGet("GetUser")]
        public User GetUser(string username) => databaseDelegate.GetUser(username);

        [HttpPost("AddUser")]
        public UserResponse AddUser([FromBody] User u)
        {
            try
            {
                if (databaseDelegate.AddUser(u))
                {
                    Console.WriteLine(u.Username + " added to the database");
                    //var mailMessage = new MimeMessage();
                    //mailMessage.From.Add(new MailboxAddress("Eloi Carbó", "eloicar@gmail.com"));
                    //mailMessage.To.Add(new MailboxAddress(u.Username, u.Mail));
                    //mailMessage.Subject = "Confirm Transmail registration";
                    //mailMessage.Body = new TextPart("html")
                    //{
                    //    Text = "Thank you for registering! We had send this email to confirm that the mail submitted to <a href=\"https://localhost:44333/\" target=\"_blank\">TransMail</a> is correct. <br />" +
                    //    "Before logging is necessary to confirm your email address or the system won't let you in. Use <a href=\"https://localhost:44333/TransConfirm/1/" + code + "\" target=\"_blank\">this link</a> to cofirm the account and have full access to the page.<br/><br/>" +
                    //    "In case of not seeing the link, copy and paste this link to the navigator: 'https://localhost:44333/TransConfirm/1/" + code + "' to confirm the account."
                    //};
                    //ConfigurationHelper.GetMailConnection().Send(mailMessage);
                    CookieHelper.SetCookie(this, "ALERT_MESSAGE", "A mail has been sent to your electronic inbox.\nCheck and verify your account to log!");
                }

                return new UserResponse(true);
            }
            catch (DatabaseUserUsernameDuplicatedException) {
                CookieHelper.SetCookie(this, "ALERT_MESSAGE", "The user alredy exists!");
                return new UserResponse(true);
            }
            catch (DatabaseUserEmailDuplicatedException)
            {
                CookieHelper.SetCookie(this, "ALERT_MESSAGE", "The electronic mail is alredy in use!");
                return new UserResponse(true);
            }
            catch (Exception e)
            {
                return new UserResponse(false, e.Message);
            }
        }

        [HttpPost("ResetUser")]
        public UserResponse ResetUser([FromBody] User u)
        {
            try
            {
                if (databaseDelegate.GetValid(u.Username, false))
                {
                    if (databaseDelegate.ResetUser(u))
                    {
                        //var mailMessage = new MimeMessage();
                        //mailMessage.From.Add(new MailboxAddress("TransMail support", "eloicar@gmail.com"));
                        //mailMessage.To.Add(new MailboxAddress(u.Username, u.Mail));
                        //mailMessage.Subject = "Password retrieval";
                        //mailMessage.Body = new TextPart("html")
                        //{
                        //    Text = "Hello dear " + u.Username + ",<br />" +
                        //    "Recently we have recived a petition to reset your account's password. This process was initiated and all fields fullfilled to reset your password. You can user your autoassigned password to enter to your account, <b>and change it!</b><br/>" +
                        //    "Actual password: " + pas + "<br /><br />" +
                        //    "If you hadn't initiate this process, please contact to our support team to ensure your account security.<br />" +
                        //    "Atte.<br />" +
                        //    "TransMail team"
                        //};
                        //ConfigurationHelper.GetMailConnection().Send(mailMessage);
                        CookieHelper.SetCookie(this, "ALERT_MESSAGE", "A mail has been sent to your electronic inbox with the new password!");
                    } else CookieHelper.SetCookie(this, "ALERT_MESSAGE", "The username or email address does not exist!\nCheck them and send again.");
                } else CookieHelper.SetCookie(this, "ALERT_MESSAGE", "The user exists but has de account suspended!");

                return new UserResponse(true);
            }
            catch (Exception e)
            {
                return new UserResponse(true, e.Message);
            }
        }
    }
}
