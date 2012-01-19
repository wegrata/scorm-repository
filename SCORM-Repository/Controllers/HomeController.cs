using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCORM_Repository.Models;
using System.Configuration;

namespace SCORM_Repository.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            Dictionary<String,IEnumerable<SCORMObject>> data= new Dictionary<string,IEnumerable<SCORMObject>>();

            using (SCORMObjectModel dataRepo = CurrentModel)
            {
                var categories = dataRepo.GetCategories();
                foreach (var cat in categories)
                {
                    data.Add(cat, dataRepo.GetByCategory(cat,4));
                }
            }
            return View(data);
        }
        public ActionResult Details(string id)
        {
            using (SCORMObjectModel dataRepo = CurrentModel)
            {
                return View(dataRepo.Get(id));
            }
        }
        public ActionResult About()
        {
            return View();
        }

    }
}
