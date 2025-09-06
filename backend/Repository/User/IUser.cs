using backend.Model;

namespace backend.Repository
{
    public interface IUser
    {
        Task<User> CreateUser(User user);
        Task<bool> UpdatePassword(string email, string password);
        Task<IEnumerable<User>> GetAllByRol();
        Task<User?> Authenticate(string email, string password);
    
        string GenerateToken(int id, string name,string role);
         Task<User?> ValidateUser(string user, string password);
}

}