using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for IncorrectControl.xaml
    /// </summary>
    public partial class IncorrectControl : UserControl
    {
        public IncorrectControl()
        {
            InitializeComponent();
        }

        public void SetData(string text)
        {
            TextLb.Content = text;
        }
    }
}
