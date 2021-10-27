using ReportToolManager.NetCore;
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
        //private const string ServerAddress = "https://toolmgr.devmoba.com";
        private const string ServerAddress = "https://localhost:44308";
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

        private async void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(SleepInterval);
            try
            {
                await CallReportAsync();
            }
            catch (Exception) {}
        }

        private async void SampleApp_Load(object sender, System.EventArgs e)
        {
            try
            {
                await CallReportAsync();
                _worker.RunWorkerAsync();
            }
            catch (Exception) { }
        }

        private async Task CallReportAsync()
        {
            await _report.ReportAsync(
                   CommonMethod.GetAppName(),
                   CommonMethod.GetAppId().Value,
                   CommonMethod.GetAppVersion());
        }

    }
}
