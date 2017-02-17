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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using System.Threading.Tasks;
using Windows.Storage;

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
        private int ALTURA = 500;
        int contador = 0;
        int posNaveX= 225;
        int posNaveY = 500;
        int palActual= -1;

        public MainPage()
        {
            this.InitializeComponent();
            CoreWindow.GetForCurrentThread().KeyUp += pulsarTecla;
            CoreWindow.GetForCurrentThread().KeyDown += pulsarTeclaDireccion;
            random = new Random();
            palabrasActuales = new List<Palabra>();
            //palabras = new Palabra[3] { new Palabra("PELOTA"), new Palabra("CASA"), new Palabra("INTERNET") };
            //palabras = cargarPalabras("ms-appx:///Assets/textos/test.txt");
            //palabras = cargarPalabras("C:\\Users\\anavarro\\Documents\\test.txt");
            cargarPalabras()

            Canvas.SetLeft(nave, posNaveX);
            Canvas.SetTop(nave, posNaveY);

            //Timer            
            DispatcherTimer spawner = new DispatcherTimer();
            spawner.Interval = TimeSpan.FromSeconds(2);
            spawner.Tick += spawnPalabra;
            spawner.Tick += moverPalabras;
            spawner.Start();

        

        }

        

        private void spawnPalabra(object sender, object e)
        {
            Palabra p = SigPalabra();
            palabrasActuales.Add(p);
            p.x = random.Next(0, 400);
            p.y = random.Next(0, 50);
            Canvas.SetTop(p.textBlock, p.y);
            Canvas.SetLeft(p.textBlock, p.x);
            pagina.Children.Add(p.textBlock);
        }

        private void pulsarTeclaDireccion(CoreWindow sender, KeyEventArgs e)
        {
            if ((e.VirtualKey.ToString() == "Right"))
                moverNave("Left", 25);
            if ((e.VirtualKey.ToString() == "Left"))
                moverNave("Left", -25);
            if ((e.VirtualKey.ToString() == "Up"))
                moverNave("Top", -25);
            if ((e.VirtualKey.ToString() == "Down"))
                moverNave("Top", 25);
        }


        private void pulsarTecla(CoreWindow sender, KeyEventArgs e)
        {            
           
            List<Palabra> aux = new List<Palabra>(palabrasActuales);
            
            foreach (Palabra p in aux)
            {
                if (p.quitarLetra(Char.ToUpper(e.VirtualKey.ToString()[0])))
                {
                    disparar(p);
                    if (p.letras.Length == 0)
                    {
                        contador = contador + 1;
                        txtContador.Text = contador.ToString();
                        palabrasActuales.Remove(p);
                        pagina.Children.Remove(p.textBlock);
                        explotar(p.x, p.y);
                    }
                }
            }
            
            
        }

        private void moverNave(string direccion, int v)
        {
            Storyboard storyboard = new Storyboard();

            DoubleAnimation animacion = new DoubleAnimation();

            if (direccion == "Right" || direccion == "Left")
            {
                animacion.From = posNaveX;
                animacion.To = posNaveX + v;
            }
            else
            {
                animacion.From = posNaveY;
                animacion.To = posNaveY + v;
            }
            animacion.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            Storyboard.SetTarget(animacion, nave); Storyboard.SetTargetProperty(animacion, "(Canvas."+ direccion + ")");
            storyboard.Children.Add(animacion);
            storyboard.Begin();
            if (direccion == "Right" || direccion == "Left")
                posNaveX += v;
            else
                posNaveY += v;
        }
       



        /// <summary>
        /// Escribe una palabra aleatoria en el TextBlock de la MainPage
        /// </summary>
        public Palabra SigPalabra()
        {
            //return new Palabra(palabras[random.Next(0, palabras.Length)].letras);            
            palActual++;
            return palabras[palActual];
        }


        public void moverPalabras(object sender, object e)
        {
            List<Palabra> aux = new List<Palabra>(palabrasActuales);
            foreach (Palabra p in aux)
            {
                if (p.y > ALTURA)
                {
                    palabrasActuales.Remove(p);
                    pagina.Children.Remove(p.textBlock);
                }
                else
                    p.moverPalabra(50, 2050);
            }
        }


        public void disparar(Palabra p)
        {
            //Image bala = new Image() { Source = new BitmapImage(new Uri("ms-appx:///Assets/images/piyun.png", UriKind.Absolute))};
            Rectangle bala = new Rectangle() { Fill = new SolidColorBrush(Colors.Red)};
            bala.Height = 5; bala.Width = 5;
            Storyboard storyboard = new Storyboard();
            pagina.Children.Add(bala);

            DoubleAnimation translateYAnimation = new DoubleAnimation();
            
            translateYAnimation.From = Canvas.GetTop(nave);
            translateYAnimation.To = p.y;
            translateYAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            Storyboard.SetTarget(translateYAnimation, bala); Storyboard.SetTargetProperty(translateYAnimation, "(Canvas.Top)");
            storyboard.Children.Add(translateYAnimation);

            DoubleAnimation translateXAnimation = new DoubleAnimation();
            translateXAnimation.From = Canvas.GetLeft(nave)+23;            
            translateXAnimation.To = p.x;
            translateXAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            Storyboard.SetTarget(translateXAnimation, bala); Storyboard.SetTargetProperty(translateXAnimation, "(Canvas.Left)");
            storyboard.Children.Add(translateXAnimation);
            storyboard.Begin();
            storyboard.Completed += delegate (object s, object e)
            {
                pagina.Children.Remove(bala);
            };
        }

        private async void explotar(int x, int y)
        {
            Image explosion = new Image() { Source = new BitmapImage(new Uri("ms-appx:///Assets/images/explosion.gif", UriKind.Absolute)) };
            explosion.MaxHeight = 30;
            explosion.MaxWidth = 30;
            pagina.Children.Add(explosion);
            Canvas.SetLeft(explosion, x);
            Canvas.SetTop(explosion, y);
            await Task.Delay(1000);
            pagina.Children.Remove(explosion);
        }
        

        private async  void cargarPalabras(string v)
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///thedata.txt"));
            string s = File.ReadAllText(v);
            Palabra[] res = new Palabra[s.Split(' ').Length];
            int i = 0;
            foreach (string temp in s.Split(' '))
            {
                res[i] = new Palabra(temp);
                i++;
            }
            palabras = res;           
        }


        private void FormName_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if ((this.Width != 500) & (this.Height != 800))
            {
                this.Width = 500;
                this.Height = 800;
            }
        }
        
    }
}

