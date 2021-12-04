namespace MrPigeonStudios.Core.Utility.DynamicProperties {

    public sealed class NullProperty : DynamicProperty {
        private static NullProperty _instance;

        private NullProperty() {
            Type = DynamicPropertyType.Null;
        }

        public static DynamicProperty Instance {
            get {
                if (_instance == null) {
                    _instance = new NullProperty();
                }
                return _instance;
            }
        }
    }
}