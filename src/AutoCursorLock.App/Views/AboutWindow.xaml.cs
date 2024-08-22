// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.App.Views;

using System.Diagnostics;
using System.Windows;

/// <summary>
/// Interaction logic for AboutWindow.xaml.
/// </summary>
public partial class AboutWindow : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AboutWindow"/> class.
    /// </summary>
    public AboutWindow()
    {
        InitializeComponent();

        this.mainGrid.DataContext = this;

        Version = typeof(AboutWindow).Assembly.GetName().Version!.ToString();
    }

    /// <summary>
    /// Gets the current AutoCursorLock version number.
    /// </summary>
    public string Version { get; }

    private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
    {
        var processStartInfo = new ProcessStartInfo(e.Uri.AbsoluteUri)
        {
            UseShellExecute = true,
        };
        Process.Start(processStartInfo);
        e.Handled = true;
    }
}
