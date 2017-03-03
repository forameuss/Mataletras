using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=234238

namespace Mataletras
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainMenu : Page
    {
        public MainMenu()
        {
            this.InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;            
            switch (b.Content.ToString())
            {
                case "Modo Normal":                    
                    this.Frame.Navigate(typeof(LevelsMenu));
                    break;
                case "Modo Extremo":                    
                    this.Frame.Navigate(typeof(LevelsMenu));
                    break;
                case "Modo Salir":
                    CoreApplication.Exit();
                    break;
            }
        }
    



     private void FormName_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if ((this.Width != 500) & (this.Height != 600))
            {
                this.Width = 500;
                this.Height = 600;
            }
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog showDialog = new MessageDialog("CREADO POR DANIEL GARCÍA Y ALBERTO NAVARRO");
            showDialog.Commands.Add(new UICommand("Ok"));
            showDialog.Commands.Add(new UICommand("Me parece correcto"));
            await showDialog.ShowAsync();
        }
    }
}
