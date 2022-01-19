using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
////using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoAlgoSorter
{
    public partial class AlgoSorter : Form
    {

        int[] TheArray;
        Graphics g;
        BackgroundWorker bgw = null;
        bool Paused = false;

        public AlgoSorter()
        {
            InitializeComponent();
            PopulateDropdown(); 
        }

        private void PopulateDropdown()
        {
            List<string> ClassList = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(ISortEngine).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => x.Name).ToList();
            ClassList.Sort();
            foreach (string entry in ClassList)
            {
                comboBox1.Items.Add(entry);
            }
            comboBox1.SelectedIndex = 0;

        }
        // ^"gets names of classes that implement SortEngine interface, sorts alphabetically, then populates dropdown"

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (TheArray == null) btnReset_Click(null, null);
            //^fixes exception if you click Start before reset button; auto-invokes reset if you click start instead
            bgw = new BackgroundWorker();
            bgw.WorkerSupportsCancellation = true;
            bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
            // ^"adds an event handler to the DoWork event, when it fires; that's why there's a += instead of =, bc it's adding to a list of things that happens when the event fires, so we're adding one"
            bgw.RunWorkerAsync(argument: comboBox1.SelectedItem);

            ////// dwn"implementation of a class has been instantiated here"
            ////ISortEngine se = new SortEngineBubble();
            ////se.DoWork(TheArray, g, panel1.Height);
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (!Paused)
            {
                // dwn"sends signal to cancel bgw, SortEngine"
                bgw.CancelAsync();
                Paused = true;
            }
            else
            {
                if (bgw.IsBusy) return;
                //^pause/resume rapid quick too fast for bgw; this will ignore clicks if bgw running
                // dwn"reminds of variables; chngs state of Paused flag"
                int NumEntries = panel1.Width;
                int MaxVal = panel1.Height;
                Paused = false;
                for (int i = 0; i < NumEntries; i++)
                {
                    g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.DarkBlue), i, 0, 1, MaxVal);
                    g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.MediumSpringGreen), i, MaxVal - TheArray[i], 1, MaxVal);
                }
                // ^"fills in display to auto-refresh view; in case display disrupted somehow, this will refresh before we go on"
                bgw.RunWorkerAsync(argument: comboBox1.SelectedItem);
                // ^"(supposedly) not actually resuming bgw, but creating a new one; consumes more memory than resuming would, so likely creating new one; will pick up where SortEngine left off (thus resuming partially sorted list); every sortingAlgo will NOT be able to be paused/resumed in this way, depends on the algo (some are not interruptible)"

            }
        }

        ////private void label1_Click(object sender, EventArgs e)
        ////{
        ////}

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

        #region BackGroundWorkerStuff

        public void bgw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // dwn"explicitly casts sender(the thing that caused the method to be invoked); makes easier as intellisense has right context"
            BackgroundWorker bw = sender as BackgroundWorker;
            string SortEngineName = (string)e.Argument;
            // ^"extracts name of SortEngine we wanted from argument e.Argument; from ln 53"
            Type type = Type.GetType("DemoAlgoSorter." + SortEngineName);
            // ^"kno name of SortEngine want to run, here we figure out the actual type, using Reflection; will figure out type of the concrete class that's going to implement the algorithm (identifying the class we're going to create"
            var ctors = type.GetConstructors();
            // ^"examined the type, and got a list of its constructors"
            try
            {
                // dwn"create a SortEngine of the type identified, and invoke(create an instance of that class) its constructor; how to do it when you don't know which type, how to identify class when you don't know, below example if you did:"
                ISortEngine se = (ISortEngine)ctors[0].Invoke(new object[] { TheArray, g, panel1.Height });
                while (!se.IsSorted() && (!bgw.CancellationPending))
                {
                    se.NextStep();
                }


                //ISortEngine se2 = new SortEngineBubble(TheArray, g, panel1.Height);
                ////^"how to do same thing, but only if you already knew the type (of algo) to run"
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

    }
}

// "if evrythin runnin on same thread, rest of app locked up, can't do async actions; problem, should run SortEngine on diffrnt thread, then UI responsive, whle wrk goin on in bckgrd (app can async, have more than 1 task simultaneously)"
