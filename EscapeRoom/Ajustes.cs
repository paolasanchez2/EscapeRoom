using System;
using System.IO;
using System.Text.Json;

namespace EscapeRoom
{
    public class Ajustes
    {
        // ── Propiedades que se guardan en JSON ──────────────────────────────
        public string Idioma { get; set; } = "es"; // "es" o "en"

        // ── Ruta del archivo de configuración ──────────────────────────────
        private static readonly string RutaConfig = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "EscapeRoom", "ajustes.json");

        // ── Guardar en JSON ─────────────────────────────────────────────────
        public void Guardar()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(RutaConfig));
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(RutaConfig, json);
        }

        // ── Cargar desde JSON (devuelve instancia por defecto si no existe) ─
        public static Ajustes Cargar()
        {
            if (!File.Exists(RutaConfig))
                return new Ajustes();

            try 
            {
                string json = File.ReadAllText(RutaConfig);
                return JsonSerializer.Deserialize<Ajustes>(json) ?? new Ajustes();
            }
            catch
            {
                return new Ajustes();
            }
        }
    }
}