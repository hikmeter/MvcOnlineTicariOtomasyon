using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcOnlineTicariOtomasyon.Models.Siniflar;
namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class CariPanelController : Controller
    {
        // GET: CariPanel
        Context c = new Context();
        [Authorize]
        public ActionResult Index()
        {
            var mail = (string)Session["CariMail"];
            var degerler = c.mesajlars.Where(x => x.Alici == mail).ToList();
            var mailid = c.Carilers.Where(y => y.CariMail == mail).Select(z => z.Cariid).FirstOrDefault();
            var satissayisi = c.SatisHarekets.Where(x => x.Cariid == mailid).Count().ToString();
            var harcama = c.SatisHarekets.Where(x => x.Cariid == mailid).Sum(y => y.ToplamTutar).ToString();
            var urunsayisi = c.SatisHarekets.Where(x => x.Cariid == mailid).Sum(y => y.Adet).ToString();
            var adsoyad = c.Carilers.Where(x => x.CariMail == mail).Select(y => y.CariAd + " " + y.CariSoyad).FirstOrDefault();
            ViewBag.sts = satissayisi;
            ViewBag.hrc = harcama;
            ViewBag.urn = urunsayisi;
            ViewBag.ads = adsoyad;
            ViewBag.m = mail;
            return View(degerler);
        }
        [Authorize]
        public ActionResult Siparislerim()
        {
            var mail = (string)Session["CariMail"];
            var id = c.Carilers.Where(x => x.CariMail == mail.ToString()).Select(y=> y.Cariid).FirstOrDefault();
            var degerler = c.SatisHarekets.Where(x => x.Cariid == id).ToList();
            return View(degerler);
        }
        [Authorize]
        public ActionResult GelenMesajlar()
        {
            var mail = (string)Session["CariMail"];
            var mesajlar = c.mesajlars.Where(x => x.Alici == mail).OrderByDescending(x=> x.MesajID).ToList();
            var gelenler= c.mesajlars.Where(x => x.Alici == mail).Count().ToString();
            var gidenler = c.mesajlars.Where(x => x.Gonderici == mail).Count().ToString();
            ViewBag.dgr1 = gelenler;
            ViewBag.dgr2 = gidenler;
            return View(mesajlar);
        }
        [Authorize]
        public ActionResult GidenMesajlar()
        {
            var mail = (string)Session["CariMail"];
            var mesajlar = c.mesajlars.Where(x => x.Gonderici == mail).OrderByDescending(y => y.MesajID).ToList();
            var gelenler = c.mesajlars.Where(x => x.Alici == mail).Count().ToString();
            var gidenler = c.mesajlars.Where(x => x.Gonderici == mail).Count().ToString();
            ViewBag.dgr1 = gelenler;
            ViewBag.dgr2 = gidenler;
            return View(mesajlar);
        }
        [Authorize]
        public ActionResult MesajDetay(int id)
        {
            var mesaj = c.mesajlars.Where(x => x.MesajID == id).ToList();
            var mail = (string)Session["CariMail"];
            var gelenler = c.mesajlars.Where(x => x.Alici == mail).Count().ToString();
            var gidenler = c.mesajlars.Where(x => x.Gonderici == mail).Count().ToString();
            ViewBag.dgr1 = gelenler;
            ViewBag.dgr2 = gidenler;
            return View(mesaj);
        }
        [Authorize]
        [HttpGet]
        public ActionResult YeniMesaj()
        {
            var mail = (string)Session["CariMail"];
            var gelenler = c.mesajlars.Where(x => x.Alici == mail).Count().ToString();
            var gidenler = c.mesajlars.Where(x => x.Gonderici == mail).Count().ToString();
            ViewBag.dgr1 = gelenler;
            ViewBag.dgr2 = gidenler;
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult YeniMesaj(mesajlar m)
        {
            var mail = (string)Session["CariMail"];
            m.Tarih = DateTime.Parse(DateTime.Now.ToString());
            m.Gonderici = mail;
            c.mesajlars.Add(m);
            c.SaveChanges();
            return RedirectToAction("GidenMesajlar","CariPanel");
        }
        [Authorize]
        public ActionResult KargoTakip(string p)
        {
            var degerler = from x in c.KargoDetays select x;
            degerler = degerler.Where(y => y.TakipKodu.Contains(p));
            return View(degerler.ToList());
        }
        [Authorize]
        public ActionResult KargoDetay(string id)
        {
            var degerler = c.KargoTakips.Where(x => x.TakipKodu == id).ToList();
            return View(degerler);
        }
        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
        public PartialViewResult Partial1()
        {
            var mail = (string)Session["CariMail"];
            var id = c.Carilers.Where(y => y.CariMail == mail).Select(z => z.Cariid).FirstOrDefault();
            var caribul = c.Carilers.Find(id);
            return PartialView("Partial1", caribul);
        }
        public PartialViewResult Partial2()
        {
            var veriler = c.mesajlars.Where(x => x.Gonderici == "Admin").OrderByDescending(y => y.Tarih).ToList();
            return PartialView(veriler);
        }
        public ActionResult CariBilgiGuncelle(Cariler cr)
        {
            var cari = c.Carilers.Find(cr.Cariid);
            cari.CariAd = cr.CariAd;
            cari.CariSoyad = cr.CariSoyad;
            cari.CariSehir = cr.CariSehir;
            cari.CariSifre = cr.CariSifre;
            c.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}