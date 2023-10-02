using DataAcces.Models;

namespace DataAcces.Services;

public class UserService:IUserService
{
    public Task<string> CreateUser()
    {
        throw new NotImplementedException();
    }

    public Task<TelegramUser> GetUsers()
    {
        throw new NotImplementedException();
    }

    public Task<TelegramUser> GetUserById(long Id)
    {
        throw new NotImplementedException();
    }
}