using Avalonia.Data.Converters;
using Material.Icons;
using Material.Icons.Avalonia;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppCore.Contract.Converters
{
    public class MessageTypeToIconCoverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DialogMessageType messageType)
            {
                switch (messageType)
                {
                    case DialogMessageType.Info:
                        return MaterialIconKind.InfoCircleOutline;
                    case DialogMessageType.Warning:
                        return MaterialIconKind.WarningBoxOutline;
                    case DialogMessageType.Error:
                        return MaterialIconKind.ErrorOutline;
                    case DialogMessageType.Success:
                        return MaterialIconKind.SuccessCircleOutline;
                    default:
                        return MaterialIconKind.InfoCircleOutline;
                }
            }
            return MaterialIconKind.Information;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
