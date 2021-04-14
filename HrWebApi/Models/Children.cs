using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrWebApi.Models
{
    public class Children
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Lastname { get; set; }
        public DateTime BirthDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
