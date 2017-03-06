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
using Windows.UI.Popups;

namespace Mataletras
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class ModoNormal : Page
    {
        private Random random;
        private Palabra[] palabras;
        private List<Palabra> palabrasActuales;
        private int ALTURA = 500;
        int contador = 0;
        int posNaveX = 225;
        int posNaveY = 500;
        int palActual = -1;
        bool queSigaLaFiesta = true;
        DispatcherTimer spawner;
        Nivel nivel;

        public int DISTANCIA_RECORRIDA_PALABRA = 50;
        public int DURACION_RECORRER_PALABRA = 1050;
        public float INTERVALO_TICK = 1f;
        public string NOMBRE_NIVEL = "";

        public MediaElement musica;
        public MediaElement disparo1;
        public MediaElement disparo2;
        public MediaElement explosionM;        




        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Nivel parameter = e.Parameter as Nivel;
            nivel = parameter;          
            
            DISTANCIA_RECORRIDA_PALABRA = parameter.DISTANCIA_RECORRIDA_PALABRA;
            DURACION_RECORRER_PALABRA = parameter.DURACION_RECORRER_PALABRA;
            INTERVALO_TICK = parameter.INTERVALO_TICK;

            cargarPalabras(parameter.NOMBRE_ARCHIVO);
            cargarSonidos(parameter.NOMBRE_ARCHIVO);

            //Timer            
            spawner = new DispatcherTimer();
            spawner.Interval = TimeSpan.FromSeconds(INTERVALO_TICK);
            spawner.Tick += spawnPalabra;
            spawner.Tick += moverPalabras;
            spawner.Start();
        }

       

        public ModoNormal()
        {
            this.InitializeComponent();
            CoreWindow.GetForCurrentThread().KeyUp += pulsarTecla;
            CoreWindow.GetForCurrentThread().KeyDown += pulsarTeclaDireccion;
            random = new Random();
            palabrasActuales = new List<Palabra>();
            

            posNaveX=(int) Canvas.GetLeft(nave);
            posNaveY = (int)Canvas.GetTop(nave);


            

        }

        



        private void spawnPalabra(object sender, object e)
        {
            if (queSigaLaFiesta)
            {
                Palabra p = SigPalabra();
                palabrasActuales.Add(p);
                p.x = random.Next(0, 400);
                //p.y = random.Next(0, 50);
                p.y = 25;
                Canvas.SetTop(p.textBlock, p.y);
                Canvas.SetLeft(p.textBlock, p.x);
                pagina.Children.Add(p.textBlock);                
            }
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
                    if (disparo1.CurrentState == MediaElementState.Playing)
                        disparo1.Play();
                    else
                        disparo2.Play();

                    disparar(p);
                    if (p.letras.Length == 0)
                    {
                        contador = contador + 1;
                        txtContador.Text = contador.ToString();
                        palabrasActuales.Remove(p);
                        pagina.Children.Remove(p.textBlock);
                        explotar(p.x, p.y);
                        
                        if (palabrasActuales.Count==0&&!queSigaLaFiesta)
                          gameGanado();
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
            Storyboard.SetTarget(animacion, nave); Storyboard.SetTargetProperty(animacion, "(Canvas." + direccion + ")");
            storyboard.Children.Add(animacion);
            storyboard.Begin();
            if (direccion == "Right" || direccion == "Left")
                posNaveX += v;
            else
                posNaveY += v;
        }




        public Palabra SigPalabra()
        {
            palActual++;
            if ((palActual + 1) == palabras.Length)
                queSigaLaFiesta = false;
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
                    int vidasActuales = txtVidas.Text.Length;
                    if (vidasActuales > 1)
                        txtVidas.Text = txtVidas.Text.Substring(0, vidasActuales - 1);
                    else
                        gameOver();

                }
                else
                    p.moverPalabra(DISTANCIA_RECORRIDA_PALABRA, DURACION_RECORRER_PALABRA);                    
            }
        }


        public void disparar(Palabra p)
        {            
            //Image bala = new Image() { Source = new BitmapImage(new Uri("ms-appx:///Assets/images/piyun.png", UriKind.Absolute))};
            Rectangle bala = new Rectangle() { Fill = new SolidColorBrush(Colors.Red) };
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
            translateXAnimation.From = Canvas.GetLeft(nave) + 23;
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
            explosionM.Play();
            Image explosion = new Image() { Source = new BitmapImage(new Uri("ms-appx:///Assets/images/explosion.gif", UriKind.Absolute)) };
            explosion.MaxHeight = 30;
            explosion.MaxWidth = 30;
            pagina.Children.Add(explosion);
            Canvas.SetLeft(explosion, x);
            Canvas.SetTop(explosion, y);
            await Task.Delay(1000);
            pagina.Children.Remove(explosion);
        }


        private void cargarPalabras(string v)
        {
            string s = System.IO.File.ReadAllText(".\\Assets\\textos\\"+v+".txt");

            Palabra[] res = new Palabra[s.Split(' ').Length];
            int i = 0;
            foreach (string temp in s.Split(' '))
            {
                res[i] = new Palabra(temp.ToUpper());
                i++;
            }
            palabras = res;
        }


        private async void cargarSonidos(string nOMBRE_ARCHIVO)
        {
            musica = new MediaElement();
            musica.IsLooping = true;
            StorageFolder Folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            Folder = await Folder.GetFolderAsync("\\Assets\\sound\\");
            StorageFile sf = await Folder.GetFileAsync("music.mp3");
            musica.SetSource(await sf.OpenAsync(FileAccessMode.Read), sf.ContentType);
            
            //musica.Play();

            disparo1= new MediaElement();
            disparo2 = new MediaElement();
            disparo1.AutoPlay = false;
            disparo2.AutoPlay = false;                        
            sf = await Folder.GetFileAsync("disparo.mp3");
            disparo1.SetSource(await sf.OpenAsync(FileAccessMode.Read), sf.ContentType);
            disparo2.SetSource(await sf.OpenAsync(FileAccessMode.Read), sf.ContentType);

            explosionM = new MediaElement();
            explosionM.AutoPlay = false;            
            sf = await Folder.GetFileAsync("explosion.mp3");
            explosionM.SetSource(await sf.OpenAsync(FileAccessMode.Read), sf.ContentType);            
        }


        private async void gameOver()
        {            
            musica.Stop();
            spawner.Stop();
            Image explosion = new Image() { Source = new BitmapImage(new Uri("ms-appx:///Assets/images/gameOver.gif", UriKind.Absolute)) };
            explosion.MaxHeight = 300;
            explosion.MaxWidth = 300;
            pagina.Children.Add(explosion);
            Canvas.SetLeft(explosion, 150);
            Canvas.SetTop(explosion, 150);
            await Task.Delay(1000);
            pagina.Children.Remove(explosion);
            mensajePerdedor();            
        }

        private void gameGanado()
        {
            musica.Stop();
            mensajeGanador();
        }

        private async void mensajePerdedor()
        {
            MessageDialog showDialog = new MessageDialog("La tierra ha sido destruida, ha muerto mucha gente, entre ellos Donald Trump. ¿Deseas reiniciar el nivel?");
            showDialog.Commands.Add(new UICommand("Sí") { Id = 0 });
            showDialog.Commands.Add(new UICommand("No") { Id = 1 });
            showDialog.Title = "GAME OVER";
            showDialog.DefaultCommandIndex = 0;
            showDialog.CancelCommandIndex = 1;
            var result = await showDialog.ShowAsync();
            if ((int)result.Id == 0)
                this.Frame.Navigate(typeof(ModoNormal), nivel);
            else
                this.Frame.Navigate(typeof(MainMenu));
        }

        private async void mensajeGanador()
        {
            MessageDialog showDialog = new MessageDialog("La tierra ha sido salvada, ha sobrevivido mucha gente, entre ellos Donald Trump");
            showDialog.Commands.Add(new UICommand("Continuar") { Id = 0 });
            showDialog.Commands.Add(new UICommand("Reiniciar") { Id = 1 });
            showDialog.Title = "YU WIN";
            showDialog.DefaultCommandIndex = 0;
            showDialog.CancelCommandIndex = 1;
            var result = await showDialog.ShowAsync();
            if ((int)result.Id == 0)
            {
                string txt = nivel.NOMBRE_ARCHIVO.Substring(0, 5);
                string lvl = nivel.NOMBRE_ARCHIVO.Substring(5, 1);
                lvl = ""+(int.Parse(lvl) + 1);
                nivel.NOMBRE_ARCHIVO = txt + lvl;
                this.Frame.Navigate(typeof(ModoNormal), nivel);
            }
            else
                this.Frame.Navigate(typeof(ModoNormal), nivel);
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
