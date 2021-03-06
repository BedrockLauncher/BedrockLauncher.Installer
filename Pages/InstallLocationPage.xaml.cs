using System;
using System.IO;
using System.Windows.Controls;
using System.Windows;
using BedrockLauncher.Installer.Classes;


namespace BedrockLauncher.Installer.Pages
{
    /// <summary>
    /// Логика взаимодействия для WelcomePage.xaml
    /// </summary>
    public partial class InstallLocationPage : Page
    {

        private bool IsInit = false;

        public InstallLocationPage()
        {
            InitializeComponent();
            registerIconCheckBox.IsChecked = BedrockLauncher.Installer.MainWindow.Installer.RegisterAsProgram;
            desktopIconCheckBox.IsChecked = BedrockLauncher.Installer.MainWindow.Installer.MakeDesktopIcon;
            startMenuCheckBox.IsChecked = BedrockLauncher.Installer.MainWindow.Installer.MakeStartMenuIcon;
            IsInit = true;
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            installPathTextBox.Text = Path.Combine(ProgramFiles(), "Minecraft Bedrock Launcher");
        }
        static string ProgramFiles()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        }

        private void browseBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var folderBrowser = new System.Windows.Forms.FolderBrowserDialog())
            {
                folderBrowser.SelectedPath = ProgramFiles();
                System.Windows.Forms.DialogResult result = folderBrowser.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
                {
                    installPathTextBox.Text = Path.Combine(folderBrowser.SelectedPath, "Minecraft Bedrock Launcher");
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void IconCheckBoxes_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (IsInit)
            {
                BedrockLauncher.Installer.MainWindow.Installer.MakeDesktopIcon = desktopIconCheckBox.IsChecked.Value;
                BedrockLauncher.Installer.MainWindow.Installer.MakeStartMenuIcon = startMenuCheckBox.IsChecked.Value;
                BedrockLauncher.Installer.MainWindow.Installer.RegisterAsProgram = registerIconCheckBox.IsChecked.Value;
            }
        }
    }
}
