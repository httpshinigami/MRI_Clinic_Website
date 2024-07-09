namespace FIT5032_Project.Migrations
{
    using FIT5032_Project.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FIT5032_Project.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FIT5032_Project.Models.ApplicationDbContext context)
        {
            // Create RoleManager and UserManager
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Check and create Admin role
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }

            // Check and create Doctor role
            if (!roleManager.RoleExists("Doctor"))
            {
                var role = new IdentityRole();
                role.Name = "Doctor";
                roleManager.Create(role);
            }

            // Check and create Staff role
            if (!roleManager.RoleExists("Staff"))
            {
                var role = new IdentityRole();
                role.Name = "Staff";
                roleManager.Create(role);
            }

            // Check and create Staff role
            if (!roleManager.RoleExists("Patient"))
            {
                var role = new IdentityRole();
                role.Name = "Patient";
                roleManager.Create(role);
            }

            // Lists of names
            List<string> firstNames = new List<string> { "Levi", "Hatsune", "Andy", "Lexie", "Misa", "Evelynn", "Xavier", "Kevin", "Jessica", "Light" };
            List<string> lastNames = new List<string> { "Ackerman", "Miku", "Xie", "Liu", "Amane", "Xie", "Lee", "Nguyen", "Tran", "Yagami" };

            Random rand = new Random();

            string adminFirstName = firstNames[rand.Next(0, firstNames.Count)];
            string adminLastName = lastNames[rand.Next(0, lastNames.Count)];

            firstNames.Remove(adminFirstName);
            lastNames.Remove(adminLastName);

            // Create 1 Admin user
            string adminEmail = "admin@sakuramri.com.au";
            if (!context.Users.Any(u => u.UserName == adminEmail))
            {
                var user = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = adminFirstName,
                    LastName = adminLastName,
                    DateOfBirth = DateTime.Now.AddYears(-30).AddDays(rand.Next(0, 30)) // Random DOB
                };

                string userPassword = "AdminPassword!";
                var chkUser = userManager.Create(user, userPassword);

                // Add user to Admin role
                if (chkUser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
            }

            // Create 5 Doctor users
            for (int i = 1; i <= 5; i++)
            {
                int doctorFirstNameIndex = rand.Next(0, firstNames.Count);
                int doctorLastNameIndex = rand.Next(0, lastNames.Count);

                string doctorFirstName = firstNames[doctorFirstNameIndex];
                string doctorLastName = lastNames[doctorLastNameIndex];

                firstNames.RemoveAt(doctorFirstNameIndex);
                lastNames.RemoveAt(doctorLastNameIndex);

                string doctorEmail = $"doctor{i}@sakuramri.com.au";
                if (!context.Users.Any(u => u.UserName == doctorEmail))
                {
                    var user = new ApplicationUser
                    {
                        UserName = doctorEmail,
                        Email = doctorEmail,
                        FirstName = doctorFirstName,
                        LastName = doctorLastName,
                        DateOfBirth = DateTime.Now.AddYears(-30).AddDays(rand.Next(0, 30)) // Random DOB
                    };

                    string userPassword = $"DoctorPassword{i}!";
                    var chkUser = userManager.Create(user, userPassword);

                    // Add user to Doctor role
                    if (chkUser.Succeeded)
                    {
                        userManager.AddToRole(user.Id, "Doctor");
                    }
                }
            }

            // Create 2 Staff members
            for (int i = 1; i <= 2; i++)
            {
                int staffFirstNameIndex = rand.Next(0, firstNames.Count);
                int staffLastNameIndex = rand.Next(0, lastNames.Count);

                string staffFirstName = firstNames[staffFirstNameIndex];
                string staffLastName = lastNames[staffLastNameIndex];

                firstNames.RemoveAt(staffFirstNameIndex);
                lastNames.RemoveAt(staffLastNameIndex);

                string staffEmail = $"staff{i}@sakuramri.com.au";
                if (!context.Users.Any(u => u.UserName == staffEmail))
                {
                    var user = new ApplicationUser
                    {
                        UserName = staffEmail,
                        Email = staffEmail,
                        FirstName = staffFirstName,
                        LastName = staffLastName,
                        DateOfBirth = DateTime.Now.AddYears(-25).AddDays(rand.Next(0, 30)) // Random DOB
                    };

                    string userPassword = $"StaffPassword{i}!";
                    var chkUser = userManager.Create(user, userPassword);

                    // Add user to Staff role
                    if (chkUser.Succeeded)
                    {
                        userManager.AddToRole(user.Id, "Staff");
                    }
                }
            }

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
