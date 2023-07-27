namespace Neuron.Nexus.Behaviors
{
    public class FrameTapScaleAnimationBehavior : Behavior<Frame>
    {
        protected override void OnAttachedTo(Frame bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    await AnimateFrameAsync(bindable);
                })
            });
        }

        private async Task AnimateFrameAsync(Frame frame)
        {
            await Task.WhenAll(
              frame.ScaleTo(1.1, 100),
              frame.RotateTo(15, 100),
              frame.RotateTo(-15, 100),
              frame.RotateTo(0, 100)
          );

            await frame.ScaleTo(1.0, 100);
        }
    }
}
