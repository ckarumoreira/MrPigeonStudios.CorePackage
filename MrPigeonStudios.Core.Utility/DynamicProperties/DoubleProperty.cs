namespace MrPigeonStudios.Core.Utility.DynamicProperties {
    public sealed class DoubleProperty : DynamicProperty {
        private readonly double _value;

        public DoubleProperty(double value) {
            _value = value;
            Type = DynamicPropertyType.Double;
        }

        public double Value => _value;
    }
}