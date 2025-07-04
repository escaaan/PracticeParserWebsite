using System;
using System.IO;
using log4net;
using log4net.Config;
using web_site_parser;

namespace web_site_parser
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            log.Info("Запуск приложения TableParser");

            string url = "https://www.wienerborse.at/en/indices-cee/";
            log.Debug($"Введен URL: {url}");

            var parser = new TableParser();
            var tableData = parser.ParseTable(url);

            string csvPath = CsvExporter.SaveAsCsv(tableData);
            log.Info("Приложение закончило работу");

        }
    }
}