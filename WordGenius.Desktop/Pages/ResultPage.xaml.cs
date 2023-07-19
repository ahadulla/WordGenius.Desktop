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
using WordGenius.Desktop.Components.Test;
using WordGenius.Desktop.Entities.Test;

namespace WordGenius.Desktop.Pages;

/// <summary>
/// Interaction logic for ResultPage.xaml
/// </summary>
public partial class ResultPage : Page
{
    public ResultPage()
    {
        InitializeComponent();
    }

    public void SetData(string correct, string incorrect, List<Test> tests, List<Test> Cortests)
    {
        Yashilchiroq.Text = correct;
        Qizilchiroq.Text = incorrect;

        foreach (Test test in Cortests)
        {
            string text = test.word.Text + " - " + test.word.Translate;
            IncorrectControl incorrectControl = new IncorrectControl();
            incorrectControl.border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#03774C"));
            incorrectControl.SetData(text);
            wrpTests.Children.Add(incorrectControl);
        }

        foreach (Test test in tests)
        {
            string text = test.word.Text + " - " + test.word.Translate;
            IncorrectControl incorrectControl = new IncorrectControl();
            incorrectControl.SetData(text);
            wrpTests.Children.Add(incorrectControl);
        }
    }

}

