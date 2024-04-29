using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;
using QuickQuiz.Core.Repositories;
using QuickQuiz.Core.Services;
using System.Text;

namespace QuickQuiz.Service.Services
{
    public class TestService : ITestService
    {
        readonly ITestRepository _testRepository;
        readonly IQuestionRepository _questionRepository;
        readonly IResultService _resultService;
        readonly IMapper _mapper;
        public TestService(ITestRepository testRepository, IQuestionRepository questionRepository, IResultService resultService, IMapper mapper)
        {
            _testRepository = testRepository;
            _questionRepository = questionRepository;
            _resultService = resultService;
            _mapper = mapper;
        }
        public async Task AddAsync(TestDTO testDTO, AppUser currentUser)
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
            if (questionList.Count == 0)
                return;
            testDTO.Question = null;
            var test = _mapper.Map<Test>(testDTO);
            test.Creater = currentUser;
            test.Question = questionList;
            await _testRepository.AddAsync(test);
        }

        public async Task<List<TestDTO>> GetAllTest(AppUser user)
        {
            var allTest = await _testRepository.GetAllTest(user);
            return _mapper.Map<List<TestDTO>>(allTest);
        }

        public async Task<TestDTO> GetTestById(int id)
        {
            var test = await _testRepository.GetTestById(id);
            return test.Adapt<TestDTO>();
        }
        public void Remove(TestDTO testDTO)
        {
            _testRepository.Remove(new Test() { Id = testDTO.Id });
        }

        public async Task<TestDTO> TestControl(AppUser user, int id)
        {
            var item = await _testRepository.GetTestById(id);
            return item.Adapt<TestDTO>();
        }

        public async Task Update(TestDTO testDTO)
        {
            if (testDTO.PictureFile != null && testDTO.PictureFile.Length > 0)
            {
                using MemoryStream ms = new();
                testDTO.PictureFile.CopyTo(ms);
                byte[] imageBytes = ms.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);
                //var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");
                //var randomFileName = $"{Guid.NewGuid()}{Path.GetExtension(testDTO.PictureFile.FileName)}";
                //var newPicturePath = Path.Combine(wwwrootFolder!.First(x => x.Name == "TestPictures").PhysicalPath!, randomFileName);
                //using var stream = new FileStream(newPicturePath, FileMode.Create);
                //await testDTO.PictureFile.CopyToAsync(stream);
                //testDTO.PictureUrl = randomFileName;
                testDTO.PictureUrl = base64String;
            }
            var test = await _testRepository.GetTestById(testDTO.Id);
            test.Name = testDTO.Name;
            if (testDTO.PictureUrl is not null)
                test.PictureUrl = testDTO.PictureUrl;
            _testRepository.Update(test);
        }
        public async Task<bool> Result(TestDTO testDTO, AppUser? curruntUser, string visitorName,string rating)
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
                if (curruntUser != null)
                    await _resultService.AddAsync(curruntUser, score, test,rating);
                else
                    await _resultService.AddVisitorAsync(visitorName, score, test, rating);
                return true;
            }
            return false;
        }
        public async Task SetStartTimeAsync(AppUser user, TestDTO testDTO)
        {
            var test = await _testRepository.GetTestById(testDTO.Id);
            await _resultService.SetStartTimeAsync(user, test);
        }

        public async Task<List<TestDTO>> GetAllTestAsync()
        {
            var allTest = await _testRepository.GetAllTest();
            return _mapper.Map<List<TestDTO>>(allTest);
        }
        public async Task<(string[]?, bool)> TestLinkControl(string link)
        {
            byte[] decodeBytes;
            string decodedText = string.Empty;
            if (link == null) return (null, false);
            try
            {
                decodeBytes = Convert.FromBase64String(link);
                decodedText = Encoding.UTF8.GetString(decodeBytes);
            }
            catch (Exception)
            {
                return (null, false);
            }
            string[] result = decodedText.Split('/');
            if (result.Length != 2)
                return (null, false);
            else
            {
                if (!int.TryParse(result[1], out int id))
                    return (null, false);

                TestDTO test = await GetTestById(id);
                if (test != null && test.Creater?.UserName == result[0].ToString())
                    return (result, true);
                else return (null, false);
            }
        }
    }
}
