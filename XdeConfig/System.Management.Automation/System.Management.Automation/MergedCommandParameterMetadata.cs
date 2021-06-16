using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x02000071 RID: 113
	internal class MergedCommandParameterMetadata
	{
		// Token: 0x06000615 RID: 1557 RVA: 0x0001C74C File Offset: 0x0001A94C
		internal List<MergedCompiledCommandParameter> ReplaceMetadata(MergedCommandParameterMetadata metadata)
		{
			List<MergedCompiledCommandParameter> list = new List<MergedCompiledCommandParameter>();
			this.bindableParameters.Clear();
			foreach (KeyValuePair<string, MergedCompiledCommandParameter> keyValuePair in metadata.BindableParameters)
			{
				this.bindableParameters.Add(keyValuePair.Key, keyValuePair.Value);
				list.Add(keyValuePair.Value);
			}
			this.aliasedParameters.Clear();
			foreach (KeyValuePair<string, MergedCompiledCommandParameter> keyValuePair2 in metadata.AliasedParameters)
			{
				this.aliasedParameters.Add(keyValuePair2.Key, keyValuePair2.Value);
			}
			this._defaultParameterSetName = metadata._defaultParameterSetName;
			this.nextAvailableParameterSetIndex = metadata.nextAvailableParameterSetIndex;
			this.parameterSetMap.Clear();
			List<string> list2 = (List<string>)this.parameterSetMap;
			list2.AddRange(metadata.parameterSetMap);
			return list;
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x0001C868 File Offset: 0x0001AA68
		internal Collection<MergedCompiledCommandParameter> AddMetadataForBinder(InternalParameterMetadata parameterMetadata, ParameterBinderAssociation binderAssociation)
		{
			if (parameterMetadata == null)
			{
				throw PSTraceSource.NewArgumentNullException("parameterMetadata");
			}
			Collection<MergedCompiledCommandParameter> collection = new Collection<MergedCompiledCommandParameter>();
			foreach (KeyValuePair<string, CompiledCommandParameter> keyValuePair in parameterMetadata.BindableParameters)
			{
				if (this.bindableParameters.ContainsKey(keyValuePair.Key))
				{
					MetadataException ex = new MetadataException("ParameterNameAlreadyExistsForCommand", null, Metadata.ParameterNameAlreadyExistsForCommand, new object[]
					{
						keyValuePair.Key
					});
					throw ex;
				}
				if (this.aliasedParameters.ContainsKey(keyValuePair.Key))
				{
					MetadataException ex2 = new MetadataException("ParameterNameConflictsWithAlias", null, Metadata.ParameterNameConflictsWithAlias, new object[]
					{
						keyValuePair.Key,
						MergedCommandParameterMetadata.RetrieveParameterNameForAlias(keyValuePair.Key, this.aliasedParameters)
					});
					throw ex2;
				}
				MergedCompiledCommandParameter mergedCompiledCommandParameter = new MergedCompiledCommandParameter(keyValuePair.Value, binderAssociation);
				this.bindableParameters.Add(keyValuePair.Key, mergedCompiledCommandParameter);
				collection.Add(mergedCompiledCommandParameter);
				foreach (string text in keyValuePair.Value.Aliases)
				{
					if (this.aliasedParameters.ContainsKey(text))
					{
						MetadataException ex3 = new MetadataException("AliasParameterNameAlreadyExistsForCommand", null, Metadata.AliasParameterNameAlreadyExistsForCommand, new object[]
						{
							text
						});
						throw ex3;
					}
					if (this.bindableParameters.ContainsKey(text))
					{
						MetadataException ex4 = new MetadataException("ParameterNameConflictsWithAlias", null, Metadata.ParameterNameConflictsWithAlias, new object[]
						{
							MergedCommandParameterMetadata.RetrieveParameterNameForAlias(text, this.bindableParameters),
							keyValuePair.Value.Name
						});
						throw ex4;
					}
					this.aliasedParameters.Add(text, mergedCompiledCommandParameter);
				}
			}
			return collection;
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x0001CA7C File Offset: 0x0001AC7C
		internal int ParameterSetCount
		{
			get
			{
				return this.parameterSetMap.Count;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000618 RID: 1560 RVA: 0x0001CA89 File Offset: 0x0001AC89
		internal uint AllParameterSetFlags
		{
			get
			{
				return (1U << this.ParameterSetCount) - 1U;
			}
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x0001CA98 File Offset: 0x0001AC98
		private int AddParameterSetToMap(string parameterSetName)
		{
			int num = -1;
			if (!string.IsNullOrEmpty(parameterSetName))
			{
				num = this.parameterSetMap.IndexOf(parameterSetName);
				if (num == -1)
				{
					if (this.nextAvailableParameterSetIndex == 4294967295U)
					{
						ParsingMetadataException ex = new ParsingMetadataException("ParsingTooManyParameterSets", null, Metadata.ParsingTooManyParameterSets, new object[0]);
						throw ex;
					}
					this.parameterSetMap.Add(parameterSetName);
					num = this.parameterSetMap.IndexOf(parameterSetName);
					this.nextAvailableParameterSetIndex += 1U;
				}
			}
			return num;
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0001CB0C File Offset: 0x0001AD0C
		internal uint GenerateParameterSetMappingFromMetadata(string defaultParameterSetName)
		{
			this.parameterSetMap.Clear();
			this.nextAvailableParameterSetIndex = 0U;
			uint result = 0U;
			if (!string.IsNullOrEmpty(defaultParameterSetName))
			{
				this._defaultParameterSetName = defaultParameterSetName;
				int num = this.AddParameterSetToMap(defaultParameterSetName);
				result = 1U << num;
			}
			foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter in this.BindableParameters.Values)
			{
				uint num2 = 0U;
				foreach (KeyValuePair<string, ParameterSetSpecificMetadata> keyValuePair in mergedCompiledCommandParameter.Parameter.ParameterSetData)
				{
					string key = keyValuePair.Key;
					ParameterSetSpecificMetadata value = keyValuePair.Value;
					if (string.Equals(key, "__AllParameterSets", StringComparison.OrdinalIgnoreCase))
					{
						value.ParameterSetFlag = 0U;
						value.IsInAllSets = true;
						mergedCompiledCommandParameter.Parameter.IsInAllSets = true;
					}
					else
					{
						int num3 = this.AddParameterSetToMap(key);
						uint num4 = 1U << num3;
						num2 |= num4;
						value.ParameterSetFlag = num4;
					}
				}
				mergedCompiledCommandParameter.Parameter.ParameterSetFlags = num2;
			}
			return result;
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x0001CC48 File Offset: 0x0001AE48
		internal string GetParameterSetName(uint parameterSet)
		{
			string text = this._defaultParameterSetName;
			if (string.IsNullOrEmpty(text))
			{
				text = "__AllParameterSets";
			}
			if (parameterSet != 4294967295U && parameterSet != 0U)
			{
				int num = 0;
				while ((parameterSet >> num & 1U) == 0U)
				{
					num++;
				}
				if ((parameterSet >> num + 1 & 1U) == 0U)
				{
					if (num < this.parameterSetMap.Count)
					{
						text = this.parameterSetMap[num];
					}
					else
					{
						text = string.Empty;
					}
				}
				else
				{
					text = string.Empty;
				}
			}
			return text;
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x0001CCBC File Offset: 0x0001AEBC
		private static string RetrieveParameterNameForAlias(string key, IDictionary<string, MergedCompiledCommandParameter> dict)
		{
			MergedCompiledCommandParameter mergedCompiledCommandParameter = dict[key];
			if (mergedCompiledCommandParameter != null)
			{
				CompiledCommandParameter parameter = mergedCompiledCommandParameter.Parameter;
				if (parameter != null && !string.IsNullOrEmpty(parameter.Name))
				{
					return parameter.Name;
				}
			}
			return string.Empty;
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0001CCF8 File Offset: 0x0001AEF8
		internal MergedCompiledCommandParameter GetMatchingParameter(string name, bool throwOnParameterNotFound, bool tryExactMatching, InvocationInfo invocationInfo)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			Collection<MergedCompiledCommandParameter> collection = new Collection<MergedCompiledCommandParameter>();
			if (name.Length > 0 && SpecialCharacters.IsDash(name[0]))
			{
				name = name.Substring(1);
			}
			foreach (string text in this.bindableParameters.Keys)
			{
				if (CultureInfo.InvariantCulture.CompareInfo.IsPrefix(text, name, CompareOptions.IgnoreCase))
				{
					if (tryExactMatching && string.Equals(text, name, StringComparison.OrdinalIgnoreCase))
					{
						return this.bindableParameters[text];
					}
					collection.Add(this.bindableParameters[text]);
				}
			}
			foreach (string text2 in this.aliasedParameters.Keys)
			{
				if (CultureInfo.InvariantCulture.CompareInfo.IsPrefix(text2, name, CompareOptions.IgnoreCase))
				{
					if (tryExactMatching && string.Equals(text2, name, StringComparison.OrdinalIgnoreCase))
					{
						return this.aliasedParameters[text2];
					}
					if (!collection.Contains(this.aliasedParameters[text2]))
					{
						collection.Add(this.aliasedParameters[text2]);
					}
				}
			}
			if (collection.Count > 1)
			{
				Collection<MergedCompiledCommandParameter> collection2 = new Collection<MergedCompiledCommandParameter>();
				foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter in collection)
				{
					if (mergedCompiledCommandParameter.BinderAssociation == ParameterBinderAssociation.DeclaredFormalParameters || mergedCompiledCommandParameter.BinderAssociation == ParameterBinderAssociation.DynamicParameters)
					{
						collection2.Add(mergedCompiledCommandParameter);
					}
				}
				if (collection2.Count != 1)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter2 in collection)
					{
						stringBuilder.Append(" -");
						stringBuilder.Append(mergedCompiledCommandParameter2.Parameter.Name);
					}
					ParameterBindingException ex = new ParameterBindingException(ErrorCategory.InvalidArgument, invocationInfo, null, name, null, null, ParameterBinderStrings.AmbiguousParameter, "AmbiguousParameter", new object[]
					{
						stringBuilder
					});
					throw ex;
				}
				collection = collection2;
			}
			else if (collection.Count == 0 && throwOnParameterNotFound)
			{
				ParameterBindingException ex2 = new ParameterBindingException(ErrorCategory.InvalidArgument, invocationInfo, null, name, null, null, ParameterBinderStrings.NamedParameterNotFound, "NamedParameterNotFound", new object[0]);
				throw ex2;
			}
			MergedCompiledCommandParameter result = null;
			if (collection.Count > 0)
			{
				result = collection[0];
			}
			return result;
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x0001CFA4 File Offset: 0x0001B1A4
		internal Collection<MergedCompiledCommandParameter> GetParametersInParameterSet(uint parameterSetFlag)
		{
			Collection<MergedCompiledCommandParameter> collection = new Collection<MergedCompiledCommandParameter>();
			foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter in this.BindableParameters.Values)
			{
				if ((parameterSetFlag & mergedCompiledCommandParameter.Parameter.ParameterSetFlags) != 0U || mergedCompiledCommandParameter.Parameter.IsInAllSets)
				{
					collection.Add(mergedCompiledCommandParameter);
				}
			}
			return collection;
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x0001D01C File Offset: 0x0001B21C
		internal IDictionary<string, MergedCompiledCommandParameter> BindableParameters
		{
			get
			{
				return this.bindableParameters;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x0001D024 File Offset: 0x0001B224
		internal IDictionary<string, MergedCompiledCommandParameter> AliasedParameters
		{
			get
			{
				return this.aliasedParameters;
			}
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0001D02C File Offset: 0x0001B22C
		internal void MakeReadOnly()
		{
			this.bindableParameters = new ReadOnlyDictionary<string, MergedCompiledCommandParameter>(this.bindableParameters);
			this.aliasedParameters = new ReadOnlyDictionary<string, MergedCompiledCommandParameter>(this.aliasedParameters);
			this.parameterSetMap = new ReadOnlyCollection<string>(this.parameterSetMap);
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x0001D061 File Offset: 0x0001B261
		internal void ResetReadOnly()
		{
			this.bindableParameters = new Dictionary<string, MergedCompiledCommandParameter>(this.bindableParameters, StringComparer.OrdinalIgnoreCase);
			this.aliasedParameters = new Dictionary<string, MergedCompiledCommandParameter>(this.aliasedParameters, StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x04000271 RID: 625
		private uint nextAvailableParameterSetIndex;

		// Token: 0x04000272 RID: 626
		private IList<string> parameterSetMap = new List<string>();

		// Token: 0x04000273 RID: 627
		private string _defaultParameterSetName;

		// Token: 0x04000274 RID: 628
		private IDictionary<string, MergedCompiledCommandParameter> bindableParameters = new Dictionary<string, MergedCompiledCommandParameter>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000275 RID: 629
		private IDictionary<string, MergedCompiledCommandParameter> aliasedParameters = new Dictionary<string, MergedCompiledCommandParameter>(StringComparer.OrdinalIgnoreCase);
	}
}
