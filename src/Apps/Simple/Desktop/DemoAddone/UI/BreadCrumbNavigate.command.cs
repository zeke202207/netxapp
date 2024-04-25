using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia;
using DemoAddone.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DemoAddone.UI
{
    public partial class BreadCrumbNavigate
    {
        public BreadCrumbNavigate()
        {
            ItemClickCommandProperty.Changed.AddClassHandler<Interactive>(HandleCommandChanged);
        }

        /// <summary>
        /// 标识<seealso cref="CommandProperty"/> avalonia附加属性。
        /// </summary>
        /// <value>提供一个派生自<see cref="ICommand"/>的对象或绑定。</value>
        public static readonly AttachedProperty<ICommand> ItemClickCommandProperty = AvaloniaProperty.RegisterAttached<BreadCrumbNavigate, Interactive, ICommand>(
            "ItemClickCommand", default(ICommand), false, BindingMode.OneTime);

        /// <summary>
        /// 附加属性<see cref="CommandProperty"/>的访问器。
        /// </summary>
        public static void SetItemClickCommand(AvaloniaObject element, ICommand commandValue)
        {
            element.SetValue(ItemClickCommandProperty, commandValue);
        }

        /// <summary>
        /// 附加属性<see cref="CommandProperty"/>的访问器。
        /// </summary>
        public static ICommand GetItemClickCommand(AvaloniaObject element)
        {
            return element.GetValue(ItemClickCommandProperty);
        }

        /// <summary>
        /// https://docs.avaloniaui.net/zh-Hans/docs/guides/custom-controls/how-to-create-attached-properties
        /// </summary>
        /// <param name="interactElem"></param>
        /// <param name="args"></param>
        private void HandleCommandChanged(Interactive interactElem, AvaloniaPropertyChangedEventArgs args)
        {
            // 添加非空值
            if (args.NewValue is ICommand commandValue)
                interactElem.AddHandler(InputElement.TappedEvent, Handler);
            else// 删除之前的值
                interactElem.RemoveHandler(InputElement.TappedEvent, Handler);
            // 本地处理函数
            static void Handler(object s, RoutedEventArgs e)
            {
                if (s is Interactive interactElem)
                {
                    var repeatBtn = GetRepeat(e.Source as Control);
                    if (null == repeatBtn || null == repeatBtn.Tag)
                        return;
                    var catalogItem = repeatBtn.Tag as BreadCrumbItem;
                    ICommand commandValue = interactElem.GetValue(ItemClickCommandProperty);
                    commandValue?.Execute(catalogItem);
                }
            }
        }

        private static RepeatButton GetRepeat(Control ctl)
        {
            if (ctl == null)
                return null;
            if (ctl is RepeatButton btn)
                return btn;
            return GetRepeat(ctl.Parent as Control);
        }
    }
}
