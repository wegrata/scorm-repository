using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCORM_Repository.Models
{
    public class FileReference
    {
        public enum FileType
        {
            SCORMPackage,
            Screenshot,            
        }
        public string Filename { get; set; }
        public string FileId { get; set; }
        public FileType Type { get; set; }
        public string  ContentType { get; set; }

    }
}