﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace QuickQuiz.Core.Model
{
    public class Question
    {
        public int Id { get; set; }
        public string Quest { get; set; }
        public List<Answer> Answers { get; set; } =new List<Answer>();
        public AppUser Creater { get; set; }
        public List<Test>? Tests { get; set; }=new List<Test>();
    }
    public class Answer
    {
        public int Id { get; set; }
        public Question Question { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
    }
}
