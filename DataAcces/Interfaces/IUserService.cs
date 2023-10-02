using DataAcces.Models;

namespace DataAcces;

public interface IUserService
{
    Task<string> CreateUser();
    Task<TelegramUser> GetUsers();
    Task<TelegramUser> GetUserById(long Id);
}