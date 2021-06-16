using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management.Automation.Language;
using System.Reflection;

namespace System.Management.Automation
{
	// Token: 0x02000633 RID: 1587
	internal static class TypeOps
	{
		// Token: 0x060044CC RID: 17612 RVA: 0x00170A00 File Offset: 0x0016EC00
		internal static Type ResolveTypeName(ITypeName typeName)
		{
			Exception ex;
			Type type = TypeResolver.ResolveITypeName(typeName, out ex);
			if (!(type == null))
			{
				return type;
			}
			if (ex == null)
			{
				GenericTypeName genericTypeName = typeName as GenericTypeName;
				if (genericTypeName != null)
				{
					Type genericType = genericTypeName.GetGenericType(TypeOps.ResolveTypeName(genericTypeName.TypeName));
					Type[] typeArguments = (from arg in genericTypeName.GenericArguments
					select TypeOps.ResolveTypeName(arg)).ToArray<Type>();
					try
					{
						if (genericType != null && genericType.GetTypeInfo().ContainsGenericParameters)
						{
							genericType.MakeGenericType(typeArguments);
						}
					}
					catch (Exception ex2)
					{
						CommandProcessorBase.CheckForSevereException(ex2);
						throw InterpreterError.NewInterpreterException(typeName, typeof(RuntimeException), null, "TypeNotFoundWithMessage", ParserStrings.TypeNotFoundWithMessage, new object[]
						{
							typeName.FullName,
							ex2.Message
						});
					}
				}
				ArrayTypeName arrayTypeName = typeName as ArrayTypeName;
				if (arrayTypeName != null)
				{
					TypeOps.ResolveTypeName(arrayTypeName.ElementType);
				}
				throw InterpreterError.NewInterpreterException(typeName, typeof(RuntimeException), null, "TypeNotFound", ParserStrings.TypeNotFound, new object[]
				{
					typeName.FullName
				});
			}
			if (ex is RuntimeException)
			{
				throw ex;
			}
			throw InterpreterError.NewInterpreterException(typeName, typeof(RuntimeException), null, "TypeNotFoundWithMessage", ParserStrings.TypeNotFoundWithMessage, new object[]
			{
				typeName.FullName,
				ex.Message
			});
		}

		// Token: 0x060044CD RID: 17613 RVA: 0x00170B78 File Offset: 0x0016ED78
		internal static bool IsInstance(object left, object right)
		{
			object obj = PSObject.Base(left);
			object obj2 = PSObject.Base(right);
			Type type = obj2 as Type;
			if (type == null)
			{
				type = ParserOps.ConvertTo<Type>(obj2, null);
				if (type == null)
				{
					throw InterpreterError.NewInterpreterException(obj2, typeof(RuntimeException), null, "IsOperatorRequiresType", ParserStrings.IsOperatorRequiresType, new object[0]);
				}
			}
			return (type == typeof(PSCustomObject) && obj is PSObject) || (type.Equals(typeof(PSObject)) && left is PSObject) || type.IsInstanceOfType(obj);
		}

		// Token: 0x060044CE RID: 17614 RVA: 0x00170C18 File Offset: 0x0016EE18
		internal static object AsOperator(object left, Type type)
		{
			if (type == null)
			{
				throw InterpreterError.NewInterpreterException(null, typeof(RuntimeException), null, "AsOperatorRequiresType", ParserStrings.AsOperatorRequiresType, new object[0]);
			}
			bool flag;
			LanguagePrimitives.ConversionData conversionData = LanguagePrimitives.FigureConversion(left, type, out flag);
			if (conversionData.Rank == ConversionRank.None)
			{
				return null;
			}
			object result;
			try
			{
				if (flag)
				{
					result = conversionData.Invoke(PSObject.Base(left), type, false, (PSObject)left, NumberFormatInfo.InvariantInfo, null);
				}
				else
				{
					result = conversionData.Invoke(left, type, false, null, NumberFormatInfo.InvariantInfo, null);
				}
			}
			catch (PSInvalidCastException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060044CF RID: 17615 RVA: 0x00170CB0 File Offset: 0x0016EEB0
		internal static string[] GetNamespacesForTypeResolutionState(IEnumerable<UsingStatementAst> usingAsts)
		{
			bool flag = false;
			List<string> list = new List<string>();
			foreach (UsingStatementAst usingStatementAst in usingAsts)
			{
				if (usingStatementAst.UsingStatementKind == UsingStatementKind.Namespace)
				{
					if (!flag && usingStatementAst.Name.Value.Equals("System", StringComparison.OrdinalIgnoreCase))
					{
						flag = true;
					}
					list.Add(usingStatementAst.Name.Value);
				}
			}
			if (!flag)
			{
				list.Insert(0, "System");
			}
			return list.ToArray();
		}

		// Token: 0x060044D0 RID: 17616 RVA: 0x00170D48 File Offset: 0x0016EF48
		internal static void AddPowerShellTypesToTheScope(Dictionary<string, TypeDefinitionAst> types, ExecutionContext context)
		{
			TypeResolutionState typeResolutionState;
			if ((typeResolutionState = context.EngineSessionState.CurrentScope.TypeResolutionState) == null)
			{
				typeResolutionState = (context.EngineSessionState.CurrentScope.TypeResolutionState = TypeResolutionState.UsingSystem);
			}
			TypeResolutionState typeResolutionState2 = typeResolutionState;
			foreach (KeyValuePair<string, TypeDefinitionAst> keyValuePair in types)
			{
				context.EngineSessionState.CurrentScope.AddType(keyValuePair.Key, keyValuePair.Value.Type);
				typeResolutionState2.typesDefined.Add(keyValuePair.Key);
			}
		}

		// Token: 0x060044D1 RID: 17617 RVA: 0x00170DF4 File Offset: 0x0016EFF4
		internal static void SetCurrentTypeResolutionState(TypeResolutionState trs, ExecutionContext context)
		{
			context.EngineSessionState.CurrentScope.TypeResolutionState = trs;
		}

		// Token: 0x060044D2 RID: 17618 RVA: 0x00170E07 File Offset: 0x0016F007
		internal static void SetAssemblyDefiningPSTypes(FunctionContext functionContext, Assembly assembly)
		{
			functionContext._scriptBlock.AssemblyDefiningPSTypes = assembly;
		}
	}
}
