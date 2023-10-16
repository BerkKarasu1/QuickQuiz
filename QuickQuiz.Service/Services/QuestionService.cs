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
        {//todo: Buradaki Answer Mapleme işlemini düzenlemek gerekiyor
            Question question = questionDTO.Adapt<Question>();
            List<Answer> answerList = new();
            foreach (var answer in question.Answers)
            {
                if (answer != null && answer.AnswerText != string.Empty && answer.AnswerText != null)
                    answer.Question = question;
                else
                    answerList.Add(answer);
            }
            foreach (var item in answerList)
                question.Answers.Remove(item);
            await _questionRepository.AddAsync(question);
        }
        //todo
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
