using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BedrockLauncher.Installer.Classes;
using BedrockLauncher.Installer.Language;

namespace BedrockLauncher.Installer.Pages
{
    /// <summary>
    /// Логика взаимодействия для LanguageSelectPage.xaml
    /// </summary>
    public partial class LanguageSelectPage : Page
    {

        public ObservableCollection<LanguageDefinition> AvaliableLangs { get; set; } = new ObservableCollection<LanguageDefinition>();

        public LanguageSelectPage()
        {
            InitializeComponent();
            LanguagesList.ItemsSource = AvaliableLangs;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).BackBtn.IsEnabled = false;
            ((MainWindow)Application.Current.MainWindow).NextBtn.IsEnabled = true;

            AvaliableLangs.Clear();
            foreach (var language in LanguageManager.GetResourceDictonaries())
            {
                language.IsSelected = language.Locale == MainWindow.Installer.CurrentLanguage;
                AvaliableLangs.Add(language);
            }

        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            var definition = ((sender as RadioButton).DataContext as LanguageDefinition).Locale;
            MainWindow.Installer.CurrentLanguage = definition;
            LanguageManager.SetLanguage(definition);
        }
    }
}
