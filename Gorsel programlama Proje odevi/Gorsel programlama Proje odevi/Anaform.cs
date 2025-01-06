using Microsoft.Azure.WebJobs.Extensions.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Drawing.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Gorsel_programlama_Proje_odevi
{
    public partial class Anaform : Form
    {

        SqlConnection cn = new SqlConnection(Properties.Settings.Default.Calisanaktar);


        public Anaform()
        {
            InitializeComponent();
            VeritabanıBağlantısınıAç();
        }

        private void VeritabanıBağlantısınıAç()
        {

            try
            {
                if (cn != null && cn.State == ConnectionState.Closed)
                    cn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }

       
        private void Form1_Load(object sender, EventArgs e)
        {


            AktarımYapıldımı();
            
        }

        private void AktarımYapıldımı()
        {

            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Employees", cn);
            object kayıtSayısı = cmd.ExecuteScalar();

            if ((int)kayıtSayısı > 0)
                aktarToolStripMenuItem.Enabled = false;
        }

        private void TxtDosyalarınıOkuveVeritabanınaAktar()
        {
          


            string abc;
            string dosyaYolu = Application.StartupPath + "\\Employees.txt";
            


            if (!File.Exists(dosyaYolu))
            {
                MessageBox.Show("Çalışan dosyası bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);  // aktarı pasif hale getirmesi gerek
                return;
            }


            StreamReader sr = new StreamReader(dosyaYolu, Encoding.GetEncoding("iso-8859-1"));

            while ((abc = sr.ReadLine()) != null)
            {
                string çalışanNo = abc.Substring(0, 3).Trim();//ilgili alanlari aliyoruz
                string unvan = abc.Substring(3, 1).Trim();
                string adsoyad = abc.Substring(4, 24).Trim();
                string dogumTarihi = abc.Substring(28, 10).Trim();
                string dogumYeri = abc.Substring(38, 12).Trim();
                string adres = abc.Substring(50).Trim();

                int etkilenKayıtSayısı = ÇalışanKişiyiVeritabanınaKaydet(çalışanNo, unvan, adsoyad, dogumTarihi, dogumYeri, adres);

                if (etkilenKayıtSayısı <= 0)
                {
                    MessageBox.Show("Hata oluştu. İşlemleri durduruyorum. Çalışan No" + çalışanNo);
                    break;
                }

            }

            ////////////////////////////////////////////////////////////////////
            ///

            dosyaYolu = Application.StartupPath + "\\Engineer.txt";

             sr = new StreamReader(dosyaYolu, Encoding.GetEncoding("iso-8859-1"));


            while ((abc = sr.ReadLine()) != null)
            {
                string çalışanNo = abc.Substring(0, 3).Trim();//ilgili alanlari aliyoruz
                string Alanlar = abc.Substring(3, 16).Trim();
                string Cgpa = abc.Substring(19, 5).Trim();
                string Diller = abc.Substring(24, 27).Trim();
                string Maaş = abc.Substring(51).Trim();   // burası aktarılmadı buaraya tekrardan bak önemli 


                int etkilenKayıtSayısı = Engineer(çalışanNo, Alanlar, Cgpa, Diller, Maaş);

                if (etkilenKayıtSayısı <= 0)
                {
                    MessageBox.Show("Hata oluştu. İşlemleri durduruyorum. Çalışan No" + çalışanNo);
                    break;
                }




            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////
            ///

             dosyaYolu = Application.StartupPath + "\\Labourer.txt";



            if (!File.Exists(dosyaYolu))
            {
                MessageBox.Show("Çalışan dosyası bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);  // aktarı pasif hale getirmesi gerek
                return;
            }


            sr = new StreamReader(dosyaYolu, Encoding.GetEncoding("iso-8859-1"));

            while ((abc = sr.ReadLine()) != null)
            {
                string çalışanNo = abc.Substring(0, 3).Trim();//ilgili alanlari aliyoruz
                string SaatlikÜcret = abc.Substring(3, 2).Trim();
                string ÇalışmaSüresi = abc.Substring(5, 3).Trim();
                string Ekstra = abc.Substring(8).Trim();
               

                int etkilenKayıtSayısı = Labourer(çalışanNo, SaatlikÜcret, ÇalışmaSüresi, Ekstra);

                if (etkilenKayıtSayısı <= 0)
                {
                    MessageBox.Show("Hata oluştu. İşlemleri durduruyorum. Çalışan No" + çalışanNo);
                    break;
                }




            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///

            dosyaYolu = Application.StartupPath + "\\Secretary.txt";



            if (!File.Exists(dosyaYolu))
            {
                MessageBox.Show("Çalışan dosyası bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);  // aktarı pasif hale getirmesi gerek
                return;
            }


             sr = new StreamReader(dosyaYolu, Encoding.GetEncoding("iso-8859-1"));

            while ((abc = sr.ReadLine()) != null)
            {
                string çalışanNo = abc.Substring(0, 3).Trim();//ilgili alanlari aliyoruz
                string KelimeSayısı = abc.Substring(3, 2).Trim();
                string MezuniyetDerecesi = abc.Substring(5, 7).Trim();
                string Yükseklik = abc.Substring(12, 4).Trim();
                string Genişlik = abc.Substring(16, 3).Trim();
                string Maaş = abc.Substring(19).Trim();   // hocaya göster

                int etkilenKayıtSayısı = Secretary(çalışanNo, KelimeSayısı, MezuniyetDerecesi, Yükseklik, Genişlik, Maaş);
                // txt dosyalara göre isimlendirilecek
                if (etkilenKayıtSayısı <= 0)
                {
                    MessageBox.Show("Hata oluştu. İşlemleri durduruyorum. Çalışan No" + çalışanNo);
                    break;
                }




            }



            sr.Close();
          
            aktarToolStripMenuItem.Enabled = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="çalışanNo"></param>
        /// <param name="alanlar"></param>
        /// <param name="cgpa"></param>
        /// <param name="diller"></param>
        /// <param name="maaş"></param>
        /// <returns></returns>
        private int Engineer(string çalışanNo, string alanlar, string cgpa, string diller, string maaş)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cn;
            cmd.CommandText = "INSERT INTO Engineer (ÇalışanNo, Alanlar, Cgp, Diller, Maaş) VALUES (@ÇalışanNo, @Alanlar, @Cgp, @Diller, @Maaş) ";
            cmd.Parameters.AddWithValue("@ÇalışanNo", çalışanNo);
            cmd.Parameters.AddWithValue("@Alanlar", alanlar);
            cmd.Parameters.AddWithValue("@Cgp", cgpa);
            cmd.Parameters.AddWithValue("@Diller", diller);
            cmd.Parameters.AddWithValue("@Maaş", maaş);


            return cmd.ExecuteNonQuery();
        }
/// <summary>
/// 
/// </summary>
/// <param name="çalışanNo"></param>
/// <param name="saatlikÜcret"></param>
/// <param name="çalışmaSüresi"></param>
/// <param name="ekstra"></param>
/// <returns></returns>
        private int Labourer(string çalışanNo, string saatlikÜcret, string çalışmaSüresi, string ekstra)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cn;
            cmd.CommandText = "INSERT INTO Laborer (ÇalışanNo, SaatlikÜcret, ÇalışmaSüresi, Ekstra) VALUES (@ÇalışanNo, @SaatlikÜcret, @ÇalışmaSüresi, @Ekstra) ";
            cmd.Parameters.AddWithValue("@ÇalışanNo", çalışanNo);
            cmd.Parameters.AddWithValue("@SaatlikÜcret", saatlikÜcret);
            cmd.Parameters.AddWithValue("@ÇalışmaSüresi", çalışmaSüresi);
            cmd.Parameters.AddWithValue("@Ekstra", ekstra);



            return cmd.ExecuteNonQuery();
        }
/// <summary>
/// 
/// </summary>
/// <param name="çalışanNo"></param>
/// <param name="kelimeSayısı"></param>
/// <param name="mezuniyetDerecesi"></param>
/// <param name="yükseklik"></param>
/// <param name="genişlik"></param>
/// <param name="maaş"></param>
/// <returns></returns>
/// <exception cref="NotImplementedException"></exception>
        private int Secretary(string çalışanNo, string kelimeSayısı, string mezuniyetDerecesi, string yükseklik, string genişlik, string maaş)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cn;
            cmd.CommandText = "INSERT INTO Secretary (ÇalışanNo, KelimeSayısı, MezuniyetDerecesi, Yükseklik , Genişlik, Maaş) VALUES (@ÇalışanNo, @KelimeSayısı, @MezuniyetDerecesi, @Yükseklik ,@Genişlik, @Maaş) ";
            cmd.Parameters.AddWithValue("@ÇalışanNo", çalışanNo);
            cmd.Parameters.AddWithValue("@KelimeSayısı", kelimeSayısı);
            cmd.Parameters.AddWithValue("@MezuniyetDerecesi", mezuniyetDerecesi);
            cmd.Parameters.AddWithValue("@Yükseklik", yükseklik);
            cmd.Parameters.AddWithValue("@Genişlik", genişlik);
            cmd.Parameters.AddWithValue("maaş", maaş);


            return cmd.ExecuteNonQuery();
        }

        

        



        /// <summary>
        /// Eğer bu fonksiyon geriye 1 döndürürse çalışan veritabanına kaydedilmiş olur. Eğer -1 döndürürse çalışan veritabnına kaydedilmemiş olur
        /// </summary>
        /// <param name="çalışanNo"></param>
        /// <param name="unvan"></param>
        /// <param name="adsoyad"></param>
        /// <param name="dogumTarihi"></param>
        /// <param name="dogumYeri"></param>
        /// <param name="adres"></param>
        /// <returns></returns>
        /// 
        private int ÇalışanKişiyiVeritabanınaKaydet(string çalışanNo, string unvan, string adsoyad, string dogumTarihi, string dogumYeri, string adres)
        {
            
            int yıl = Convert.ToInt32(dogumTarihi.Substring(6, 4));
            int ay = Convert.ToInt32(dogumTarihi.Substring(3, 2));
            int gun = Convert.ToInt32(dogumTarihi.Substring(0, 2));
            DateTime dt = new DateTime(yıl, ay, gun);


            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cn;
            cmd.CommandText = "INSERT INTO Employees (ÇalışanNo, Unvan, Ad, DoğumTarihi, DoğumYeri, Adres) VALUES (@ÇalışanNo, @Unvan, @Ad, @DoğumTarihi, @DoğumYeri, @Adres) ";
            cmd.Parameters.AddWithValue("@ÇalışanNo", çalışanNo);
            cmd.Parameters.AddWithValue("@Unvan", unvan);
            cmd.Parameters.AddWithValue("@Ad", adsoyad);
            cmd.Parameters.AddWithValue("@DoğumTarihi", dt);
            cmd.Parameters.AddWithValue("@DoğumYeri", dogumYeri);
            cmd.Parameters.AddWithValue("@Adres", adres);

            return cmd.ExecuteNonQuery();


        }

        //private string connectionString = "Server=.;Database=CalisanDB;Trusted_Connection=True;";

        

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void onlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Online onlineForm = new Online();
            onlineForm.Show();
        }

        private void aktarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TxtDosyalarınıOkuveVeritabanınaAktar();
        }

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            VeritabanıBağlantısınıKapat();


        }

        private void VeritabanıBağlantısınıKapat()
        {
            try
            {
                if (cn != null && cn.State == ConnectionState.Open)
                    cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
    }
}
