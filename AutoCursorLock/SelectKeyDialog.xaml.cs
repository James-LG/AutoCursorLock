// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for SelectKeyDialog.xaml.
    /// </summary>
    public partial class SelectKeyDialog : Window, INotifyPropertyChanged
    {
        public SelectKeyDialog()
        {
            InitializeComponent();

            this.mainGrid.DataContext = this;
            this.mainGrid.Focus();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Key SelectedKey { get; set; }

        private void mainGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            SelectedKey = e.Key;

            NotifyPropertyChanged(nameof(SelectedKey));
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
