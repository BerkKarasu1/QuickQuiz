using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQuiz.Core.Repositories
{
    public interface IStatisticRepository
    {
        Task SetQuestionStatistics(Dictionary<int, bool> answerStatistics);
    }
}
