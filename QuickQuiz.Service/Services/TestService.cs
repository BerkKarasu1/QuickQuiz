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
            var allTest = await _testRepository.GetAllTest();
            return _mapper.Map<List<TestDTO>>(allTest);
        }
        public async Task<(bool, int)> TestLinkControl(string link)
        {
            byte[] decodeBytes;
            string decodedText = string.Empty;
            int id = 0;
            try
            {

                decodeBytes = Convert.FromBase64String(link);
                decodedText = Encoding.UTF8.GetString(decodeBytes);
            }
            catch (Exception)
            {
                return (false, id);
            }
            string[] linkInfo = decodedText.Split("_");

            if (linkInfo.Length == 2)
            {
                try
                {
                    id = int.Parse(linkInfo[1]);
                }
                catch
                {
                    return (false, id);
                }
                Test test = await _testRepository.GetTestById(id);
                if (test != null && test.Creater.UserName == linkInfo[0])
                    return (true, id);
            }
            return (false, id);
        }
    }
}
