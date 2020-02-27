using System.Threading.Tasks;

namespace AMS.Services
{
    public interface ICodeGenerator
    {
        Task<int> GetAssetCode(int tenantId);
        Task<int> GetTicketCode(int tenantId);
    }
}