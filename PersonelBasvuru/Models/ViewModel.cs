using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonelBasvuru.Models
{
    public class ViewModel
    {
        public PERSONEL employees { get; set; }
        public ILLER country { get; set; }
        public ILLER personelcountry { get; set; }
        public ILCELER city { get; set; }
        public PERSONELISBASVURULARI employeejob{ get; set; }
    }
}