using QuickQuiz.Core.Model;
using QuickQuiz.Core.Repositories;

namespace QuickQuiz.Repository.Repositories
{
    public class StatisticRepository : IStatisticRepository
    {
        AppDbContext _context;
        public StatisticRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task SetQuestionStatistics(Dictionary<int, bool> answerStatistics)
        {
            foreach (var answerStat in answerStatistics)
            {
                QuestionStatistic? questionStatistic = _context.QuestionStatistics.FirstOrDefault(x => x.Question.Id == answerStat.Key);

                if (questionStatistic is null)
                {
                    Question? question = _context.Questions.FirstOrDefault(x => x.Id == answerStat.Key);
                    if (question is null)
                        break;
                    questionStatistic = new QuestionStatistic() { Question = question };
                    AddStat(answerStat.Value, questionStatistic);
                    _context.Add(questionStatistic);
                }
                else
                {
                    AddStat(answerStat.Value, questionStatistic);
                    _context.Update(questionStatistic);
                }
            }
            await _context.SaveChangesAsync();

            static void AddStat(bool isCorrect, QuestionStatistic? questionStatistic)
            {
                if (isCorrect)
                    questionStatistic!.CorrectAnswerCount++;
                else
                    questionStatistic!.InCorrectAnswerCount++;
            }
        }
    }
}
