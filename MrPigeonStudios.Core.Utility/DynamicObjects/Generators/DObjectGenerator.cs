using System;
using System.Reflection;
using System.Reflection.Emit;
using MrPigeonStudios.Core.Utility.DynamicProperties;

namespace MrPigeonStudios.Core.Utility.DynamicObjects.Generators {

    public class DObjectGenerator {

        public static DObject CreateInstance(Type type, DMetadata metadata) {
            return (DObject)Activator.CreateInstance(type, metadata);
        }

        public static Type GenerateType(DMetadata metadata) {
            AssemblyName aName = new AssemblyName("MrPigeonStudios.Runtime.DynamicObjects");
            AssemblyBuilder ab = AssemblyBuilder.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);
            ModuleBuilder mb = ab.DefineDynamicModule(aName.Name);
            Type baseType = typeof(DObject);

            MethodInfo attachMetadata = baseType.GetMethod("AttachMetadata", BindingFlags.Public | BindingFlags.Instance);

            TypeBuilder tb = mb.DefineType($"DObject_{metadata.Id}", TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit, baseType);
            PropertyBuilder propertyBuilder;
            FieldBuilder backingFieldBuilder;
            ILGenerator il;

            Type propertyType = typeof(DynamicProperty);
            MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            var ctorBuilder = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { typeof(DMetadata) });
            il = ctorBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.EmitCall(OpCodes.Callvirt, attachMetadata, null);
            il.Emit(OpCodes.Ret);

            foreach (var property in metadata.MappedProperties) {
                backingFieldBuilder = tb.DefineField($"f{property.PropertyName}", propertyType, FieldAttributes.Private);
                propertyBuilder = tb.DefineProperty(property.PropertyName, PropertyAttributes.None, propertyType, null);

                var getter = tb.DefineMethod($"get_{property.PropertyName}", getSetAttr, propertyType, Type.EmptyTypes);
                il = getter.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, backingFieldBuilder);
                il.Emit(OpCodes.Ret);
                propertyBuilder.SetGetMethod(getter);

                var setter = tb.DefineMethod($"set_{property.PropertyName}", getSetAttr, null, new[] { propertyType });
                il = setter.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Stfld, backingFieldBuilder);
                il.Emit(OpCodes.Ret);
                propertyBuilder.SetSetMethod(setter);
            }

            // Finish the type.
            Type createdType = tb.CreateType();

            foreach (var property in metadata.MappedProperties) {
                property.MapAccessors(createdType);
            }

            return createdType;
        }
    }
}