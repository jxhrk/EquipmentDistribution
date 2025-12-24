using System;
using System.Collections;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace EquipmentDistribution.Converters;

public class IndexerConverter : IValueConverter
{
    IDictionary dictionary;
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IDictionary dict && parameter is string index)
        {
            dictionary = dict;
            return dict[index];
        }
        
        // converter used for the wrong type
        return new BindingNotification(new InvalidCastException(), 
            BindingErrorType.Error);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string str && int.TryParse(str, out int parsed))
            dictionary[parameter] = parsed;
        return dictionary;
    }
}