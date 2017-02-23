using System;
using System.ComponentModel.DataAnnotations.Schema;

using SweetHome.Core.Entities.Identity;


namespace SweetHome.Core.Entities
{
    public class Message : BaseEntity
    {
        public string ExceptionMessage { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime CreateDate { get; set; }
        public long AttemptsCount { get; set; }
        public short MessageState { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime UpdateDate { get; set; }
        public byte[] SerializedMessage { get; set; }
        public virtual AppUser AspNetUser { get; set; }
    }
}
