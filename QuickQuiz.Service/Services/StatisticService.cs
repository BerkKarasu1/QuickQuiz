using Mapster;
using QuickQuiz.Core.Dtos;
using QuickQuiz.Core.Model;
using QuickQuiz.Core.Models;
using QuickQuiz.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQuiz.Service.Services
{
    public class StatisticService : IStatisticService
    {
        readonly IStatisticRepository _statisticRepository;
        readonly IQuestionRepository _questionRepository;
        readonly ITestRepository _testRepository;
        readonly IResultRepository _resultRepository;
        public StatisticService(IStatisticRepository statisticRepository, IQuestionRepository questionRepository, ITestRepository testRepository, IResultRepository resultRepository)
        {
            _statisticRepository = statisticRepository;
            _questionRepository = questionRepository;
            _testRepository = testRepository;
            _resultRepository = resultRepository;
        }

        public async Task SetQuestionStatistics(Dictionary<int, bool> answerStatistics) => await _statisticRepository.SetQuestionStatistics(answerStatistics);
        /// <summary>
        /// First returned value Correct Answer
        /// Second returned value Incorrect Answer
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<(QuestionDTO?, QuestionDTO?)> GetMostCorrectlyAnsweredAndInCorrectlyQuestion(AppUser user)
        {
            List<Question> questions = await _questionRepository.GetAllQuestionsWithStatistic(user);
            Question mostCorrectlyResult = null;
            Question mostInCorrectlyResult = null;
            float mostCorrectlyPoint = -1;
            float mostInCorrectlyPoint = -1;
            foreach (var question in questions)
            {
                if (question.Statistic is null || (question.Statistic.CorrectAnswerCount + question.Statistic.InCorrectAnswerCount) == 0) continue;
                float correctPercent = question.Statistic.CorrectAnswerCount / (question.Statistic.CorrectAnswerCount + question.Statistic.InCorrectAnswerCount);
                float inCorrectPercent = 1 - correctPercent;
                if (correctPercent > mostCorrectlyPoint)
                {
                    mostCorrectlyPoint = correctPercent;
                    mostCorrectlyResult = question;
                }
                else if (inCorrectPercent < mostInCorrectlyPoint)
                {
                    mostInCorrectlyPoint = inCorrectPercent;
                    mostInCorrectlyResult = question;
                }
                if (mostInCorrectlyPoint == -1)
                {
                    mostInCorrectlyPoint = inCorrectPercent;
                    mostInCorrectlyResult = question;
                }
            }
            if (mostCorrectlyResult != null && mostInCorrectlyResult != null)
                return (mostCorrectlyResult?.Adapt<QuestionDTO>(), mostInCorrectlyResult?.Adapt<QuestionDTO>());
            return (null,null);
        }
        //public async Task<TestDTO> GetTestWithMaxSuccessRate(AppUser user)
        //{
        //    IEnumerable<Test> allTest = await _testRepository.GetAllTest(user);
        //}
        public async Task<(TestDTO?, float)> GetMostPopularTest(AppUser user)
        {
            IEnumerable<Test> allTest = await _testRepository.GetAllTest(user);
            Test? mostLikedTest = null;
            float mostPopularity = 0;
            foreach (var test in allTest)
            {
                int totalPoint = 0;
                int totalUser = 0;
                IEnumerable<ExamResult> examResults = await _resultRepository.GetAllResultByTestIdAsync(test.Id);
                foreach (var result in examResults)
                {
                    if (result.ExamRating > 0)
                    {
                        totalPoint += (int)result.ExamRating;
                        totalUser++;
                    }
                }
                float popularity = (float)totalPoint / totalUser;
                if (popularity > mostPopularity && totalUser > 3)
                {
                    mostPopularity = popularity;
                    mostLikedTest = test;
                }
            }
            return (mostLikedTest?.Adapt<TestDTO>(), mostPopularity);
        }
        public async Task<(Dictionary<TestCategorys, int>, int)> GetTestCountByCategory(AppUser user)
        {

            List<Test> allTest = await _testRepository.GetAllTest(user);
            Dictionary<TestCategorys, int> testCategoriesCount = new();
            foreach (var test in allTest)
            {
                if (testCategoriesCount.TryGetValue(test.TestCategorys, out int count))
                    testCategoriesCount[test.TestCategorys] = ++count;
                else
                    testCategoriesCount.Add(test.TestCategorys, 1);
            }
            return (testCategoriesCount.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value), allTest.Count);
        }
    }
}
