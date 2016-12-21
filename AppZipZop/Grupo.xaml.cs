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

namespace AppZipZop
{
    public partial class Grupo : PhoneApplicationPage
    {
        private string ip = "http://10.21.0.137";

        public Grupo()
        {
            InitializeComponent();
        }
        public Models.Grupo grupo;

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string parameter;

            if (NavigationContext.QueryString.TryGetValue("id", out parameter))
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ip);

                var response = await httpClient.GetAsync("/20131011110061/api/grupousuario");
                var str = response.Content.ReadAsStringAsync().Result;
                List<Models.Grupo> obj = JsonConvert.DeserializeObject<List<Models.Grupo>>(str);
                grupo = obj.Where(g => g.Id == int.Parse(parameter)).Single();

                txtDescricao.Text = grupo.Descricao;

                getUsuarios();
            }
            else
            {
                NavigationService.GoBack();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            getUsuarios();
        }

        public async void getUsuarios()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);

            var response = await httpClient.GetAsync("/20131011110061/api/relgrupousuario");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.RelGrupoUsuario> obj = JsonConvert.DeserializeObject<List<Models.RelGrupoUsuario>>(str);
            List<Models.RelGrupoUsuario> listaRel = (from Models.RelGrupoUsuario rel in obj where rel.GrupoUsuario_Id == grupo.Id select rel).ToList();

            var response2 = await httpClient.GetAsync("/20131011110061/api/usuario");
            var str2 = response2.Content.ReadAsStringAsync().Result;
            List<Models.Usuario> obj2 = JsonConvert.DeserializeObject<List<Models.Usuario>>(str2);

            List<Models.Usuario> usuarioslul = new List<Models.Usuario>();

            foreach(Models.RelGrupoUsuario rg in listaRel)
            {
                foreach(Models.Usuario u in obj2)
                {
                    if (u.Id == rg.Usuario_Id)
                        usuarioslul.Add(u);
                }
            }


            Usuarios.ItemsSource = obj2;
            listaUsuárioGrupo.ItemsSource = usuarioslul;
            Administradores.ItemsSource = usuarioslul;
            //Administradores.SelectedItem = (from Models.Usuario u in usuarioslul where u.Id == grupo.IdAdm select u).Single();
        }

        private async void Button_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);

            var response = await httpClient.DeleteAsync("/20131011110061/api/relgrupousuario/" + (sender as Button).CommandParameter.ToString());
            //getUsuarios();
        }

        private async void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);

            Models.RelGrupoUsuario g = new Models.RelGrupoUsuario
            {
                Usuario_Id = (Usuarios.SelectedItem as Models.Usuario).Id,
                GrupoUsuario_Id = grupo.Id
            };
            string s = JsonConvert.SerializeObject(g);
            var content = new StringContent(s, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/20131011110061/api/relgrupousuario", content);

            //getUsuarios();
        }

        private async void btnDeletarGrupo_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);
            await httpClient.DeleteAsync("/20131011110061/api/grupousuario/" + grupo.Id);

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private async void btnEditarGrupo_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ip);

            var response = await httpClient.GetAsync("/20131011110061/api/grupousuario");
            var str = response.Content.ReadAsStringAsync().Result;
            List<Models.Grupo> obj = JsonConvert.DeserializeObject<List<Models.Grupo>>(str);

            Models.Grupo u = (from Models.Grupo g in obj where g.Id == grupo.Id select g).Single();
            u.Descricao = txtDescricao.Text;

            string s = JsonConvert.SerializeObject(u);
            var content = new StringContent(s, Encoding.UTF8,
                "application/json");
            await httpClient.PutAsync("/20131011110061/api/grupousuario/" + grupo.Id, content);
            MessageBox.Show("Acho que editou");
        }
    }
}