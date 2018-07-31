using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace No7.Solution
{
    public static class Repository
    {
        //Нужно для последущего расширения, если надо будет сохранять не только в базу данных
        // Если придет нулевая ссылка на репозиторий, по-стандарту сохранит в базу данных
        public static void Save(List<TradeRecord> trades, IRepository repository = null)
        {
            if (repository == null)
            {
                repository = new SaveToDatabase();;
            }
            repository.Save(trades);
        }
    }
}
