using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BiletRezervasyon
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		SqlConnection baglanti = new SqlConnection(@"Data Source=CEMC\SQLEXPRESS01;Initial Catalog=DbBiletRezervasyon;Integrated Security=True");

		Colors color = new Colors();
		string TBLYolcuKaydetScript = "insert into TblYolcuBilgi (AD,SOYAD,TELEFON,TCKN,CINSIYET,MAIL) values (@p1,@p2,@p3,@p4,@p5,@p6)";
		private void Form1_Load(object sender, EventArgs e)
		{
			this.BackColor = ColorTranslator.FromHtml(color.acikMavi);
			BtnKaydet.BackColor = ColorTranslator.FromHtml(color.acikOrange);
			BtnSeferOlustur.BackColor = ColorTranslator.FromHtml(color.acikOrange);
			BtnKaptan.BackColor = ColorTranslator.FromHtml(color.acikOrange);
			BtnRezervasyonYap.BackColor = ColorTranslator.FromHtml(color.acikOrange);
			seferListele();
		}

		private void BtnKaydet_Click(object sender, EventArgs e)
		{
			baglanti.Open();
			SqlCommand komut = new SqlCommand(TBLYolcuKaydetScript, baglanti);
			komut.Parameters.AddWithValue("p1", TxtAd.Text);
			komut.Parameters.AddWithValue("p2", TxtSoyad.Text);
			komut.Parameters.AddWithValue("p3", TxtTelefon.Text);
			komut.Parameters.AddWithValue("p4", TxtTCKN.Text);
			komut.Parameters.AddWithValue("p6", TxtMail.Text);
			komut.Parameters.AddWithValue("p5", CboxCinsiyet.Text);
			komut.ExecuteNonQuery();
			baglanti.Close();
			MessageBox.Show("Yolcu bilgisi başarı ile kaydedilmiştir.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			ClearAreas();
		}

		private void BtnKaptan_Click(object sender, EventArgs e)
		{
			baglanti.Open();
			SqlCommand komut = new SqlCommand("insert into TblKaptan (KAPTANNO,ADSOYAD,TELEFON) values (@p1,@p2,@p3)", baglanti);
			komut.Parameters.AddWithValue("p1", TxtKaptanNo.Text);
			komut.Parameters.AddWithValue("p2", TxtKaptanAdSoyad.Text);
			komut.Parameters.AddWithValue("p3", MskKaptanTelefon.Text);
			komut.ExecuteNonQuery();
			baglanti.Close();
			MessageBox.Show("Kaptan bilgileri başarı ile kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
			ClearAreas();
		}

		private void BtnSeferOlustur_Click(object sender, EventArgs e)
		{
			baglanti.Open();
			SqlCommand komut = new SqlCommand("insert into TblSeferBilgi (KALKIS,VARIS,TARIH,SAAT,KAPTAN,FİYAT) values (@p1,@p2,@p3,@p4,@p5,@p6)", baglanti);
			komut.Parameters.AddWithValue("p1", TxtKalkis.Text);
			komut.Parameters.AddWithValue("p2", TxtVaris.Text);
			komut.Parameters.AddWithValue("p3", MskTarih.Text);
			komut.Parameters.AddWithValue("p4", MskSaat.Text);
			komut.Parameters.AddWithValue("p5", MskKaptan.Text);
			komut.Parameters.AddWithValue("p6", TxtFiyat.Text);
			komut.ExecuteNonQuery();
			baglanti.Close();
			MessageBox.Show("Sefer bilgisi başarı ile kaydedilmiştir.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			ClearAreas();
			seferListele();
		}

		void seferListele()
		{
			SqlDataAdapter da = new SqlDataAdapter("select * from TblSeferBilgi", baglanti);
			DataTable dt = new DataTable();
			da.Fill(dt);
			dataGridView1.DataSource = dt;
		}

		void ClearAreas()
		{
			TxtAd.Clear();
			TxtSoyad.Clear();
			TxtTelefon.Clear();
			TxtTCKN.Clear();
			TxtMail.Clear();
			CboxCinsiyet.SelectedIndex = -1;
			TxtKaptanAdSoyad.Clear();
			TxtKaptanNo.Clear();
			MskKaptanTelefon.Clear();
			TxtKalkis.Clear();
			TxtVaris.Clear();
			MskTarih.Clear();
			MskSaat.Clear();
			MskKaptan.Clear();
			TxtFiyat.Clear();
			TxtSeferNo2.Clear();
			MskYolcuTCKN.Clear();
			TxtKoltukNo.Clear();
		}
		private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			int secilen = dataGridView1.SelectedCells[0].RowIndex;
			TxtSeferNo2.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
		}

		private void BtnRezervasyonYap_Click(object sender, EventArgs e)
		{
			try
			{
				if (TxtSeferNo2.Text.Equals("") || MskYolcuTCKN.Text.Equals("") || TxtKoltukNo.Text.Equals(""))
				{
					MessageBox.Show("Tüm bilgileri kontrol ederek tekrar giriş yapınız.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				else
				{
					baglanti.Open();
					SqlCommand komut = new SqlCommand("insert into TblSeferDetay (SEFERNO,YOLCUTCKN,KOLTUK) values (@p1,@p2,@p3)", baglanti);
					komut.Parameters.AddWithValue("p1", TxtSeferNo2.Text);
					komut.Parameters.AddWithValue("p2", MskYolcuTCKN.Text);
					komut.Parameters.AddWithValue("p3", TxtKoltukNo.Text);
					komut.ExecuteNonQuery();
					baglanti.Close();
					MessageBox.Show("Rezervasyon başarı ile kaydedilmiştir.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					ClearAreas();
				}
			}
			catch (Exception)
			{

				MessageBox.Show("Tüm bilgileri kontrol ederek tekrar giriş yapınız.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

		}

		private void Btn1_Click(object sender, EventArgs e)
		{
			TxtKoltukNo.Text = "1";
		}

		private void Btn2_Click(object sender, EventArgs e)
		{
			TxtKoltukNo.Text = "2";
		}

		private void Btn3_Click(object sender, EventArgs e)
		{
			TxtKoltukNo.Text = "3";
		}

		private void Btn4_Click(object sender, EventArgs e)
		{
			TxtKoltukNo.Text = "4";
		}

		private void Btn5_Click(object sender, EventArgs e)
		{
			TxtKoltukNo.Text = "5";
		}

		private void Btn6_Click(object sender, EventArgs e)
		{
			TxtKoltukNo.Text = "6";
		}

		private void Btn7_Click(object sender, EventArgs e)
		{
			TxtKoltukNo.Text = "7";
		}

		private void Btn8_Click(object sender, EventArgs e)
		{
			TxtKoltukNo.Text = "8";
		}

		private void Btn9_Click(object sender, EventArgs e)
		{
			TxtKoltukNo.Text = "9";
		}

	}
}
