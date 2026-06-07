using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EscapeRoom
{
    public class Acertijo
    {
        string pregunta;
        string respuesta;
       public string pista;
        string[] opciones; 
        public string[] Opciones { get; set; } // son los 4 botones
        public string Pregunta{get; set; }
       string Respuesta { get; set; }
      public string Pista { get; set; }
        public Acertijo(string pregunta, string respuesta, string pista, string[] opciones)
        {
            this.Pregunta = pregunta;
            this.Respuesta = respuesta;
            this.Pista = pista;
            this.Opciones = opciones;

        }
        public bool VerificarRespuesta(string respuesta)
        {
            return this.Respuesta.Equals(respuesta, StringComparison.OrdinalIgnoreCase);
        }

        // POLIMORFISMO: 'virtual' permite que las clases hijas cambien este método
        public virtual void MostrarPista()
        {
            MessageBox.Show("Pista General: " + this.Pista, "Ayuda");
        }

    }

}
