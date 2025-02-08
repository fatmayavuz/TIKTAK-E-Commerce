using bitirme_database_new.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;


namespace bitirme_database_new.Controllers
{
    public class HomeController : Controller
    {
        private readonly Bitirme_ProjesiEntities4 db = new Bitirme_ProjesiEntities4();

        // GET: Ilanlar/Index
        public ActionResult Index()
        {
            var ilanlar = db.Ilanlar.Include("Ilan_Resimleri").ToList();

            // Debugging için:
            foreach (var ilan in ilanlar)
            {
                Console.WriteLine($"Ilan ID: {ilan.id}, Resim Sayısı: {ilan.Ilan_Resimleri.Count}");
            }

            return View(ilanlar);
        }


        


        public ActionResult IlanSil1(int id)
        {
            // İlanı ID ile bul
            var ilan = db.Ilanlar.Find(id);
            if (ilan != null)
            {
                // Önce Favoriler tablosundan ilişkili kayıtları sil
                var favoriler = db.Favoriler.Where(f => f.ilan_id == id).ToList();
                db.Favoriler.RemoveRange(favoriler);

                // Sonra ilanı sil
                db.Ilanlar.Remove(ilan);

                // Değişiklikleri kaydet
                db.SaveChanges();
            }

            // İlanlar listesine geri dön
            return RedirectToAction("IlanListesi");
        }



        // Kullanıcı Silme işlemi
        public ActionResult IlanSil2(int id)
        {
            // İlanı ID ile bul
            var ilan = db.Ilanlar.Find(id);
            if (ilan != null)
            {
                // Önce Favoriler tablosundan ilişkili kayıtları sil
                var favoriler1 = db.Favoriler.Where(f => f.ilan_id == id).ToList();
                db.Favoriler.RemoveRange(favoriler1);

                // Sonra ilanı sil
                db.Ilanlar.Remove(ilan);

                // Değişiklikleri kaydet
                db.SaveChanges();
            }

            // Kullanıcı ilanlar listesine geri dön
            return RedirectToAction("KullaniciIlanListesi");
        }

        public ActionResult IlanListesi()
        {
            var ilanlar = db.Ilanlar.ToList(); // Tüm ilanları al
            return View(ilanlar); // Görünüme gönder
        } 

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var user = db.Kullanıcılar.Find(id);

            if (user != null)
            {
                db.Kullanıcılar.Remove(user); // Cascade delete devreye girer
                db.SaveChanges();
            }

            return RedirectToAction("KullanıcıListesi");
        }


        // Kullanıcı Listesi
        public ActionResult KullaniciListesi()
        {
            var kullanicilar = db.Kullanıcılar.ToList();
            return View(kullanicilar);
        }

        // Kullanıcı İlan Listesi
        public ActionResult KullaniciIlanListesi()
        {
            // Giriş yapan kullanıcıyı al (örneğin, Session veya Identity kullanarak)
            string currentUserName = (string)Session["KullaniciAdi"];
            if (string.IsNullOrEmpty(currentUserName))
            {
                return RedirectToAction("KullaniciGiris", "Home"); // Giriş yapılmadıysa login sayfasına yönlendirin.
            }

            // Kullanıcıyı veritabanından al
            var currentUser = db.Kullanıcılar.FirstOrDefault(k => k.kullanici_adi == currentUserName);

            if (currentUser == null)
            {
                return HttpNotFound(); // Kullanıcı bulunamazsa hata ver.
            }

            // Kullanıcıya ait ilanları al
            var ilanlar = currentUser.Ilanlar.ToList();

            return View(ilanlar);
        }


