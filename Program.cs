using System;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Concierge;

class Program
{
    static void Main(string[] args) {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

        var configuration = builder.Build();

        var fileName = configuration.GetSection("AppSettings")["Input"];
        if (string.IsNullOrEmpty(fileName)) {
            throw new ArgumentNullException("Input");
        }

        var tab = configuration.GetSection("AppSettings")["Tab"];
        if (string.IsNullOrEmpty(tab)) {
            throw new ArgumentNullException("Tab");
        }

        var month = configuration.GetSection("AppSettings")["Month"];
        if (string.IsNullOrEmpty(month)) {
            throw new ArgumentNullException("Month");
        }

        var reader = new ReportReader(fileName, tab, int.Parse(month));
        reader.ReadAsync();

        Console.WriteLine("Done.");
    }
}
