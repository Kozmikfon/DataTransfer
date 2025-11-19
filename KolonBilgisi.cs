using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer
{
    public class KolonBilgisi
    {
        public string DataType { get; set; }
        public int? Length { get; set; }
        public bool IsNullable { get; set; }
        public bool IsUnique { get; set; } //mukerrer kontrolu için benzersiz alan
    }
}
