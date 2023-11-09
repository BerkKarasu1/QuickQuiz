using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQuiz.Core.Model
{
    public enum TestCategorys
    {
        [Description("Teknoloji ve Dijital Dünya")]
        Teknoloji_ve_Dijital_Dünya,
        [Description("Bilim")]
        Bilim_ve_Teknoloji,
        [Description("Sanat")]
        Sanat_ve_Edebiyat,
        [Description("Tarih")]
        Tarih_ve_Tarihçe,
        [Description("Coğrafya")]
        Coğrafya_ve_Ülkeler,
        [Description("Müzik")]
        Müzik_ve_Müzik_Tarihi,
        [Description("Spor")]
        Spor,
        [Description("Oyun")]
        Oyun,
        [Description("Sinema")]
        Sinema,
        [Description("Doğa ve Çevre")]
        Doğa_ve_Çevre,
        [Description("Kişisel Gelişim")]
        Kişisel_Gelişim,
        [Description("Psikoloji")]
        Psikoloji,
        [Description("Din ve Mitoloji")]
        Din_ve_Mitoloji,
        [Description("Yemek ve Mutfak")]
        Yemek_ve_Mutfak,
        [Description("Popüler Kültür")]
        Popüler_Kültür_ve_Ünlüler,
        [Description("Sağlık")]
        Sağlık_ve_Tıp,
        [Description("Moda ve Güzellik")]
        Moda_ve_Güzellik,
        [Description("Toplumsal Konular")]
        İnsan_Hakları_ve_Toplumsal_Konular,
        [Description("Diğer")]
        Diger,

    }
}
