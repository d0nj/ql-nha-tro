# Assets & Attribution

## Icons — Lucide (ISC License)

50 SVG icons under `Resources/Icons/` from [Lucide](https://lucide.dev) via the
[Iconify](https://iconify.design) CDN. ISC License — free for commercial and
personal use, attribution appreciated.

Re-download with the script in the project root (`/scripts/download-icons.ps1`
or inline from chat history).

### Categories
- `nav_*` — sidebar navigation (8)
- `kpi_*` — dashboard KPI cards (10)
- `btn_*` — CRUD / toolbar actions (12)
- `status_*` — status indicators (4)
- `empty_*` — empty-state illustrations (5)
- `misc_*` — supplemental (10)
- `logo_home` — app brand mark

## Photos — Unsplash

Photos under `Resources/Images/` are licensed under the
[Unsplash License](https://unsplash.com/license) — free for commercial and
personal use, no permission needed. Attribution is appreciated.

| File | Photographer | Unsplash ID | Link |
|------|--------------|-------------|------|
| `login_bg.jpg` | Danilo Rios | `AgK_XAqSbfk` | https://unsplash.com/photos/AgK_XAqSbfk |
| `dashboard_hero.jpg` | Felix Ngo (Hanoi) | `_yR3Dn0kLKI` | https://unsplash.com/photos/_yR3Dn0kLKI |
| `empty_room.jpg` | Brian Wangenheim | `SCbkyJR3QSM` | https://unsplash.com/photos/SCbkyJR3QSM |
| `keys_about.jpg` | Maria Ziegler | `jJnZg7vBfMs` | https://unsplash.com/photos/jJnZg7vBfMs |
| `room_placeholder.jpg` | Point3D Commercial Imaging | `5d-aYsO2g7U` | https://unsplash.com/photos/5d-aYsO2g7U |

## Runtime Loading

Icons render via the `Svg` NuGet package (v3.4.7) through
`Helpers/AppIcons.cs` which:
- Caches rendered `Bitmap`s by `name|size|color`
- Replaces `currentColor` in SVGs at load time to support runtime tinting
- Lazy-loads JPGs with center-crop helper for arbitrary aspect ratios

See `Helpers/AppIcons.cs` for the strongly-typed icon name constants.
