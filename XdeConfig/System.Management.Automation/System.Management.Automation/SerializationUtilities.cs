using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Tracing;
using System.Reflection;

namespace System.Management.Automation
{
	// Token: 0x0200045A RID: 1114
	internal static class SerializationUtilities
	{
		// Token: 0x060030A3 RID: 12451 RVA: 0x0010A21C File Offset: 0x0010841C
		internal static object GetPropertyValue(PSObject psObject, string propertyName)
		{
			PSNoteProperty psnoteProperty = (PSNoteProperty)psObject.Properties[propertyName];
			if (psnoteProperty == null)
			{
				return null;
			}
			return psnoteProperty.Value;
		}

		// Token: 0x060030A4 RID: 12452 RVA: 0x0010A248 File Offset: 0x00108448
		internal static object GetPsObjectPropertyBaseObject(PSObject psObject, string propertyName)
		{
			PSObject psobject = (PSObject)SerializationUtilities.GetPropertyValue(psObject, propertyName);
			if (psobject == null)
			{
				return null;
			}
			return psobject.BaseObject;
		}

		// Token: 0x060030A5 RID: 12453 RVA: 0x0010A270 File Offset: 0x00108470
		internal static void GetKnownContainerTypeInfo(object source, out ContainerType ct, out IDictionary dictionary, out IEnumerable enumerable)
		{
			ct = ContainerType.None;
			dictionary = null;
			enumerable = null;
			dictionary = (source as IDictionary);
			if (dictionary != null)
			{
				ct = ContainerType.Dictionary;
			}
			else if (source is Stack)
			{
				ct = ContainerType.Stack;
				enumerable = LanguagePrimitives.GetEnumerable(source);
			}
			else if (source is Queue)
			{
				ct = ContainerType.Queue;
				enumerable = LanguagePrimitives.GetEnumerable(source);
			}
			else if (source is IList)
			{
				ct = ContainerType.List;
				enumerable = LanguagePrimitives.GetEnumerable(source);
			}
			else
			{
				Type type = source.GetType();
				if (type.GetTypeInfo().IsGenericType)
				{
					if (SerializationUtilities.DerivesFromGenericType(type, typeof(Stack<>)))
					{
						ct = ContainerType.Stack;
						enumerable = LanguagePrimitives.GetEnumerable(source);
					}
					else if (SerializationUtilities.DerivesFromGenericType(type, typeof(Queue<>)))
					{
						ct = ContainerType.Queue;
						enumerable = LanguagePrimitives.GetEnumerable(source);
					}
					else if (SerializationUtilities.DerivesFromGenericType(type, typeof(List<>)))
					{
						ct = ContainerType.List;
						enumerable = LanguagePrimitives.GetEnumerable(source);
					}
				}
			}
			if (ct == ContainerType.None)
			{
				try
				{
					enumerable = LanguagePrimitives.GetEnumerable(source);
					if (enumerable != null)
					{
						ct = ContainerType.Enumerable;
					}
				}
				catch (Exception ex)
				{
					CommandProcessorBase.CheckForSevereException(ex);
					PSEtwLog.LogAnalyticWarning(PSEventId.Serializer_EnumerationFailed, PSOpcode.Exception, PSTask.Serialization, PSKeyword.Serializer, new object[]
					{
						source.GetType().AssemblyQualifiedName,
						ex.ToString()
					});
				}
			}
			if (ct == ContainerType.None)
			{
				enumerable = (source as IEnumerable);
				if (enumerable != null)
				{
					IEnumerator enumerator = enumerable.GetEnumerator();
					if (enumerator != null && enumerator.MoveNext())
					{
						ct = ContainerType.Enumerable;
					}
				}
			}
		}

		// Token: 0x060030A6 RID: 12454 RVA: 0x0010A3D4 File Offset: 0x001085D4
		private static bool DerivesFromGenericType(Type derived, Type baseType)
		{
			while (derived != null)
			{
				if (derived.GetTypeInfo().IsGenericType)
				{
					derived = derived.GetGenericTypeDefinition();
				}
				if (derived == baseType)
				{
					return true;
				}
				derived = derived.GetTypeInfo().BaseType;
			}
			return false;
		}

		// Token: 0x060030A7 RID: 12455 RVA: 0x0010A410 File Offset: 0x00108610
		internal static string GetToString(object source)
		{
			string result = null;
			try
			{
				result = Convert.ToString(source, CultureInfo.InvariantCulture);
			}
			catch (Exception ex)
			{
				CommandProcessorBase.CheckForSevereException(ex);
				PSEtwLog.LogAnalyticWarning(PSEventId.Serializer_ToStringFailed, PSOpcode.Exception, PSTask.Serialization, (PSKeyword)4611686018427387968UL, new object[]
				{
					source.GetType().AssemblyQualifiedName,
					ex.ToString()
				});
			}
			return result;
		}

