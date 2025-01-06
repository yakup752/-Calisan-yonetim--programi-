using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Gorsel_programlama_Proje_odevi
{
    public partial class Online : Form
    {
        //SqlConnection cn;
        SqlConnection cn = new SqlConnection(Properties.Settings.Default.Calisanaktar);

        public Online()
        {
            InitializeComponent();

        }

        public Online(SqlConnection cn)
        {
            InitializeComponent();
        }



        private void Online_Load(object sender, EventArgs e)
        {

            btnYeniKayit.Enabled = true;
            btnAnaMenu.Enabled = true;

            btnEkle.Enabled = false;
            btnGuncelle.Enabled = false;
            btnSil.Enabled = false;


            string secilmişİsimAralığı = Calisantabcontrol.SelectedTab.Text;
            KayıtlarıGetir(secilmişİsimAralığı);

        }

        private void KayıtlarıGetir(string secilmişİsimAralığı)
        {

            if (cn.State != ConnectionState.Open)
            {
                cn.Open();
            }

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();

            if (secilmişİsimAralığı == "Tüm Kayıtlar") 
            {
                adapter = new SqlDataAdapter("SELECT * FROM Employees ORDER BY Ad", cn);
            }
            else if (secilmişİsimAralığı.Length == 1) 
            {
                adapter = new SqlDataAdapter("SELECT * FROM Employees WHERE Ad LIKE @Ad  ORDER BY Ad", cn);
                adapter.SelectCommand.Parameters.AddWithValue("@Ad", secilmişİsimAralığı + "%");
            }
            else if (secilmişİsimAralığı.Contains("-")) 
            {
                char başlangıç = secilmişİsimAralığı[0]; 
                char bitiş = secilmişİsimAralığı[2];    
                adapter = new SqlDataAdapter("SELECT * FROM Employees WHERE Ad >= @Başlangıç AND Ad <= @Bitiş  ORDER BY Ad", cn);
                adapter.SelectCommand.Parameters.AddWithValue("@Başlangıç", başlangıç.ToString());
                adapter.SelectCommand.Parameters.AddWithValue("@Bitiş", bitiş.ToString() + "z"); 
            }
            else 
            {
                adapter = new SqlDataAdapter("SELECT * FROM Employees", cn);
            }

            adapter.Fill(dataTable);
            dataGridViewCalisanlar.DataSource = dataTable;



        }




        private void btnAnaMenu_Click(object sender, EventArgs e)
        {

            Anaform anaform = new Anaform();
            this.Close();


            //// Ana form zaten açıksa onu göster
            //foreach (Form form in Application.OpenForms)
            //{
            //    if (form is Anaform)
            //    {
            //        form.Show();
            //        this.Close();
            //        return;
            //    }
            //}
        }

      
        private void dataGridViewCalisanlar_DoubleClick(object sender, EventArgs e)
        {

            DataGridViewRow row = dataGridViewCalisanlar.CurrentRow;
            string çalışanTipi = dataGridViewCalisanlar.CurrentRow.Cells["Unvan"].Value.ToString();
            string çalışanNo = row.Cells["ÇalışanNo"].Value.ToString();

            cbxtxtUnvan.Text = row.Cells["Unvan"].Value.ToString();
            switch (çalışanTipi)
            {
                case "E":
                    tabControlunvan.SelectedTab = tabPagemühendis;

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Engineer WHERE ÇalışanNo=@ÇalışanNo", cn);
                    cmd.Parameters.AddWithValue("@ÇalışanNo", çalışanNo);

                    SqlDataReader read = cmd.ExecuteReader();

                    if (read != null)
                    {
                        while (read.Read())
                        {

                            string alanlar = read["Alanlar"].ToString();
                            string cpga = read["Cgp"].ToString();
                            string butunDiller = read["Diller"].ToString();
                            string maaş = read["Maaş"].ToString();

                            mühendisalancombobox.Text = alanlar;
                            txtMezuniyetDerecesi.Text = cpga;
                            txtMuhendisMaas.Text = maaş;

                            string[] diller = butunDiller.Split(' ');

                            foreach (string dil in diller)
                            {
                                for (int i = 0; i < cblMuhendisDiller.Items.Count; i++)
                                {
                                    if (cblMuhendisDiller.Items[i].ToString() == dil)
                                        cblMuhendisDiller.SetItemChecked(i, true);
                                }
                            }

                        }
                       
                        read.Close();

                    }
                    else if (cn.State == System.Data.ConnectionState.Closed)
                        cn.Open();
                    break;

                case "S":
                    tabControlunvan.SelectedTab = tabPagesekreter;

                    cmd = new SqlCommand("SELECT * FROM Secretary WHERE ÇalışanNo=@ÇalışanNo", cn);
                    cmd.Parameters.AddWithValue("@ÇalışanNo", çalışanNo);

                    read = cmd.ExecuteReader();

                    if (read != null)
                    {
                        while (read.Read())
                        {

                            string Mezuniyetderecesi = read["MezuniyetDerecesi"].ToString();
                            string kelimesayısı = read["KelimeSayısı"].ToString();
                            string Yükseklik = read["Yükseklik"].ToString();
                            string sekretermaas = read["Maaş"].ToString();
                            string Genislik = read["Genişlik"].ToString();

                            Mezuniyetderecesicombobox.SelectedText = Mezuniyetderecesi;
                            textboxkelimesayısı.Text = kelimesayısı;
                            textBoxyükseklik.Text = Yükseklik;
                            sekretertextBoxmaas.Text = sekretermaas;
                            textBoxgenişlik.Text = Genislik;



                        }

                        read.Close();
                    }
                    else if (cn.State == System.Data.ConnectionState.Closed)
                        cn.Open();
                    break;

                case "L":
                    tabControlunvan.SelectedTab = tabPagelaborant;
                    cmd = new SqlCommand("SELECT * FROM Laborer WHERE ÇalışanNo=@ÇalışanNo", cn);
                    cmd.Parameters.AddWithValue("@ÇalışanNo", çalışanNo);

                    read = cmd.ExecuteReader();

                    if (read != null)
                    {
                        while (read.Read())
                        {

                            string saatlikücret = read["SaatlikÜcret"].ToString();
                            string calismasuresi = read["ÇalışmaSüresi"].ToString();
                            string ekstra = read["Ekstra"].ToString();
                            //string labmaas = read["Maaş"].ToString();


                            txtsaatlikucret.Text = saatlikücret;
                            txtcalismasuresi.Text = calismasuresi;
                            txtekstra.Text = ekstra;
                            //sekretertextBoxmaas.Text = labmaas;

                            // Maaşı dinamik olarak hesaplıyoruz
                            if (decimal.TryParse(saatlikücret, out decimal saatlikUcret) &&
                                decimal.TryParse(calismasuresi, out decimal calismaSuresi) &&
                                decimal.TryParse(ekstra, out decimal Ekstra2))
                            {
                                decimal maas = (saatlikUcret * calismaSuresi) + (saatlikUcret * 2 * Ekstra2);
                                txtmaas.Text = maas.ToString("C2"); // Maaşı para birimi formatında göster
                            }
                            else
                            {
                                txtmaas.Text = "Hatalı veri"; // Eğer hesaplama yapılamazsa, kullanıcıya bilgi ver
                            }


                        }

                        read.Close();
                    }


                    


                    break;

                default:
                    MessageBox.Show("Belirtilen tip için uygun bir sekme bulunamadı.");
                    break;
            }

            btnGuncelle.Enabled = true;
            btnSil.Enabled = true;

            calisannumaratxt.ReadOnly = true;





            calisannumaratxt.Text = çalışanNo;
            Adsoyadtxt.Text = row.Cells["Ad"].Value.ToString();
            dtpDogumtarihi.Value = Convert.ToDateTime(row.Cells["DoğumTarihi"].Value);
            dogumyeritxt.Text = row.Cells["DoğumYeri"].Value.ToString();
            adrestxt.Text = row.Cells["Adres"].Value.ToString();
        }

      

        private void btnYeniKayit_Click(object sender, EventArgs e)
        {
            ////ANA BİLGİLER KISMINI TEMİZLER
            //Adsoyadtxt.Text = "";
            ////dtpDogumtarihi.Text = "";
            // dtpDogumtarihi.Value = DateTime.Now;
            //dogumyeritxt.Text = "";
            //adrestxt.Text = "";
            //calisannumaratxt.Text = "";

            
            if (tabControlunvan.SelectedTab == tabPagemühendis)
            {
                Mezuniyetderecesicombobox.Text = string.Empty;
                txtMuhendisMaas.Text = string.Empty;
                cblMuhendisDiller.Text = string.Empty;
                mühendisalancombobox.Text = string.Empty;
            }
            else if (tabControlunvan.SelectedTab == tabPagelaborant)
            {
                txtsaatlikucret.Text = string.Empty;
                txtcalismasuresi.Text = string.Empty;
                txtekstra.Text = string.Empty;
                txtmaas.Text = string.Empty;
            }
            else if (tabControlunvan.SelectedTab == tabPagesekreter)
            {
                textboxkelimesayısı.Text = string.Empty;
                Mezuniyetderecesicombobox.Text = string.Empty;
                textBoxyükseklik.Text = string.Empty;
                textBoxgenişlik.Text = string.Empty;
                sekretertextBoxmaas.Text = string.Empty;
            }


            btnEkle.Enabled = true;    
            btnGuncelle.Enabled = false; 
            btnSil.Enabled = false;    
            btnYeniKayit.Enabled = false; 

            calisannumaratxt.ReadOnly = false;

            cbxtxtUnvan.Visible= true;
            
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {


            // Bağlantıyı açma kontrolü
            if (cn.State == System.Data.ConnectionState.Closed)
                cn.Open();

            SqlTransaction trans = cn.BeginTransaction();
            try
            {
                string çalışanNo = calisannumaratxt.Text;
                string ad = Adsoyadtxt.Text;
                DateTime doğumTarihi = dtpDogumtarihi.Value;
                string doğumYeri = dogumyeritxt.Text;
                string adres = adrestxt.Text;
                string unvan = cbxtxtUnvan.Text;

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.Transaction = trans;

                if (unvan == "E")
                {
                    string seçilenDiller = String.Empty;
                    for (int i = 0; i < cblMuhendisDiller.Items.Count; i++)
                    {
                        if (cblMuhendisDiller.GetItemChecked(i) == true)
                        {
                            seçilenDiller += cblMuhendisDiller.Items[i].ToString() + " ";
                        }
                    }

                    if (seçilenDiller.Length > 0)
                        seçilenDiller = seçilenDiller.Remove(seçilenDiller.Length - 1);

                    cmd.CommandText = @"
            UPDATE Engineer 
            SET Alanlar = @Alanlar, 
                Cgp = @Cgp, 
                Diller = @Diller, 
                Maaş = @Maaş 
            WHERE ÇalışanNo = @ÇalışanNo";

                    cmd.Parameters.AddWithValue("@ÇalışanNo", çalışanNo);
                    cmd.Parameters.AddWithValue("@Alanlar", mühendisalancombobox.Text);
                    cmd.Parameters.AddWithValue("@Cgp", txtMezuniyetDerecesi.Text.Trim());
                    cmd.Parameters.AddWithValue("@Diller", seçilenDiller);
                    cmd.Parameters.AddWithValue("@Maaş", txtMuhendisMaas.Text.Trim());

                    cmd.ExecuteNonQuery();
                }
                else if (unvan == "L")
                {
                    cmd.CommandText = @"
            UPDATE Laborer 
            SET SaatlikÜcret = @SaatlikÜcret, 
                ÇalışmaSüresi = @ÇalışmaSüresi, 
                Ekstra = @Ekstra  
            WHERE ÇalışanNo = @ÇalışanNo";

                    cmd.Parameters.AddWithValue("@ÇalışanNo", çalışanNo);
                    cmd.Parameters.AddWithValue("@SaatlikÜcret", txtsaatlikucret.Text);
                    cmd.Parameters.AddWithValue("@ÇalışmaSüresi", txtcalismasuresi.Text.Trim());
                    cmd.Parameters.AddWithValue("@Ekstra", txtekstra.Text);

                    cmd.ExecuteNonQuery();
                }
                else if (unvan == "S")
                {
                    cmd.CommandText = @"
            UPDATE Secretary
            SET KelimeSayısı = @KelimeSayısı, 
                MezuniyetDerecesi = @MezuniyetDerecesi, 
                Yükseklik = @Yükseklik,
                Genişlik = @Genişlik,
                Maaş = @Maaş
            WHERE ÇalışanNo = @ÇalışanNo";

                    cmd.Parameters.AddWithValue("@ÇalışanNo", çalışanNo);
                    cmd.Parameters.AddWithValue("@KelimeSayısı", textboxkelimesayısı.Text);
                    cmd.Parameters.AddWithValue("@MezuniyetDerecesi", Mezuniyetderecesicombobox.Text);
                    cmd.Parameters.AddWithValue("@Yükseklik", textBoxyükseklik.Text);
                    cmd.Parameters.AddWithValue("@Genişlik", textBoxgenişlik.Text);
                    cmd.Parameters.AddWithValue("@Maaş", sekretertextBoxmaas.Text);

                    cmd.ExecuteNonQuery();
                }

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.Transaction = trans;
                cmd.CommandText = @"
        UPDATE Employees 
        SET Ad = @Ad, 
            DoğumTarihi = @DoğumTarihi, 
            DoğumYeri = @DoğumYeri, 
            Adres = @Adres 
        WHERE ÇalışanNo = @ÇalışanNo";

                cmd.Parameters.AddWithValue("@ÇalışanNo", çalışanNo);
                cmd.Parameters.AddWithValue("@Ad", ad);
                cmd.Parameters.AddWithValue("@DoğumTarihi", doğumTarihi.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@DoğumYeri", doğumYeri);
                cmd.Parameters.AddWithValue("@Adres", adres);

                int rowsAffected = cmd.ExecuteNonQuery();
                trans.Commit();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Kayıt başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    string secilmişİsimAralığı = Calisantabcontrol.SelectedTab.Text;
                    KayıtlarıGetir(secilmişİsimAralığı);
                }
                else
                {
                    MessageBox.Show("Güncelleme sırasında bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           





            //SqlTransaction trans = cn.BeginTransaction();
            //try
            //{


            //    string çalışanNo = calisannumaratxt.Text;
            //    string ad = Adsoyadtxt.Text;
            //    DateTime doğumTarihi = dtpDogumtarihi.Value;
            //    string doğumYeri = dogumyeritxt.Text;
            //    string adres = adrestxt.Text;
            //    string unvan = cbxtxtUnvan.Text;



            //    SqlCommand cmd = new SqlCommand();
            //    cmd.Connection = cn;
            //    cmd.Transaction = trans;

            //    if (unvan == "E")
            //    {
            //        string seçilenDiller = String.Empty;
            //        for (int i = 0; i < cblMuhendisDiller.Items.Count; i++)
            //        {
            //            if (cblMuhendisDiller.GetItemChecked(i) == true)
            //            {
            //                seçilenDiller += cblMuhendisDiller.Items[i].ToString() + " ";
            //            }

            //        }

            //        if (seçilenDiller.Length > 0)
            //            seçilenDiller = seçilenDiller.Remove(seçilenDiller.Length - 1);


            //        cmd.CommandText = @"
            //                 UPDATE Engineer 
            //                 SET 

            //                     Alanlar = @Alanlar, 
            //                     Cgp = @Cgp, 
            //                     Diller = @Diller, 
            //                     Maaş = @Maaş 
            //                 WHERE ÇalışanNo = @ÇalışanNo";

            //        cmd.Parameters.AddWithValue("@ÇalışanNo", çalışanNo);
            //        cmd.Parameters.AddWithValue("@Alanlar", mühendisalancombobox.Text);
            //        cmd.Parameters.AddWithValue("@Cgp", txtMezuniyetDerecesi.Text.Trim());
            //        cmd.Parameters.AddWithValue("@Diller", seçilenDiller);
            //        cmd.Parameters.AddWithValue("@Maaş", txtMuhendisMaas.Text.Trim());

            //        cmd.ExecuteNonQuery();
            //    }
            //    else if (unvan == "L")
            //    {
            //        cmd.CommandText = @"
            //                 UPDATE Laborer 
            //                 SET 

            //                     SaatlikÜcret = @SaatlikÜcret, 
            //                     ÇalışmaSüresi = @ÇalışmaSüresi, 
            //                     Ekstra = @Ekstra  
            //                 WHERE ÇalışanNo = @ÇalışanNo";

            //        cmd.Parameters.AddWithValue("@ÇalışanNo", çalışanNo);
            //        cmd.Parameters.AddWithValue("@SaatlikÜcret", txtsaatlikucret.Text);
            //        cmd.Parameters.AddWithValue("@ÇalışmaSüresi", txtcalismasuresi.Text.Trim());
            //        cmd.Parameters.AddWithValue("@Ekstra", txtekstra.Text);


            //        cmd.ExecuteNonQuery();




            //    }
            //    else if (unvan == "S")
            //    {

            //        cmd.CommandText = @"
            //                 UPDATE Secretary
            //                 SET 

            //                     KelimeSayısı = @KelimeSayısı, 
            //                     MezuniyetDerecesi = @MezuniyetDerecesi, 
            //                     Yükseklik = @Yükseklik,
            //                      Genişlik = @Genişlik,
            //                      Maaş = @Maaş
            //                 WHERE ÇalışanNo = @ÇalışanNo";

            //        cmd.Parameters.AddWithValue("@ÇalışanNo", çalışanNo);
            //        cmd.Parameters.AddWithValue("@KelimeSayısı", textboxkelimesayısı.Text);
            //        cmd.Parameters.AddWithValue("@MezuniyetDerecesi", Mezuniyetderecesicombobox.Text);
            //        cmd.Parameters.AddWithValue("@Yükseklik", textBoxyükseklik.Text);
            //        cmd.Parameters.AddWithValue("@Genişlik", textBoxgenişlik.Text);
            //        cmd.Parameters.AddWithValue("@Maaş", sekretertextBoxmaas.Text);


            //        cmd.ExecuteNonQuery();

            //    }




            //    cmd = new SqlCommand();
            //    cmd.Connection = cn;
            //    cmd.Transaction = trans;
            //    cmd.CommandText = @"UPDATE Employees 
            //             SET 

            //                 Ad = @Ad, 
            //                 DoğumTarihi = @DoğumTarihi, 
            //                 DoğumYeri = @DoğumYeri, 
            //                 Adres = @Adres 
            //             WHERE ÇalışanNo = @ÇalışanNo";


            //    cmd.Parameters.AddWithValue("@ÇalışanNo", çalışanNo);
            //    cmd.Parameters.AddWithValue("@Ad", ad);
            //    cmd.Parameters.AddWithValue("@DoğumTarihi", doğumTarihi.ToString("yyyy-MM-dd"));
            //    cmd.Parameters.AddWithValue("@DoğumYeri", doğumYeri);
            //    cmd.Parameters.AddWithValue("@Adres", adres);


            //    int rowsAffected = cmd.ExecuteNonQuery();

            //    trans.Commit();
            //    if (rowsAffected > 0)
            //    {
            //        MessageBox.Show("Kayıt başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //        string secilmişİsimAralığı = Calisantabcontrol.SelectedTab.Text;
            //        KayıtlarıGetir(secilmişİsimAralığı);
            //    }
            //    else
            //    {
            //        MessageBox.Show("Güncelleme sırasında bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }

            //}
            //catch (Exception ex)
            //{
            //    trans.Rollback();
            //    MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}




        }

        private void VerileriYenile()
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Employees", cn))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                dataGridViewCalisanlar.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veriler yenilenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtsaatlikucret_TextChanged(object sender, EventArgs e)
        {
            //HesaplaMaaş();
        }

        private void txtcalismasuresi_TextChanged(object sender, EventArgs e)
        {
            //HesaplaMaaş();
        }

        private void txtekstra_TextChanged(object sender, EventArgs e)
        {
            //HesaplaMaaş();
        }

        private void HesaplaMaaş()
        {

            float saatlikUcret, calismaSuresi, ekstra;


            bool saatlikUcretGecerli = float.TryParse(txtsaatlikucret.Text, out saatlikUcret);
            bool calismaSuresiGecerli = float.TryParse(txtcalismasuresi.Text, out calismaSuresi);
            bool ekstraGecerli = float.TryParse(txtekstra.Text, out ekstra);


            if (saatlikUcretGecerli && calismaSuresiGecerli && ekstraGecerli)
            {
                float maas = (saatlikUcret * calismaSuresi) + (saatlikUcret * 2 * ekstra);
                txtmaas.Text = maas.ToString("C2");
            }
            else
            {

                txtmaas.Clear();
                MessageBox.Show("Lütfen tüm alanları geçerli sayılarla doldurun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Calisantabcontrol_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string secilmişİsimAralığı = Calisantabcontrol.SelectedTab.Text;
            KayıtlarıGetir(secilmişİsimAralığı);

        }

    
        private void btnEkle_Click(object sender, EventArgs e)
        {

            SqlTransaction trans = cn.BeginTransaction();

            try
            {
                if (cn.State == System.Data.ConnectionState.Closed)
                    cn.Open();

                System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("tr-TR");
                DateTime dogumTarihi = DateTime.Parse(dtpDogumtarihi.Text, cultureinfo);

                string unvan = "";
                if (tabControlunvan.SelectedTab == tabPagemühendis)
                    unvan = "E";
                else if (tabControlunvan.SelectedTab == tabPagelaborant)
                    unvan = "L";
                else if (tabControlunvan.SelectedTab == tabPagesekreter)
                    unvan = "S";

                string query = "INSERT INTO Employees (Ad, Unvan, DoğumTarihi, DoğumYeri, Adres, ÇalışanNo) " +
                               "VALUES (@Ad, @Unvan, @DoğumTarihi, @DoğumYeri, @Adres, @ÇalışanNo)";

                SqlCommand cmd = new SqlCommand(query, cn, trans);

                cmd.Parameters.AddWithValue("@Ad", Adsoyadtxt.Text);
                cmd.Parameters.AddWithValue("@Unvan", unvan);
                cmd.Parameters.AddWithValue("@DoğumTarihi", dogumTarihi);
                cmd.Parameters.AddWithValue("@DoğumYeri", dogumyeritxt.Text);
                cmd.Parameters.AddWithValue("@Adres", adrestxt.Text);
                cmd.Parameters.AddWithValue("@ÇalışanNo", calisannumaratxt.Text);

                cmd.ExecuteNonQuery();

                if (tabControlunvan.SelectedTab == tabPagemühendis)
                {
                    query = "INSERT INTO Engineer (Alanlar, Cgp, Diller, Maaş, ÇalışanNo) " +
                            "VALUES (@Alanlar, @Cgp, @Diller, @Maaş, @ÇalışanNo)";

                    cmd = new SqlCommand(query, cn, trans);

                    cmd.Parameters.AddWithValue("@Alanlar", mühendisalancombobox.Text);
                    cmd.Parameters.AddWithValue("@Cgp", txtMezuniyetDerecesi.Text);
                    cmd.Parameters.AddWithValue("@Diller", cblMuhendisDiller.Text);
                    cmd.Parameters.AddWithValue("@Maaş", txtMuhendisMaas.Text);
                    cmd.Parameters.AddWithValue("@ÇalışanNo", calisannumaratxt.Text);

                    cmd.ExecuteNonQuery();
                }
                else if (tabControlunvan.SelectedTab == tabPagelaborant)
                {
                    query = "INSERT INTO Labourer (SaatlikÜcret, ÇalışmaSüresi, Ekstra, ÇalışanNo) " +
                            "VALUES (@SaatlikÜcret, @ÇalışmaSüresi, @Ekstra, @ÇalışanNo)";

                    cmd = new SqlCommand(query, cn, trans);

                    cmd.Parameters.AddWithValue("@SaatlikÜcret", txtsaatlikucret.Text);
                    cmd.Parameters.AddWithValue("@ÇalışmaSüresi", txtcalismasuresi.Text);
                    cmd.Parameters.AddWithValue("@Ekstra", txtekstra.Text);
                    cmd.Parameters.AddWithValue("@ÇalışanNo", calisannumaratxt.Text);

                    cmd.ExecuteNonQuery();
                }
                else if (tabControlunvan.SelectedTab == tabPagesekreter)
                {
                    query = "INSERT INTO Secretary (KelimeSayısı, MezuniyetDerecesi, Yükseklik, Genişlik, Maaş, ÇalışanNo) " +
                            "VALUES (@KelimeSayısı, @MezuniyetDerecesi, @Yükseklik, @Genişlik, @Maaş, @ÇalışanNo)";

                    cmd = new SqlCommand(query, cn, trans);

                    cmd.Parameters.AddWithValue("@KelimeSayısı", textboxkelimesayısı.Text);
                    cmd.Parameters.AddWithValue("@MezuniyetDerecesi", txtMezuniyetDerecesi.Text);
                    cmd.Parameters.AddWithValue("@Yükseklik", textBoxyükseklik.Text);
                    cmd.Parameters.AddWithValue("@Genişlik", textBoxgenişlik.Text);
                    cmd.Parameters.AddWithValue("@Maaş", sekretertextBoxmaas.Text);
                    cmd.Parameters.AddWithValue("@ÇalışanNo", calisannumaratxt.Text);

                    cmd.ExecuteNonQuery();
                }

                trans.Commit();

                // Kayıtları tekrar getir
                KayıtlarıGetir(Calisantabcontrol.SelectedTab.Text);

                btnYeniKayit.Enabled = true;
                btnEkle.Enabled = false;

                MessageBox.Show("Yeni kayıt başarıyla eklendi!");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                MessageBox.Show($"Bir hata oluştu: {ex.Message}");
            }
          




        }



        private void txtmaas_TextChanged(object sender, EventArgs e)
        {
            HesaplaMaaş();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dataGridViewCalisanlar.SelectedRows.Count > 0) // Seçili satır var mı kontrol et
            {
                DialogResult result = MessageBox.Show("Seçilen veriyi silmek istediğinize emin misiniz?",
                                                       "Silme Onayı",
                                                       MessageBoxButtons.YesNo,
                                                       MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // Veritabanı bağlantısını aç
                        if (cn.State == System.Data.ConnectionState.Closed)
                            cn.Open();

                        SqlTransaction trans = cn.BeginTransaction();

                        // Seçili satırın ÇalışanNo değerini al
                        string çalışanNo = dataGridViewCalisanlar.SelectedRows[0].Cells["ÇalışanNo"].Value.ToString();

                        // Silme sorgusu
                        string query = "DELETE FROM Employees WHERE ÇalışanNo = @ÇalışanNo; " +
                                       "DELETE FROM Engineer WHERE ÇalışanNo = @ÇalışanNo; " +
                                       "DELETE FROM Laborer WHERE ÇalışanNo = @ÇalışanNo; " +
                                       "DELETE FROM Secretary WHERE ÇalışanNo = @ÇalışanNo;";

                        SqlCommand cmd = new SqlCommand(query, cn, trans);
                        cmd.Parameters.AddWithValue("@ÇalışanNo", çalışanNo);

                        // Sorguyu çalıştır
                        cmd.ExecuteNonQuery();

                        trans.Commit();

                        // DataGridView'i güncelle
                        KayıtlarıGetir(Calisantabcontrol.SelectedTab.Text);

                        MessageBox.Show("Kayıt başarıyla silindi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                   
                }
            }
            else
            {
                MessageBox.Show("Silmek için bir satır seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
