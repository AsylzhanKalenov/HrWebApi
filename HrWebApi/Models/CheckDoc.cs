using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrWebApi.Models
{
    public class CheckDoc
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsChecked { get; set; }
        public bool IsCurrect { get; set; }
        public bool IsComplete { get; set; }
        public string Comments { get; set; }
        public int UserDocId { get; set; }
        public UserDoc UserDoc { get; set; }
    }
}
