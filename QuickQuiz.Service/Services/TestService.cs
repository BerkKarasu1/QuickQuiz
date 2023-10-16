using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;
using QuickQuiz.Core.Repositories;
using QuickQuiz.Core.Services;

namespace QuickQuiz.Service.Services
{
    public class TestService : ITestService
    {
        readonly ITestRepository _testRepository;
        readonly IQuestionRepository _questionRepository;
        readonly IResultService _resultService;
        public TestService(ITestRepository testRepository, IQuestionRepository questionRepository, IResultService resultService)
        {
            _testRepository = testRepository;
            _questionRepository = questionRepository;
            _resultService = resultService;
        }
        public async Task AddAsync(TestDTO testDTO)
        {
            List<Question> questionList = new();
            foreach (var question in testDTO.Question)
            {
                if (question.Check)
                {
                    var quest = await _questionRepository.FindQuestionByIdAsync(question.Id);
                    questionList.Add(quest);
                }
            }
            Test test = new()
            {
                Name = testDTO.Name,
                //Creater = testDTO.Creater,
                Question = questionList,
                PictureUrl = testDTO.PictureUrl,

            };
            await _testRepository.AddAsync(test);
        }

        public async Task<List<TestDTO>> GetAllTest(AppUser user)
        {
            List<TestDTO> testList = new();
            var allTest = await _testRepository.GetAllTest(user);
            foreach (var item in allTest)
            { //todo:
              //                TestDTO test = new() { Name = item.Name, Creater = item.Creater, PictureUrl = item.PictureUrl, Id = item.Id };
                TestDTO test = new() { Name = item.Name,  PictureUrl = item.PictureUrl, Id = item.Id };
                List<QuestionDTO> questionDTO = new();
                for (int i = 0; i < test.Question.Count; i++)
                {
                    questionDTO.Add(new QuestionDTO
                    {
                        Id = item.Question[i].Id,
                        Answers = test.Question[i].Answers,
                        Check = test.Question[i].Check,
                        TrueAnswer = test.Question[i].TrueAnswer
                    });
                }
                test.Question = questionDTO;
                testList.Add(test);

            }
            return testList;
        }

        public async Task<TestDTO> GetTestById(int id)
        {
            var test = await _testRepository.GetTestById(id);
            //todo:
            TestDTO testDTO = new() { Name = test.Name, PictureUrl = test.PictureUrl, Id = test.Id };
            List<QuestionDTO> questionDTOs = new();
            for (int i = 0; i < test.Question.Count; i++)
            {
                QuestionDTO questionDTO = new()
                {
                    Id = test.Question[i].Id,
                    Question = test.Question[i].Quest,
                    //Answers = test.Question[i].Answers,
                };
                foreach (var item2 in test.Question[i].Answers)
                {
                    if (item2.IsCorrect)
                    {
                        //questionDTO.TrueAnswer = item2;
                        break;
                    }
                }
                questionDTOs.Add(questionDTO);
            }
            testDTO.Question = questionDTOs;
            return testDTO;

        }
        public void Remove(TestDTO testDTO)
        {
            _testRepository.Remove(new Test() { Id = testDTO.Id });
        }

        public async Task<TestDTO> TestControl(AppUser user, int id)
        {
            var item = await _testRepository.GetTestById(id);
            if (item == null || item.Creater.Id != user.Id) return null;
            TestDTO test = new() { Name = item.Name, PictureUrl = item.PictureUrl, Id = item.Id };
            List<QuestionDTO> questionDTOs = new();
            for (int i = 0; i < item.Question.Count; i++)
            {
                QuestionDTO questionDTO = new()
                {
                    Id = item.Question[i].Id,
                    //Answers = item.Question[i].Answers,
                };
                foreach (var item2 in item.Question[i].Answers)
                {
                    if (item2.IsCorrect)
                    {
                        //questionDTO.TrueAnswer = item2;
                        break;
                    }
                }
                questionDTOs.Add(questionDTO);
            }
            test.Question = questionDTOs;
            return test;
        }

        public async Task Update(TestDTO testDTO)
        {
            var test = await _testRepository.GetTestById(testDTO.Id);
            test.PictureUrl = testDTO.PictureUrl;
            test.Name = testDTO.Name;
            _testRepository.Update(test);
        }
        public async Task<bool> Result(TestDTO testDTO, AppUser curruntUser)
        {
            var test = await _testRepository.GetTestById(testDTO.Id);
            if (test != null)
            {
                int CorrectAnswer = 0;
                int WrongAnswer = 0;
                foreach (var testQuest in test.Question)
                {
                    foreach (var dtoQuest in testDTO.Question)
                    {
                        if (testQuest.Id == dtoQuest.Id)
                        {
                            foreach (var answer in testQuest.Answers)
                            {
                                if (answer.AnswerText == dtoQuest.TrueAnswer.AnswerText)
                                {
                                    if (answer.IsCorrect)
                                        CorrectAnswer++;
                                    else
                                        WrongAnswer++;
                                }
                            }
                        }
                    }
                }
                float score = (float.Parse(CorrectAnswer.ToString()) / (float.Parse(WrongAnswer.ToString()) + float.Parse(CorrectAnswer.ToString()))) * 100;

                await _resultService.AddAsync(curruntUser, score, test);
                return true;
            }
            return false;
        }

        public async Task<List<TestDTO>> GetAllTestAsync()
        {
            List<TestDTO> testList = new();
            var allTest = await _testRepository.GetAllTest();
            foreach (var item in allTest)
            {
                TestDTO test = new() { Name = item.Name, PictureUrl = item.PictureUrl, Id = item.Id };
                List<QuestionDTO> questionDTO = new();
                for (int i = 0; i < test.Question.Count; i++)
                {
                    questionDTO.Add(new QuestionDTO
                    {
                        Id = item.Question[i].Id,
                        Answers = test.Question[i].Answers,
                        Check = test.Question[i].Check,
                        TrueAnswer = test.Question[i].TrueAnswer,
                    });
                }
                test.Question = questionDTO;
                testList.Add(test);
            }
            return testList;
        }
    }
}
