using Autocomplete.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autocomplete.Data;

public class AutocompleteItemConfiguration : IEntityTypeConfiguration<AutocompleteItem>
{
    public void Configure(EntityTypeBuilder<AutocompleteItem> builder)
    {
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.Category).HasMaxLength(100);
    }
}
