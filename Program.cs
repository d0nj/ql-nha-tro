using QLNhaTro.Helpers;

namespace QLNhaTro
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Auto-initialize the database (runs migrations, ensures path is dynamic, handles errors)
            if (!DatabaseInitializer.Initialize())
            {
                return; // Exit if database initialization fails
            }

            Application.Run(new Forms.Main.FrmMain());
        }
    }
}