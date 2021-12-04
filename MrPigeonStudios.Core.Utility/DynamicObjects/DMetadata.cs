using System;
using System.Collections.Generic;
using System.Linq;
using MrPigeonStudios.Core.Utility.DynamicProperties;

namespace MrPigeonStudios.Core.Utility.DynamicObjects {

    public sealed class DMetadata {
        public Guid Id { get; set; }

        public IEnumerable<DMappedProperty> MappedProperties {
            get { return Properties; }
            set {
                Properties = value.ToArray();
                PropertyNames = value.Select(x => x.PropertyName).ToArray();
            }
        }

        private DMappedProperty[] Properties { get; set; }
        private string[] PropertyNames { get; set; }

        public DynamicProperty Get(DObject instance, string propertyName) {
            var index = Array.IndexOf(PropertyNames, propertyName);
            if (index < 0)
                return null;
            return Properties[index].GetValueExpression(instance);
        }

        public void Set(DObject instance, string propertyName, object value) {
            var index = Array.IndexOf(PropertyNames, propertyName);
            if (index < 0)
                return;
            Properties[index].SetValueExpression(instance, DynamicProperty.Create(value));
        }
    }
}