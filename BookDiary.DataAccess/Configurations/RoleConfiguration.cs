using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace BookDiary.DataAccess.Configurations
{
    public class RoleConfiguration:IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(GetRoles());
        }

        private List<IdentityRole> GetRoles()
        {
            var roles = new List<IdentityRole>()
            {
               
                new IdentityRole()
                {
                    Id = "308aa550-7cec-496d-8df4-44e7b2244b0b",
                    Name = "Admin"
                },
                 new IdentityRole()
                {
                    Id = "bf6a1a7e-3b94-4076-90f3-511fd2ed8a66" ,
                    Name = "User"
                },

            };

            return roles;
        }

        
    }
}
