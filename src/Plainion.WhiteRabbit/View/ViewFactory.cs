namespace Plainion.WhiteRabbit.View
{
    internal class ViewFactory
    {
        internal static IView CreateMainView(IController controller) => new MainUI(controller);

        internal static IView CreateTimerView(IController controller) => new SlimForm(controller);
    }
}
