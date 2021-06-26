using Devmoba.ToolClient.Models;
using Devmoba.ToolClient.Services;
using Devmoba.ToolClient.Shareds;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;
using Newtonsoft.Json;
using NLog;
using NLog.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Console = Devmoba.ToolClient.Services.Console;

namespace Devmoba.ToolClient
{
    public partial class Main : Form
    {
        private string _serverUrl;
        private string _client;

        private HubConnection _connection;
        private CookieContainer _cookieContainer;
        private LoadScript _loadScript;
        private MainScript _mainScript;
        private readonly BackgroundWorker _worker;
        private Logger _logger;
        private int _interval;

        public Main(string client, CookieContainer cookieContainer)
        {

            InitializeComponent();
            this.ActiveControl = label1;
            _interval = GetInterval();
            _serverUrl = ConfigurationManager.AppSettings[Constants.ServerUrl];
            _client = client;
            _cookieContainer = cookieContainer;
            _loadScript = LoadScript.GetInstance();
            _mainScript = new MainScript();
            _worker = new BackgroundWorker();
            _worker.DoWork += _worker_DoWork;
            _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
            _worker.WorkerSupportsCancellation = true;
            SetConnection();
            FormClosed += Main_FormClosedAsync;
            Load += Main_Load;
            toolStripMenuItem1.Click += ToolStripMenuItem1_Click;
            toolStripMenuItem2.Click += ToolStripMenuItem2_Click;
            toolStripMenuItem3.Click += ToolStripMenuItem3_Click;
            this.Text = $"{_client} - target server: {_serverUrl}";
        }

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _worker.RunWorkerAsync();
        }

