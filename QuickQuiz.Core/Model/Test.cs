using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQuiz.Core.Model
{
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AppUser Creater { get; set; }
        public List<Question>? Question { get; set; }
        public TestCategorys TestCategorys { get; set; }
        public string? PictureUrl { get; set; }
    }
}
