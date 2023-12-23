using DiffPlex.Wpf;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Media;
using TestingSamples.FilesDiffControls;


namespace TestingSamples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string namespaceName = "TestingSamples.TestingClasses";
            string className = "Person.cs";

            // Content to be added dynamically
            string dynamicContent = "public string DynamicContent() { return \"This is dynamically added content.\"; }" +
                "" + Environment.NewLine + "\r\n" +
                " public string DynamicContent2() { return \"This is dynamically added content.\"; }" + "\r\n";

            try
            {
                // Read the existing content of the class
                string filePath = $"{AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug\\net6.0-windows\\", "")}TestingClasses\\{className}";
                string existingContent = File.ReadAllText(filePath);

                // Append the dynamic content to the existing content
                //string modifiedContent = existingContent + Environment.NewLine + dynamicContent;
                // Append the dynamic content to the existing content
                string modifiedContent = existingContent.Replace("//#", $"{Environment.NewLine}{dynamicContent}");
                // Write back the modified content to the file
                File.WriteAllText(filePath, modifiedContent);
                LoadDataAfterDynamicaAdditionClass(existingContent, modifiedContent);
                // TxtShow.Text = modifiedContent;
                //MessageBox.Show(modifiedContent, "Dynamic Content", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //Delete registry
        private void UnregisterExtension()
        {
            string fileExtension = ".ariarchfle";  // Replace with your desired file extension
            UnregisterFileExtension(fileExtension);
        }

        private void UnregisterFileExtension(string fileExtension)
        {
            try
            {
                string progId = "CustomType";//[ Declares a string variable named progId and assigns it the value "CustomType". This variable is used to represent the ProgID (Program ID), which is a string identifier associated with a specific file type.]

                // Delete the key for the file extension
                Registry.ClassesRoot.DeleteSubKeyTree(fileExtension, false);//[Deletes the registry key for the specified file extension (fileExtension) under HKEY_CLASSES_ROOT. The false argument indicates that the operation should not throw an exception if the subkey does not exist.]

                // Delete the ProgID key
                Registry.ClassesRoot.DeleteSubKeyTree(progId, false);//[Deletes the registry key for the specified ProgID (progId) under HKEY_CLASSES_ROOT. This step removes the association between the file extension and the ProgID.]

                Console.WriteLine($"File extension '{fileExtension}' unregistered with associated items.");
                MessageBox.Show($"File extension '{fileExtension}' unregistered with associated items.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        //Ends here
        private void SetRegisterExtension()
        {
            string fileExtension = ".ariarchfle";  // Replace with your desired file extension
            string iconFilePath = "C:\\ProgramData\\Testing\\Imagesicons\\icons_32x32.ico";  // Replace with the path to your icon file
            string executablePath = "C:\\ProgramData\\Testing\\Executable\\WpfAppTest.exe";  // Replace with the path to your executable
            RegisterFileExtension(fileExtension, iconFilePath, executablePath);
        }
        private void RegisterFileExtension(string fileExtension, string iconFilePath, string executablePath)
        {
            try
            {
                // Open the key for the file extension
                using (RegistryKey extensionKey = Registry.ClassesRoot.CreateSubKey(fileExtension))//[Opens or creates a registry key under HKEY_CLASSES_ROOT for the specified file extension (fileExtension). ]
                {
                    if (extensionKey != null)//[Checks if the extension key was successfully created or opened.]
                    {
                        // Set the default value to the ProgID (file type identifier)
                        extensionKey.SetValue(null, "CustomType");//[ Sets the default value of the extension key to a custom ProgID ("CustomType"), which is a file type identifier.]

                        // Create a new key for the ProgID
                        using (RegistryKey progIdKey = Registry.ClassesRoot.CreateSubKey("CustomType"))//[Creates or opens a registry key for the specified ProgID ("CustomType")]
                        {
                            if (progIdKey != null)
                            {
                                // Set the default value for the ProgID key to the description of your file type
                                progIdKey.SetValue(null, "CustomTypeId");//[Sets the default value of the ProgID key to a custom description ("CustomTypeId") of your file type.]

                                // Create a new key for the DefaultIcon
                                using (RegistryKey iconKey = progIdKey.CreateSubKey("DefaultIcon"))//[Creates or opens a registry key for the DefaultIcon associated with the ProgID.]
                                {
                                    if (iconKey != null)
                                    {
                                        // Set the default value of the DefaultIcon key to the path of your icon file
                                        iconKey.SetValue(null, iconFilePath);//[Sets the default value of the DefaultIcon key to the path of your icon file (iconFilePath).]

                                        // Create a new key for the shell open command
                                        using (RegistryKey commandKey = progIdKey.CreateSubKey(@"shell\open\command"))//[Creates or opens a registry key for the shell open command associated with the ProgID.]
                                        {
                                            if (commandKey != null)
                                            {
                                                // Set the default value of the command key to the path of your executable
                                                commandKey.SetValue(null, executablePath);//[Sets the default value of the command key to the path of your executable (executablePath).]
                                                Console.WriteLine($"File extension '{fileExtension}' registered with icon and executable.");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void ButtonExetnsion_Click(object sender, RoutedEventArgs e)
        {
            SetRegisterExtension();
            Console.WriteLine("File extension association created successfully.");
            MessageBox.Show("File extension association created successfully.");
        }
        private bool IsFileExtensionRegistered(string fileExtension)
        {
            string keyName = $@"Software\Classes\{fileExtension}";
            using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(keyName))
            {
                return key != null;
            }
        }
        private void LoadData()
        {
            var now = DateTime.Now;
            var isDark = now.Hour < 6 || now.Hour >= 18;
            DiffView.Foreground = new SolidColorBrush(isDark ? Color.FromRgb(240, 240, 240) : Color.FromRgb(32, 32, 32));
            DiffView.OldText = SampleData.DuplicateText(SampleData.OldText, 2);
            DiffView.NewText = SampleData.DuplicateText(SampleData.NewText, 2);
            DiffView.SetHeaderAsOldToNew();
            Background = new SolidColorBrush(isDark ? Color.FromRgb(32, 32, 32) : Color.FromRgb(251, 251, 251));
            DiffButton.Background = FutherActionsButton.Background = WindowButton.Background = new SolidColorBrush(isDark ? Color.FromRgb(80, 160, 240) : Color.FromRgb(160, 216, 240));
            IgnoreUnchangedCheckBox.Content = SampleData.RemoveHotkey(DiffView.CollapseUnchangedSectionsToggleTitle);
            MarginLineCountLabel.Content = SampleData.RemoveHotkey(DiffView.ContextLinesMenuItemsTitle);
        }
        private void LoadDataAfterDynamicaAdditionClass(string oldText, string newText)
        {
            var now = DateTime.Now;
            var isDark = now.Hour < 6 || now.Hour >= 18;
            DiffViewForClass.Foreground = new SolidColorBrush(isDark ? Color.FromRgb(240, 240, 240) : Color.FromRgb(32, 32, 32));
            DiffViewForClass.OldText = oldText; //SampleData.DuplicateText(SampleData.OldText, 100);
            DiffViewForClass.NewText = newText; //SampleData.DuplicateText(SampleData.NewText, 100);
            DiffViewForClass.SetHeaderAsOldToNew();
            Background = new SolidColorBrush(isDark ? Color.FromRgb(32, 32, 32) : Color.FromRgb(251, 251, 251));
            DiffButton.Background = FutherActionsButton.Background = WindowButton.Background = new SolidColorBrush(isDark ? Color.FromRgb(80, 160, 240) : Color.FromRgb(160, 216, 240));
            IgnoreUnchangedCheckBox.Content = SampleData.RemoveHotkey(DiffView.CollapseUnchangedSectionsToggleTitle);
            MarginLineCountLabel.Content = SampleData.RemoveHotkey(DiffView.ContextLinesMenuItemsTitle);
        }
        private void DiffButton_Click(object sender, RoutedEventArgs e)
        {
            if (DiffView.IsInlineViewMode)
            {
                DiffView.ShowSideBySide();
                return;
            }

            DiffView.ShowInline();
        }

        private void FutherActionsButton_Click(object sender, RoutedEventArgs e)
        {
            DiffView.OpenViewModeContextMenu();
        }

        private void WindowButton_Click(object sender, RoutedEventArgs e)
        {
            var has = false;
            foreach (var w in Application.Current.Windows)
            {
                if (w is DiffWindow dw)
                {
                    dw.Activate();
                    has = true;
                    break;
                }
            }

            if (has) return;
            var win = new DiffWindow();
            win.OpenFileOnBoth();
            win.Show();
        }

        private void DeleteExtension_Click(object sender, RoutedEventArgs e)
        {
            UnregisterExtension();
        }

        //private void XmlButton_Click(object sender, RoutedEventArgs e)
        //{
        //    Serialize.ToJson();
        //}
    }
}