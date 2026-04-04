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
        int indiceActual = 0; // Para saber qué pregunta mostrar
        int intentosRestantes = 3; // El jugador tiene 3 vidas
        public Form1()
        {
            InitializeComponent();
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

        }

        // Agrega esto donde están tus otras variables (como puntuacionTotal)
        string[] imagenesEscenarios = {
    "C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\puertaabierta.png",
    "C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\nivel1.png",
    "C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\nivel2.png",
    "C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\nivel3.png",
    "C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\nivel4.png",
};


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
                // 1. Obtenemos la ruta automática al escritorio del usuario actual
                string escritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                // 2. Creamos un nombre único con la fecha y hora (AñoMesDia_HoraMinutoSegundo)
                string nombreArchivo = "Resultado_Escape_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";

                // 3. Combinamos la ruta del escritorio con el nombre del archivo
                string rutaCompleta = Path.Combine(escritorio, nombreArchivo);

                // 4. Preparamos el contenido
                string contenido = "--- REPORTE DE LABORATORIO: ESCAPE ROOM ---\n";
                contenido += $"Fecha de la misión: {DateTime.Now.ToString()}\n";
                contenido += $"Puntuación lograda: {puntuacionTotal} puntos\n";
                contenido += $"Intentos restantes: {intentosRestantes}\n";
                contenido += "-------------------------------------------\n";
                contenido += "Estado: Misión Cumplida.";

                // 5. Escribimos el archivo físicamente en el escritorio
                File.WriteAllText(rutaCompleta, contenido);

                MessageBox.Show($"¡Datos exportados al escritorio!\nArchivo: {nombreArchivo}", "Éxito");
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo guardar en el escritorio: " + ex.Message);
            }
        }

        private void MostrarSiguientePregunta()
        {
            if (indiceActual < bancoDeAcertijos.Count)
            {
                nivel = bancoDeAcertijos[indiceActual];
                // --- RESET DEL MONSTRUO ---
                pictureBox3.Left = this.Width - pictureBox3.Width - 20;
                pictureBox3.Top = 100; // Altura media para que no tape los botones
                pictureBox3.Visible = true;

                lblPregunta.Text = nivel.Pregunta;
                if (indiceActual < imagenesEscenarios.Length)
                {
                    pictureBox2.Image = Image.FromFile(imagenesEscenarios[indiceActual]);
                    pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                // CORRECCIÓN AQUÍ: Agregamos los índices [ ]
                button1.Text = nivel.Opciones[0];
                button2.Text = nivel.Opciones[1];
                button3.Text = nivel.Opciones[2];
                button4.Text = nivel.Opciones[3];


                tiempoRestante = 30;
                lblTiempo.Text = "Tiempo: 30s";
                timer1.Start();
            }
            else
            {
                timer1.Stop();
                MessageBox.Show($"¡Felicidades! Has completado todos los acertijos.\nPuntuación final: {puntuacionTotal}", "Juego Terminado");
                ExportarPuntuacion(); // <--- Llamamos a la función del TXT aquí
                this.Close();
            }
        }

        Acertijo nivel;
        int tiempoRestante = 30; // 30 segundos para responder
        int contadorMovimiento = 0; // Para contar los 3 segundos
        Random rnd = new Random();
        int puntuacionTotal = 0;

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
          

            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;

            lblPregunta.Visible = true;
            indiceActual = 0; // Empezamos desde la primera
            MostrarSiguientePregunta();

            btnIniciar.Visible = false; // Esconde el botón de inicio para que no se pueda presionar de nuevo

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
            // 1. Verificamos con el método de tu clase
            if (nivel.VerificarRespuesta(button4.Text))
            {
                // --- LÓGICA DE VICTORIA ---
                timer1.Stop(); // Detenemos al monstruo de inmediato
                puntuacionTotal += tiempoRestante;

                // Actualizamos el Label de puntuación
                lblPuntuacion.Text = "Puntuación: " + puntuacionTotal.ToString();
                // Efecto visual: El monstruo desaparece y la puerta cambia
                pictureBox3.Visible = false;
                
                MessageBox.Show("¡EXCELENTE! El código era correcto.",
                                "Nivel Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                indiceActual++; // Pasamos a la siguiente pregunta
              
                MostrarSiguientePregunta(); // Cargamos los nuevos datos
               pictureBox1.Visible = false; 
             
            }
            else
            {
                intentosRestantes--;
                if (intentosRestantes > 0)
                {
                    MessageBox.Show($"¡Incorrecto! Te quedan {intentosRestantes} intentos. El monstruo se acerca...");
                }
                else
                {
                    timer1.Stop();
                    MessageBox.Show("Has agotado tus intentos. El laboratorio se ha cerrado permanentemente.", "Game Over");
                    this.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. Verificamos con el método de tu clase
            if (nivel.VerificarRespuesta(button1.Text))
            {
                // --- LÓGICA DE VICTORIA ---
                timer1.Stop(); // Detenemos al monstruo de inmediato
                puntuacionTotal += tiempoRestante;

                // Actualizamos el Label de puntuación
                lblPuntuacion.Text = "Puntuación: " + puntuacionTotal.ToString();
                // Efecto visual: El monstruo desaparece y la puerta cambia
                pictureBox3.Visible = false;

                MessageBox.Show("¡EXCELENTE! El código era correcto. ¡Has escapado del laboratorio!",
                                "Nivel Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                indiceActual++; // Pasamos a la siguiente pregunta
                MostrarSiguientePregunta(); // Cargamos los nuevos datos
                pictureBox1.Visible = false;

              
            }
            else
            {
                intentosRestantes--;
                if (intentosRestantes > 0)
                {
                    MessageBox.Show($"¡Incorrecto! Te quedan {intentosRestantes} intentos. El monstruo se acerca...");
                }
                else
                {
                    timer1.Stop();
                    MessageBox.Show("Has agotado tus intentos. El laboratorio se ha cerrado permanentemente.", "Game Over");
                    this.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 1. Verificamos con el método de tu clase
            if (nivel.VerificarRespuesta(button2.Text))
            {
                // --- LÓGICA DE VICTORIA ---
                timer1.Stop(); // Detenemos al monstruo de inmediato
                puntuacionTotal += tiempoRestante;

                // Actualizamos el Label de puntuación
                lblPuntuacion.Text = "Puntuación: " + puntuacionTotal.ToString();
                // Efecto visual: El monstruo desaparece y la puerta cambia
                pictureBox3.Visible = false;

                MessageBox.Show("¡EXCELENTE! El código era correcto. ¡Has escapado del laboratorio!",
                                "Nivel Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                indiceActual++; // Pasamos a la siguiente pregunta
                MostrarSiguientePregunta(); // Cargamos los nuevos datos
                pictureBox1.Visible = false;

            }
            else
            {
                intentosRestantes--;
                if (intentosRestantes > 0)
                {
                    MessageBox.Show($"¡Incorrecto! Te quedan {intentosRestantes} intentos. El monstruo se acerca...");
                }
                else
                {
                    timer1.Stop();
                    MessageBox.Show("Has agotado tus intentos. El laboratorio se ha cerrado permanentemente.", "Game Over");
                    this.Close();
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 1. Verificamos con el método de tu clase
            if (nivel.VerificarRespuesta(button3.Text))
            {
                // --- LÓGICA DE VICTORIA ---
                timer1.Stop(); // Detenemos al monstruo de inmediato
                puntuacionTotal += tiempoRestante;

                // Actualizamos el Label de puntuación
                lblPuntuacion.Text = "Puntuación: " + puntuacionTotal.ToString();
                // Efecto visual: El monstruo desaparece y la puerta cambia
                pictureBox3.Visible = false;

                MessageBox.Show("¡EXCELENTE! El código era correcto. ¡Has escapado del laboratorio!",
                                "Nivel Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                indiceActual++; // Pasamos a la siguiente pregunta
                MostrarSiguientePregunta(); // Cargamos los nuevos datos
                pictureBox1.Visible = false;

                
            }
            else
            {
                intentosRestantes--;
                if (intentosRestantes > 0)
                {
                    MessageBox.Show($"¡Incorrecto! Te quedan {intentosRestantes} intentos. El monstruo se acerca...");
                }
                else
                {
                    timer1.Stop();
                    MessageBox.Show("Has agotado tus intentos. El laboratorio se ha cerrado permanentemente.", "Game Over");
                    this.Close();
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        int asedioIntensidad = 5; // Qué tan "brinco" se mueve el monstruo
        private void timer1_Tick(object sender, EventArgs e)
        {
            // --- 1. Lógica del Tiempo Límite (Cada 1 segundo) ---
            // (Asegúrate que el Interval de tu Timer sea 1000ms para que esto baje bien)
            tiempoRestante--;
            lblTiempo.Text = "Tiempo: " + tiempoRestante + "s";

            if (tiempoRestante <= 0)
            {
                timer1.Stop();
                MessageBox.Show("¡EL MONSTRUO TE ATRAPÓ! No lograste abrir la puerta a tiempo.");
                this.Close(); // O reiniciar el juego
            }

            // --- 2. Lógica de Asedio (Movimiento del Monstruo) ---
            // (Asegúrate de que el DoubleBuffered = true; esté en tu constructor para no tener el rastro)

            // Calculamos hacia dónde debe moverse el monstruo (pictureBox3)
            int targetX = pictureBox2.Left; // La puerta (donde está el científico)
            int targetY = pictureBox2.Top;

            // Movimiento base del monstruo (asedio constante)
            int velocidadBase = 2;

            // Calculamos la dirección y le añadimos un "efecto de temblor" al azar
            int temblorX = rnd.Next(-asedioIntensidad, asedioIntensidad);
            int temblorY = rnd.Next(-asedioIntensidad, asedioIntensidad);

            // Movimiento en X
            if (pictureBox3.Left < targetX) pictureBox3.Left += (velocidadBase + temblorX);
            if (pictureBox3.Left > targetX) pictureBox3.Left -= (velocidadBase + temblorX);

            // Movimiento en Y
            if (pictureBox3.Top < targetY) pictureBox3.Top += (velocidadBase + temblorY);
            if (pictureBox3.Top > targetY) pictureBox3.Top -= (velocidadBase + temblorY);


            // --- 3. Condición de Derrota (Si el monstruo "toca" la puerta/científico) ---
            // Usamos IntersectsWith para detectar la colisión
            if (pictureBox3.Bounds.IntersectsWith(pictureBox2.Bounds))
            {
                timer1.Stop();
                MessageBox.Show("¡EL MONSTRUO FORZÓ LA PUERTA! El científico fue atrapado.");
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
                lblTiempo.Text = "Tiempo: " + tiempoRestante + "s";
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
    }
}
