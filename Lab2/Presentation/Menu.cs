using Lab2.Application;
using Lab2.Domain.Entities;

namespace Lab2.Presentation;

public class Menu
{
    private readonly QueriesMenu _queriesMenu;
    private readonly DataService _dataService;

    public Menu(QueriesPrinter queriesPrinter, DataService dataService)
    {
        _queriesMenu = new QueriesMenu(queriesPrinter);
        _dataService = dataService;
    }

    public void Print() =>
        Console.WriteLine
        (
            "1. Show file content\n" +
            "2. Rent car\n" +
            "3. To all queries\n" +
            "0. Exit"
        );

    public void Start()
    {
        Console.Write("Enter your choice: ");
        var exit = false;
        var choice = 0;
        
        while (!exit)
        {
            int.TryParse(Console.ReadLine(), out choice);
            switch (choice)
            {
                case 1:
                    PrintFileContent();
                    break;
                case 2:
                    RentCar();
                    break;
                case 3: 
                    _queriesMenu.Print();
                    _queriesMenu.Start();
                    Print();
                    break;
                case 0:
                    exit = true;
                    break;
            }
            
            if(exit != true)
                Console.Write("\nEnter your choice: ");
        }       
    }

    private void PrintFileContent()
    {
        var cars = _dataService.GetAll();
        var tab = "";
        foreach (var car in cars)
        {
            Console.WriteLine
            (
                $"Name: {car.Name}\n" +
                $"IssueYear: {car.IssueYear}\n" +
                $"CarType: {car.CarType}\n" +
                $"Price: {car.Price}\n" +
                $"PricePerDay: {car.PricePerDay}\n" +
                $"Manufacturer: {car.CarMake.Name}"
            );
            Console.WriteLine("Rentals:");
            foreach (var rental in car.Rentals)
            {
                tab = "\t";
                Console.WriteLine
                (
                    $"{tab}IssueDate: {rental.IssueDate.LocalDateTime}\n" +
                    $"{tab}DueDate: {rental.DueDate.LocalDateTime}\n" +
                    $"{tab}Pledge: {rental.Pledge}\n" +
                    $"{tab}RentalPrice: {rental.RentalPrice}"
                );
                Console.WriteLine($"{tab}Client:");
                tab = "\t\t";
                Console.WriteLine
                (
                    $"{tab}FirstName: {rental.Client.FirstName}\n" +
                    $"{tab}LastName: {rental.Client.LastName}\n" +
                    $"{tab}PhoneNumber: {rental.Client.PhoneNumber}"
                );
                var address = rental.Client.Address;
                Console.WriteLine($"{tab}Address:");
                tab = "\t\t\t";
                Console.WriteLine
                (
                    $"{tab}Country: {address.Country}\n" +
                    $"{tab}City: {address.City}\n" +
                    $"{tab}Street: {address.Street}\n" +
                    $"{tab}Building: {address.Building}\n" +
                    $"{tab}PostalCode: {address.PostalCode}"
                );
            }
        }
    }

    private void RentCar()
    {
        var cars = _dataService.GetAll();
        var length = cars.Count();
        Console.WriteLine("Select car:");
        for (int i = 0; i < length; i++)
        {
            Console.WriteLine($"{i} -> {cars.ElementAt(i)}");
        }

        var chosenCar = 0;
        Console.Write("Enter your choice: ");
        while (!int.TryParse(Console.ReadLine(), out chosenCar) || chosenCar < 0 || chosenCar >= length)
        {
            Console.Write("\nEnter your choice: ");
        }
        var car = cars.ElementAt(chosenCar);
        
        var client = new Client();
        client.FirstName = EnterNotEmptyString("First name");
        client.LastName = EnterNotEmptyString("Last name");
        client.PhoneNumber = EnterNotEmptyString("Phone number");

        var rental = new Rental();
        rental.Client = client;
        rental.IssueDate = DateTimeOffset.Now;
        DateTimeOffset dueDate;
        while (!DateTimeOffset.TryParse(EnterNotEmptyString("Due date"), out dueDate) || DateTimeOffset.Now >= dueDate)
        {
            Console.WriteLine("Wrong due date");
        }
        rental.DueDate = dueDate;
        decimal pledge;
        while (!decimal.TryParse(EnterNotEmptyString("Pledge"), out pledge) || pledge < 0)
        {
            Console.WriteLine("Wrong pledge");
        }
        rental.RentalPrice = (decimal)Math.Round((rental.DueDate - rental.IssueDate).TotalDays * Decimal.ToDouble(car.PricePerDay), 2);
        
        _dataService.AddRental(car, rental);
        Console.WriteLine("Rental was successfully added");
    }

    private string EnterNotEmptyString(string field)
    {
        Console.Write($"{field}: ");
        string result = Console.ReadLine();
        while (string.IsNullOrEmpty(result))
        {
            Console.WriteLine($"{field} cannot be empty");
            Console.Write($"{field}: ");
            result = Console.ReadLine();
        }
        
        return result;
    }
}