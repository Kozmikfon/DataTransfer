using DataTransfer.Model;
using DataTransfer.Service;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DataTransfer.Repository
{
    public class SqlTransferRepository : IDisposable
    {
        private readonly string _connectionString;
        private readonly BaglantiBilgileri _info;

        private SqlConnection _connection;
        private SqlTransaction _transaction;


        public SqlTransferRepository(BaglantiBilgileri info)
        {
            _info = info;
            _connectionString = $"Server={info.Sunucu};Database={info.Veritabani};User Id={info.Kullanici};Password={info.Sifre};TrustServerCertificate=True;";

            _connection = new SqlConnection(_connectionString);
        }

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

        public async Task<List<string>> TabloAdlariniGetirAsync()
        {
            var list = new List<string>();
            // SQL Server'da tüm kullanıcı tablolarını çeken standart sorgu
            string sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
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






        public DataTable VeriGetir(string tablo, List<EslestirmeBilgisi> eslestirmeler, string kosul = "")
        {
            string connStr = _connectionString;
            var kolonlarinListesi = new List<string>();

            foreach (var eslestirme in eslestirmeler)
            {
                // YENİ 1. KONTROL: Lookup Eşleştirme Kontrolü (En Yüksek Öncelik)
                // EslestirmeBilgisi modelinizdeki Sonuc alanının dolu olduğunu varsayıyoruz.
                if (eslestirme.Sonuc != null && eslestirme.Sonuc.DonusumTipi == DonusumTuru.LookupEslestirme)
                {
                    // Dönüşüm varsa, CASE WHEN ifadesini kullan
                    string caseWhen = sqlIfadesiOlustur(eslestirme);
                    kolonlarinListesi.Add(caseWhen);
                }
                // MEVCUT 2. KONTROL: Manuel Giriş
                else if (eslestirme.KaynakKolon == "(MANUEL GİRİŞ)")
                {
                    string manuelDeger = eslestirme.ManuelDeger;
                    string sqlLiteral;

                    if (string.IsNullOrEmpty(manuelDeger))
                    {
                        sqlLiteral = "NULL";
                    }
                    else
                    {
                        // Mevcut tırnak kaçırma mantığı
                        sqlLiteral = $"'{manuelDeger.Replace("'", "''")}'";
                    }

                    kolonlarinListesi.Add($"{sqlLiteral} AS [{eslestirme.HedefKolon}]");
                }
                // MEVCUT 3. KONTROL: Direkt Kolon Eşleşmesi veya Diğer Dönüşüm Tipleri
                else
                {
                    // Dönüşüm gerektirmeyen veya basit tip dönüşümü olan kolonlar
                    kolonlarinListesi.Add($"[{eslestirme.KaynakKolon}] AS [{eslestirme.HedefKolon}]");
                }
            }

            // SQL sorgusunun geri kalanı aynı kalır
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
                throw new Exception($"VeriGetir metodu hata: {ex.Message} SQL: {sql}", ex);
            }

            return dt;
        }


        private string sqlIfadesiOlustur(EslestirmeBilgisi eslesme)
        {
            // ... (başlangıç kısmı aynı) ...
            var sozluk = eslesme.Sonuc?.DonusumSozlugu;
            if (sozluk == null || sozluk.Count == 0)
            {
                return $"[{eslesme.KaynakKolon}] AS [{eslesme.HedefKolon}]";
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("CASE");

            foreach (var kvp in sozluk)
            {
                string kaynakDeger = kvp.Key;
                object hedefDeger = kvp.Value;

                // 1. Kaynak değeri tırnak içine al (Örn: '1' veya 'MAHMUT')
                string kaynakDegerSQL = $"'{kaynakDeger.Replace("'", "''")}'";

                // 2. KRİTİK DÜZELTME: Hedef değeri işleme
                string hedefDegerString = hedefDeger.ToString();
                string hedefDegerSQL;

                // Varsayım: Eğer hedef değer sayısal değilse (yani string ise), tırnak içine alınmalıdır.
                // En basit kontrol: Sayısal bir değer mi? (Daha kapsamlı kontrol yapılabilir)
                if (int.TryParse(hedefDegerString, out int _))
                {
                    // Sayısal ID ise tırnak yok (Örn: 101)
                    hedefDegerSQL = hedefDegerString;
                }
                else
                {
                    // String ise tırnak içine al (Örn: 'musteri')
                    hedefDegerSQL = $"'{hedefDegerString.Replace("'", "''")}'";
                }

                // SQL'de: WHEN KaynakKolon = 'KaynakDeger' THEN HedefDeger
                sb.AppendLine($"  WHEN [{eslesme.KaynakKolon}] = {kaynakDegerSQL} THEN {hedefDegerSQL}");
            }

            // ... (ELSE NULL ve END AS aynı) ...
            sb.AppendLine($"  ELSE NULL");
            sb.Append($"END AS [{eslesme.HedefKolon}]");

            return sb.ToString();
        }

        // SqlTransferRepository sınıfında (Hedef bağlantısını kullandığını varsayıyorum)

        // Bu metot, herhangi bir kolonda arama yapıp, başka bir kolondan sonuç döndürebilir.
        public object HedefDegerGetir(
            string tabloAdi,
            string aramaKolonu,
            object arananDeger,
            string donenKolon)
        {
            // SQL sorgusu: Aranan değere göre dönen kolonu getir
            string sql = $@"
        SELECT TOP 1 [{donenKolon}] 
        FROM [{tabloAdi}] 
        WHERE [{aramaKolonu}] = @ArananDeger";

            try
            {
                // KRİTİK 3: Command, Repository'nin bağlantı ve Transaction'ını kullanır.
                using (var cmd = new SqlCommand(sql, _connection, _transaction))
                {
                    cmd.Parameters.AddWithValue("@ArananDeger", arananDeger);

                    if (_connection.State != ConnectionState.Open)
                    {
                        _connection.Open();
                    }

                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda null veya özel bir hata fırlatılabilir.
                throw new Exception($"Hedef sistemde dinamik arama hatası: {ex.Message} SQL: {sql}", ex);
            }

            return null;
        }



        // SqlTransferRepository.cs içinde

        public async Task<object> ExecuteScalarAsync(string sqlCommand, Dictionary<string, object> parameters)
        {
            object result = null;

            try
            {
                using (var cmd = new SqlCommand(sqlCommand, _connection, _transaction))
                {
                    if (_connection.State != ConnectionState.Open)
                    {
                        await _connection.OpenAsync();
                    }
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            // Parametreleri eklerken await kullanmaya gerek yok.
                            cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }
                    // Asenkron olarak çalıştır
                    result = await cmd.ExecuteScalarAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"ExecuteScalarAsync metodu hata: {ex.Message} SQL: {sqlCommand}", ex);
            }
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

            try
            {
                using (var cmd = new SqlCommand(sqlCommand, _connection, _transaction))
                {
                    if (_connection.State != ConnectionState.Open)
                    {
                        _connection.Open();
                    }
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }
                    result = cmd.ExecuteScalar();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"ExecuteScalar metodu hata: {ex.Message} SQL: {sqlCommand}", ex);
            }

            return result;
        }


        public async Task<List<KaynakDonusumSatiri>> LookupDegerleriCekAsync(
     string kaynakTablo, string kaynakKolon,
     string aramaTablo, string aramaDegerKolon, string aramaIdKolon)
        {
            // DISTINCT kaynak değerlerini çek
            string sql = $"SELECT DISTINCT [{kaynakKolon}] FROM [{kaynakTablo}]";

            // Asenkron çalışmak için Task.Run kullanıyoruz
            var dt = await Task.Run(() => DataTableCalistir(sql));

            var eslesmeListesi = new List<KaynakDonusumSatiri>();

            foreach (DataRow row in dt.Rows)
            {
                // DBNull kontrolü ve string'e çevirme. Null ise string.Empty döner.
                string kaynakDeger = row[0] is DBNull ? string.Empty : row[0].ToString();
                object eslesenID = null;

                // Lookup işlemi için SQL sorgusu
                string lookupSql = $"SELECT TOP 1 [{aramaIdKolon}] FROM [{aramaTablo}] WHERE [{aramaDegerKolon}] = @kaynakDeger";
                var parameters = new Dictionary<string, object> { { "@kaynakDeger", kaynakDeger } };

                try
                {
                    // ExecuteScalar, kaynak veritabanında çalışır.
                    eslesenID = ExecuteScalar(lookupSql, parameters);
                }
                catch (Exception) { /* Hata yoksayılır, eşleşme bulunamadı sayılır */ }

                string durum = (eslesenID != null && eslesenID != DBNull.Value) ? "Oto Eşleşti" : "Eşleşme Bulunamadı";

                eslesmeListesi.Add(new KaynakDonusumSatiri
                {
                    KaynakDeger = kaynakDeger,
                    HedefKaynagaAtanacakDeger = eslesenID,
                    Durum = durum
                });
            }

            return eslesmeListesi;
        }

        public List<ZorunluKolonBilgisi> ZorunluKolonlariCek(string tabloAdi, string idKolonAdi)
        {
            // ... (SQL sorgusu aynı kalır, OBJECT_ID kullanılan sağlam sorgu)

            string sql = $@"
        SELECT 
            c.name AS COLUMN_NAME, 
            t.name AS DATA_TYPE
        FROM 
            sys.columns c
        INNER JOIN
            sys.types t ON t.user_type_id = c.user_type_id
        WHERE 
            c.object_id = OBJECT_ID(@tabloAdi) AND 
            c.is_nullable = 0 AND 
            c.is_identity = 0 AND 
            c.default_object_id = 0 AND 
            c.name != @idKolonAdi";

            var zorunluKolonlar = new List<ZorunluKolonBilgisi>();

            try
            {
                // ... (Bağlantı ve Command nesneleri)
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@tabloAdi", tabloAdi);
                    cmd.Parameters.AddWithValue("@idKolonAdi", idKolonAdi);

                    conn.Open();
                    // DataReader ile dönerek listeyi dolduruyoruz
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            zorunluKolonlar.Add(new ZorunluKolonBilgisi
                            {
                                KolonAdi = reader["COLUMN_NAME"].ToString(),
                                veriTipi = reader["DATA_TYPE"].ToString()
                            });
                        }
                    }
                    return zorunluKolonlar; // Listeyi döndür
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Zorunlu Kolon Metadata Sorgusu Hatası: {ex.Message} SQL: {sql}", ex);
            }
        }

        //veri bütünlüğü için kontroller

        public void BeginTransaction()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            _transaction = _connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction = null;
            }
        }

        public void RollbackTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction = null;
            }
        }


        public void Dispose()
        {
            if (_transaction != null)
            {

                try { _transaction.Rollback(); }
                catch { }
                _transaction.Dispose();
            }

            if (_connection != null && _connection.State != ConnectionState.Closed)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }
    }
}