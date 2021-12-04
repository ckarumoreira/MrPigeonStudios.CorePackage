using System;
using System.Linq.Expressions;
using MrPigeonStudios.Core.Utility.DynamicObjects.TypeResolvers;
using MrPigeonStudios.Core.Utility.DynamicProperties;

namespace MrPigeonStudios.Core.Utility.DynamicObjects {

    public sealed class DMappedProperty {
        public Func<DObject, DynamicProperty> GetValueExpression { get; private set; }
        public string PropertyName { get; private set; }
        public ITypeResolver Resolver { get; private set; }
        public Action<DObject, DynamicProperty> SetValueExpression { get; private set; }

        public static DMappedProperty Create(string propertyName, ITypeResolver typeResolver = null) {
            return new DMappedProperty() {
                PropertyName = propertyName,
                Resolver = typeResolver,
                GetValueExpression = null,
                SetValueExpression = null
            };
        }

        public void MapAccessors(Type ownerType) {
            GetValueExpression = CreateGetExpression(ownerType, PropertyName);
            SetValueExpression = CreateSetExpression(ownerType, PropertyName);
        }

        private static Func<DObject, DynamicProperty> CreateGetExpression(Type ownerType, string propertyName) {
            var ownerParam = Expression.Parameter(typeof(DObject));
            var ownerParsed = Expression.Convert(ownerParam, ownerType);
            var body = Expression.Property(ownerParsed, propertyName);
            return Expression.Lambda<Func<DObject, DynamicProperty>>(body, ownerParam).Compile();
        }

        private static Action<DObject, DynamicProperty> CreateSetExpression(Type ownerType, string propertyName) {
            var ownerParam = Expression.Parameter(typeof(DObject));
            var valueParam = Expression.Parameter(typeof(DynamicProperty));
            var ownerParsed = Expression.Convert(ownerParam, ownerType);
            var propExpression = Expression.Property(ownerParsed, propertyName);
            var body = Expression.Assign(propExpression, valueParam);
            return Expression.Lambda<Action<DObject, DynamicProperty>>(body, ownerParam, valueParam).Compile();
        }
    }
}