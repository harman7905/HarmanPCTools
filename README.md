# Harman PC Tools

A modular Windows desktop toolkit for gaming, system information, cleanup, organization, diagnostics, startup visibility, customization and everyday PC tasks.

## v2.0.2

### Included
- Live Home dashboard: CPU, RAM, storage, free RAM, uptime, network and GPU name
- Gaming Center with session timer
- Persistent game shortcuts
- Steam / Epic / OBS launchers
- OBS configuration
- Game Mode / Display / Graphics / Network / Task Manager shortcuts
- Cleanup Center
- User temp cleanup
- Recycle Bin cleanup
- Windows Storage and Disk Cleanup shortcuts
- Downloads Organizer
- Largest-download scanner
- PC utility shortcuts
- Ping diagnostics and DNS flush
- Startup Manager
- Five dark themes
- Subtle hover effects
- Settings
- Optional start-with-Windows
- Configurable GitHub repository URL
- Fun Zone
- Windows single-file publishing
- Inno Setup installer
- GitHub Actions CI
- GitHub tag-based release workflow

## Requirements

- Windows 10/11
- Visual Studio Community with WPF
- .NET 8 SDK

## Run

Open `HarmanPCTools.sln`.

Use:

`Build -> Rebuild Solution`

Then:

`Ctrl + F5`

## Publish

```powershell
.\scripts\publish.ps1
```

## Build installer

Install Inno Setup 6, then:

```powershell
.\scripts\build-installer.ps1
```

## Public GitHub repository

Create a public repository named `HarmanPCTools`, then:

```powershell
git init
git add .
git commit -m "Initial Harman PC Tools 2.0 release"
git branch -M main
git remote add origin https://github.com/YOUR-USERNAME/HarmanPCTools.git
git push -u origin main
```

Create the current release using:

```powershell
git tag v2.0.2
git push origin v2.0.2
```

The included GitHub Actions workflow publishes a Windows executable and installer for version tags.

## Updating later

For a future patch such as `2.0.3`:
1. Update the version values in `HarmanPCTools.csproj`.
2. Add the changes to `CHANGELOG.md`.
3. Test locally.
4. Commit and push.
5. Create and push `v2.0.3`.

## Safety

System-changing actions remain explicit. Cleanup asks for confirmation. Startup entries are shown rather than silently disabled. The app does not self-update or silently execute downloaded code.


## 2.0.2 release note
This release includes the Startup Manager readability/UI improvements and the v2.0.1 compilation fixes.
