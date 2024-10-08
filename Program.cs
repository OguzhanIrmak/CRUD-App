using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp2
{
    class Program
    {

        static SqlConnection BaglantiOlustur()
        {
            SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=NORTHWND;Integrated Security=true");
            return connection;
        }
        static int EkleSilGuncelle(string sqlstr)
        {
            SqlConnection connection = BaglantiOlustur();
            SqlCommand cmd = new SqlCommand(sqlstr, connection);
            cmd.CommandType = CommandType.Text;
            int kayitSayisi = 0;

            try
            {
                connection.Open();
                 kayitSayisi = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            finally
            {
                if(connection.State!= ConnectionState.Closed) 
                   connection.Close();
            }
            return kayitSayisi;
        }

        static SqlDataReader Listele(string sqlstr)
        {

            SqlConnection connection = BaglantiOlustur();
            SqlCommand cmd = new SqlCommand(sqlstr, connection);
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader = null;
            try
            {
                connection.Open();
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);   
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            
            return reader;
                
        }


        static void Main(string[] args)
        {
            char secim;
            do
            {
                Console.Clear();
                Console.WriteLine("İşlem Tipi");
                Console.WriteLine("------------");
                Console.WriteLine("1-Kayıt Ekle");
                Console.WriteLine("2-Kayıt Güncelle");
                Console.WriteLine("3-Kayıt Sil");
                Console.WriteLine("4-Kayıt Listesi");
                Console.WriteLine("-Çıkış için <ESC>-");
                Console.WriteLine();
                Console.Write("Seçiminiz : ");

                secim = Console.ReadKey().KeyChar;
                switch (secim)
                {
                    case '1':
                        //kayıt eklenecek
                        Console.Clear();
                        Console.Write("Kategori Adı : ");
                        string kategoriAdi = Console.ReadLine();
                        Console.Write("Açıklama :");
                        string aciklama= Console.ReadLine();
                        string sqlInsStr = string.Format("insert into Categories(CategoryName,Description) values('{0}','{1}')",kategoriAdi,aciklama);
                        Console.WriteLine("{0} adet kayıt eklendi",EkleSilGuncelle(sqlInsStr));
                        Console.ReadKey();
                        break;
                    case '2':
                        //kayıt güncellenecek
                        Console.Clear();
                        Console.Write("Güncellenecek Kategori no :"); 
                        string Upid = Console.ReadLine();
                        Console.Write("Kategorinin Yeni Adı :");
                         string kategoriYeniAdi = Console.ReadLine();
                        Console.Write(" Yeni Açıklama :");
                        string yeniaciklama = Console.ReadLine();
                        string sqlUpStr = string.Format(" update Categories set CAtegoryName='{0}', Description='{1}' where CategoryID={2}",kategoriYeniAdi,yeniaciklama,Upid);
                        Console.WriteLine("{0} adet kayıt güncellendi", EkleSilGuncelle(sqlUpStr));
                        Console.ReadKey();
                        break;
                    case '3':
                        //kayıt silinecek
                        Console.Clear();
                        Console.Write("Slinecek Kategori Id : ");
                        string delId = Console.ReadLine();  
                        string sqlDelStr = String.Format("delete Categories where CategoryID={0}",delId);
                        Console.WriteLine("{0} adet kayıt silindi", EkleSilGuncelle(sqlDelStr));
                        Console.ReadKey();
                        break;
                    case '4':
                        //kayıtlar listelenecek 
                        Console.Clear() ;   
                        SqlDataReader okuyucu = Listele("select CategoryID,CategoryName,Description from Categories");
                        while (okuyucu.Read())
                        {
                            int id =  okuyucu.GetInt32(okuyucu.GetOrdinal("CategoryID"));
                            string name = okuyucu.GetString(okuyucu.GetOrdinal("CategoryName"));
                            string desc = okuyucu.IsDBNull(okuyucu.GetOrdinal("Description"))? "Belirtilmemiş": okuyucu.GetString(okuyucu.GetOrdinal("Description"));
                            Console.WriteLine("{0,-5}{1,-20}{2}", id, name, desc);
                           
                        }
                        okuyucu.Close();
                        Console.ReadKey();


                        break;
                    default:
                        break;
                }
            } while (secim != (char)27);

        }



    }
}

