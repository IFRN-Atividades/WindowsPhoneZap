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

namespace AppZipZop
{
    public partial class MainPage : PhoneApplicationPage
    {
        private string ip = "http://10.21.0.137";
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            getUsuarios();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Notificação recebida com o aplicativo fechado
            // Este método é chamado e os dados vem na QueryString
            var dic = NavigationContext.QueryString;
            // Atualiza lista de mensagens
            if (dic.ContainsKey("Msg1")) listMsg.Items.Add(dic["Msg1"]);
            if (dic.ContainsKey("Msg2")) listMsg.Items.Add(dic["Msg2"]);
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


        private Models.Usuario usuario;

        List<Models.Usuario> usuarios = new List<Models.Usuario>();

        public async void getUsuarios()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);

            var response = await httpClient.GetAsync("/20131011110029/api/usuuario");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Usuario> obj = JsonConvert.DeserializeObject<List<Models.Usuario>>(str);
            usuarios = obj;
        }
    }
}