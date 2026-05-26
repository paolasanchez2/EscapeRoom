using EscapeRoom.imagenes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace EscapeRoom
{
    public partial class Form1 : Form
    {

        List<Acertijo> bancoDeAcertijos = new List<Acertijo>();
        Random azar = new Random();
        Random rnd = new Random();
        int indiceActual = 0;
        int intentosRestantes = 3;
        int tiempoRestante = 30;
        int contadorMovimiento = 0;
        int puntuacionTotal = 0;
        int asedioIntensidad = 5;
        Acertijo nivel;
        Ajustes ajustes;

      
            public Form1()
            {
                InitializeComponent();

                // Cargar ajustes guardados (idioma)
                ajustes = Ajustes.Cargar();
                Textos.IdiomaActivo = ajustes.Idioma;

                InicializarJuego();

                pictureBox1.Visible = false;
                pictureBox2.Visible = false;
                pictureBox3.Visible = false;
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
                button4.Visible = false;
                lblPregunta.Visible = false;

                this.DoubleBuffered = true;

                AgregarBotonesExtra();
                AplicarIdioma();

                // Teclado sin mouse
                KeyPreview = true;
                KeyDown += Form1_KeyDown;
            }

          
         
        // Agrega esto donde están tus otras variables (como puntuacionTotal)
        string[] imagenesEscenarios = {
    "C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\puertaabierta.png",
    "C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\nivel1.png",
    "C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\nivel2.png",
    "C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\nivel3.png",
    "C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\nivel4.png",
};
        Button btnAjustes;
        Button btnCargar;
        Button btnGuardarPartida;

        private void Form1_Load(object sender, EventArgs e)
        {
           this.BackgroundImageLayout = ImageLayout.Stretch;
           pictureBox1.BackColor = Color.Transparent; //  fondo del control transparente 
           pictureBox1.Parent = this; // Le dice que su "fondo" es el Formulario

            pictureBox3.BackColor = Color.Transparent; //  fondo del control transparente 
            pictureBox3.Parent = this; // Le dice que su "fondo" es el Formulario

        }

        private void ExportarPuntuacion()
        {
            try
            {
                string escritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string nombreArchivo = "Resultado_Escape_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".json";
                string rutaCompleta = Path.Combine(escritorio, nombreArchivo);

                // Construimos el JSON a mano (compatible con .NET Framework / C# 7)
                string json = "{\n";
                json += "  \"Titulo\": \"" + Textos.Get("reporteTitulo") + "\",\n";
                json += "  \"Fecha\": \"" + DateTime.Now.ToString() + "\",\n";
                json += "  \"Puntuacion\": " + puntuacionTotal + ",\n";
                json += "  \"IntentosRestantes\": " + intentosRestantes + ",\n";
                json += "  \"Estado\": \"" + Textos.Get("reporteEstado") + "\",\n";
                json += "  \"Idioma\": \"" + Textos.IdiomaActivo + "\"\n";
                json += "}";

                File.WriteAllText(rutaCompleta, json);
                MessageBox.Show(Textos.Get("msgExportOk", nombreArchivo),
                                Textos.Get("msgExportTitulo"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(Textos.Get("msgExportError", ex.Message));
            }
        }
        private void AgregarBotonesExtra()
        {
            // ⚙ Ajustes — esquina superior derecha
            btnAjustes = new Button
            {
                Text = Textos.Get("btnAjustes"),
                Width = 110,
                Height = 32,
                Left = this.ClientSize.Width - 120,
                Top = 10,
                BackColor = Color.FromArgb(0, 80, 100),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                TabIndex = 10
            };
            btnAjustes.Click += BtnAjustes_Click;

            // 📂 Cargar partida
            btnCargar = new Button
            {
                Text = Textos.Get("btnCargar"),
                Width = 130,
                Height = 32,
                Left = this.ClientSize.Width - 240,
                Top = 10,
                BackColor = Color.FromArgb(80, 50, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                TabIndex = 11
            };
            btnCargar.Click += BtnCargar_Click;

            // 💾 Guardar partida (visible sólo mientras se juega)
            btnGuardarPartida = new Button
            {
                Text = "💾 " + Textos.Get("tituloGuardar"),
                Width = 120,
                Height = 32,
                Left = this.ClientSize.Width - 120,
                Top = 40,
                BackColor = Color.FromArgb(0, 70, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Visible = false,
                TabIndex = 12
            };
            btnGuardarPartida.Click += BtnGuardarPartida_Click;

            Controls.AddRange(new Control[] { btnAjustes, btnCargar, btnGuardarPartida });
        }
        private void MostrarSiguientePregunta()
        {
            if (indiceActual < bancoDeAcertijos.Count)
            {
                nivel = bancoDeAcertijos[indiceActual];

                // Reset posición del monstruo
                pictureBox3.Left = this.Width - pictureBox3.Width - 20;
                pictureBox3.Top = 100;
                pictureBox3.Visible = true;

                lblPregunta.Text = nivel.Pregunta;

                if (indiceActual < imagenesEscenarios.Length)
                {
                    pictureBox2.Image = Image.FromFile(imagenesEscenarios[indiceActual]);
                    pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                }

                button1.Text = nivel.Opciones[0];
                button2.Text = nivel.Opciones[1];
                button3.Text = nivel.Opciones[2];
                button4.Text = nivel.Opciones[3];

                tiempoRestante = 30;
                lblTiempo.Text = Textos.Get("lblTiempoFmt", tiempoRestante);
                timer1.Start();
            }
            else
            {
                timer1.Stop();
                MessageBox.Show(Textos.Get("msgFin", puntuacionTotal),
                                Textos.Get("msgFinTitulo"));
                ExportarPuntuacion();
                this.Close();
            }
        }

        private void AplicarIdioma()
        {
            btnIniciar.Text = Textos.Get("btnIniciar");
            btnPista.Text = Textos.Get("btnPista");
            lblPuntuacion.Text = Textos.Get("lblPuntuacion");
            lblTiempo.Text = Textos.Get("lblTiempo");

            if (btnAjustes != null) btnAjustes.Text = Textos.Get("btnAjustes");
            if (btnCargar != null) btnCargar.Text = Textos.Get("btnCargar");
            if (btnGuardarPartida != null) btnGuardarPartida.Text = "💾 " + Textos.Get("tituloGuardar");
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (nivel == null) return;

            switch (e.KeyCode)
            {
                case Keys.D1: case Keys.NumPad1: button1.PerformClick(); break;
                case Keys.D2: case Keys.NumPad2: button2.PerformClick(); break;
                case Keys.D3: case Keys.NumPad3: button3.PerformClick(); break;
                case Keys.D4: case Keys.NumPad4: button4.PerformClick(); break;
                case Keys.P: btnPista.PerformClick(); break;
                case Keys.S: btnGuardarPartida.PerformClick(); break;
                case Keys.L: btnCargar.PerformClick(); break;
                case Keys.C: btnAjustes.PerformClick(); break;

            }
            e.Handled = true;
        }

        private void BtnAjustes_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            var form = new FormAjustes(ajustes);
            if (form.ShowDialog() == DialogResult.OK)
            {
                ajustes = Ajustes.Cargar();
                Textos.IdiomaActivo = ajustes.Idioma;
                AplicarIdioma();
            }
            if (nivel != null) timer1.Start();
        }

        private void BtnCargar_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            var form = new FormPartidas(FormPartidas.Modo.Cargar);
            if (form.ShowDialog() == DialogResult.OK && form.PartidaCargada != null)
            {
                var p = form.PartidaCargada;
                indiceActual = p.IndiceNivel;
                puntuacionTotal = p.Puntuacion;
                intentosRestantes = p.IntentosRestantes;

                // Mostrar controles de juego
                pictureBox1.Visible = true;
                pictureBox2.Visible = true;
                button1.Visible = button2.Visible = button3.Visible = button4.Visible = true;
                lblPregunta.Visible = true;
                btnGuardarPartida.Visible = true;
                btnIniciar.Visible = false;

                lblPuntuacion.Text = Textos.Get("puntuacionFmt", puntuacionTotal);

                // Cargar imagen del científico y monstruo
                pictureBox1.Image = Image.FromFile("C:\\Users\\junio\\source\\repos\\EscapeRoom\\EscapeRoom\\imagenes\\cientifico2.png");
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox3.Image = Image.FromFile("C:\\Users\\junio\\source\\repos\\EscapeRoom\\EscapeRoom\\imagenes\\mounstro3.png");
                pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;

                MostrarSiguientePregunta();
            }
            else if (nivel != null) timer1.Start();
        }

        private void BtnGuardarPartida_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            var estado = new Partida
            {
                IndiceNivel = indiceActual,
                Puntuacion = puntuacionTotal,
                IntentosRestantes = intentosRestantes
            };
            var form = new FormPartidas(FormPartidas.Modo.Guardar, estado);
            form.ShowDialog();
            if (nivel != null) timer1.Start();
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {

            pictureBox1.Visible = true;
            pictureBox1.Image = Image.FromFile("C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\cientifico2.png");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

          pictureBox2.Visible = true;
            pictureBox2.Image = Image.FromFile("C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\puerta.jpg");
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

            pictureBox3.Visible = true;
            pictureBox3.Image = Image.FromFile("C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\mounstro3.png");
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;

            // Elegimos un índice al azar de la lista
            int indiceCualquiera = azar.Next(bancoDeAcertijos.Count);

            // Guardamos el acertijo seleccionado en nuestra variable 'nivel'
            nivel = bancoDeAcertijos[indiceCualquiera];

            lblPregunta.Text = nivel.Pregunta;
          
            label1.Visible = false;
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;

            lblPregunta.Visible = true;
            indiceActual = 0; // Empezamos desde la primera
            MostrarSiguientePregunta();

            btnIniciar.Visible = false; // Esconde el botón de inicio para que no se pueda presionar de nuevo
            btnGuardarPartida.Visible = true;  

            tiempoRestante = 30; // Resetear tiempo
            contadorMovimiento = 0;
            lblTiempo.Text = "Tiempo: 30s";
            timer1.Start(); // ¡Aquí empieza la acción!

        }



        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            ProcesarRespuesta(button4.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProcesarRespuesta(button1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ProcesarRespuesta(button2.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ProcesarRespuesta(button3.Text);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            tiempoRestante--;
            lblTiempo.Text = Textos.Get("lblTiempoFmt", tiempoRestante);

            if (tiempoRestante <= 0)
            {
                timer1.Stop();
                MessageBox.Show(Textos.Get("msgTiempo"));
                this.Close();
                return;
            }

            int targetX = pictureBox2.Left;
            int targetY = pictureBox2.Top;
            int velocidadBase = 2;
            int temblorX = rnd.Next(-asedioIntensidad, asedioIntensidad);
            int temblorY = rnd.Next(-asedioIntensidad, asedioIntensidad);

            if (pictureBox3.Left < targetX) pictureBox3.Left += (velocidadBase + temblorX);
            if (pictureBox3.Left > targetX) pictureBox3.Left -= (velocidadBase + temblorX);
            if (pictureBox3.Top < targetY) pictureBox3.Top += (velocidadBase + temblorY);
            if (pictureBox3.Top > targetY) pictureBox3.Top -= (velocidadBase + temblorY);

            if (pictureBox3.Bounds.IntersectsWith(pictureBox2.Bounds))
            {
                timer1.Stop();
                MessageBox.Show(Textos.Get("msgColision"));
                this.Close();
            }
        }

        private void btnPista_Click(object sender, EventArgs e)
        {
            if (nivel != null)
            {
                // Usamos el método que ya programaste en tu clase
                nivel.MostrarPista();

                //  quitarle 5 segundos al tiempo restante como "pago".
                tiempoRestante -= 5;
                lblTiempo.Text = Textos.Get("lblTiempoFmt", tiempoRestante);
            }
        }

        void InicializarJuego()
        {
            // Mezcla de Acertijos normales y de Laboratorio (Herencia/Polimorfismo)

            // Pregunta 1: Clase Base
            bancoDeAcertijos.Add(new Acertijo("Soy un gas noble que hace flotar globos", "Helio", "Elemento ligero", new string[] { "Oxigeno", "Helio", "Nitrogeno", "Neon" }));

            // Pregunta 2: Clase Hija
            bancoDeAcertijos.Add(new AcertijoLaboratorio("Instrumento para medir la masa de sustancias", "Balanza", "No es una báscula de baño", new string[] { "Regla", "Probeta", "Balanza", "Termometro" }, "Metal"));

            // Pregunta 3: Clase Hija
            bancoDeAcertijos.Add(new AcertijoLaboratorio("Recipiente de cristal para calentar líquidos", "Matraz", "Tiene cuello largo", new string[] { "Vaso", "Matraz", "Tubo", "Plato" }, "Vidrio Pyrex"));

            // Pregunta 4: Clase Base
            bancoDeAcertijos.Add(new Acertijo("Símbolo químico del Oro", "Au", "Aurum", new string[] { "Ag", "Fe", "Au", "Cu" }));

            // Pregunta 5: Clase Hija (Tu pregunta original)
            string[] ops5 = { "botella", "tubo de ensayo", "vaso", "florero" };
            bancoDeAcertijos.Add(new AcertijoLaboratorio("Soy de cristal y guardo muestras pequeñas", "tubo de ensayo", "Recipiente delgado", ops5, "Vidrio"));
        }

        private void ProcesarRespuesta(string respuesta)
        {
            if (nivel.VerificarRespuesta(respuesta))
            {
                timer1.Stop();
                puntuacionTotal += tiempoRestante;
                lblPuntuacion.Text = Textos.Get("puntuacionFmt", puntuacionTotal);
                pictureBox3.Visible = false;

                MessageBox.Show(Textos.Get("msgCorrecto"),
                                Textos.Get("msgNivelOk"),
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                indiceActual++;
                MostrarSiguientePregunta();
                pictureBox1.Visible = false;
            }
            else
            {
                intentosRestantes--;
                if (intentosRestantes > 0)
                {
                    MessageBox.Show(Textos.Get("msgIncorrecto", intentosRestantes));
                }
                else
                {
                    timer1.Stop();
                    MessageBox.Show(Textos.Get("msgSinIntentos"), Textos.Get("msgGameOver"));
                    this.Close();
                }
            }
        }


        private void lblTiempo_Click(object sender, EventArgs e)
        {

        }
    }
}
