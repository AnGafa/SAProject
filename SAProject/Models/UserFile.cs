using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAProject.Models
{
    public class UserFile
    {
        [Key]
        public int UserFileId { get; set; }

        [ForeignKey("User")]
        [Column(Order=1)]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("File")]
        [Column(Order = 1)]
        public int FileId { get; set; }
        public virtual File File { get; set; }
    }
}
