﻿#pragma checksum "C:\Users\Nudes\workspace\ifrn\WindowsPhoneZap\AppZipZop\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A6A564A58BDEFB2AC25E01977C4EA4CD"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace AppZipZop {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal Microsoft.Phone.Controls.LongListSelector listMsg;
        
        internal Microsoft.Phone.Controls.LongListSelector listaGruposAdministrados;
        
        internal System.Windows.Controls.TextBox txtTituloUsuario;
        
        internal System.Windows.Controls.TextBox txtMensagemUsuario;
        
        internal Microsoft.Phone.Controls.ListPicker ListaUsuario;
        
        internal System.Windows.Controls.Button btnEnviarUsuario;
        
        internal System.Windows.Controls.TextBox txtTituloGrupo;
        
        internal System.Windows.Controls.TextBox txtMensagemGrupo;
        
        internal Microsoft.Phone.Controls.ListPicker ListaGrupos;
        
        internal System.Windows.Controls.Button btnEnviarGrupo;
        
        internal System.Windows.Controls.TextBox txtNomeGrupo;
        
        internal Microsoft.Phone.Controls.ListPicker ListaUsuariosAdm;
        
        internal System.Windows.Controls.Button btnCriarGrupo;
        
        internal System.Windows.Controls.TextBox txtNomeUsuario;
        
        internal System.Windows.Controls.Button btnEditarUsuário;
        
        internal System.Windows.Controls.Button btnDeletarUsuário;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/AppZipZop;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.listMsg = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("listMsg")));
            this.listaGruposAdministrados = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("listaGruposAdministrados")));
            this.txtTituloUsuario = ((System.Windows.Controls.TextBox)(this.FindName("txtTituloUsuario")));
            this.txtMensagemUsuario = ((System.Windows.Controls.TextBox)(this.FindName("txtMensagemUsuario")));
            this.ListaUsuario = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("ListaUsuario")));
            this.btnEnviarUsuario = ((System.Windows.Controls.Button)(this.FindName("btnEnviarUsuario")));
            this.txtTituloGrupo = ((System.Windows.Controls.TextBox)(this.FindName("txtTituloGrupo")));
            this.txtMensagemGrupo = ((System.Windows.Controls.TextBox)(this.FindName("txtMensagemGrupo")));
            this.ListaGrupos = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("ListaGrupos")));
            this.btnEnviarGrupo = ((System.Windows.Controls.Button)(this.FindName("btnEnviarGrupo")));
            this.txtNomeGrupo = ((System.Windows.Controls.TextBox)(this.FindName("txtNomeGrupo")));
            this.ListaUsuariosAdm = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("ListaUsuariosAdm")));
            this.btnCriarGrupo = ((System.Windows.Controls.Button)(this.FindName("btnCriarGrupo")));
            this.txtNomeUsuario = ((System.Windows.Controls.TextBox)(this.FindName("txtNomeUsuario")));
            this.btnEditarUsuário = ((System.Windows.Controls.Button)(this.FindName("btnEditarUsuário")));
            this.btnDeletarUsuário = ((System.Windows.Controls.Button)(this.FindName("btnDeletarUsuário")));
        }
    }
}

