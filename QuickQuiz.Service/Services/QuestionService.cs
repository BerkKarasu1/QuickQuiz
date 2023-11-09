using Mapster;
using MapsterMapper;
using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;
using QuickQuiz.Core.Repositories;
using QuickQuiz.Core.Services;

namespace QuickQuiz.Service.Services
{
    public class QuestionService : IQuestionService
    {
        readonly IQuestionRepository _questionRepository;
        readonly IMapper _mapper;
        public QuestionService(IQuestionRepository questionRepository, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(QuestionDTO questionDTO)
        {//todo: Buradaki Answer Mapleme işlemini düzenlemek gerekiyor: Yapıldı
            //Doğru Cevap gelmiyor.
            Question question = questionDTO.Adapt<Question>();
            await _questionRepository.AddAsync(question);
        }
        public async Task<List<QuestionDTO>> GetAllQuestionAsync(AppUser user)
        {
            List<Question> questions = await _questionRepository.GetAllQuestion(user);
            return _mapper.Map<List<QuestionDTO>>(questions);
        }

        public async Task<QuestionDTO> GetQuestionByIdAsync(int questionId)
        {
            Question question = await _questionRepository.FindQuestionByIdAsync(questionId);
            return question.Adapt<QuestionDTO>();
        }

        public void Remove(QuestionDTO questionDTO)
        {
            Question question = _mapper.Map<Question>(questionDTO);
            _questionRepository.Remove(question);
        }

        public void Update(QuestionDTO questionDTO)
        {
            Question question = _mapper.Map<Question>(questionDTO);
            _questionRepository.Update(question);
        }
    }
}
