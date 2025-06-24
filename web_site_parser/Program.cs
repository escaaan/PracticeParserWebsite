using System;
using System.IO;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using web_site_parser;

namespace web_site_parser
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        static async Task Main(string[] args)
        {
            
                      
            var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            log.Info("Запуск приложения TableParser");

            try
            {
                string url = "https://www.wienerborse.at/en/bonds/?c7928-page=1&per-page=50&c";
                log.Debug($"Введен URL: {url}");

                var parser = new TableParser();
                var tableData = await parser.ParseTable(url);

                log.Info($"Успешно распарсено: {tableData.Rows.Count} строк");

                string csvPath = CsvExporter.SaveAsCsv(tableData);

                Console.WriteLine($"\nДанные сохранены в: {csvPath}");
                Console.WriteLine($"\nЗаголовки: {string.Join(" | ", tableData.Headers)}");
                Console.WriteLine($"Всего строк: {tableData.Rows.Count}");

                log.Info($"Данные сохранены в файл: {csvPath}");
            }
            catch (Exception ex)
            {
                log.Error("Произошла ошибка в работе приложения", ex);
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
            finally
            {
                log.Info("Завершение работы приложения");
                Console.ReadKey();
            }
        }
    }
}