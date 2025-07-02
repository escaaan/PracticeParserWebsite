using System;
using System.Collections.Generic;

namespace web_site_parser
{
    public class TableData
    {
        public List<string> Headers { get; set; }
        public List<List<string>> Rows { get; set; }

        public TableData(List<string> headers, List<List<string>> rows)
        {
            Headers = headers;
            Rows = rows;
        }
    }
}