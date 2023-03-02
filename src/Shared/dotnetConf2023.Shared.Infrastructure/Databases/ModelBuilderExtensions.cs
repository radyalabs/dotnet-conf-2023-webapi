using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace dotnetConf2023.Shared.Infrastructure.Databases;

public static class ModelBuilderExtensions
{
    public static readonly ValueConverter<DateTime, DateTime> UtcValueConverter =
        new(outside => outside, inside => DateTime.SpecifyKind(inside, DateTimeKind.Utc));

    /// <summary>
    /// Applies the UTC date-time converter to all of the properties that are <see cref="DateTime"/> and end with Utc.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    public static void ApplyUtcDateTimeConverter(this ModelBuilder modelBuilder) =>
        modelBuilder.Model.GetEntityTypes()
            .ForEach(mutableEntityType => mutableEntityType
                .GetProperties()
                .Where(p => p.ClrType == typeof(DateTime) && p.Name.EndsWith("Utc", StringComparison.Ordinal))
                .ForEach(mutableProperty => mutableProperty.SetValueConverter(UtcValueConverter)));
}