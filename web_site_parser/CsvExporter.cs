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
            try
            {
                log.Debug($"Начало экспорта данных в CSV. Получено {tableData.Headers?.Count ?? 0} заголовков и {tableData.Rows?.Count ?? 0} строк");

                string fileName = $"bonds_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.csv";
                string directory = Path.Combine(Directory.GetCurrentDirectory(), "Exports");

                log.Info($"Подготовка директории для экспорта: {directory}");
                Directory.CreateDirectory(directory);

                string fullPath = Path.Combine(directory, fileName);
                log.Info($"Создание CSV файла по пути: {fullPath}");

                using (var writer = new StreamWriter(fullPath, false, System.Text.Encoding.UTF8))
                {
                    log.Debug($"Запись заголовков (используется табуляция как разделитель)");
                    writer.WriteLine(string.Join("\t", tableData.Headers));

                    log.Debug($"Начало записи строк данных (всего {tableData.Rows.Count})");
                    int counter = 0;
                    foreach (var row in tableData.Rows)
                    {
                        writer.WriteLine(string.Join("\t", row));
                        counter++;
                    }
                    log.Debug($"Всего записано строк: {counter}");
                }

                log.Debug($"CSV файл успешно сохранен. Размер: {new FileInfo(fullPath).Length} байт");

                return fullPath;
            }
            catch (Exception ex)
            {
                log.Error($"Ошибка при экспорте в CSV: {ex.Message}", ex);
                throw;
            }

        }
    }
}


