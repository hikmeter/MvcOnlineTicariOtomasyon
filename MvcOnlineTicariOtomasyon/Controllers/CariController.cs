using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcOnlineTicariOtomasyon.Models.Siniflar;
using PagedList;
using PagedList.Mvc;
namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class CariController : Controller
    {
        // GET: Cari
        Context c = new Context();
        public ActionResult Index(string p, int sayfa = 1)
        {
            var degerler = from x in c.Carilers where x.Durum == true select x;
            if(!string.IsNullOrEmpty(p))
            {
                degerler = degerler.Where(y => y.CariAd.Contains(p) || y.CariSoyad.Contains(p));
            }
            return View(degerler.ToList().ToPagedList(sayfa, 10));
        }
        [HttpGet]
        public ActionResult CariEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CariEkle(Cariler cr)
        {
            cr.Durum = true;
            c.Carilers.Add(cr);
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult CariSil(int id)
        {
            var cr = c.Carilers.Find(id);
            cr.Durum = false;
            c.SaveChanges();
            return RedirectToAction("Index");   
        }
        public ActionResult CariGetir(int id)
        {
            var cari = c.Carilers.Find(id);
            return View("CariGetir", cari);
        }
        public ActionResult CariGuncelle(Cariler car)
        {
            if (!ModelState.IsValid)
            {
                return View("CariGetir");
            }
            var cari = c.Carilers.Find(car.Cariid);
            cari.CariAd = car.CariAd;
            cari.CariSoyad = car.CariSoyad;
            cari.CariSehir = car.CariSehir;
            cari.CariMail = car.CariMail;
            c.SaveChanges();
            return RedirectToAction("Index");   
        }
        public ActionResult CariSatinAlim(int id)
        {
            var degerler = c.SatisHarekets.Where(x => x.Cariid == id).ToList();
            var car = c.Carilers.Where(x => x.Cariid == id).Select(y => y.CariAd + " " + y.CariSoyad).FirstOrDefault();
            ViewBag.deger = car;
            return View(degerler);
        }
    }
}