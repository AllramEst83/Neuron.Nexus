using Neuron.Nexus.ViewModels;
using System.ComponentModel;

namespace Neuron.Nexus.Pages;

public partial class AiNotesSummaryPage : ContentPage
{
    private AiNotesSummaryViewModel viewModel;

    public AiNotesSummaryPage()
    {
        InitializeComponent();
        viewModel = new AiNotesSummaryViewModel();
        BindingContext = viewModel;
        viewModel.PropertyChanged += ViewModel_PropertyChanged;
    }

    private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(viewModel.IsHorizontal))
        {
            UpdateLayoutOrientation();
        }
    }

    private void UpdateLayoutOrientation()
    {
        var grid = (Grid)Content;
        var splitView = (Grid)grid.Children[0];

        splitView.ColumnDefinitions.Clear();
        splitView.RowDefinitions.Clear();

        if (viewModel.IsHorizontal)
        {
            splitView.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            splitView.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        }
        else
        {
            splitView.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            splitView.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        }

        //RawEditor.SetBinding(Editor.TextProperty, new Binding("RawMarkdownText", source: viewModel));
    }


}