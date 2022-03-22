using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GsmBayiOtomasyon
{
    public partial class Form1 : Form
    {
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-HF0EKVJ;Initial Catalog=gsmOtomasyonDB;Integrated Security=True");
        SqlCommand komut = new SqlCommand();

        private bool btnTelefonGosterClicked = false;
        private bool btnMusteriGosterClicked = false;

        public Form1()
        {
            InitializeComponent();
        }

        int secilenSatisID;
        int idnumarasi;
        private void dgv2_Click(object sender, DataGridViewCellEventArgs e)
        {
            idnumarasi = Convert.ToInt32(dataGridView2.CurrentRow.Cells[0].Value); //Tablodan seçilen itemin id no sunu alma
            label10.Text = "Seçilen Yerin ID'si: " + idnumarasi;
            label11.Text = "Seçilen Müşteri ID: " + idnumarasi;

            Button clickedButton = sender as Button;
            if (btnMusteriGosterClicked)
            {
                textBox7.Text = Convert.ToInt32(dataGridView2.CurrentRow.Cells[0].Value).ToString();
            }
            if (btnTelefonGosterClicked)
            {
                textBox5.Text = Convert.ToInt32(dataGridView2.CurrentRow.Cells[0].Value).ToString();
            }
            
        }

        private void dgv1_click(object sender, DataGridViewCellEventArgs e)
        {
            secilenSatisID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value); //Tablodan seçilen itemin id no sunu alma
            label12.Text = "Seçilen Satış ID: " + secilenSatisID;
        }

        private void btnMusteriEkle_Click(object sender, EventArgs e)
        {
            komut.CommandText = "musteriEkle";
            komut.CommandType = CommandType.StoredProcedure;
            komut.Parameters.AddWithValue("@musteriAdiSoyadi", textBox1.Text);
            komut.Parameters.AddWithValue("@musteriTelefon", textBox2.Text);
            komut.Parameters.AddWithValue("@MusteriAdres", textBox3.Text);
            komut.Parameters.AddWithValue("@MusteriMail", textBox4.Text);

            komut.Connection = baglanti;
            baglanti.Open();
            komut.ExecuteNonQuery();
            komut.Parameters.Clear();
            komut.CommandType = CommandType.Text;
            baglanti.Close();

            MusterileriGoster();
        }

        private void btnMusteriGoster_Click(object sender, EventArgs e)
        {
            MusterileriGoster();
            btnMusteriGosterClicked = true;
            btnTelefonGosterClicked = false;
        }

        private void MusterileriGoster()
        {
            komut.CommandText = "select * from Musteri";

            komut.Connection = baglanti;
            DataTable dt = new DataTable();
            baglanti.Open();
            dt.Load(komut.ExecuteReader());
            dataGridView2.DataSource = dt;
            baglanti.Close();
            komut.CommandType = CommandType.Text;
            komut.Parameters.Clear();
            komut.Dispose();
            dt.Dispose();
        }

        private void btnMusteriSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (idnumarasi!=0)
                {
                    komut.CommandText = "musteriSil";
                    komut.Parameters.Add("@musteriNo", SqlDbType.Int);
                    komut.Parameters["@musteriNo"].Value = idnumarasi;
                    komut.CommandType = CommandType.StoredProcedure;

                    DataTable dt = new DataTable();
                    baglanti.Open();
                    dt.Load(komut.ExecuteReader());
                    dataGridView2.DataSource = dt;

                    komut.Connection = baglanti;
                    baglanti.Close();
                    komut.CommandType = CommandType.Text;
                    komut.Parameters.Clear();
                    komut.Dispose();
                    dt.Dispose();

                    MusterileriGoster();
                }
                else
                {
                    MessageBox.Show("Lütfen Müşteriyi Tekrar Seçiniz.");
                }
                
            }
            catch (Exception)
            {
                MessageBox.Show("Beklenmeyen bir hata oluştu");
            }
            
        }

        private void btnTelefonGoster_Click(object sender, EventArgs e)
        {
            TelefonlariGoster();
            btnTelefonGosterClicked = true;
            btnMusteriGosterClicked = false;
        }

        private void TelefonlariGoster()
        {
            komut.CommandText = "telefonlariGetir";
            komut.CommandType = CommandType.StoredProcedure;

            komut.Connection = baglanti;
            DataTable dt = new DataTable();
            baglanti.Open();
            dt.Load(komut.ExecuteReader());
            dataGridView2.DataSource = dt;
            baglanti.Close();
            komut.CommandType = CommandType.Text;
            komut.Parameters.Clear();
            komut.Dispose();
            dt.Dispose();
        }

        private void btnTelefonSil_Click(object sender, EventArgs e)
        {
            if (idnumarasi!=0)
            {
                
            }
            else
            {
                MessageBox.Show("Lütfen Telefonu Tekrar Seçiniz.");
            }
            
        }

        private void btnSatisYap_Click(object sender, EventArgs e)
        {
            try
            {
                komut.CommandText = "satisYap";
                komut.CommandType = CommandType.StoredProcedure;
                komut.Parameters.AddWithValue("@musteriNo", textBox7.Text);
                komut.Parameters.AddWithValue("@urunID", textBox5.Text);
                komut.Parameters.AddWithValue("@fiyat", textBox6.Text);

                komut.Connection = baglanti;
                baglanti.Open();
                komut.ExecuteNonQuery();
                komut.Parameters.Clear();
                komut.CommandType = CommandType.Text;
                baglanti.Close();

                stokHesapla();

                yapilanSatislar();

                satisMiktariGoster();
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.Message+"Satış yaparken bir hata oluştu");
            }
            
        }

        private void IDyapilanSatislar_Click(object sender, EventArgs e)
        {
            IDyapilanSatisGoster();
        }
        private void IDyapilanSatisGoster()
        {
            if (idnumarasi!=0)
            {
                komut.CommandText = "yapilanSatislar";
                komut.Parameters.Add("@musteriNo", SqlDbType.Int);
                komut.Parameters["@musteriNo"].Value = idnumarasi;
                komut.CommandType = CommandType.StoredProcedure;

                komut.Connection = baglanti;
                DataTable dt = new DataTable();
                baglanti.Open();
                dt.Load(komut.ExecuteReader());
                dataGridView1.DataSource = dt;

                baglanti.Close();
                komut.CommandType = CommandType.Text;
                komut.Parameters.Clear();
                komut.Dispose();
                dt.Dispose();
            }
            else
            {
                MessageBox.Show("Lütfen Bir Müşteri Seçiniz.");
            }
            
        }

        private void btnYapilanSatislar_Click(object sender, EventArgs e)
        {
            yapilanSatislar();
        }

        private void yapilanSatislar()
        {
            komut.CommandText = "SatislariGoster";
            komut.Connection = baglanti;
            DataTable dt = new DataTable();
            baglanti.Open();
            dt.Load(komut.ExecuteReader());
            dataGridView1.DataSource = dt;
            baglanti.Close();
            komut.CommandType = CommandType.Text;
            komut.Parameters.Clear();
            komut.Dispose();
            dt.Dispose();
        }

        private void stokHesapla()  //function kullanarak stok miktarı hesaplama
        {
            baglanti.Open();
            komut = new SqlCommand("select dbo.stokSayisi()", baglanti);
            komut.CommandType = CommandType.Text;
            label9.Text = "Toplam Stok Miktarı: " + komut.ExecuteScalar().ToString();
            baglanti.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            stokHesapla();
            satisMiktariGoster();
        }

        private void btnIadeYap_Click(object sender, EventArgs e)
        {
            komut.CommandText = "iadeYap";
            komut.Parameters.Add("@satisID", SqlDbType.Int);
            komut.Parameters["@satisID"].Value = secilenSatisID;
            komut.CommandType = CommandType.StoredProcedure;

            komut.Connection = baglanti;
            DataTable dt = new DataTable();
            baglanti.Open();
            dt.Load(komut.ExecuteReader());
            dataGridView1.DataSource = dt;
            baglanti.Close();
            komut.CommandType = CommandType.Text;
            komut.Parameters.Clear();
            komut.Dispose();
            dt.Dispose();

            stokHesapla();

            yapilanSatislar();

            satisMiktariGoster();
        }

        private void btnIadeGoster_Click(object sender, EventArgs e)
        {
            komut.CommandText = "select * from iadeler";
            komut.Connection = baglanti;
            DataTable dt = new DataTable();
            baglanti.Open();
            dt.Load(komut.ExecuteReader());
            dataGridView1.DataSource = dt;
            baglanti.Close();
            komut.CommandType = CommandType.Text;
            komut.Parameters.Clear();
            komut.Dispose();
            dt.Dispose();
        }

        private void satisMiktariGoster()   //function kullanarak Toplam satis Miktarını hesaplama
        {
            baglanti.Open();
            komut = new SqlCommand("select dbo.SatisMiktari()", baglanti);
            komut.CommandType = CommandType.Text;
            label13.Text = "Toplam Satış: " + komut.ExecuteScalar().ToString();
            baglanti.Close();
        }
    }
}
