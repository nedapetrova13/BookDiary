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
    public class UserRoleConfiguration:IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            if (!builder.Metadata.GetDefaultTableName().Contains("AspNetRoles"))
            {
                builder.HasData(GetUserRoles());
            }
        }
        private List<IdentityUserRole<string>> GetUserRoles()
        {
            var userRoles = new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>()
                {
                    RoleId = "308aa550-7cec-496d-8df4-44e7b2244b0b",
                    UserId = "e18f622d-dbf6-447a-9aac-00fa1589e1a0"
                },
                new IdentityUserRole<string>()
                {
                    RoleId = "bf6a1a7e-3b94-4076-90f3-511fd2ed8a66",
                    UserId = "6fa501af-2cf7-4a04-9def-536e827ae894"
                }
               
            };


            return userRoles;
        }
    }
}
