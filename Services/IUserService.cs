using Exam.Models;

namespace Exam.Services
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string email, string password);
        Task RegisterUserAsync(UserRegisterDto userDto);
    }
}
