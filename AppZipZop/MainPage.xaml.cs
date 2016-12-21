
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AppZipZop.Resources;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;

namespace AppZipZop
{
    public partial class MainPage : PhoneApplicationPage
    {
        private string ip = "http://10.21.0.137";

        private IsolatedStorageFile file;
        private IsolatedStorageFileStream filestream;
        private XmlSerializer xml;
        private Models.Usuario usuario;

        private string arquivo = "UsuarioDados.xml";

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            
            Loaded += (s, e) =>
            {
                if (!checkifUserExists())
                {
                    NavigationService.Navigate(new Uri("/PageLogin.xaml", UriKind.Relative));
                }
                else
                {
                    abrirArquivo();
                   // getDados();
                }
            };
            if (checkifUserExists()) getDados();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void abrirArquivo()
        {
            using (file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (filestream = file.OpenFile(arquivo, FileMode.Open))
                {
                    xml = new XmlSerializer(typeof(Models.Usuario));
                    usuario = (Models.Usuario)xml.Deserialize(filestream);
                }
            }
        }

        private bool checkifUserExists()
        {
            using (file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!file.FileExists(arquivo)) return false;
            }
            return true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Notificação recebida com o aplicativo fechado
            // Este método é chamado e os dados vem na QueryString
            var dic = NavigationContext.QueryString;
            // Atualiza lista de mensagens
            //if (dic.ContainsKey("Msg1")) listMsg.Items.Add(dic["Msg1"]);
            //if (dic.ContainsKey("Msg2")) listMsg.Items.Add(dic["Msg2"]);
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}


        List<Models.Usuario> usuarios = new List<Models.Usuario>();

        public async void getDados()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);

            var response = await httpClient.GetAsync("/20131011110061/api/usuario");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Usuario> obj = JsonConvert.DeserializeObject<List<Models.Usuario>>(str);

            var response2 = await httpClient.GetAsync("/20131011110061/api/grupousuario");
            var str2 = response2.Content.ReadAsStringAsync().Result;
            List<Models.Grupo> grupos = JsonConvert.DeserializeObject<List<Models.Grupo>>(str2);
            var grupinhos = (from Models.Grupo g in grupos where g.IdAdm == usuario.Id select g).ToList();

            var response3 = await httpClient.GetAsync("/20131011110061/api/relgrupousuario");
            var str3 = response3.Content.ReadAsStringAsync().Result;
            List<Models.RelGrupoUsuario> relGrupo = JsonConvert.DeserializeObject<List<Models.RelGrupoUsuario>>(str3);
            var gP = (from Models.RelGrupoUsuario y in relGrupo where y.Usuario_Id == usuario.Id select y).ToList();

            List<Models.Grupo> gruposParticipa = new List<Models.Grupo>();

            foreach(Models.RelGrupoUsuario tu in gP)
            {
                foreach (Models.Grupo xu in grupos)
                {
                    if (tu.GrupoUsuario_Id == xu.Id)
                        gruposParticipa.Add(xu);
                }
            }

            usuarios = obj;
            
            ListaUsuario.ItemsSource = obj;
            ListaUsuariosAdm.ItemsSource = obj;
            listaGruposAdministrados.ItemsSource = grupinhos;
            ListaGrupos.ItemsSource = gruposParticipa;
            txtNomeUsuario.Text = usuario.Nome;

        }

        private async void btnEnviarUsuario_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);

            Models.Mensagem m = new Models.Mensagem
            {
                Uri = (ListaUsuario.SelectedItem as Models.Usuario).Uri,
                Texto1 = txtTituloUsuario.Text,
                Texto2 = txtTituloGrupo.Text,
                Param = "MainPage.xaml"
            };
            string s = "=" + JsonConvert.SerializeObject(m);
            var content = new StringContent(s, Encoding.UTF8,
                "application/json");
            await httpClient.PostAsync("/20131011110061/api/mensagem", content);
            MessageBox.Show("Acho que enviou");
        }

        private async void btnCriarGrupo_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);

            Models.Grupo g = new Models.Grupo
            {
                Descricao = txtNomeGrupo.Text,
                IdAdm = (ListaUsuariosAdm.SelectedItem as Models.Usuario).Id
            };
            string s = JsonConvert.SerializeObject(g);
            var content = new StringContent(s, Encoding.UTF8,"application/json");
            var response = await httpClient.PostAsync("/20131011110061/api/grupousuario", content);

            //Fazer a piruletagem para pegar o id do grupo quando criar

            Models.RelGrupoUsuario rel = new Models.RelGrupoUsuario
            {
                Usuario_Id = g.IdAdm,
                GrupoUsuario_Id = int.Parse(await response.Content.ReadAsStringAsync())
            };

            string s2 = JsonConvert.SerializeObject(rel);
            var content2 = new StringContent(s2, Encoding.UTF8, "application/json");
            var response2 = await httpClient.PostAsync("/20131011110061/api/relgrupousuario", content2);

            MessageBox.Show("Acho que criou");


            //getDados();

        }

        private async void btnEditarUsuário_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);

            var response = await httpClient.GetAsync("/20131011110061/api/usuario");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Usuario> obj = JsonConvert.DeserializeObject<List<Models.Usuario>>(str);

            Models.Usuario u = (from Models.Usuario user in obj where user.Id == usuario.Id select user).Single();
            u.Nome = txtNomeUsuario.Text;

            string s = JsonConvert.SerializeObject(u);
            var content = new StringContent(s, Encoding.UTF8,
                "application/json");
            await httpClient.PutAsync("/20131011110061/api/usuario/" + usuario.Id, content);
            MessageBox.Show("Acho que editou");
        }

        private async void btnDeletarUsuário_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            await httpClient.DeleteAsync("/20131011110061/api/usuario/" + usuario.Id);
            MessageBox.Show("Acho que Deletou");

            IsolatedStorageFile.GetUserStoreForApplication().Remove();

            NavigationService.Navigate(new Uri("/PageLogin.xaml", UriKind.Relative));
        }

        private void Button_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Grupo.xaml?id=" + (sender as Button).CommandParameter, UriKind.RelativeOrAbsolute));
        }
    }
}