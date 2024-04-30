using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DataAccessLayer.Models;
using WebApi.DataAccessLayer.Repositories.Interfaces;

namespace WebApi.DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MainDbContext _userContext;

        public UserRepository(MainDbContext userContext)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userContext.Users.ToListAsync();
        }

        public async Task<User?> GetById(int id)
        {
            return await _userContext.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<User?> GetByUserName(string username)
        {
            return await _userContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<Review>> GetUserReviews(int userId)
        {
            // Implement logic to retrieve user reviews from the database
            return await _userContext.Reviews.Where(r => r.UserId == userId).ToListAsync();
        }

        public async Task Create(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _userContext.Users.Add(user);
            await _userContext.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _userContext.Users.Update(user);
            await _userContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var userToDelete = await _userContext.Users.FindAsync(id);
            if (userToDelete != null)
            {
                _userContext.Users.Remove(userToDelete);
                await _userContext.SaveChangesAsync();
            }
        }
    }
}
