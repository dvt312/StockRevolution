using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;

namespace CSharpAssignment.DataModelEntities
{
    public class StockEntitiesModel : DbContext
    {
        private string connectionName { get; set; }

        public StockEntitiesModel() : base("name=StockEntitiesModel") { }

        /// <summary>Protected constructor, used just with tests purpose</summary>
        protected StockEntitiesModel(string name)
        {
            connectionName = name;
        }

        public virtual DbSet<AppUser> AppUser { get; set; }
        public virtual DbSet<AppUserStockLst> AppUserStockLst { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<StockLst> StockLst { get; set; }
        public virtual DbSet<TokenValid> TokenValid { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null) return;
            modelBuilder.Entity<AppUser>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<AppUser>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<AppUser>()
                .HasMany(e => e.AppUserStockLst)
                .WithRequired(e => e.AppUser)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<AppUser>()
                .HasMany(e => e.TokenValid)
                .WithRequired(e => e.AppUser)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Person>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .Property(e => e.Mail)
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Person>()
                .HasMany(e => e.AppUser)
                .WithRequired(e => e.Person)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<StockLst>()
                .Property(e => e.StockCode)
                .IsUnicode(false);

            modelBuilder.Entity<StockLst>()
                .Property(e => e.StockName)
                .IsUnicode(false);

            modelBuilder.Entity<StockLst>()
                .HasMany(e => e.AppUserStockLst)
                .WithRequired(e => e.StockLst)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<TokenValid>()
                .Property(e => e.TokenId)
                .IsUnicode(false);
        }


        [ExcludeFromCodeCoverage]
        // Excluding from coverage since Moq and NUnit are not able to mock non virtual methods
        public virtual void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }
    }
}