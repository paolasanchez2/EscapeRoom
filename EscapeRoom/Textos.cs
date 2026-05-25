using System.Collections.Generic;

namespace EscapeRoom
{
    public static class Textos
    {
        public static string IdiomaActivo { get; set; } = "es";

        private static readonly Dictionary<string, Dictionary<string, string>> _dict =
            new Dictionary<string, Dictionary<string, string>>
        {
            // ── UI General ──────────────────────────────────────────────────
            { "btnIniciar",      new Dictionary<string,string> { {"es","Iniciar"},          {"en","Start"} } },
            { "btnPista",        new Dictionary<string,string> { {"es","Pista (-5s)"},      {"en","Hint (-5s)"} } },
            { "btnAjustes",      new Dictionary<string,string> { {"es","⚙ Ajustes"},       {"en","⚙ Settings"} } },
            { "btnCargar",       new Dictionary<string,string> { {"es","Cargar Partida"},   {"en","Load Game"} } },
            { "lblPuntuacion",   new Dictionary<string,string> { {"es","Puntuación: 0"},    {"en","Score: 0"} } },
            { "lblTiempo",       new Dictionary<string,string> { {"es","Tiempo: 30s"},      {"en","Time: 30s"} } },
            { "lblTiempoFmt",    new Dictionary<string,string> { {"es","Tiempo: {0}s"},     {"en","Time: {0}s"} } },
            { "puntuacionFmt",   new Dictionary<string,string> { {"es","Puntuación: {0}"},  {"en","Score: {0}"} } },

            // ── Mensajes del juego ───────────────────────────────────────────
            { "msgCorrecto",     new Dictionary<string,string> { {"es","¡EXCELENTE! El código era correcto. ¡Has escapado!"},
                                                                  {"en","EXCELLENT! Correct code. You escaped!"} } },
            { "msgNivelOk",      new Dictionary<string,string> { {"es","Nivel Completado"},  {"en","Level Complete"} } },
            { "msgIncorrecto",   new Dictionary<string,string> { {"es","¡Incorrecto! Te quedan {0} intentos. El monstruo se acerca..."},
                                                                  {"en","Wrong! {0} attempts left. The monster approaches..."} } },
            { "msgSinIntentos",  new Dictionary<string,string> { {"es","Has agotado tus intentos. El laboratorio se cerró permanentemente."},
                                                                  {"en","Out of attempts. The lab is sealed forever."} } },
            { "msgGameOver",     new Dictionary<string,string> { {"es","Game Over"},         {"en","Game Over"} } },
            { "msgTiempo",       new Dictionary<string,string> { {"es","¡EL MONSTRUO TE ATRAPÓ! No lograste abrir la puerta a tiempo."},
                                                                  {"en","THE MONSTER GOT YOU! You didn't open the door in time."} } },
            { "msgColision",     new Dictionary<string,string> { {"es","¡EL MONSTRUO FORZÓ LA PUERTA! El científico fue atrapado."},
                                                                  {"en","THE MONSTER BROKE THROUGH! The scientist was caught."} } },
            { "msgFin",          new Dictionary<string,string> { {"es","¡Felicidades! Completaste todos los acertijos.\nPuntuación final: {0}"},
                                                                  {"en","Congratulations! All riddles solved.\nFinal score: {0}"} } },
            { "msgFinTitulo",    new Dictionary<string,string> { {"es","Juego Terminado"},   {"en","Game Over"} } },
            { "msgExportOk",     new Dictionary<string,string> { {"es","¡Datos exportados!\nArchivo: {0}"}, {"en","Data exported!\nFile: {0}"} } },
            { "msgExportTitulo", new Dictionary<string,string> { {"es","Éxito"},             {"en","Success"} } },
            { "msgExportError",  new Dictionary<string,string> { {"es","No se pudo guardar: {0}"}, {"en","Could not save: {0}"} } },

            // ── Partidas ─────────────────────────────────────────────────────
            { "tituloCargar",       new Dictionary<string,string> { {"es","Cargar Partida"},    {"en","Load Game"} } },
            { "tituloGuardar",      new Dictionary<string,string> { {"es","Guardar Partida"},   {"en","Save Game"} } },
            { "msgPartidaGuardada", new Dictionary<string,string> { {"es","Partida guardada en ranura {0}."},
                                                                     {"en","Game saved to slot {0}."} } },
            { "msgSinPartida",      new Dictionary<string,string> { {"es","Sin datos"},         {"en","Empty"} } },
            { "msgPartidaCargada",  new Dictionary<string,string> { {"es","Partida cargada."},  {"en","Game loaded."} } },

            // ── Reporte JSON ─────────────────────────────────────────────────
            { "reporteTitulo",   new Dictionary<string,string> { {"es","REPORTE DE LABORATORIO: ESCAPE ROOM"},
                                                                  {"en","LAB REPORT: ESCAPE ROOM"} } },
            { "reporteEstado",   new Dictionary<string,string> { {"es","Misión Cumplida"},   {"en","Mission Complete"} } },
        };

        public static string Get(string clave)
        {
            if (_dict.TryGetValue(clave, out var traducciones))
            {
                string valor;
                return traducciones.TryGetValue(IdiomaActivo, out valor) ? valor : clave;
            }
            return clave;
        }

        public static string Get(string clave, params object[] args)
        {
            return string.Format(Get(clave), args);
        }
    }
}