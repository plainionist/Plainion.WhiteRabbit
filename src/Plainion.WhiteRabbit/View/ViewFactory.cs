using Plainion.WhiteRabbit.Presentation;

namespace Plainion.WhiteRabbit.View
{
    internal class ViewFactory
    {
        internal static IView CreateMainView(Controller controller) => new MainUI(controller);

        internal static IView CreateTimerView(Controller controller) => new SlimForm(controller);
    }
}
