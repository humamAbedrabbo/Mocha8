using AMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.ViewModels.BankApp
{
    public class IncomingPost
    {
        public IncomingPost()
        {
            ReceivedOn = DateTime.Today;
            Deadline = DateTime.Today;
        }

        public int Id { get; set; }

        [Required]
        public string Subject { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public string From { get; set; }

        [Display(Name = "Letter Ref.")]
        public string LetterReference { get; set; }

        [Display(Name = "Letter Outgoing Ref.")]
        public string LetterOutReference { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Received On")]
        public DateTime ReceivedOn { get; set; }

        [Display(Name = "Deadline")]
        public DeadlineType DeadlineType { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Deadline { get; set; }

        public int DayNo { get; set; }
        public int MonthNo { get; set; }
        public string AttentionToId { get; set; }
        public string CCToId { get; set; }
        public List<Attachment> Attachments { get; set; } = new List<Attachment>();
        
        [Display(Name = "Letter Type")]
        public IncomingPostType IncomingPostType { get; set; }
        public string Translation { get; set; }
        public List<IFormFile> Files { get; set; }

        [Display(Name = "CEO Comments")]
        public string CEOComments { get; set; }
        public List<ResponsiblePerson> ResponsiblePeople { get; set; } = new List<ResponsiblePerson>();
        public bool IsCeoStatusCompleted { get; set; }
        public string OutgoingCode { get; set; }
        public List<PostTask> PostTasks { get; set; } = new List<PostTask>();
        public PostStatus Status { get; set; }
        public bool IsDelivered { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public IFormFile DeliveryNoteFile { get; set; }

        public bool CanBeClosed => (Status == PostStatus.InProgress || Status == PostStatus.Incomplete) && PostTasks.All(x => x.IsDone);

        public SelectList Senders { get; set; }
        public SelectList Attentions { get; set; }
        public SelectList Users { get; set; }
    }

    public class ResponsiblePerson
    {
        public ResponsiblePerson()
        {

        }

        public ResponsiblePerson(string value)
        {
            Value = value;
        }

        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }

    public class PostTask
    {
        public int Id { get; set; }
        public int SortId { get; set; }
        public string ResponsilePerson { get; set; }
        public bool IsDone { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string Comments { get; set; }
        public List<IFormFile> WorkFiles { get; set; }
        public string UserName { get; set; }
    }

    public enum IncomingPostType
    {
        Letter,
        Fax
    }

    public enum DeadlineType
    {
        OnlyOnce,
        Monthly,
        Quarterly,
        Annual
    }

    public enum PostStatus
    {
        New,
        Pending,
        InProgress,
        Incomplete,
        Completed
    }
}
