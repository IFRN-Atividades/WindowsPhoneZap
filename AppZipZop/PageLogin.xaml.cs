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
using System.Xml;

namespace AppZipZop
{
    public partial class PageLogin : PhoneApplicationPage
    {
        private string ip = "http://10.21.0.137/";

        private IsolatedStorageFile file;
        private IsolatedStorageFileStream filestream;
        private XmlReader xmlReader;
        private XmlSerializer xml;

        private Models.Mensagens mensagens = new Models.Mensagens();

        private string arquivo = "UsuarioDados.xml";
        private string arquivomensagem = "UsuarioMensagens.xml";

        public void saveMessageToFile(string txt1, string txt2)
        {
            Models.Mensagem m = new Models.Mensagem
            {
                Texto1 = txt1,
                Texto2 = txt2
            };
            using (file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                xml = new XmlSerializer(typeof(Models.Mensagens));
                bool existe = file.FileExists(arquivomensagem);

                using (filestream = file.OpenFile(arquivomensagem, FileMode.OpenOrCreate))
                {
                    if (!existe)
                        using (var writer = XmlWriter.Create(filestream))
                            xml.Serialize(writer, new Models.Mensagens());

                    filestream.Seek(0, SeekOrigin.Begin);

                    using (xmlReader = XmlReader.Create(filestream)) {
                        foreach (Models.Mensagem emi in (Models.Mensagens)xml.Deserialize(filestream))
                        {
                            mensagens.Add(emi);
                        }

                        mensagens.Add(m);
                        xml.Serialize(filestream, mensagens);
                    }
                }
            }
        }

        public bool checkIfMessageFileExists()
        {
            using (file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!file.FileExists(arquivomensagem)) return false;
            }
            return true;
        }

        public PageLogin()
        {

            InitializeComponent();
        }

        private string channelName = "channelIfrn";

        private async void cadastrarUsuario(string nome, string uri)
        {
            Models.Usuario usuario = new Models.Usuario
            {
                Nome = nome,
                Uri = uri
            };

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            string s = JsonConvert.SerializeObject(usuario);
            try
            {
                var content = new StringContent(s, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("/20131011110061/api/usuario", content);
                //string c = await response.Content.ReadAsStringAsync();
                usuario.Id = int.Parse(await response.Content.ReadAsStringAsync());
                using (file = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (filestream = file.OpenFile(arquivo, FileMode.Create))
                    {
                        xml = new XmlSerializer(typeof(Models.Usuario));
                        xml.Serialize(filestream, usuario);
                    }
                }
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {      
            HttpNotificationChannel httpChannel = HttpNotificationChannel.Find(channelName);

            try
            {
                // Verifica se o canal de notificação existe
                if (httpChannel == null)
                {
                    // Canal de notificação não existe, instancia novo canal.
                    httpChannel = new HttpNotificationChannel(channelName);

                    // Delegates para atualização, erro e recebimento de mensagem
                    httpChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(httpChannel_ChannelUriUpdated);
                    httpChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(httpChannel_ErrorOccurred);
                    httpChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(httpChannel_ShellToastNotificationReceived);

                    // Abre o canal de notificação com o Microsoft Push Notification Service
                    httpChannel.Open();

                    // Efetiva a ligação com o canal de notificação
                    httpChannel.BindToShellToast();
                }
                else
                {
                    // Canal existe

                    // Delegates para atualização, erro e recebimento de mensagem
                    httpChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(httpChannel_ChannelUriUpdated);
                    httpChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(httpChannel_ErrorOccurred);
                    httpChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(httpChannel_ShellToastNotificationReceived);

                    // Mostra dados do canal
                    System.Diagnostics.Debug.WriteLine(httpChannel.ChannelUri.ToString());
                    // MessageBox.Show(String.Format("O canal URI é {0}", httpChannel.ChannelUri.ToString()));

                    // Registra o usuário no serviço de usuários
                    cadastrarUsuario(txbNome.Text, httpChannel.ChannelUri.ToString());
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
        }

        private void httpChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                // Mostra dados do canal
                System.Diagnostics.Debug.WriteLine(e.ChannelUri.ToString());
                // MessageBox.Show(String.Format("O canal URI é {0}", e.ChannelUri.ToString()));

                // Registra o usuário no serviço de usuários
                cadastrarUsuario(txbNome.Text, e.ChannelUri.ToString());
            });
        }

        private void httpChannel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            // Mostra dados do erro
            Dispatcher.BeginInvoke(() => MessageBox.Show(String.Format("A push notification {0} error occurred.  {1} ({2}) {3}",
                e.ErrorType, e.Message, e.ErrorCode, e.ErrorAdditionalData)));
        }

        private void httpChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            // Notificação recebida com o aplicativo aberto
            // Este método é chamado e os dados vem no parâmetro e.Collection (NotificationEventArgs)

            // Página destino
            string relativeUri = string.Empty;

            // Mensagem apresentada ao usuário
            StringBuilder msg = new StringBuilder();
            msg.AppendFormat("Toast Recebido {0}:\n", DateTime.Now.ToShortTimeString());

            // Recupera dados da notificação
            foreach (string key in e.Collection.Keys)
            {
                msg.AppendFormat("{0}: {1}\n", key, e.Collection[key]);

                // Página destino
                if (string.Compare(key, "wp:Param",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.CompareOptions.IgnoreCase) == 0)
                {
                    relativeUri = e.Collection[key];
                }
            }

            // Mostra dados da notificação
            Dispatcher.BeginInvoke(() =>
            {
                // Mensagem apresentada ao usuário
                MessageBox.Show(msg.ToString());
                // Atualiza lista de mensagens
                saveMessageToFile(e.Collection["wp:Text1"], e.Collection["wp:Text2"]);
                
            });
        }
       


    }
}
