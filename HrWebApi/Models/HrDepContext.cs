using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrWebApi.Models
{
    public class HrDepContext : DbContext
    {
        public DbSet<Role> Role { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Position> Position { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserDoc> UserDoc { get; set; }
        public DbSet<Children> Children { get; set; }
        public DbSet<CheckDoc> CheckDoc { get; set; }
        public DbSet<CheckCat> CheckCat { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<FileCat> FileCat { get; set; }
        public DbSet<UserPriv> UserPriv { get; set; }

        public HrDepContext(DbContextOptions<HrDepContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string depRoleName = "hrmanager";
            string userRoleName = "user";

            string adminEmail = "asyl@mail.ru";
            string adminPassword = "123Pass";

            // добавляем роли
            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role departRole = new Role { Id = 2, Name = depRoleName };
            Role userRole = new Role { Id = 3, Name = userRoleName };
            Company company1 = new Company { Id = 1, Name = "MCS" };
            Company company2 = new Company { Id = 2, Name = "Clinic" };
            Company company3 = new Company { Id = 3, Name = "Ansar" };
            Company company4 = new Company { Id = 4, Name = "Serber" };
            Company company5 = new Company { Id = 5, Name = "Normandy" };

            Position position1 = new Position { Id = 1, Name="Хирург" };
            Position position2 = new Position { Id = 2, Name = "Акушер" };
            Position position3 = new Position { Id = 3, Name = "Кассир" };
            Position position4 = new Position { Id = 4, Name = "Администратор" };
            Position position5 = new Position { Id = 5, Name = "Айтишник" };

            User adminUser = new User { Id = 1, Email = adminEmail, Password = BCrypt.Net.BCrypt.HashPassword(adminPassword), RoleId = adminRole.Id, CompanyId = company1.Id};

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, departRole, userRole });
            modelBuilder.Entity<Company>().HasData(new Company[] { company1, company2, company3, company4, company5 });
            modelBuilder.Entity<Position>().HasData(new Position[] { position1, position2, position3, position4, position5 });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
            base.OnModelCreating(modelBuilder);
        }
    }
}
