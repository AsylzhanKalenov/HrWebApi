using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrWebApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Lastname { get; set; }
        public bool Male { get; set; }
        public byte Status { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Defaultpass { get; set; }
        public DateTime RegDate { get; set; }
        public int? RoleId { get; set; }
        public int? CompanyId { get; set; }
        public int? PositionId { get; set; }
        public Role Role { get; set; }
        public Company Company { get; set; }
        public Position Position { get; set; }

    }

    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
