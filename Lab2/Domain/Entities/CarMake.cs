namespace Lab2.Domain.Entities;

public class CarMake
{
    public string Name { get; set; } = string.Empty;

    public override bool Equals(object? obj)
    {
        if (obj is not CarMake other)
            return false;

        return Name == other.Name;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public override string ToString()
    {
        return $"Manufacturer: {Name}";
    }
}