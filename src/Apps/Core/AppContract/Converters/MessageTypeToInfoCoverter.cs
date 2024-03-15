using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppCore.Contract.Converters
{
    internal class MessageTypeToInfoCoverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DialogMessageType messageType)
            {
                switch (messageType)
                {
                    case DialogMessageType.Info:
                        return "Info";
                    case DialogMessageType.Warning:
                        return "Warning";
                    case DialogMessageType.Error:
                        return "Error";
                    case DialogMessageType.Success:
                        return "Success";
                    default:
                        return "Info";
                }
            }
            return "Info";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
