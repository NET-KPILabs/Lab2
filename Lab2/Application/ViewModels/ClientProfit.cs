using Lab2.Domain.Entities;

namespace Lab2.Application.ViewModels;

public class ClientProfit
{
    public Client Client { get; set; }
    public decimal TotalProfit { get; set; }
}