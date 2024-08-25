using Exam.Models;

namespace Exam.Repositories
{
    public interface IUserRepository
    {
        Task<User> Login(string email);
        Task Register(User user);
    }
}
