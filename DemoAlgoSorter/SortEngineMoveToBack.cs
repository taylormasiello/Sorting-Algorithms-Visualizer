using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAlgoSorter
{
    class SortEngineMoveToBack : ISortEngine
    {
        private int[] TheArray;
        private Graphics g;
        private int MaxVal;
        Brush BlueBrush = new System.Drawing.SolidBrush(System.Drawing.Color.DarkBlue);
        Brush GreenBrush = new System.Drawing.SolidBrush(System.Drawing.Color.MediumSpringGreen);

        private int CurrentListPointer = 0;


        public SortEngineMoveToBack(int[] TheArray_In, Graphics g_In, int MaxVal_In)
        {
            TheArray = TheArray_In;
            g = g_In;
            MaxVal = MaxVal_In;
        }


        public void NextStep()
        {
            if (CurrentListPointer >= TheArray.Count() - 1) CurrentListPointer = 0;
            if (TheArray[CurrentListPointer] > TheArray[CurrentListPointer + 1])
            {
                RotateFlipType(CurrentListPointer);
            }
            CurrentListPointer++;

            //^compares value in array currently looking at, to the next one that follows it;
            //then increments next element in array and returns
        }

        private void RotateFlipType(int currentListPointer)
        {
            int temp = TheArray[CurrentListPointer];
            int EndPoint = TheArray.Count() - 1;

            for (int i = CurrentListPointer; i < EndPoint; i++)
            {
                TheArray[i] = TheArray[i + 1];
                DrawBar(i, TheArray[i]);
            }

            TheArray[EndPoint] = temp;
            DrawBar(EndPoint, TheArray[EndPoint]);
        }
        //^when out-of-order element found, takes higher one and moves it to back of array
        //^"means everything between the point we found it to the end of the list, needs to be shifted towards the front of the list by one slot"

        private void DrawBar(int position, int height)
        {
            g.FillRectangle(BlueBrush, position, 0, 1, MaxVal);
            g.FillRectangle(GreenBrush, position, MaxVal - TheArray[position], 1, MaxVal);
        }


        public bool IsSorted()
        {
            for (int i = 0; i < (TheArray.Count() - 1); i++)
            {
                if (TheArray[i] > TheArray[i + 1]) return false;
            }
            return true;
        }

        public void ReDraw()
        {
            for (int i = 0; i < (TheArray.Count() - 1); ++i)
            {
                g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.MediumSpringGreen), i, MaxVal - TheArray[i], 1, MaxVal);
            }
        }
    }
}
