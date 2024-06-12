using ProyectoU3.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU3.Helpers
{
    public class DepartamentoToIdConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var dep = value as DepartamentosDTO;
            return dep != null ? dep.Id : 0;
        }
    }
}
