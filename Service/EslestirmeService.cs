using DataTransfer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Service
{
    public class EslestirmeService
    {
       
        private bool IsMetinselTip(string tip)
        {
            return new[] { "nvarchar", "nchar", "varchar", "char", "text", "ntext" }.Contains(tip);
        }

        private bool IsSayisalTip(string tip)
        {
            return IsTamSayiliTip(tip) || IsOndalikliTip(tip);
        }

        private bool IsTamSayiliTip(string tip)
        {
            return new[] { "tinyint", "smallint", "int", "bigint" }.Contains(tip);
        }

        private bool IsOndalikliTip(string tip)
        {
            return new[] { "real", "float", "decimal", "numeric", "money" }.Contains(tip);
        }

        private bool IsTarihTip(string tip)
        {
            return new[] { "date", "datetime", "datetime2", "smalldatetime", "time" }.Contains(tip);
        }


        public EslestirmeSonucu KontrolEt(KolonBilgisi kaynak, KolonBilgisi hedef, string kaynakKolonAdi)
        {
            var sonuc = new EslestirmeSonucu();
            sonuc.Mesajlar = new List<string>();

            if (kaynak == null || hedef == null)
            {
                sonuc.KritikHataVar = true;
                sonuc.Mesajlar.Add("Kolon bilgisi eksik.");
                return sonuc;
            }

            string kaynakTip = kaynak.DataType.ToLower();
            string hedefTip = hedef.DataType.ToLower();

            if (!hedef.IsNullable && kaynak.IsNullable)
            {
                sonuc.KritikHataVar = true;
                sonuc.Mesajlar.Add("Hedef NULL kabul etmiyor");
            }

            bool kaynakMetin = IsMetinselTip(kaynakTip);
            bool hedefMetin = IsMetinselTip(hedefTip);

            if (kaynakMetin && hedefMetin)
            {
              
                if (!string.Equals(kaynakTip, hedefTip, StringComparison.OrdinalIgnoreCase))
                {
                    sonuc.Mesajlar.Add($"Tip Dönüşümü ({kaynakTip}->{hedefTip})");
                    sonuc.UyariGerekli = true;
                }

                if (kaynak.Length.HasValue && hedef.Length.HasValue)
                {
                    long kaynakLen = kaynak.Length.Value == -1 ? int.MaxValue : kaynak.Length.Value;
                    long hedefLen = hedef.Length.Value == -1 ? int.MaxValue : hedef.Length.Value;

                    if (hedefLen < kaynakLen)
                    {
                        string kStr = kaynak.Length.Value == -1 ? "MAX" : kaynak.Length.Value.ToString();
                        string hStr = hedef.Length.Value == -1 ? "MAX" : hedef.Length.Value.ToString();

                        sonuc.Mesajlar.Add($"Kırpılma Riski ({kStr}->{hStr})");
                        sonuc.UyariGerekli = true;
                    }
                    else if (hedefLen > kaynakLen)
                    {
                        string kStr = kaynak.Length.Value == -1 ? "MAX" : kaynak.Length.Value.ToString();
                        string hStr = hedef.Length.Value == -1 ? "MAX" : hedef.Length.Value.ToString();
                        sonuc.Mesajlar.Add($" ({kStr}->{hStr})");
                    }
                }
            }

            else if (IsSayisalTip(kaynakTip) && IsSayisalTip(hedefTip))
            {
                // Ondalıklı Sayı Tam Sayı 
                if (IsOndalikliTip(kaynakTip) && IsTamSayiliTip(hedefTip))
                {
                    sonuc.Mesajlar.Add("Uygun Değil");
                    sonuc.KritikHataVar = true;
                }
                
            }
            
            else
            {
                bool kaynakTarih = IsTarihTip(kaynakTip);
                bool hedefTarih = IsTarihTip(hedefTip);

                
                if ((IsSayisalTip(kaynakTip) || kaynakTarih) && hedefMetin)
                {
                    sonuc.Mesajlar.Add($"UYUŞMAZLIK: {kaynakTip} -> {hedefTip}");
                    sonuc.KritikHataVar = true;
                }
                
                else if (kaynakMetin && (IsSayisalTip(hedefTip) || hedefTarih))
                {
                    sonuc.Mesajlar.Add($"UYUŞMAZLIK: {kaynakTip} -> {hedefTip} (Tip Çakışması)");
                    sonuc.KritikHataVar = true;
                }
              
                else if (!string.Equals(kaynakTip, hedefTip, StringComparison.OrdinalIgnoreCase))
                {
                    sonuc.Mesajlar.Add($"Alakasız Tip Uyuşmazlığı: {kaynakTip} -> {hedefTip}");
                    sonuc.UyariGerekli = true;
                }
            }
           
            sonuc.EslestirmeUygun = !sonuc.KritikHataVar && !sonuc.UyariGerekli;

            return sonuc;
        }


    }
}
