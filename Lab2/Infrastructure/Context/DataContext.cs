using Lab2.Domain.Entities;

namespace Lab2.Infrastructure.Context;

public class DataContext
{
    public string Path { get; init; } = string.Empty;
    public ICollection<Car> Cars { get; set; } = new List<Car>();
}