        public ActionResult IlanDuzenle(int id)
        {
            var ilan = db.Ilanlar.Find(id);
            if (ilan == null)
            {
                return HttpNotFound();
            }
            return View(ilan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IlanDuzenle(Ilanlar ilan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ilan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IlanListesi");
            }
            return View(ilan);
        }

        public ActionResult IlanDuzenle1(int id)
        {
            var ilan = db.Ilanlar.Find(id);
            if (ilan == null)
            {
                return HttpNotFound();
            }
            return View(ilan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IlanDuzenle1(Ilanlar ilan)
        {
            if (ModelState.IsValid)
            {
                // Ensure the kullanici_id is not null when updating the advertisement
                if (Session["KullaniciAdi"] != null)
                {
                    string kullaniciAdi = Session["KullaniciAdi"].ToString();
                    var kullanici = db.Kullanıcılar.FirstOrDefault(k => k.kullanici_adi == kullaniciAdi);

                    if (kullanici != null)
                    {
                        ilan.kullanici_id = kullanici.id;  // Set the kullanici_id to ensure it's saved correctly
                    }
                }

                // Update the advertisement in the database
                db.Entry(ilan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("KullaniciIlanListesi");
            }
            return View(ilan);
        }


      

        public ActionResult Emlak()
        {
            return View();
        }
        public ActionResult Vasıta()
        {
            return View();
        }
        public ActionResult Giyim()
        {
            return View();
        }
        public ActionResult İş_İlanları()
        {
            return View();
        }





        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||








        public ActionResult Kullanıcılar_Kayıt()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kullanıcılar_Kayıt(Kullanıcılar yeniKullanıcı)
        {
            if (ModelState.IsValid)
            {
                
                    using (var db = new Bitirme_ProjesiEntities4())
                    {
                        db.Kullanıcılar.Add(yeniKullanıcı);
                        db.SaveChanges();
                    }

                    TempData["SuccessMessage"] = "Tebrikler! Kaydınız başarıyla oluşturulmuştur.";
                    return RedirectToAction("Kullanıcılar_Kayıt");
               
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                TempData["ErrorMessage"] = "Beklenmeyen bir hata oluştu. Lütfen tekrar deneyin.";
                TempData["ErrorMessage"] = "Lütfen tüm alanları doğru şekilde doldurduğunuzdan emin olun.";
            }

            return View(yeniKullanıcı);
        }




        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||


        // Admin Giriş Sayfası GET
        [HttpGet]  // GET methodu için belirtilmiş
        public ActionResult AdminGiris()
        {
            return View();
        }

        // Admin Giriş POST
        [HttpPost] // POST methodu için belirtilmiş
        [ValidateAntiForgeryToken]
        public ActionResult AdminGiris(string e_posta, string sifre)
        {
            var admin = db.Adminler1
                .FirstOrDefault(a => a.e_posta == e_posta && a.şifre == sifre);

            if (admin != null)
            {
                // Giriş başarılıysa yönlendirme
                Session["AdminAdi"] = admin.ad;
                Session["AdminSoyadi"] = admin.soyad;

                return RedirectToAction("AdminPaneli");
            }

            // Hatalı giriş
            TempData["Hata"] = "E-posta veya şifre hatalı!";
            return RedirectToAction("AdminGiris");
        }


        // Admin Paneli
        public ActionResult AdminPaneli()
        {
            if (Session["AdminAdi"] != null)
            {
                ViewBag.AdminAdi = $"{Session["AdminAdi"]} {Session["AdminSoyadi"]}";
                return View();
            }

            // Giriş yapılmamışsa yönlendirme
            return RedirectToAction("AdminGiris");
        }

        // Logout
        public ActionResult Logout()
        {
            Session.Clear(); // Tüm session verilerini temizle
            return RedirectToAction("AdminGiris");
        }




        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||



        // Kullanıcı Giriş Sayfası GET
        public ActionResult KullaniciGiris()
        {
            return View();
        }

        // Kullanıcı Giriş POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KullaniciGiris(string e_posta, string sifre)
        {
            var kullanici = db.Kullanıcılar
                .FirstOrDefault(k => k.e_posta == e_posta && k.sifre == sifre);

            if (kullanici != null)
            {
                // Giriş başarılıysa kullanıcı adı session'a kaydedilir.
                Session["KullaniciAdi"] = kullanici.kullanici_adi;
                Session["KullaniciId"] = kullanici.id; // Kullanıcı ID'si de oturuma kaydedilir.
                return RedirectToAction("KullaniciPaneli");
            }

            ViewBag.Hata = "E-posta veya şifre hatalı!";
            return View();
        }

        // Çıkış işlemi
        public ActionResult Logout1()
        {
            Session.Clear(); // Tüm session verilerini temizle
            return RedirectToAction("KullaniciGiris");
        }


        [HttpPost]
        public JsonResult SaveMessage(int ilanId, string messageText)
        {
            if (string.IsNullOrEmpty(messageText))
            {
                return Json(new { success = false });
            }

            try
            {
                // Oturum açmış kullanıcıyı kontrol et
                if (Session["KullaniciId"] == null)
                {
                    return Json(new { success = false, message = "Oturum açılmamış!" });
                }

                // Oturumdan kullanıcı ID'sini al
                int gonderenId = (int)Session["KullaniciId"];

                // Ilanlar tablosundan ilgili ilanı bul
                var ilan = db.Ilanlar.FirstOrDefault(i => i.id == ilanId);
                if (ilan == null)
                {
                    return Json(new { success = false, message = "İlan bulunamadı." });
                }

                // Yeni mesaj oluştur ve veritabanına ekle
                var message = new Mesajlar
                {
                    ilan_id = ilanId,
                    gonderen_id = gonderenId, // Oturum açan kullanıcı
                    alici_id = ilan.kullanici_id ?? 0, // İlanın sahibi
                    mesaj = messageText,
                    tarih = DateTime.Now
                };

                db.Mesajlar.Add(message);
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }


        public ActionResult MesajListesi()
        {
            // Kullanıcı ID'sini Session'dan al
            int kullaniciId = Convert.ToInt32(Session["KullaniciId"]);

            // Mesajlar modelinde alıcı ID'si bu kullanıcı ID'sine eşit olan mesajları filtrele
            var mesajlar = db.Mesajlar
                             .Where(m => m.alici_id == kullaniciId)
                             .OrderByDescending(m => m.tarih)
                             .ToList();

            return View(mesajlar); // Mesajları view'e gönder
        }

        public ActionResult AdminMesajListesi()
        {
            // Veritabanından tüm mesajları çekiyoruz
            var mesajlar = db.Mesajlar.Include("Kullanıcılar").Include("Kullanıcılar1").Include("Ilanlar").ToList();
            return View(mesajlar);
        }


        public ActionResult DeleteMesaj(int id)
        {
            var mesaj = db.Mesajlar.Find(id); // Mesajı bul
            if (mesaj != null)
            {
                db.Mesajlar.Remove(mesaj); // Mesajı sil
                db.SaveChanges(); // Değişiklikleri kaydet
            }
            return RedirectToAction("MesajListesi"); // MesajListesi görünümüne geri dön
        }


        public ActionResult CevapVer(int id)
        {
            var mesaj = db.Mesajlar.Find(id); // Mesajı bul
            if (mesaj == null) return HttpNotFound();

            return View(mesaj); // Mesaj detaylarını görünüme gönder
        }


        [HttpPost]
        public ActionResult GonderCevap(int alici_id, string mesaj, int ilan_id)
        {
            // Oturumdan giriş yapan kullanıcının ID'sini alıyoruz
            int gonderenId = Convert.ToInt32(Session["KullaniciId"]);

            var yeniMesaj = new Mesajlar
            {
                gonderen_id = gonderenId, // Formdan gelen alici_id'yi gonderen_id olarak kullanıyoruz
                alici_id = alici_id,   // Oturumdan alınan gonderenId'yi alici_id olarak kullanıyoruz
                ilan_id = ilan_id,       // İlan ID'sini ekliyoruz
                mesaj = mesaj,           // Mesaj içeriği
                tarih = DateTime.Now     // Mevcut tarih ve saat
            };

            db.Mesajlar.Add(yeniMesaj); // Yeni mesajı veritabanına ekle
            db.SaveChanges();           // Değişiklikleri kaydet

            return RedirectToAction("MesajListesi");
        }





        // Kullanıcı Paneli
        [HttpGet]
        public ActionResult KullaniciPaneli()
        {
            if (Session["KullaniciAdi"] != null)
            {
                string kullaniciAdi = Session["KullaniciAdi"].ToString();
                var kullanici = db.Kullanıcılar.FirstOrDefault(k => k.kullanici_adi == kullaniciAdi);

                if (kullanici != null)
                {
                    var favoriler = db.Favoriler
                                             .Where(f => f.kullanici_id == kullanici.id)
                                             .Select(f => new
                                             {
                                                 f.Ilanlar.baslik,
                                                 f.Ilanlar.aciklama,
                                                 f.tarih
                                             }).ToList();
                    ViewBag.Favoriler = favoriler;
                }

                ViewBag.Kategoriler = db.Kategoriler.ToList();
                ViewBag.KullaniciAdi = kullaniciAdi;
                return View();
            }
            return RedirectToAction("KullaniciGiris");
        }




        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        public string UploadImage(HttpPostedFileBase file)
        {
            string path = "-1";
            Random random = new Random();

            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName).ToLower();

                // Sadece belirli uzantılara izin ver
                if (extension.Equals(".jpg") || extension.Equals(".jpeg") || extension.Equals(".png"))
                {
                    try
                    {
                        string fileName = random.Next() + "_" + Path.GetFileName(file.FileName);
                        string serverPath = Server.MapPath("~/Content/Images/");

                        // Sunucuda belirtilen dizin yoksa oluştur
                        if (!Directory.Exists(serverPath))
                        {
                            Directory.CreateDirectory(serverPath);
                        }

                        // Dosyayı kaydet
                        string fullPath = Path.Combine(serverPath, fileName);
                        file.SaveAs(fullPath);

                        // Veri tabanı için göreceli yolu ayarla
                        path = "/Content/Images/" + fileName;
                    }
                    catch (Exception)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    path = "-1";
                }
            }
            return path;
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IlanKaydet(Ilanlar ilan, HttpPostedFileBase uploadedPhoto)
        {
            if (ModelState.IsValid)
            {
                if (Session["KullaniciAdi"] != null)
                {
                    string kullaniciAdi = Session["KullaniciAdi"].ToString();
                    var kullanici = db.Kullanıcılar.FirstOrDefault(k => k.kullanici_adi == kullaniciAdi);

                    if (kullanici != null)
                    {
                        // İlan bilgilerini ayarla
                        ilan.kullanici_id = kullanici.id;
                        ilan.tarih = DateTime.Now;

                        // Resim yüklenmişse işlemi yap
                        if (uploadedPhoto != null && uploadedPhoto.ContentLength > 0)
                        {
                            string imagePath = UploadImage(uploadedPhoto);
                            if (imagePath != "-1")
                            {
                                ilan.resim_url = imagePath; // Resim yolunu ilan nesnesine ekle
                            }
                            else
                            {
                                // Resim yükleme hatası durumunda KullaniciPaneli'ne dön
                                ViewBag.ErrorMessage = "Resim yüklenirken bir hata oluştu.";
                                return View("KullaniciPaneli");  // Hata durumunda geri dönebiliriz
                            }
                        }

                        // İlan kaydetme işlemi
                        db.Ilanlar.Add(ilan);
                        db.SaveChanges(); // Veritabanına kaydet

                        // Başarı mesajı
                        ViewBag.SuccessMessage = "İlan başarıyla kaydedildi.";
                        return RedirectToAction("KullaniciPaneli");  // İlanlar sayfasına yönlendir
                    }
                }
            }

            // Kategoriler için veri gönder
            ViewBag.Kategoriler = db.Kategoriler.ToList(); 
            return View("KullaniciPaneli");  // Bu şekilde doğru view'e yönlendirme yapabilirsiniz.

        }

        


        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||



        // Favoriye ilan eklemek
        [HttpPost]
            public JsonResult AddToFavorites(int ilanId)
            {
                if (Session["KullaniciAdi"] != null)
                {
                    string kullaniciAdi = Session["KullaniciAdi"].ToString();
                    var kullanici = db.Kullanıcılar.FirstOrDefault(k => k.kullanici_adi == kullaniciAdi);

                    if (kullanici != null)
                    {
                        // Favori ürün ekleme işlemi
                        var favori = new Favoriler
                        {
                            kullanici_id = kullanici.id,
                            ilan_id = ilanId,
                            tarih = DateTime.Now
                        };

                        db.Favoriler.Add(favori);
                        db.SaveChanges();

                        return Json(new { success = true });
                    }
                }

                return Json(new { success = false });
            }

            // Favori ürünler sayfası
            public ActionResult FavoriUrunler()
            {
                if (Session["KullaniciAdi"] != null)
                {
                    string kullaniciAdi = Session["KullaniciAdi"].ToString();
                    var kullanici = db.Kullanıcılar.FirstOrDefault(k => k.kullanici_adi == kullaniciAdi);

                    if (kullanici != null)
                    {
                        var favoriler = db.Favoriler
                            .Where(f => f.kullanici_id == kullanici.id)
                            .Include(f => f.Ilanlar)
                            .ToList();

                        return View(favoriler);
                    }
                }

                return RedirectToAction("Login", "Account"); // Kullanıcı girişi yapılmamışsa login sayfasına yönlendir
            }


        public ActionResult FavoriSil(int id)
        {
            // İlgili favoriyi bul
            var favori = db.Favoriler.Find(id);

            if (favori != null)
            {
                // Favoriyi sil
                db.Favoriler.Remove(favori);
                db.SaveChanges();
            }

            // Favorilerim sayfasına yönlendirme
            return RedirectToAction("FavoriUrunler");
        }


        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||


        private readonly Bitirme_ProjesiEntities4 db5 = new Bitirme_ProjesiEntities4(); // db2 nesnesini kullanıyoruz

        [HttpPost]
        public JsonResult AddToCart(int ilanId)
        {
            
            if (Session["KullaniciAdi"] != null)
            {
                string kullaniciAdi = Session["KullaniciAdi"].ToString();
                var kullanici = db5.Kullanıcılar.FirstOrDefault(k => k.kullanici_adi == kullaniciAdi);

                if (kullanici != null)
                {
                    // İlan bilgilerini al
                    var ilan = db5.Ilanlar.FirstOrDefault(i => i.id == ilanId);
                    if (ilan != null)
                    {
                        // Sepet'e ürün ekleme
                        var sepet = new Alısveris
                        {
                            kullanici_id = Convert.ToInt32(kullanici.id),
                            kategori_adi = ilan.ana_kategori,
                            alt_kategori_adi = ilan.alt_kategori,
                            urun_adi = ilan.baslik,
                            fiyat = ilan.fiyat,
                            aciklama = ilan.aciklama
                        };

                        db5.Alısveris.Add(sepet);
                        db5.SaveChanges();
                        


                        return Json(new { success = true });
                    }
                }
            }

            return Json(new { success = false });
        }


        public ActionResult ShoppingCart()
        {
            if (Session["KullaniciAdi"] != null)
            {
                string kullaniciAdi = Session["KullaniciAdi"].ToString();
                var kullanici = db5.Kullanıcılar.FirstOrDefault(k => k.kullanici_adi == kullaniciAdi); // db2 kullanılıyor

                if (kullanici != null)
                {
                    var sepet = db5.Alısveris
                        .Where(s => s.kullanici_id == kullanici.id)
                        .ToList(); // db2 kullanılıyor

                    return View(sepet);
                }
            }

            return RedirectToAction("Login", "Account"); // Kullanıcı girişi yapılmamışsa login sayfasına yönlendirilir
        }



        [HttpPost]
        public JsonResult DeleteFromCart(int id)
        {
            if (Session["KullaniciAdi"] != null)
            {
                string kullaniciAdi = Session["KullaniciAdi"].ToString();
                var kullanici = db5.Kullanıcılar.FirstOrDefault(k => k.kullanici_adi == kullaniciAdi);

                if (kullanici != null)
                {
                    var urun = db5.Alısveris.FirstOrDefault(a => a.id == id && a.kullanici_id == kullanici.id);
                    if (urun != null)
                    {
                        db5.Alısveris.Remove(urun);
                        db5.SaveChanges();
                        return Json(new { success = true });
                    }
                }
            }

            return Json(new { success = false });
        }




        //|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||



        // GET: Edit
        public ActionResult KullaniciEdit(int id)
        {
            var kullanici = db5.Kullanıcılar.Find(id);
            if (kullanici == null)
            {
                return HttpNotFound();
            }

            return View(kullanici);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KullaniciEdit(Kullanıcılar kullanici)
        {
            if (ModelState.IsValid)
            {
                db5.Entry(kullanici).State = EntityState.Modified;
                db5.SaveChanges();
                return RedirectToAction("KullaniciListesi");
            }
            return View(kullanici);
        }


        public ActionResult KullanıcıSil(int id)
        {
            // Kullanıcıyı veritabanından bul
            var kullanici = db.Kullanıcılar.Find(id);

            if (kullanici == null)
            {
                return HttpNotFound();
            }

            // Kullanıcının ilişkili olduğu verileri sil
            db.Kullanıcılar.Remove(kullanici);
            db.SaveChanges();  // Değişiklikleri veritabanına kaydet

            return RedirectToAction("KullaniciListesi");  // Kullanıcı listesine geri dön
        }



        [HttpPost]
        public JsonResult SaveComment(string yorum, int ilanId, int userId)
        {
            if (string.IsNullOrWhiteSpace(yorum))
            {
                return Json(new { success = false, message = "Yorum boş olamaz." });
            }

            try
            {
                // Create a new comment object
                var comment = new Yorumlar
                {
                    kullanici_id = userId,
                    ilan_id = ilanId,
                    yorum = yorum,
                    tarih = DateTime.Now
                };

                // Save the comment to the database
                db.Yorumlar.Add(comment);
                db.SaveChanges();

                // Get the username of the user
                var user = db.Kullanıcılar.FirstOrDefault(u => u.id == userId);
                var userName = user?.kullanici_adi ?? "Unknown User";

                return Json(new { success = true, message = "Yorum başarıyla kaydedildi.", userName = userName, date = comment.tarih?.ToString("dd/MM/yyyy HH:mm") });
            }
            catch (Exception ex)
            {
                // Log the error message
                var innerExceptionMessage = ex.InnerException?.Message ?? "No inner exception details.";
                return Json(new { success = false, message = "Yorum kaydedilemedi: " + ex.Message, innerException = innerExceptionMessage });
            }
        }



























    }
}
