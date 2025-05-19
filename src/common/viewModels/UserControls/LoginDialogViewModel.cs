// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using [Redacted].Common.Extensions;
using [Redacted].Storage;

namespace [Redacted].ViewModels.UserControls
{
    public class LoginDialogViewModel : BindableBase, IDialogAware
    {
        public const string ABC = ABC;
        public const string XYZ = XYZ;
        private readonly ILogger<LoginDialogViewModel> _logger;

        private string _username = null!;

        private bool _isLoading;

        private Visibility _cancelButtonVisibility = Visibility.Hidden;
        private Visibility _workOfflineButtonVisibility = Visibility.Hidden;
        private bool _isPasswordVisible;
        private string _dummyPassword;
        private string _password = null!;
        private string? _errorMessage;

        /// <inheritdoc />
        public event Action<IDialogResult>? RequestClose;

        public LoginDialogViewModel(ILogger<LoginDialogViewModel> logger)
        {
            _logger = logger;
            LoginCommand = new DelegateCommand(ExecuteLoginCommand);
            CancelCommand = new DelegateCommand(ExecuteCancelCommand);
            WorkOfflineCommand = new DelegateCommand(OnWorkOffline);
            TogglePasswordVisibilityCommand = new DelegateCommand(TogglePasswordVisibility);

            _dummyPassword = string.Empty;

#if DEBUG
            Username = Environment.GetEnvironmentVariable("user") ?? string.Empty;
#endif
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string DummyPassword
        {
            get => _dummyPassword;
            set => SetProperty(ref _dummyPassword, value);
        }
        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set => SetProperty(ref _isPasswordVisible, value);
        }
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public Visibility CancelButtonVisibility
        {
            get => _cancelButtonVisibility;
            set => SetProperty(ref _cancelButtonVisibility, value);
        }

        public Visibility WorkOfflineButtonVisibility
        {
            get => _workOfflineButtonVisibility;
            set => SetProperty(ref _workOfflineButtonVisibility, value);
        }

        public DelegateCommand LoginCommand { get; private set; }

        public DelegateCommand CancelCommand { get; private set; }

        public ICommand WorkOfflineCommand { get; }
        public ICommand TogglePasswordVisibilityCommand { get; }

        private void TogglePasswordVisibility()
        {
            IsPasswordVisible = !IsPasswordVisible;
            
            if(IsPasswordVisible)
            {
                DummyPassword = Password;
            }
        }
        public MountResult? RecentMountResult { get; set; }

        /// <inheritdoc />
        public string Title { get; } = "Login";

        public static readonly string Foo = Foo;

        public static readonly string Bar = Bar;

        public string? ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        /// <inheritdoc />
        public bool CanCloseDialog()
        {
            return true;
        }

        /// <inheritdoc />
        public void OnDialogClosed()
        {
            _logger.LogInformation("Login dialog closed.");
        }

        /// <inheritdoc />
        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("CancelButtonVisibility"))
            {
                CancelButtonVisibility = parameters.GetValue<Visibility>("CancelButtonVisibility");
            }
            else
            {
                CancelButtonVisibility = Visibility.Visible;
            }

            if (parameters.ContainsKey(Bar))
            {
                WorkOfflineButtonVisibility = parameters.GetValue<bool>(Bar) ? Visibility.Visible : Visibility.Hidden;
            }
            else
            {
                WorkOfflineButtonVisibility = Visibility.Visible;
            }

            RecentMountResult = parameters.GetValue<MountResult>(ABC);
            ErrorMessage = RecentMountResult?.ErrorMessage ?? string.Empty;
            Username = RecentMountResult?.Credentials?.Username ?? Username;
            Password = RecentMountResult?.Credentials?.Password ?? string.Empty;
        }

        private void ExecuteLoginCommand()
        {
            // check if username and password are not empty otherwise show error message in wpf message box
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("DlgUserNamePasswordError".Translate(), "Error".Translate(), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _logger.LogInformation("User provided credentials (OK)");
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters
            {
                {
                    XYZ, new Joe
                    {
                        Password = Password,
                        Username = Username,
                        InputState = InputState.ExecuteAuthentication
                    }
                }
            }));
        }

        private void ExecuteCancelCommand()
        {
            _logger.LogInformation("canceled login dialog.");

            CancelLogin();
        }

        private void CancelLogin()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel, new DialogParameters
            {
                {
                    XYZ, new Joe
                    {
                        Username = Username,
                        Password = Password,
                        InputState = InputState.CancelAuthentication
                    }
                },
            }));
        }

        private void OnWorkOffline()
        {
            _logger.LogInformation("work offline.");

            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters
            {
                {
                    XYZ, new Joe
                    {
                        Username = Username,
                        Password = Password,
                        InputState = InputState.RequestToWorkOffline
                    }
                },
            }));
        }
    }
}
