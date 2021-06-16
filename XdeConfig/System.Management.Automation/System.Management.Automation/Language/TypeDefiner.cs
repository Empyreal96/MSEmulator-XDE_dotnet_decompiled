using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Microsoft.PowerShell;
using Microsoft.PowerShell.Telemetry.Internal;

namespace System.Management.Automation.Language
{
	// Token: 0x02000643 RID: 1603
	internal class TypeDefiner
	{
		// Token: 0x06004567 RID: 17767 RVA: 0x00173DCC File Offset: 0x00171FCC
		private static bool TryConvertArg(object arg, Type type, out object result, Parser parser, IScriptExtent errorExtent)
		{
			if (arg != null && arg.GetType() == type)
			{
				result = arg;
				return true;
			}
			if (!LanguagePrimitives.TryConvertTo(arg, type, out result))
			{
				parser.ReportError(errorExtent, () => ParserStrings.CannotConvertValue, ToStringCodeMethods.Type(type, false));
				return false;
			}
			return true;
		}

		// Token: 0x06004568 RID: 17768 RVA: 0x00173E2C File Offset: 0x0017202C
		private static CustomAttributeBuilder GetAttributeBuilder(Parser parser, AttributeAst attributeAst, AttributeTargets attributeTargets)
		{
			Type reflectionAttributeType = attributeAst.TypeName.GetReflectionAttributeType();
			object[] array = new object[attributeAst.PositionalArguments.Count];
			ConstantValueVisitor visitor = new ConstantValueVisitor
			{
				AttributeArgument = false
			};
			for (int i = 0; i < attributeAst.PositionalArguments.Count; i++)
			{
				ExpressionAst expressionAst = attributeAst.PositionalArguments[i];
				array[i] = expressionAst.Accept(visitor);
			}
			ConstructorInfo[] constructors = reflectionAttributeType.GetConstructors();
			MethodInformation[] methodInformationArray = DotNetAdapter.GetMethodInformationArray(constructors);
			string errorId = null;
			string format = null;
			int num = array.Length;
			bool flag;
			bool flag2;
			MethodInformation methodInformation = Adapter.FindBestMethod(methodInformationArray, null, array, ref errorId, ref format, out flag, out flag2);
			if (methodInformation == null)
			{
				parser.ReportError(new ParseError(attributeAst.Extent, errorId, string.Format(CultureInfo.InvariantCulture, format, new object[]
				{
					reflectionAttributeType.Name,
					attributeAst.PositionalArguments.Count
				})));
				return null;
			}
			ConstructorInfo constructorInfo = (ConstructorInfo)methodInformation.method;
			ParameterInfo[] parameters = constructorInfo.GetParameters();
			object[] array2 = new object[parameters.Length];
			for (int j = 0; j < parameters.Length; j++)
			{
				Type parameterType = parameters[j].ParameterType;
				object[] customAttributes = parameters[j].GetCustomAttributes(typeof(ParamArrayAttribute), true);
				object obj;
				if (customAttributes != null && customAttributes.Any<object>() && flag)
				{
					Type elementType = parameters[j].ParameterType.GetElementType();
					Array array3 = Array.CreateInstance(elementType, num - j);
					array2[j] = array3;
					int k = 0;
					while (k < array3.Length)
					{
						if (!TypeDefiner.TryConvertArg(array[j], elementType, out obj, parser, attributeAst.PositionalArguments[j].Extent))
						{
							return null;
						}
						array3.SetValue(obj, k);
						k++;
						j++;
					}
					break;
				}
				if (!TypeDefiner.TryConvertArg(array[j], parameterType, out obj, parser, attributeAst.PositionalArguments[j].Extent))
				{
					return null;
				}
				array2[j] = obj;
			}
			if (attributeAst.NamedArguments.Count == 0)
			{
				return new CustomAttributeBuilder(constructorInfo, array2);
			}
			List<PropertyInfo> list = new List<PropertyInfo>();
			List<object> list2 = new List<object>();
			List<FieldInfo> list3 = new List<FieldInfo>();
			List<object> list4 = new List<object>();
			foreach (NamedAttributeArgumentAst namedAttributeArgumentAst in attributeAst.NamedArguments)
			{
				string argumentName = namedAttributeArgumentAst.ArgumentName;
				MemberInfo[] member = reflectionAttributeType.GetMember(argumentName, MemberTypes.Field | MemberTypes.Property, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
				object obj = namedAttributeArgumentAst.Argument.Accept(visitor);
				PropertyInfo propertyInfo = member[0] as PropertyInfo;
				if (propertyInfo != null)
				{
					if (!TypeDefiner.TryConvertArg(obj, propertyInfo.PropertyType, out obj, parser, namedAttributeArgumentAst.Argument.Extent))
					{
						return null;
					}
					list.Add(propertyInfo);
					list2.Add(obj);
				}
				else
				{
					FieldInfo fieldInfo = (FieldInfo)member[0];
					if (!TypeDefiner.TryConvertArg(obj, fieldInfo.FieldType, out obj, parser, namedAttributeArgumentAst.Argument.Extent))
					{
						return null;
					}
					list3.Add(fieldInfo);
					list4.Add(obj);
				}
			}
			return new CustomAttributeBuilder(constructorInfo, array2, list.ToArray(), list2.ToArray(), list3.ToArray(), list4.ToArray());
		}

		// Token: 0x06004569 RID: 17769 RVA: 0x0017417C File Offset: 0x0017237C
		internal static void DefineCustomAttributes(TypeBuilder member, ReadOnlyCollection<AttributeAst> attributes, Parser parser, AttributeTargets attributeTargets)
		{
			if (attributes != null)
			{
				foreach (AttributeAst attributeAst in attributes)
				{
					CustomAttributeBuilder attributeBuilder = TypeDefiner.GetAttributeBuilder(parser, attributeAst, attributeTargets);
					if (attributeBuilder != null)
					{
						member.SetCustomAttribute(attributeBuilder);
					}
				}
			}
		}

		// Token: 0x0600456A RID: 17770 RVA: 0x001741D4 File Offset: 0x001723D4
		internal static void DefineCustomAttributes(PropertyBuilder member, ReadOnlyCollection<AttributeAst> attributes, Parser parser, AttributeTargets attributeTargets)
		{
			if (attributes != null)
			{
				foreach (AttributeAst attributeAst in attributes)
				{
					CustomAttributeBuilder attributeBuilder = TypeDefiner.GetAttributeBuilder(parser, attributeAst, attributeTargets);
					if (attributeBuilder != null)
					{
						member.SetCustomAttribute(attributeBuilder);
					}
				}
			}
		}

		// Token: 0x0600456B RID: 17771 RVA: 0x0017422C File Offset: 0x0017242C
		internal static void DefineCustomAttributes(ConstructorBuilder member, ReadOnlyCollection<AttributeAst> attributes, Parser parser, AttributeTargets attributeTargets)
		{
			if (attributes != null)
			{
				foreach (AttributeAst attributeAst in attributes)
				{
					CustomAttributeBuilder attributeBuilder = TypeDefiner.GetAttributeBuilder(parser, attributeAst, attributeTargets);
					if (attributeBuilder != null)
					{
						member.SetCustomAttribute(attributeBuilder);
					}
				}
			}
		}

		// Token: 0x0600456C RID: 17772 RVA: 0x00174284 File Offset: 0x00172484
		internal static void DefineCustomAttributes(MethodBuilder member, ReadOnlyCollection<AttributeAst> attributes, Parser parser, AttributeTargets attributeTargets)
		{
			if (attributes != null)
			{
				foreach (AttributeAst attributeAst in attributes)
				{
					CustomAttributeBuilder attributeBuilder = TypeDefiner.GetAttributeBuilder(parser, attributeAst, attributeTargets);
					if (attributeBuilder != null)
					{
						member.SetCustomAttribute(attributeBuilder);
					}
				}
			}
		}

		// Token: 0x0600456D RID: 17773 RVA: 0x001742DC File Offset: 0x001724DC
		internal static void DefineCustomAttributes(EnumBuilder member, ReadOnlyCollection<AttributeAst> attributes, Parser parser, AttributeTargets attributeTargets)
		{
			if (attributes != null)
			{
				foreach (AttributeAst attributeAst in attributes)
				{
					CustomAttributeBuilder attributeBuilder = TypeDefiner.GetAttributeBuilder(parser, attributeAst, attributeTargets);
					if (attributeBuilder != null)
					{
						member.SetCustomAttribute(attributeBuilder);
					}
				}
			}
		}

		// Token: 0x0600456E RID: 17774 RVA: 0x00174408 File Offset: 0x00172608
		private static IEnumerable<CustomAttributeBuilder> GetAssemblyAttributeBuilders()
		{
			yield return new CustomAttributeBuilder(typeof(DynamicClassImplementationAssemblyAttribute).GetConstructor(Type.EmptyTypes), TypeDefiner.emptyArgArray);
			yield break;
		}

		// Token: 0x0600456F RID: 17775 RVA: 0x00174420 File Offset: 0x00172620
		internal static Assembly DefineTypes(Parser parser, Ast rootAst, TypeDefinitionAst[] typeDefinitions)
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			string text = string.IsNullOrWhiteSpace(rootAst.Extent.File) ? "powershell" : rootAst.Extent.File.Replace('\\', '⧹').Replace(':', '։');
			AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(text), AssemblyBuilderAccess.RunAndCollect, TypeDefiner.GetAssemblyAttributeBuilders());
			ModuleBuilder module = assemblyBuilder.DefineDynamicModule(text);
			List<TypeDefiner.DefineTypeHelper> list = new List<TypeDefiner.DefineTypeHelper>();
			List<TypeDefiner.DefineEnumHelper> list2 = new List<TypeDefiner.DefineEnumHelper>();
			foreach (TypeDefinitionAst typeDefinitionAst in typeDefinitions)
			{
				string classNameInAssembly = TypeDefiner.GetClassNameInAssembly(typeDefinitionAst);
				if (!hashSet.Contains(classNameInAssembly))
				{
					hashSet.Add(classNameInAssembly);
					if ((typeDefinitionAst.TypeAttributes & TypeAttributes.Class) == TypeAttributes.Class)
					{
						list.Add(new TypeDefiner.DefineTypeHelper(parser, module, typeDefinitionAst, classNameInAssembly));
						TelemetryAPI.TraceDefinedPowerShellType(typeDefinitionAst);
					}
					else if ((typeDefinitionAst.TypeAttributes & TypeAttributes.Enum) == TypeAttributes.Enum)
					{
						list2.Add(new TypeDefiner.DefineEnumHelper(parser, module, typeDefinitionAst, classNameInAssembly));
					}
				}
			}
			list2 = TypeDefiner.DefineEnumHelper.Sort(list2, parser);
			foreach (TypeDefiner.DefineEnumHelper defineEnumHelper in list2)
			{
				defineEnumHelper.DefineEnum();
			}
			foreach (TypeDefiner.DefineTypeHelper defineTypeHelper in list)
			{
				defineTypeHelper.DefineMembers();
			}
			foreach (TypeDefiner.DefineTypeHelper defineTypeHelper2 in list)
			{
				bool flag = false;
				if (!defineTypeHelper2.HasFatalErrors)
				{
					try
					{
						Type type = defineTypeHelper2._typeBuilder.CreateTypeInfo().AsType();
						defineTypeHelper2._typeDefinitionAst.Type = type;
						flag = true;
						Type type2 = defineTypeHelper2._staticHelpersTypeBuilder.CreateTypeInfo().AsType();
						if (defineTypeHelper2._fieldsToInitForMemberFunctions != null)
						{
							foreach (Tuple<string, object> tuple in defineTypeHelper2._fieldsToInitForMemberFunctions)
							{
								type2.GetField(tuple.Item1, BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, tuple.Item2);
							}
						}
					}
					catch (TypeLoadException ex)
					{
						parser.ReportError(defineTypeHelper2._typeDefinitionAst.Extent, () => ParserStrings.TypeCreationError, defineTypeHelper2._typeBuilder.Name, ex.Message);
					}
				}
				if (!flag)
				{
					defineTypeHelper2._typeDefinitionAst.Type = null;
				}
			}
			return assemblyBuilder;
		}

