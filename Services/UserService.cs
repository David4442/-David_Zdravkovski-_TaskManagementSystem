using Exam.Models;
using Exam.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Exam.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly JwtService _jwtService;

        public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, JwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.Login(email);
            if (user == null)
            {
                return null;
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Failed ? null : user;
        }

        public async Task RegisterUserAsync(UserRegisterDto userDto)
        {
            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                Role = "User"
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, userDto.Password);
            await _userRepository.Register(user);
        }
    }
}
