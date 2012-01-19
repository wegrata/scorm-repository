using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Norm;
using System.ComponentModel.DataAnnotations;
namespace SCORM_Repository.Models
{
    public class SCORMObject
    {
        public SCORMObject()
        {
        }
        [MongoIdentifier]
        public ObjectId ID { get; set; }
        [Required(ErrorMessage="Title Required")]
        [StringLength(25,ErrorMessage="Must be under 25 characters")]
        public String Title { get; set; }
        [Required(ErrorMessage = "Description Required")]
        [StringLength(250, ErrorMessage = "Must be under 250 characters")]
        public String Description { get; set; }
        [Required(ErrorMessage = "Category Required")]
        public String Category { get; set; }
        public IEnumerable<FileReference> Files { get; set; }
    }
}