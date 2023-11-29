using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WPFSQLL.Model;
using WPFSQLL.View;

namespace WPFSQLL.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private User _currentUser = new User();

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private DatabaseLogic _databaseLogic;
        public string Nickname
        {
            get { return _currentUser.Nickname; }
            set
            {
                if (_currentUser.Nickname != value)
                {
                    _currentUser.Nickname = value;
                    OnPropertyChanged(nameof(Nickname));
                }
            }
        }

        public string Password
        {
            get { return _currentUser.Password; }
            set
            {
                if (_currentUser.Password != value)
                {
                    _currentUser.Password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public ICommand LoginCommand { get; private set; }
        public ICommand RegisterCommand { get; private set; }
        public ICommand SettingsCommand { get; private set; }
        public ICommand OpenLoginCommand { get; private set; }
        public ICommand RegCommand { get; private set; }

        public LoginViewModel()
        {
            _databaseLogic = new DatabaseLogic("Data Source=localhost;Initial Catalog=WPFS;Integrated Security=True");
            LoginCommand = new RelayCommand(Login);
            RegisterCommand = new RelayCommand(Register);
            SettingsCommand = new RelayCommand(Settings);
            OpenLoginCommand = new RelayCommand(OpenLogin);
            RegCommand = new RelayCommand(Reg);
        }


        public void Login(object parameter)
        {
            bool isLoggedIn = _databaseLogic.UserExists(_currentUser);

            if (isLoggedIn)
            {
                MessageBox.Show("Вход выполнен успешно!");
                Data DataWindow = new Data();
                DataWindow.Show();
                Application.Current.Windows[0].Close();
            }
            else
            {
                MessageBox.Show("Ошибка входа. Пожалуйста, проверьте учетные данные.");
            }
        }


        public void Register(object parameter)
        {
            Register register = new Register();
            register.Show();

        }

        public void Settings(object parameter)
        {
            Settings Settings = new Settings(); // передача ссылки на LoginViewModel в конструкторе
            Settings.Show();
            
        }

        public void OpenLogin(object parameter)
        {
            Login login = new Login();
            login.Show();
            Application.Current.Windows[0].Close();
        }

        public void Reg(object parameter)
        {
            // Проверка наличия заполненных данных о пользователе
            if (!string.IsNullOrWhiteSpace(_currentUser.Nickname) && !string.IsNullOrWhiteSpace(_currentUser.Password))
            {
                // Создание нового пользователя на основе введенных данных
                User newUser = new User
                {
                    Nickname = _currentUser.Nickname,
                    Password = _currentUser.Password
                };

                // Добавление нового пользователя в базу данных
                _databaseLogic.AddUser(newUser);

                MessageBox.Show("Пользователь успешно зарегистрирован!");

                // Очистка полей после регистрации
                _currentUser.Nickname = string.Empty;
                _currentUser.Password = string.Empty;
            }
            else
            {
                MessageBox.Show("Введите данные для регистрации!");
            }
        }
    }
}
