
using Mapster;
using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;
using System.ComponentModel;
using System.Reflection;

namespace Exams.Service.Mapping
{
    public class QuestionMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
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
                .Ignore(x => x.PictureFile)
                .IgnoreNullValues(true);
            config.NewConfig<TestDTO, Test>()
                .Ignore()
                .IgnoreNullValues(true);
        }
    }
}
