using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DGPCE.Sigemad.Infrastructure.Converters;
public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    public DateOnlyConverter() : base(
        dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue), // De DateOnly a DateTime
        dateTime => DateOnly.FromDateTime(dateTime)) // De DateTime a DateOnly
    {
    }
}
