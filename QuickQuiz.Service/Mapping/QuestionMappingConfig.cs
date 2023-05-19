
using Mapster;
using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;

namespace Exams.Service.Mapping
{
    public class QuestionMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<QuestionDTO, Question>().IgnoreNullValues(true);
            config.NewConfig<List<QuestionDTO>,List<Question>>().IgnoreNullValues(true);
            //config.NewConfig<Question, QuestionDTO>().IgnoreIgnoreNullValues(true);            
        }
    }
}
