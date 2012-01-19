using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCORM_Repository.Models;
using System.Configuration;

namespace SCORM_Repository.Controllers
{
    public class ControllerBase : Controller
    {
        protected SCORMObjectModel CurrentModel
        {
            get
            {
                if (Session["connectionString"] != null && User.Identity.IsAuthenticated)
                {
                    return new SCORMObjectModel(Session["connectionString"].ToString());                    
                }
                return new SCORMObjectModel(ConfigurationManager.ConnectionStrings["MongoConnectionString"].ConnectionString);
                

            }
        }
    }
}
