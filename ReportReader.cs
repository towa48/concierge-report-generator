using System;
using System.IO;
using ExcelDataReader;

namespace Concierge;

class ReportReader {
    private string _file;
    private string _tab;
    private int _month;

    public ReportReader(string file, string tab, int month) {
        _file = file;
        _tab = tab;
        _month = month;
    }

    public void ReadAsync() {
        using var stream = File.Open(_file, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(stream);
        var book = reader.AsDataSet();

        var tab = book.Tables[_tab];
        var row = 0;
        var column = 1;
        Console.WriteLine(tab?.Rows[row][column]);
    }
}