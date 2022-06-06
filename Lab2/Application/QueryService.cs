using System.Xml.Linq;
using Lab2.Application.ViewModels;
using Lab2.Domain.Entities;
using Lab2.Domain.Enums;
using Lab2.Infrastructure.Context;

namespace Lab2.Application;

public class QueryService
{
    private readonly XDocument _document;

    public QueryService(DataContext dataContext)
    {
        _document = XDocument.Load(dataContext.Path);
    }

    public IEnumerable<Car> GetAllCars()
    {
        return GetCarsStructure();
    }
    
    public decimal GetProfit(DateTimeOffset dateStartFrom)
    {
        return _document.Descendants("rental")
            .Select(r => GetRental(r))
            .Where(r => r.DueDate >= dateStartFrom)
            .Sum(r => r.RentalPrice);
    }

    public Car GetTheMostPopularCar()
    {
        return GetCarsStructure()
            .OrderByDescending(c => c.Rentals.Count)
            .First();
    }
    
    public IEnumerable<Client> GetClientsSortedByFullName()
    {
        return _document.Descendants("client")
            .Select(c => GetClient(c))
            .Distinct()
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName);
    }

    public IEnumerable<Car> GetCarsIssuedAfter(int startFromYear)
    {
        return GetCarsStructure()
            .Where(c => c.IssueYear > startFromYear);
    }
    
    public IEnumerable<CarMake> GetManufacturersOrderedByPopularity()
    {
        return _document.Descendants("carMake")
            .Select(cm => new CarMake() {Name = cm.Element("name").Value})
            .Distinct()
            .GroupJoin(GetCarsStructure(),
                cm => cm,
                c => c.CarMake,
                (cm, c) => new
                {
                    CarMare = cm,
                    CarRentalsQuantity = c.Sum(c => c.Rentals.Count)
                })
            .OrderByDescending(r => r.CarRentalsQuantity)
            .Select(r => r.CarMare);
    }
    
    public IEnumerable<Car> GetAvailableCarsAtTheMoment()
    {
        return GetCarsStructure()
            .Where(c => c.Rentals.All(r => r.DueDate < DateTimeOffset.Now));
    }
    
    public decimal GetAverageCarsTypeRentalPrice(CarType carType)
    {
        return GetCarsStructure()
            .Where(c => c.CarType == carType)
            .Average(c => c.PricePerDay);
    }
    
    public IDictionary<CarType, int> GetCarsQuantityByType()
    {
        return GetCarsStructure()
            .GroupBy(c => c.CarType)
            .ToDictionary(key => key.Key,
                value => value.Count());
    }
    
    public IEnumerable<Rental> GetAllRentalsSortedByPrice()
    {
        return from rental in _document.Descendants("rental")
                .Select(r => GetRental(r))
            orderby rental.RentalPrice descending
            select rental;
    }
    
    public IEnumerable<Car> GetCarsByType(CarType carType)
    {
        return GetCarsStructure()
            .Where(c => c.CarType == carType)
            .OrderByDescending(c => c.Price);
    }
    
    public IEnumerable<ClientProfit> GetClientsSortedByProfit()
    {
        return _document.Descendants("client")
            .Select(c => GetClient(c))
            .Distinct()
            .GroupJoin(_document.Descendants("rental")
                    .Select(r => GetRental(r)),
                c => c,
                r => r.Client,
                (c, r) => new ClientProfit()
                {
                    Client = c,
                    TotalProfit = r.Sum(rental => rental.RentalPrice)
                })
            .OrderByDescending(t => t.TotalProfit);
    }
    
    public Car GetCarWithTheMostExpensiveRentalPrice()
    {
        return GetCarsStructure().MaxBy(c => c.PricePerDay) ?? 
               throw new InvalidOperationException("There`re no cars");
    }
    
    public IDictionary<string, IEnumerable<Client>> GetClientsByCountries()
    {
        return _document.Descendants("address")
            .Select(a => GetAddress(a))
            .Distinct()
            .GroupJoin(_document.Descendants("client")
                    .Select(c => GetClient(c))
                    .Distinct(),
                a => a,
                c => c.Address,
                (a, c) => new
                {
                    a.Country,
                    Clients = c
                })
            .ToDictionary(key => key.Country, value => value.Clients);
    }

    public IEnumerable<CarRating> GetCarsWithRentalsQuantity()
    {
        return GetCarsStructure()
            .Select(c => new CarRating()
            {
                Car = c,
                RentalsQuantity = c.Rentals.Count
            })
            .OrderByDescending(r => r.RentalsQuantity);
    }

    private IEnumerable<Car> GetCarsStructure()
    {
        return _document.Descendants("car")
            .Select(c => new Car()
            {
                Name = c.Element("name").Value,
                IssueYear = ushort.Parse(c.Element("issueYear").Value),
                CarType = (CarType) int.Parse(c.Element("carType").Value),
                Price = decimal.Parse(c.Element("price").Value),
                PricePerDay = Decimal.Parse(c.Element("pricePerDay").Value),
                CarMake = c.Descendants("carMake").Select(cm => new CarMake()
                {
                    Name = cm.Element("name").Value
                }).First(),
                Rentals = c.Descendants("rental").Select(r => GetRental(r))
                    .ToList()
            });
    }

    private Client GetClient(XElement element)
    {
        return new Client()
        {
            FirstName = element.Element("firstName").Value,
            LastName = element.Element("lastName").Value,
            PhoneNumber = element.Element("phoneNumber").Value,
            Address = GetAddress(element.Descendants("address").First())
        };
    }

    private Address GetAddress(XElement element)
    {
        return new Address()
        {
            Country = element.Element("country").Value,
            City = element.Element("city").Value,
            Street = element.Element("street").Value,
            Building = element.Element("building").Value,
            PostalCode = element.Element("postalCode").Value
        };
    }

    private Rental GetRental(XElement element)
    {
        return new Rental()
        {
            IssueDate = DateTimeOffset.Parse(element.Element("issueDate").Value),
            DueDate = DateTimeOffset.Parse(element.Element("dueDate").Value),
            Pledge = decimal.Parse(element.Element("pledge").Value),
            RentalPrice = decimal.Parse(element.Element("rentalPrice").Value),
            Client = GetClient(element.Descendants("client").First())
        };
    }
}