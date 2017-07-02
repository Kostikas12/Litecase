using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Litecase.Models;
using Litecase.DAL;
using System.Data.SqlClient;

namespace Litecase.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {            
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Footer()
        {
            return PartialView();
        }

        public ActionResult Panel()
        {
            HttpCookie cookie = Request.Cookies["cookielitecase"];
            if(cookie != null)
            {
                using(DataContext context = new DataContext())
                {
                    int id_sessioncookie = int.Parse(cookie.Value);
                    int num = context.Sessions.Count(x => x.ID_Session == id_sessioncookie);
                    if (num!=0)
                    {
                        DateTime timestart = context.Sessions.FirstOrDefault(x => x.ID_Session == id_sessioncookie).TimeStart; // check time
                        int difftime = (DateTime.Now - timestart).Minutes;

                        if (difftime<=10)
                        {
                            Sessions sessionupdate = context.Sessions.Where(x => x.ID_Session == id_sessioncookie).FirstOrDefault();
                            sessionupdate.TimeStart = DateTime.Now;
                            context.SaveChanges(); // update database

                            cookie.Expires = DateTime.Now.AddDays(10);
                            Response.Cookies.Add(cookie);  // update cookies

                            int id_user = context.Sessions.FirstOrDefault(x => x.ID_Session == id_sessioncookie).ID_User;

                            ViewBag.mail = context.Users.FirstOrDefault(x => x.ID_User == id_user).Mail;
                        }
                        else
                        {
                            context.Sessions.RemoveRange(context.Sessions.Where(x => x.ID_Session == id_sessioncookie));
                            cookie.Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies.Add(cookie);
                        }
                    }
                }
            }
            return PartialView();
        }

        public ActionResult Exit()
        {
            HttpCookie cookie = Request.Cookies["cookielitecase"];
            if (cookie !=null)
            {
                int id_sessioncookie = int.Parse(cookie.Value);
                using(DataContext context = new DataContext())
                {
                    context.Sessions.RemoveRange(context.Sessions.Where(x => x.ID_Session == id_sessioncookie));
                    context.SaveChanges();
                }
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            return View("Index");
        }

        public ActionResult TryRegister(string name, string mail, string password1, string password2)
        {
            if (name.Length == 0 || mail.Length == 0 || password1.Length == 0 || password2.Length == 0)
            {
                ViewBag.ErrorMessage = "Не все поля заполнены!";
                ViewBag.name = name;
                ViewBag.mail = mail;
            }
            else
            {
                if (!password1.Equals(password2))
                {
                    ViewBag.ErrorMessage = "Пароли не совпадают!";
                    ViewBag.name = name;
                    ViewBag.mail = mail;
                }
                else
                {
                    int num;
                    using (DataContext context = new DataContext())
                    {
                        num = context.Users.Count(x => x.Mail == mail);
                    }
                    if (num != 0)
                    {
                        ViewBag.ErrorMessage = "Данная почта уже используется!";
                        ViewBag.name = name;
                        ViewBag.mail = mail;
                    }
                    else
                    {   // Регистрация
                        using (DataContext context = new DataContext())
                        {
                            User user = new User(name, password1.GetHashCode().ToString(), mail);
                            context.Users.Add(user);
                            context.SaveChanges();
                        }
                        ViewBag.mailregistered = mail;
                        return View("Login");
                    }
                }
            }
            return View("Register");
        }

        public ActionResult EnterLogin(string mail, string password)
        {
            if (mail.Length == 0 || password.Length == 0)
            {
                ViewBag.ErrorMessage = "Не все поля заполнены!";
                ViewBag.mail = mail;
                return View("Login");
            }
            else
            {
                int num;
                string q = password.GetHashCode().ToString();
                using(DataContext context = new DataContext())
                {
                    num = context.Users.Count(x => x.Mail == mail && x.Password == q);
                }
                if (num == 0)
                {
                    ViewBag.ErrorMessage = "Почта или пароль указаны не верно!";
                    ViewBag.mail = mail;
                    return View("Login");
                }
                else
                {
                    using (DataContext context = new DataContext())
                    {
                        int id_user = context.Users.First(x=>x.Mail == mail && x.Password == q).ID_User; // get id_user

                        context.Sessions.RemoveRange(context.Sessions.Where(x=>x.ID_User == id_user));  // clear

                        Sessions session = new Sessions(id_user, DateTime.Now);    // making the new session
                        context.Sessions.Add(session);
                        context.SaveChanges();

                        int id_session = context.Sessions.First(x=>x.ID_User == id_user).ID_Session;   // get id 

                        HttpCookie cookie = new HttpCookie("cookielitecase");        
                        cookie.Value = id_session.ToString();
                        cookie.Expires = DateTime.Now.AddDays(10);
                        Response.Cookies.Add(cookie);

                        return View("RoomUser");
                    }
                }
            }
        }

        public ActionResult RoomUser()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

    }
}