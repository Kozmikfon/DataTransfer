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
        public bool IsUnique { get; set; } //mukerrer kontrolu için benzersiz alan


        private static bool SayiKontrolu(string dataType)
        {
            // Mevcut metot tanımınızı kullanır
            string[] numericTypes = { "int", "bigint", "smallint", "tinyint", "decimal", "numeric", "float", "real", "money", "smallmoney" };
            // "money" ve "smallmoney" eklenmiştir (genellikle sayısal kabul edilir).
            return numericTypes.Contains(dataType.ToLower());
        }

        private static bool MetinselTip(string dataType)
        {
            // Mevcut metot tanımınızı kullanır
            string[] textTypes = { "nvarchar", "varchar", "nchar", "char", "text", "ntext" };
            // "text" ve "ntext" eklenmiştir (genellikle metinsel olarak kabul edilir).
            return textTypes.Contains(dataType.ToLower());
        }
        public bool IsStringBased
        {
            get
            {
                // Statik yardımcı metodu kullanarak sonucu döndürür
                return MetinselTip(this.DataType);
            }
        }
        public bool IsNumeric
        {
            get
            {
                // Statik yardımcı metodu kullanarak sonucu döndürür
                return SayiKontrolu(this.DataType);
            }
        }
    }
}
