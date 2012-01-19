using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCORM_Repository.Models;
using System.Configuration;

namespace SCORM_Repository.Controllers
{
    public class CategoryController : ControllerBase
    {
        //
        // GET: /Category/

        public ActionResult Index(string id)
        {
            ViewBag.Title = id;
            if (!String.IsNullOrEmpty(id))
            {
                IEnumerable<SCORMObject> data;
                using (SCORMObjectModel dataRepo = CurrentModel)
                {
                    data = dataRepo.GetByCategory(id); ;
                }
                return View(data);
            }
            return View();
        }

    }
}
