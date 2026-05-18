using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using Svg;

namespace QLNhaTro.Helpers
{
    public static class AppIcons
    {
        public static class Nav
        {
            public const string Dashboard = "nav_dashboard";
            public const string Room = "nav_room";
            public const string Tenant = "nav_tenant";
            public const string Contract = "nav_contract";
            public const string Utility = "nav_utility";
            public const string Invoice = "nav_invoice";
            public const string Report = "nav_report";
            public const string Settings = "nav_settings";
        }

        public static class Kpi
        {
            public const string Building = "kpi_building";
            public const string DoorOpen = "kpi_door_open";
            public const string DoorClosed = "kpi_door_closed";
            public const string Wallet = "kpi_wallet";
            public const string Users = "kpi_users";
            public const string Alarm = "kpi_alarm";
            public const string Receipt = "kpi_receipt";
            public const string Zap = "kpi_zap";
            public const string Droplet = "kpi_droplet";
            public const string Calendar = "kpi_calendar";
        }

        public static class Btn
        {
            public const string Add = "btn_add";
            public const string Edit = "btn_edit";
            public const string Delete = "btn_delete";
            public const string Save = "btn_save";
            public const string Cancel = "btn_cancel";
            public const string Search = "btn_search";
            public const string Export = "btn_export";
            public const string Print = "btn_print";
            public const string Filter = "btn_filter";
            public const string Refresh = "btn_refresh";
            public const string View = "btn_view";
            public const string Check = "btn_check";
        }

        public static class Status
        {
            public const string Ok = "status_ok";
            public const string Bad = "status_bad";
            public const string Warn = "status_warn";
            public const string Pending = "status_pending";
        }

        public static class Empty
        {
            public const string Inbox = "empty_inbox";
            public const string Users = "empty_users";
            public const string Door = "empty_door";
            public const string Document = "empty_doc";
            public const string Receipt = "empty_receipt";
        }

        public static class Misc
        {
            public const string Calendar = "misc_calendar";
            public const string Phone = "misc_phone";
            public const string Mail = "misc_mail";
            public const string User = "misc_user";
            public const string IdCard = "misc_id_card";
            public const string MapPin = "misc_map_pin";
            public const string Clock = "misc_clock";
            public const string Chevron = "misc_chevron";
            public const string ArrowUp = "misc_arrow_up";
            public const string ArrowDown = "misc_arrow_down";
        }

        public static class Logo
        {
            public const string Home = "logo_home";
        }

        public static class Image
        {
            public const string LoginBg = "login_bg";
            public const string DashboardHero = "dashboard_hero";
            public const string EmptyRoom = "empty_room";
            public const string KeysAbout = "keys_about";
            public const string RoomPlaceholder = "room_placeholder";
        }

        private static readonly string IconsRoot = Path.Combine(AppContext.BaseDirectory, "Resources", "Icons");
        private static readonly string ImagesRoot = Path.Combine(AppContext.BaseDirectory, "Resources", "Images");
        private static readonly Dictionary<string, Bitmap> _iconCache = new();
        private static readonly Dictionary<string, System.Drawing.Image> _imgCache = new();
        private static readonly object _lock = new();

        public static Bitmap? Load(string name, int size, Color color)
        {
            if (string.IsNullOrEmpty(name)) return null;
            var key = $"{name}|{size}|{color.ToArgb()}";

            lock (_lock)
            {
                if (_iconCache.TryGetValue(key, out var cached)) return cached;

                var path = Path.Combine(IconsRoot, name + ".svg");
                if (!File.Exists(path)) return null;

                try
                {
                    var svgText = File.ReadAllText(path);
                    var hex = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
                    svgText = svgText.Replace("currentColor", hex, StringComparison.OrdinalIgnoreCase);

                    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(svgText));
                    var doc = SvgDocument.Open<SvgDocument>(stream);
                    doc.Width = size;
                    doc.Height = size;
                    var bmp = doc.Draw(size, size);
                    _iconCache[key] = bmp;
                    return bmp;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static System.Drawing.Image? LoadImage(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            lock (_lock)
            {
                if (_imgCache.TryGetValue(name, out var cached)) return cached;

                var path = Path.Combine(ImagesRoot, name + ".jpg");
                if (!File.Exists(path)) return null;

                try
                {
                    using var fs = File.OpenRead(path);
                    var img = System.Drawing.Image.FromStream(fs);
                    _imgCache[name] = img;
                    return img;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static Bitmap? LoadImageCropped(string name, int width, int height)
        {
            var src = LoadImage(name);
            if (src == null) return null;

            var key = $"crop:{name}|{width}x{height}";
            lock (_lock)
            {
                if (_iconCache.TryGetValue(key, out var cached)) return cached;

                var bmp = new Bitmap(width, height);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    float srcRatio = (float)src.Width / src.Height;
                    float dstRatio = (float)width / height;
                    int cropW, cropH, cropX, cropY;
                    if (srcRatio > dstRatio)
                    {
                        cropH = src.Height;
                        cropW = (int)(src.Height * dstRatio);
                        cropX = (src.Width - cropW) / 2;
                        cropY = 0;
                    }
                    else
                    {
                        cropW = src.Width;
                        cropH = (int)(src.Width / dstRatio);
                        cropX = 0;
                        cropY = (src.Height - cropH) / 2;
                    }

                    g.DrawImage(src, new Rectangle(0, 0, width, height),
                        cropX, cropY, cropW, cropH, GraphicsUnit.Pixel);
                }
                _iconCache[key] = bmp;
                return bmp;
            }
        }
    }
}
