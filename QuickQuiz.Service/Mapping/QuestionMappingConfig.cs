
using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Web;

namespace Exams.Service.Mapping
{
    public class QuestionMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            string site = string.Empty;
#if DEBUG
            site = "https://localhost:7147";
#else
            site = "https://app.quizck.com";
#endif

            config.NewConfig<AppUser, UserEditViewModel>()
                .Ignore(dest => dest.Picture)
                .Map(dest => dest.Phone, src => src.PhoneNumber);
            config.NewConfig<UserEditViewModel, AppUser>()
                .Ignore(dest => dest.Picture)
                .Map(dest => dest.PhoneNumber, src => src.Phone);
            config.NewConfig<AppUser, UserDTO>()
                .Map(dest => dest.PictureUrl, src => src.Picture);
            config.NewConfig<UserDTO, AppUser>()
                .Map(dest => dest.Picture, src => src.PictureUrl);
            config.NewConfig<QuestionDTO, Question>()
                .Map(dest => dest.Quest, src => src.Question)
                .Map(dest => dest.Answers, src => src.Answers.Where(a => !string.IsNullOrWhiteSpace(a.AnswerText)).ToList())
                .AfterMapping(act => act.Answers.ForEach(x => x.Question = act));
            config.NewConfig<Question, QuestionDTO>()
                .Map(dest => dest.Question, src => src.Quest)
                .Map(dest => dest.Answers, src => src.Answers.Where(a => !string.IsNullOrWhiteSpace(a.AnswerText)).ToList())
                .MaxDepth(2);


            config.NewConfig<Answer, AnswerDTO>()
                .IgnoreNullValues(true);
            config.NewConfig<AnswerDTO, Answer>()
                .IgnoreNullValues(true);
            //testCategory.GetType().GetField(testCategory.ToString())?.GetCustomAttributes<DescriptionAttribute>().FirstOrDefault()?.Description
            config.NewConfig<Test, TestDTO>()
                .Map(dest=>dest.TestCategoryDescription,src =>src.TestCategorys.GetType().GetField(src.TestCategorys.ToString()).GetCustomAttributes<DescriptionAttribute>().FirstOrDefault().Description)
                .Map(dest=>dest.Link,src=>$"{site}/quiz/visitor/{Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(src.Creater!=null? src.Creater.UserName + "/"
                + src.Id.ToString() : "UserNotFound" ))}")
                .Map(dest => dest.PictureUrl,src=> src.PictureUrl ?? Convert.ToBase64String(File.ReadAllBytes("wwwroot/defaulttest.png")))
                .Ignore(x => x.PictureFile)
                .IgnoreNullValues(true);
            config.NewConfig<TestDTO, Test>()
                .Ignore()
                .IgnoreNullValues(true);

        }
    }
}
