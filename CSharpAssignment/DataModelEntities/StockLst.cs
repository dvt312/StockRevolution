using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CSharpAssignment.DataModelEntities
{
    [ExcludeFromCodeCoverage]
    [Table("StockLst")]
    public class StockLst
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StockLst()
        {
            AppUserStockLst = new HashSet<AppUserStockLst>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StockLstId { get; set; }

        [Required]
        [StringLength(50)]
        public string StockCode { get; set; }

        [Required]
        [StringLength(100)]
        public string StockName { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AppUserStockLst> AppUserStockLst { get; set; }
    }
}