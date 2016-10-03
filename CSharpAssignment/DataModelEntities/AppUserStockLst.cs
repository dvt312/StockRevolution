using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CSharpAssignment.DataModelEntities
{
    [ExcludeFromCodeCoverage]
    [Table("AppUserStockLst")]
    public class AppUserStockLst
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppUserStockLstId { get; set; }

        public int AppUserId { get; set; }

        public int StockLstId { get; set; }

        public virtual AppUser AppUser { get; set; }

        public virtual StockLst StockLst { get; set; }
    }
}