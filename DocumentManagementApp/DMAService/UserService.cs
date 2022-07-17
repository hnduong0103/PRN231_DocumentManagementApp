using DataAccess;
using DataAccess.DBModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DMAService
{
    public class UserService
    {
        private readonly DMSDatabaseContext _context;
        private readonly IConfiguration _config;
        public UserService(DMSDatabaseContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        /*
         * GET LIST OF USERS
         */
        public List<User> GetAll()
        {
            var _users = _context.Users.ToList();
            return _users;
        }

        /*
         * CREATE USER
         */
        public bool Create(UserCreateModel user)
        {
            bool status;
            User item = new User();
            item.UserName = user.UserName;
            item.UserEmail = user.UserEmail;
            item.Password = user.Password;
            item.UserRole = user.UserRole;
            try
            {
                _context.Users.Add(item);
                _context.SaveChanges();
                SendMail(user.UserName, user.UserEmail, user.Password);
                status = true;
            }
            catch (Exception ex)
            {
                var exp = ex;
                status = false;
            }
            return status;
        }

        /*
         * DELETE USER
         */
        public bool Delete(int id)
        {
            bool status;
            var item = _context.Users.Find(id);
            try
            {
                _context.Users.Remove(item);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                var exp = ex;
                status = false;
            }
            return status;
        }

        /*
         * SEND EMAIL
         */
        public bool SendMail(string name, string email, string password)
        {
            var gmailAddress = _config.GetValue<string>("SendMail:Setting:Gmail");
            var gmailPassword = _config.GetValue<string>("SendMail:Setting:Password");

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            MailAddress from = new MailAddress(gmailAddress, "Admin-DMS");
            MailAddress to = new MailAddress(email, name);
            MailMessage message = new MailMessage(from, to);
            message.Body = "Hello," + name + "! Please use these credentials to sign in DMS Software. Email=" + email + " and password= " + password + "  .Thank you!";
            message.Subject = "DMS- USER LOGIN DETAILS";
            NetworkCredential myCreds = new NetworkCredential(gmailAddress, gmailPassword, "");
            client.Credentials = myCreds;
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                var exp = ex;
            }
            return true;
        }
    }
}
