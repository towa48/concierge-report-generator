using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Xml;

namespace Concierge;

public class ReportGenerator {
    const string templateFile = "template.odt";
    private string _outputFile;

    public ReportGenerator(string outputFile) {
        _outputFile = outputFile;
    }

    public Task CreateReportAsync(IReadOnlyDictionary<int, bool> neighbors, string month, string year) {
        var tmpFilePath = Path.GetTempFileName();
        if (File.Exists(tmpFilePath)) {
            File.Delete(tmpFilePath);
        }
        File.Copy(templateFile, tmpFilePath);

        var outputPath = GetFileNameWithoutExtension(tmpFilePath);
        ZipFile.ExtractToDirectory(tmpFilePath, outputPath);

        var contentFile = Path.Combine(outputPath, "./content.xml");
        var doc = new XmlDocument();
        doc.Load(contentFile);

        XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
        manager.AddNamespace("table", "urn:oasis:names:tc:opendocument:xmlns:table:1.0");
        manager.AddNamespace("text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0");

        var monthNode = doc?.DocumentElement?.SelectSingleNode($"//text:span[text()='месяц-месяц']", manager);
        if (monthNode != null) {
            monthNode.InnerText = GetMonthsText(month);
        }

        var yearNode = doc?.DocumentElement?.SelectSingleNode($"//text:span[text()='год']", manager);
        if (yearNode != null) {
            yearNode.InnerText = year;
        }

        foreach(var kvp in neighbors) {
            var node = doc?.DocumentElement?.SelectSingleNode($"//table:table-cell//text:p[text()='{kvp.Key}']", manager);

            if (node?.Attributes != null) {
                var styleAttribute = node.Attributes["text:style-name"];
                if (styleAttribute != null) {
                    styleAttribute.Value = kvp.Value ? "P9" : "P10";
                }
            }
        }

        doc?.Save(contentFile);

        var outputFile = Path.Combine(Directory.GetCurrentDirectory(), $"./{_outputFile}");
        if (File.Exists(outputFile)) {
            File.Delete(outputFile);
        }
        ZipFile.CreateFromDirectory(outputPath, outputFile);

        // cleanup
        File.Delete(tmpFilePath);
        Directory.Delete(outputPath, true);

        return Task.FromResult(true);
    }

    private static string GetFileNameWithoutExtension(string filePath) {
        var ind = filePath.LastIndexOf('.');
        return filePath.Substring(0, ind);
    }

    private static string GetMonthsText(string firstMonths) {
        if (!int.TryParse(firstMonths, out var month)) {
            return "?-?";
        }

        var months = new string[] { "январь", "февраль", "март", "апрель", "май", "июнь", "июль", "август", "сентябрь", "октяюрь", "ноябрь", "декабрь" };
        return $"{months[month-1]}-{months[month]}";
    }
}