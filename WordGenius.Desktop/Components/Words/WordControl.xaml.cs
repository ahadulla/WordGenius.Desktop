using System;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WordGenius.Desktop.Entities.Words;
using WordGenius.Desktop.Repository.Words;
using WordGenius.Desktop.Windows.Words;

namespace WordGenius.Desktop.Components.Words
{
    /// <summary>
    /// Interaction logic for WordControl.xaml
    /// </summary>
    public partial class WordControl : UserControl
    {
        public Word myWord = new Word();

        public Func<Task> Refresh { get; set; }

        public Action<Word> AlterBorder { get; set; }

        private readonly WordRepository _wordRepository;


        public WordControl()
        {
            InitializeComponent();
            this._wordRepository = new WordRepository();
        }

        public void SetData(Word word)
        {
            myWord = word;
            wordLb.Text = word.Translate + " - " + word.Text;
            descriptionTb.Text = word.Discription;
        }

        private async void playSoundBtn_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => SpeechAsync(myWord.Translate.ToString()));
        }

        public void SpeechAsync(string text)
        {
            string matn = $"{text}";

            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult, 0, new System.Globalization.CultureInfo("uz-Latn-UZ"));

            synthesizer.Speak(matn);

        }

        private void UserControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                ContextMenu contextMenu = FindResource("MyContextMenu") as ContextMenu;
                if (contextMenu != null)
                {
                    contextMenu.PlacementTarget = this;
                    contextMenu.IsOpen = true;
                }
            }
        }

        private void MenuItem_Add_Click(object sender, RoutedEventArgs e)
        {
            CreateSentence createSentence = new CreateSentence();
            createSentence.word_id = myWord.Id;
            createSentence.ShowDialog();
        }

        private async void MenuItem_Edit_Click(object sender, RoutedEventArgs e)
        {
            WordUpdate wordUpdate = new WordUpdate();
            wordUpdate.uzbekWordTb.Text = myWord.Text;
            wordUpdate.englishWordTb.Text = myWord.Translate;
            wordUpdate.descriptionTb.Text = myWord.Discription;
            wordUpdate.Id = myWord.Id;
            wordUpdate.ShowDialog();
            await Refresh();

        }

        private async void MenuItem_Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Rostan ham o'chirishni hohlaysizmi?", "Information", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.Cancel, MessageBoxOptions.None);

            if (result == MessageBoxResult.OK)
            {
                var number = await _wordRepository.DeleteAsync(myWord.Id);
                await Refresh();
            }

        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AlterBorder(myWord);
        }
    }
}
