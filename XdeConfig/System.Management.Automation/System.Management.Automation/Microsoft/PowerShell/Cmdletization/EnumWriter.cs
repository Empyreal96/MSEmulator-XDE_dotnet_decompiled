using System;
using System.Globalization;
using System.Management.Automation;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.PowerShell.Cmdletization.Xml;

namespace Microsoft.PowerShell.Cmdletization
{
	// Token: 0x020009A5 RID: 2469
	internal static class EnumWriter
	{
		// Token: 0x06005AEF RID: 23279 RVA: 0x001E8C38 File Offset: 0x001E6E38
		private static ModuleBuilder CreateModuleBuilder()
		{
			AssemblyName assemblyName = new AssemblyName("Microsoft.PowerShell.Cmdletization.GeneratedTypes");
			AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
			return assemblyBuilder.DefineDynamicModule(assemblyName.Name);
		}

		// Token: 0x06005AF0 RID: 23280 RVA: 0x001E8C66 File Offset: 0x001E6E66
		internal static string GetEnumFullName(EnumMetadataEnum enumMetadata)
		{
			return "Microsoft.PowerShell.Cmdletization.GeneratedTypes." + enumMetadata.EnumName;
		}

		// Token: 0x06005AF1 RID: 23281 RVA: 0x001E8C78 File Offset: 0x001E6E78
		internal static void Compile(EnumMetadataEnum enumMetadata)
		{
			string enumFullName = EnumWriter.GetEnumFullName(enumMetadata);
			Type type;
			if (enumMetadata.UnderlyingType != null)
			{
				type = (Type)LanguagePrimitives.ConvertTo(enumMetadata.UnderlyingType, typeof(Type), CultureInfo.InvariantCulture);
			}
			else
			{
				type = typeof(int);
			}
			ModuleBuilder value = EnumWriter._moduleBuilder.Value;
			EnumBuilder enumBuilder;
			lock (EnumWriter._moduleBuilderUsageLock)
			{
				enumBuilder = value.DefineEnum(enumFullName, TypeAttributes.Public, type);
			}
			if (enumMetadata.BitwiseFlagsSpecified && enumMetadata.BitwiseFlags)
			{
				CustomAttributeBuilder customAttribute = new CustomAttributeBuilder(typeof(FlagsAttribute).GetConstructor(PSTypeExtensions.EmptyTypes), new object[0]);
				enumBuilder.SetCustomAttribute(customAttribute);
			}
			foreach (EnumMetadataEnumValue enumMetadataEnumValue in enumMetadata.Value)
			{
				string name = enumMetadataEnumValue.Name;
				object literalValue = LanguagePrimitives.ConvertTo(enumMetadataEnumValue.Value, type, CultureInfo.InvariantCulture);
				enumBuilder.DefineLiteral(name, literalValue);
			}
			ClrFacade.CreateEnumType(enumBuilder);
		}

		// Token: 0x040030B0 RID: 12464
		private const string namespacePrefix = "Microsoft.PowerShell.Cmdletization.GeneratedTypes";

		// Token: 0x040030B1 RID: 12465
		private static Lazy<ModuleBuilder> _moduleBuilder = new Lazy<ModuleBuilder>(new Func<ModuleBuilder>(EnumWriter.CreateModuleBuilder), true);

		// Token: 0x040030B2 RID: 12466
		private static object _moduleBuilderUsageLock = new object();
	}
}
