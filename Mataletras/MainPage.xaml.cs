using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Mataletras
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Random random;

        //Dani was here

        public MainPage()
        {
            this.InitializeComponent();
            CoreWindow.GetForCurrentThread().KeyUp += Grid_KeyUp;
            random = new Random();
            SigLetra();
        }

        public void SigLetra()
        {
            int num = random.Next(0, 26);
            char let = (char)('A' + num);
            Texto.Text = let.ToString();
        }

        private void Grid_KeyUp(CoreWindow sender, KeyEventArgs e)
        {           
            if (e.VirtualKey.ToString() == Texto.Text)
                SigLetra();
        }
    }
}