		// Token: 0x06004570 RID: 17776 RVA: 0x00174738 File Offset: 0x00172938
		private static string GetClassNameInAssembly(TypeDefinitionAst typeDefinitionAst)
		{
			List<string> list = null;
			Ast ast = typeDefinitionAst.Parent;
			while (ast.Parent != null)
			{
				if (ast is IParameterMetadataProvider)
				{
					list = (list ?? new List<string>());
					FunctionDefinitionAst functionDefinitionAst = ast.Parent as FunctionDefinitionAst;
					if (functionDefinitionAst != null)
					{
						ast = functionDefinitionAst;
						list.Add(functionDefinitionAst.Name);
					}
					else
					{
						list.Add("<" + ast.Extent.Text.GetHashCode().ToString("x", CultureInfo.InvariantCulture) + ">");
					}
				}
				ast = ast.Parent;
			}
			if (list == null)
			{
				return typeDefinitionAst.Name;
			}
			list.Reverse();
			list.Add(typeDefinitionAst.Name);
			return string.Join(".", list);
		}

		// Token: 0x06004571 RID: 17777 RVA: 0x001747EF File Offset: 0x001729EF
		private static void EmitLdc(ILGenerator emitter, int c)
		{
			if (c < TypeDefiner._ldc.Length)
			{
				emitter.Emit(TypeDefiner._ldc[c]);
				return;
			}
			emitter.Emit(OpCodes.Ldc_I4, c);
		}

