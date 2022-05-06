using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAProject.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Key]
        public override string Id { get; set; }
        public override string Email { get; set; }

        //[Required]
        public string Name { get; set; }

        public virtual ICollection<UserFile> UserFiles { get; set; }
        public virtual ICollection<AccessLog> AccessLogs { get; set; }
    }
}
