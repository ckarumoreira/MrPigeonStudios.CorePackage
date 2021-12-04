using System;

namespace MrPigeonStudios.Core.Utility.DynamicProperties {

    public abstract class DynamicProperty {

        public enum DynamicPropertyType {
            Double,
            DateTime,
            String,
            Boolean,
            Null
        }

        public bool AsBoolean => ((BooleanProperty)this).Value;
        public DateTime AsDateTime => ((DateTimeProperty)this).Value;
        public double AsDouble => ((DoubleProperty)this).Value;

        public object AsObject => Type switch {
            DynamicPropertyType.DateTime => AsDateTime,
            DynamicPropertyType.Double => AsDouble,
            DynamicPropertyType.String => AsString,
            DynamicPropertyType.Boolean => AsBoolean,
            _ => null
        };

        public string AsString => ((StringProperty)this).Value;
        public bool IsBoolean => Type == DynamicPropertyType.Boolean;
        public bool IsDateTime => Type == DynamicPropertyType.DateTime;
        public bool IsDouble => Type == DynamicPropertyType.Double;
        public bool IsNull => Type == DynamicPropertyType.Null;
        public bool IsString => Type == DynamicPropertyType.String;
        public DynamicPropertyType Type { get; protected set; }

        public static DynamicProperty Create(object value) {
            return value switch {
                null => NullProperty.Instance,
                DynamicProperty dynamic => dynamic,
                double numberValue => new DoubleProperty(numberValue),
                float numberValue => new DoubleProperty(numberValue),
                int numberValue => new DoubleProperty(numberValue),
                long numberValue => new DoubleProperty(numberValue),
                short numberValue => new DoubleProperty(numberValue),
                byte numberValue => new DoubleProperty(numberValue),
                string stringValue => new StringProperty(stringValue),
                DateTime dateTimeValue => new DateTimeProperty(dateTimeValue),
                bool booleanValue => new BooleanProperty(booleanValue),
                _ => throw new InvalidOperationException()
            };
        }

        public static implicit operator bool(DynamicProperty prop) => prop.IsBoolean && prop.AsBoolean;

        public static implicit operator DateTime(DynamicProperty prop) => prop.IsDateTime ? prop.AsDateTime : default;

        public static implicit operator double(DynamicProperty prop) => prop.IsDouble ? prop.AsDouble : default;

        public static implicit operator DynamicProperty(string prop) => new StringProperty(prop);

        public static implicit operator DynamicProperty(bool prop) => new BooleanProperty(prop);

        public static implicit operator DynamicProperty(DateTime date) => new DateTimeProperty(date);

        public static implicit operator DynamicProperty(double prop) => new DoubleProperty(prop);

        public static implicit operator DynamicProperty(float prop) => new DoubleProperty(prop);

        public static implicit operator DynamicProperty(long prop) => new DoubleProperty(prop);

        public static implicit operator DynamicProperty(int prop) => new DoubleProperty(prop);

        public static implicit operator DynamicProperty(short prop) => new DoubleProperty(prop);

        public static implicit operator DynamicProperty(byte prop) => new DoubleProperty(prop);

        public static implicit operator string(DynamicProperty prop) => prop.IsString ? prop.AsString : default;
    }
}