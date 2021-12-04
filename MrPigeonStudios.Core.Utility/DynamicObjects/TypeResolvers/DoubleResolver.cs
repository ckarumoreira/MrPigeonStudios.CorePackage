using System;
using System.Globalization;
using MrPigeonStudios.Core.Utility.DynamicProperties;

namespace MrPigeonStudios.Core.Utility.DynamicObjects.TypeResolvers {

    public class DoubleResolver : ITypeResolver {
        private readonly string _decimalSeparator;
        private readonly string _format;
        private readonly IFormatProvider _formatProvider;
        private readonly string _thousandSeparator;

        public DoubleResolver(string format, string decimalSeparator, string thousandSeparator) {
            if (string.IsNullOrEmpty(format))
                throw new ArgumentNullException(nameof(format));
            _format = format;

            if (string.IsNullOrEmpty(decimalSeparator))
                throw new ArgumentNullException(nameof(decimalSeparator));
            _decimalSeparator = decimalSeparator;

            if (string.IsNullOrEmpty(thousandSeparator))
                throw new ArgumentNullException(nameof(thousandSeparator));
            _thousandSeparator = thousandSeparator;

            var culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            culture.NumberFormat.NumberDecimalSeparator = _decimalSeparator;
            culture.NumberFormat.NumberGroupSeparator = _thousandSeparator;
            culture.NumberFormat.CurrencyDecimalSeparator = _decimalSeparator;
            culture.NumberFormat.CurrencyGroupSeparator = _thousandSeparator;

            _formatProvider = culture;
        }

        public DynamicProperty Resolve(string source) {
            if (double.TryParse(source, NumberStyles.Number, _formatProvider, out var number)) {
                return number;
            }

            return NullProperty.Instance;
        }

        public string Resolve(DynamicProperty source) {
            if (source.IsDouble)
                return source.AsDouble.ToString(_format, _formatProvider);
            return null;
        }
    }
}