using System.Threading.Tasks;

namespace KompaniaPchor.Services.Interfaces
{
    public interface IRequestService
    {
        Task AcceptRequest(int requestId, bool accepted);
    }
}