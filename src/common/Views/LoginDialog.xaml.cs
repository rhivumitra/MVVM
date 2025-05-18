// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using [Redact].Common.ViewModels.UserControls;

namespace [Redact].Common.Views.UserControls
{
    /// <summary>
    /// Interaction logic for LoginDialog.xaml
    /// </summary>
    public partial class LoginDialog : UserControl
    {
        public LoginDialog()
        {
            InitializeComponent();
            PasswordBoxHidden.PasswordChanged += (_, _) =>
            {
                if (DataContext is LoginDialogViewModel viewModel && !viewModel.IsPasswordVisible)
                {
                    viewModel.Password = PasswordBoxHidden.Password;
                }
            };
            PasswordBoxVisible.TextChanged += (_, _) =>
            {
                if (DataContext is LoginDialogViewModel viewModel && viewModel.IsPasswordVisible)
                {
                    viewModel.Password = PasswordBoxVisible.Text;
                }
            };
            DataContextChanged += LoginDialog_DataContextChanged;
        }
        private void LoginDialog_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is LoginDialogViewModel newViewModel)
            {
                newViewModel.PropertyChanged += ViewModel_PropertyChanged;
            }
        }
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LoginDialogViewModel.Password))
            {
                if (DataContext is LoginDialogViewModel viewModel)
                {
                    // Only update PasswordBox if necessary to avoid feedback loop
                    if (PasswordBoxHidden.Password != viewModel.Password)
                    {
                        PasswordBoxHidden.Password = viewModel.Password;
                    }

                    if (PasswordBoxVisible.Text != viewModel.Password)
                    {
                        PasswordBoxVisible.Text = viewModel.Password;
                    }
                }
            }
        }

        public void PasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && DataContext is LoginDialogViewModel viewModel)
            {
                if (viewModel.IsPasswordVisible)
                {
                    viewModel.Password = PasswordBoxVisible.Text;
                }
                else
                {
                    viewModel.Password = PasswordBoxHidden.Password;
                }
                if (viewModel.LoginCommand.CanExecute())
                {
                    viewModel.LoginCommand.Execute();
                }
            }
        }

        private void PasswordBoxHidden_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is LoginDialogViewModel viewModel && sender is PasswordBox pb)
            {
                viewModel.Password = pb.Password;
                
            }
        }
    }
}