		// Token: 0x06004572 RID: 17778 RVA: 0x0017481E File Offset: 0x00172A1E
		private static void EmitLdarg(ILGenerator emitter, int c)
		{
			if (c < TypeDefiner._ldarg.Length)
			{
				emitter.Emit(TypeDefiner._ldarg[c]);
				return;
			}
			emitter.Emit(OpCodes.Ldarg, c);
		}

		// Token: 0x0400222C RID: 8748
		private static int globalCounter = 0;

		// Token: 0x0400222D RID: 8749
		private static readonly object[] emptyArgArray = new object[0];

		// Token: 0x0400222E RID: 8750
		private static readonly CustomAttributeBuilder hiddenCustomAttributeBuilder = new CustomAttributeBuilder(typeof(HiddenAttribute).GetConstructor(Type.EmptyTypes), TypeDefiner.emptyArgArray);

		// Token: 0x0400222F RID: 8751
		private static OpCode[] _ldc = new OpCode[]
		{
			OpCodes.Ldc_I4_0,
			OpCodes.Ldc_I4_1,
			OpCodes.Ldc_I4_2,
			OpCodes.Ldc_I4_3,
			OpCodes.Ldc_I4_4,
			OpCodes.Ldc_I4_5,
			OpCodes.Ldc_I4_6,
			OpCodes.Ldc_I4_7,
			OpCodes.Ldc_I4_8
		};

		// Token: 0x04002230 RID: 8752
		private static OpCode[] _ldarg = new OpCode[]
		{
			OpCodes.Ldarg_0,
			OpCodes.Ldarg_1,
			OpCodes.Ldarg_2,
			OpCodes.Ldarg_3
		};

		// Token: 0x02000644 RID: 1604
		private class DefineTypeHelper
		{
			// Token: 0x17000EB2 RID: 3762
			// (get) Token: 0x06004575 RID: 17781 RVA: 0x00174991 File Offset: 0x00172B91
			// (set) Token: 0x06004576 RID: 17782 RVA: 0x00174999 File Offset: 0x00172B99
			public bool HasFatalErrors { get; private set; }

			// Token: 0x06004577 RID: 17783 RVA: 0x001749A4 File Offset: 0x00172BA4
			public DefineTypeHelper(Parser parser, ModuleBuilder module, TypeDefinitionAst typeDefinitionAst, string typeName)
			{
				this._moduleBuilder = module;
				this._parser = parser;
				this._typeDefinitionAst = typeDefinitionAst;
				List<Type> list;
				Type baseTypes = this.GetBaseTypes(parser, typeDefinitionAst, out list);
				this._typeBuilder = module.DefineType(typeName, TypeAttributes.Public, baseTypes, list.ToArray());
				this._staticHelpersTypeBuilder = module.DefineType(string.Format(CultureInfo.InvariantCulture, "<{0}_staticHelpers>", new object[]
				{
					typeName
				}), TypeAttributes.NotPublic);
				TypeDefiner.DefineCustomAttributes(this._typeBuilder, typeDefinitionAst.Attributes, this._parser, AttributeTargets.Class);
				this._typeDefinitionAst.Type = this._typeBuilder.AsType();
				this._fieldsToInitForMemberFunctions = new List<Tuple<string, object>>();
				this._definedMethods = new Dictionary<string, List<Tuple<FunctionMemberAst, Type[]>>>(StringComparer.OrdinalIgnoreCase);
				this._definedProperties = new Dictionary<string, PropertyMemberAst>(StringComparer.OrdinalIgnoreCase);
			}

