using System.Xml;
using Lab2.Domain.Entities;

namespace Lab2.Infrastructure;

public static class XmlWriterExtensions
{
    public static void WriteAddressToXml(this XmlWriter xmlWriter, Address address)
    {
        if (address is null)
            throw new ArgumentNullException(nameof(address), "Address cannot be null");
        
        xmlWriter.WriteStartElement("address");
        xmlWriter.WriteElementString("country", address.Country);
        xmlWriter.WriteElementString("city", address.City);
        xmlWriter.WriteElementString("street", address.Street);
        xmlWriter.WriteElementString("building", address.Building);
        xmlWriter.WriteElementString("postalCode", address.PostalCode);
        xmlWriter.WriteEndElement();
    }
    
    public static void WriteCarMakeToXml(this XmlWriter xmlWriter, CarMake carMake)
    {
        if (carMake is null)
            throw new ArgumentNullException(nameof(carMake), "CarMake cannot be null");
        
        xmlWriter.WriteStartElement("carMake");
        xmlWriter.WriteElementString("name", carMake.Name);
        xmlWriter.WriteEndElement();
    }
    
    public static void WriteCarsToXml(this XmlWriter xmlWriter, ICollection<Car> cars)
    {
        if (cars is null)
            throw new ArgumentNullException(nameof(cars), "Cars cannot be null");
        
        xmlWriter.WriteStartElement("cars");
        foreach (var car in cars)
        {
            xmlWriter.WriteStartElement("car");
            xmlWriter.WriteElementString("name", car.Name);
            xmlWriter.WriteElementString("issueYear", car.IssueYear.ToString());
            xmlWriter.WriteElementString("carType", ((int)car.CarType).ToString());
            xmlWriter.WriteElementString("price", car.Price.ToString());
            xmlWriter.WriteElementString("pricePerDay", car.PricePerDay.ToString());
            xmlWriter.WriteCarMakeToXml(car.CarMake);
            xmlWriter.WriteRentalsToXml(car.Rentals);
            xmlWriter.WriteEndElement();
        }
        xmlWriter.WriteEndElement();
    }
    
    public static void WriteRentalsToXml(this XmlWriter xmlWriter, ICollection<Rental> rentals)
    {
        if (rentals is null)
            throw new ArgumentNullException(nameof(rentals), "Rentals cannot be null");
        
        xmlWriter.WriteStartElement("rentals");
        foreach (var rental in rentals)
        {
            xmlWriter.WriteStartElement("rental");
            xmlWriter.WriteElementString("issueDate", rental.IssueDate.LocalDateTime.ToString());
            xmlWriter.WriteElementString("dueDate", rental.DueDate.LocalDateTime.ToString());
            xmlWriter.WriteElementString("pledge", rental.Pledge.ToString());
            xmlWriter.WriteElementString("rentalPrice", rental.RentalPrice.ToString());
            xmlWriter.WriteClientToXml(rental.Client);
            xmlWriter.WriteEndElement();
        }
        xmlWriter.WriteEndElement();
    }
    
    public static void WriteClientToXml(this XmlWriter xmlWriter, Client client)
    {
        if (client is null)
            throw new ArgumentNullException(nameof(client), "Client cannot be null");
        
        xmlWriter.WriteStartElement("client");
        xmlWriter.WriteElementString("firstName", client.FirstName);
        xmlWriter.WriteElementString("lastName", client.LastName);
        xmlWriter.WriteElementString("phoneNumber", client.PhoneNumber);
        xmlWriter.WriteAddressToXml(client.Address);
        xmlWriter.WriteEndElement();
    }
}