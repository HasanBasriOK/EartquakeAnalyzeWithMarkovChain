using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Analyze
{
    //son gün hafif 
    public partial class frmMain : DevExpress.XtraEditors.XtraForm
    {

        double sondeprem = 0;
        // depremleri listelemek için getobjects metoduna gider
        List<DEPREM> Depremler = new DEPREM().GetObjects(); 
        List<DateTime> Tarihler = new List<DateTime>();// deprem tarihlerini ayrıca not alaiblmek için
        
        // geçiş matrisi için default tanımlama önce 0 a erşitlenip snora artırılıyor
        #region Durum 1
        double hafiftohafif = 0;
        double hafiftoorta = 0;
        double hafiftoyuksek = 0;
        double hafiftoolumcul = 0;

        #endregion
        #region Durum 2
        double ortatohafif = 0;
        double ortatoorta = 0;
        double ortatoyuksek = 0;
        double ortatoolumcul = 0;
        #endregion
        #region Durum 3
        double yuksektohafif = 0;
        double yuksektoorta = 0;
        double yuksektoyuksek = 0;
        double yuksektoolumcul = 0;

        #endregion
        #region Durum 4
        double Olumcultohafif = 0;
        double Olumcultoorta = 0;
        double Olumcultoyuksek = 0;
        double Olumcultoolumcul = 0;
        #endregion
        // burada ise deprem sayılarını 0 a eşitleyip artırılacak
        public int hafif = 0;
        public int orta = 0;
        public int yuksek = 0;
        public int olumcul = 0;
        public int depremSayisi = 0;

        //vir gülden snraki basamakları tanımlamak için tanımlanan değişkenler (matrisler için)
        double hafiftohafiftoplam;
        double hafiftoortatoplam;
        double hafiftoyuksektoplam;
        double hafiftoolumcultoplam;
        double ortatohafiftoplam;
        double ortatoortatoplam;
        double ortatoyuksektoplam;
        double ortatoolumcultoplam;
        double yuksektohafiftoplam;
        double yuksektoortatoplam;
        double yuksektoyuksektoplam;
        double yuksektoolumcultoplam;
        double olumcultohafiftoplam;
        double olumcultoortatoplam;
        double olumcultoyuksektoplam;
        double olumcultoolumcultoplam;



        //key ve value , intler key olsun matirisn kaçınc elamanı olduğunu göstersin , double ise o elemanın değeri oluyor.
        // matristki, çarpım işlemleri için yapıldı.
        Dictionary<int, double> YeniMatrisElemanlari = new Dictionary<int, double>();

        Dictionary<int, double> MatrisElemanlari = new Dictionary<int, double>();
        Dictionary<int, double> MatrisKopya = new Dictionary<int, double>();


        public frmMain()
        {
            InitializeComponent();
        }

        // en baştaki geçiş matirism neyse deyip dctionary e bunu atıyoruz. Matris çarpımını aşağıda yapacağımız için dictionry elimizde olsun sebepli.
        public Dictionary<int,double> DefaultGecisMatris()
        {
            Dictionary<int, double> matris = new Dictionary<int, double>();
            matris.Add(1, hafiftohafiftoplam);
            matris.Add(2, hafiftoortatoplam);
            matris.Add(3, hafiftoyuksektoplam);
            matris.Add(4, hafiftoolumcultoplam);
            matris.Add(5, ortatohafiftoplam);
            matris.Add(6, ortatoortatoplam);
            matris.Add(7, ortatoyuksektoplam);
            matris.Add(8, ortatoolumcultoplam);
            matris.Add(9, yuksektohafiftoplam);
            matris.Add(10, yuksektoortatoplam);
            matris.Add(11, yuksektoyuksektoplam);
            matris.Add(12, yuksektoolumcultoplam);
            matris.Add(13, olumcultohafiftoplam);
            matris.Add(14, olumcultoortatoplam);
            matris.Add(15, olumcultoyuksektoplam);
            matris.Add(16, olumcultoolumcultoplam);
            return matris;
        }

        // listele butonuna basınca getobject gelsin , depreml listelensin
        private void btnListele_Click(object sender, EventArgs e)
        {
            gridControl1.DataSource = new DEPREM().GetObjects();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            // deprem sayısı kadar elemanı olan dizi oluşturuldu. 
            double[] a = new double[Depremler.Count];
            int i = 0;
            // depremler listesi içnde dolanıyoruz.
            foreach (var item in Depremler)
            {

                depremSayisi++;
                double sayi = Convert.ToDouble(item.SIDDET);// veritabanında şiddet string o yüzden doublea çevrildi
                double gerceksayi = 0;// veri tabanından sayı 10 katı geldiği geldiği için aşağıda bölğm işlemi yapılıyor

                if (sayi < 100)
                {
                    gerceksayi = sayi / 10;
                }
                if (gerceksayi < 3.1)
                {
                    hafif++;
                }
                else if (gerceksayi > 3.0 && gerceksayi < 5.1)
                {
                    orta++;
                }
                else if (gerceksayi > 5.0 && gerceksayi < 7.1)
                {
                    yuksek++;
                }
                else if (gerceksayi > 7.0)
                {
                    olumcul++;
                }
                a[i] = gerceksayi;
                i++; // bütün diziyi deprem değerleriyle dolduruyoruz

            }
            //geçiş matrisi için depremlerin birbirinden sonra gelme durumlarını buluyoruz
            //Bulma işleminde dikkat ettiğimiz nokta markov zincirinin mantığı olan bi önceki veriyle kıyaslama olayı
            //bu yüzden a dizisinin j ve j+1. elemanını kıyaslıyoruz her seferinde
            for (int j = 0; j < a.Length; j++)
            {// a dizisnin uzunluğu kadar döner

                // geçiş matirisnin elemanları oluşturuluyor
                if (j + 1 != a.Length)
                {
                    if (a[j] < 3.1 && a[j + 1] < 3.1)
                    {
                        hafiftohafif++;
                    }
                    else if (a[j] < 3.1 && a[j + 1] > 3.0 && a[j + 1] < 5.1)
                    {
                        hafiftoorta++;
                    }
                    else if (a[j] < 3.1 && a[j + 1] > 5.0 && a[j + 1] < 7.1)
                    {
                        hafiftoyuksek++;
                    }
                    else if (a[j] < 3.1 && a[j + 1] > 7.0)
                    {
                        hafiftoolumcul++;
                    }
                    else if (a[j] > 3.0 && a[j] < 5.1 && a[j + 1] < 3.1)
                    {
                        ortatohafif++;
                    }
                    else if (a[j] > 3.0 && a[j] < 5.1 && a[j + 1] > 3.0 && a[j + 1] < 5.1)
                    {
                        ortatoorta++;
                    }
                    else if (a[j] > 3.0 && a[j] < 5.1 && a[j + 1] > 5.0 && a[j + 1] < 7.1)
                    {
                        ortatoyuksek++;
                    }
                    else if (a[j] > 3.0 && a[j] < 5.1 && a[j + 1] > 7.0)
                    {
                        ortatoolumcul++;
                    }
                    else if (a[j] > 5.0 && a[j] < 7.1 && a[j + 1] < 3.1)
                    {
                        yuksektohafif++;
                    }
                    else if (a[j] > 5.0 && a[j] < 7.1 && a[j + 1] > 3.0 && a[j + 1] < 5.1)
                    {
                        yuksektoorta++;
                    }
                    else if (a[j] > 5.0 && a[j] < 7.1 && a[j + 1] > 5.0 && a[j + 1] < 7.1)
                    {
                        yuksektoyuksek++;
                    }
                    else if (a[j] > 5.0 && a[j] < 7.1 && a[j + 1] > 7.0)
                    {
                        yuksektoolumcul++;
                    }
                    else if (a[j] > 7.0 && a[j + 1] < 3.1)
                    {
                        Olumcultohafif++;
                    }
                    else if (a[j] > 7.0 && a[j + 1] > 3.0 && a[j + 1] < 5.1)
                    {
                        Olumcultoorta++;
                    }
                    else if (a[j] > 7.0 && a[j + 1] > 5.0 && a[j + 1] < 7.1)
                    {
                        Olumcultoyuksek++;
                    }
                    else if (a[j] > 7.0 && a[j + 1] > 7.0)
                    {
                        Olumcultoolumcul++;
                    }
                }
            }
            // toplamda kaçtane deprem olduğunu buluyor
            double toplam = hafiftohafif + hafiftoolumcul + hafiftoorta + hafiftoyuksek + ortatohafif + ortatoorta + ortatoolumcul + ortatoyuksek + yuksektohafif + yuksektoolumcul + yuksektoorta + yuksektoyuksek + Olumcultohafif + Olumcultoolumcul + Olumcultoorta + Olumcultoyuksek;

            //Geçiş matrisinin elemanlarını buluyoruz 
            #region Osman Hocadan sonraki geliştirdiğim metod 
            hafiftohafiftoplam = hafiftohafif / hafif;
            hafiftohafiftoplam = Math.Round(hafiftohafiftoplam, 3);// virgülden sona 3 basamak alınsın diye 

            hafiftoolumcultoplam = hafiftoolumcul / hafif;
            hafiftoolumcultoplam = Math.Round(hafiftoolumcultoplam, 3);

            hafiftoortatoplam = hafiftoorta / hafif;
            hafiftoortatoplam = Math.Round(hafiftoortatoplam, 3);

            hafiftoyuksektoplam = hafiftoyuksek / hafif;
            hafiftoyuksektoplam = Math.Round(hafiftoyuksektoplam, 3);

            olumcultohafiftoplam = Olumcultohafif / olumcul;
            olumcultohafiftoplam = Math.Round(olumcultohafiftoplam, 3);

            olumcultoolumcultoplam = Olumcultoolumcul / olumcul;
            olumcultoolumcultoplam = Math.Round(olumcultoolumcultoplam, 3);

            olumcultoortatoplam = Olumcultoorta / olumcul;
            olumcultoortatoplam = Math.Round(olumcultoortatoplam, 3);

            olumcultoyuksektoplam = Olumcultoyuksek / olumcul;
            olumcultoyuksektoplam = Math.Round(olumcultoyuksektoplam, 3);

            ortatoolumcultoplam = ortatoolumcul / orta;
            ortatoolumcultoplam = Math.Round(ortatoolumcultoplam, 3);

            ortatohafiftoplam = ortatohafif / orta;
            ortatohafiftoplam = Math.Round(ortatohafiftoplam, 3);

            ortatoortatoplam = ortatoorta / orta;
            ortatoortatoplam = Math.Round(ortatoortatoplam, 3);

            ortatoyuksektoplam = ortatoyuksek / orta;
            ortatoyuksektoplam = Math.Round(ortatoyuksektoplam, 3);

            yuksektohafiftoplam = yuksektohafif / yuksek;
            yuksektohafiftoplam = Math.Round(yuksektohafiftoplam, 3);

            yuksektoolumcultoplam = yuksektoolumcul / yuksek;
            yuksektoolumcultoplam = Math.Round(yuksektoolumcultoplam, 3);

            yuksektoortatoplam = yuksektoorta / yuksek;
            yuksektoortatoplam = Math.Round(yuksektoortatoplam, 3);

            yuksektoyuksektoplam = yuksektoyuksek / yuksek;
            yuksektoyuksektoplam = Math.Round(yuksektoyuksektoplam, 3);
            #endregion
            

            //bulduğumuz elemanları label ile ürettiğimiz matrise aktarıyoruz
            lblHafiftoHafif.Text = hafiftohafiftoplam.ToString();
            lblHafifToOlumcul.Text = hafiftoolumcultoplam.ToString();
            lblHafifToOrta.Text = hafiftoortatoplam.ToString();
            lblHafiftoYuksek.Text = hafiftoyuksektoplam.ToString();
            lblOlumculToHafif.Text = olumcultohafiftoplam.ToString();
            lblOlumculToOlumcul.Text = olumcultoolumcultoplam.ToString();
            lblOlumculToOrta.Text = olumcultoortatoplam.ToString();
            lblOlumculToYuksek.Text = olumcultoyuksektoplam.ToString();
            lblOrtaOlumcul.Text = ortatoolumcultoplam.ToString();
            lblOrtaToHafif.Text = ortatohafiftoplam.ToString();
            lblOrtaToOrta.Text = ortatoortatoplam.ToString();
            lblOrtaToYuksek.Text = ortatoyuksektoplam.ToString();
            lblYuksekToHafif.Text = yuksektohafiftoplam.ToString();
            lblYuksekToOlumcul.Text = yuksektoolumcultoplam.ToString();
            lblYuksekToOrta.Text = yuksektoortatoplam.ToString();
            lblYuksekToYuksek.Text = yuksektoyuksektoplam.ToString();

            #region Default Geçiş Matrisi

            //Dictionary tipinde bir değişkene key değeri olarak matristeki elemanın sırasını value olarak da o sıradaki değeri set ediyoruz.


            MatrisKopya = DefaultGecisMatris();


            #endregion

            //Form Yüklendiğinde(aktarıldığında) görünürlüğü false olan matris elemanlarının görünürlüğünü true yapıyoruz
            lblHafiftoHafif.Visible = true;
            lblHafifToOlumcul.Visible = true;
            lblHafifToOrta.Visible = true;
            lblHafiftoYuksek.Visible = true;
            lblOlumculToHafif.Visible = true;
            lblOlumculToOlumcul.Visible = true;
            lblOlumculToOrta.Visible = true;
            lblOlumculToYuksek.Visible = true;
            lblOrtaOlumcul.Visible = true;
            lblOrtaToHafif.Visible = true;
            lblOrtaToOrta.Visible = true;
            lblOrtaToYuksek.Visible = true;
            lblYuksekToHafif.Visible = true;
            lblYuksekToOlumcul.Visible = true;
            lblYuksekToOrta.Visible = true;
            lblYuksekToYuksek.Visible = true;
            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            label8.Visible = true;

            //son depreme göre gelecek depremlerin kıyaslamasını yapacağımız için son depremin değerini buluyor ve kullanıcıyı bilgilendiriyoruz
            int lasteartquake = a.Length;

            sondeprem = a[lasteartquake - 1];



            MessageBox.Show(depremSayisi + " Depremden " + hafif + " tanesi hafif, " + orta + " tanesi orta, " + yuksek + " tanesi yüksek, " + olumcul + " tanesi de ölümcül depremdir.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MessageBox.Show("Son Depremin Şiddeti :" + sondeprem + " olup son olan deprem hafif şiddettedir.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);



        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //Form İlk Yüklendiğinde geçiş matrisi elemanları görünmeyecek

            lblHafiftoHafif.Visible = false;
            lblHafifToOlumcul.Visible = false;
            lblHafifToOrta.Visible = false;
            lblHafiftoYuksek.Visible = false;
            lblOlumculToHafif.Visible = false;
            lblOlumculToOlumcul.Visible = false;
            lblOlumculToOrta.Visible = false;
            lblOlumculToYuksek.Visible = false;
            lblOrtaOlumcul.Visible = false;
            lblOrtaToHafif.Visible = false;
            lblOrtaToOrta.Visible = false;
            lblOrtaToYuksek.Visible = false;
            lblYuksekToHafif.Visible = false;
            lblYuksekToOlumcul.Visible = false;
            lblYuksekToOrta.Visible = false;
            lblYuksekToYuksek.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label11.Visible = false;


            #region Sonradan Oluşturulacak olan Matris

            //Tahmin üstüne oluşturulacak olan matris görünmeyecek (kaç gün sonra ne olacak sorusu)
            groupControl4.Visible = false;// grupboxta yaoıldığı için ilk etapta görünürlüğü false


            #endregion



        }
        // burada da text boxta sadecesayı girilebimesi için aşağısı oluşturuldu harf grilirse yazmayacak
        private void textEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        // hesaplama kısmı (tahminde bulun butonu)
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int day = 0;
            //Kullanıcı Gün Sayısı Girmediyse Hata Mesajı çıkacak
            if (txtDay.Text == string.Empty)
            {
                MessageBox.Show("Gün Sayısını Girin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                day = Convert.ToInt32(txtDay.Text);

                MatrisElemanlari.Clear();
                MatrisElemanlari = DefaultGecisMatris();
                //en başta matris elemanlarımda önceden hesap yapılmış olabilme ihtimaline karşı,temizliyoruz,
                //sonrasında yukarıda tanımladığımız defaultgecismatrisi metodunu kullanarak geçiş matrisimizin atamasını yapıyoruz

                //Her çarpma işleminde yeni değerler YeniMatrisElemanları değişkenine atanacak
                if (YeniMatrisElemanlari.Count == 0)
                {
                    YeniMatrisElemanlari = MatrisKopya;
                }
                else
                { // daha önceden matris çarpımı yapılmış ise yeni matris elemanlarını 0 lıyoruz
                    // dicionary dinamik bir değişken olduğu için matrisi eşitlemesinde 3. bi,r matrisi matris kopya olarak kullanıyoruz(sebebi değişmesi)
                    YeniMatrisElemanlari = new Dictionary<int, double>();// içinde leman kalmışsa yeniden oluştur
                    YeniMatrisElemanlari = DefaultGecisMatris();
                    // ilk çarpımda amaç matrisin karesini almak olduğu için yeni matris elemanlaını da geçiş matrisine eşitliyorum
                }
                //Gün Sayısı Girdiyse matrisin gün sayısı kadar kuvveti alınacak

                for (int i = 1; i < day; i++)
                {
                    MatrisElemanlari.Clear();
                    MatrisElemanlari = DefaultGecisMatris();
                    #region Birinci Satır
                    // matris çarpım işlemi yapılıyor yani 1. satır 1. sütün u bulabilmek için yapılan işlem
                    double birebir = (MatrisElemanlari.FirstOrDefault(x => x.Key == 1).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 1).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 2).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 5).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 3).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 9).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 4).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 13).Value);
                    double birebirvirgul = Math.Round(birebir, 5);

                    double bireiki = (MatrisElemanlari.FirstOrDefault(x => x.Key == 1).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 2).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 2).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 6).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 3).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 10).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 4).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 14).Value);
                    double bireikivirgul = Math.Round(bireiki, 5);

                    double bireuc = (MatrisElemanlari.FirstOrDefault(x => x.Key == 1).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 3).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 2).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 7).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 3).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 11).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 4).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 15).Value);
                    double bireucvirgul = Math.Round(bireuc, 5);

                    double biredort = (MatrisElemanlari.FirstOrDefault(x => x.Key == 1).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 4).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 2).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 8).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 3).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 12).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 4).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 16).Value);
                    double biredortvirgul = Math.Round(biredort, 5);

                    #endregion
                    #region İkinci Satır

                    double ikiyebir = (MatrisElemanlari.FirstOrDefault(x => x.Key == 5).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 1).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 6).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 5).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 7).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 9).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 8).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 13).Value);
                    double ikiyebirvirgul = Math.Round(ikiyebir, 5);


                    double ikiyeiki = (MatrisElemanlari.FirstOrDefault(x => x.Key == 5).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 2).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 6).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 6).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 7).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 10).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 8).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 14).Value);
                    double ikiyeikivirgul = Math.Round(ikiyeiki, 5);


                    double ikiyeuc = (MatrisElemanlari.FirstOrDefault(x => x.Key == 5).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 3).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 6).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 7).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 7).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 11).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 8).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 15).Value);
                    double ikiyeucvirgul = Math.Round(ikiyeuc, 5);

                    double ikiyedort = (MatrisElemanlari.FirstOrDefault(x => x.Key == 5).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 4).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 6).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 8).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 7).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 12).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 8).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 16).Value);
                    double ikiyedortvirgul = Math.Round(ikiyedort, 5);
                    #endregion
                    #region Üçüncü Satır

                    double ucebir = (MatrisElemanlari.FirstOrDefault(x => x.Key == 9).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 1).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 10).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 5).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 11).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 9).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 12).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 13).Value);
                    double ucebirvirgul = Math.Round(ucebir, 5);

                    double uceiki = (MatrisElemanlari.FirstOrDefault(x => x.Key == 9).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 2).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 10).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 6).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 11).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 10).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 12).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 14).Value);
                    double uceikivirgul = Math.Round(uceiki, 5);



                    double uceuc = (MatrisElemanlari.FirstOrDefault(x => x.Key == 9).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 3).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 10).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 7).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 11).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 11).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 12).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 15).Value);
                    double uceucvirgul = Math.Round(uceuc, 5);


                    double ucedort = (MatrisElemanlari.FirstOrDefault(x => x.Key == 9).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 4).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 10).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 8).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 11).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 12).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 12).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 16).Value);
                    double ucedortvirgul = Math.Round(ucedort, 5);

                    #endregion
                    #region Dördüncü Satır
                    double dortebir = (MatrisElemanlari.FirstOrDefault(x => x.Key == 13).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 1).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 14).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 5).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 15).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 9).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 16).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 13).Value);
                    double dortebirvirgul = Math.Round(dortebir, 5);

                    double dorteiki = (MatrisElemanlari.FirstOrDefault(x => x.Key == 13).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 2).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 14).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 6).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 15).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 10).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 16).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 14).Value);
                    double dorteikivirgul = Math.Round(dorteiki, 5);


                    double dorteuc = (MatrisElemanlari.FirstOrDefault(x => x.Key == 13).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 3).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 14).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 7).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 15).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 11).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 16).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 15).Value);
                    double dorteucvirgul = Math.Round(dorteuc, 5);


                    double dortedort = (MatrisElemanlari.FirstOrDefault(x => x.Key == 13).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 4).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 14).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 8).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 15).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 12).Value) + (MatrisElemanlari.FirstOrDefault(x => x.Key == 16).Value * YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 16).Value);
                    double dortedortvirgul = Math.Round(dortedort, 5);

                    #endregion

                    // önce temizlendi yeni matri oluşturulan içne ekleniyor 
                    YeniMatrisElemanlari.Clear();

                    YeniMatrisElemanlari.Add(1, birebirvirgul);
                    YeniMatrisElemanlari.Add(2, bireikivirgul);
                    YeniMatrisElemanlari.Add(3, bireucvirgul);
                    YeniMatrisElemanlari.Add(4, biredortvirgul);
                    YeniMatrisElemanlari.Add(5, ikiyebirvirgul);
                    YeniMatrisElemanlari.Add(6, ikiyeikivirgul);
                    YeniMatrisElemanlari.Add(7, ikiyeucvirgul);
                    YeniMatrisElemanlari.Add(8, ikiyedortvirgul);
                    YeniMatrisElemanlari.Add(9, ucebirvirgul);
                    YeniMatrisElemanlari.Add(10, uceikivirgul);
                    YeniMatrisElemanlari.Add(11, uceucvirgul);
                    YeniMatrisElemanlari.Add(12, ucedortvirgul);
                    YeniMatrisElemanlari.Add(13, dortebirvirgul);
                    YeniMatrisElemanlari.Add(14, dorteikivirgul);
                    YeniMatrisElemanlari.Add(15, dorteucvirgul);
                    YeniMatrisElemanlari.Add(16, dortedortvirgul);



                }
                //Kuvveti Alınmış olan matrisin yeni elemanlarını yeni matrisimize set ediyoruz
                // firstordefault matirisin içindeki elemanlara belirlediğimiz kriter bazında ulaşmak için
                //

                labelControl9.Text = YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 1).Value.ToString();
                labelControl10.Text = YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 2).Value.ToString();
                labelControl11.Text = YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 3).Value.ToString();
                labelControl12.Text = YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 4).Value.ToString();
                labelControl13.Text = YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 5).Value.ToString();
                labelControl14.Text = YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 6).Value.ToString();
                labelControl15.Text = YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 7).Value.ToString();
                labelControl16.Text = YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 8).Value.ToString();
                labelControl17.Text = YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 9).Value.ToString();
                labelControl18.Text = YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 10).Value.ToString();
                labelControl19.Text = YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 11).Value.ToString();
                labelControl20.Text = YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 12).Value.ToString();
                labelControl21.Text = YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 13).Value.ToString();
                labelControl22.Text = YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 14).Value.ToString();
                labelControl23.Text = YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 15).Value.ToString();
                labelControl24.Text = YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 16).Value.ToString();

                //Yeni matrisimizin görünürlüğünü true yapıyoruz
                groupControl4.Visible = true;


                //kullanıcının seçtiği şidete göre geçiş matrisinden değeri alıp sonuç label'a olarak yazdırıyoruz
                //en başta kullanıcının şiddet seçip seçmediğini kontrol ediyoruz
                if (comboBoxEdit1.Text != string.Empty)
                {
                    //son deprem hafif ise
                    if (sondeprem < 3.1)
                    {
                        if (comboBoxEdit1.Text == "Yüksek")
                        {
                            label11.Text = "Tahmin Sonucu :" + YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 3).Value.ToString();
                        }
                        else if (comboBoxEdit1.Text == "Hafif")
                        {
                            label11.Text = "Tahmin Sonucu :" + YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 1).Value.ToString();

                        }
                        else if (comboBoxEdit1.Text == "Orta")
                        {
                            label11.Text = "Tahmin Sonucu :" + YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 2).Value.ToString();

                        }
                        else if (comboBoxEdit1.Text == "Ölümcül")
                        {
                            label11.Text = "Tahmin Sonucu :" + YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 4).Value.ToString();

                        }
                    }
                    //son deprem orta boyutta ise
                    else if (sondeprem > 3.0 && sondeprem < 5.1)
                    {
                        if (comboBoxEdit1.Text == "Yüksek")
                        {
                            label11.Text = "Tahmin Sonucu :" + YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 7).Value.ToString();
                        }
                        else if (comboBoxEdit1.Text == "Hafif")
                        {
                            label11.Text = "Tahmin Sonucu :" + YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 5).Value.ToString();

                        }
                        else if (comboBoxEdit1.Text == "Orta")
                        {
                            label11.Text = "Tahmin Sonucu :" + YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 6).Value.ToString();

                        }
                        else if (comboBoxEdit1.Text == "Ölümcül")
                        {
                            label11.Text = "Tahmin Sonucu :" + YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 8).Value.ToString();

                        }
                    }
                    //son deprem yüksek boyutta ise
                    else if (sondeprem > 5.0 && sondeprem < 7.1)
                    {
                        if (comboBoxEdit1.Text == "Yüksek")
                        {
                            label11.Text = "Tahmin Sonucu :" + YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 11).Value.ToString();
                        }
                        else if (comboBoxEdit1.Text == "Hafif")
                        {
                            label11.Text = "Tahmin Sonucu :" + YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 9).Value.ToString();

                        }
                        else if (comboBoxEdit1.Text == "Orta")
                        {
                            label11.Text = "Tahmin Sonucu :" + YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 10).Value.ToString();

                        }
                        else if (comboBoxEdit1.Text == "Ölümcül")
                        {
                            label11.Text = "Tahmin Sonucu :" + YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 12).Value.ToString();

                        }
                    }
                    //son deprem ölümcül boyutta ise
                    else
                    {
                        if (comboBoxEdit1.Text == "Yüksek")
                        {
                            label11.Text = "Tahmin Sonucu :" + YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 15).Value.ToString();
                        }
                        else if (comboBoxEdit1.Text == "Hafif")
                        {
                            label11.Text = "Tahmin Sonucu :" + YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 13).Value.ToString();

                        }
                        else if (comboBoxEdit1.Text == "Orta")
                        {
                            label11.Text = "Tahmin Sonucu :" + YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 14).Value.ToString();

                        }
                        else if (comboBoxEdit1.Text == "Ölümcül")
                        {
                            label11.Text = "Tahmin Sonucu :" + YeniMatrisElemanlari.FirstOrDefault(x => x.Key == 16).Value.ToString();

                        }
                    }


                    label11.Visible = true;
                }
                else
                {
                    MessageBox.Show("Lütfen Tahmin Edilmesini İstediğiniz Şiddeti Seçin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}