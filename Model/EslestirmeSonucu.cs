using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Model
{
    public class EslestirmeSonucu
    {
        public bool EslestirmeUygun { get; set; } = false;      
        public List<string> Mesajlar { get; set; } = new List<string>();     
        public bool KritikHataVar { get; set; } = false;
        public bool UyariGerekli { get; set; } = false;
        public DonusumTuru DonusumTipi { get; set; }

        public Dictionary<string,object> DonusumSozlugu { get; set; } = new Dictionary<string, object>();

        
    }

    public enum DonusumTuru
    {
        Yok,
        BasitTipDonusumu, 
        FormatDonusumu,  
        LookupEslestirme,
        KaynakLookupEslestirme
    }
}
