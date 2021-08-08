using Devmoba.ToolClient.Shareds;
using RestSharp;
using System;
using System.Configuration;
using System.Net;
using System.Windows.Forms;

namespace Devmoba.ToolClient
{
    public partial class Login : Form
    {
        private string _serverUrl;
        private RestClient _restClient;

        public Login()
        {
            InitializeComponent();
            this.AcceptButton = btnLogin;
            _serverUrl = ConfigurationManager.AppSettings.Get(Constants.ServerUrl);
            var uri = new Uri(_serverUrl);
            _restClient = new RestClient($"{uri.Scheme}{Uri.SchemeDelimiter}{uri.Authority}");
            Load += Login_Load;
            FormClosing += Login_FormClosing;
        }
        private void Login_Load(object sender, EventArgs e)
        {
            txbUsername.Focus();
            var rememberMe = bool.Parse(ConfigurationManager.AppSettings.Get(Constants.RememberMe));
            if (rememberMe)
            {
                txbUsername.Text = ConfigurationManager.AppSettings.Get(Constants.Username);
                txbPassword.Text = ConfigurationManager.AppSettings.Get(Constants.Password);
                cbRememberMe.Checked = rememberMe;
            }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có thực sự muốn thoát chương trình?", "Thông báo", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                e.Cancel = true;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnLogin_ClickAsync(object sender, EventArgs e)
        {
            CookieContainer cookieContainer;
            var result = TryLogin(txbUsername.Text, txbPassword.Text, cbRememberMe.Checked, out cookieContainer);
            SetRemmeberMe(txbUsername.Text, txbPassword.Text, cbRememberMe.Checked);
            if (result)
            {
                var formMain = new Main(txbUsername.Text, cookieContainer);
                formMain.StartPosition = FormStartPosition.CenterScreen;
                this.Hide();
                formMain.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Lỗi đăng nhập do server chưa hoạt động hoặc sai username, password!", 
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private bool TryLogin(string userNameOrEmailAddress, string password, bool rememberMe, out CookieContainer cookieContainer)
        {
            var request = new RestRequest("/api/account/login", Method.POST, DataFormat.Json);
            request.AddJsonBody(new { userNameOrEmailAddress = userNameOrEmailAddress, password = password, rememberMe = true });
            var rs = _restClient.Execute(request);
            if (rs.IsSuccessful && rs.StatusCode == HttpStatusCode.OK && rs.Cookies.Count > 0)
            {
                cookieContainer = new CookieContainer();
                cookieContainer.Add(new Cookie()
                {
                    Name = rs.Cookies[0].Name,
                    Value = rs.Cookies[0].Value,
                    Domain = rs.Cookies[0].Domain
                });
                cookieContainer.Add(new Cookie()
                {
                    Name = rs.Cookies[1].Name,
                    Value = rs.Cookies[1].Value,
                    Domain = rs.Cookies[1].Domain
                });
                return true;
            }
            else
            {
                cookieContainer = null;
                return false;
            }
        }

        private void SetRemmeberMe(string username, string password, bool rememberMe)
        {
            var path = Application.ExecutablePath;
            var config = ConfigurationManager.OpenExeConfiguration(path);
            if (rememberMe)
            {
                config.AppSettings.Settings[Constants.Username].Value = username;
                config.AppSettings.Settings[Constants.Password].Value = password;
            }
            config.AppSettings.Settings[Constants.RememberMe].Value = rememberMe.ToString();
            config.Save(ConfigurationSaveMode.Minimal, true);
            ConfigurationManager.RefreshSection("appSettings");
        }


    }
}
