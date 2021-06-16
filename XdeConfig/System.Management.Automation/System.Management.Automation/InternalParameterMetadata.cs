using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.Management.Automation
{
	// Token: 0x02000089 RID: 137
	internal class InternalParameterMetadata
	{
		// Token: 0x060006F8 RID: 1784 RVA: 0x00021C6C File Offset: 0x0001FE6C
		internal static InternalParameterMetadata Get(RuntimeDefinedParameterDictionary runtimeDefinedParameters, bool processingDynamicParameters, bool checkNames)
		{
			if (runtimeDefinedParameters == null)
			{
				throw PSTraceSource.NewArgumentNullException("runtimeDefinedParameter");
			}
			return new InternalParameterMetadata(runtimeDefinedParameters, processingDynamicParameters, checkNames);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x00021C84 File Offset: 0x0001FE84
		internal static InternalParameterMetadata Get(Type type, ExecutionContext context, bool processingDynamicParameters)
		{
			if (type == null)
			{
				throw PSTraceSource.NewArgumentNullException("type");
			}
			InternalParameterMetadata internalParameterMetadata;
			if (context != null && InternalParameterMetadata.ParameterMetadataCache.ContainsKey(type.AssemblyQualifiedName))
			{
				internalParameterMetadata = InternalParameterMetadata.ParameterMetadataCache[type.AssemblyQualifiedName];
			}
			else
			{
				internalParameterMetadata = new InternalParameterMetadata(type, processingDynamicParameters);
				if (context != null)
				{
					InternalParameterMetadata.ParameterMetadataCache.TryAdd(type.AssemblyQualifiedName, internalParameterMetadata);
				}
			}
			return internalParameterMetadata;
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00021CF0 File Offset: 0x0001FEF0
		internal InternalParameterMetadata(RuntimeDefinedParameterDictionary runtimeDefinedParameters, bool processingDynamicParameters, bool checkNames)
		{
			if (runtimeDefinedParameters == null)
			{
				throw PSTraceSource.NewArgumentNullException("runtimeDefinedParameters");
			}
			this.ConstructCompiledParametersUsingRuntimeDefinedParameters(runtimeDefinedParameters, processingDynamicParameters, checkNames);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00021D48 File Offset: 0x0001FF48
		internal InternalParameterMetadata(Type type, bool processingDynamicParameters)
		{
			if (type == null)
			{
				throw PSTraceSource.NewArgumentNullException("type");
			}
			this.type = type;
			this.typeName = type.Name;
			this.ConstructCompiledParametersUsingReflection(processingDynamicParameters);
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x00021DB4 File Offset: 0x0001FFB4
		internal string TypeName
		{
			get
			{
				return this.typeName;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x00021DBC File Offset: 0x0001FFBC
		internal Dictionary<string, CompiledCommandParameter> BindableParameters
		{
			get
			{
				return this.bindableParameters;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x00021DC4 File Offset: 0x0001FFC4
		internal Dictionary<string, CompiledCommandParameter> AliasedParameters
		{
			get
			{
				return this.aliasedParameters;
			}
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00021DCC File Offset: 0x0001FFCC
		private void ConstructCompiledParametersUsingRuntimeDefinedParameters(RuntimeDefinedParameterDictionary runtimeDefinedParameters, bool processingDynamicParameters, bool checkNames)
		{
			foreach (RuntimeDefinedParameter runtimeDefinedParameter in runtimeDefinedParameters.Values)
			{
				if (runtimeDefinedParameter != null)
				{
					CompiledCommandParameter parameter = new CompiledCommandParameter(runtimeDefinedParameter, processingDynamicParameters);
					this.AddParameter(parameter, checkNames);
				}
			}
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x00021E2C File Offset: 0x0002002C
		private void ConstructCompiledParametersUsingReflection(bool processingDynamicParameters)
		{
			PropertyInfo[] properties = this.type.GetProperties(InternalParameterMetadata.metaDataBindingFlags);
			FieldInfo[] fields = this.type.GetFields(InternalParameterMetadata.metaDataBindingFlags);
			foreach (PropertyInfo member in properties)
			{
				if (InternalParameterMetadata.IsMemberAParameter(member))
				{
					this.AddParameter(member, processingDynamicParameters);
				}
			}
			foreach (FieldInfo member2 in fields)
			{
				if (InternalParameterMetadata.IsMemberAParameter(member2))
				{
					this.AddParameter(member2, processingDynamicParameters);
				}
			}
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x00021EB4 File Offset: 0x000200B4
		private void CheckForReservedParameter(string name)
		{
			if (name.Equals("SelectProperty", StringComparison.OrdinalIgnoreCase) || name.Equals("SelectObject", StringComparison.OrdinalIgnoreCase))
			{
				throw new MetadataException("ReservedParameterName", null, DiscoveryExceptions.ReservedParameterName, new object[]
				{
					name
				});
			}
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x00021EFC File Offset: 0x000200FC
		private void AddParameter(MemberInfo member, bool processingDynamicParameters)
		{
			bool flag = false;
			bool flag2 = false;
			this.CheckForReservedParameter(member.Name);
			if (this.bindableParameters.ContainsKey(member.Name))
			{
				CompiledCommandParameter compiledCommandParameter = this.bindableParameters[member.Name];
				Type declaringType = compiledCommandParameter.DeclaringType;
				if (declaringType == null)
				{
					flag = true;
				}
				else if (declaringType.IsSubclassOf(member.DeclaringType))
				{
					flag2 = true;
				}
				else if (member.DeclaringType.IsSubclassOf(declaringType))
				{
					this.RemoveParameter(compiledCommandParameter);
				}
				else
				{
					flag = true;
				}
			}
			if (flag)
			{
				throw new MetadataException("DuplicateParameterDefinition", null, ParameterBinderStrings.DuplicateParameterDefinition, new object[]
				{
					member.Name
				});
			}
			if (!flag2)
			{
				CompiledCommandParameter parameter = new CompiledCommandParameter(member, processingDynamicParameters);
				this.AddParameter(parameter, true);
			}
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x00021FBC File Offset: 0x000201BC
		private void AddParameter(CompiledCommandParameter parameter, bool checkNames)
		{
			if (checkNames)
			{
				this.CheckForReservedParameter(parameter.Name);
			}
			this.bindableParameters.Add(parameter.Name, parameter);
			foreach (string text in parameter.Aliases)
			{
				if (this.aliasedParameters.ContainsKey(text))
				{
					throw new MetadataException("AliasDeclaredMultipleTimes", null, DiscoveryExceptions.AliasDeclaredMultipleTimes, new object[]
					{
						text
					});
				}
				this.aliasedParameters.Add(text, parameter);
			}
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0002205C File Offset: 0x0002025C
		private void RemoveParameter(CompiledCommandParameter parameter)
		{
			this.bindableParameters.Remove(parameter.Name);
			foreach (string key in parameter.Aliases)
			{
				this.aliasedParameters.Remove(key);
			}
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x000220C4 File Offset: 0x000202C4
		private static bool IsMemberAParameter(MemberInfo member)
		{
			bool result = false;
			try
			{
				object[] customAttributes = member.GetCustomAttributes(typeof(ParameterAttribute), false);
				if (customAttributes.Any<object>())
				{
					result = true;
				}
			}
			catch (MetadataException ex)
			{
				throw new MetadataException("GetCustomAttributesMetadataException", ex, Metadata.MetadataMemberInitialization, new object[]
				{
					member.Name,
					ex.Message
				});
			}
			catch (ArgumentException ex2)
			{
				throw new MetadataException("GetCustomAttributesArgumentException", ex2, Metadata.MetadataMemberInitialization, new object[]
				{
					member.Name,
					ex2.Message
				});
			}
			return result;
		}

		// Token: 0x040002F5 RID: 757
		private string typeName = string.Empty;

		// Token: 0x040002F6 RID: 758
		private Dictionary<string, CompiledCommandParameter> bindableParameters = new Dictionary<string, CompiledCommandParameter>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040002F7 RID: 759
		private Dictionary<string, CompiledCommandParameter> aliasedParameters = new Dictionary<string, CompiledCommandParameter>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040002F8 RID: 760
		private Type type;

		// Token: 0x040002F9 RID: 761
		internal static readonly BindingFlags metaDataBindingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;

		// Token: 0x040002FA RID: 762
		private static ConcurrentDictionary<string, InternalParameterMetadata> ParameterMetadataCache = new ConcurrentDictionary<string, InternalParameterMetadata>(StringComparer.Ordinal);
	}
}
