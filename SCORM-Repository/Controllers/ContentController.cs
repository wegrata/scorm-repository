using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using SCORM_Repository.Models;

namespace SCORM_Repository.Controllers
{
    public class ContentController : ControllerBase
    {
        //
        // GET: /Content/

        public ActionResult Index()
        {
            return View();

        }

        private FileResult GetFile(string id, string fileName)
        {
            using (SCORMObjectModel dataRepo = CurrentModel)
            {
                var result = dataRepo.GetFileBytes(id);
                var fileContent = new FileContentResult(result.Content, result.ContentType);
                fileContent.FileDownloadName = fileName;
                return fileContent;

            }
        }
        public FileResult ScreenShot(string id)
        {
            return GetFile(id, "screenshot.jpg");
        }
        public FileResult Package(string id)
        {
            return GetFile(id, "package.zip");
        }
    }
}
