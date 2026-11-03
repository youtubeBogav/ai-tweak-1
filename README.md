# Ultimate Tweaker

A starter Windows 10/11 PC tweaking utility built with C# and WPF (.NET 8).

## Get a standalone Windows EXE without installing developer tools

This project includes a GitHub Actions workflow that builds a self-contained Windows 64-bit EXE in the cloud.

1. Create a free account at https://github.com/ if you do not already have one.
2. Create a new **public** repository (for example, `UltimateTweaker`).
3. Upload the contents of this ZIP to that repository. Make sure `.github/workflows/build.yml` is included.
4. Open the repository's **Actions** tab and select **Build Windows EXE**.
5. Click **Run workflow**. Wait for the workflow to finish successfully.
6. Open the completed run and download the artifact named `UltimateTweaker-Windows-x64`.
7. Extract the downloaded artifact and double-click `UltimateTweaker.exe`.

To create a GitHub Release with the EXE attached, create and push a tag beginning with `v`, such as `v1.0.0`. The workflow will build and attach the EXE to that release.

The EXE is self-contained, so users do not need to install .NET separately. It targets Windows 10/11 64-bit. Windows SmartScreen may still show a warning because the app is not code-signed.

## Build locally (optional)

Install the .NET 8 SDK, then run:

```powershell
dotnet publish UltimateTweaker.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
```

The EXE will be under `bin\\Release\\net8.0-windows\\win-x64\\publish\\`.

## Included actions

- Enable Windows Game Mode for the current user.
- Switch to Windows' built-in High performance power plan.
- Flush the DNS resolver cache.
- Clean the current user's Temp directory (files in use are skipped).

## Safety notes

- Some actions request Administrator permission through Windows UAC.
- High performance mode can increase power use, heat, and fan noise.
- Flushing DNS is a troubleshooting action, not a guaranteed ping or speed improvement.
- Temp cleanup permanently deletes selected temporary files. Review the confirmation prompt.
- This is an initial safe-feature release, not a magic FPS booster. Results depend on your hardware, drivers, games, and Windows configuration.
- Create a Windows restore point before experimenting with system-level changes.
