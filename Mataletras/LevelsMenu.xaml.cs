using Mataletras.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class LevelsMenu : Page
    {
        public LevelsMenu()
        {
            this.InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Nivel nivel = null;
            switch (b.Content.ToString())
            {
                case "Nivel 1":
                    nivel = new Nivel("Nivel1",50,2050,2f);
                    this.Frame.Navigate(typeof(ModoNormal), nivel);
                    break;
                case "Nivel 2":
                    nivel = new Nivel("genesis", 50, 2050, 2f);
                    this.Frame.Navigate(typeof(ModoNormal), nivel);
                    break;
                case "¡Atrás!":
                    this.Frame.Navigate(typeof(MainMenu));
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

    }
}
