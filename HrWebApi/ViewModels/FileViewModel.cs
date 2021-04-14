using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HrWebApi.ViewModels
{
    public class FileViewModel
    { 
        public int UserId { get; set; }
        public int FileCatsId { get; set; }
        public List<string> Specialization { get; set; }
        public List<string> Udv { get; set; }
        public List<string> Pension { get; set; }
        public List<string> Certificates { get; set; }
        public List<string> Employhis { get; set; }
        public List<string> Addres { get; set; }
        public List<string> Conviction { get; set; }
        public List<string> Narcodisp { get; set; }
        public List<string> Psychodisp { get; set; }
        public List<string> Military { get; set; }
        public List<string> Docphoto { get; set; }
        public List<string> Refmainjob { get; set; }
        public List<string> Marriage { get; set; }
        public List<string> Cash { get; set; }
        public List<string> Forma086 { get; set; }
        public Byte[] FileByte { get; set; }
    }
}
