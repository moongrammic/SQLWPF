using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WPFSQLL.Model;
using WPFSQLL.View;

namespace WPFSQLL.ViewModel
{
    public class DataViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<User> _users;
        private User _selectedUser;

        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged("Users");
            }
        }
        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        private DatabaseLogic _databaseLogic;

        public ICommand SetCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand ResCommand { get; set; }
        public DataViewModel()
        {
            _databaseLogic = new DatabaseLogic("Data Source=localhost;Initial Catalog=WPFS;Integrated Security=True");
            LoadUserData();
            SetCommand = new RelayCommand(Set);
            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);
            AddCommand = new RelayCommand(Add);
            ResCommand = new RelayCommand(Data);
        }

        private void LoadUserData()
        {
            var dataTable = _databaseLogic.GetUserData();
            Users = ConvertDataTableToObservableCollection(dataTable);
        }

        private void Data(object parameter)
        {
            LoadUserData();
        }
        private ObservableCollection<User> ConvertDataTableToObservableCollection(System.Data.DataTable dt)
        {
            var users = new ObservableCollection<User>();

            foreach (System.Data.DataRow row in dt.Rows)
            {
                var user = new User
                {
                    ID = Convert.ToInt32(row["ID"]),
                    Nickname = row["Nickname"].ToString(),
                    Password = row["Password"].ToString()
                };
                users.Add(user);
            }

            return users;
        }
        public void Set(object parameter)
        {
            Settings Settings = new Settings(); // передача ссылки на DataViewModel в конструкторе
            Settings.Show();
            
        }

        public void Edit(object parameter)
        {
            Edit Edit = new Edit();
            Edit.Show();
        }

        public void Delete(object parameter)
        {
            if (SelectedUser != null)
            {
                _databaseLogic.DeleteUser(SelectedUser.ID); // Удаляем из БД

                Users.Remove(SelectedUser); // Удаляем из списка в ViewModel

                MessageBox.Show("Пользователь удален!");
            }
            else
            {
                MessageBox.Show("Выберите пользователя для удаления!");
            }
        }
       
        public void Add(object parameter)
        {
            Register register = new Register();
            register.Show();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
