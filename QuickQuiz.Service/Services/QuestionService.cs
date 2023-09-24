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
        {

            Question question = new()
            {
                Quest = questionDTO.Question,
                Creater = questionDTO.Creater
            };
            List<Answer> answers = new();
            Question question1 = questionDTO.Adapt<Question>();//Quest iletilmiyor
            foreach (var answer in questionDTO.Answers)
            {
                if (answer != null && answer.AnswerText != string.Empty && answer.AnswerText != null)
                {
                    Answer answerModel = answer.Adapt<Answer>();
                    if (answer.AnswerText == questionDTO.TrueAnswer.AnswerText)
                        answerModel.IsCorrect = true;
                    answers.Add(answerModel);
                }
            }
            question.Answers = answers;
            await _questionRepository.AddAsync(question);
        }
        //todo
        public async Task<List<QuestionDTO>> GetAllQuestionAsync(AppUser user)
        {
            List<Question> questions = await _questionRepository.GetAllQuestion(user);
            List<QuestionDTO> questionDTOs = new();
            try
            {

            var asds= _mapper.Map<List<QuestionDTO>>(questions);
            }
            catch (Exception e)
            {

                throw;
            }
            foreach (var item in questions)
            {
                questionDTOs.Add(new QuestionDTO
                {
                    Id = item.Id,
                    Answers = item.Answers,
                    Creater = item.Creater,
                    Question = item.Quest,
                });
            }
            return questionDTOs;
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
