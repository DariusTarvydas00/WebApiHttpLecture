
﻿using WebApi.Controllers;
using WebApi.DataAccessLayer.Models;


namespace WebApi.ServiceLayer;

public interface IUserService
{
    ICollection<ReviewModel> GetReviews(string username);
    UserModel GetById(int id);
    IEnumerable<UserModel> GetAll();
    UserModel GetByUsername(string username);
    IEnumerable<ReviewModel> GetUserReviews(int userId);
    UserModel Create(RegisterRequestDto model);
    void Update(int id, RegisterRequestDto model);
    void Delete(int id);
}