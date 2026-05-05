using System;
using System.Threading;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using QLNhaTro.Data;

namespace QLNhaTro.Helpers
{
    public static class DatabaseInitializer
    {
        private static int _initialized = 0;

        /// <summary>
        /// Initializes the database by applying migrations. Ensures this only runs once.
        /// </summary>
        /// <returns>True if successful or already initialized, false if an error occurred.</returns>
        public static bool Initialize()
        {
            if (Interlocked.Exchange(ref _initialized, 1) == 1)
            {
                return true;
            }

            try
            {
                using (var context = new AppDbContext())
                {
                    context.Database.Migrate();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo cơ sở dữ liệu. Vui lòng kiểm tra quyền truy cập thư mục.\n\nChi tiết lỗi: {ex.Message}",
                                "Lỗi Cơ Sở Dữ Liệu",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
