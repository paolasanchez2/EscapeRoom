using System;
using System.IO;
using System.Text.Json;

namespace EscapeRoom
{
    /// <summary>
    /// Representa el estado de una partida guardada (una "ranura").
    /// </summary>
    public class Partida
    {
        public int    IndiceNivel      { get; set; } = 0;
        public int    Puntuacion       { get; set; } = 0;
        public int    IntentosRestantes{ get; set; } = 3;
        public string FechaGuardado    { get; set; } = "";
        public bool   Ocupada          { get; set; } = false;   // ¿Tiene datos?

        // ── Carpeta donde se guardan las 3 ranuras ───────────────────────────
        private static string CarpetaGuardados =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                         "EscapeRoom", "partidas");

        private static string RutaRanura(int ranura) =>
            Path.Combine(CarpetaGuardados, $"partida_{ranura}.json");

        // ── Guardar ranura (0, 1 ó 2) ────────────────────────────────────────
        public void Guardar(int ranura)
        {
            Directory.CreateDirectory(CarpetaGuardados);
            FechaGuardado = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            Ocupada = true;
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(RutaRanura(ranura), json);
        }

        // ── Cargar ranura ────────────────────────────────────────────────────
        public static Partida Cargar(int ranura)
        { 
            string ruta = RutaRanura(ranura);
            if (!File.Exists(ruta)) return new Partida();          // ranura vacía

            try
            {
                string json = File.ReadAllText(ruta);
                return JsonSerializer.Deserialize<Partida>(json) ?? new Partida();
            }
            catch { return new Partida(); }
        }

        /// <summary>Devuelve las 3 ranuras (puede tener Ocupada=false si están vacías).</summary>
        public static Partida[] CargarTodas()
        {
            var resultado = new Partida[3];
            for (int i = 0; i < 3; i++)
                resultado[i] = Cargar(i);
            return resultado;
        }
    }
}