			// Token: 0x06004578 RID: 17784 RVA: 0x00174A70 File Offset: 0x00172C70
			private Type GetBaseTypes(Parser parser, TypeDefinitionAst typeDefinitionAst, out List<Type> interfaces)
			{
				Type type = null;
				interfaces = new List<Type>();
				this._baseClassHasDefaultCtor = true;
				if (typeDefinitionAst.BaseTypes.Any<TypeConstraintAst>())
				{
					ReadOnlyCollection<TypeConstraintAst> baseTypes = typeDefinitionAst.BaseTypes;
					TypeConstraintAst typeConstraintAst = baseTypes[0];
					if (typeConstraintAst.TypeName.IsArray)
					{
						parser.ReportError(typeConstraintAst.Extent, () => ParserStrings.SubtypeArray, typeConstraintAst.TypeName.FullName);
					}
					else
					{
						type = typeConstraintAst.TypeName.GetReflectionType();
						if (type == null)
						{
							parser.ReportError(typeConstraintAst.Extent, () => ParserStrings.TypeNotFound, typeConstraintAst.TypeName.FullName);
						}
						else if (type.GetTypeInfo().IsSealed)
						{
							parser.ReportError(typeConstraintAst.Extent, () => ParserStrings.SealedBaseClass, type.Name);
							type = null;
						}
						else if (type.GetTypeInfo().IsGenericType && !type.IsConstructedGenericType)
						{
							parser.ReportError(typeConstraintAst.Extent, () => ParserStrings.SubtypeUnclosedGeneric, type.Name);
							type = null;
						}
						else if (type.GetTypeInfo().IsInterface)
						{
							interfaces.Add(type);
							type = null;
						}
					}
					if (type != null)
					{
						TypeName typeName = typeConstraintAst.TypeName as TypeName;
						if (typeName != null)
						{
							this._baseClassHasDefaultCtor = typeName.HasDefaultCtor();
						}
						else
						{
							this._baseClassHasDefaultCtor = type.HasDefaultCtor();
						}
					}
					for (int i = 0; i < baseTypes.Count; i++)
					{
						if (baseTypes[i].TypeName.IsArray)
						{
							parser.ReportError(baseTypes[i].Extent, () => ParserStrings.SubtypeArray, baseTypes[i].TypeName.FullName);
							this.HasFatalErrors = true;
						}
					}
					for (int j = 1; j < baseTypes.Count; j++)
					{
						if (baseTypes[j].TypeName.IsArray)
						{
							parser.ReportError(baseTypes[j].Extent, () => ParserStrings.SubtypeArray, baseTypes[j].TypeName.FullName);
						}
						else
						{
							Type reflectionType = baseTypes[j].TypeName.GetReflectionType();
							if (reflectionType == null)
							{
								parser.ReportError(baseTypes[j].Extent, () => ParserStrings.TypeNotFound, baseTypes[j].TypeName.FullName);
							}
							else if (reflectionType.GetTypeInfo().IsInterface)
							{
								interfaces.Add(reflectionType);
							}
							else
							{
								parser.ReportError(baseTypes[j].Extent, () => ParserStrings.InterfaceNameExpected, reflectionType.Name);
							}
						}
					}
				}
				if (type == null)
				{
					type = typeof(object);
				}
				return type;
			}

			// Token: 0x06004579 RID: 17785 RVA: 0x00174DD8 File Offset: 0x00172FD8
			public void DefineMembers()
			{
				bool flag = false;
				bool flag2 = false;
				List<FunctionMemberAst> list = new List<FunctionMemberAst>();
				List<FunctionMemberAst> list2 = new List<FunctionMemberAst>();
				foreach (MemberAst memberAst in this._typeDefinitionAst.Members)
				{
					PropertyMemberAst propertyMemberAst = memberAst as PropertyMemberAst;
					if (propertyMemberAst != null)
					{
						this.DefineProperty(propertyMemberAst);
						if (propertyMemberAst.InitialValue != null)
						{
							if (propertyMemberAst.IsStatic)
							{
								flag = true;
							}
							else
							{
								flag2 = true;
							}
						}
					}
					else
					{
						FunctionMemberAst functionMemberAst = memberAst as FunctionMemberAst;
						if (functionMemberAst.IsConstructor)
						{
							if (functionMemberAst.IsStatic)
							{
								list.Add(functionMemberAst);
							}
							else
							{
								list2.Add(functionMemberAst);
							}
						}
						this.DefineMethod(functionMemberAst);
					}
				}
				if (flag)
				{
					foreach (FunctionMemberAst functionMemberAst2 in list)
					{
						ReadOnlyCollection<ParameterAst> parameters = ((IParameterMetadataProvider)functionMemberAst2.Body).Parameters;
						if (parameters == null || parameters.Count == 0)
						{
							flag = false;
						}
					}
				}
				if (flag2)
				{
					flag2 = !list2.Any<FunctionMemberAst>();
				}
				if (flag)
				{
					CompilerGeneratedMemberFunctionAst ipmp = new CompilerGeneratedMemberFunctionAst(PositionUtilities.EmptyExtent, this._typeDefinitionAst, SpecialMemberFunctionType.StaticConstructor);
					this.DefineConstructor(ipmp, null, true, MethodAttributes.Private | MethodAttributes.Static, Type.EmptyTypes);
				}
				if (this._baseClassHasDefaultCtor)
				{
					if (flag2)
					{
						CompilerGeneratedMemberFunctionAst ipmp2 = new CompilerGeneratedMemberFunctionAst(PositionUtilities.EmptyExtent, this._typeDefinitionAst, SpecialMemberFunctionType.DefaultConstructor);
						this.DefineConstructor(ipmp2, null, true, MethodAttributes.Public, Type.EmptyTypes);
						return;
					}
				}
				else if (!list2.Any<FunctionMemberAst>())
				{
					this._parser.ReportError(this._typeDefinitionAst.Extent, () => ParserStrings.BaseClassNoDefaultCtor, this._typeBuilder.BaseType.Name);
					this.HasFatalErrors = true;
				}
			}

