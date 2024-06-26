﻿using System.Security.Cryptography;
using System.Text;
using WebApi.DataAccessLayer.Models;
using WebApi.DataAccessLayer.Repositories.Interfaces;
using WebApi.ServiceLayer.Interfaces;

namespace WebApi.ServiceLayer
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task<IEnumerable<User?>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task<User?> GetById(int id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<User?> GetByUserName(string username)
        {
            return await _userRepository.GetByUserName(username);
        }

        public async Task Update(int id,string username, string password, string email, string role)
        {
            var user = CreateUser(username, password, email, role);
            user.Id = id;
            await _userRepository.Update(user);
        }

        public async Task Create(User model)
        {
            await _userRepository.Create(model);
        }
        
        public async Task Delete(int id)
        {
            await _userRepository.Delete(id);
        }

        public async Task SignUp(string username, string password, string email,string role)
        {
            var usr = await _userRepository.GetByUserName(username);
            if (usr != null)
            {
                throw new InvalidOperationException("Username already exists. Please choose a different one.");
            }
            var user = CreateUser(username, password, email, role);
            await _userRepository.Create(user);
        }

        public async Task<User?> LogIn(string username, string password)
        {
            var user = await _userRepository.GetByUserName(username);
            return user is { PasswordSalt: not null, PasswordHash: not null } && !VerifyPassword(password, user.PasswordHash, user.PasswordSalt) ? null : user;
        }
        
        private bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }

        private User CreateUser(string username, string password, string email, string role)
        {
            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
            var user = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = email,
                Role = role
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
