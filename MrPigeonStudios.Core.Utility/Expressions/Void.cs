namespace MrPigeonStudios.Core.Utility.Expressions {

    /// <summary>
    /// Source for expression with no inputs.
    /// </summary>
    public class Void {
        private static Void _instance;

        private Void() {
        }

        public static Void Instance {
            get {
                if (_instance == null) {
                    _instance = new Void();
                }
                return _instance;
            }
        }
    }
}