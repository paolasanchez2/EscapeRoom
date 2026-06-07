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

            // Carga ajustes guardados y aplica idioma
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
    "C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\puerta.jpg",
    "C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\nivel 22.png",
    "C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\nivel333.png",
    "C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\nivel44.png",
    "C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\nivel55.png",
    "C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\nivel66.png",
    "C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\nivel77.png",
    "C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\nivel88.png",
    "C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\nivel99.png",
    "C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\nivel1000.png",
    "C:\\Proyecto2026 csharp\\EscapeRoom\\EscapeRoom\\imagenes\\nivel122.png",

};
        Button btnAjustes;
        Button btnCargar;
        Button btnGuardarPartida;

        private void Form1_Load(object sender, EventArgs e)
        {
           this.BackgroundImageLayout = ImageLayout.Stretch;
           pictureBox1.BackColor = Color.Transparent; //  fondo del control transparente 
           pictureBox1.Parent = this; 

            pictureBox3.BackColor = Color.Transparent; //  fondo del control transparente 
            pictureBox3.Parent = this; 


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
            // --- Ajustes --- 
            btnAjustes = new Button
            {
                Text = Textos.Get("btnAjustes"),
                Width = 90,
                Height = 24,
                Left = this.ClientSize.Width - 100,
                Top = 10,
                BackColor = Color.BlueViolet,                    // Fondo BlueViolet eléctrico
                ForeColor = Color.White,                         // Letras blancas para que resalten
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                TabIndex = 10
            };
            btnAjustes.FlatAppearance.BorderSize = 0;           // Sin orilla blanca
            btnAjustes.FlatAppearance.MouseOverBackColor = Color.MediumPurple; // Se aclara al pasar el mouse
            btnAjustes.Click += BtnAjustes_Click;

            // --- Cargar partida ---
            btnCargar = new Button
            {
                Text = Textos.Get("btnCargar"),
                Width = 100,
                Height = 24,
                Left = this.ClientSize.Width - 210,
                Top = 10,
                BackColor = Color.BlueViolet,                    // Fondo BlueViolet
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                TabIndex = 11
            };
            btnCargar.FlatAppearance.BorderSize = 0;
            btnCargar.FlatAppearance.MouseOverBackColor = Color.MediumPurple;
            btnCargar.Click += BtnCargar_Click;

            // --- Guardar partida --- 
            btnGuardarPartida = new Button
            {
                Text = "💾 " + Textos.Get("tituloGuardar"),
                Width = 110,
                Height = 24,
                Left = this.ClientSize.Width - 120,
                Top = 38,
                BackColor = Color.BlueViolet,                    // Fondo BlueViolet
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Visible = false,
                TabIndex = 12
            };
            btnGuardarPartida.FlatAppearance.BorderSize = 0;
            btnGuardarPartida.FlatAppearance.MouseOverBackColor = Color.MediumPurple;
            btnGuardarPartida.Click += BtnGuardarPartida_Click;

            // Agregar los botones al formulario
            Controls.AddRange(new Control[] { btnAjustes, btnCargar, btnGuardarPartida });
        
        }
        

        private void MostrarSiguientePregunta()
        {
            if (indiceActual < bancoDeAcertijos.Count)
            {
                nivel = bancoDeAcertijos[indiceActual];

                // posición del monstruo
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
            lblPregunta.Text = Textos.Get("lblPregunta");

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

               
                InicializarJuego(); //  la lista de preguntas con el nuevo idioma
                if (nivel != null)
                {
                    // Actualiza botones y pregunta con el nuevo idioma
                    nivel = bancoDeAcertijos[indiceActual];
                    lblPregunta.Text = nivel.Pregunta;
                    button1.Text = nivel.Opciones[0];
                    button2.Text = nivel.Opciones[1];
                    button3.Text = nivel.Opciones[2];
                    button4.Text = nivel.Opciones[3];
                }

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

            btnIniciar.Visible = false; //nos esconde el boton de inicio
            btnGuardarPartida.Visible = true;  

            tiempoRestante = 30; 
            contadorMovimiento = 0;
            lblTiempo.Text = "Tiempo: 30s";
            timer1.Start();

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
                nivel.MostrarPista();

                // se resta tiempo  por usar la pista
                tiempoRestante -= 5;
                lblTiempo.Text = Textos.Get("lblTiempoFmt", tiempoRestante);
            }
        }

        void InicializarJuego()
        {
            // aqui es para cambiar el idioma de los acertijos, se vacia la lista y se vuelven a cargar con el idioma correcto
            bancoDeAcertijos.Clear();

            if (Textos.IdiomaActivo == "en")
            {
                // --- LEVEL 1: ENTRANCE (Base Class) ---
                string[] ops1 = { "3.14", "2.15", "1.16", "4.20" };
                bancoDeAcertijos.Add(new Acertijo("The monster is coming! Enter the emergency PIN (the first digits of PI) to access.", "3.14", "It is the mathematical constant used to calculate circles.", ops1));

                // --- LEVEL 2: THE PANEL (Derived Class) ---
                string[] ops2 = { "Glass", "Copper", "Plastic", "Paper" };
                bancoDeAcertijos.Add(new AcertijoLaboratorio("Access granted! The scientist entered, but the power went out due to a short circuit in the control board. Which conductive metal will you reconnect?", "Copper", "A reddish transition metal with high electrical conductivity.", ops2, "Control Panel"));

                // --- LEVEL 3: WAREHOUSE (Derived Class) ---
                string[] ops3 = { "Glass", "Metal", "Wood", "Plastic" };
                bancoDeAcertijos.Add(new AcertijoLaboratorio("Lights on! But the warehouse hallway is blocked by heavy pallets. What lightweight, organic material are the movable boxes made of?", "Wood", "An organic, fibrous material derived from trees.", ops3, "Pine Boxes"));

                // --- LEVEL 4: GAS LEAK (Derived Class) ---
                string[] ops4 = { "Oxygen", "Nitrogen", "Chlorine", "Methane" };
                bancoDeAcertijos.Add(new AcertijoLaboratorio("Path cleared! However, a broken cooling pipe is releasing a freezing white vapor. What cryogenic gas is it?", "Nitrogen", "Its chemical symbol is N, and it freezes matter instantly upon contact.", ops4, "Steel Pipe"));

                // --- LEVEL 5: THE SPILL (Derived Class) ---
                string[] ops5 = { "Oil", "Water", "Alcohol", "Vinegar" };
                bancoDeAcertijos.Add(new AcertijoLaboratorio("You passed the vapor! But there is a slippery chemical spill on the analysis surface. Which universal neutral solvent should you use to clean the bench?", "Water", "A transparent liquid compound made of hydrogen and oxygen (H2O).", ops5, "Granite Bench"));

                // --- LEVEL 6: COMPUTER (Derived Class) ---
                string[] ops6 = { "0 and 1", "5 and 9", "A and B", "Yes and No" };
                bancoDeAcertijos.Add(new AcertijoLaboratorio("Surface clean! You reach the main terminal to lock the monster out. Which two-digit numerical system does the PC's processor run on?", "0 and 1", "Machine language based on on and off states (Binary system).", ops6, "Silicon Circuits"));

                // --- LEVEL 7: MICROSCOPIOS (Base Class) ---
                string[] ops7 = { "Cell", "Stone", "Dirt", "Bottle" };
                bancoDeAcertijos.Add(new Acertijo("System hacked! To get the override code, you analyze a biological sample under the microscope. What is the fundamental unit of life you are observing?", "Cell", "The basic structural and functional unit of all living organisms.", ops7));

                // --- LEVEL 8: THERMOMETER (Derived Class) ---
                string[] ops8 = { "Ruler", "Thermometer", "Scale", "Clock" };
                bancoDeAcertijos.Add(new AcertijoLaboratorio("Analysis complete! The room temperature is rising as the monster burns the entrance. Which laboratory instrument measures environmental temperature?", "Thermometer", "A glass device that utilizes the thermal expansion of a mercury line.", ops8, "Thermal Glass"));

                // --- LEVEL 9: SAFE (Base Class) ---
                string[] ops9 = { "Fe", "Au", "Ag", "Cu" };
                bancoDeAcertijos.Add(new Acertijo("Almost out! The security safe containing the master card requires the chemical symbol for the precious metal Gold. What is it?", "Au", "Derived from the Latin word 'Aurum', meaning shining dawn.", ops9));

                // --- LEVEL 10: FINAL ESCAPE (Derived Class) ---
                string[] ops10 = { "Closed", "Open", "Escape", "Mission" };
                bancoDeAcertijos.Add(new AcertijoLaboratorio("You have the card! Run to the laboratory's armored exit. The final panel asks you to confirm the action on screen: What is your objective?", "Escape", "The act of breaking free from the facility before the specimen reaches you.", ops10, "Titanium Door"));
            }
            else
            {
                // --- NIVEL 1: ENTRADA (Clase Base) ---
                string[] ops1 = { "3.14", "2.15", "1.16", "4.20" };
                bancoDeAcertijos.Add(new Acertijo("¡El monstruo viene! Introduce el PIN de emergencia (las primeras cifras de PI) para entrar.", "3.14", "Es el número que usamos para calcular círculos", ops1));

                // --- NIVEL 2: EL PANEL (Clase Hija) ---
                string[] ops2 = { "Vidrio", "Cobre", "Plástico", "Papel" };
                bancoDeAcertijos.Add(new AcertijoLaboratorio("¡Acceso correcto! El científico entró, pero se cortó la energía. Hay un cortocircuito. ¿Qué material conductor reconectas?", "Cobre", "Metal rojizo que transporta electricidad", ops2, "Panel de control"));

                // --- NIVEL 3: ALMACÉN (Clase Hija) ---
                string[] ops3 = { "Vidrio", "Metal", "Madera", "Plástico" };
                bancoDeAcertijos.Add(new AcertijoLaboratorio("¡Luces encendidas! Pero el pasillo está bloqueado por cajas pesadas. ¿De qué material son las que puedes mover?", "Madera", "Material orgánico ligero", ops3, "Cajas de pino"));

                // --- NIVEL 4: FUGA DE GAS (Clase Hija) ---
                string[] ops4 = { "Oxigeno", "Nitrogeno", "Cloro", "Metano" };
                bancoDeAcertijos.Add(new AcertijoLaboratorio("¡Camino despejado! Pero una tubería rota suelta un vapor blanco muy frío. ¿Qué gas es?", "Nitrogeno", "Su símbolo es N y congela al contacto", ops4, "Tubería de acero"));

                // --- NIVEL 5: EL DERRAME (Clase Hija) ---
                string[] ops5 = { "Aceite", "Agua", "Alcohol", "Vinagre" };
                bancoDeAcertijos.Add(new AcertijoLaboratorio("¡Pasaste el vapor! Pero hay un derrame resbaloso en la mesa de química. ¿Qué solvente universal usas para limpiar?", "Agua", "Compuesto H2O", ops5, "Mesa de Granito"));

                // --- NIVEL 6: COMPUTADORA (Clase Hija) ---
                string[] ops6 = { "0 y 1", "5 y 9", "A y B", "Si y No" };
                bancoDeAcertijos.Add(new AcertijoLaboratorio("¡Mesa limpia! Llegas a la terminal para bloquear al monstruo. ¿En qué sistema de dos dígitos trabaja la PC?", "0 y 1", "Es el lenguaje Binario", ops6, "Circuitos de Silicio"));

                // --- NIVEL 7: MICROSCOPIO (Clase Base) ---
                string[] ops7 = { "Célula", "Piedra", "Tierra", "Botella" };
                bancoDeAcertijos.Add(new Acertijo("¡Sistema hackeado! Analizas una muestra en el microscopio. ¿Cómo se llama la unidad mínima de vida que ves?", "Célula", "Forma a todos los seres vivos", ops7));

                // --- NIVEL 8: TERMÓMETRO (Clase Hija) ---
                string[] ops8 = { "Regla", "Termómetro", "Balanza", "Reloj" };
                bancoDeAcertijos.Add(new AcertijoLaboratorio("¡Análisis hecho! El calor aumenta, el monstruo quema la puerta. ¿Qué instrumento mide la temperatura ambiente?", "Termómetro", "Tiene una línea de mercurio", ops8, "Vidrio térmico"));

                // --- NIVEL 9: CAJA FUERTE (Clase Base) ---
                string[] ops9 = { "Fe", "Au", "Ag", "Cu" };
                bancoDeAcertijos.Add(new Acertijo("¡Casi escapas! La caja fuerte con la llave maestra pide el símbolo químico del Oro. ¿Cuál es?", "Au", "Viene de Aurum", ops9));

                // --- NIVEL 10: ESCAPE FINAL (Clase Hija) ---
                string[] ops10 = { "Cerrado", "Abierto", "Escape", "Misión" };
                bancoDeAcertijos.Add(new AcertijoLaboratorio("¡Tienes la llave! Corres a la salida blindada. El panel final pregunta: ¿Cuál es tu objetivo?", "Escape", "Lo que estás a punto de lograr", ops10, "Puerta de Titanio"));
            }
        }
        private void ProcesarRespuesta(string respuesta)
        {
            if (nivel.VerificarRespuesta(respuesta))
            {
                timer1.Stop();
                puntuacionTotal += tiempoRestante;
                lblPuntuacion.Text = Textos.Get("puntuacionFmt", puntuacionTotal);
                pictureBox3.Visible = false;

                MessageBox.Show("¡EXCELENTE! La respuesta era correcta. ¡Has avanzado!",
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

        private void lblPuntuacion_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }
    }
}
