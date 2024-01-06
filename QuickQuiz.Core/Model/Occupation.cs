using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQuiz.Core.Model
{
    public enum Occupation
    {
        [Description("Doktor/Hemşire")]
        DoktorHemsire,

        [Description("Mühendis")]
        Muhendis,

        [Description("Öğretmen")]
        Ogretmen,

        [Description("Bankacı/Finans Uzmanı")]
        Bankaci,

        [Description("Kamu Görevlisi (Memur)")]
        KamuGorevlisi,

        [Description("Avukat/Hukukçu")]
        Avukat,

        [Description("Yazılım Geliştirici/Bilgisayar Programcısı")]
        YazilimGelistirici,

        [Description("Satış ve Pazarlama Uzmanı")]
        SatisPazarlamaUzmani,

        [Description("İnsan Kaynakları Uzmanı")]
        InsanKaynaklariUzmani,

        [Description("İşletme Yöneticisi")]
        IsletmeYoneticisi,

        [Description("Eczacı")]
        Eczaci,

        [Description("Diş Hekimi")]
        DisHekimi,

        [Description("Psikolog")]
        Psikolog,

        [Description("Mimar")]
        Mimar,

        [Description("Reklamcı/PR Uzmanı")]
        Reklamci,

        [Description("Grafik Tasarımcı")]
        GrafikTasarimci,

        [Description("Veteriner")]
        Veteriner,

        [Description("Yatırım Danışmanı")]
        YatirimDanismani,

        [Description("Turizm ve Otelcilik Meslekleri")]
        TurizmOtelcilik,

        [Description("Bilgisayar Sistemleri Analisti")]
        BilgisayarSistemAnalisti,

        [Description("Endüstriyel Tasarımcı")]
        EndustriyelTasarimci,

        [Description("Veri Bilimci/Analitik Uzmanı")]
        VeriBilimci,

        [Description("Telekomünikasyon Uzmanı")]
        TelekomunikasyonUzmani,

        [Description("İş Sağlığı ve Güvenliği Uzmanı")]
        IsSagligiGuvenligiUzmani,

        [Description("Fizyoterapist")]
        Fizyoterapist,

        [Description("Tarım ve Ziraat Uzmanı")]
        TarimZiraatUzmani,

        [Description("Jeolog")]
        Jeolog,

        [Description("Biyolog")]
        Biyolog,

        [Description("Radyolog")]
        Radyolog,

        [Description("Pilot")]
        Pilot,

        [Description("Lojistik Uzmanı")]
        LojistikUzmani,

        [Description("Oyun Geliştirici")]
        OyunGelistirici,

        [Description("Güvenlik Uzmanı")]
        GuvenlikUzmani,

        [Description("Yatırım Analisti")]
        YatirimAnalisti,

        [Description("Antrenör")]
        Antrenor,

        [Description("Yat Kaptanı")]
        YatKaptani,

        [Description("Biyoistatistikçi")]
        Biyoistatistikci,

        [Description("Sigorta Brokerı")]
        SigortaBrokeri,

        [Description("Şef/Aşçı")]
        SefAsci,

        [Description("İllüstratör")]
        Illustrato,

        [Description("Yönetim Danışmanı")]
        YonetimDanismani,

        [Description("Kıyafet Tasarımcısı")]
        KiyafetTasarimcisi
    }
}

