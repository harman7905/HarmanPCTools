# Changelog

## v2.1.0 — UI Refresh & Rebrand

- Rebranded the user-facing product as **Harman PC Toolkit**.
- Added the tagline **“A Windows PC Toolkit — Designed by Harman”** throughout the user interface and documentation.
- Redesigned the WPF shell, sidebar, and Home dashboard around the Harman Blue visual language.
- Redesigned Gaming, System, Cleanup, Organizer, Tools, Startup, Customize, Fun, Settings, and About pages for consistent card hierarchy, spacing, typography, and action layouts.
- Improved Startup Manager table readability and spacing around the command column and scrollbar.
- Expanded Settings into a practical application configuration center with startup preferences, local data controls, and official GitHub/update links.
- Preserved existing application behavior and system tools while modernizing the presentation.
- Updated installer and release metadata for the v2.1.0 public release.

## v2.0.2

- Improved Startup Manager table readability and dark-theme styling.
- Added clearer column headers, row spacing, selection styling, and subtle hover states.
- Updated public-release documentation and installer versioning.

## v2.0.1

- Fixed the `WindowsToolsService.Open(..., label: ...)` compilation error.
- Fixed the startup registry reader to pass `RegistryHive` values rather than `RegistryKey` objects.

## v2.0.0

- Consolidated the public-ready project structure.
- Added Dashboard, Gaming Center, game shortcuts, gaming session timer, OBS configuration, Cleanup Center, Recycle Bin, Downloads Organizer, largest-file scanner, PC utilities, network diagnostics, DNS flush, Startup Manager, theme system, Settings, start-with-Windows option, GitHub release tooling, installer configuration, and GitHub Actions.
