using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MrPigeonStudios.Core.Utility.DynamicObjects.Comparers {

    public class DObjectEqualityComparer : IEqualityComparer<DObject> {
        private readonly string[] _properties;

        public DObjectEqualityComparer(params string[] properties) {
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public bool Equals([AllowNull] DObject x, [AllowNull] DObject y) {
            if (x == null && y == null) {
                return true;
            }

            if (x == null || y == null) {
                return false;
            }

            foreach (var property in _properties) {
                var xProp = x.Get(property);
                var yProp = y.Get(property);

                if (xProp.Type != yProp.Type || xProp.AsObject != yProp.AsObject) {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode([DisallowNull] DObject obj) {
            unchecked {
                var hash = 17;

                foreach (var property in _properties) {
                    var prop = obj.Get(property);
                    hash *= 23 + prop.AsObject?.GetHashCode() ?? 0;
                }

                return hash;
            }
        }
    }
}