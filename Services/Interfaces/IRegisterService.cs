using System.Threading.Tasks;
using KompaniaPchor.DTO_Models;

namespace KompaniaPchor.Services.Interfaces
{
    public interface IRegisterService
    {
        Task<string> RegisterNewUser(DTO_RegisterForm registerForm);
    }
}