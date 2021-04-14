using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrWebApi.ViewModels
{
    public class PrivViewModel
    {
        public int id { get; set; }
        public int userid { get; set; }
        [Required(ErrorMessage = "Не указан ИИН")]
        public string iin { get; set; }
        [Required(ErrorMessage = "Не указан Адрес")]
        public string address { get; set; }
        public bool ismarrige { get; set; }
        public Childrens[] Children { get; set; }
    }
    public class Childrens
    {
        public string fio { get; set; }
        public DateTime BirthDate { get; set; }

    }
}
