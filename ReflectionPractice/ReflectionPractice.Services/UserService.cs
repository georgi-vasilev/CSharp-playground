using System.Linq;
using ReflectionPractice.Data;

namespace ReflectionPractice.Services
{
    public class UserSerivce : IUserService
    {
        private readonly ApplicationDbContext context;

        public UserSerivce(ApplicationDbContext context)
        {
            this.context = context;
        }

        public int GetUsersCount()
        {
            return this.context.Users.Count();
        }
    }
}