		// Token: 0x060030A8 RID: 12456 RVA: 0x0010A47C File Offset: 0x0010867C
		internal static string GetToStringForPrimitiveObject(PSObject pso)
		{
			if (pso == null)
			{
				return null;
			}
			if (pso.ToStringFromDeserialization != null)
			{
				return pso.ToStringFromDeserialization;
			}
			string tokenText = pso.TokenText;
			if (tokenText != null)
			{
				string toString = SerializationUtilities.GetToString(pso.BaseObject);
				if (toString == null || !string.Equals(tokenText, toString, StringComparison.Ordinal))
				{
					return tokenText;
				}
			}
			return null;
		}

		// Token: 0x060030A9 RID: 12457 RVA: 0x0010A4C4 File Offset: 0x001086C4
		internal static PSMemberInfoInternalCollection<PSPropertyInfo> GetSpecificPropertiesToSerialize(PSObject source, Collection<CollectionEntry<PSPropertyInfo>> allPropertiesCollection, TypeTable typeTable)
		{
			if (source == null)
			{
				return null;
			}
			if (source.GetSerializationMethod(typeTable) == SerializationMethod.SpecificProperties)
			{
				PSEtwLog.LogAnalyticVerbose(PSEventId.Serializer_ModeOverride, PSOpcode.SerializationSettings, PSTask.Serialization, (PSKeyword)4611686018427387968UL, new object[]
				{
					source.InternalTypeNames.Key,
					2U
				});
				PSMemberInfoInternalCollection<PSPropertyInfo> psmemberInfoInternalCollection = new PSMemberInfoInternalCollection<PSPropertyInfo>();
				PSMemberInfoIntegratingCollection<PSPropertyInfo> psmemberInfoIntegratingCollection = new PSMemberInfoIntegratingCollection<PSPropertyInfo>(source, allPropertiesCollection);
				Collection<string> specificPropertiesToSerialize = source.GetSpecificPropertiesToSerialize(typeTable);
				foreach (string text in specificPropertiesToSerialize)
				{
					PSPropertyInfo pspropertyInfo = psmemberInfoIntegratingCollection[text];
					if (pspropertyInfo == null)
					{
						PSEtwLog.LogAnalyticWarning(PSEventId.Serializer_SpecificPropertyMissing, PSOpcode.Exception, PSTask.Serialization, (PSKeyword)4611686018427387968UL, new object[]
						{
							source.InternalTypeNames.Key,
							text
						});
					}
					else
					{
						psmemberInfoInternalCollection.Add(pspropertyInfo);
					}
				}
				return psmemberInfoInternalCollection;
			}
			return null;
		}

		// Token: 0x060030AA RID: 12458 RVA: 0x0010A5BC File Offset: 0x001087BC
		internal static object GetPropertyValueInThreadSafeManner(PSPropertyInfo property, bool canUseDefaultRunspaceInThreadSafeManner, out bool success)
		{
			if (!property.IsGettable)
			{
				success = false;
				return null;
			}
			PSAliasProperty psaliasProperty = property as PSAliasProperty;
			if (psaliasProperty != null)
			{
				property = (psaliasProperty.ReferencedMember as PSPropertyInfo);
			}
			PSScriptProperty psscriptProperty = property as PSScriptProperty;
			if (psscriptProperty != null && !canUseDefaultRunspaceInThreadSafeManner)
			{
				PSEtwLog.LogAnalyticWarning(PSEventId.Serializer_ScriptPropertyWithoutRunspace, PSOpcode.Exception, PSTask.Serialization, (PSKeyword)4611686018427387968UL, new object[]
				{
					property.Name,
					(property.instance == null) ? string.Empty : PSObject.GetTypeNames(property.instance).Key,
					psscriptProperty.GetterScript.ToString()
				});
				success = false;
				return null;
			}
			object result;
			try
			{
				object value = property.Value;
				success = true;
				result = value;
			}
			catch (ExtendedTypeSystemException ex)
			{
				PSEtwLog.LogAnalyticWarning(PSEventId.Serializer_PropertyGetterFailed, PSOpcode.Exception, PSTask.Serialization, (PSKeyword)4611686018427387968UL, new object[]
				{
					property.Name,
					(property.instance == null) ? string.Empty : PSObject.GetTypeNames(property.instance).Key,
					ex.ToString(),
					(ex.InnerException == null) ? string.Empty : ex.InnerException.ToString()
				});
				success = false;
				result = null;
			}
			return result;
		}
	}
}
