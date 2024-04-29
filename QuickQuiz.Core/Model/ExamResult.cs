using QuickQuiz.Core.Model;

namespace QuickQuiz.Core.Models
{
    public class ExamResult
    {
        public int Id { get; set; }
        public Test Exam { get; set; }
        public DateTime? StartTime { get; set; } 
        public DateTime? EndTime { get; set; } 
        public float? Result { get; set; }
        public AppUser? Student { get; set; }
        public string? VisitorName { get; set; }
        public byte? ExamRating { get; set; }
    }
}
