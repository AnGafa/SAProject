using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAProject.Models
{
    public class File
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Password { get; set; }
        //public string FileUrl { get; set; }
        public DateTime FileExpiry { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        public virtual ApplicationUser Users { get; set; }
    }
}
