using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
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
using WordGenius.Desktop.Entities.Words;

namespace WordGenius.Desktop.Components.Words
{
    /// <summary>
    /// Interaction logic for WordControl.xaml
    /// </summary>
    public partial class WordControl : UserControl
    {
        public  Word word1 = new Word();

        public WordControl()
        {
            InitializeComponent();
        }

        public void SetData(Word word)
        {
            word1 = word;
            wordLb.Text = word.Translate + " - " + word.Text;
            descriptionTb.Text = word.Discription;
        }

        private async void playSoundBtn_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(()=>SpeechAsync(word1.Translate.ToString()));
        }

        public void SpeechAsync(string text)
        {
            string metin = $"{text}";

            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult, 0, new System.Globalization.CultureInfo("uz-Latn-UZ"));

            synthesizer.Speak(metin);

        }
    }
}
