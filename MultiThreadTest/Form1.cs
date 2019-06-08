using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiThreadTest
{
    public partial class Form1 : Form
    {
        private Worker _worker;
        public Form1()
        {
            InitializeComponent();

            startButton.Click += StartButton_Click;
            stopButton.Click += StopButton_Click;
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            if (_worker != null)
                _worker.Cancel();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            _worker = new Worker();
            _worker.ProcessChanged += Worker_ProcessChanged;
            _worker.WorkCompleted += Worker_WorkCompleted;

            startButton.Enabled = false;

            Thread thread = new Thread(_worker.Work);
            thread.Start();
        }
        
        private void Worker_WorkCompleted(bool cancelled)
        {
            Action action = () =>
            {
                string message = cancelled ? "Процесс отменен" : "Процесс завершен!";
                MessageBox.Show(message);
                startButton.Enabled = true;
            };

            this.InvokeEx(action);

            //if (InvokeRequired)  // пишем так, Если не создавать класс ControlHelper
            //    Invoke(action);
            //else
            //    action();
        }

        private void Worker_ProcessChanged(int progress)
        {
            Action action = () => 
            {
                progressBar.Value = progress + 1; // чтобы при нажатии на стоп прогрессбар незамедлительно останавливался
                progressBar.Value = progress;
            };

            this.InvokeEx(action);

            //if (InvokeRequired)  // пишем так, Если не создавать класс ControlHelper
            //    Invoke(action);
            //else
            //    action();
        }
    }
}
