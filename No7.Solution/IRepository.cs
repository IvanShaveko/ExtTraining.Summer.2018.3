using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace No7.Solution
{
    //Интерфейс для различных репозториев
    public interface IRepository
    {
        void Save(List<TradeRecord> trades);
    }
}
