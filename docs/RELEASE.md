# Release guide

1. Update `<Version>`, `<AssemblyVersion>` and `<FileVersion>`.
2. Update `CHANGELOG.md`.
3. Test locally.
4. Commit and push.
5. Push a matching `vX.Y.Z` tag.

GitHub Actions builds the Windows publish artifact and Inno Setup installer.

Local:

```powershell
.\scripts\publish.ps1
.\scripts\build-installer.ps1
```
