using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace No7.Solution
{
    //Разделили на отдельные методы, чтоб каждый метод выполнял свою роль
    public class TradeHandler
    {
        //Стандартный размер лота
        private readonly float LotSize = 100000f;

        public TradeHandler()
        {
        }

        //Добавил конструктор с параметром, если размер лота будет другой
        public TradeHandler(float lotSize)
        {
            LotSize = lotSize;
        }

        // Метод, который вызывает различные методы для сохранения трейдов в репозиторий
        // Добавил параметр IRepository для того, чтобы, если надо будет сохранять в другой репозиторий была возможность 
        // расширить добавив класс с реализованным интерфейсом
        // При смене репозитория не будет проблем расширить код 
        public void HandleTrades(Stream stream, IRepository repository = null)
        {
            var lines = StreamToList(stream);
            var trades = new List<TradeRecord>();
            GetTrades(trades, lines);
            Repository.Save(trades, repository);
            Console.WriteLine("INFO: {0} trades processed", trades.Count);
        }

        private static List<string> StreamToList(Stream stream)
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }

        // Разделил на 2 метода валидацию и на получение трейдов
        private void GetTrades(List<TradeRecord> trades, List<string> lines)
        {
            var lineCount = 1;
            foreach (var line in lines)
            {
                var fields = line.Split(new char[] {','});

                if (Valiation(fields, lineCount, out var tradeAmount, out var tradePrice))
                {
                    var sourceCurrencyCode = fields[0].Substring(0, 3);
                    var destinationCurrencyCode = fields[0].Substring(3, 3);

                    var trade = new TradeRecord(destinationCurrencyCode, sourceCurrencyCode, tradeAmount / LotSize,
                        tradePrice);

                    trades.Add(trade);
                }

                lineCount++;
            }
        }

        // В decimal добавил культуру
        private static bool Valiation(string[] fields, int lineCount, out int tradeAmount, out decimal tradePrice)
        {
            if (fields.Length != 3)
            {
                Console.WriteLine("WARN: Line {0} malformed. Only {1} field(s) found.", lineCount,
                    fields.Length);

                tradeAmount = 0;
                tradePrice = 0;
                return false;
            }

            if (fields[0].Length != 6)
            {
                Console.WriteLine("WARN: Trade currencies on line {0} malformed: '{1}'", lineCount,
                    fields[0]);

                tradeAmount = 0;
                tradePrice = 0;
                return false;
            }

            if (!int.TryParse(fields[1], out tradeAmount))
            {
                Console.WriteLine("WARN: Trade amount on line {0} not a valid integer: '{1}'", lineCount,
                    fields[1]);
                tradePrice = 0;
                return false;
            }

            if (!decimal.TryParse(fields[2], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out tradePrice))
            {
                Console.WriteLine("WARN: Trade price on line {0} not a valid decimal: '{1}'", lineCount,
                    fields[2]);
                return false;
            }

            return true;
        }
    }
}
