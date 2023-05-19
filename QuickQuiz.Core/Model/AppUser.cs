﻿using Microsoft.AspNetCore.Identity;

namespace QuickQuiz.Core.Model
{
    public class AppUser : IdentityUser
    {
        public string? City { get; set; }
        public string? Picture { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }
        public List<Question>? Question { get; set; }
    }
}