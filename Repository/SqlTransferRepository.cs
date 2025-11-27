using DataTransfer.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Repository
{
    public class SqlTransferRepository : IDisposable
    {
        private readonly string _connectionString;
        private readonly BaglantiBilgileri _info;
        

        private const string SQL_GET_TABLES =
            "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME NOT IN ('__EFMigrationsHistory','sysdiagrams') ORDER BY TABLE_NAME";

        public async Task<List<string>> TabloGetirAsync()
        {
            var list = new List<string>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(SQL_GET_TABLES, conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(reader.GetString(0));
                    }
                }
            }
            return list;
        }

        public async Task<List<string>> KolonAdlariniGetirAsync(string tableName)
        {
            var list = new List<string>();
            // SQL sorgusu sadece kolon isimlerini çekecek.
            string sql = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName ORDER BY ORDINAL_POSITION";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@TableName", tableName);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(reader.GetString(0));
                    }
                }
            }
            return list;
        }



        public async Task<Dictionary<string, KolonBilgisi>> KolonBilgileriniGetirAsync(string tabloAdi)
        {
            var kolonlar = new Dictionary<string, KolonBilgisi>(StringComparer.OrdinalIgnoreCase);
            
            if (string.IsNullOrWhiteSpace(tabloAdi))
                return kolonlar;

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(SQL_GET_COLUMNS, conn)) 
            {
                cmd.Parameters.AddWithValue("@TableName", tabloAdi);
                await conn.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string KolonIsmi = reader["COLUMN_NAME"].ToString();
                        string tip = reader["DATA_TYPE"].ToString();

                        int? uzunluk = reader["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value ? null : Convert.ToInt32(reader["CHARACTER_MAXIMUM_LENGTH"]);
                        bool isNullable = reader["IS_NULLABLE"].ToString().Equals("YES", StringComparison.OrdinalIgnoreCase);

                        bool isUnique = Convert.ToBoolean(reader["IsUnique"]);

                        kolonlar[KolonIsmi] = new KolonBilgisi
                        {
                            DataType = tip,
                            Length = uzunluk,
                            IsNullable = isNullable,
                            IsUnique = isUnique
                        };
                    }
                }
            }
            return kolonlar;
        }



        private const string SQL_GET_COLUMNS = @"
        SELECT 
            C.COLUMN_NAME, 
            C.DATA_TYPE, 
            C.CHARACTER_MAXIMUM_LENGTH, 
            C.IS_NULLABLE,
            ISNULL(INDEXES.IsUnique, 0) AS IsUnique
        FROM INFORMATION_SCHEMA.COLUMNS C
        JOIN sys.tables T ON T.name = C.TABLE_NAME AND T.name = @TableName
        JOIN sys.schemas S ON S.schema_id = T.schema_id
        LEFT JOIN (
            SELECT 
                COL.name AS COLUMN_NAME, 
                MAX(CAST(I.is_unique_constraint AS INT)) AS IsUnique
            FROM sys.indexes I
            JOIN sys.index_columns IC ON I.object_id = IC.object_id AND I.index_id = IC.index_id
            JOIN sys.columns COL ON COL.object_id = I.object_id AND COL.column_id = IC.column_id
            WHERE I.object_id = OBJECT_ID(@TableName) AND (I.is_unique_constraint = 1 OR I.is_primary_key = 1)
            GROUP BY COL.name
        ) AS INDEXES ON INDEXES.COLUMN_NAME = C.COLUMN_NAME
        WHERE C.TABLE_NAME = @TableName
        ORDER BY C.ORDINAL_POSITION";


        public SqlTransferRepository(BaglantiBilgileri info)
        {
            _info = info;
            _connectionString = $"Server={info.Sunucu};Database={info.Veritabani};User Id={info.Kullanici};Password={info.Sifre};TrustServerCertificate=True;";
        }



        public DataTable VeriGetir(string tablo, List<EslestirmeBilgisi> eslestirmeler, string kosul = "")
        {
            string connStr = _connectionString;
            var kolonlarinListesi = new List<string>();

            foreach (var eslestirme in eslestirmeler)
            {
                if (eslestirme.KaynakKolon == "(MANUEL GİRİŞ)")
                {

                    string manuelDeger = eslestirme.ManuelDeger;
                    string sqlLiteral;

                    if (string.IsNullOrEmpty(manuelDeger))
                    {
                        sqlLiteral = "NULL";
                    }
                    else
                    {
                        sqlLiteral = $"'{manuelDeger.Replace("'", "''")}'";
                    }

                    kolonlarinListesi.Add($"{sqlLiteral} AS [{eslestirme.HedefKolon}]");
                }
                else
                {
                   
                    kolonlarinListesi.Add($"[{eslestirme.KaynakKolon}] AS [{eslestirme.HedefKolon}]");
                }
            }

            // SQL sorgusunu oluştur
            string kolonListe = string.Join(", ", kolonlarinListesi);
            string sql = $"SELECT {kolonListe} FROM [{tablo}]";

            if (!string.IsNullOrWhiteSpace(kosul))
                sql += " WHERE " + kosul;

            var dt = new DataTable();

            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var da = new SqlDataAdapter(sql, conn))
                {
                    conn.Open();
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                // Hata mesajına SQL sorgusunu dahil etmek hata ayıklama için harika.
                throw new Exception($"VeriGetir metodu hata: {ex.Message} SQL: {sql}", ex);
            }

            return dt;
        }

        //filtre testi
        public async Task<int> SatirSayisiGetirAsync(string tablo, string kosul)
        {
            string sql = $"SELECT COUNT(1) FROM [{tablo}] WHERE {kosul}";
            if (!tablo.StartsWith("["))
            {
                tablo = $"[{tablo}]";
            }
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    await conn.OpenAsync();
                    var result = await cmd.ExecuteScalarAsync();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Filtre Testi SQL Hatası: {ex.Message}. SQL: {sql}", ex);
            }
        }

        public DataTable DataTableCalistir(string sqlCommand)
        {
            var dt = new DataTable();
            string connStr = _connectionString; // Repository'nin constructor'da oluşturduğu bağlantı dizesi

            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var da = new SqlDataAdapter(sqlCommand, conn))
                {
                    conn.Open();
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                // Hata ayıklama için SQL sorgusunu hata mesajına dahil et.
                throw new Exception($"DataTable metodu hata: {ex.Message} SQL: {sqlCommand}", ex);
            }

            return dt;
        }
        public object ExecuteScalar(string sqlCommand, Dictionary<string, object> parameters)
        {
            object result = null;
            string connStr = _connectionString; // Repository'nin constructor'da oluşturduğu bağlantı dizesi

            try
            {
                using (var conn = new SqlConnection(connStr))
                using (var cmd = new SqlCommand(sqlCommand, conn))
                {
                    conn.Open();

                    // Parametreleri komuta ekle
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            // Parametre değeri null ise DBNull.Value olarak ayarla
                            cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

                    // Sorguyu çalıştır ve ilk satırın ilk kolonundaki değeri al
                    result = cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"ExecuteScalar metodu hata: {ex.Message} SQL: {sqlCommand}", ex);
            }

            return result;
        }

        public void Dispose()
        {
           
        }
    }
}
