using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoAlgoSorter
{
    public partial class Form1 : Form
    {

        int[] TheArray;
        Graphics g;

        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            g = panel1.CreateGraphics();
            int NumEntries = panel1.Width;
            int MaxVal = panel1.Height;
            TheArray = new int[NumEntries];
            g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.DarkBlue), 0, 0, NumEntries, MaxVal);
            Random rand = new Random();
            for (int i = 0; i < NumEntries; i++)
            {
                TheArray[i] = rand.Next(0, MaxVal);
            }
            for (int i = 0; i < NumEntries; i++)
            {
                g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.MediumSpringGreen), i, MaxVal - TheArray[i], 1, MaxVal);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // dwn"implementation of a class has been instantiated here"
            ISortEngine se = new SortEngineBubble();
            se.DoWork(TheArray, g, panel1.Height);
        }
        // "if evrythin runnin on same thread, rest of app locked up, can't do async actions; problem, should run SortEngine on diffrnt thread, then UI responsive, whle wrk goin on in bckgrd (app can async, have more than 1 task simultaneously)"
    }
}
