using System.Linq;
using MrPigeonStudios.Core.Utility.DynamicProperties;

namespace MrPigeonStudios.Core.Utility.DynamicObjects {

    public abstract class DObject {
        protected DMetadata _metadata;

        public virtual void AttachMetadata(DMetadata metadata) {
            _metadata = metadata;
        }

        public DynamicProperty Get(string propertyName) {
            return _metadata.Get(this, propertyName);
        }

        public void Set(string propertyName, object value) {
            _metadata.Set(this, propertyName, value);
        }
    }
}