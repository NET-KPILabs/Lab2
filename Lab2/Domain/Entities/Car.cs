using Lab2.Domain.Enums;

namespace Lab2.Domain.Entities;

public class Car
{
    public string Name { get; set; } = string.Empty;
    public ushort IssueYear { get; set; }
    public CarType CarType { get; set; }
    public decimal Price { get; set; }
    public decimal PricePerDay { get; set; }
    public CarMake CarMake { get; set; } = new();
    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();

    public override bool Equals(object? obj)
    {
        if (obj is not Car other)
            return false;

        return Name == other.Name
               && IssueYear == other.IssueYear
               && CarType == other.CarType
               && Price == other.Price
               && PricePerDay == other.PricePerDay
               && CarMake == other.CarMake;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, IssueYear, CarMake);
    }

    public override string ToString()
    {
        return $"{Name} - {IssueYear} - {CarType} - Price: {Price} - PricePerDay: {PricePerDay}" +
               $"\n{CarMake}";
    }
}