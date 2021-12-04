namespace MrPigeonStudios.Core.Utility.DynamicProperties {
    public sealed class StringProperty : DynamicProperty {
        private readonly string _value;

        public StringProperty(string value) {
            _value = string.Intern(value);
            Type = DynamicPropertyType.String;
        }

        public string Value => _value;
    }
}