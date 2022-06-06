namespace Lab2.Domain.Entities;

public class Address
{
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string Building { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;

    public override bool Equals(object? obj)
    {
        if (obj is not Address other)
            return false;

        return Country == other.Country
               && City == other.City
               && Street == other.Street
               && Building == other.Building
               && PostalCode == other.PostalCode;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Country, City, Street, Building, PostalCode);
    }

    public override string ToString()
    {
        return $"{Country} - {City} - {Street} - {Building} - PostalCode: {PostalCode}";
    }
}