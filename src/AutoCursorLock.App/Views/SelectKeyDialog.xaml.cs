// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.App
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for SelectKeyDialog.xaml.
    /// </summary>
    public partial class SelectKeyDialog : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectKeyDialog"/> class.
        /// </summary>
        public SelectKeyDialog()
        {
            InitializeComponent();

            this.mainGrid.DataContext = this;
            this.mainGrid.Focus();
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets the key selected by the user.
        /// </summary>
        public Key SelectedKey { get; set; }

        private void MainGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            SelectedKey = e.Key;

            NotifyPropertyChanged(nameof(SelectedKey));
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
