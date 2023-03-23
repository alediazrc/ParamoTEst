using Sat.Recruitment.Api.Model;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Services
{
    public interface IUserService
    {
        public Task<Result> CreateUser(User newUser);
    }
}
