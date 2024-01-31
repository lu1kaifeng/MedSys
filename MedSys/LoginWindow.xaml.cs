using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MedSys
{
    class LoginDataContext : INotifyPropertyChanged
    {
        public string BaseURL { get; set; }
        public string UserName { get; set; }

        public bool Valid { get {
                //Uri uriResult;
                bool urlValid = true; //Uri.TryCreate(BaseURL, UriKind.Absolute, out uriResult)
        //&& (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                return urlValid && UserName != null && UserName!= "";
            } }
        public string Reason
        {
            get
            {
                string reason = "";
                //Uri uriResult;
                bool urlValid = true;//Uri.TryCreate(BaseURL, UriKind.Absolute, out uriResult)
        //&& (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                if (!urlValid) reason += "服务器地址错误；";
                if (UserName == null || UserName == "") reason += "用户名为空；";
                return reason;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        LoginDataContext LoginDataContext = new LoginDataContext();
        Dictionary<string, string> info = new Dictionary<string, string>();
        private static string SavePath = System.Environment.SpecialFolder.LocalApplicationData + "\\info.txt";


        public static void NoAuto()
        {
            if (File.Exists(SavePath))
            {
                var info = InfoLoad();
                info["autoLogin"] = false.ToString();
                if (File.Exists(SavePath)) File.Delete(SavePath);
                Directory.CreateDirectory(System.Environment.SpecialFolder.LocalApplicationData.ToString());
                File.CreateText(SavePath).Close();
                File.WriteAllLines(SavePath,
                    info.Select(x => $"{x.Key},{x.Value}"));
            }
        }

        public LoginWindow()
        {
            if(File.Exists(SavePath))
            {
                info = InfoLoad();
                LoginDataContext.BaseURL = info["BaseURL"];
                LoginDataContext.UserName = info["UserName"];
                InitializeComponent();
                DataContext = LoginDataContext;
                pbPassword.Password = info["Password"];
                cbRemember.IsChecked = true;
                if (bool.Parse(info["autoLogin"]))
                {
                    cbAuto.IsChecked = true;
                    Button_Click(this, null);
                }
            }
            InitializeComponent();
            DataContext = LoginDataContext;
        }
        private static Dictionary<string, string> InfoLoad()
        {
            Dictionary<string,string> info= new Dictionary<string,string>();
            var lines = File.ReadLines(System.Environment.SpecialFolder.LocalApplicationData+"\\info.txt");
            foreach (var line in lines)
            {
                string[] arr = line.Split(',');
                info.Add(arr[0], String.Join(",", arr.Skip(1)));
            }
            return info;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (LoginDataContext.Valid)
            {             
                if(cbRemember
                   .IsChecked == true)
                {
                    info["BaseURL"] = LoginDataContext.BaseURL;
                    info["UserName"] = LoginDataContext.UserName;
                    info["Password"] = pbPassword.Password;
                    info["autoLogin"] = cbAuto.IsChecked.ToString();
                    if (File.Exists(SavePath)) File.Delete(SavePath);
                    Directory.CreateDirectory(System.Environment.SpecialFolder.LocalApplicationData.ToString());
                    File.CreateText(SavePath).Close();
                    File.WriteAllLines(SavePath,
                        info.Select(x => $"{x.Key},{x.Value}"));
                }
                GridMain.IsEnabled = false;
                //Client Action
                //HttpClient httpClient = new HttpClient();
                //SerialCommunication.Client client = new SerialCommunication.Client(httpClient);
                //client.BaseUrl = LoginDataContext.BaseURL;
                Dispatcher.Invoke(async () =>
                {
                    try
                    {
                        //string token = await client.LoginAsync(LoginDataContext.UserName, pbPassword.Password);
                        //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                        updateConnectionString(LoginDataContext.BaseURL, LoginDataContext.UserName, pbPassword.Password);
                    }catch(Exception ex)
                    {
                        Application.Current.Dispatcher.Invoke(() => {
                            MessageBox.Show("登陆错误：" + ex.Message);
                        });
                        this.Hide();
                        new LoginWindow().Show();
                        this.Close();
                        return;
                    }
                    Application.Current.Dispatcher.Invoke(() => {
                        this.Hide();
                        new MainWindow().Show();
                        this.Close();
                    });
                });

            }
            else
            {
                MessageBox.Show(LoginDataContext.Reason);
            }
        }

        private void cbRemember_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void cbRemember_Unchecked(object sender, RoutedEventArgs e)
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
            }
        }

        private void updateConnectionString(string dataSource,string userName,string password) 
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            var cnntStr = connectionStringsSection.ConnectionStrings["medEntities"].ConnectionString;
            //var regex = new Regex("(.*);provider connection string=\"(.*)\"(.*)");
            //var result = regex.Match(cnntStr);
            //result.Groups[1];
            connectionStringsSection.ConnectionStrings["medEntities"].ConnectionString =
                "metadata=res://*/MedModel.csdl|res://*/MedModel.ssdl|res://*/MedModel.msl;provider=System.Data.SqlClient;provider connection string=\"data source="+dataSource+ ";User Id="+userName+";Password="+password+";" + "initial catalog=med;multipleactiveresultsets=True;application name=EntityFramework\"" ;
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");
            try
            {
                using (var ctx = new medEntities())
                {
                    var count = ctx.meds.Count();
                }
            }
            catch (Exception ex)
            {
                connectionStringsSection.ConnectionStrings["medEntities"].ConnectionString = cnntStr;
                config.Save();
                ConfigurationManager.RefreshSection("connectionStrings");
                throw ex;
            }
        }

        private void cbAuto_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void cbAuto_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
