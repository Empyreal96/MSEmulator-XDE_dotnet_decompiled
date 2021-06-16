using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Threading;
using Microsoft.PowerShell;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x020002DE RID: 734
	internal static class RemotingDecoder
	{
		// Token: 0x060022E9 RID: 8937 RVA: 0x000C4994 File Offset: 0x000C2B94
		private static T ConvertPropertyValueTo<T>(string propertyName, object propertyValue)
		{
			if (propertyName == null)
			{
				throw PSTraceSource.NewArgumentNullException("propertyName");
			}
			if (typeof(T).GetTypeInfo().IsEnum)
			{
				if (propertyValue is string)
				{
					try
					{
						string value = (string)propertyValue;
						return (T)((object)Enum.Parse(typeof(T), value, true));
					}
					catch (ArgumentException)
					{
						throw new PSRemotingDataStructureException(RemotingErrorIdStrings.CantCastPropertyToExpectedType, new object[]
						{
							propertyName,
							typeof(T).FullName,
							propertyValue.GetType().FullName
						});
					}
				}
				try
				{
					Type underlyingType = Enum.GetUnderlyingType(typeof(T));
					object obj = LanguagePrimitives.ConvertTo(propertyValue, underlyingType, CultureInfo.InvariantCulture);
					return (T)((object)obj);
				}
				catch (InvalidCastException)
				{
					throw new PSRemotingDataStructureException(RemotingErrorIdStrings.CantCastPropertyToExpectedType, new object[]
					{
						propertyName,
						typeof(T).FullName,
						propertyValue.GetType().FullName
					});
				}
			}
			if (typeof(T).Equals(typeof(PSObject)))
			{
				if (propertyValue == null)
				{
					return default(T);
				}
				return (T)((object)PSObject.AsPSObject(propertyValue));
			}
			else if (propertyValue == null)
			{
				TypeInfo typeInfo = typeof(T).GetTypeInfo();
				if (!typeInfo.IsValueType)
				{
					return default(T);
				}
				if (typeInfo.IsGenericType && typeof(T).GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
				{
					return default(T);
				}
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.CantCastPropertyToExpectedType, new object[]
				{
					propertyName,
					typeof(T).FullName,
					(propertyValue != null) ? propertyValue.GetType().FullName : "null"
				});
			}
			else
			{
				if (propertyValue is T)
				{
					return (T)((object)propertyValue);
				}
				if (propertyValue is PSObject)
				{
					PSObject psobject = (PSObject)propertyValue;
					return RemotingDecoder.ConvertPropertyValueTo<T>(propertyName, psobject.BaseObject);
				}
				if (propertyValue is Hashtable && typeof(T).Equals(typeof(PSPrimitiveDictionary)))
				{
					try
					{
						return (T)((object)new PSPrimitiveDictionary((Hashtable)propertyValue));
					}
					catch (ArgumentException)
					{
						throw new PSRemotingDataStructureException(RemotingErrorIdStrings.CantCastPropertyToExpectedType, new object[]
						{
							propertyName,
							typeof(T).FullName,
							(propertyValue != null) ? propertyValue.GetType().FullName : "null"
						});
					}
				}
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.CantCastPropertyToExpectedType, new object[]
				{
					propertyName,
					typeof(T).FullName,
					propertyValue.GetType().FullName
				});
			}
			T result;
			return result;
		}

		// Token: 0x060022EA RID: 8938 RVA: 0x000C4C8C File Offset: 0x000C2E8C
		private static PSPropertyInfo GetProperty(PSObject psObject, string propertyName)
		{
			if (psObject == null)
			{
				throw PSTraceSource.NewArgumentNullException("psObject");
			}
			if (propertyName == null)
			{
				throw PSTraceSource.NewArgumentNullException("propertyName");
			}
			PSPropertyInfo pspropertyInfo = psObject.Properties[propertyName];
			if (pspropertyInfo == null)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.MissingProperty, new object[]
				{
					propertyName
				});
			}
			return pspropertyInfo;
		}

		// Token: 0x060022EB RID: 8939 RVA: 0x000C4CE0 File Offset: 0x000C2EE0
		internal static T GetPropertyValue<T>(PSObject psObject, string propertyName)
		{
			if (psObject == null)
			{
				throw PSTraceSource.NewArgumentNullException("psObject");
			}
			if (propertyName == null)
			{
				throw PSTraceSource.NewArgumentNullException("propertyName");
			}
			PSPropertyInfo property = RemotingDecoder.GetProperty(psObject, propertyName);
			object value = property.Value;
			return RemotingDecoder.ConvertPropertyValueTo<T>(propertyName, value);
		}

		// Token: 0x060022EC RID: 8940 RVA: 0x000C4F18 File Offset: 0x000C3118
		internal static IEnumerable<T> EnumerateListProperty<T>(PSObject psObject, string propertyName)
		{
			if (psObject == null)
			{
				throw PSTraceSource.NewArgumentNullException("psObject");
			}
			if (propertyName == null)
			{
				throw PSTraceSource.NewArgumentNullException("propertyName");
			}
			IEnumerable e = RemotingDecoder.GetPropertyValue<IEnumerable>(psObject, propertyName);
			if (e != null)
			{
				foreach (object o in e)
				{
					yield return RemotingDecoder.ConvertPropertyValueTo<T>(propertyName, o);
				}
			}
			yield break;
		}

		// Token: 0x060022ED RID: 8941 RVA: 0x000C5188 File Offset: 0x000C3388
		internal static IEnumerable<KeyValuePair<KeyType, ValueType>> EnumerateHashtableProperty<KeyType, ValueType>(PSObject psObject, string propertyName)
		{
			if (psObject == null)
			{
				throw PSTraceSource.NewArgumentNullException("psObject");
			}
			if (propertyName == null)
			{
				throw PSTraceSource.NewArgumentNullException("propertyName");
			}
			Hashtable h = RemotingDecoder.GetPropertyValue<Hashtable>(psObject, propertyName);
			if (h != null)
			{
				foreach (object obj in h)
				{
					DictionaryEntry e = (DictionaryEntry)obj;
					DictionaryEntry dictionaryEntry = e;
					KeyType key = RemotingDecoder.ConvertPropertyValueTo<KeyType>(propertyName, dictionaryEntry.Key);
					DictionaryEntry dictionaryEntry2 = e;
					ValueType value = RemotingDecoder.ConvertPropertyValueTo<ValueType>(propertyName, dictionaryEntry2.Value);
					yield return new KeyValuePair<KeyType, ValueType>(key, value);
				}
			}
			yield break;
		}

		// Token: 0x060022EE RID: 8942 RVA: 0x000C51AC File Offset: 0x000C33AC
		internal static RunspacePoolStateInfo GetRunspacePoolStateInfo(PSObject dataAsPSObject)
		{
			if (dataAsPSObject == null)
			{
				throw PSTraceSource.NewArgumentNullException("dataAsPSObject");
			}
			RunspacePoolState propertyValue = RemotingDecoder.GetPropertyValue<RunspacePoolState>(dataAsPSObject, "RunspaceState");
			Exception exceptionFromStateInfoObject = RemotingDecoder.GetExceptionFromStateInfoObject(dataAsPSObject);
			return new RunspacePoolStateInfo(propertyValue, exceptionFromStateInfoObject);
		}

		// Token: 0x060022EF RID: 8943 RVA: 0x000C51E1 File Offset: 0x000C33E1
		internal static PSPrimitiveDictionary GetApplicationPrivateData(PSObject dataAsPSObject)
		{
			if (dataAsPSObject == null)
			{
				throw PSTraceSource.NewArgumentNullException("dataAsPSObject");
			}
			return RemotingDecoder.GetPropertyValue<PSPrimitiveDictionary>(dataAsPSObject, "ApplicationPrivateData");
		}

		// Token: 0x060022F0 RID: 8944 RVA: 0x000C51FC File Offset: 0x000C33FC
		internal static string GetPublicKey(PSObject dataAsPSObject)
		{
			if (dataAsPSObject == null)
			{
				throw PSTraceSource.NewArgumentNullException("dataAsPSObject");
			}
			return RemotingDecoder.GetPropertyValue<string>(dataAsPSObject, "PublicKey");
		}

		// Token: 0x060022F1 RID: 8945 RVA: 0x000C5217 File Offset: 0x000C3417
		internal static string GetEncryptedSessionKey(PSObject dataAsPSObject)
		{
			if (dataAsPSObject == null)
			{
				throw PSTraceSource.NewArgumentNullException("dataAsPSObject");
			}
			return RemotingDecoder.GetPropertyValue<string>(dataAsPSObject, "EncryptedSessionKey");
		}

		// Token: 0x060022F2 RID: 8946 RVA: 0x000C5234 File Offset: 0x000C3434
		internal static PSEventArgs GetPSEventArgs(PSObject dataAsPSObject)
		{
			if (dataAsPSObject == null)
			{
				throw PSTraceSource.NewArgumentNullException("dataAsPSObject");
			}
			int propertyValue = RemotingDecoder.GetPropertyValue<int>(dataAsPSObject, "PSEventArgs.EventIdentifier");
			string propertyValue2 = RemotingDecoder.GetPropertyValue<string>(dataAsPSObject, "PSEventArgs.SourceIdentifier");
			object propertyValue3 = RemotingDecoder.GetPropertyValue<object>(dataAsPSObject, "PSEventArgs.Sender");
			object propertyValue4 = RemotingDecoder.GetPropertyValue<object>(dataAsPSObject, "PSEventArgs.MessageData");
			string propertyValue5 = RemotingDecoder.GetPropertyValue<string>(dataAsPSObject, "PSEventArgs.ComputerName");
			Guid propertyValue6 = RemotingDecoder.GetPropertyValue<Guid>(dataAsPSObject, "PSEventArgs.RunspaceId");
			ArrayList arrayList = new ArrayList();
			foreach (object value in RemotingDecoder.EnumerateListProperty<object>(dataAsPSObject, "PSEventArgs.SourceArgs"))
			{
				arrayList.Add(value);
			}
			return new PSEventArgs(propertyValue5, propertyValue6, propertyValue, propertyValue2, propertyValue3, arrayList.ToArray(), (propertyValue4 == null) ? null : PSObject.AsPSObject(propertyValue4))
			{
				TimeGenerated = RemotingDecoder.GetPropertyValue<DateTime>(dataAsPSObject, "PSEventArgs.TimeGenerated")
			};
		}

		// Token: 0x060022F3 RID: 8947 RVA: 0x000C5324 File Offset: 0x000C3524
		internal static int GetMinRunspaces(PSObject dataAsPSObject)
		{
			if (dataAsPSObject == null)
			{
				throw PSTraceSource.NewArgumentNullException("dataAsPSObject");
			}
			return RemotingDecoder.GetPropertyValue<int>(dataAsPSObject, "MinRunspaces");
		}

		// Token: 0x060022F4 RID: 8948 RVA: 0x000C533F File Offset: 0x000C353F
		internal static int GetMaxRunspaces(PSObject dataAsPSObject)
		{
			if (dataAsPSObject == null)
			{
				throw PSTraceSource.NewArgumentNullException("dataAsPSObject");
			}
			return RemotingDecoder.GetPropertyValue<int>(dataAsPSObject, "MaxRunspaces");
		}

		// Token: 0x060022F5 RID: 8949 RVA: 0x000C535A File Offset: 0x000C355A
		internal static PSPrimitiveDictionary GetApplicationArguments(PSObject dataAsPSObject)
		{
			if (dataAsPSObject == null)
			{
				throw PSTraceSource.NewArgumentNullException("dataAsPSObject");
			}
			return RemotingDecoder.GetPropertyValue<PSPrimitiveDictionary>(dataAsPSObject, "ApplicationArguments");
		}

		// Token: 0x060022F6 RID: 8950 RVA: 0x000C5378 File Offset: 0x000C3578
		internal static RunspacePoolInitInfo GetRunspacePoolInitInfo(PSObject dataAsPSObject)
		{
			if (dataAsPSObject == null)
			{
				throw PSTraceSource.NewArgumentNullException("dataAsPSObject");
			}
			int propertyValue = RemotingDecoder.GetPropertyValue<int>(dataAsPSObject, "MaxRunspaces");
			int propertyValue2 = RemotingDecoder.GetPropertyValue<int>(dataAsPSObject, "MinRunspaces");
			return new RunspacePoolInitInfo(propertyValue2, propertyValue);
		}

		// Token: 0x060022F7 RID: 8951 RVA: 0x000C53B2 File Offset: 0x000C35B2
		internal static PSThreadOptions GetThreadOptions(PSObject dataAsPSObject)
		{
			if (dataAsPSObject == null)
			{
				throw PSTraceSource.NewArgumentNullException("dataAsPSObject");
			}
			return RemotingDecoder.GetPropertyValue<PSThreadOptions>(dataAsPSObject, "PSThreadOptions");
		}

		// Token: 0x060022F8 RID: 8952 RVA: 0x000C53D0 File Offset: 0x000C35D0
		internal static HostInfo GetHostInfo(PSObject dataAsPSObject)
		{
			if (dataAsPSObject == null)
			{
				throw PSTraceSource.NewArgumentNullException("dataAsPSObject");
			}
			PSObject propertyValue = RemotingDecoder.GetPropertyValue<PSObject>(dataAsPSObject, "HostInfo");
			return RemoteHostEncoder.DecodeObject(propertyValue, typeof(HostInfo)) as HostInfo;
		}

		// Token: 0x060022F9 RID: 8953 RVA: 0x000C540C File Offset: 0x000C360C
		private static Exception GetExceptionFromStateInfoObject(PSObject stateInfo)
		{
			PSPropertyInfo pspropertyInfo = stateInfo.Properties["ExceptionAsErrorRecord"];
			if (pspropertyInfo != null && pspropertyInfo.Value != null)
			{
				return RemotingDecoder.GetExceptionFromSerializedErrorRecord(pspropertyInfo.Value);
			}
			return null;
		}

		// Token: 0x060022FA RID: 8954 RVA: 0x000C5444 File Offset: 0x000C3644
		internal static Exception GetExceptionFromSerializedErrorRecord(object serializedErrorRecord)
		{
			ErrorRecord errorRecord = ErrorRecord.FromPSObjectForRemoting(PSObject.AsPSObject(serializedErrorRecord));
			if (errorRecord == null)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.DecodingErrorForErrorRecord);
			}
			return errorRecord.Exception;
		}

		// Token: 0x060022FB RID: 8955 RVA: 0x000C5471 File Offset: 0x000C3671
		internal static object GetPowerShellOutput(object data)
		{
			return data;
		}

		// Token: 0x060022FC RID: 8956 RVA: 0x000C5474 File Offset: 0x000C3674
		internal static PSInvocationStateInfo GetPowerShellStateInfo(object data)
		{
			PSObject psobject = data as PSObject;
			if (psobject == null)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.DecodingErrorForPowerShellStateInfo);
			}
			PSInvocationState propertyValue = RemotingDecoder.GetPropertyValue<PSInvocationState>(psobject, "PipelineState");
			Exception exceptionFromStateInfoObject = RemotingDecoder.GetExceptionFromStateInfoObject(psobject);
			return new PSInvocationStateInfo(propertyValue, exceptionFromStateInfoObject);
		}

		// Token: 0x060022FD RID: 8957 RVA: 0x000C54B0 File Offset: 0x000C36B0
		internal static ErrorRecord GetPowerShellError(object data)
		{
			if (data == null)
			{
				throw PSTraceSource.NewArgumentNullException("data");
			}
			PSObject serializedErrorRecord = data as PSObject;
			return ErrorRecord.FromPSObjectForRemoting(serializedErrorRecord);
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x000C54DA File Offset: 0x000C36DA
		internal static WarningRecord GetPowerShellWarning(object data)
		{
			if (data == null)
			{
				throw PSTraceSource.NewArgumentNullException("data");
			}
			return new WarningRecord((PSObject)data);
		}

		// Token: 0x060022FF RID: 8959 RVA: 0x000C54F5 File Offset: 0x000C36F5
		internal static VerboseRecord GetPowerShellVerbose(object data)
		{
			if (data == null)
			{
				throw PSTraceSource.NewArgumentNullException("data");
			}
			return new VerboseRecord((PSObject)data);
		}

		// Token: 0x06002300 RID: 8960 RVA: 0x000C5510 File Offset: 0x000C3710
		internal static DebugRecord GetPowerShellDebug(object data)
		{
			if (data == null)
			{
				throw PSTraceSource.NewArgumentNullException("data");
			}
			return new DebugRecord((PSObject)data);
		}

		// Token: 0x06002301 RID: 8961 RVA: 0x000C552C File Offset: 0x000C372C
		internal static ProgressRecord GetPowerShellProgress(object data)
		{
			PSObject psobject = PSObject.AsPSObject(data);
			if (psobject == null)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.CantCastRemotingDataToPSObject, new object[]
				{
					data.GetType().FullName
				});
			}
			return ProgressRecord.FromPSObjectForRemoting(psobject);
		}

		// Token: 0x06002302 RID: 8962 RVA: 0x000C556C File Offset: 0x000C376C
		internal static InformationRecord GetPowerShellInformation(object data)
		{
			PSObject psobject = PSObject.AsPSObject(data);
			if (psobject == null)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.CantCastRemotingDataToPSObject, new object[]
				{
					data.GetType().FullName
				});
			}
			return InformationRecord.FromPSObjectForRemoting(psobject);
		}

		// Token: 0x06002303 RID: 8963 RVA: 0x000C55AC File Offset: 0x000C37AC
		internal static PowerShell GetPowerShell(object data)
		{
			PSObject psobject = PSObject.AsPSObject(data);
			if (psobject == null)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.CantCastRemotingDataToPSObject, new object[]
				{
					data.GetType().FullName
				});
			}
			PSObject propertyValue = RemotingDecoder.GetPropertyValue<PSObject>(psobject, "PowerShell");
			return PowerShell.FromPSObjectForRemoting(propertyValue);
		}

		// Token: 0x06002304 RID: 8964 RVA: 0x000C55F8 File Offset: 0x000C37F8
		internal static PowerShell GetCommandDiscoveryPipeline(object data)
		{
			PSObject psobject = PSObject.AsPSObject(data);
			if (psobject == null)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.CantCastRemotingDataToPSObject, new object[]
				{
					data.GetType().FullName
				});
			}
			CommandTypes propertyValue = RemotingDecoder.GetPropertyValue<CommandTypes>(psobject, "CommandType");
			string[] value;
			if (RemotingDecoder.GetPropertyValue<PSObject>(psobject, "Name") != null)
			{
				IEnumerable<string> collection = RemotingDecoder.EnumerateListProperty<string>(psobject, "Name");
				value = new List<string>(collection).ToArray();
			}
			else
			{
				value = new string[]
				{
					"*"
				};
			}
			string[] value2;
			if (RemotingDecoder.GetPropertyValue<PSObject>(psobject, "Namespace") != null)
			{
				IEnumerable<string> collection2 = RemotingDecoder.EnumerateListProperty<string>(psobject, "Namespace");
				value2 = new List<string>(collection2).ToArray();
			}
			else
			{
				value2 = new string[]
				{
					""
				};
			}
			ModuleSpecification[] array = null;
			if (DeserializingTypeConverter.GetPropertyValue<PSObject>(psobject, "FullyQualifiedModule", DeserializingTypeConverter.RehydrationFlags.NullValueOk | DeserializingTypeConverter.RehydrationFlags.MissingPropertyOk) != null)
			{
				IEnumerable<ModuleSpecification> collection3 = RemotingDecoder.EnumerateListProperty<ModuleSpecification>(psobject, "FullyQualifiedModule");
				array = new List<ModuleSpecification>(collection3).ToArray();
			}
			object[] value3;
			if (RemotingDecoder.GetPropertyValue<PSObject>(psobject, "ArgumentList") != null)
			{
				IEnumerable<object> collection4 = RemotingDecoder.EnumerateListProperty<object>(psobject, "ArgumentList");
				value3 = new List<object>(collection4).ToArray();
			}
			else
			{
				value3 = null;
			}
			PowerShell powerShell = PowerShell.Create();
			powerShell.AddCommand("Get-Command");
			powerShell.AddParameter("Name", value);
			powerShell.AddParameter("CommandType", propertyValue);
			if (array != null)
			{
				powerShell.AddParameter("FullyQualifiedModule", array);
			}
			else
			{
				powerShell.AddParameter("Module", value2);
			}
			powerShell.AddParameter("ArgumentList", value3);
			return powerShell;
		}

		// Token: 0x06002305 RID: 8965 RVA: 0x000C577C File Offset: 0x000C397C
		internal static bool GetNoInput(object data)
		{
			PSObject psobject = PSObject.AsPSObject(data);
			if (psobject == null)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.CantCastRemotingDataToPSObject, new object[]
				{
					data.GetType().FullName
				});
			}
			return RemotingDecoder.GetPropertyValue<bool>(psobject, "NoInput");
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x000C57C0 File Offset: 0x000C39C0
		internal static bool GetAddToHistory(object data)
		{
			PSObject psobject = PSObject.AsPSObject(data);
			if (psobject == null)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.CantCastRemotingDataToPSObject, new object[]
				{
					data.GetType().FullName
				});
			}
			return RemotingDecoder.GetPropertyValue<bool>(psobject, "AddToHistory");
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x000C5804 File Offset: 0x000C3A04
		internal static bool GetIsNested(object data)
		{
			PSObject psobject = PSObject.AsPSObject(data);
			if (psobject == null)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.CantCastRemotingDataToPSObject, new object[]
				{
					data.GetType().FullName
				});
			}
			return RemotingDecoder.GetPropertyValue<bool>(psobject, "IsNested");
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x000C5848 File Offset: 0x000C3A48
		internal static ApartmentState GetApartmentState(object data)
		{
			PSObject psObject = PSObject.AsPSObject(data);
			return RemotingDecoder.GetPropertyValue<ApartmentState>(psObject, "ApartmentState");
		}

		// Token: 0x06002309 RID: 8969 RVA: 0x000C5868 File Offset: 0x000C3A68
		internal static RemoteStreamOptions GetRemoteStreamOptions(object data)
		{
			PSObject psObject = PSObject.AsPSObject(data);
			return RemotingDecoder.GetPropertyValue<RemoteStreamOptions>(psObject, "RemoteStreamOptions");
		}

		// Token: 0x0600230A RID: 8970 RVA: 0x000C5888 File Offset: 0x000C3A88
		internal static RemoteSessionCapability GetSessionCapability(object data)
		{
			PSObject psobject = data as PSObject;
			if (psobject == null)
			{
				throw new PSRemotingDataStructureException(RemotingErrorIdStrings.CantCastRemotingDataToPSObject, new object[]
				{
					data.GetType().FullName
				});
			}
			Version propertyValue = RemotingDecoder.GetPropertyValue<Version>(psobject, "protocolversion");
			Version propertyValue2 = RemotingDecoder.GetPropertyValue<Version>(psobject, "PSVersion");
			Version propertyValue3 = RemotingDecoder.GetPropertyValue<Version>(psobject, "SerializationVersion");
			RemoteSessionCapability remoteSessionCapability = new RemoteSessionCapability(RemotingDestination.InvalidDestination, propertyValue, propertyValue2, propertyValue3);
			if (psobject.Properties["TimeZone"] != null)
			{
				remoteSessionCapability.TimeZone = TimeZone.CurrentTimeZone;
			}
			return remoteSessionCapability;
		}

		// Token: 0x0600230B RID: 8971 RVA: 0x000C5912 File Offset: 0x000C3B12
		internal static bool ServerSupportsBatchInvocation(Runspace runspace)
		{
			return runspace != null && runspace.RunspaceStateInfo.State != RunspaceState.BeforeOpen && runspace.GetRemoteProtocolVersion() >= RemotingConstants.ProtocolVersionWin8RTM;
		}
	}
}
