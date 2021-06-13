using System;
using System.Collections.Generic;
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
using System.IO;
using BedrockLauncher.Installer.Pages;
using System.Diagnostics;
using System.Reflection;
using BedrockLauncher.Installer.Classes;

namespace BedrockLauncher.Installer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LanguageSelectPage languageSelectPage = new LanguageSelectPage();
        private WelcomePage welcomePage = new WelcomePage();
        private LicenseAgreementPage licenseAgreementPage = new LicenseAgreementPage();
        private InstallLocationPage installLocationPage = new InstallLocationPage();
        private InstallationProgressPage installationProgressPage = new InstallationProgressPage();
        private InstallTypePage installTypePage = new InstallTypePage();


        public static LauncherSetup Installer = new LauncherSetup();

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) { this.DragMove(); }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Installer.CancelInstall();
        }

        private void Navigate(object content)
        {
            if (content == installLocationPage) NextBtn.SetResourceReference(Button.ContentProperty, "Installer_Install_Text");
            else NextBtn.SetResourceReference(Button.ContentProperty, "Installer_Next_Text");

            MainFrame.Navigate(content);
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            // if u know how to make it better, contact me
            switch (MainFrame.Content.GetType().Name.ToString())
            {
                case nameof(LanguageSelectPage):
                    Navigate(welcomePage);
                    break;
                case nameof(WelcomePage):
                    Navigate(licenseAgreementPage);
                    break;
                case nameof(LicenseAgreementPage):
                    Navigate(installTypePage);
                    break;
                case nameof(InstallTypePage):
                    Navigate(installLocationPage);
                    break;
                case nameof(InstallLocationPage):
                    VerifyForInstall();
                    break;
                default:
                    break;
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (MainFrame.Content.GetType().Name.ToString())
            {
                case nameof(WelcomePage):
                    Navigate(languageSelectPage);
                    break;
                case nameof(LicenseAgreementPage):
                    Navigate(welcomePage);
                    break;
                case nameof(InstallTypePage):
                    Navigate(licenseAgreementPage);
                    break;
                case nameof(InstallLocationPage):
                    Navigate(installTypePage);
                    break;
                default:
                    break;
            }
        }

        private void VerifyForInstall()
        {
            Installer.Path = installLocationPage.installPathTextBox.Text;

            if (Directory.Exists(Installer.Path))
            {
                var files = Directory.GetFiles(Installer.Path).ToList();
                var directory = Directory.GetDirectories(Installer.Path).ToList();
                if (files.Count() == 0 && directory.Count() == 0) RunInstaller();
                else ShowError();
            }
            else RunInstaller();

            void RunInstaller()
            {
                Installer.StartInstall();
            }

            void ShowError()
            {
                var result = MessageBox.Show("Directory is not empty! Do you want to delete all of it's contents?", "", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Directory.Delete(Installer.Path, true);
                    RunInstaller();
                }
            }
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            Installer.ProgressPage = installationProgressPage;
            string[] ConsoleArgs = Environment.GetCommandLineArgs();
            bool isSilent = false;
            bool isBeta = false;
            string Path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            foreach (string argument in ConsoleArgs)
            {
                if (argument.StartsWith("--"))
                {
                    Console.WriteLine("Recieved argument: " + argument);
                    if (argument == "--silent") isSilent = true;
                    if (argument == "--beta") isBeta = true;
                    if (argument.StartsWith("--path=")) Path = argument.Replace("--path=", "");
                }
            }

            Installer.Path = Path;
            Installer.IsBeta = isBeta;

            if (isSilent)
            {
                this.Hide();
                Installer.Silent = isSilent;
                Installer.StartInstall();
            }
        }
    }
}
