using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Model
{
    public class KolonBilgisi
    {
        public string DataType { get; set; }
        public int? Length { get; set; }
        public bool IsNullable { get; set; }
        public bool IsUnique { get; set; } 


        private static bool SayiKontrolu(string dataType)
        {
            
            string[] numericTypes = { "int", "bigint", "smallint", "tinyint", "decimal", "numeric", "float", "real", "money", "smallmoney" };            
            return numericTypes.Contains(dataType.ToLower());
        }

        private static bool MetinselTip(string dataType)
        {
            string[] textTypes = { "nvarchar", "varchar", "nchar", "char", "text", "ntext" };
           
            return textTypes.Contains(dataType.ToLower());
        }
        public bool IsStringBased
        {
            get
            {
                return MetinselTip(this.DataType);
            }
        }
        public bool IsNumeric
        {
            get
            {
                return SayiKontrolu(this.DataType);
            }
        }
    }
}
