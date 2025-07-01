using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace web_site_parser
{
    public static class CsvExporter
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CsvExporter));

        public static string SaveAsCsv(TableData tableData)
        {
            log.Debug("Начало сохранения данных в CSV");

            string fileName = $"bonds_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.csv";
            string directory = Path.Combine(Directory.GetCurrentDirectory(), "Exports");

            Directory.CreateDirectory(directory);
            string fullPath = Path.Combine(directory, fileName);

            log.Info($"Создание файла: {fullPath}");

            using (var writer = new StreamWriter(fullPath, false, System.Text.Encoding.UTF8))
            {
                // Заголовки
                writer.WriteLine(string.Join(";", tableData.Headers.Select(EscapeCsvValue)));

                // Данные
                foreach (var row in tableData.Rows)
                {
                    writer.WriteLine(string.Join(";", row.Select(EscapeCsvValue)));
                }
            }

            log.Debug($"Файл успешно сохранен: {fullPath}");
            return fullPath;
        }

        private static string EscapeCsvValue(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";

            // Экранируем, если есть спецсимволы
            if (value.Contains(';') || value.Contains('"') || value.Contains('\n'))
            {
                return '"' + value.Replace("\"", "\"\"") + '"';
            }
            return value;
        }
    }
}