using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Projet_Session_Entreprise.Converters
{
    public class AppointmentStatusChangeConverter : IMultiValueConverter
    {
        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 || values[0] is not Appointment appointment || values[1] is not string status)
            {
                return null;
            }

            return new AppointmentStatusChange(appointment, status);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
