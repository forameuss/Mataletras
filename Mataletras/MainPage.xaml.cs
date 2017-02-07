using Mataletras.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics;
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

namespace Mataletras
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Random random;
        private Palabra[] palabras;
        private Palabra palabraActual;        

        public MainPage()
        {
            this.InitializeComponent();
            CoreWindow.GetForCurrentThread().KeyUp += pulsarTecla;
            random = new Random();
            palabras = new Palabra[3] { new Palabra("PELOTA"), new Palabra("CASA"), new Palabra("INTERNET") };
            SigPalabra();
        }

        

        private void pulsarTecla(CoreWindow sender, KeyEventArgs e)
        {
            if (palabraActual.quitarLetra(Char.ToUpper(e.VirtualKey.ToString()[0])))
            {
                if (palabraActual.letras.Length==0)
                    SigPalabra();                
                else
                    Texto.Text = palabraActual.letras;
            }                
            /*if (e.VirtualKey.ToString() == Texto.Text)
                SigLetra();*/
        }


        /// <summary>
        /// Escribe una letra aleatoria en el TextBlock de la MainPage
        /// </summary>
        public void SigLetra()
        {
            int num = random.Next(0, 26);
            char let = (char)('A' + num);
            Texto.Text = let.ToString();
        }


        /// <summary>
        /// Escribe una palabra aleatoria en el TextBlock de la MainPage
        /// </summary>
        public void SigPalabra()
        {
            palabraActual = new Palabra(palabras[random.Next(0, palabras.Length)].letras);
            Debug.Write(palabraActual.letras);
            Texto.Text = palabraActual.letras;
        }
    }   
}
