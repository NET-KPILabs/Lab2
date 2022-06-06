using System.Xml;
using Bogus;
using Lab2.Domain.Entities;
using Lab2.Domain.Enums;

namespace Lab2.Infrastructure.Context;

public static class DataContextExtensions
{
    public static void Seed(this DataContext dataContext)
    {
        var addresses = GenerateRandomAddresses(10);
        var carMakes = GenerateRandomCarMakes(10);
        var cars = GenerateRandomCars(carMakes, 10);
        var clients = GenerateRandomClients(addresses, 10);
        var rentals = GenerateRandomRentals(cars, clients, 25);

        dataContext.Cars = cars;
    }
    
    public static void Save(this DataContext dataContext)
    {
        if (string.IsNullOrEmpty(dataContext.Path))
        {
            throw new ArgumentException("Path cannot be null or empty");
        }
        
        var setting = new XmlWriterSettings()
        {
            Indent = true
        };

        using var writer = XmlWriter.Create(dataContext.Path, setting);
        writer.WriteCarsToXml(dataContext.Cars);
    }

    public static void Load(this DataContext dataContext)
    {
        if (string.IsNullOrEmpty(dataContext.Path))
        {
            throw new ArgumentException("Path cannot be null or empty");
        }
        
        var xmlDocument = new XmlDocument();
        xmlDocument.Load(dataContext.Path);
        dataContext.Cars = Read(xmlDocument) ?? new List<Car>();
    }

    private static ICollection<Car>? Read(XmlNode? root)
    {
        if (root is null)
            return default;
    
        return root.ReadCarsFromNode();
    }

    private static IList<Address> GenerateRandomAddresses(int count)
    {
        return new Faker<Address>()
            .RuleFor(p => p.Country, f => f.Address.Country())
            .RuleFor(p => p.City, f => f.Address.City())
            .RuleFor(p => p.Street, f => f.Address.StreetName())
            .RuleFor(p => p.Building, f => f.Address.BuildingNumber())
            .RuleFor(p => p.PostalCode, f => f.Address.ZipCode())
            .Generate(count);
    }

    private static IList<CarMake> GenerateRandomCarMakes(int count)
    {
        return new Faker<CarMake>()
            .RuleFor(p => p.Name, f => f.Vehicle.Manufacturer())
            .Generate(count)
            .DistinctBy(c => c.Name)
            .ToList();
    }

    private static IList<Car> GenerateRandomCars(IList<CarMake> carMakes, int count)
    {
        return new Faker<Car>()
            .RuleFor(p => p.Name, f => f.Vehicle.Model())
            .RuleFor(p => p.IssueYear, f => f.Random.UShort(1980, 2022))
            .RuleFor(p => p.CarType, f => f.PickRandom<CarType>())
            .RuleFor(p => p.Price, f => f.Random.Int(1000, 50000))
            .RuleFor(p => p.PricePerDay, f => f.Random.Int(20, 50))
            .RuleFor(p => p.CarMake, f => f.PickRandom(carMakes))
            .Generate(count);
    }
    
    private static IList<Rental> GenerateRandomRentals(IList<Car> cars, IList<Client> clients, int count)
    {
        return new Faker<Rental>()
            .RuleFor(p => p.Pledge, f => f.Random.Int(300, 1000))
            .RuleFor(p => p.Client, f => f.PickRandom(clients))
            .FinishWith((f, r) =>
            {
                var car = f.PickRandom(cars);
                r.IssueDate = f.Date.BetweenOffset(DateTimeOffset.Now - TimeSpan.FromDays(30), DateTimeOffset.Now);
                r.DueDate = f.Date.BetweenOffset(r.IssueDate, DateTimeOffset.Now);
                var profit = Math.Round((r.DueDate - r.IssueDate).TotalDays * Decimal.ToDouble(car.PricePerDay), 2);
                r.RentalPrice = (decimal) profit;
                car.Rentals.Add(r);
            })
            .Generate(count);
    }
    
    private static IList<Client> GenerateRandomClients(IList<Address> addresses, int count)
    {
        return new Faker<Client>()
            .RuleFor(p => p.FirstName, f => f.Person.FirstName)
            .RuleFor(p => p.LastName, f => f.Person.LastName)
            .RuleFor(p => p.PhoneNumber, f => f.Phone.PhoneNumber("+380-###-##-##"))
            .RuleFor(p => p.Address, f => f.PickRandom(addresses))
            .Generate(count);
    }
}