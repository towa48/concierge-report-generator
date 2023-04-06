using System;

namespace Concierge;

class ReportReader {
    private string _file;

    public ReportReader(string file) {
        _file = file;
    }

    public void ReadAsync() {
        Console.WriteLine(_file);
    }
}