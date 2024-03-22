using Avalonia.Controls;

namespace NetX.AppCore.Contract
{
    public interface IControlCreator
    {
        Control? CreateControl(Type controlType , bool keepalive = false);
    }
}
