using Lab2.Application;
using Lab2.Infrastructure.Context;
using Lab2.Presentation;

var context = new DataContext
{
    Path = "data.xml"
};

try
{
    context.Load();
}
catch (FileNotFoundException)
{
    context.Seed();
    context.Save();
}

var dataService = new DataService(context);
var queryService = new QueryService(context);
var queriesPrinter = new QueriesPrinter(queryService);
var menu = new Menu(queriesPrinter, dataService);
menu.Print();
menu.Start();