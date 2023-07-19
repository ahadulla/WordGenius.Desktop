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
using WordGenius.Desktop.Entities.Words;
using WordGenius.Desktop.Interfaces.Sentences;
using WordGenius.Desktop.ViewModels;
using WordGenius.Desktop.Windows.Sentences;
using WordGenius.Repositories.Sentences;

namespace WordGenius.Desktop.Components.Sentences
{
    /// <summary>
    /// Interaction logic for SentencesControl.xaml
    /// </summary>
    public partial class SentencesControl : UserControl
    {
        private readonly SentenceRepository _sentenceRepository;

        public SentenceViewModel MySentence { get; set; }

        public long wordId { get; set; }

        public Func<Task> Refresh { get; set; }

        public SentencesControl()
        {
            InitializeComponent();
            _sentenceRepository = new SentenceRepository();
        }

        public void SetData(SentenceViewModel obj)
        {
            MySentence = obj;
            TbSentence.Text = obj.sentence;
        }

        private async void MenuItem_Edit_Click(object sender, RoutedEventArgs e)
        {
            UpdateSentence updateSentence = new UpdateSentence();
            updateSentence.SetData(MySentence);
            updateSentence.wordId = wordId;
            updateSentence.ShowDialog();
            await Refresh();
        }

        private async void MenuItem_Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Rostan ham o'chirishni hohlaysizmi?", "Information", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.Cancel, MessageBoxOptions.None);

            if (result == MessageBoxResult.OK)
            {
                var number = await _sentenceRepository.DeleteAsync(MySentence.Id);
                await Refresh();
            }
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
    }
}
