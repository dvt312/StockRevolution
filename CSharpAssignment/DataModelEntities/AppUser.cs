using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CSharpAssignment.DataModelEntities
{
    [ExcludeFromCodeCoverage]
    [Table("AppUser")]
    public class AppUser
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AppUser()
        {
            AppUserStockLst = new HashSet<AppUserStockLst>();
            TokenValid = new HashSet<TokenValid>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppUserId { get; set; }

        public int PersonId { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppUserStockLst> AppUserStockLst { get; set; }

        public virtual Person Person { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TokenValid> TokenValid { get; set; }
    }
}