			// Token: 0x0600457A RID: 17786 RVA: 0x00174FB0 File Offset: 0x001731B0
			private void DefineProperty(PropertyMemberAst propertyMemberAst)
			{
				if (this._definedProperties.ContainsKey(propertyMemberAst.Name))
				{
					this._parser.ReportError(propertyMemberAst.Extent, () => ParserStrings.MemberAlreadyDefined, propertyMemberAst.Name);
					return;
				}
				this._definedProperties.Add(propertyMemberAst.Name, propertyMemberAst);
				Type type;
				if (propertyMemberAst.PropertyType == null)
				{
					type = typeof(object);
				}
				else
				{
					type = propertyMemberAst.PropertyType.TypeName.GetReflectionType();
				}
				PropertyBuilder member = this.EmitPropertyIl(propertyMemberAst, type);
				TypeDefiner.DefineCustomAttributes(member, propertyMemberAst.Attributes, this._parser, AttributeTargets.Property | AttributeTargets.Field);
			}

			// Token: 0x0600457B RID: 17787 RVA: 0x00175060 File Offset: 0x00173260
			private PropertyBuilder EmitPropertyIl(PropertyMemberAst propertyMemberAst, Type type)
			{
				FieldAttributes fieldAttributes = FieldAttributes.Private;
				MethodAttributes methodAttributes = MethodAttributes.HideBySig | MethodAttributes.SpecialName;
				methodAttributes |= (propertyMemberAst.IsPublic ? MethodAttributes.Public : MethodAttributes.Private);
				if (propertyMemberAst.IsStatic)
				{
					fieldAttributes |= FieldAttributes.Static;
					methodAttributes |= MethodAttributes.Static;
				}
				string fieldName = string.Format(CultureInfo.InvariantCulture, "<{0}>k__BackingField", new object[]
				{
					propertyMemberAst.Name
				});
				FieldBuilder field = this._typeBuilder.DefineField(fieldName, type, fieldAttributes);
				bool flag = false;
				if (propertyMemberAst.Attributes != null)
				{
					for (int i = 0; i < propertyMemberAst.Attributes.Count; i++)
					{
						Type reflectionAttributeType = propertyMemberAst.Attributes[i].TypeName.GetReflectionAttributeType();
						if (reflectionAttributeType != null && reflectionAttributeType.IsSubclassOf(typeof(ValidateArgumentsAttribute)))
						{
							flag = true;
							break;
						}
					}
				}
				PropertyBuilder propertyBuilder = this._typeBuilder.DefineProperty(propertyMemberAst.Name, PropertyAttributes.None, type, null);
				MethodBuilder methodBuilder = this._typeBuilder.DefineMethod("get_" + propertyMemberAst.Name, methodAttributes, type, Type.EmptyTypes);
				ILGenerator ilgenerator = methodBuilder.GetILGenerator();
				if (propertyMemberAst.IsStatic)
				{
					ilgenerator.Emit(OpCodes.Ldsfld, field);
					ilgenerator.Emit(OpCodes.Ret);
				}
				else
				{
					ilgenerator.Emit(OpCodes.Ldarg_0);
					ilgenerator.Emit(OpCodes.Ldfld, field);
					ilgenerator.Emit(OpCodes.Ret);
				}
				MethodBuilder methodBuilder2 = this._typeBuilder.DefineMethod("set_" + propertyMemberAst.Name, methodAttributes, null, new Type[]
				{
					type
				});
				ILGenerator ilgenerator2 = methodBuilder2.GetILGenerator();
				if (flag)
				{
					Type cls = this._typeBuilder.AsType();
					ilgenerator2.Emit(OpCodes.Ldtoken, cls);
					ilgenerator2.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
					ilgenerator2.Emit(OpCodes.Ldstr, propertyMemberAst.Name);
					ilgenerator2.Emit(propertyMemberAst.IsStatic ? OpCodes.Ldarg_0 : OpCodes.Ldarg_1);
					if (type.GetTypeInfo().IsValueType)
					{
						ilgenerator2.Emit(OpCodes.Box, type);
					}
					ilgenerator2.Emit(OpCodes.Call, CachedReflectionInfo.ClassOps_ValidateSetProperty);
				}
				if (propertyMemberAst.IsStatic)
				{
					ilgenerator2.Emit(OpCodes.Ldarg_0);
					ilgenerator2.Emit(OpCodes.Stsfld, field);
				}
				else
				{
					ilgenerator2.Emit(OpCodes.Ldarg_0);
					ilgenerator2.Emit(OpCodes.Ldarg_1);
					ilgenerator2.Emit(OpCodes.Stfld, field);
				}
				ilgenerator2.Emit(OpCodes.Ret);
				propertyBuilder.SetGetMethod(methodBuilder);
				propertyBuilder.SetSetMethod(methodBuilder2);
				if (propertyMemberAst.IsHidden)
				{
					propertyBuilder.SetCustomAttribute(TypeDefiner.hiddenCustomAttributeBuilder);
				}
				return propertyBuilder;
			}

			// Token: 0x0600457C RID: 17788 RVA: 0x00175304 File Offset: 0x00173504
			private bool CheckForDuplicateOverload(FunctionMemberAst functionMemberAst, Type[] newParameters)
			{
				List<Tuple<FunctionMemberAst, Type[]>> list;
				if (!this._definedMethods.TryGetValue(functionMemberAst.Name, out list))
				{
					list = new List<Tuple<FunctionMemberAst, Type[]>>();
					this._definedMethods.Add(functionMemberAst.Name, list);
				}
				else
				{
					foreach (Tuple<FunctionMemberAst, Type[]> tuple in list)
					{
						Type[] item = tuple.Item2;
						if (newParameters.Length == item.Length)
						{
							bool flag = true;
							for (int i = 0; i < newParameters.Length; i++)
							{
								if (newParameters[i] != item[i])
								{
									flag = false;
									break;
								}
							}
							if (flag && (tuple.Item1.IsStatic == functionMemberAst.IsStatic || !functionMemberAst.IsConstructor))
							{
								this._parser.ReportError(functionMemberAst.NameExtent ?? functionMemberAst.Extent, () => ParserStrings.MemberAlreadyDefined, functionMemberAst.Name);
								return true;
							}
						}
					}
				}
				list.Add(Tuple.Create<FunctionMemberAst, Type[]>(functionMemberAst, newParameters));
				return false;
			}

