using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Model
{
    public class EslestirmeBilgisi
    {
        public string KaynakKolon { get; set; }
        public string HedefKolon { get; set; }
        public string ManuelDeger { get; set; }

        public EslestirmeSonucu Sonuc { get; set; }
      
    }
}
