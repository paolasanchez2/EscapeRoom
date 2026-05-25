using System;
using System.Windows.Forms;

namespace EscapeRoom
{
    /// <summary>
    /// Muestra las 3 ranuras de partida.
    /// Modo Guardar  → el usuario elige dónde guardar.
    /// Modo Cargar   → el usuario elige qué partida cargar (ranuras vacías deshabilitadas).
    /// Completamente navegable con teclado: flechas ↑↓ entre ranuras, Enter para confirmar, Escape para cancelar.
    /// </summary>
    public class FormPartidas : Form
    {
        public enum Modo { Guardar, Cargar }

        // ── Resultado público que Form1 leerá ────────────────────────────────
        public int    RanuraSeleccionada { get; private set; } = -1;
        public Partida PartidaCargada   { get; private set; }

        // ── Internos ─────────────────────────────────────────────────────────
        private Modo      _modo;
        private Partida[] _ranuras;
        private Button[]  _botones = new Button[3];
        private Button    btnCancelar;
        private Partida   _estadoActual;  // sólo en modo Guardar

        public FormPartidas(Modo modo, Partida estadoActual = null)
        {
            _modo         = modo;
            _estadoActual = estadoActual;
            _ranuras      = Partida.CargarTodas();
            ConstruirUI();

            KeyPreview = true;
            KeyDown   += FormPartidas_KeyDown;
        }

        private void ConstruirUI()
        {
            Text            = _modo == Modo.Guardar
                                ? Textos.Get("tituloGuardar")
                                : Textos.Get("tituloCargar");
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox     = false;
            MinimizeBox     = false;
            StartPosition   = FormStartPosition.CenterParent;
            Width           = 380;
            Height          = 280;
            BackColor       = System.Drawing.Color.FromArgb(20, 20, 20);
            ForeColor       = System.Drawing.Color.White;

            var lblTitulo = new Label
            {
                Text      = Text,
                Font      = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.Gold,
                Left = 20, Top = 12, Width = 320, Height = 28
            };
            Controls.Add(lblTitulo);

            // ── Tres botones de ranura ────────────────────────────────────────
            for (int i = 0; i < 3; i++)
            {
                var ranura  = _ranuras[i];
                int tabIdx  = i;

                string etiqueta = ranura.Ocupada
                    ? $"🔖 Ranura {i + 1} — Nivel {ranura.IndiceNivel + 1} | " +
                      $"Pts: {ranura.Puntuacion} | ❤ {ranura.IntentosRestantes} | {ranura.FechaGuardado}"
                    : $"📂 Ranura {i + 1} — {Textos.Get("msgSinPartida")}";

                var btn = new Button
                {
                    Text      = etiqueta,
                    Left      = 20,
                    Top       = 50 + i * 55,
                    Width     = 320,
                    Height    = 44,
                    TabIndex  = tabIdx,
                    BackColor = ranura.Ocupada
                                  ? System.Drawing.Color.FromArgb(0, 60, 40)
                                  : System.Drawing.Color.FromArgb(40, 40, 40),
                    ForeColor = System.Drawing.Color.White,
                    FlatStyle = FlatStyle.Flat,
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                    Enabled   = _modo == Modo.Guardar || ranura.Ocupada
                };

                int ranuraIdx = i;   // captura correcta del índice en el closure
                btn.Click += (s, e) => SeleccionarRanura(ranuraIdx);
                _botones[i] = btn;
                Controls.Add(btn);
            }

            // ── Botón Cancelar ────────────────────────────────────────────────
            btnCancelar = new Button
            {
                Text      = "Cancelar / Cancel",
                Left      = 20, Top = 215, Width = 150, Height = 32,
                TabIndex  = 3,
                BackColor = System.Drawing.Color.FromArgb(80, 0, 0),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            Controls.Add(btnCancelar);

            CancelButton = btnCancelar;
            // Foco inicial en primer botón habilitado
            for (int i = 0; i < 3; i++)
                if (_botones[i].Enabled) { _botones[i].Select(); break; }
        }

        private void SeleccionarRanura(int idx)
        {
            RanuraSeleccionada = idx;

            if (_modo == Modo.Guardar)
            {
                _estadoActual.Guardar(idx);
                MessageBox.Show(Textos.Get("msgPartidaGuardada", idx + 1),
                                Textos.Get("tituloGuardar"));
            }
            else
            {
                PartidaCargada = Partida.Cargar(idx);
                MessageBox.Show(Textos.Get("msgPartidaCargada"),
                                Textos.Get("tituloCargar"));
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        // ── Navegación con flechas y Enter ───────────────────────────────────
        private void FormPartidas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { DialogResult = DialogResult.Cancel; Close(); return; }

            // Buscar botón con foco actual
            int actual = -1;
            for (int i = 0; i < 3; i++)
                if (_botones[i].Focused) { actual = i; break; }

            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Tab)
            {
                // Mover foco al siguiente botón habilitado
                for (int i = 1; i <= 3; i++)
                {
                    int next = (actual + i) % 3;
                    if (_botones[next].Enabled) { _botones[next].Select(); break; }
                }
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                for (int i = 1; i <= 3; i++)
                {
                    int prev = (actual - i + 3) % 3;
                    if (_botones[prev].Enabled) { _botones[prev].Select(); break; }
                }
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter && actual >= 0)
            {
                SeleccionarRanura(actual);
                e.Handled = true;
            }
        }
    }
}
