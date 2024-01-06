using Newtonsoft.Json.Linq;
using QuickQuiz.Core.Model;
using QuickQuiz.Repository;
using System.Net;
using System.Text;

AppDbContext dbContext = new();
List<Question> quest = new();
string file = File.ReadAllText("C:\\Users\\odya\\source\\repos\\BerkKarasu1\\QuickQuiz\\AddCustomTest\\test.json");
JObject json = JObject.Parse(file);
string title = (string)json["testBasligi"];
if (string.IsNullOrEmpty(title))
{
    foreach (var property in json.Properties())
    {
        if (property.Value["Sorular"] != null)
        {
            title = property.Name;
            break;
        }
    }
}
JArray questions = (JArray)json["Sorular"];
bool questionCheck = false;
foreach (var item in questions)
{
    List<Answer> ans = new (){
    
    new()
    {
        AnswerText = item["Secenekler"]["a"].ToString().Replace(" (Doğru Cevap)", "").Trim(),
        IsCorrect = item["DogruCevap"].ToString().Equals("a")
    },
        new()
        {
            AnswerText = item["Secenekler"]["b"].ToString().Replace(" (Doğru Cevap)", "").Trim(),
            IsCorrect = item["DogruCevap"].ToString().Equals("b"),            
        },
        new()
        {
            AnswerText = item["Secenekler"]["c"].ToString().Replace(" (Doğru Cevap)", "").Trim(),
            IsCorrect = item["DogruCevap"].ToString().Equals("c")
        },
        new()
        {
            AnswerText = item["Secenekler"]["d"].ToString().Replace(" (Doğru Cevap)", "").Trim(),
            IsCorrect = item["DogruCevap"].ToString().Equals("d")
        }
    };
    if (item["Secenekler"]["e"] != null)
    {
        ans.Add(new()
        {
            AnswerText = item["Secenekler"]["e"].ToString().Replace(" (Doğru Cevap)", "").Trim(),
            IsCorrect = item["DogruCevap"].ToString().Equals("e")
        });
    }
    if (!(item["DogruCevap"].ToString().Equals("a") || item["DogruCevap"].ToString().Equals("b") || item["DogruCevap"].ToString().Equals("c") || item["DogruCevap"].ToString().Equals("d") || item["DogruCevap"].ToString().Equals("e")))
    {
        throw new Exception();
    }
    if (item["SoruMetni"].ToString().StartsWith("1."))
        questionCheck = true;

    quest.Add(
        new()
        {
            Quest = questionCheck ? item["SoruMetni"].ToString()[2..].Trim() : item["SoruMetni"].ToString().Trim(),
            Answers = ans,
            Creater= dbContext.Users.FirstOrDefault(x => x.UserName == "berkkarasu")
        });
}
Test test = new()
{
    Creater = dbContext.Users.FirstOrDefault(x => x.UserName == "berkkarasu"),
    TestCategorys = TestCategorys.Coğrafya_ve_Ülkeler,
    Question = quest,
    Name = title,
    PictureUrl = ConvertImageURLToBase64("https://image.posta.com.tr/i/posta/75/0x0/622f7d31932151938c7de8b5.jpg")
};
dbContext.Tests.Add(test);
dbContext.SaveChanges();

String ConvertImageURLToBase64(String url)
{
    StringBuilder _sb = new();

    Byte[] _byte = GetImage(url);

    _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));

    return _sb.ToString();
}

byte[] GetImage(string url)
{
    Stream stream = null;
    byte[] buf;

    try
    {
        //WebProxy myProxy = new ();
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

        HttpWebResponse response = (HttpWebResponse)req.GetResponse();
        stream = response.GetResponseStream();

        using (BinaryReader br = new (stream))
        {
            int len = (int)(response.ContentLength);
            buf = br.ReadBytes(len);
            br.Close();
        }

        stream.Close();
        response.Close();
    }
    catch (Exception exp)
    {
        buf = null;
    }

    return (buf);
}