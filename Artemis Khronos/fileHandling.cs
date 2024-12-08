using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace Artemis_Chronos
{
    internal class fileHandling
    {
        
        public static string getFolderPath()
        {
            try
            {
                string FolderPath = "Images";
                string FullFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FolderPath);
                if (!Directory.Exists(FullFolderPath))
                {
                    Directory.CreateDirectory(FullFolderPath);
                }
                return FolderPath;
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show($"Access denied: \n You do not have have permission to access or create the folder.\\n\\nDetails: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            catch (IOException ex)
            {
                MessageBox.Show($"I/O Error: \n An error ocourred while accessing the folder path. \n\n Details: {ex.Message}","Error", MessageBoxButton.OK , MessageBoxImage.Error);
                throw;
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Unexpected Error: \n An unexpected error has occurred. \n Details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }

            
        }

        public static void storeData(dataStructure dataStructure)
        {
            List<dataStructure> dataStructures;
            string FilePath = Path.Combine(getFolderPath(), "ArtworkData.json");
            try
            {
                dataStructures = loadPreviouslyData(FilePath);
                dataStructures.Add(dataStructure);
                string UpdatedContent = JsonConvert.SerializeObject(dataStructures, Formatting.Indented);

                File.WriteAllText(FilePath, UpdatedContent);
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Failed to process data structure.\\n\\nDetails: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            catch (IOException ex)
            {
                MessageBox.Show($"I/O Error: \n An error ocourred while accessing the folde path. \n\n Details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected Error: \n An unexpected error has occurred. \n Details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        public static List<dataStructure> loadPreviouslyData(string FilePath)
        {
            List<dataStructure> dataStructures;
            
            try
            {
                if (File.Exists(FilePath))
                {
                    string Content = File.ReadAllText(FilePath);
                    dataStructures = JsonConvert.DeserializeObject<List<dataStructure>>(Content) ?? new List<dataStructure>();
                }
                else
                {
                    dataStructures = new List<dataStructure>();
                }
                return dataStructures;
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Failed to process data structure.\\n\\nDetails: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            catch (IOException ex)
            {
                MessageBox.Show($"I/O Error: \n An error ocourred while accessing the folde path. \n\n Details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected Error: \n An unexpected error has occurred. \n Details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
        

    }
}
