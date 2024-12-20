﻿namespace QuickQuiz.Core.Services
{
    public interface IEmailService
    {
        Task SendResetPasswordEmail(string resetPasswordEmailLink, string ToEmail);
        Task SendAccountConfirmEmail(string url, string userName, string ToEmail);
    }
}