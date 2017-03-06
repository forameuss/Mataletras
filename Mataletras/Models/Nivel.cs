using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mataletras.Models
{
    public class Nivel
    {
        public string NOMBRE_ARCHIVO { get; set; }         
        public int DISTANCIA_RECORRIDA_PALABRA { get; set; }
        public int DURACION_RECORRER_PALABRA { get; set; }
        public float INTERVALO_TICK { get; set; }
        
        public Nivel(string NOMBRE_ARCHIVO, int DISTANCIA_RECORRIDA_PALABRA, int DURACION_RECORRER_PALABRA, float INTERVALO_TICK)
        {
            this.DISTANCIA_RECORRIDA_PALABRA = DISTANCIA_RECORRIDA_PALABRA;
            this.DURACION_RECORRER_PALABRA = DURACION_RECORRER_PALABRA;
            this.INTERVALO_TICK = INTERVALO_TICK;
            this.NOMBRE_ARCHIVO = NOMBRE_ARCHIVO;
        }
    }
}
