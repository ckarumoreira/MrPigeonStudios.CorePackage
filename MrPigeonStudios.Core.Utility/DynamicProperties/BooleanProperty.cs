namespace MrPigeonStudios.Core.Utility.DynamicProperties {

    public sealed class BooleanProperty : DynamicProperty {
        private readonly bool _value;

        public BooleanProperty(bool value) {
            _value = value;
            Type = DynamicPropertyType.Boolean;
        }

        public bool Value => _value;
    }
}