using Fakefit.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace Fakefit.Controllers
{
    public class DefaultController : Controller
    {

        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        // GET: Default
        public ActionResult About()
        {
            return View();
        }
        // GET: Default
        public ActionResult Feature()
        {
            return View();
        }
        // GET: Default
        public ActionResult Contact()
        {

            var hata = TempData["hata"];
            ViewBag.hata = hata;

            TempData["hata"] = null;

            return View();
        }

        [HttpPost]
        public ActionResult Contact(FormCollection form)
        {
            var mesaj = TempData["mesaj"];
            TempData.Keep();

            FakefitEntities6 db = new FakefitEntities6();
            contact contact = new contact();

            var isim = form["name"].ToString();

            if(isim == "")
            {
                TempData["hata"] = "İsim boş olamaz..";
                return RedirectToAction("/Contact", "Default");
            }

            var mail = form["email"].ToString();
            if(mail == "")
            {
                TempData["hata"] = "Mail boş olamaz..";
                return RedirectToAction("/Contact", "Default");
            }

            var telefon = form["phone"].ToString();
            if(telefon == "")
            {
                TempData["hata"] = "Telefon boş olamaz..";
                return RedirectToAction("/Contact", "Default");
            }

            var konu = form["topic"].ToString();
            if(konu == "")
            {
                TempData["hata"] = "Konu boş olamaz..";
                return RedirectToAction("/Contact", "Default");
            }

            var açıklama = form["message"].ToString();
            if(açıklama == "")
            {
                TempData["hata"] = "Mesaj boş olamaz..";
                return RedirectToAction("/Contact", "Default");
            }
            contact.name = form["name"].ToString();
            contact.mail = form["email"].ToString();
            contact.topic = form["topic"].ToString();
            contact.message = form["message"].ToString();
            contact.phone = form["phone"].ToString();
            contact.date = DateTime.Now;
            db.contact.Add(contact);
            db.SaveChanges();

            TempData["hata"] = "Mesajınız başarıyla iletilmiştir..";
            return RedirectToAction("/Contact", "Default");
        }


        // GET: Default
        public ActionResult Login()
        {
           
            return View();
        }
        public ActionResult Alogin(register registerlogin)
        {
            using (FakefitEntities6 db = new FakefitEntities6()) {
                var kullanicivarmi = db.register.FirstOrDefault(
                    x=>x.username== registerlogin.username && x.password == registerlogin.password);

                var role = db.register.FirstOrDefault(
                    x => x.username == registerlogin.username && x.password == registerlogin.password && x.user == "Admin");

                var role1 = db.register.FirstOrDefault(
                    x => x.username == registerlogin.username && x.password == registerlogin.password && x.user == "Eğitmen");

                var role2 = db.register.FirstOrDefault(
                    x => x.username == registerlogin.username && x.password == registerlogin.password && x.user == "Üye");

                TempData["mesaj"] = registerlogin.username;

                if (kullanicivarmi != null && role != null)
                {
                    FormsAuthentication.SetAuthCookie(kullanicivarmi.username, false);
                    return RedirectToAction("/IndexAdmin/"+ registerlogin.username, "Default");
                }
                else if (kullanicivarmi != null && role1 != null)
                {
                    FormsAuthentication.SetAuthCookie(kullanicivarmi.username, false);
                    return RedirectToAction("/IndexInstructor/"+ registerlogin.username, "Default");
                }
                else if (kullanicivarmi != null && role2 != null)
                {
                    FormsAuthentication.SetAuthCookie(kullanicivarmi.username, false);
                    return RedirectToAction("/IndexUser/"+ registerlogin.username, "Default");
                }

                ViewBag.Hata = "Kullanıcı adı veya şifre hatalı";
                return View("Login");
            }
        }

        [Authorize]
        [Route("Default/Register/{mesaj}")]
        // GET: Default
        public ActionResult Register()
        {
            var mesaj = TempData["mesaj"];
            TempData.Keep();

            var hata = TempData["hata"];
            ViewBag.hata = hata;

            TempData["hata"] = null;

            FakefitEntities6 db = new FakefitEntities6();
            var model = db.register.ToList();
            return View(model);
            
            
        }
        // GET: Default


        [HttpPost]
        public ActionResult Register(FormCollection form)
        {
                var mesaj = TempData["mesaj"];
                TempData.Keep();

                FakefitEntities6 db = new FakefitEntities6();
                register register = new register();

                var isim = form["Name"].Trim();
                if(isim == "")
                {
                    TempData["hata"] = "İsim boş olamaz..";
                    return RedirectToAction("/Register/" + mesaj, "Default");
                }

                var soyisim = form["Surname"].Trim();
                if (soyisim == "")
                {
                    TempData["hata"] = "Soyisim boş olamaz..";
                    return RedirectToAction("/Register/" + mesaj, "Default");
                }

                var kullanıcı_adı = register.username = form["Username"].Trim();
                if (kullanıcı_adı == "")
                {
                    TempData["hata"] = "Kullanıcı adı boş olamaz..";
                    return RedirectToAction("/Register/" + mesaj, "Default");
                }

                var şifre = form["Password"].Trim();
                if (şifre == "")
                {
                    TempData["hata"] = "şifre boş olamaz..";
                    return RedirectToAction("/Register/" + mesaj, "Default");
                }
           
                var email = form["Email"].Trim();
                if (email == "")
                {
                    TempData["hata"] = "email boş olamaz..";
                    return RedirectToAction("/Register/" + mesaj, "Default");
                }

                var telefon = form["phone"].Trim();
                if (telefon == "")
                {
                    TempData["hata"] = "telefon boş olamaz..";
                    return RedirectToAction("/Register/" + mesaj, "Default");
                }

                
                var boy1 = form["Tall"].Trim();
                if (boy1 == "")
                {
                    TempData["hata"] = "Boy boş olamaz..";
                    return RedirectToAction("/Register/" + mesaj, "Default");
                }
                var boy = Convert.ToInt32(form["Tall"].Trim());
                
                var kilo1 = form["Kg"].Trim();
                if(kilo1 == "")
                {
                    TempData["hata"] = "Kg boş olamaz..";
                    return RedirectToAction("/Register/" + mesaj, "Default");
                }
                var kilo = Convert.ToInt32(form["Kg"].Trim());


                var ünvan = form["User"].Trim();
                if (ünvan == "----")
                {
                    TempData["hata"] = "Ünvan boş olamaz..";
                    return RedirectToAction("/Register/" + mesaj, "Default");
                }


                var başlangıç = Convert.ToString(form["StartDate"]);

                if (başlangıç == "")
                {
                    TempData["hata"] = "Bir başlangıç tarihi seçin..";
                    return RedirectToAction("/Register/" + mesaj, "Default");
                }

                var başlangıç1 = Convert.ToDateTime(form["StartDate"].Trim());
                if (başlangıç1 < DateTime.Today)
                {
                    TempData["hata"] = "Başlangıç tarihi bugünden eski olamaz..";
                    return RedirectToAction("/Register/" + mesaj, "Default");
                }


            var bitiş = Convert.ToString(form["FinishDate"]);
                if (bitiş == "")
                {
                    TempData["hata"] = "Bir bitiş tarihi seçin..";
                    return RedirectToAction("/Register/" + mesaj, "Default");
                }
                

                var bitiş1 = Convert.ToDateTime(form["FinishDate"].Trim());
                if (bitiş1 < DateTime.Today)
                {
                    TempData["hata"] = "Bitiş tarihi bugünden eski olamaz..";
                    return RedirectToAction("/Register/" + mesaj, "Default");
                }


            var kullanıcı_var_mı = db.register.FirstOrDefault(x=>x.username == kullanıcı_adı);
                if(kullanıcı_var_mı != null)
                {
                    TempData["hata"] = "Bu kullanıcı adı zaten mevcut..";
                    return RedirectToAction("/Register/" + mesaj, "Default");
                }

                var mail_var_mı = db.register.FirstOrDefault(x=>x.email ==email);
                if (mail_var_mı != null)
                {
                    TempData["hata"] = "Bu email zaten mevcut..";
                    return RedirectToAction("/Register/" + mesaj, "Default");
                }

                var telefon_var_mı = db.register.FirstOrDefault(x=>x.phone ==telefon);
                if (telefon_var_mı != null)
                {
                    TempData["hata"] = "Bu telefon numarası zaten mevcut..";
                    return RedirectToAction("/Register/" + mesaj, "Default");
                }

                var name = form["Name"].Trim().ToLower();
                var surname = form["Surname"].Trim().ToLower();
                var total = name + " " + surname;
                var eğitmenler = db.register.Where(x=>x.user == "Eğitmen").ToList();

                if (form["User"].Trim() == "Eğitmen")
                {
                    foreach (var item in eğitmenler)
                    {
                        var eğitmen_name = item.name.Trim().ToLower();
                        var eğitmen_surname = item.surname.Trim().ToLower();
                        var eğitmen_total = eğitmen_name + " " + eğitmen_surname;
                        if(total == eğitmen_total)
                    {
                        {
                            TempData["hata"] = "Aynı isimde 2 eğitmen olamaz..";
                            return RedirectToAction("/Register/" + mesaj, "Default");
                        }
                    }
                    }
                }
                


            register.name = form["Name"].Trim();
                register.surname = form["Surname"].Trim();
                register.username = form["Username"].Trim();
                register.password = form["Password"].Trim();
                register.email = form["Email"].Trim();
                register.phone = form["phone"].Trim();
                register.tall = Convert.ToInt32(form["Tall"].Trim());
                register.kg = Convert.ToInt32(form["Kg"].Trim());
                register.user = form["User"].Trim();
                register.startdate = Convert.ToDateTime(form["Startdate"].Trim());
                register.finishdate = Convert.ToDateTime(form["Finishdate"].Trim());
                db.register.Add(register);

                db.SaveChanges();





            TempData["hata"] = "Kayıt Başarıyla oluşturuldu..";
            return RedirectToAction("/Register/" + mesaj, "Default");
        }
        // GET: Default

        
        [Authorize]
        [Route("Default/IndexAdmin/{mesaj}")]
        public ActionResult IndexAdmin()
        {

            var mesaj = TempData["mesaj"];
            TempData.Keep();
            return View();
        }

        [Authorize]
        [Route("Default/FeatureAdmin/{mesaj}")]
        // GET: Default
        public ActionResult FeatureAdmin()
        {
            var mesaj = TempData["mesaj"];
            TempData.Keep();
            return View();
        }

        [Authorize]
        [Route("Default/IndexInstructor/{mesaj}")]
        // GET: Default
        public ActionResult IndexInstructor()
        {
            var mesaj = TempData["mesaj"];
            TempData.Keep();
            return View();
        }

        [Authorize]
        [Route("Default/IndexUser/{mesaj}")]
        // GET: Default
        public ActionResult IndexUser()
        {
            var mesaj = TempData["mesaj"];
            TempData.Keep();
            return View();
        }

        [Authorize]
        [Route("Default/ContactInstructor/{mesaj}")]
        // GET: Default
        public ActionResult ContactInstructor()
        {
            var mesaj = TempData["mesaj"];
            TempData.Keep();

            var hata = TempData["hata"];
            ViewBag.hata = hata;

            TempData["hata"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult ContactInstructor(FormCollection form)
        {
            var mesaj = TempData["mesaj"];
            TempData.Keep();

            FakefitEntities6 db = new FakefitEntities6();
            contact contact = new contact();

            var isim = form["name"].ToString();

            if (isim == "")
            {
                TempData["hata"] = "İsim boş olamaz..";
                return RedirectToAction("/ContactInstructor/" + mesaj, "Default");
            }

            var mail = form["email"].ToString();
            if (mail == "")
            {
                TempData["hata"] = "Mail boş olamaz..";
                return RedirectToAction("/ContactInstructor/" + mesaj, "Default");
            }

            var telefon = form["phone"].ToString();
            if (telefon == "")
            {
                TempData["hata"] = "Telefon boş olamaz..";
                return RedirectToAction("/ContactInstructor/" + mesaj, "Default");
            }

            var konu = form["topic"].ToString();
            if (konu == "")
            {
                TempData["hata"] = "Konu boş olamaz..";
                return RedirectToAction("/ContactInstructor/" + mesaj, "Default");
            }

            var açıklama = form["message"].ToString();
            if (açıklama == "")
            {
                TempData["hata"] = "Mesaj boş olamaz..";
                return RedirectToAction("/ContactInstructor/" + mesaj, "Default");
            }
            contact.name = form["name"].ToString();
            contact.mail = form["email"].ToString();
            contact.topic = form["topic"].ToString();
            contact.message = form["message"].ToString();
            contact.phone = form["phone"].ToString();
            contact.date = DateTime.Now;
            db.contact.Add(contact);
            db.SaveChanges();

            TempData["hata"] = "Mesajınız başarıyla iletilmiştir..";
            return RedirectToAction("/ContactInstructor/" + mesaj, "Default");
        }

        [Authorize]
        [Route("Default/ContactUser/{mesaj}")]
        // GET: Default
        public ActionResult ContactUser()
        {
            var mesaj = TempData["mesaj"];
            TempData.Keep();

            var hata = TempData["hata"];
            ViewBag.hata = hata;

            TempData["hata"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult ContactUser(FormCollection form)
        {

            var mesaj = TempData["mesaj"];
            TempData.Keep();

            FakefitEntities6 db = new FakefitEntities6();
            contact contact = new contact();

            var isim = form["name"].ToString();

            if (isim == "")
            {
                TempData["hata"] = "İsim boş olamaz..";
                return RedirectToAction("/ContactUser/" + mesaj, "Default");
            }

            var mail = form["email"].ToString();
            if (mail == "")
            {
                TempData["hata"] = "Mail boş olamaz..";
                return RedirectToAction("/ContactUser/" + mesaj, "Default");
            }

            var telefon = form["phone"].ToString();
            if (telefon == "")
            {
                TempData["hata"] = "Telefon boş olamaz..";
                return RedirectToAction("/ContactUser/" + mesaj, "Default");
            }

            var konu = form["topic"].ToString();
            if (konu == "")
            {
                TempData["hata"] = "Konu boş olamaz..";
                return RedirectToAction("/ContactUser/" + mesaj, "Default");
            }

            var açıklama = form["message"].ToString();
            if (açıklama == "")
            {
                TempData["hata"] = "Mesaj boş olamaz..";
                return RedirectToAction("/ContactUser/" + mesaj, "Default");
            }
            contact.name = form["name"].ToString();
            contact.mail = form["email"].ToString();
            contact.topic = form["topic"].ToString();
            contact.message = form["message"].ToString();
            contact.phone = form["phone"].ToString();
            contact.date = DateTime.Now;
            db.contact.Add(contact);
            db.SaveChanges();

            TempData["hata"] = "Mesajınız başarıyla iletilmiştir..";
            return RedirectToAction("/ContactUser/" + mesaj, "Default");
        }

        [Authorize]
        [Route("Default/ClassesUser/{mesaj}")]
        // GET: Default
        public ActionResult ClassesUser()
        {
            var mesaj = TempData["mesaj"];
            TempData.Keep();

            



            var hata = TempData["hata"];
            ViewBag.hata = hata;

            TempData["hata"] = null;

            FakefitEntities6 db = new FakefitEntities6();
            ViewModel model = new ViewModel();

            model.LessonsGroup = db.all_lessons.Where(x => x.kind_of_lesson == "Grup" && x.date_of_lesson >= DateTime.Today && x.name_of_user == "Admin").ToList();
            model.MyLessons = db.all_lessons.Where(x => x.name_of_user == mesaj && x.date_of_lesson >= DateTime.Today).ToList();
            model.register = db.register.Where(x => x.user == "Eğitmen").ToList();
    
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ClassesUser(FormCollection form)
        {
            var mesaj = TempData["mesaj"];
            TempData.Keep();

           


            FakefitEntities6 db = new FakefitEntities6();
            all_lessons lesson = new all_lessons();
            var user = db.register.FirstOrDefault(x => x.username == mesaj).username.Trim();

            var eğitmenin_adı = form["name"].ToString();

            var gün1 = Convert.ToString(form["Date"]);

            if (gün1 == "")
            {
                TempData["hata"] = "Bir tarih seçin..";
                return RedirectToAction("/ClassesUser/" + user, "Default");
            }
            var gün = Convert.ToDateTime(form["Date"].Trim());
            
            var dersin_saati = form["Time"].ToString();
            var dersin_adı = form["Lessons"].ToString();
            var dersin_türü = form["Kind"].ToString();

            var ders = db.all_lessons.FirstOrDefault(x => x.name_of_teacher == eğitmenin_adı && x.date_of_lesson == gün &&
                    x.hour_of_lesson == dersin_saati && x.name_of_lesson == dersin_adı && x.kind_of_lesson == "Grup");

            var ders1 = db.all_lessons.FirstOrDefault(x => x.name_of_teacher == eğitmenin_adı && x.date_of_lesson == gün &&
                    x.hour_of_lesson == dersin_saati && x.kind_of_lesson == "Özel");

            var ders2 = db.all_lessons.FirstOrDefault(x => x.name_of_teacher == eğitmenin_adı && x.date_of_lesson == gün &&
                    x.hour_of_lesson == dersin_saati && x.name_of_lesson == dersin_adı && x.kind_of_lesson == "Grup" && x.name_of_user == user);

            if(eğitmenin_adı == "Eğitmeni seç")
            {
                TempData["hata"] = "Eğitmen seçin..";
                return RedirectToAction("/ClassesUser/" + user, "Default");
            }
            else if (gün <= DateTime.Today)
            {
                TempData["hata"] = "Geçmiş bir ders tarihi seçemezsiniz..";
                return RedirectToAction("/ClassesUser/" + user, "Default");
            }
            else if(dersin_saati=="Dersin saatini seç")
            {
                TempData["hata"] = "Dersin saatini seçin..";
                return RedirectToAction("/ClassesUser/" + user, "Default");
            }
            else if (dersin_adı == "Dersin adını seç")
            {
                TempData["hata"] = "Dersin adını seçin..";
                return RedirectToAction("/ClassesUser/" + user, "Default");
            }
            else if (dersin_türü == "Dersin türünü seç")
            {
                TempData["hata"] = "Dersin türünü seçin..";
                return RedirectToAction("/ClassesUser/" + user, "Default");
            }
            else if (ders2 != null)
            {
                TempData["hata"] = "Bu grup dersine zaten kayıtlısınız..";
                return RedirectToAction("/ClassesUser/" + user, "Default");
            }
            else if (ders != null)
            {
               
                lesson.name_of_teacher = form["name"].ToString();
                lesson.date_of_lesson = Convert.ToDateTime(form["Date"].Trim());
                lesson.hour_of_lesson = form["Time"].ToString();
                lesson.name_of_lesson = form["Lessons"].ToString();
                lesson.kind_of_lesson = form["Kind"].ToString();
                lesson.name_of_user = user;
                db.all_lessons.Add(lesson);
                db.SaveChanges();
                
                TempData["hata"] = "Seçtiğiniz grup dersine başarıyla kaydoldunuz..";
                return RedirectToAction("/ClassesUser/" + user, "Default");
            }
            else if(form["Kind"].ToString() == "Grup")
            {
                
                TempData["hata"] = "Böyle bir grup dersi yok..";
                return RedirectToAction("/ClassesUser/" + user, "Default");
            }
            else if(ders1 != null)
            {
               
                TempData["hata"] = "Seçtiğiniz gün ve saatte bu eğitmenin başka bir özel dersi var";
                return RedirectToAction("/ClassesUser/" + user, "Default");
            }
            else
            {
                lesson.name_of_teacher = form["name"].ToString();
                lesson.date_of_lesson = Convert.ToDateTime(form["Date"].Trim());
                lesson.hour_of_lesson = form["Time"].ToString();
                lesson.name_of_lesson = form["Lessons"].ToString();
                lesson.kind_of_lesson = form["Kind"].ToString();
                lesson.name_of_user = user;
                db.all_lessons.Add(lesson);
                db.SaveChanges();


                TempData["hata"] = "Seçtiğiniz özel derse başarıyla kaydoldunuz..";
                return RedirectToAction("/ClassesUser/" + user, "Default");
            }
            
            
        }

        [Authorize]
        [Route("Default/ClassesInstructor/{mesaj}")]
        // GET: Default
        public ActionResult ClassesInstructor()
        {
            var mesaj = TempData["mesaj"];
            TempData.Keep();

            FakefitEntities6 db = new FakefitEntities6();
            var name = db.register.FirstOrDefault(x => x.username == mesaj).name;
            var surname = db.register.FirstOrDefault(x => x.username == mesaj).surname;
            var all_name = name + " " + surname;
            var model = db.all_lessons.Where(x => x.name_of_teacher == all_name && x.date_of_lesson >= DateTime.Today).OrderBy(x=>x.date_of_lesson).ToList();
            return View(model);
        }

        [Authorize]
        [Route("Default/ContactAdmin/{mesaj}")]
        // GET: Default
        public ActionResult ContactAdmin()
        {   


            var mesaj = TempData["mesaj"];
            TempData.Keep();
            FakefitEntities6 db = new FakefitEntities6();
            var model = db.contact.OrderByDescending(x=>x.date).ToList();

            return View(model);
        }

        [Authorize]
        [Route("Default/BilgilerimInstructor/{mesaj}")]
        // GET: Default
        public ActionResult BilgilerimInstructor()
        {
            var mesaj = TempData["mesaj"];
            TempData.Keep();
            FakefitEntities6 db = new FakefitEntities6();
            var model = db.register.Where(x => x.username == mesaj).ToList();
            return View(model);
        }

        [Authorize]
        [Route("Default/InformationUser/{mesaj}")]
        public ActionResult InformationUser()
        {

            var mesaj = TempData["mesaj"];
            TempData.Keep();

            FakefitEntities6 db = new FakefitEntities6();
            var model = db.register.Where(x => x.username == mesaj).ToList();
            return View(model);
        }


        [Authorize]
        [Route("Default/DataBase/{mesaj}")]
        public ActionResult DataBase()
        {

            var mesaj = TempData["mesaj"];
            TempData.Keep();

            var hata = TempData["hata"];
            ViewBag.hata = hata;
            TempData["hata"] = null;

            FakefitEntities6 db = new FakefitEntities6();
            ViewModel model = new ViewModel();
            


            model.LessonsGroup = db.all_lessons.Where(x => x.kind_of_lesson == "Grup" && x.date_of_lesson >= DateTime.Today && x.name_of_user == "Admin").ToList();
            model.MyLessons = db.all_lessons.Where(x => x.name_of_user != "Admin" && x.date_of_lesson >= DateTime.Today).ToList();
            model.register = db.register.ToList();

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult DataBase(FormCollection form)
        {

            var mesaj = TempData["mesaj"];
            TempData.Keep();

            
            FakefitEntities6 db = new FakefitEntities6();
            all_lessons lesson = new all_lessons();

            var eğitmenin_adı = form["name"].ToString();
            if(eğitmenin_adı == "Eğitmeni seç")
            {
                TempData["hata"] = "Eğitmen seçin..";
                return RedirectToAction("/DataBase/" + mesaj, "Default");
            }

            var ders_tarihi = Convert.ToString(form["Date"]);

            if (ders_tarihi == "")
            {
                TempData["hata"] = "Dersin tarihini seçin..";
                return RedirectToAction("/DataBase/" + mesaj, "Default");
            }
            

            var ders_tarihi1 = Convert.ToDateTime(form["Date"].Trim());
            if (ders_tarihi1 <= DateTime.Today)
            {
                TempData["hata"] = "Dersin tarihi en erken yarın olabilir..";
                return RedirectToAction("/DataBase/" + mesaj, "Default");
            }

            var dersin_saati = form["Time"].ToString();
            if(dersin_saati == "Dersin saatini seç")
            {
                TempData["hata"] = "Dersin saatini seçin..";
                return RedirectToAction("/DataBase/" + mesaj, "Default");
            }

            var dersin_adı = form["Lessons"].ToString();
            if(dersin_adı == "Dersin adını seç")
            {
                TempData["hata"] = "Dersin adını seçin..";
                return RedirectToAction("/DataBase/" + mesaj, "Default");
            }

            var dersin_türü = form["Kind"].ToString();
            if(dersin_türü == "Dersin türünü seç")
            {
                TempData["hata"] = "Dersin türünü seçin..";
                return RedirectToAction("/DataBase/" + mesaj, "Default");
            }

            var dersi_alan = form["Username"].ToString();

            if (dersin_türü == "Özel" && dersi_alan == "Admin")
            {
                TempData["hata"] = "Özel dersler admine atanamaz..";
                return RedirectToAction("/DataBase/" + mesaj, "Default");
            }


            var ders = db.all_lessons.FirstOrDefault(x => x.name_of_teacher == eğitmenin_adı && x.date_of_lesson == ders_tarihi1 &&
                    x.hour_of_lesson == dersin_saati && x.name_of_lesson == dersin_adı && x.kind_of_lesson == dersin_türü && x.name_of_user == dersi_alan);

            if(dersin_türü == "Grup" && ders!=null)
            {
                TempData["hata"] = "Bu grup dersi sistemde zaten var..";
                return RedirectToAction("/DataBase/" + mesaj, "Default");
            }

            var ders1 = db.all_lessons.FirstOrDefault(x => x.name_of_teacher == eğitmenin_adı && x.date_of_lesson == ders_tarihi1 &&
                    x.hour_of_lesson == dersin_saati);

            if(ders1 != null)
            {
                TempData["hata"] = "Bu Eğitmenin seçilen gün ve saatte başka bir dersi bulunmaktadır..";
                return RedirectToAction("/DataBase/" + mesaj, "Default");
            }



            var user = db.register.FirstOrDefault(x => x.username == mesaj).username.Trim();
            lesson.name_of_teacher = form["name"].ToString();
            lesson.date_of_lesson = Convert.ToDateTime(form["Date"].Trim());
            lesson.hour_of_lesson = form["Time"].ToString();
            lesson.name_of_lesson = form["Lessons"].ToString();
            lesson.kind_of_lesson = form["Kind"].ToString();
            lesson.name_of_user = form["Username"].ToString();
            db.all_lessons.Add(lesson);
            db.SaveChanges();

            TempData["hata"] = "Ders başarılı şekilde kaydedildi..";
            return RedirectToAction("/DataBase/" + user, "Default");
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteLesson(int id)
        {
            var mesaj = TempData["mesaj"];
            TempData.Keep();
            FakefitEntities6 db = new FakefitEntities6();
            var delete = db.all_lessons.Find(id);
            db.all_lessons.Remove(delete);
            db.SaveChanges();
            return RedirectToAction("/DataBase/"+mesaj,"Default");
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteContact(int id)
        {
            var mesaj = TempData["mesaj"];
            TempData.Keep();
            FakefitEntities6 db = new FakefitEntities6();
            var delete = db.contact.Find(id);
            db.contact.Remove(delete);
            db.SaveChanges();
            return RedirectToAction("/ContactAdmin/" + mesaj, "Default");
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteUser(int id)
        {
            var mesaj = TempData["mesaj"];
            TempData.Keep();
            FakefitEntities6 db = new FakefitEntities6();

            
            var ünvan = db.register.Find(id).user;
            
            

            if(ünvan == "Üye")
            {
                var user_name = db.register.Find(id).username;
                var list = db.all_lessons.Where(x => x.name_of_user == user_name && x.date_of_lesson >= DateTime.Today).ToList();
                foreach (var item in list)
                {
                    var delete = db.all_lessons.FirstOrDefault(x => x.id == item.id);
                    db.all_lessons.Remove(delete);
                    db.SaveChanges() ;
                }
                
            }





                var delete1 = db.register.Find(id);
            db.register.Remove(delete1);
            db.SaveChanges();


            


            return RedirectToAction("/Register/" + mesaj, "Default");
        }
    }
}