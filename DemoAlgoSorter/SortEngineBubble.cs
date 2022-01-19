using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
////using System.Threading.Tasks;

namespace DemoAlgoSorter
{
    class SortEngineBubble : ISortEngine
    {
        ////private bool _sorted = false;
        private int[] TheArray;
        private Graphics g;
        // ^"graphics object"
        private int MaxVal;
        Brush BlueBrush = new System.Drawing.SolidBrush(System.Drawing.Color.DarkBlue);
        Brush GreenBrush = new System.Drawing.SolidBrush(System.Drawing.Color.MediumSpringGreen);
        // ^"painting rectangles into the graphics object"

        public SortEngineBubble(int[] TheArray_In, Graphics g_In, int MaxVal_In)
        {
            TheArray = TheArray_In;
            g = g_In;
            MaxVal = MaxVal_In;
        }
        // ^" _In for input; "made local, better practice"

        public void NextStep()
        {
            for (int i = 0; i < (TheArray.Count() - 1); i++)
            {
                if (TheArray[i] > TheArray[i + 1])
                {
                    Swap(i, i + 1);
                }
            }
        }

        private void Swap(int i, int p)
        {
            int temp = TheArray[i];
            TheArray[i] = TheArray[i + 1];
            TheArray[i + 1] = temp;

            DrawBar(i, TheArray[i]);
            DrawBar(p, TheArray[p]);
        }

        //    REFACTORED FOR BETTER REUSEABILITY
        //    g.FillRectangle(BlueBrush, i, 0, 1, MaxVal);
        //    g.FillRectangle(BlueBrush, p, 0, 1, MaxVal);
        //    // ^"removes old values frm display, shows blue bckgrnd behind"

        //    g.FillRectangle(GreenBrush, i, MaxVal - TheArray[i], 1, MaxVal);
        //    g.FillRectangle(GreenBrush, p, MaxVal - TheArray[p], 1, MaxVal);
        //    // ^"shows new values"

        //// "non-optimized version of algorithm, challenge to optimize in future"
        //// "ex^"lrg imporvement: combine drw elmnts frm lns 47 & 48 into sngle 2pxl wide, instead of 2 at 1pxl wide"
        //// "ex^sml improvement: bblSort, aftr pass made thru arry, dnt need to go to end of arry again, bc you kno last item is in place; could go to nxt to last item, then 1 before, then 1 before that"

        private void DrawBar(int position, int height)
        {
            g.FillRectangle(BlueBrush, position, 0, 1, MaxVal);
            g.FillRectangle(GreenBrush, position, MaxVal - TheArray[position], 1, MaxVal);
        }
        // consolidated functionality into its own method

        public bool IsSorted()
        {
            for (int i = 0; i < (TheArray.Count() - 1); i++)
            {
                if (TheArray[i] > TheArray[i + 1]) return false;
            }
            return true;
        }
        // ^"IsSorted ? test"

        public void ReDraw()
        {
            for (int i = 0; i < (TheArray.Count() - 1); ++i)
            {
                g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.MediumSpringGreen), i, MaxVal - TheArray[i], 1, MaxVal);
            }
        }
    }
}
