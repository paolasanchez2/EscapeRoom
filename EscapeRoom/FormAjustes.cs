using System;
using System.Windows.Forms;

namespace EscapeRoom
{
    /// <summary>
    /// Ventana de Ajustes.  100 % navegable con teclado (Tab / Enter / Escape).
    /// </summary>
    public class FormAjustes : Form
    {
        // ── Controles ────────────────────────────────────────────────────────
        private Label      lblTitulo;
        private Label      lblIdioma;
        private RadioButton rbEspanol;
        private RadioButton rbIngles;
        private Button     btnGuardar;
        private Button     btnCancelar;

        // ── Ajustes actuales que se pasaron al abrir el form ─────────────────
        private Ajustes _ajustes;

        public FormAjustes(Ajustes ajustes)
        {
            _ajustes = ajustes;
            ConstruirUI();
            CargarValores();

            // Navegación completa con teclado
            KeyPreview = true;
            KeyDown += FormAjustes_KeyDown;
        }

        // ── Construir controles en código (sin Designer) ─────────────────────
        private void ConstruirUI()
        {
            Text            = "Ajustes / Settings";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox     = false;
            MinimizeBox     = false;
            StartPosition   = FormStartPosition.CenterParent;
            Width           = 340;
            Height          = 220;
            BackColor       = System.Drawing.Color.FromArgb(30, 30, 30);
            ForeColor       = System.Drawing.Color.White;

            // Título
            lblTitulo = new Label
            {
                Text      = "⚙  Ajustes / Settings",
                Font      = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.Cyan,
                Left = 20, Top = 15, Width = 280, Height = 28
            };

            // Etiqueta idioma
            lblIdioma = new Label
            {
                Text      = "Idioma / Language:",
                Left = 20, Top = 55, Width = 150, Height = 22
            };

            // Radio Español
            rbEspanol = new RadioButton
            {
                Text     = "Español",
                Left     = 30, Top = 82, Width = 120,
                TabIndex = 0,
                ForeColor = System.Drawing.Color.White
            };

            // Radio English
            rbIngles = new RadioButton
            {
                Text     = "English",
                Left     = 170, Top = 82, Width = 120,
                TabIndex = 1,
                ForeColor = System.Drawing.Color.White
            };

            // Botón Guardar  (Tab=2, es el primero que recibe Enter por defecto)
            btnGuardar = new Button
            {
                Text        = "Guardar / Save",
                Left        = 30,  Top = 130, Width = 120, Height = 35,
                TabIndex    = 2,
                BackColor   = System.Drawing.Color.DarkCyan,
                ForeColor   = System.Drawing.Color.White,
                FlatStyle   = FlatStyle.Flat
            };
            btnGuardar.Click += BtnGuardar_Click;

            // Botón Cancelar  (Tab=3)
            btnCancelar = new Button
            {
                Text      = "Cancelar / Cancel",
                Left      = 170, Top = 130, Width = 120, Height = 35,
                TabIndex  = 3,
                BackColor = System.Drawing.Color.FromArgb(80, 0, 0),
                ForeColor = System.Drawing.Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.AddRange(new Control[]
            {
                lblTitulo, lblIdioma, rbEspanol, rbIngles, btnGuardar, btnCancelar
            });

            // Enter activa Guardar cuando el foco está ahí
            AcceptButton = btnGuardar;
            CancelButton = btnCancelar;  // Escape cierra
        }

        private void CargarValores()
        {
            rbEspanol.Checked = _ajustes.Idioma == "es";
            rbIngles.Checked  = _ajustes.Idioma == "en";
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            _ajustes.Idioma = rbIngles.Checked ? "en" : "es";
            _ajustes.Guardar();
            Textos.IdiomaActivo = _ajustes.Idioma;
            DialogResult = DialogResult.OK;
            Close();
        }

        // ── Navegación con teclado ───────────────────────────────────────────
        private void FormAjustes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { DialogResult = DialogResult.Cancel; Close(); }
        }
    }
}
