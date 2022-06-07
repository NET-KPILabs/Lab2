using System.Xml;
using Lab2.Domain.Entities;

namespace Lab2.Infrastructure.Context;

public class DataContext
{
    public event Action? OnChange;
    public string Path { get; init; } = string.Empty;
    public ICollection<Car> Cars { get; set; } = new List<Car>();
    
    public void Save()
    {
        if (string.IsNullOrEmpty(Path))
        {
            throw new ArgumentException("Path cannot be null or empty");
        }
        
        var setting = new XmlWriterSettings()
        {
            Indent = true
        };

        using(var writer = XmlWriter.Create(Path, setting))
            writer.WriteCarsToXml(Cars);
        OnChange?.Invoke();
    }
}