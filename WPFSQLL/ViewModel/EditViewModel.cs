using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WPFSQLL.Model;
using WPFSQLL.View;

namespace WPFSQLL.ViewModel
{
    public class EditViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly DatabaseLogic _databaseLogic;
        private User _currentUser = new User();
        private ObservableCollection<User> _users;
        private User _selectedUser;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        public string SelectedNickname
        {
            get => _currentUser.Nickname;
            set
            {
                if (_currentUser.Nickname != value)
                {
                    _currentUser.Nickname = value;
                    OnPropertyChanged(nameof(SelectedNickname));
                }
            }
        }

        public string SelectedPassword
        {
            get => _currentUser.Password;
            set
            {
                if (_currentUser.Password != value)
                {
                    _currentUser.Password = value;
                    OnPropertyChanged(nameof(SelectedPassword));
                }
            }
        }
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));

                // Установка ID выбранного пользователя для обновления
                if (_selectedUser != null)
                {
                    _currentUser.ID = _selectedUser.ID;
                }
            }
        }


        public ICommand EditCommand { get; private set; }

        public EditViewModel()
        {
            _databaseLogic = new DatabaseLogic("Data Source=localhost;Initial Catalog=WPFS;Integrated Security=True");
            EditCommand = new RelayCommand(EditUser);
            Users = new ObservableCollection<User>(_databaseLogic.GetUsers());
        }

        public void EditUser(object parameter)
        {
            if (!string.IsNullOrEmpty(_currentUser.Nickname) && !string.IsNullOrEmpty(_currentUser.Password))
            {
                _databaseLogic.UpdateUser(_currentUser.ID, _currentUser.Nickname, _currentUser.Password);
                MessageBox.Show("Данные пользователя обновлены!");

                // Обновление списка пользователей после изменения
                Users = new ObservableCollection<User>(_databaseLogic.GetUsers());
            }
            else
            {
                MessageBox.Show("Не все данные заполнены!");
            }
        }
    }
}