        private async void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Thread.Sleep(_interval);
                await UpdateSelfInfo();
            }
            catch (Exception) { }
        }

        private async Task UpdateSelfInfo()
        {
            await _connection.InvokeAsync(
                "UpdateClient",
                CommonMethods.GetIPLan(),
                //txtIP.Text, // fix ip để test
                CommonMethods.GetCpuPercentage(),
                CommonMethods.GetMemoryPercentage());
        }

        #region Event
        private void Main_Load(object sender, EventArgs e)
        {
            _logger = LogManager.GetLogger("reportLogger");
            _logger.Debug("Start reporting");
            _logger.Debug("Wait 3 seconds to connect to the server");
            ConnectionStart();
        }

        private async void ConnectionStart()
        {
            try
            {
                await _connection.StartAsync();
                await UpdateSelfInfo();
                if (!_worker.IsBusy)
                {
                    _worker.RunWorkerAsync();
                }
                _logger.Debug($"{_client} - Connection started");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e) // connect
        {
            ConnectionStart();
        }

        private async void ToolStripMenuItem2_Click(object sender, EventArgs e) // disconnect
        {
            try
            {
                await _connection.StopAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void ToolStripMenuItem3_Click(object sender, EventArgs e) // show log
        {
            try
            {
                var folderPath = Directory.GetCurrentDirectory();
                var filePath = $"{folderPath}//logs//report.log";
                Process.Start(@"notepad.exe", filePath);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private async void Main_FormClosedAsync(object sender, FormClosedEventArgs e)
        {
            await _connection.DisposeAsync();
            var logConfig = LogManager.Configuration;
            logConfig.RemoveTarget("operationLog");
        }

        #endregion
      
        private void SetConnection()
        {
            _connection = new HubConnectionBuilder()
               .WithUrl($"{_serverUrl}/exchange-hub", options =>
               {
                   options.Cookies = _cookieContainer;
               })
               .WithAutomaticReconnect(new[]
               {
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
               })
               .Build();

            _connection.Closed += async (error) =>
            {
                await Task.CompletedTask;
                _logger.Debug($"{_client} - Connection stopped");
            };

            _connection.On<string, List<Script>, string, bool>(
                "ReceiveFromUser",
                async (connectionId, dependencies, content, available) =>
            {
                _mainScript.Content = content;
                _mainScript.Dependencies = dependencies;
                _mainScript.Available = available;

                try
                {
                    var result = ExecuteMainScript(_mainScript);
                    await _connection.InvokeAsync("ReplyToUser", connectionId, _client, result);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
                Console.Result.Clear();
            });

            _connection.On<object>("ReceiveError", (obj) => { });

            _connection.On<string, string>("TurnOnTool", async (connectionId, exeFilePath) =>
            {
                var message = string.Empty;
                var isSuccess = true;

                try
                {
                    using (var process = Process.Start(exeFilePath))
                    {
                        _logger.Debug($"Process start - file path: {exeFilePath}");
                    }
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                    message = ex.Message;
                    _logger.Error(ex);
                }
                await _connection.InvokeAsync("ReportTurnOn", new ReportMessage()
                {
                    ConnectionId = connectionId,
                    ToolId = null,
                    IsSuccess = isSuccess,
                    ErrorMessage = message
                });
            });

            _connection.On<string, long, int>("TurnOffTool", async (connectionId, toolId, processId) =>
            {
                var message = string.Empty;
                var isSuccess = true;

                try
                {
                    var process = Process.GetProcessById(processId);

                    if (!process.HasExited)
                    {
                        process.Kill();
                        _logger.Debug($"Process kill - processId: {processId}");
                    } 
                    else
                    {
                        message = $"There is a problem that the application has stopped working. ProcessId {processId} has exited";
                        _logger.Debug(message);
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    isSuccess = false;
                    _logger.Error(ex);
                }

                await _connection.InvokeAsync("ReportTurnOff", new ReportMessage()
                {
                    ConnectionId = connectionId,
                    ToolId = toolId,
                    IsSuccess = isSuccess,
                    ErrorMessage = message
                });
            });
        }

        private List<string> ExecuteMainScript(MainScript mainScript)
        {
            using (var engine = new V8ScriptEngine(V8ScriptEngineFlags.EnableDebugging))
            {
                engine.AccessContext = typeof(Program);
                engine.AddHostObject("host", new HostFunctions());
                engine.AddHostObject("xHost", new ExtendedHostFunctions());
                engine.AddHostObject("lib", new HostTypeCollection("mscorlib", "System", "System.Core"));
                engine.AddHostType("Console", typeof(Console));
                engine.AddHostType("File", typeof(File));
                engine.AddHostType("Directory", typeof(Directory));
                engine.AddHostType("Path", typeof(Path));
                engine.AddHostType("Process", typeof(Process));
                engine.AddHostType("WebClient", typeof(WebClient));
                engine.AddHostType("Uri", typeof(Uri));
                
                //engine.AddHostType("object", typeof(object));

                try
                {
                    _loadScript.ExecuteLibraries(engine);
                    if (_mainScript.Dependencies.Count > 0)
                    {
                        var funcIncluded = _loadScript.CreateFuncIncluded(mainScript.Dependencies, engine);
                        engine.Execute(funcIncluded);
                    }
                    if (mainScript.Available)
                    {
                        var jsonResult = engine.Script.getFunctionInfo((mainScript.Content));
                        List<FunctionDeclaration> functionDeclarations = JsonConvert.DeserializeObject<List<FunctionDeclaration>>(jsonResult);

                        var excludeStatement = _loadScript.ExcludeStatement(mainScript.Content, functionDeclarations);
                        engine.Execute(excludeStatement);
                        if (functionDeclarations.Where(x => x.Name == "main").Any())
                        {
                            engine.Execute("main();");
                        }
                    }
                    else
                        engine.Execute(mainScript.Content);

                    if (Console.Result.Count == 0)
                        Console.Log($"Done");
                }
                catch (ScriptEngineException ex)
                {
                    Console.Log($"<span class='error-result'>{ex.Message}<br>{ex.ErrorDetails}</span>");
                    _logger.Error(ex);
                }
                return Console.Result;
            }
        }

        private int GetInterval()
        {
            var interval = ConfigurationManager.AppSettings[Constants.UpdateInterval];
            int time;
            if (int.TryParse(interval, out time))
                return time;
            return 5000;
        }
    }
}
