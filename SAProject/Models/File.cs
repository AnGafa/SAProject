using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAProject.Models
{
    public class File
    {
        public File()
        {
            this.Users = new HashSet<ApplicationUser>();
        }

        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Password { get; set; }
        //public string FileUrl { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DateValidation(ErrorMessage = "date can't be earlier than today's date")]
        public DateTime? FileExpiry { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
    }

    public class DateValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime todayDate = Convert.ToDateTime(value);
            return todayDate > DateTime.Now;
        }
    }
}
