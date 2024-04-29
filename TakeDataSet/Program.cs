using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using QuickQuiz.Core.Model;
using QuickQuiz.Repository;

AppDbContext dbContext = new();
List<AppUser> users = [.. dbContext.Users];
List<Model> models = new();
var examList = dbContext.ExamResults.Include(x => x.Exam).ToList();
foreach (var user in users)
{
    if (string.IsNullOrEmpty(user.City))
        continue;
    var exams = examList.Where(x => x.Student.Id == user.Id).ToList();
    int spor = 0;
    int tarih = 0;
    int cografya = 0;
    int din = 0;
    int bilim = 0;
    foreach (var item in exams)
    {
        switch (item.Exam.TestCategorys)
        {
            case TestCategorys.Spor:
                spor++;
                break;
            case TestCategorys.Tarih_ve_Tarihçe:
                tarih++;
                break;
            case TestCategorys.Coğrafya_ve_Ülkeler:
                cografya++;
                break;
            case TestCategorys.Din_ve_Mitoloji:
                din++;
                break;
            case TestCategorys.Bilim_ve_Teknoloji:
                bilim++;
                break;
            default:
                break;
        }
    }
    Model mod = new()
    {
        Age = GetAgeCategory(user.BirthDate),
        City = user?.City,
        EducationLevel = GetEducationDegree(user.EducationLevel),
        Gender = user.Gender.ToString(),
        Occupation = user.Occupation.ToString()
    };
    int[] numbs = [spor, tarih, cografya, din, bilim];
    if (numbs.Max().Equals(spor))
        mod.Choice = "Spor";
    else if (numbs.Max().Equals(tarih))
        mod.Choice = "Tarih";
    else if (numbs.Max().Equals(cografya))
        mod.Choice = "Coğrafya";
    else if (numbs.Max().Equals(din))
        mod.Choice = "Din ve Mitoloji";
    else if (numbs.Max().Equals(bilim))
        mod.Choice = "Bilim";
    models.Add(mod);
}
try
{
    ExcelTemp(models);

}
catch 
{

}
Console.ReadLine();
Console.ReadLine();





void ExcelTemp(List<Model> models)
{
    var workbook = new XLWorkbook();

    // Sayfa ekleyin
    var worksheet = workbook.Worksheets.Add("Sheet1");
    worksheet.Cell("A1").Value = "Age";
    worksheet.Cell("B1").Value = "City";
    worksheet.Cell("C1").Value = "Education Level";
    worksheet.Cell("D1").Value = "Gender";
    worksheet.Cell("E1").Value = "Occupation";
    worksheet.Cell("F1").Value = "Choice";
    for (int i = 0; i < models.Count; i++)
    {
        worksheet.Cell($"A{i+2}").Value = models[i].Age;
        worksheet.Cell($"B{i+2}").Value = models[i].City;
        worksheet.Cell($"C{i+2}").Value = models[i].EducationLevel;
        worksheet.Cell($"D{i+2}").Value = models[i].Gender;
        worksheet.Cell($"E{i+2}").Value = models[i].Occupation;
        worksheet.Cell($"F{i+2}").Value = models[i].Choice;
    }

    // Dosyayı kaydet
    workbook.SaveAs("C:\\Users\\odya\\source\\repos\\BerkKarasu1\\QuickQuiz\\TakeDataSet\\example.xlsx");
}
string GetEducationDegree(EducationLevel? education)
{
    if (education == EducationLevel.HighSchool)
        return "Lise";
    else if (education == EducationLevel.AssociatesDegree)
        return "ÖnLisans";
    else if (education == EducationLevel.BachelorsDegree)
        return "Lisans";
    else if (education == EducationLevel.MastersDegree)
        return "Yüksek Lisans";
    else if (education == EducationLevel.Doctorate)
        return "Doktora";
    else return "";
}
string GetAgeCategory(DateTime? date)
{
    int age = DateTime.Now.Year - date.Value.Year;
    if (age >= 65)
        return "65+";
    else if (age < 65 && age >= 50)
        return "50-64";
    else if (age < 50 && age >= 40)
        return "40-49";
    else if (age < 40 && age >= 30)
        return "30-39";
    else if (age < 30 && age >= 20)
        return "20-29";
    else
        return "20-";
}
class Model()
{
    public string City { get; set; }
    public string Age { get; set; }
    public string Gender { get; set; }
    public string EducationLevel { get; set; }
    public string Occupation { get; set; }
    public string Choice { get; set; }
}
