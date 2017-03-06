using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
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
        public int tipo { get; set; }

        //Constructor
        public Palabra(string letras, int tipo, Color c)
        {
            this.letras = letras;
            textBlock = new TextBlock() { Text = letras };
            textBlock.Foreground = new SolidColorBrush(c);
            this.tipo = tipo;
        }

        /// <summary>
        /// Intenta quitar la primera letra de la palabra actual.
        /// </summary>
        /// <param name="letra">Letra a quitar de la palabra.</param>
        /// <returns>Devuelve true si se ha quitado con éxito y false en caso contrario.</returns>
        public bool quitarLetra(char letra)
        {
            bool res = false;
            if(letras.Length > 0)
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
        /// <param name="distancia">La distancia a recorrer</param>
        /// <param name="milisegundos">El tiempo que tarda en recorrer la distancia</param>
        public void moverPalabra(int distancia, int milisegundos)
        {
            Storyboard storyboard = new Storyboard();

            DoubleAnimation animacion = new DoubleAnimation();

            animacion.From = y;            
            animacion.To = y+distancia;            
            animacion.Duration = new Duration(TimeSpan.FromMilliseconds(milisegundos));
            Storyboard.SetTarget(animacion, textBlock); Storyboard.SetTargetProperty(animacion, "(Canvas.Top)");
            storyboard.Children.Add(animacion);
            storyboard.Begin();
            y += distancia;
        }
        
    }
}
