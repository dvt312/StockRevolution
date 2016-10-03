using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CSharpAssignment.DataModelEntities
{
    [ExcludeFromCodeCoverage]
    [Table("TokenValid")]
    public class TokenValid
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TokenValidId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AppUserId { get; set; }

        [StringLength(100)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string TokenId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IsValid { get; set; }

        public virtual AppUser AppUser { get; set; }
    }
}