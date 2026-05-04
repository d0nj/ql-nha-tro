using Microsoft.EntityFrameworkCore;
using QLNhaTro.Data;

namespace QLNhaTro
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var db = new AppDbContext())
            {
                db.Database.Migrate();
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new Forms.Main.FrmMain());
        }
    }
}