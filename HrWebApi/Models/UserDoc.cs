using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrWebApi.Models
{
    public class UserDoc
    {
        public int Id { get; set; }
        public DateTime SendedDate { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public int Countturn { get; set; }
        public int UserId { get; set; }
        public int FileCatsId { get; set; }
        public User User { get; set; }
        public FileCat FileCats { get; set; }
    }
}
