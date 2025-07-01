using AngleSharp;
using AngleSharp.Dom;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace web_site_parser
{
    public class TableParser
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TableParser));

        public TableData ParseTable(string url)
        {
            log.Info($"Начало парсинга таблицы с URL: {url}");

            try
            {
                var config = Configuration.Default.WithDefaultLoader();
                var context = BrowsingContext.New(config);
                var document = context.OpenAsync(url).GetAwaiter().GetResult();

                var table = document.QuerySelector("table") ?? throw new Exception("Таблица не найдена");
                log.Debug("Таблица найдена на странице");

                // Парсим заголовки
                var headers = table.QuerySelectorAll("thead th, tr:first-child th")
                    .Select(th => th.TextContent.Trim())
                    .ToList();

                log.Debug($"Найдено {headers.Count} заголовков");

                // Парсим данные
                var rows = new List<List<string>>();
                foreach (var row in table.QuerySelectorAll("tbody tr, tr:not(:first-child)"))
                {
                    var cells = row.QuerySelectorAll("td, th")
                        .Select(cell => cell.TextContent.Trim())
                        .ToList();

                    if (cells.Count == headers.Count)
                        rows.Add(cells);
                }

                log.Info($"Успешно распарсено {rows.Count} строк таблицы");
                return new TableData(headers, rows);
            }
            catch (Exception ex)
            {
                log.Error("Ошибка при парсинге таблицы", ex);
                throw;
            }
        }
    }
}