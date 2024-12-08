using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Artemis_Chronos.createPrompt;
using static Artemis_Chronos.gptConnection;
using static Artemis_Chronos.fileHandling;
using System.IO;
using Newtonsoft.Json;
using Microsoft.VisualBasic;

namespace Artemis_Chronos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<dataStructure> oldImages = new List<dataStructure>();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private async void BtnCreateImage_Click(object sender, RoutedEventArgs e)
        {
            dataStructure data = new dataStructure();
            int TokenHistoricalPrompt = 200;
            int TokenArtworkConcept = 400;

            try
            {
                Status.Text = "Henter Historisk prompt";
                data.HistoricalPrompt = createHistoricalPrompt();
                Status.Text = "Henter historisk hendelse";
                data.HistoricalEvent = await sendPromptToChatGPT(data.HistoricalPrompt, TokenHistoricalPrompt);
                Status.Text = "Lager bilde prompt";
                data.ArtworkPrompt = createArtworkConcept(data.HistoricalEvent);
                Status.Text = "Lager bilde konsept";
                data.ArtworkConcept = await sendPromptToChatGPT(data.ArtworkPrompt, TokenArtworkConcept);

                string Filepath = getFolderPath();
                Status.Text = "Lager bilde";
                data.Filepath = await downloadImageFromDalle(data.ArtworkConcept, Filepath);

                Status.Text = "Lagrer opplysninger";
                storeData(data);

                string TextOut = $"{data.HistoricalEvent}\r\n\n{data.ArtworkConcept}";
                TodaysTheme.Text = TextOut;
                string FullFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,data.Filepath);
                TodaysImage.Source = new BitmapImage(new Uri(FullFilePath));
                Status.Text = "Ferdig med å lage dagens bilde";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected Error: \n An unexpected error has occurred. \n Details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }



        private void PreviouslyImages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(PreviouslyImages.SelectedItem != null)
            {
                dataStructure selectedItem = PreviouslyImages.SelectedItem as dataStructure;

                string Summary = $"{selectedItem.HistoricalEvent}\n\n {selectedItem.ArtworkConcept}";
                OldTheme.Text = Summary;
                string FullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, selectedItem.Filepath);
                OldImage.Source = new BitmapImage(new Uri(FullPath));
            }
            
        }

        private void GetPreviouslyImages_Click(object sender, RoutedEventArgs e)
        {
            string FilePath = System.IO.Path.Combine(getFolderPath(), "ArtworkData.json");
            oldImages = loadPreviouslyData(FilePath);
            PreviouslyImages.ItemsSource = oldImages;
        }

        private void BtnCopyTodaysTheme_Click(object sender, RoutedEventArgs e)
        {
            string TransferToClipBoard = TodaysTheme.Text;
            if (!string.IsNullOrEmpty(TransferToClipBoard))
            {
                Clipboard.SetText(TransferToClipBoard);
            }
            else
            {
                MessageBox.Show("There is no text to in the field to be copied");
            }
        }

        private void CopyOldTheme_Click(object sender, RoutedEventArgs e)
        {
            string TransferToClipBoard = OldTheme.Text;
            if (!string.IsNullOrEmpty(TransferToClipBoard))
            {
                Clipboard.SetText(TransferToClipBoard);
            }
            else
            {
                MessageBox.Show("There is no text to in the field to be copied");
            }

        }

        private void BtnChangeAPI_Click(object sender, RoutedEventArgs e)
        {
            string API = string.Empty;
            API = Interaction.InputBox("Please, enter your OpenAI API-key:");

            if (!string.IsNullOrEmpty(API))
            {
                setApiKeyLocally(API);
            }
        }
    }
}