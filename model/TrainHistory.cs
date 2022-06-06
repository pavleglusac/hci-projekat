using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCIProjekat.model
{
    public class TrainHistory
    {
        public List<Train> History { get; set; }
        public int Index { get; set; }

        public TrainHistory()
        {
            History = new List<Train>();
            Index = -1;
        }

        public Train Undo()
        {
            if (History == null) return null;
            if (History.Count == 0 || Index == 0) return null;
            return History[--Index];
        }

        public Train Redo()
        {
            if(History.Count() == Index + 1) return null;
            return History[++Index];
        }

        public void AddTrain(Train train)
        {
            System.Diagnostics.Debug.WriteLine($"before {History.Count} {Index}");
            if(Index < History.Count() - 1)
            {
                History.RemoveRange(Index + 1, History.Count() - 1 - Index);
                System.Diagnostics.Debug.WriteLine($"removing {Index - 1} {History.Count() - 1 - Index}");
            }
            Index++;
            History.Add(train);
            System.Diagnostics.Debug.WriteLine($"after {History.Count} {Index}");
        }

        public Boolean CanUndo()
        {
            return Index > 0;
        }

        public Boolean CanRedo()
        {
            return Index < History.Count() - 1;
        }

        public Train CurrentTrain()
        {
            if (Index == -1) return null;
            return History[Index];
        }
    }
}
