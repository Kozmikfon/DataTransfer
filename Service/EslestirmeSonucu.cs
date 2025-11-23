using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Service
{
    public class EslestirmeSonucu
    {
        public bool EslestirmeUygun { get; set; } = false;      
        public List<string> Mesajlar { get; set; } = new List<string>();     
        public bool KritikHataVar { get; set; } = false;
        public bool UyariGerekli { get; set; } = false;
    }
}
