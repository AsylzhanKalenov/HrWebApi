using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrWebApi.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
