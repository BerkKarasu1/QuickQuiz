
using Mapster;
using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;

namespace Exams.Service.Mapping
{
    public class QuestionMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<AppUser, UserEditViewModel>().Ignore(
                dest => dest.Picture).Map(dest => dest.Phone, src => src.PhoneNumber);
            config.NewConfig<UserEditViewModel, AppUser>().Ignore(
               dest => dest.Picture).Map(dest => dest.PhoneNumber, src => src.Phone);
            config.NewConfig<AppUser, UserDTO>()
                .Map(dest => dest.PictureUrl, src => src.Picture);
            config.NewConfig<UserDTO, AppUser>()
                .Map(dest => dest.Picture, src => src.PictureUrl);
            config.NewConfig<QuestionDTO, Question>().Map(dest => dest.Quest, src => src.Question);
            //    .BeforeMapping((src, dest) =>
            //{
            //    // Özel kontrolü burada gerçekleştirin
            //    if (src.Answers != null)
            //    {
            //        dest.Answers = src.Answers
            //                             .Select(answerDTO => answerDTO.Adapt<Answer>(config))
            //                             .Where(answer => !string.IsNullOrEmpty(answer.AnswerText))
            //                             .ToList();
            //    }
            //}); 
            config.NewConfig<Question, QuestionDTO>().Map(dest => dest.Question, src => src.Quest);


            config.NewConfig<Answer, AnswerDTO>()
                .IgnoreNullValues(true);
                
            config.NewConfig<AnswerDTO, Answer>()
                .Map(dest => dest.AnswerText, src => !string.IsNullOrEmpty(src.AnswerText) ? src.AnswerText : null)
    .AfterMapping((src, dest) =>
    {
        if (string.IsNullOrEmpty(dest.AnswerText))
        {
            dest = null; // Boş veya null AnswerText'e sahip AnswerDTO'ları mapleme
        }
    }).IgnoreNullValues(true);

            //todo:
            //config.NewConfig<Answer, AnswerDTO>().Map(
            //    dest => dest.AnswerText, src => src.AnswerText,
            //    dest => dest.IsCorrect, src => src.AnswerText,

            //    );
            //        config.NewConfig<QuestionDTO, Question>().Map(dest => dest.Tests, src => src.Tests).IgnoreNullValues(true);
            //        config.NewConfig<Question, QuestionDTO>().Map(dest => dest.Id, src => src.Id)
            //            .Map(dest => dest.Question, src => src.Quest)
            //            .Map(dest => dest.Answers, src => src.Answers)
            //            .Map(dest => dest.TrueAnswer, src => src.Answers.FirstOrDefault(answer => answer.IsCorrect))
            //            .Map(dest => dest.Tests, src => src.Tests)
            //            .Map(dest => dest.Creater, src => src.Creater)
            //            .Map(dest => dest.Check, src => false)
            //            .Map(dest => dest.TestName, src => string.Empty)
            //            .IgnoreNullValues(true);
            //        config.ForType<Question, QuestionDTO>()
            //             .Ignore(dest => dest.Question) // Döngüsel referansı önlemek için Question özelliğini yoksay
            //.Ignore(dest => dest.Answers);
            //        config.ForType<Question, QuestionDTO>()
            //.Map(dest => dest.Question, src => src.Quest, (dest, src) => src);

            //config.NewConfig<List<QuestionDTO>, List<Question>>().IgnoreNullValues(true);
            //config.NewConfig<Question, QuestionDTO>().IgnoreIgnoreNullValues(true);            
        }
    }
}
