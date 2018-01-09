using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Y28XML_JSON_SerializeDeserialize
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<Kisi> kisiler = new List<Kisi>();

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbCinsiyet.Items.AddRange(Enum.GetNames(typeof(Cinsiyet)));
            cmbMedeniDurum.Items.AddRange(Enum.GetNames(typeof(MedeniDurum)));
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                string profilResmi = ProfilResmiOlustur();
                Kisi yeniKisi = new Kisi
                {
                    Ad = txtAd.Text,
                    Boy = int.Parse(txtBoy.Text),
                    Cinsiyet = (Cinsiyet)Enum.Parse(typeof(Cinsiyet), cmbCinsiyet.SelectedItem.ToString()),
                    DogumTarihi = dtpDogumTarihi.Value,
                    Kilo = int.Parse(txtKilo.Text),
                    MedeniDurum = (MedeniDurum)Enum.Parse(typeof(MedeniDurum), cmbMedeniDurum.SelectedItem.ToString()),
                    Meslek = txtMeslek.Text,
                    Soyad = txtSoyad.Text,
                    TCKN = txtTCKN.Text,
                    ProfilResmi= profilResmi
                    
                };
                kisiler.Add(yeniKisi);
                ListeyiDoldur();
                FormuTemizle();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string ProfilResmiOlustur()
        {
            if (dosyaAc.FileName!=null & dosyaAc.FileName!="")
            {
                byte[] resim=  File.ReadAllBytes(dosyaAc.FileName);
                return Convert.ToBase64String(resim);
            }
            else
            {
                return null;
            }
        }

        void FormuTemizle()
        {
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                    item.Text = string.Empty;
                else if (item is ComboBox)
                    (item as ComboBox).SelectedIndex = 0;
                else if (item is DateTimePicker)
                    (item as DateTimePicker).Value = DateTime.Now;
                else if (item is PictureBox)
                    (item as PictureBox).Image = null;
            }
        }
        void ListeyiDoldur()
        {
            lstKisi.Items.Clear();
            foreach (Kisi item in kisiler)
            {
                lstKisi.Items.Add(item);
            }
        }
        Kisi seciliKisi;
        MemoryStream memoryStream = new MemoryStream();
        private void lstKisi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstKisi.SelectedIndex == -1) return;
            seciliKisi = lstKisi.SelectedItem as Kisi;
            txtAd.Text = seciliKisi.Ad;
            txtSoyad.Text = seciliKisi.Soyad;
            txtBoy.Text = seciliKisi.Boy.ToString();
            txtKilo.Text = seciliKisi.Kilo.ToString();
            txtMeslek.Text = seciliKisi.Meslek;
            txtTCKN.Text = seciliKisi.TCKN;
            cmbCinsiyet.SelectedIndex = (int)seciliKisi.Cinsiyet;
            cmbMedeniDurum.SelectedIndex = (int)seciliKisi.MedeniDurum;
            dtpDogumTarihi.Value = seciliKisi.DogumTarihi;
            using (var ms = new MemoryStream(Convert.FromBase64String(seciliKisi.ProfilResmi), 0, Convert.FromBase64String(seciliKisi.ProfilResmi).Length))
            {
                Image image = Image.FromStream(ms, true);
                pbProfilResmi.Image = new Bitmap(image);
            }
            
        }

        private void dışarıAktarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (kisiler.Count == 0)
            {
                MessageBox.Show("Zaten kimse kayıtlı değil neyi dışarı aktaracaksın?");
                return;
            }
            dosyaKaydet.Title = $"{kisiler.Count} adet kişi dışarı aktarılacak";
            dosyaKaydet.Filter = "XML Format | *.xml";
            dosyaKaydet.FileName = string.Empty;
            dosyaKaydet.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (dosyaKaydet.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Kisi>));
                using (TextWriter writer = new StreamWriter(dosyaKaydet.FileName))
                {
                    xmlSerializer.Serialize(writer, kisiler);
                    writer.Close();
                }
                MessageBox.Show("XML aktarma işlemi başarılı");
            }
        }

        private void içeriAktarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dosyaAc.Title = "Bir Kisi XML dosyasını seçiniz";
            dosyaAc.Filter = "XML Format | *.xml";
            dosyaAc.Multiselect = false;
            dosyaAc.FileName = string.Empty;
            dosyaAc.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (dosyaAc.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Kisi>));
                using (TextReader reader = new StreamReader(dosyaAc.FileName))
                {
                    //kisiler = (List<Kisi>)xmlSerializer.Deserialize(reader);
                    kisiler = xmlSerializer.Deserialize(reader) as List<Kisi>;
                    reader.Close();
                    ListeyiDoldur();
                    MessageBox.Show($"{kisiler.Count} adet kişi programa aktarıldı");
                }
            }
        }

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            dosyaAc.Title = "Bir resim dosyasını seçiniz";
            dosyaAc.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            dosyaAc.Multiselect = false;
            dosyaAc.FileName = string.Empty;
            dosyaAc.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (dosyaAc.ShowDialog() == DialogResult.OK)
            {
                pbProfilResmi.ImageLocation = dosyaAc.FileName;
            }
        }
    }
}
