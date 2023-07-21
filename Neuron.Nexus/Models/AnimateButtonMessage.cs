using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Neuron.Nexus.Models
{
    public enum ButtonsEnum
    {
        StopBtn,
        LanguageOneBtn,
        LanguageTwoBtn
    }

    public class AnimateButtonMessage : ValueChangedMessage<ButtonsEnum>
    {
        public ButtonsEnum ButtonName { get; private set; }
        public AnimateButtonMessage(ButtonsEnum buttonName) : base(buttonName)
        {
            ButtonName = buttonName;
        }
    }
}
