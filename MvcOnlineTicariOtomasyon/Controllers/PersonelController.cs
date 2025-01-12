using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcOnlineTicariOtomasyon.Models.Siniflar;
using PagedList;
using PagedList.Mvc;
namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class PersonelController : Controller
    {
        // GET: Personel
        Context c = new Context();
        public ActionResult Index(string p, int sayfa = 1)
        {
            var degerler = from x in c.Personels select x;
            if(!string.IsNullOrEmpty(p))
            {
                degerler = degerler.Where(y => y.PersonelAd.Contains(p) || y.PersonelSoyad.Contains(p));
            }
            return View(degerler.ToList().ToPagedList(sayfa, 5));
        }
        [HttpGet]
        public ActionResult PersonelEkle()
        {
            List<SelectListItem> deger1 = (from x in c.Departmans.ToList()
                                                select new SelectListItem
                                                {
                                                    Text = x.DepartmanAd,
                                                    Value = x.Departmanid.ToString()
                                                }).ToList();
            ViewBag.dgr1 = deger1;
            return View();
        }
        [HttpPost]
        public ActionResult PersonelEkle(Personel p)
        {
            if(Request.Files.Count > 0)
            {
                string personelAd = p.PersonelAd;
                string personelSoyad = p.PersonelSoyad;
                string uzanti = Path.GetExtension(Request.Files[0].FileName);
                string dosyaAdi = personelAd + "_" + personelSoyad + uzanti;
                string yol = "~/Image/" + dosyaAdi;
                Request.Files[0].SaveAs(Server.MapPath(yol));
                p.PersonelGorsel = "/Image/" + dosyaAdi;
            }
            c.Personels.Add(p);
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult PersonelGetir(int id)
        {
            List<SelectListItem> deger1 = (from x in c.Departmans.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.DepartmanAd,
                                               Value = x.Departmanid.ToString()
                                           }).ToList();
            ViewBag.dgr1 = deger1;
            var pers = c.Personels.Find(id);
            return View("PersonelGetir", pers);
        }
        public ActionResult PersonelGuncelle(Personel per)
        {
            if (Request.Files.Count > 0)
            {
                string personelAd = per.PersonelAd;
                string personelSoyad = per.PersonelSoyad;
                string uzanti = Path.GetExtension(Request.Files[0].FileName);
                string dosyaAdi = personelAd + "_" + personelSoyad + uzanti;
                string yol = "~/Image/" + dosyaAdi;
                Request.Files[0].SaveAs(Server.MapPath(yol));
                per.PersonelGorsel = "/Image/" + dosyaAdi;
            }
            var p = c.Personels.Find(per.Personelid);
            p.PersonelAd = per.PersonelAd;
            p.PersonelSoyad = per.PersonelSoyad;
            p.PersonelGorsel = per.PersonelGorsel;
            p.Departmanid = per.Departmanid;
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult PersonelListe()
        {
            var sorgu = c.Personels.ToList();
            return View(sorgu);
        }
    }
}