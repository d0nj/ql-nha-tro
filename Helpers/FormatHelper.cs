using System.Globalization;

namespace QLNhaTro.Helpers
{
    public static class FormatHelper
    {
        private static readonly CultureInfo VnCulture = new("vi-VN");

        public static string FormatVND(decimal amount)
        {
            return amount.ToString("N0", VnCulture) + " đ";
        }

        public static string FormatDate(DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }

        public static string FormatDateTime(DateTime date)
        {
            return date.ToString("dd/MM/yyyy HH:mm");
        }

        public static string GenerateMa(string prefix, int id)
        {
            return $"{prefix}{id:D5}";
        }
    }
}
