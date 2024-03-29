using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication.Data;
using WebApplication.Models;
using WebApplication.Helper;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        PasswordSecurify passwordSecurify = new PasswordSecurify();

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            // 查詢資料
            User user = db.Users.FirstOrDefault(u => u.UserName == username);

            // 0:HashPassword 1:Salt
            string[] passwordHash = user.Password.Split(':');
            bool isPasswordPass = passwordSecurify.VerifyPassword(password, passwordHash[0], passwordHash[1]);

            // 驗證資料
            if (user == null || !isPasswordPass)
            {
                ModelState.AddModelError("", "帳號或密碼錯誤");
                return View("Index");
            }

            FormsAuthentication.SetAuthCookie(username, false);

            // 導向首頁
            return RedirectToAction("Index", "Home");
        }

        // GET: Account
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register([Bind(Include = "ID,UserName,Email,Password,UserPermissions")] User user)
        {
            user.UserPermissions = Permissions.User;

            if (ModelState.IsValid)
            {
                string salt = passwordSecurify.GenerateSalt();
                string passwordHash = passwordSecurify.HashPassword(user.Password, salt);

                user.Password = $"{passwordHash}:{salt}";

                user.ID = Guid.NewGuid();
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        public ActionResult Logout()
        {
            // 清除身份验证 cookie
            FormsAuthentication.SignOut();

            // 可以选择重定向到登出页面或任何其他页面
            return RedirectToAction("Index", "Home");
        }
    }
}