using System.Xml;
using Lab2.Domain.Entities;
using Lab2.Domain.Enums;

namespace Lab2.Infrastructure;

public static class XmlDocumentExtensions
{
    public static Address ReadAddressFromNode(this XmlNode xmlNode)
    {
        var address = xmlNode["address"];
        if (address is null)
            throw new InvalidOperationException("There's no address in this node");

        return new Address()
        {
            Country = address["country"].InnerText,
            City = address["city"].InnerText,
            Street = address["street"].InnerText,
            Building = address["building"].InnerText,
            PostalCode = address["postalCode"].InnerText
        };
    }
    
    public static CarMake ReadCarMakeFromNode(this XmlNode xmlNode)
    {
        var carMake = xmlNode["carMake"];
        if (carMake is null)
            throw new InvalidOperationException("There's no carMake in this node");

        return new CarMake()
        {
            Name = carMake["name"].InnerText,
        };
    }
    
    public static ICollection<Car> ReadCarsFromNode(this XmlNode xmlNode)
    {
        var node = xmlNode["cars"];
        if (node is null)
            throw new InvalidOperationException("There're no cars in this node");
        
        var cars = new List<Car>();
    
        foreach (XmlElement element in node)
        {
            cars.Add(new Car()
            {
                Name = element["name"].InnerText,
                IssueYear = ushort.Parse(element["issueYear"].InnerText),
                CarType = (CarType)int.Parse(element["carType"].InnerText),
                Price = decimal.Parse(element["price"].InnerText),
                PricePerDay = decimal.Parse(element["pricePerDay"].InnerText),
                CarMake = element.ReadCarMakeFromNode(),
                Rentals = element.ReadRentalsFromNode()
            });
        }
    
        return cars;
    }
    
    public static ICollection<Rental> ReadRentalsFromNode(this XmlNode xmlNode)
    {
        var node = xmlNode["rentals"];
        if (node is null)
            throw new InvalidOperationException("There're no rentals in this node");
    
        var rentals = new List<Rental>();
    
        foreach (XmlElement element in node)
        {
            rentals.Add(new Rental()
            {
                IssueDate = DateTimeOffset.Parse(element["issueDate"].InnerText),
                DueDate = DateTimeOffset.Parse(element["dueDate"].InnerText),
                Pledge = decimal.Parse(element["pledge"].InnerText),
                RentalPrice = decimal.Parse(element["rentalPrice"].InnerText),
                Client = element.ReadClientFromNode()
            });
        }
    
        return rentals;
    }
    
    public static Client ReadClientFromNode(this XmlNode xmlNode)
    {
        var client = xmlNode["client"];
        if (client is null)
            throw new InvalidOperationException("There's no client in this node");

        return new Client()
        {
            FirstName = client["firstName"].InnerText,
            LastName = client["lastName"].InnerText,
            PhoneNumber = client["phoneNumber"].InnerText,
            Address = client.ReadAddressFromNode()
        };
    }
}