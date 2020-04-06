using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.ViewModels.Archive
{
    public class DocumentDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int RepositoryId { get; set; }
        public string Repository { get; set; }
        public int? ParentId { get; set; }
        public string Parent { get; set; }
        public int Version { get; set; }
        public string ContentType { get; set; }
        public long Length { get; set; }
        public string CheckInKey { get; set; }
        public DocumentOperation LastOperation { get; set; }
        public string OperationBy { get; set; }
        public DateTime OperationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Dictionary<string, string> Meta { get; set; }
        public string Thumbnail { get; set; }
        public IEnumerable<DocumentHistoryDetailModel> History { get; set; }
    }

    public class DocumentHistoryDetailModel
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public int Version { get; set; }
        public DocumentOperation Operation { get; set; }
        public string OperationBy { get; set; }
        public DateTime OperationOn { get; set; }
    }

    public class DocumentAddModel
    {
        public string RepositoryId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public int? ParentId { get; set; }
        public string ContentType { get; set; }
        public long Length { get; set; }
        public string UserName { get; set; }
        public Dictionary<string, string> Meta { get; set; }
    }

    public enum DocumentOperation
    {
        CheckedIn,
        CheckedOut,
        Reset
    }

    public class ChunkAddModel
    {
        public int RepositoryId { get; set; }
        public int DocumentId { get; set; }
        public int Version { get; set; }
        public int SortId { get; set; }
        public string OriginalName { get; set; }
        public string CheckInKey { get; set; }
        public string UserName { get; set; }
        public IFormFile File { get; set; }
    }
}
