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
        private List<Palabra> palabrasActuales;
        Canvas canvas;

        public MainPage()
        {
            this.InitializeComponent();
            CoreWindow.GetForCurrentThread().KeyUp += pulsarTecla;
            random = new Random();
            palabrasActuales = new List<Palabra>();
            palabras = new Palabra[3] { new Palabra("PELOTA"), new Palabra("CASA"), new Palabra("INTERNET") };
            canvas = pagina;

            //Timer
            //Stopwatch reloj = new Stopwatch();
            DispatcherTimer spawner = new DispatcherTimer();
            spawner.Interval = TimeSpan.FromSeconds(2);
            spawner.Tick += spawnPalabra;
            spawner.Tick += moverPalabras;
            spawner.Start();

        /*    DispatcherTimer animationer = new DispatcherTimer();
            animationer.Interval = TimeSpan.FromSeconds(1);
           // animationer.Tick += moverPalabra;
            animationer.Start();
            */
            //reloj.Start();

        }

        private void spawnPalabra(object sender, object e)
        {
            Palabra p = SigPalabra();
            palabrasActuales.Add(p);
            p.x = random.Next(0, 500);
            p.y = random.Next(0, 50);
            Canvas.SetTop(p.textBlock, p.y);
            Canvas.SetLeft(p.textBlock, p.x);
            pagina.Children.Add(p.textBlock);
        }


        private void pulsarTecla(CoreWindow sender, KeyEventArgs e)
        {

            List<Palabra> aux = new List<Palabra>(palabrasActuales);
            
            foreach (Palabra p in aux)
            {
                if (p.quitarLetra(Char.ToUpper(e.VirtualKey.ToString()[0])))
                {
                    if (p.letras.Length == 0)
                    {
                        palabrasActuales.Remove(p);
                        pagina.Children.Remove(p.textBlock);
                    }
                }
            }
            
            
        }

        /// <summary>
        /// Escribe una palabra aleatoria en el TextBlock de la MainPage
        /// </summary>
        public Palabra SigPalabra()
        {
            return new Palabra(palabras[random.Next(0, palabras.Length)].letras);            
        }


        public void moverPalabras(object sender, object e)
        {            
            foreach (Palabra p in palabrasActuales)
            {
                p.moverPalabra(50, 1999);
            }
        }
    }
}

