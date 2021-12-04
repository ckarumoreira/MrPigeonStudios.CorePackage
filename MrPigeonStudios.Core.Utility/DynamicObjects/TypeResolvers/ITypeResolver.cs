using MrPigeonStudios.Core.Utility.DynamicProperties;

namespace MrPigeonStudios.Core.Utility.DynamicObjects.TypeResolvers {

    public interface ITypeResolver {

        DynamicProperty Resolve(string source);

        string Resolve(DynamicProperty source);
    }
}