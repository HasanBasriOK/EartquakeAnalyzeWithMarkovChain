using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;


namespace Analyze
{
    public class DEPREM
        // property tanımlandı , displayname de istediğimiz ismi yazdırmak içn component model kullanıldı
    {
        [DisplayName("Tarih")]
        public string DATE { get; set; }
        [DisplayName("Yer")]
        public string PLACE { get; set; }
        [DisplayName("Şiddet")]
        public string SIDDET { get; set; }
        [DisplayName("Tip")]
        public string TYPE { get; set; }

        // deneme için yapılan kısım 
        public DEPREM GetTop1Objects() // okuma işlemeni denemek için top 1 obje veri tabanından alındı
        {
            //List<DEPREM> items = new List<DEPREM>();
            string ConnectionString = "Server=ROCK;Database=DEPREM;Integrated Security=true;";

            string CommandString = "SELECT TOP 1 * FROM DepremBilgileri";
            DEPREM item = new DEPREM();


            using (SqlConnection con = new SqlConnection(ConnectionString)) // uising kullanılmasının nedeni , tanımlanan nesnenin kullanımı bittiğinde otomatik olarak hafızadan silinmesi istendiği içn
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(CommandString, con))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        // ? string empty ,veri tabanında null ise çevriliyor program patlamasın diye
                        item.DATE = dr["time"].ToString();
                        item.SIDDET = string.IsNullOrEmpty(dr["mag"].ToString()) ? string.Empty : dr["mag"].ToString();
                        item.PLACE = string.IsNullOrEmpty(dr["place"].ToString()) ? string.Empty : dr["place"].ToString();
                        item.TYPE = string.IsNullOrEmpty(dr["type"].ToString()) ? string.Empty : dr["type"].ToString();

                        //items.Add(item);

                    }
                }
            }
            return item;
            //return items;
        }
        public List<DEPREM> GetObjects() // items listeme veritabanımda okuduğum her satırı item olarak okuyup ekliyorum,deprem tipinde (propertiler siddet vs...)
       {
            List<DEPREM> items = new List<DEPREM>();
            string ConnectionString = "Server=ROCK;Database=DEPREM;Integrated Security=true;";

            string CommandString = "SELECT * FROM DepremBilgileri";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(CommandString, con))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        DEPREM item = new DEPREM();

                        item.DATE = dr["time"].ToString();
                        item.SIDDET = string.IsNullOrEmpty(dr["mag"].ToString()) ? string.Empty : dr["mag"].ToString();
                        item.PLACE = string.IsNullOrEmpty(dr["place"].ToString()) ? string.Empty : dr["place"].ToString();
                        item.TYPE = string.IsNullOrEmpty(dr["type"].ToString()) ? string.Empty : dr["type"].ToString();

                        items.Add(item);

                    }
                }
            }
            return items;
        }
    }
}
