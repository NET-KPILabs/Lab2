using Lab2.Domain.Enums;

namespace Lab2.Presentation;

public class QueriesMenu
{
    private readonly QueriesPrinter _queriesPrinter;

    public QueriesMenu(QueriesPrinter queriesPrinter)
    {
        _queriesPrinter = queriesPrinter;
    }
    
    public void Print() =>
        Console.WriteLine
        (
            "1. Weekly profit\n" +
            "2. All cars\n" +
            "3. All rentals`re sorted by price\n" +
            "4. All clients`re sorted by profit\n" +
            "5. All transport cars\n" +
            "6. All cars with rentals quantity\n" +
            "7. All available cars at the moment\n" +
            "8. The average special cars rental price\n" +
            "9. Cars quantity by different types\n" +
            "10. The cars with the most expensive rental price\n" +
            "11. The manufactures`re ordered by popularity\n" +
            $"12. The cars`re issued after {DateTime.UtcNow.Year - 10}\n" +
            "13. All clients`re sorted by full name\n" +
            "14. All clients`re grouped by countries\n" +
            "15. The most popular car\n" +
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
                case 1: _queriesPrinter.PrintProfit(DateTimeOffset.Now - TimeSpan.FromDays(7));
                    break;
                case 2: _queriesPrinter.PrintAllCars();
                    break;
                case 3: _queriesPrinter.PrintRentalsSortedByPrice();
                    break;
                case 4: _queriesPrinter.PrintClientsSortedByProfit();
                    break;
                case 5: _queriesPrinter.PrintCarsByType(CarType.Transport);
                    break;
                case 6: _queriesPrinter.PrintCarsWithRentalsQuantity();
                    break;
                case 7: _queriesPrinter.PrintAvailableCarsAtTheMoment();
                    break;
                case 8: _queriesPrinter.PrintAverageCarsTypeRentalPrice(CarType.Special);
                    break;
                case 9: _queriesPrinter.PrintCarsQuantityByType();
                    break;
                case 10: _queriesPrinter.PrintCarWithTheMostExpensiveRentalPrice();
                    break;
                case 11: _queriesPrinter.PrintManufacturersOrderedByPopularity();
                    break;
                case 12: _queriesPrinter.PrintCarsIssuedAfter(DateTime.UtcNow.Year - 10);
                    break;
                case 13: _queriesPrinter.PrintClientsSortedByFullName();
                    break;
                case 14: _queriesPrinter.PrintClientsByCountries();
                    break;
                case 15: _queriesPrinter.PrintTheMostPopularCar();
                    break;
                case 0: 
                    exit = true;
                    break;
            }
            
            Console.Write("\nEnter your choice: ");
            if(exit)
                Console.Clear();
        }       
    }
}