using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAProject.Models
{
    public class ApplicationUser: IdentityUser
    {
        public ApplicationUser()
        {
            this.Files = new HashSet<File>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<File> Files { get; set; }
    }
}
