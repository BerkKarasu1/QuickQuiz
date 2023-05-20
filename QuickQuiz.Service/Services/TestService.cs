using Mapster;
using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;
using QuickQuiz.Core.Repositories;
using QuickQuiz.Core.Services;
using QuickQuiz.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace QuickQuiz.Service.Services
{
    public class TestService : ITestService
    {
        readonly ITestRepository _testRepository;
        readonly IQuestionRepository _questionRepository;
        public TestService(ITestRepository testRepository, IQuestionRepository questionRepository)
        {
            _testRepository = testRepository;
            _questionRepository = questionRepository;
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
                Creater = testDTO.Creater,
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
            {
                TestDTO test = new() { Name = item.Name, Creater = item.Creater, PictureUrl = item.PictureUrl, Id = item.Id };
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

        public void Remove(TestDTO testDTO)
        {
            _testRepository.Remove(new Test() { Id = testDTO.Id });
        }

        public async Task<TestDTO> TestControl(AppUser user, int id)
        {
            var item = await _testRepository.GetTestById(id);
            if (item == null || item.Creater.Id != user.Id) return null;
            TestDTO test = new() { Name = item.Name, Creater = item.Creater, PictureUrl = item.PictureUrl, Id = item.Id };
            List<QuestionDTO> questionDTOs = new();
            for (int i = 0; i < item.Question.Count; i++)
            {
                QuestionDTO questionDTO =new ()
                {
                    Id = item.Question[i].Id,
                    Answers = item.Question[i].Answers,
                };
                foreach (var item2 in item.Question[i].Answers)
                {
                    if(item2.IsCorrect)
                    {
                        questionDTO.TrueAnswer = item2;
                        break;
                    }
                }
                questionDTOs.Add(questionDTO);
            }
            test.Question = questionDTOs;
            return test;
        }

        public async Task Update(TestDTO TestDTO)
        {
            var test = await _testRepository.GetTestById(TestDTO.Id);
            test.PictureUrl = TestDTO.PictureUrl;
            test.Name = TestDTO.Name;
            _testRepository.Update(test);
        }
    }
}
