using AMS.ViewModels.Archive;
using System.Threading.Tasks;

namespace AMS.Services
{
    public interface IArchiveAdapter
    {
        Task<DocumentDetailModel> PostDocument(DocumentAddModel doc);
        Task<bool> UploadChunk(ChunkAddModel doc);
    }
}