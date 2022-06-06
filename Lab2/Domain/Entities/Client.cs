namespace Lab2.Domain.Entities;

public class Client
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public Address Address { get; set; } = new();

    public override bool Equals(object? obj)
    {
        if (obj is not Client other)
            return false;

        return FirstName == other.FirstName
               && LastName == other.LastName
               && PhoneNumber == other.PhoneNumber;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FirstName, LastName, PhoneNumber);
    }

    public override string ToString()
    {
        return $"{LastName} {FirstName} - {PhoneNumber}";
    }
}