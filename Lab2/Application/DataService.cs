using Lab2.Domain.Entities;
using Lab2.Infrastructure.Context;

namespace Lab2.Application;

public class DataService
{
    private readonly DataContext _context;

    public DataService(DataContext context)
    {
        _context = context;
    }

    public IEnumerable<Car> GetAll() => _context.Cars;

    public void AddRental(Car car, Rental rental)
    {
        car.Rentals.Add(rental);
        _context.Save();
    }
}