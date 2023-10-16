using Microsoft.AspNetCore.Identity;
using QuickQuiz.Core.Dtos;

namespace QuickQuiz.Core.Services
{
    public interface IMemberService
    {
        Task<UserDTO> GetUserViewModelByUserNameAsync(string userName);
        Task<List<UserDTO>> GetUserViewModelBySearchedAsync(string userName);
        Task LogoutAsync();
        Task<bool> CheckPasswordAsync(string userName, string password);
        Task<(bool, IEnumerable<IdentityError>?)> ChangePasswordAsync(string userName, string oldPassword, string newPassword);
        Task<UserEditViewModel> GetUserEditViewModelAsync(string userName);
        Task<(bool, IEnumerable<IdentityError>?)> EditUserAsync(UserEditViewModel request, string userName);
        
    }
}