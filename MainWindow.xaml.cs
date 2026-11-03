using System.Diagnostics;
using System.Security.Principal;
using System.Windows;
using Microsoft.Win32;
using UltimateTweaker.Services;

namespace UltimateTweaker;

public partial class MainWindow : Window
{
    private readonly TweakService _tweaks = new();

    public MainWindow()
    {
        InitializeComponent();
        OsValue.Text = Environment.OSVersion.VersionString;
        DeviceValue.Text = Environment.MachineName;
        Log("Application started. No changes have been made.");
        Log("Some actions require running as Administrator.");
    }

    private void Log(string message)
    {
        LogBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        LogBox.ScrollToEnd();
    }

    private void Dashboard_Click(object sender, RoutedEventArgs e) => SetPage("Dashboard", "A safer, simpler way to tune your Windows PC.");
    private void Performance_Click(object sender, RoutedEventArgs e) => SetPage("Performance", "Game Mode and power plan controls are available on the dashboard.");
    private void Network_Click(object sender, RoutedEventArgs e) => SetPage("Network", "Flush the local DNS cache when troubleshooting name-resolution issues.");
    private void Cleanup_Click(object sender, RoutedEventArgs e) => SetPage("Cleanup", "Clean only your current user's temporary files. Files in use are skipped.");
    private void About_Click(object sender, RoutedEventArgs e) => SetPage("About & Safety", "This app avoids bulk registry scripts and disabling security features. Review each action before using it.");

    private void SetPage(string title, string subtitle)
    {
        PageTitle.Text = title;
        PageSubtitle.Text = subtitle;
    }

    private void GameMode_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            _tweaks.EnableGameMode();
            Log("Windows Game Mode enabled for the current user.");
            MessageBox.Show("Windows Game Mode was enabled for your user account.", "Ultimate Tweaker", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex) { ShowError(ex); }
    }

    private void Power_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show("This switches Windows to its built-in High performance power plan. It may increase power use, heat, and fan noise. Continue?",
            "Confirm power plan change", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result != MessageBoxResult.Yes) return;
        try
        {
            _tweaks.SetHighPerformancePowerPlan();
            Log("Requested High performance power plan.");
            MessageBox.Show("High performance power plan activated (if available on this PC).", "Ultimate Tweaker", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex) { ShowError(ex); }
    }

    private void FlushDns_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            _tweaks.FlushDns();
            Log("DNS cache flushed.");
            MessageBox.Show("DNS cache flushed successfully.", "Ultimate Tweaker", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex) { ShowError(ex); }
    }

    private void CleanTemp_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show("Delete files and folders inside your current user's Temp folder? Files currently in use will be skipped.",
            "Confirm cleanup", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result != MessageBoxResult.Yes) return;
        try
        {
            int removed = _tweaks.CleanUserTemp();
            Log($"Temporary file cleanup completed. {removed} item(s) removed.");
            MessageBox.Show($"Cleanup completed. Removed {removed} item(s).", "Ultimate Tweaker", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex) { ShowError(ex); }
    }

    private void ShowError(Exception ex)
    {
        Log("Error: " + ex.Message);
        MessageBox.Show(ex.Message, "Action failed", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
