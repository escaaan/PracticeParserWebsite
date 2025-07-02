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
                //writer.WriteLine(string.Join(";", tableData.Headers));// сохраняет каждый элемент в отдельную ячейку
                writer.WriteLine(string.Join("\t", tableData.Headers));  // сохраняет все элементы через таб в одну ячейку

                foreach (var row in tableData.Rows)
                {
                 //   writer.WriteLine(string.Join(";", row));// сохраняет каждый элемент в отдельную ячейку
                    writer.WriteLine(string.Join("\t", row));// сохраняет все элементы через таб в одну ячейку
                }
            }

            log.Debug($"Файл успешно сохранен: {fullPath}");
            return fullPath;
        }
    }
}


