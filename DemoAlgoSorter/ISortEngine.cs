using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAlgoSorter
{
    public interface ISortEngine
    {
        // dwn"chngd interface bc we chngd the routines its goin to need to offer to its implementers ("implemented" is like "inherited")
        // dwn"chngd proj from runnin in a single thread to instead run on a bckgrnd thread; main form functionality while SortEngine runs"
        // dwn"chngd from 1 to multiple .DoWork() methods"
        void NextStep();
        bool IsSorted();
        void ReDraw();
    }
}
