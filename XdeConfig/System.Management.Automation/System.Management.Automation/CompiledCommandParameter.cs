using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace System.Management.Automation
{
	// Token: 0x0200005F RID: 95
	internal class CompiledCommandParameter
	{
		// Token: 0x06000515 RID: 1301 RVA: 0x00018970 File Offset: 0x00016B70
		internal CompiledCommandParameter(RuntimeDefinedParameter runtimeDefinedParameter, bool processingDynamicParameters)
		{
			if (runtimeDefinedParameter == null)
			{
				throw PSTraceSource.NewArgumentNullException("runtimeDefinedParameter");
			}
			this.name = runtimeDefinedParameter.Name;
			this.type = runtimeDefinedParameter.ParameterType;
			this.isDynamic = processingDynamicParameters;
			this.collectionTypeInformation = new ParameterCollectionTypeInformation(runtimeDefinedParameter.ParameterType);
			this.ConstructCompiledAttributesUsingRuntimeDefinedParameter(runtimeDefinedParameter);
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00018A00 File Offset: 0x00016C00
		internal CompiledCommandParameter(MemberInfo member, bool processingDynamicParameters)
		{
			if (member == null)
			{
				throw PSTraceSource.NewArgumentNullException("member");
			}
			this.name = member.Name;
			this.declaringType = member.DeclaringType;
			this.isDynamic = processingDynamicParameters;
			PropertyInfo propertyInfo = member as PropertyInfo;
			if (propertyInfo != null)
			{
				this.type = propertyInfo.PropertyType;
			}
			else
			{
				FieldInfo fieldInfo = member as FieldInfo;
				if (!(fieldInfo != null))
				{
					ArgumentException ex = PSTraceSource.NewArgumentException("member", DiscoveryExceptions.CompiledCommandParameterMemberMustBeFieldOrProperty, new object[0]);
					throw ex;
				}
				this.type = fieldInfo.FieldType;
			}
			this.collectionTypeInformation = new ParameterCollectionTypeInformation(this.type);
			this.ConstructCompiledAttributesUsingReflection(member);
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x00018AE9 File Offset: 0x00016CE9
		internal string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x00018AF1 File Offset: 0x00016CF1
		// (set) Token: 0x06000519 RID: 1305 RVA: 0x00018AF9 File Offset: 0x00016CF9
		internal string PSTypeName { get; private set; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x00018B02 File Offset: 0x00016D02
		internal Type Type
		{
			get
			{
				if (this.type == null)
				{
					this.type = Type.GetType(this.typeName);
				}
				return this.type;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x00018B29 File Offset: 0x00016D29
		internal Type DeclaringType
		{
			get
			{
				return this.declaringType;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x00018B31 File Offset: 0x00016D31
		internal bool IsDynamic
		{
			get
			{
				return this.isDynamic;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x00018B39 File Offset: 0x00016D39
		internal ParameterCollectionTypeInformation CollectionTypeInformation
		{
			get
			{
				if (this.collectionTypeInformation == null)
				{
					this.collectionTypeInformation = new ParameterCollectionTypeInformation(this.Type);
				}
				return this.collectionTypeInformation;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x00018B5C File Offset: 0x00016D5C
		internal Collection<Attribute> CompiledAttributes
		{
			get
			{
				if (this.attributes == null)
				{
					MemberInfo[] member = this.Type.GetMember(this.Name, InternalParameterMetadata.metaDataBindingFlags);
					if (member.Length > 0)
					{
						this.ConstructCompiledAttributesUsingReflection(member[0]);
					}
				}
				return this.attributes;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x00018B9D File Offset: 0x00016D9D
		internal ReadOnlyCollection<ArgumentTransformationAttribute> ArgumentTransformationAttributes
		{
			get
			{
				return new ReadOnlyCollection<ArgumentTransformationAttribute>(this.argumentTransformationAttributes);
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x00018BAA File Offset: 0x00016DAA
		internal ReadOnlyCollection<ValidateArgumentsAttribute> ValidationAttributes
		{
			get
			{
				return new ReadOnlyCollection<ValidateArgumentsAttribute>(this.validationAttributes);
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x00018BB7 File Offset: 0x00016DB7
		// (set) Token: 0x06000522 RID: 1314 RVA: 0x00018BBF File Offset: 0x00016DBF
		internal ObsoleteAttribute ObsoleteAttribute { get; private set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x00018BC8 File Offset: 0x00016DC8
		internal bool AllowsNullArgument
		{
			get
			{
				return this.allowsNullArgument;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x00018BD0 File Offset: 0x00016DD0
		internal bool AllowsEmptyStringArgument
		{
			get
			{
				return this.allowsEmptyStringArgument;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x00018BD8 File Offset: 0x00016DD8
		internal bool AllowsEmptyCollectionArgument
		{
			get
			{
				return this.allowsEmptyCollectionArgument;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x00018BE0 File Offset: 0x00016DE0
		// (set) Token: 0x06000527 RID: 1319 RVA: 0x00018BE8 File Offset: 0x00016DE8
		internal bool IsInAllSets
		{
			get
			{
				return this.isInAllSets;
			}
			set
			{
				this.isInAllSets = value;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x00018BF1 File Offset: 0x00016DF1
		internal bool IsPipelineParameterInSomeParameterSet
		{
			get
			{
				return this._isPipelineParameterInSomeParameterSet;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x00018BF9 File Offset: 0x00016DF9
		internal bool IsMandatoryInSomeParameterSet
		{
			get
			{
				return this._isMandatoryInSomeParameterSet;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x00018C01 File Offset: 0x00016E01
		// (set) Token: 0x0600052B RID: 1323 RVA: 0x00018C09 File Offset: 0x00016E09
		internal uint ParameterSetFlags
		{
			get
			{
				return this._parameterSetFlags;
			}
			set
			{
				this._parameterSetFlags = value;
			}
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00018C14 File Offset: 0x00016E14
		internal bool DoesParameterSetTakePipelineInput(uint validParameterSetFlags)
		{
			if (!this._isPipelineParameterInSomeParameterSet)
			{
				return false;
			}
			foreach (ParameterSetSpecificMetadata parameterSetSpecificMetadata in this.ParameterSetData.Values)
			{
				if ((parameterSetSpecificMetadata.IsInAllSets || (parameterSetSpecificMetadata.ParameterSetFlag & validParameterSetFlags) != 0U) && (parameterSetSpecificMetadata.ValueFromPipeline || parameterSetSpecificMetadata.ValueFromPipelineByPropertyName))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00018C98 File Offset: 0x00016E98
		internal ParameterSetSpecificMetadata GetParameterSetData(uint parameterSetFlag)
		{
			ParameterSetSpecificMetadata result = null;
			foreach (ParameterSetSpecificMetadata parameterSetSpecificMetadata in this.ParameterSetData.Values)
			{
				if (parameterSetSpecificMetadata.IsInAllSets)
				{
					result = parameterSetSpecificMetadata;
				}
				else if ((parameterSetSpecificMetadata.ParameterSetFlag & parameterSetFlag) != 0U)
				{
					result = parameterSetSpecificMetadata;
					break;
				}
			}
			return result;
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00018EEC File Offset: 0x000170EC
		internal IEnumerable<ParameterSetSpecificMetadata> GetMatchingParameterSetData(uint parameterSetFlags)
		{
			foreach (ParameterSetSpecificMetadata setData in this.ParameterSetData.Values)
			{
				if (setData.IsInAllSets)
				{
					yield return setData;
				}
				else if ((setData.ParameterSetFlag & parameterSetFlags) != 0U)
				{
					yield return setData;
				}
			}
			yield break;
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x00018F10 File Offset: 0x00017110
		internal Dictionary<string, ParameterSetSpecificMetadata> ParameterSetData
		{
			get
			{
				return this._parameterSetData;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x00018F18 File Offset: 0x00017118
		internal ReadOnlyCollection<string> Aliases
		{
			get
			{
				return new ReadOnlyCollection<string>(this.aliases);
			}
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00018F28 File Offset: 0x00017128
		private void ConstructCompiledAttributesUsingRuntimeDefinedParameter(RuntimeDefinedParameter runtimeDefinedParameter)
		{
			this.attributes = new Collection<Attribute>();
			this._parameterSetData = new Dictionary<string, ParameterSetSpecificMetadata>(StringComparer.OrdinalIgnoreCase);
			foreach (Attribute attribute in runtimeDefinedParameter.Attributes)
			{
				if (!(attribute is ArgumentTypeConverterAttribute))
				{
					this.ProcessAttribute(runtimeDefinedParameter.Name, attribute);
				}
			}
			if (this.type == typeof(PSCredential) && this.argumentTransformationAttributes.Count == 0)
			{
				this.ProcessAttribute(runtimeDefinedParameter.Name, new CredentialAttribute());
			}
			foreach (ArgumentTypeConverterAttribute attribute2 in runtimeDefinedParameter.Attributes.OfType<ArgumentTypeConverterAttribute>())
			{
				this.ProcessAttribute(runtimeDefinedParameter.Name, attribute2);
			}
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0001901C File Offset: 0x0001721C
		private void ConstructCompiledAttributesUsingReflection(MemberInfo member)
		{
			this.attributes = new Collection<Attribute>();
			this._parameterSetData = new Dictionary<string, ParameterSetSpecificMetadata>(StringComparer.OrdinalIgnoreCase);
			object[] customAttributes = member.GetCustomAttributes(false);
			foreach (Attribute attribute in customAttributes)
			{
				this.ProcessAttribute(member.Name, attribute);
			}
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00019074 File Offset: 0x00017274
		private void ProcessAttribute(string memberName, Attribute attribute)
		{
			if (attribute == null)
			{
				return;
			}
			this.attributes.Add(attribute);
			ParameterAttribute parameterAttribute = attribute as ParameterAttribute;
			if (parameterAttribute != null)
			{
				this.ProcessParameterAttribute(memberName, parameterAttribute);
				return;
			}
			AliasAttribute aliasAttribute = attribute as AliasAttribute;
			if (aliasAttribute != null)
			{
				this.ProcessAliasAttribute(aliasAttribute);
				return;
			}
			ArgumentTransformationAttribute argumentTransformationAttribute = attribute as ArgumentTransformationAttribute;
			if (argumentTransformationAttribute != null)
			{
				this.argumentTransformationAttributes.Add(argumentTransformationAttribute);
				return;
			}
			ValidateArgumentsAttribute validateArgumentsAttribute = attribute as ValidateArgumentsAttribute;
			if (validateArgumentsAttribute != null)
			{
				this.validationAttributes.Add(validateArgumentsAttribute);
				return;
			}
			ObsoleteAttribute obsoleteAttribute = attribute as ObsoleteAttribute;
			if (obsoleteAttribute != null)
			{
				this.ObsoleteAttribute = obsoleteAttribute;
				return;
			}
			AllowNullAttribute allowNullAttribute = attribute as AllowNullAttribute;
			if (allowNullAttribute != null)
			{
				this.allowsNullArgument = true;
				return;
			}
			AllowEmptyStringAttribute allowEmptyStringAttribute = attribute as AllowEmptyStringAttribute;
			if (allowEmptyStringAttribute != null)
			{
				this.allowsEmptyStringArgument = true;
				return;
			}
			AllowEmptyCollectionAttribute allowEmptyCollectionAttribute = attribute as AllowEmptyCollectionAttribute;
			if (allowEmptyCollectionAttribute != null)
			{
				this.allowsEmptyCollectionArgument = true;
				return;
			}
			PSTypeNameAttribute pstypeNameAttribute = attribute as PSTypeNameAttribute;
			if (pstypeNameAttribute != null)
			{
				this.PSTypeName = pstypeNameAttribute.PSTypeName;
			}
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00019150 File Offset: 0x00017350
		private void ProcessParameterAttribute(string parameterName, ParameterAttribute parameter)
		{
			if (this._parameterSetData.ContainsKey(parameter.ParameterSetName))
			{
				MetadataException ex = new MetadataException("ParameterDeclaredInParameterSetMultipleTimes", null, DiscoveryExceptions.ParameterDeclaredInParameterSetMultipleTimes, new object[]
				{
					parameterName,
					parameter.ParameterSetName
				});
				throw ex;
			}
			if (parameter.ValueFromPipeline || parameter.ValueFromPipelineByPropertyName)
			{
				this._isPipelineParameterInSomeParameterSet = true;
			}
			if (parameter.Mandatory)
			{
				this._isMandatoryInSomeParameterSet = true;
			}
			ParameterSetSpecificMetadata value = new ParameterSetSpecificMetadata(parameter);
			this._parameterSetData.Add(parameter.ParameterSetName, value);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x000191D8 File Offset: 0x000173D8
		private void ProcessAliasAttribute(AliasAttribute attribute)
		{
			foreach (string item in attribute.aliasNames)
			{
				this.aliases.Add(item);
			}
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0001920A File Offset: 0x0001740A
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x04000206 RID: 518
		private string name = string.Empty;

		// Token: 0x04000207 RID: 519
		private Type type;

		// Token: 0x04000208 RID: 520
		private string typeName = string.Empty;

		// Token: 0x04000209 RID: 521
		private Type declaringType;

		// Token: 0x0400020A RID: 522
		private bool isDynamic;

		// Token: 0x0400020B RID: 523
		private ParameterCollectionTypeInformation collectionTypeInformation;

		// Token: 0x0400020C RID: 524
		private Collection<Attribute> attributes;

		// Token: 0x0400020D RID: 525
		private Collection<ArgumentTransformationAttribute> argumentTransformationAttributes = new Collection<ArgumentTransformationAttribute>();

		// Token: 0x0400020E RID: 526
		private Collection<ValidateArgumentsAttribute> validationAttributes = new Collection<ValidateArgumentsAttribute>();

		// Token: 0x0400020F RID: 527
		private bool allowsNullArgument;

		// Token: 0x04000210 RID: 528
		private bool allowsEmptyStringArgument;

		// Token: 0x04000211 RID: 529
		private bool allowsEmptyCollectionArgument;

		// Token: 0x04000212 RID: 530
		private bool isInAllSets;

		// Token: 0x04000213 RID: 531
		private bool _isPipelineParameterInSomeParameterSet;

		// Token: 0x04000214 RID: 532
		private bool _isMandatoryInSomeParameterSet;

		// Token: 0x04000215 RID: 533
		private uint _parameterSetFlags;

		// Token: 0x04000216 RID: 534
		private Dictionary<string, ParameterSetSpecificMetadata> _parameterSetData;

		// Token: 0x04000217 RID: 535
		private Collection<string> aliases = new Collection<string>();
	}
}
