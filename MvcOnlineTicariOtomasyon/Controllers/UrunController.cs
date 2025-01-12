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
    public class UrunController : Controller
    {
        // GET: Urun
        Context c = new Context();
        public ActionResult Index(string p, int sayfa = 1)
        {
            var urunler = from x in c.Uruns where x.Durum == true select x;
            if (!string.IsNullOrEmpty(p))
            {
                urunler = urunler.Where(y => y.UrunAd.Contains(p));
            }
            return View(urunler.ToList().ToPagedList(sayfa, 10));
        }
        [HttpGet]
        public ActionResult UrunEkle()
        {
            List<SelectListItem> deger1 = (from x in c.Kategoris.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.KategoriAd,
                                               Value = x.KategoriID.ToString()
                                           }).ToList();
            ViewBag.dgr1 = deger1;
            return View();
        }
        [HttpPost]
        public ActionResult UrunEkle(Urun u)
        {
            c.Uruns.Add(u);
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult UrunSil(int id)
        {
            var deger = c.Uruns.Find(id);
            deger.Durum = false;
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult UrunGetir(int id)
        {
            List<SelectListItem> deger1 = (from x in c.Kategoris.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.KategoriAd,
                                               Value = x.KategoriID.ToString()
                                           }).ToList();
            ViewBag.dgr1 = deger1;
            var urun = c.Uruns.Find(id);
            return View("UrunGetir", urun);
        }
        public ActionResult UrunGuncelle(Urun u)
        {
            var urn = c.Uruns.Find(u.Urunid);
            urn.UrunAd = u.UrunAd;
            urn.Marka = u.Marka;
            urn.Stok = u.Stok;
            urn.AlisFiyat = u.AlisFiyat;
            urn.SatisFiyat = u.SatisFiyat;
            urn.Kategoriid = u.Kategoriid;
            urn.UrunGorsel = u.UrunGorsel;
            urn.Durum = u.Durum;
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult UrunDetay()
        {
            var degerler = c.Uruns.ToList();
            return View(degerler);
        }
        [HttpGet]
        public ActionResult SatisYap(int id)
        {
            List<SelectListItem> deger1 = (from x in c.Personels.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.PersonelAd + " " + x.PersonelSoyad,
                                               Value = x.Personelid.ToString(),
                                           }).ToList();
            var deger2 = c.Uruns.Find(id);
            ViewBag.dgr1 = deger1;
            ViewBag.dgr2 = deger2.Urunid;
            ViewBag.dgr3 = deger2.SatisFiyat;
            return View();
        }
        [HttpPost]
        public ActionResult SatisYap(SatisHareket p)
        {
            p.Tarih = DateTime.Parse(DateTime.Now.ToShortDateString());
            c.SatisHarekets.Add(p);
            c.SaveChanges();
            return RedirectToAction("Index", "Satis");
        }
    }
}