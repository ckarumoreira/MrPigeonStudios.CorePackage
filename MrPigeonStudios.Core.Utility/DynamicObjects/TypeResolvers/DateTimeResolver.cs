using System;
using System.Globalization;
using MrPigeonStudios.Core.Utility.DynamicProperties;

namespace MrPigeonStudios.Core.Utility.DynamicObjects.TypeResolvers {

    public class DateTimeResolver : ITypeResolver {
        private readonly string _format;
        private readonly IFormatProvider _formatProvider;

        public DateTimeResolver(string format) {
            if (string.IsNullOrEmpty(format))
                throw new ArgumentNullException(nameof(format));
            _formatProvider = CultureInfo.InvariantCulture;
            _format = format;
        }

        public DynamicProperty Resolve(string source) {
            if (DateTime.TryParseExact(source, _format, _formatProvider, DateTimeStyles.AdjustToUniversal, out var date)) {
                date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                return date;
            }

            return NullProperty.Instance;
        }

        public string Resolve(DynamicProperty source) {
            if (source.IsDateTime)
                return source.AsDateTime.ToString(_format, _formatProvider);
            return null;
        }
    }
}