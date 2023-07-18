using Neuron.Nexus.Models;

namespace Neuron.Nexus.ViewModels;
public class ChatTemplateSelector : DataTemplateSelector
{
    public DataTemplate User1Template { get; set; }
    public DataTemplate User2Template { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        var message = item as UserMessage;
        if (message == null)
            return null;

        return message.User == 1 ? User1Template : User2Template;
    }
}
