using System;
using System.IO;
using System.Text.Json;

namespace EscapeRoom
{
    public class Ajustes
    {
       //guardan en JSON 
        public string Idioma { get; set; } = "es"; // "es" o "en"

        // La Ruta 
        private static readonly string RutaConfig = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "EscapeRoom", "ajustes.json");

        // Se Guarda en JSON
        public void Guardar()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(RutaConfig));
            string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(RutaConfig, json);
        }

        // se Carga desde JSON
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