using Fantasy_quest.Formularios;

namespace Fantasy_quest
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
#if DEBUG
            // desde el IDE (Debug): menu de desarrollo con acceso directo a cada ventana
            Application.Run(new FrmDesarrollo());
#else
            // version compilada (Release): el juego completo, por capas
            Application.Run(new FrmPrincipal());
#endif
        }
    }
}
