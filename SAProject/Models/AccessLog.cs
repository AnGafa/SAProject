using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAProject.Models
{
    public class AccessLog
    {
        [Key]
        public int LogId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string IpAddress { get; set; }

        [ForeignKey("User")]
        [Column(Order = 1)]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("File")]
        [Column(Order = 1)]
        public int FileId { get; set; }
        public virtual File File { get; set; }
    }
}
