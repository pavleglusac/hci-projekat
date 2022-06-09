using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCIProjekat.model
{
    public class HistoryManager<T>
    {
        public List<T> History { get; set; }
        public int Index { get; set; }

        public HistoryManager()
        {
            History = new List<T>();
            Index = -1;
        }

        public T? Undo()
        {
            if (History == null) return default(T);
            if (History.Count == 0 || Index == 0) return default;
            return History[--Index];
        }

        public T? Redo()
        {
            if(History.Count() == Index + 1) return default;
            return History[++Index];
        }

        public void AddEntry(T other)
        {
            if(other.Equals(CurrentEntry()) && Index != -1)
            {
                return;
            }    

            if(Index < History.Count() - 1)
            {
                History.RemoveRange(Index + 1, History.Count() - 1 - Index);
            }
            Index++;
            History.ForEach(x => System.Diagnostics.Debug.Write($"{x.ToString()} ->"));
            System.Diagnostics.Debug.WriteLine("");
            History.Add(other);
        }

        public Boolean CanUndo()
        {
            return Index > 0;
        }

        public Boolean CanRedo()
        {
            return Index < History.Count() - 1;
        }

        public T? CurrentEntry()
        {
            if (Index == -1) return default;
            return History[Index];
        }
    }
}
