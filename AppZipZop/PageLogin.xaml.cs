using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Phone.Notification;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;

namespace AppZipZop
{
    public partial class PageLogin : PhoneApplicationPage
    {
        private string ip = "http://10.21.0.137/";

        private IsolatedStorageFile file;
        private IsolatedStorageFileStream filestream;
        private XmlSerializer xml;

        private string arquivo = "UsuarioDados.xml";

        public PageLogin()
        {
            InitializeComponent();
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            Models.Usuario usuario = new Models.Usuario
            {
                Nome = txbNome.Text
            };

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            string s = JsonConvert.SerializeObject(usuario);
            try
            {
                using (file = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (filestream = file.OpenFile(arquivo, FileMode.Create))
                    {
                        xml = new XmlSerializer(typeof(Models.Usuario));
                        xml.Serialize(filestream, usuario);

                    }
                }

                var content = new StringContent(s, Encoding.UTF8, "application/json");
                await httpClient.PostAsync("/20131011110029/api/usuario", content);
                MessageBox.Show("Usuário criado");
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            /*Dispatcher.BeginInvoke(() => {
               /* var settings = IsolatedStorageSettings.ApplicationSettings;
                settings.Add("Nome", usuario.Nome);
                settings.Save();

                
            });*/
        }
    }
}