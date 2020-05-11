using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PersonelBasvuru.Models;

namespace PersonelBasvuru.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            BasvuruEntities3 DB = new BasvuruEntities3();
            ViewBag.ILID = new SelectList(DB.ILLERs, "ID", "ILADI");
            return View();
        }
        public ActionResult Job()
        {
            if(TempData["prid"]!=null)
            {
                BasvuruEntities3 DB = new BasvuruEntities3();
                ViewBag.ILID = new SelectList(DB.ILLERs, "ID", "ILADI");
                ViewBag.PERSONELID = TempData["prid"];
                return View();
            }
            return RedirectToAction("Index");
        }

        public ActionResult GetIlce(int Id)
        {
    
            BasvuruEntities3 db = new BasvuruEntities3();
            
            List<ILCELER> selectList = db.ILCELERs.Where(x => x.ILID == Id).ToList();
            ViewBag.slist = new SelectList(selectList, "ID", "ILCEADI");
            return PartialView("City");
        }

        public ActionResult SaveRecord(PERSONEL model)
        {
            try
            {
                BasvuruEntities3 DB = new BasvuruEntities3();
                PERSONEL pr = new PERSONEL();

                pr.ADSOYAD = model.ADSOYAD;
                pr.ILID = model.ILID;
                pr.ILCEID = model.ILCEID;
                pr.DOGUMTARIHI = model.DOGUMTARIHI;
                pr.ACIKLAMA = model.ACIKLAMA;
                pr.CINSIYET = model.CINSIYET;
                DB.PERSONELs.Add(pr);
                DB.SaveChanges();
                int latestPrId = pr.ID;
                ViewBag.Message = "Kişi Başarıyla Kaydedildi";
                TempData["prid"] = latestPrId;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Job");
        }

        public ActionResult SaveBasvuru(PERSONELISBASVURULARI model)
        {
            try
            {
                 BasvuruEntities3 DB = new BasvuruEntities3();
                 PERSONELISBASVURULARI prs = new PERSONELISBASVURULARI();

                prs.PERSONELID = model.PERSONELID;
                prs.ILID = model.ILID;
                prs.BASVURUTARIHI = model.BASVURUTARIHI;
                prs.ACIKLAMA = model.ACIKLAMA;
                prs.SEYAHATENGELIYOK = model.SEYAHATENGELIYOK;
                prs.ISYERIADI = model.ISYERIADI;
                prs.POZISYON = model.POZISYON;
                DB.PERSONELISBASVURULARIs.Add(prs);
                DB.SaveChanges();
                TempData["prid"] = prs.PERSONELID;

                ViewBag.Message = "Basvuru Kaydedildi";

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Job");
        }

        public ActionResult Personel()
        {
            TempData["prid"] = null;
            using (BasvuruEntities3 db = new BasvuruEntities3())
            {
                List<PERSONEL> employees = db.PERSONELs.ToList();
                List<ILLER> country= db.ILLERs.ToList();
                List<ILCELER> city= db.ILCELERs.ToList();

                var employeeRecord = from e in employees
                                     join d in country on e.ILID equals d.ID into table1
                                     from d in table1.ToList()
                                     join i in city on e.ILCEID equals i.ID into table2
                                     from i in table2.ToList()
                                     select new ViewModel
                                     {
                                         employees = e,
                                         country = d,
                                         city = i
                                     };
                return View(employeeRecord);
            }

        }
    
        public ActionResult PersonelBasvuru(int Id)
        {

            using (BasvuruEntities3 db = new BasvuruEntities3())
            {
                List<PERSONEL> employees = db.PERSONELs.ToList();
                List<ILLER> country = db.ILLERs.ToList();
                List<ILLER> personelcountry = db.ILLERs.ToList();
                List<ILCELER> city = db.ILCELERs.ToList();
                List<PERSONELISBASVURULARI> employeejob = db.PERSONELISBASVURULARIs.ToList();

                var employeeRecord = from jo in employeejob
                                  
                                     join p in country on jo.ILID equals p.ID into table4
                                     from p in table4.ToList()
                                     where jo.PERSONELID == Id
                                     select new ViewModel
                                     {
                                         employeejob = jo,
                                         country = p
                                     };
                return View(employeeRecord);
            }
        }
       
    }

}