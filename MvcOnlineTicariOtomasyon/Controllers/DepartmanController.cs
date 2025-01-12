using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcOnlineTicariOtomasyon.Models.Siniflar;
namespace MvcOnlineTicariOtomasyon.Controllers
{
    public class DepartmanController : Controller
    {
        // GET: Departman
        Context c = new Context();
        public ActionResult Index(string p)
        {
            var degerler = from x in c.Departmans select x;
            if(!string.IsNullOrEmpty(p))
            {
                degerler = degerler.Where(y => y.DepartmanAd.Contains(p));  
            }
            return View(degerler.ToList());
        }
        [Authorize(Roles ="A")]
        [HttpGet]
        public ActionResult DepartmanEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DepartmanEkle(Departman d)
        {
            c.Departmans.Add(d);
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DepartmanSil(int id)
        {
            var dep = c.Departmans.Find(id);
            dep.Durum = false;
            c.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DepartmanGetir (int id)
        {
            var dprt = c.Departmans.Find(id);
            return View("DepartmanGetir", dprt);
        }
        public ActionResult DepartmanGuncelle(Departman d)
        {
            var departman = c.Departmans.Find(d.Departmanid);
            departman.DepartmanAd = d.DepartmanAd;
            c.SaveChanges();
            return RedirectToAction("Index");   
        }
        public ActionResult DepartmanDetay(int id)
        {
            var degerler = c.Personels.Where(x => x.Departmanid == id).ToList();
            var dpt = c.Departmans.Where(x => x.Departmanid == id).Select(y => y.DepartmanAd).FirstOrDefault();
            ViewBag.deger1 = dpt;
            return View(degerler);
        }
        public ActionResult DepartmanPersonelSatis(int id)
        {
            var degerler = c.SatisHarekets.Where(x => x.Personelid == id).ToList();
            var per = c.Personels.Where(x => x.Personelid == id).Select(y => y.PersonelAd +" "+ y.PersonelSoyad).FirstOrDefault();
            ViewBag.deger = per;
            return View(degerler);
        }
    }
}