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
        {
            Question question = _mapper.Map<Question>(questionDTO);
            await _questionRepository.AddAsync(question);
        }

        public async Task<List<Question>> GetAllQuestionAsync(AppUser user)
        {
           return await _questionRepository.GetAllQuestion(user);
        }

        public async Task<Question> GetQuestionByIdAsync(int questionId)
        {
          return  await _questionRepository.FindQuestionByIdAsync(questionId);
        }

        public  void Remove(QuestionDTO questionDTO)
        {
            Question question = _mapper.Map<Question>(questionDTO);
             _questionRepository.RemoveAsync(question);
        }

        public void Update(QuestionDTO questionDTO)
        {
            Question question = _mapper.Map<Question>(questionDTO);
            _questionRepository.UpdateAsync(question);
        }
    }
}