			// Token: 0x0600457D RID: 17789 RVA: 0x00175434 File Offset: 0x00173634
			private Type[] GetParameterTypes(FunctionMemberAst functionMemberAst)
			{
				ReadOnlyCollection<ParameterAst> parameters = ((IParameterMetadataProvider)functionMemberAst).Parameters;
				if (parameters == null)
				{
					return PSTypeExtensions.EmptyTypes;
				}
				bool flag = false;
				Type[] array = new Type[parameters.Count];
				for (int i = 0; i < parameters.Count; i++)
				{
					TypeConstraintAst typeConstraintAst = parameters[i].Attributes.OfType<TypeConstraintAst>().FirstOrDefault<TypeConstraintAst>();
					Type type = (typeConstraintAst != null) ? typeConstraintAst.TypeName.GetReflectionType() : typeof(object);
					if (type == null)
					{
						this._parser.ReportError(typeConstraintAst.Extent, () => ParserStrings.TypeNotFound, typeConstraintAst.TypeName.FullName);
						flag = true;
					}
					else if (type == typeof(void) || type.GetTypeInfo().IsGenericTypeDefinition)
					{
						this._parser.ReportError(typeConstraintAst.Extent, () => ParserStrings.TypeNotAllowedForParameter, typeConstraintAst.TypeName.FullName);
						flag = true;
					}
					array[i] = type;
				}
				if (!flag)
				{
					return array;
				}
				return null;
			}

			// Token: 0x0600457E RID: 17790 RVA: 0x00175564 File Offset: 0x00173764
			private bool MethodExistsOnBaseClassAndFinal(string methodName, Type[] parameterTypes)
			{
				TypeInfo typeInfo = this._typeBuilder.BaseType.GetTypeInfo();
				if (typeInfo is TypeBuilder)
				{
					return false;
				}
				MethodInfo method = typeInfo.AsType().GetMethod(methodName, parameterTypes);
				return method != null && method.IsFinal;
			}

			// Token: 0x0600457F RID: 17791 RVA: 0x001755C4 File Offset: 0x001737C4
			private void DefineMethod(FunctionMemberAst functionMemberAst)
			{
				Type[] parameterTypes = this.GetParameterTypes(functionMemberAst);
				if (parameterTypes == null)
				{
					return;
				}
				if (this.CheckForDuplicateOverload(functionMemberAst, parameterTypes))
				{
					return;
				}
				if (functionMemberAst.IsConstructor)
				{
					MethodAttributes methodAttributes = MethodAttributes.Public;
					if (functionMemberAst.IsStatic)
					{
						ReadOnlyCollection<ParameterAst> parameters = functionMemberAst.Parameters;
						if (parameters.Count > 0)
						{
							this._parser.ReportError(Parser.ExtentOf(parameters.First<ParameterAst>(), parameters.Last<ParameterAst>()), () => ParserStrings.StaticConstructorCantHaveParameters);
							return;
						}
						methodAttributes |= MethodAttributes.Static;
					}
					this.DefineConstructor(functionMemberAst, functionMemberAst.Attributes, functionMemberAst.IsHidden, methodAttributes, parameterTypes);
					return;
				}
				MethodAttributes methodAttributes2 = functionMemberAst.IsPublic ? MethodAttributes.Public : MethodAttributes.Private;
				if (functionMemberAst.IsStatic)
				{
					methodAttributes2 |= MethodAttributes.Static;
				}
				else
				{
					if (this.MethodExistsOnBaseClassAndFinal(functionMemberAst.Name, parameterTypes))
					{
						methodAttributes2 |= MethodAttributes.HideBySig;
						methodAttributes2 |= MethodAttributes.VtableLayoutMask;
					}
					methodAttributes2 |= MethodAttributes.Virtual;
				}
				Type returnType = functionMemberAst.GetReturnType();
				if (returnType == null)
				{
					this._parser.ReportError(functionMemberAst.ReturnType.Extent, () => ParserStrings.TypeNotFound, functionMemberAst.ReturnType.TypeName.FullName);
					return;
				}
				MethodBuilder method = this._typeBuilder.DefineMethod(functionMemberAst.Name, methodAttributes2, returnType, parameterTypes);
				TypeDefiner.DefineCustomAttributes(method, functionMemberAst.Attributes, this._parser, AttributeTargets.Method);
				if (functionMemberAst.IsHidden)
				{
					method.SetCustomAttribute(TypeDefiner.hiddenCustomAttributeBuilder);
				}
				ILGenerator ilgenerator = method.GetILGenerator();
				this.DefineMethodBody(functionMemberAst, ilgenerator, this.GetMetaDataName(method.Name, parameterTypes.Count<Type>()), functionMemberAst.IsStatic, parameterTypes, returnType, delegate(int i, string n)
				{
					method.DefineParameter(i, ParameterAttributes.None, n);
				});
			}

			// Token: 0x06004580 RID: 17792 RVA: 0x001757B0 File Offset: 0x001739B0
			private void DefineConstructor(IParameterMetadataProvider ipmp, ReadOnlyCollection<AttributeAst> attributeAsts, bool isHidden, MethodAttributes methodAttributes, Type[] parameterTypes)
			{
				bool flag = (methodAttributes & MethodAttributes.Static) != MethodAttributes.PrivateScope;
				ConstructorBuilder ctor = flag ? this._typeBuilder.DefineTypeInitializer() : this._typeBuilder.DefineConstructor(methodAttributes, CallingConventions.Standard, parameterTypes);
				TypeDefiner.DefineCustomAttributes(ctor, attributeAsts, this._parser, AttributeTargets.Constructor);
				if (isHidden)
				{
					ctor.SetCustomAttribute(TypeDefiner.hiddenCustomAttributeBuilder);
				}
				ILGenerator ilgenerator = ctor.GetILGenerator();
				this.DefineMethodBody(ipmp, ilgenerator, this.GetMetaDataName(ctor.Name, parameterTypes.Count<Type>()), flag, parameterTypes, typeof(void), delegate(int i, string n)
				{
					ctor.DefineParameter(i, ParameterAttributes.None, n);
				});
			}

