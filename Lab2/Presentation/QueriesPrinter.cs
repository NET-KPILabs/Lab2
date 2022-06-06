using Lab2.Application;
using Lab2.Domain.Entities;
using Lab2.Domain.Enums;

namespace Lab2.Presentation;

public class QueriesPrinter
{
    private readonly QueryService _queryService;

    public QueriesPrinter(QueryService queryService)
    {
        _queryService = queryService;
    }

    public void PrintProfit(DateTimeOffset dateStartFrom)
    {
        Console.WriteLine(_queryService.GetProfit(dateStartFrom));
    }

    public void PrintAllCars()
    {
        PrintCars(_queryService.GetAllCars());
    }

    public void PrintClientsSortedByProfit()
    {
        var clients = _queryService.GetClientsSortedByProfit();

        foreach (var client in clients)
        {
            Console.WriteLine($"{client.Client} - {client.TotalProfit}");
        }
    }

    public void PrintCarsByType(CarType carType)
    {
        PrintCars(_queryService.GetCarsByType(carType));
    }

    public void PrintAvailableCarsAtTheMoment()
    {
        PrintCars(_queryService.GetAvailableCarsAtTheMoment());
    }

    public void PrintAverageCarsTypeRentalPrice(CarType carType)
    {
        Console.WriteLine(_queryService.GetAverageCarsTypeRentalPrice(carType).ToString(".##"));
    }

    public void PrintCarsQuantityByType()
    {
        var result = _queryService.GetCarsQuantityByType();

        foreach (var pair in result)
        {
            Console.WriteLine($"{pair.Key} - {pair.Value}");
        }
    }

    public void PrintRentalsSortedByPrice()
    {
        var rentals = _queryService.GetAllRentalsSortedByPrice();
        
        foreach (var rental in rentals)
        {
            Console.WriteLine(rental);
        }
    }

    public void PrintCarWithTheMostExpensiveRentalPrice()
    {
        Console.WriteLine(_queryService.GetCarWithTheMostExpensiveRentalPrice());
    }

    public void PrintManufacturersOrderedByPopularity()
    {
        var manufacturers = _queryService.GetManufacturersOrderedByPopularity();

        foreach (var manufacturer in manufacturers)
        {
            Console.WriteLine(manufacturer);
        }
    }

    public void PrintCarsIssuedAfter(int startFromYear)
    {
        PrintCars(_queryService.GetCarsIssuedAfter(startFromYear));
    }

    public void PrintClientsSortedByFullName()
    {
        var clients = _queryService.GetClientsSortedByFullName();

        foreach (var client in clients)
        {
            Console.WriteLine(client);
        }
    }

    public void PrintClientsByCountries()
    {
        var result = _queryService.GetClientsByCountries();

        foreach (var pair in result)
        {
            Console.WriteLine($"{pair.Key}:");
            foreach (var client in pair.Value)
            {
                Console.WriteLine(client);
            }
        }
    }

    public void PrintTheMostPopularCar()
    {
        Console.WriteLine(_queryService.GetTheMostPopularCar());
    }
    
    public void PrintCarsWithRentalsQuantity()
    {
        var carRatings = _queryService.GetCarsWithRentalsQuantity();

        foreach (var carRating in carRatings)
        {
            Console.WriteLine(carRating.Car);
            Console.WriteLine($"Total rentals: {carRating.RentalsQuantity}");
        }
    }

    private void PrintCars(IEnumerable<Car> cars)
    {
        foreach (var car in cars)
        {
            Console.WriteLine(car);
        }
    }
}