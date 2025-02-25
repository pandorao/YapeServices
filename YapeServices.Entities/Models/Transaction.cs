using System.ComponentModel.DataAnnotations;
using Yape.Entities.Base;
using YapeServices.Entities.Enumerations;

namespace YapeServices.Entities.Models
{
    public class Transaction : BaseModel
    {
        [Key]
        public string TransactionExternalId { get; set; }
        [Required]
        public string SourceAccountId { get; set; }
        [Required]
        public string TargetAccountId { get; set; }
        [Required]
        public int TransferTypeId { get; set; }
        [Required]
        public decimal Value { get; set; }
        [Required]
        public EnumTransactionStatus TransactionStatus { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime? ExecutedAt { get; set; }
    }
}
