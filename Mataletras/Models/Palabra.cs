using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mataletras.Models
{
    public class Palabra
    {
        //Atributos
        public string letras { get; set; }
        public int velocidad { get; set; }

        //Constructor
        public Palabra(string letras)
        {
            this.letras = letras;
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
                res = true;
            } 
            return res;
        }
    }
}
