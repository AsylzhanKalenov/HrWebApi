using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrWebApi.Models
{
    public class UserPriv
    {
        public int Id { get; set; }
        public string Iin { get; set; }
        public string Address { get; set; }
        public bool IsMarriage { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}

