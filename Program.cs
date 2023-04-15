using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Concierge;

class Program
{
    static async Task Main(string[] args) {
        // Set encoder to read xlsx
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

        var configuration = builder.Build();
        var settingsSection = configuration.GetSection("AppSettings");

        var fileName = settingsSection["Input"];
        if (string.IsNullOrEmpty(fileName)) {
            throw new ArgumentNullException("Input");
        }

        var tab = settingsSection["Tab"];
        if (string.IsNullOrEmpty(tab)) {
            throw new ArgumentNullException("Tab");
        }

        var month = settingsSection["Month"];
        if (string.IsNullOrEmpty(month)) {
            throw new ArgumentNullException("Month");
        }

        var reader = new ReportReader(fileName, tab, int.Parse(month));
        var neighbors = reader.Read();

        var outputFile = settingsSection["OutputFile"];
        if (string.IsNullOrEmpty(outputFile)) {
            throw new ArgumentNullException("OutputFile");
        }

        var generator = new ReportGenerator(outputFile.Replace("{Tab}", tab).Replace("{Month}", month));
        await generator.CreateReportAsync(neighbors, month, tab);

        Console.WriteLine("Done.");
    }
}
