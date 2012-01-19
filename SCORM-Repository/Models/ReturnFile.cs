using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace SCORM_Repository.Models
{
    public class ReturnFileBytes
    {
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
    public class ReturnFileStream
    {
        public Stream Content { get; set; }
        public string ContentType { get; set; }
    }
}