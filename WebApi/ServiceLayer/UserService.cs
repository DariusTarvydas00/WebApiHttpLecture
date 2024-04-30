using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApi.DataAccessLayer.Models;
using WebApi.DataAccessLayer.Repositories.Interfaces;
using WebApi.ServiceLayer.Interfaces;
using WebApi.ServiceLayer.JwtLayer;

namespace WebApi.ServiceLayer
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public UserService(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }
        
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task<User> GetById(int id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<User> GetByUserName(string username)
        {
            return await _userRepository.GetByUserName(username);
        }

        public async Task Create(User model)
        {
            await _userRepository.Create(model);
        }

        public async Task Update(User model)
        {
            await _userRepository.Update(model);
        }
        
        public async Task Delete(int id)
        {
            await _userRepository.Delete(id);
        }

        public async Task SignUp(string username, string password)
        {
            var usr = await _userRepository.GetByUserName(username);
            if (usr != null)
            {
                throw new InvalidOperationException("Username already exists. Please choose a different one.");
            }
            var user = CreateUser(username, password);
            await _userRepository.Create(user);
        }

        public async Task<User?> LogIn(string username, string password)
        {
            var user = await _userRepository.GetByUserName(username);
            if (user == null || !VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            return user;
        }
        
        private bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }

        private User CreateUser(string username, string password)
        {
            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
            var user = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };
            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}
