using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EscapeRoom.imagenes
{
    public class AcertijoLaboratorio : Acertijo
    {
        public string Material { get; set; }

        
        public AcertijoLaboratorio(string preg, string resp, string pist, string[] ops, string material)
            : base(preg, resp, pist, ops)
        {
            this.Material = material;
        }

      
        public override void MostrarPista()
        {
            MessageBox.Show($"Pista Especializada: {this.Pista}. Tip: El objeto es de {Material}.", "Análisis Químico");
        }
    }
}
