namespace Lab2.Domain.Entities;

public class Rental
{
    public DateTimeOffset IssueDate { get; set; }
    public DateTimeOffset DueDate { get; set; }
    public decimal Pledge { get; set; }
    public decimal RentalPrice { get; set; }
    public Client Client { get; set; } = new();

    public override bool Equals(object? obj)
    {
        if (obj is not Rental other)
            return false;

        return IssueDate == other.IssueDate
               && DueDate == other.DueDate
               && Pledge == other.Pledge
               && RentalPrice == other.RentalPrice
               && Client == other.Client;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IssueDate, DueDate, Pledge, RentalPrice);
    }

    public override string ToString()
    {
        return $"IssueDate: {IssueDate.LocalDateTime} - DueDate - {DueDate.LocalDateTime}" +
               $" - Pledge - {Pledge} - RentalPrice - {RentalPrice}";
    }
}