using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCORM_Repository.Models;
using System.Configuration;

namespace SCORM_Repository.Controllers
{
    public class ResultsController : ControllerBase
    {
        //
        // GET: /Results/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Results/Details/5

        public ActionResult Details(string id)
        {
            IEnumerable<SCORMObject> data;
            using (SCORMObjectModel dataRepo = CurrentModel)
            {
                data = dataRepo.Search(id).ToArray() ;
            }
            return View(data);

        }
    }
}
