using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCORM_Repository.Models;
using System.Configuration;
using System.Web.Security;

namespace SCORM_Repository.Controllers
{
    public class UploadController : ControllerBase
    {
        //
        // GET: /Upload/
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Upload/Details/5
        [Authorize]
        public ActionResult Details(string id)
        {
            return View();
        }

        //
        // GET: /Upload/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Upload/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(SCORMObject package, HttpPostedFileBase scormPackage, HttpPostedFileBase screenshot)
        {
            try
            {
                using (SCORMObjectModel dataRepo = CurrentModel)
                {                    
                    FileReference fr = new FileReference { 
                         ContentType = scormPackage.ContentType,
                         Filename = scormPackage.FileName,
                         Type = FileReference.FileType.SCORMPackage
                    };
                    dataRepo.AttachFile(scormPackage.InputStream, fr, package);
                    fr = new FileReference
                    {
                        ContentType = screenshot.ContentType,
                        Filename = screenshot.FileName,
                        Type = FileReference.FileType.Screenshot
                    };
                    dataRepo.AttachFile(screenshot.InputStream, fr, package);
                    dataRepo.Save(package);
                }
                // TODO: Add insert logic here
                return RedirectToAction("Index","Home");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Upload/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Upload/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Upload/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Upload/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
