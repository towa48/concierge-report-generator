using System;
using System.Collections.Generic;
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

    public IReadOnlyDictionary<int, bool> Read() {
        using var stream = File.Open(_file, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(stream);
        var book = reader.AsDataSet();
        var tab = book.Tables[_tab];

        var aptColumn = 1;
        var monthColumn = 2*_month + 2;
        var secondColumnt = monthColumn + 2;
        var apartments = 133;

        var result = new Dictionary<int, bool>();
        for(var i=3; i<apartments+3; i++) {
            var apt = int.Parse(tab?.Rows[i][aptColumn].ToString());
            var amount = tab?.Rows[i][monthColumn].ToString();
            var amount2 = tab?.Rows[i][monthColumn].ToString();

            result.Add(apt, !string.IsNullOrEmpty(amount) || !string.IsNullOrEmpty(amount2));
        }

        return result;
    }
}
