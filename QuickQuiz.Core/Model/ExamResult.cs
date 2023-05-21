using QuickQuiz.Core.Model;

namespace QuickQuiz.Core.Models
{
    public class ExamResult
    {
        public int Id { get; set; }
        public Test Exam { get; set; }
        public float Result { get; set; }
        public AppUser Student { get; set; }
    }
}
