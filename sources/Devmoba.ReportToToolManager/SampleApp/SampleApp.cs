using Devmoba.ReportToToolManager;
using SampleApp.Shareds;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SampleApp
{
    public partial class SampleApp : Form
    {
        //private const string ServerAddress = "https://toolmgr.devmoba.com/";
        private const string ServerAddress = "https://localhost:44308/";
        private const int SleepInterval = 180000;
        private readonly BackgroundWorker _worker;
        private readonly ReportTool _report;

        public SampleApp()
        {
            InitializeComponent();
            _worker = new BackgroundWorker();
            _worker.DoWork += _worker_DoWork;
            _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
            _report = new ReportTool(ServerAddress);
            txtStatus.Text = "Sample App is running...";
            Text = $"Sample App - Target Server: {ServerAddress}";
        }

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _worker.RunWorkerAsync();
        }

        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(SleepInterval);
            try
            {
                CallReport();
            }
            catch (Exception) {}
        }

        private void SampleApp_Load(object sender, System.EventArgs e)
        {
            try
            {
                CallReport();
                _worker.RunWorkerAsync();
            }
            catch (Exception) { }
        }

        private void CallReport()
        {
            _report.Report(
                   CommonMethod.GetAppName(),
                   CommonMethod.GetAppId().Value,
                   CommonMethod.GetAppVersion(),
                   CommonMethod.GetExeFilePath(),
                   CommonMethod.GetProcessId());
        }

    }
}