			// Token: 0x06004581 RID: 17793 RVA: 0x00175864 File Offset: 0x00173A64
			private string GetMetaDataName(string name, int numberOfParameters)
			{
				int num = Interlocked.Increment(ref TypeDefiner.globalCounter);
				return string.Concat(new object[]
				{
					name,
					"_",
					numberOfParameters,
					"_",
					num
				});
			}

			// Token: 0x06004582 RID: 17794 RVA: 0x001758B4 File Offset: 0x00173AB4
			private void DefineMethodBody(IParameterMetadataProvider ipmp, ILGenerator ilGenerator, string metadataToken, bool isStatic, Type[] parameterTypes, Type returnType, Action<int, string> parameterNameSetter)
			{
				string text = string.Format(CultureInfo.InvariantCulture, "<{0}>", new object[]
				{
					metadataToken
				});
				FieldBuilder field = this._staticHelpersTypeBuilder.DefineField(text, typeof(ScriptBlockMemberMethodWrapper), FieldAttributes.Private | FieldAttributes.FamANDAssem | FieldAttributes.Static);
				ilGenerator.Emit(OpCodes.Ldsfld, field);
				if (isStatic)
				{
					ilGenerator.Emit(OpCodes.Ldnull);
				}
				else
				{
					TypeDefiner.EmitLdarg(ilGenerator, 0);
				}
				int num = parameterTypes.Length;
				if (num > 0)
				{
					ReadOnlyCollection<ParameterAst> parameters = ipmp.Parameters;
					LocalBuilder local = ilGenerator.DeclareLocal(typeof(object[]));
					TypeDefiner.EmitLdc(ilGenerator, num);
					ilGenerator.Emit(OpCodes.Newarr, typeof(object));
					ilGenerator.Emit(OpCodes.Stloc, local);
					int num2 = isStatic ? 0 : 1;
					int i = 0;
					while (i < num)
					{
						ilGenerator.Emit(OpCodes.Ldloc, local);
						TypeDefiner.EmitLdc(ilGenerator, i);
						TypeDefiner.EmitLdarg(ilGenerator, num2);
						if (parameterTypes[i].GetTypeInfo().IsValueType)
						{
							ilGenerator.Emit(OpCodes.Box, parameterTypes[i]);
						}
						ilGenerator.Emit(OpCodes.Stelem_Ref);
						parameterNameSetter(i + 1, parameters[i].Name.VariablePath.UserPath);
						i++;
						num2++;
					}
					ilGenerator.Emit(OpCodes.Ldloc, local);
				}
				else
				{
					ilGenerator.Emit(OpCodes.Ldsfld, typeof(ScriptBlockMemberMethodWrapper).GetField("_emptyArgumentArray", BindingFlags.Static | BindingFlags.Public));
				}
				MethodInfo methodInfo;
				if (returnType == typeof(void))
				{
					methodInfo = typeof(ScriptBlockMemberMethodWrapper).GetMethod("InvokeHelper", BindingFlags.Instance | BindingFlags.Public);
				}
				else
				{
					methodInfo = typeof(ScriptBlockMemberMethodWrapper).GetMethod("InvokeHelperT", BindingFlags.Instance | BindingFlags.Public).MakeGenericMethod(new Type[]
					{
						returnType
					});
				}
				ilGenerator.Emit(OpCodes.Tailcall);
				ilGenerator.EmitCall(OpCodes.Call, methodInfo, null);
				ilGenerator.Emit(OpCodes.Ret);
				ScriptBlockMemberMethodWrapper item = new ScriptBlockMemberMethodWrapper(ipmp);
				this._fieldsToInitForMemberFunctions.Add(Tuple.Create<string, object>(text, item));
			}

			// Token: 0x04002231 RID: 8753
			private readonly Parser _parser;

			// Token: 0x04002232 RID: 8754
			internal readonly TypeDefinitionAst _typeDefinitionAst;

			// Token: 0x04002233 RID: 8755
			internal readonly TypeBuilder _typeBuilder;

			// Token: 0x04002234 RID: 8756
			internal readonly ModuleBuilder _moduleBuilder;

			// Token: 0x04002235 RID: 8757
			internal readonly TypeBuilder _staticHelpersTypeBuilder;

			// Token: 0x04002236 RID: 8758
			private readonly Dictionary<string, PropertyMemberAst> _definedProperties;

			// Token: 0x04002237 RID: 8759
			private readonly Dictionary<string, List<Tuple<FunctionMemberAst, Type[]>>> _definedMethods;

			// Token: 0x04002238 RID: 8760
			internal readonly List<Tuple<string, object>> _fieldsToInitForMemberFunctions;

			// Token: 0x04002239 RID: 8761
			private bool _baseClassHasDefaultCtor;
		}

		// Token: 0x02000645 RID: 1605
		private class DefineEnumHelper
		{
			// Token: 0x06004583 RID: 17795 RVA: 0x00175ABF File Offset: 0x00173CBF
			internal DefineEnumHelper(Parser parser, ModuleBuilder module, TypeDefinitionAst enumDefinitionAst, string typeName)
			{
				this._parser = parser;
				this._enumDefinitionAst = enumDefinitionAst;
				this._moduleBuilder = module;
				this._typeName = typeName;
			}

