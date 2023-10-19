using CommunityToolkit.Mvvm.Input;
using Markdig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuron.Nexus.ViewModels
{
    [QueryProperty(nameof(SpokenText), "spokenText")]
    public partial class AiNotesSummaryViewModel : BaseViewModel
    {
        public string SpokenText
        {
            set
            {
                RawMarkdownText = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(Uri.UnescapeDataString(value));
            }
        }

        private string _rawMarkdownText;
        public string RawMarkdownText
        {
            get => _rawMarkdownText;
            set
            {
                SetProperty(ref _rawMarkdownText, value);
                UpdateMarkdownPreview();
            }
        }

        private string _markdownPreview;
        public string MarkdownPreview
        {
            get => _markdownPreview;
            set => SetProperty(ref _markdownPreview, value);
        }

        private bool _isHorizontal = true;
        public bool IsHorizontal
        {
            get => _isHorizontal;
            set => SetProperty(ref _isHorizontal, value);
        }
        public bool IsVertical => !IsHorizontal;

        public AiNotesSummaryViewModel() { }

        [RelayCommand]
        private void ToggleOrientation()
        {
            IsHorizontal = !IsHorizontal;
            OnPropertyChanged(nameof(IsVertical));
        }

        [RelayCommand]
        private void CopyToClipboard()
        {
            Clipboard.SetTextAsync(RawMarkdownText);
        }

        private void UpdateMarkdownPreview()
        {
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            MarkdownPreview = Markdown.ToHtml(RawMarkdownText ?? string.Empty, pipeline);
        }
    }
}
