using AngleSharp;
using AngleSharp.Dom;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;

namespace web_site_parser
{
    public class TableParser
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TableParser));

        public TableData ParseTable()
        {
            string url = "https://www.wienerborse.at/en/indices-austria/";
            log.Debug($"Начало парсинга таблицы с URL: {url}");


            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = context.OpenAsync(url).GetAwaiter().GetResult();
            if (document == null || document.DocumentElement == null)
            {
                log.Error($"Не удалось загрузить документ по URL: {url}");
            }
            var table = document.QuerySelector("table") ?? throw new Exception("Таблица не найдена");
            log.Debug("Таблица найдена на странице");


            var headers = table.QuerySelectorAll("thead th, tr:first-child th")
                .Select(th => th.TextContent.Trim())
                .ToList();
            log.Debug($"Найдено {headers.Count} заголовков");

            var rows = new List<List<string>>();

            try
            {

                foreach (var row in table.QuerySelectorAll("tbody tr, tr:not(:first-child)"))
                {
                    var cells = row.QuerySelectorAll("td, th")
                        .Select(cell => cell.TextContent.Trim())
                        .ToList();

                    if (cells.Count == headers.Count)
                        rows.Add(cells);
                    else
                    {
                        log.Error("Количество заголовков не совпадает с количеством столбцов");
                    }
                }

                log.Info($"Успешно распарсено {rows.Count} строк таблицы");

            }
            catch (Exception ex)
            {
                log.Error("Ошибка при парсинге таблицы", ex);
            }
            return new TableData(headers, rows);
        }
    }
}