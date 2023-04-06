using System;
using Microsoft.Extensions.Configuration;

namespace Concierge;

class Program
{
    static void Main(string[] args) {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

        var configuration = builder.Build();

        var fileName = configuration.GetSection("AppSettings")["Input"];
        if (string.IsNullOrEmpty(fileName)) {
            throw new ArgumentNullException("Input");
        }

        var reader = new ReportReader(fileName);
        reader.ReadAsync();

        Console.WriteLine("Done.");
    }
}
