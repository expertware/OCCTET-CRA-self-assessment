using exp.Infrastructure.Entities;
using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Web;

namespace exp.Infrastructure.Context
{
    public partial class SurveyDBContext : DbContext
    {
        public HtmlSanitizer sanitizer = new HtmlSanitizer();

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Organisation>().HasQueryFilter(p => !(p.IsDeleted ?? false));
            modelBuilder.Entity<AspNetUser>().HasQueryFilter(p => !(p.IsDeleted ?? false));
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(string))
                    {
                        modelBuilder.Entity(entityType.ClrType)
                                    .Property(property.Name)
                                    .HasConversion(
                                        new ValueConverter<string, string>(
                                            v => v, // Convert when saving (no change)
                                            v => HttpUtility.HtmlDecode(v) // Decode when loading
                                        )
                                    );
                    }
                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Modified:
                        foreach (var property in entry.Properties)
                        {
                            if (property.CurrentValue is string currentValue)
                            {
                                sanitizer.AllowedTags.Clear(); // Reset allowed tags to default 

                                property.CurrentValue = sanitizer.Sanitize(currentValue);
                            }
                        }
                        break;
                }
            }
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }

    }
}
