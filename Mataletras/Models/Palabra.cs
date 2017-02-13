using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Mataletras.Models
{
    public class Palabra
    {
        //Atributos
        public string letras { get; set; }
        public TextBlock textBlock { get; set; }
        public int x { get; set;}
        public int y { get; set; }

        //Constructor
        public Palabra(string letras)
        {
            this.letras = letras;
            textBlock = new TextBlock() { Text = letras };
        }

        /// <summary>
        /// Intenta quitar la primera letra de la palabra actual.
        /// </summary>
        /// <param name="letra">Letra a quitar de la palabra.</param>
        /// <returns>Devuelve true si se ha quitado con éxito y false en caso contrario.</returns>
        public bool quitarLetra(char letra)
        {
            bool res = false;
            if (letras[0] == letra){
                letras = letras.Remove(0,1);
                textBlock.Text = letras;
                res = true;
            } 
            return res;
        }

        /// <summary>
        /// Mueve un textBlock hacia abajo
        /// </summary>
        /// <param name="tb">El TextBlock a mover</param>
        /// <param name="distancia">La distancia a recorrer</param>
        /// <param name="milisegundos">El tiempo que tarda en recorrer la distancia</param>
        public void moverPalabra(int distancia, int milisegundos)
        {
        /*    Storyboard storyboard = new Storyboard();

            DoubleAnimation translateYAnimation = new DoubleAnimation();


            translateYAnimation.From = 0;
            y = distancia;
            translateYAnimation.To = distancia;
            translateYAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(milisegundos));
            //Storyboard.SetTarget(translateYAnimation, tb); Storyboard.SetTargetProperty(translateYAnimation, "(Canvas.RenderTransform).(TranslateTransform.Y)");
            storyboard.Children.Add(translateYAnimation); */
        }
    }
}
