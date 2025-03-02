using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Watchdog.Models;

namespace Watchdog.Converters;

public class TaskTypeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is HttpWatchdogTask)
        {
            return "HTTP";
        }
        else if (value is UdpWatchdogTask)
        {
            return "UDP";
        }
        else
        {
            return "Unknown";
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class TaskDetailsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is WatchdogTask task)
        {
            return task.GetDetails();
        }
        else
        {
            return "No Details Available";
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class InverseBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return !boolValue;
        }
        return DependencyProperty.UnsetValue; // Retorna UnsetValue se o valor não for um booleano
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException(); // Ou implemente a conversão de volta, se necessário
    }
}