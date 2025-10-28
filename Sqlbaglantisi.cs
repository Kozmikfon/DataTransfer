using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace DataTransfer
{
    class Sqlbaglantisi
    {
        public SqlConnection baglanti(string server, string user, string sifre)
        {
            
            string KaynakSorgu = $"Server={server}; User Id={user}; Password={sifre}; TrustServerCertificate=True;";
            string HedefSorgu = $"Server={server}; User Id={user}; Password={sifre}; TrustServerCertificate=True;";

            SqlConnection Kaynakbaglan = new SqlConnection(KaynakSorgu);
            SqlConnection Hedefbaglan = new SqlConnection(HedefSorgu);
            try
            {
                Kaynakbaglan.Open();
                Hedefbaglan.Open();
                MessageBox.Show("Bağlantı Başarılı");
                return Kaynakbaglan; 
                return Hedefbaglan;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bağlantı Başarısız: " + ex.Message);
                return null;
            }
        }
    }
}
 