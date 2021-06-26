using Devmoba.ReportToToolManager;
using System;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using TestReport.Shareds;

namespace TestReport
{
    public partial class TestReport : Form
    {
        private const string ServerAddress = "https://toolmgr.devmoba.com/";
        private const int Interval = 180000;
        private readonly BackgroundWorker _worker;
        private readonly ReportTool _report;

        public TestReport()
        {
            InitializeComponent();
            _worker = new BackgroundWorker();
            _worker.DoWork += _worker_DoWork;
            _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
            _report = new ReportTool(ServerAddress);
            txtStatus.Text = "Test Report is running...";
            Text = $"Test Report - Target Server: {ServerAddress}";
        }

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _worker.RunWorkerAsync();
        }

        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(Interval);
            try
            {
                CallReport();
            }
            catch (Exception) { }
        }

        private void TestReport_Load(object sender, EventArgs e)
        {
            try
            {
                CallReport();
                _worker.RunWorkerAsync();
            }
            catch (Exception) {}
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
