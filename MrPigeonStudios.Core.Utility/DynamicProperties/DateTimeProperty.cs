using System;

namespace MrPigeonStudios.Core.Utility.DynamicProperties {
    public sealed class DateTimeProperty : DynamicProperty {
        private readonly DateTime _value;

        public DateTimeProperty(DateTime value) {
            _value = value;
            Type = DynamicPropertyType.DateTime;
        }

        public DateTime Value => _value;
    }
}