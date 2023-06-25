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

namespace WordGenius.Desktop.Components.Test
{
    /// <summary>
    /// Interaction logic for Answer.xaml
    /// </summary>
    public partial class Answer : UserControl
    {
        public Answer()
        {
            InitializeComponent();
        }

        public void setdata(string text)
        {
            wordLb.Text = text;
        }
    }
}
