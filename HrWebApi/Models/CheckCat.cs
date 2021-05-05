using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrWebApi.Models
{
    public class CheckCat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        public bool IsCurrect { get; set; }
        public bool IsComplete { get; set; }
        public string Comments { get; set; }
        public int FileCatId { get; set; }
        public int UserId { get; set; }
        public FileCat FileCat { get; set; }
        public User User { get; set; }
    }
}
