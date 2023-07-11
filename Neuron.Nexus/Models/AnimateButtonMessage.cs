using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Neuron.Nexus.Models
{
    public enum AnimationButtonsEnum
    {
        StopBtn,
        LanguageOneBtn,
        LanguageTwoBtn
    }

    public class AnimateButtonMessage : ValueChangedMessage<AnimationButtonsEnum>
    {
        public AnimationButtonsEnum ButtonName { get; private set; }
        public AnimateButtonMessage(AnimationButtonsEnum buttonName) : base(buttonName)
        {
            ButtonName = buttonName;
        }
    }
}
