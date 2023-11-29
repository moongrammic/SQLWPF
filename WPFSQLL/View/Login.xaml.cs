using System.Windows;
using WPFSQLL.ViewModel;

namespace WPFSQLL.View
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }

    }
}
