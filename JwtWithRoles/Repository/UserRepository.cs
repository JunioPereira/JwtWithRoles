using JwtWithRoles.Models;

namespace JwtWithRoles.Repository
{
    public interface IUserRepository 
    {
        User Get(string username, string password);
    }

    public class UserRepository : IUserRepository
    {
        List<User> users { get; }

        public UserRepository() 
        {
            users = new List<User>();
            users.Add(new User { Id = 1, Username = "batman", Password = "batman", Role = "manager" });
            users.Add(new User { Id = 2, Username = "robin", Password = "robin", Role = "employee" });
        }

        public User Get(string username, string password)
        {
            return users.Where(x => x.Username.ToLower() == username.ToLower() && x.Password == x.Password).FirstOrDefault();
        }
    }
}
