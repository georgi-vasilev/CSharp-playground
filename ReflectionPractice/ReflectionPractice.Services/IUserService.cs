using ReflectionPractice.Common;

namespace ReflectionPractice.Services
{
    public interface IUserService : ITransientService
    {
        public int GetUsersCount();
    }

}
