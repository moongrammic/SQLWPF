using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WPFSQLL.View;


namespace WPFSQLL.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private Stack<Window> windowStack = new Stack<Window>();
        public ICommand LightThemeCommand { get; set; }
        public ICommand DarkThemeCommand { get; set; }
        public ICommand GreenThemeCommand { get; set; }
        public ICommand BackCommand { get; set; }

        public SettingsViewModel()
        {
            // Инициализация команд
            LightThemeCommand = new RelayCommand(LoadLightTheme);
            DarkThemeCommand = new RelayCommand(LoadDarkTheme);
            GreenThemeCommand = new RelayCommand(LoadGreenTheme);
            BackCommand = new RelayCommand(Back);
        }

        public void LoadLightTheme(object parameter)
        {
            // Установка светлой темы
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Themes/Light.xaml", UriKind.Relative) });
        }

        public void LoadDarkTheme(object parameter)
        {
            // Установка темной темы
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("/Themes/Dark.xaml", UriKind.Relative) });
        }

        public void LoadGreenTheme(object parameter)
        {
            // Установка темной темы
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("/Themes/Greenoblue.xaml", UriKind.Relative) });
        }


        public void Back(object parameter)
        {
            Login Login = new Login(); // передача ссылки на LoginViewModel в конструкторе
            Login.Show();
            Application.Current.Windows[0].Close();
        }
        
    }
}
