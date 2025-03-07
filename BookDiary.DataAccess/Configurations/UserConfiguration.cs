using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookDiary.Models;
using BookDiary.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookDiary.DataAccess.Configurations
{
    public class UserConfiguration:IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(GetUsers());
        }
        public List<User> GetUsers()
        {
            var users = new List<User>();
            var passwordHasher = new PasswordHasher<User>();
            var user = new User()
            {
                Id = "e18f622d-dbf6-447a-9aac-00fa1589e1a0",
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@abv.bg",
                NormalizedEmail = "ADMIN@ABV.BG",
                Name = "Admin",
                Gender = GenderEnum.Мъж.ToString(),
                Birthdate = DateTime.ParseExact("2015-07-18 11:20", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                ProfilePictureURL = "https://api-private.atlassian.com/users/d28c8f9e6777c0b0b8c2d844ce1c0bef/avatar"
            };
            user.PasswordHash = passwordHasher.HashPassword(user, "admin123");

            users.Add(user);
            var user2 = new User()
            {
                Id = "6fa501af-2cf7-4a04-9def-536e827ae894",
                UserName = "neda",
                NormalizedUserName = "NEDA",
                Email = "neda@abv.bg",
                NormalizedEmail = "NEDA@ABV.BG",
                Name = "Neda",
                Gender = GenderEnum.Жена.ToString(),
                Birthdate = DateTime.ParseExact("2006-01-13 11:20", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                ProfilePictureURL = "https://www.pngkey.com/png/detail/297-2978655_profile-picture-default-female.png"
            };
            user2.PasswordHash = passwordHasher.HashPassword(user2, "123456");

            users.Add(user2);

            return users;
        }
    }
}