			// Token: 0x06004584 RID: 17796 RVA: 0x00175B08 File Offset: 0x00173D08
			internal static List<TypeDefiner.DefineEnumHelper> Sort(List<TypeDefiner.DefineEnumHelper> defineEnumHelpers, Parser parser)
			{
				if (defineEnumHelpers.Count == 1)
				{
					return defineEnumHelpers;
				}
				Dictionary<TypeDefinitionAst, Tuple<TypeDefiner.DefineEnumHelper, List<TypeDefinitionAst>>> dictionary = new Dictionary<TypeDefinitionAst, Tuple<TypeDefiner.DefineEnumHelper, List<TypeDefinitionAst>>>();
				foreach (TypeDefiner.DefineEnumHelper defineEnumHelper in defineEnumHelpers)
				{
					dictionary.Add(defineEnumHelper._enumDefinitionAst, Tuple.Create<TypeDefiner.DefineEnumHelper, List<TypeDefinitionAst>>(defineEnumHelper, new List<TypeDefinitionAst>()));
				}
				foreach (TypeDefiner.DefineEnumHelper defineEnumHelper2 in defineEnumHelpers)
				{
					foreach (MemberAst memberAst in defineEnumHelper2._enumDefinitionAst.Members)
					{
						ExpressionAst initialValue = ((PropertyMemberAst)memberAst).InitialValue;
						if (initialValue != null)
						{
							foreach (Ast ast2 in initialValue.FindAll((Ast ast) => ast is MemberExpressionAst, false))
							{
								TypeExpressionAst typeExpressionAst = ((MemberExpressionAst)ast2).Expression as TypeExpressionAst;
								if (typeExpressionAst != null)
								{
									TypeName typeName = typeExpressionAst.TypeName as TypeName;
									if (typeName != null && typeName._typeDefinitionAst != null && typeName._typeDefinitionAst != defineEnumHelper2._enumDefinitionAst && dictionary.ContainsKey(typeName._typeDefinitionAst))
									{
										List<TypeDefinitionAst> item = dictionary[defineEnumHelper2._enumDefinitionAst].Item2;
										if (!item.Contains(typeName._typeDefinitionAst))
										{
											item.Add(typeName._typeDefinitionAst);
										}
									}
								}
							}
						}
					}
				}
				List<TypeDefiner.DefineEnumHelper> list = new List<TypeDefiner.DefineEnumHelper>(defineEnumHelpers.Count);
				List<TypeDefiner.DefineEnumHelper> list2 = new List<TypeDefiner.DefineEnumHelper>(defineEnumHelpers.Count);
				list2.AddRange(from value in dictionary.Values
				where value.Item2.Count == 0
				select value.Item1);
				while (list2.Count > 0)
				{
					TypeDefiner.DefineEnumHelper defineEnumHelper3 = list2[list2.Count - 1];
					list2.RemoveAt(list2.Count - 1);
					list.Add(defineEnumHelper3);
					foreach (Tuple<TypeDefiner.DefineEnumHelper, List<TypeDefinitionAst>> tuple in dictionary.Values)
					{
						tuple.Item2.Remove(defineEnumHelper3._enumDefinitionAst);
						if (tuple.Item2.Count == 0 && !list.Contains(tuple.Item1) && !list2.Contains(tuple.Item1))
						{
							list2.Add(tuple.Item1);
						}
					}
				}
				if (list.Count < defineEnumHelpers.Count)
				{
					foreach (TypeDefiner.DefineEnumHelper defineEnumHelper4 in defineEnumHelpers)
					{
						if (!list.Contains(defineEnumHelper4))
						{
							parser.ReportError(defineEnumHelper4._enumDefinitionAst.Extent, () => ParserStrings.CycleInEnumInitializers);
						}
					}
				}
				return list;
			}

			// Token: 0x06004585 RID: 17797 RVA: 0x00175EF4 File Offset: 0x001740F4
			internal void DefineEnum()
			{
				HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				EnumBuilder enumBuilder = this._moduleBuilder.DefineEnum(this._typeName, TypeAttributes.Public, typeof(int));
				TypeDefiner.DefineCustomAttributes(enumBuilder, this._enumDefinitionAst.Attributes, this._parser, AttributeTargets.Enum);
				int num = 0;
				bool flag = false;
				foreach (MemberAst memberAst in this._enumDefinitionAst.Members)
				{
					PropertyMemberAst propertyMemberAst = (PropertyMemberAst)memberAst;
					if (propertyMemberAst.InitialValue != null)
					{
						object obj;
						if (IsConstantValueVisitor.IsConstant(propertyMemberAst.InitialValue, out obj, false, false))
						{
							if (obj is int)
							{
								num = (int)obj;
							}
							else if (!LanguagePrimitives.TryConvertTo<int>(obj, out num))
							{
								if (obj != null && LanguagePrimitives.IsNumeric(LanguagePrimitives.GetTypeCode(obj.GetType())))
								{
									this._parser.ReportError(propertyMemberAst.InitialValue.Extent, () => ParserStrings.EnumeratorValueTooLarge);
								}
								else
								{
									this._parser.ReportError(propertyMemberAst.InitialValue.Extent, () => ParserStrings.CannotConvertValue, ToStringCodeMethods.Type(typeof(int), false));
								}
							}
						}
						else
						{
							this._parser.ReportError(propertyMemberAst.InitialValue.Extent, () => ParserStrings.EnumeratorValueMustBeConstant);
						}
					}
					else if (flag)
					{
						this._parser.ReportError(propertyMemberAst.Extent, () => ParserStrings.EnumeratorValueTooLarge);
					}
					if (hashSet.Contains(propertyMemberAst.Name))
					{
						this._parser.ReportError(propertyMemberAst.Extent, () => ParserStrings.MemberAlreadyDefined, propertyMemberAst.Name);
					}
					else
					{
						hashSet.Add(propertyMemberAst.Name);
						enumBuilder.DefineLiteral(propertyMemberAst.Name, num);
						if (num < 2147483647)
						{
							num++;
							flag = false;
						}
						else
						{
							flag = true;
						}
					}
				}
				this._enumDefinitionAst.Type = enumBuilder.CreateTypeInfo().AsType();
			}

			// Token: 0x0400223B RID: 8763
			private readonly Parser _parser;

			// Token: 0x0400223C RID: 8764
			private readonly TypeDefinitionAst _enumDefinitionAst;

			// Token: 0x0400223D RID: 8765
			private readonly ModuleBuilder _moduleBuilder;

			// Token: 0x0400223E RID: 8766
			private readonly string _typeName;
		}
	}
}
