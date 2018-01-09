using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y28XML_JSON_SerializeDeserialize
{
    public class Kisi
    {
        /*
         * Ad 
         * Soyad -- ad ve soyad Baş harf büyük sonraki harfler küçük olmalı ve harf,boşluk dışında bir karakter içermemelidir.
         * Doğum Tarihi -- en az 18 yaşından gün almış.
         * TCKN ! 11 haneli olmalı +2tane de kural(sadece rakamlar,son hanesi çift olmalı)
         * Boy, kilo 
         * Meslek
         * Medeni Durum,
         * Cinsiyet
         * 
         */
        private string _ad;
        private string _soyad;
        private string _tckn;
        private DateTime _dogumTarihi;
        private int _yas;
        public string ProfilResmi { get; set; }

        public string Ad
        {
            get { return _ad; }
            set { _ad = AdSoyadValid(value); }
        }
        public string Soyad
        {
            get { return _soyad; }
            set { _soyad = AdSoyadValid(value); }
        }
        public DateTime DogumTarihi
        {
            get { return _dogumTarihi; }
            set { _dogumTarihi = DogumTarihiValid(value); }
        }
        public int Yas { get { return this._yas; } }
        public string TCKN
        {
            get { return _tckn; }
            set { _tckn = TCKNValid(value); }
        }
        public int Boy { get; set; }
        public int Kilo { get; set; }
        public string Meslek { get; set; }
        public MedeniDurum MedeniDurum { get; set; }
        public Cinsiyet Cinsiyet { get; set; }
        private string AdSoyadValid(string kelime)
        {
            string adsoyad = kelime.Trim();
            foreach (char harf in adsoyad)
            {
                if (!char.IsLetter(harf) || char.IsWhiteSpace(harf))
                    throw new Exception("Ad veya soyad içerisinde geçersiz karakter bulunmaktadır!");
            }
            if (adsoyad.Length < 3)
                throw new Exception("Ad veya soyad en az 3 karakter olmalı");
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(adsoyad);
        }
        private string TCKNValid(string tckn)
        {
            if (tckn.Length != 11)
                throw new Exception("TCKN 11 haneli olmalıdır");
            foreach (char harf in tckn)
                if (!char.IsDigit(harf))
                    throw new Exception("TCKN içerisinde sadece rakam bulunmalıdır");
            if (Convert.ToInt32(tckn[10]) % 2 != 0)
                throw new Exception("TCKN son rakamı çift olmalı");
            if (tckn[0] == '0')
                throw new Exception("TCKN '0' ile başlayamaz");
            return tckn;
        }
        private DateTime DogumTarihiValid(DateTime dogumTarihi)
        {
            TimeSpan aralik = DateTime.Now - dogumTarihi;
            double yil = aralik.TotalDays / 365;
            this._yas = (int)yil;
            if (yil <= 17)
                throw new Exception("Sisteme 18 yaşından gün almamış kişileri kaydedemiyoruz!");
            return dogumTarihi;
        }
        public override string ToString()
        {
            return $"{Ad} {Soyad} - {Yas} {Cinsiyet.ToString().Substring(0, 1)}";
        }
    }
    public enum MedeniDurum
    {
        Evli, Bekar, Dul
    }
    public enum Cinsiyet { Kadın, Erkek, Belirsiz }
}
