using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Model
{
    public class KaynakDonusumUyarıBilgisi
    {
        public string KontrolEdilenAlan { get; set; }
        public string KaynakTipi { get; set; }
        public long? KaynakUzunluk { get; set; }
        public string HedefTipi { get; set; }
        public long? HedefUzunluk { get; set; }
        public string UyariMesaji { get; set; }
    }
}
