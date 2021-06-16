using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using Microsoft.PowerShell.Commands;

namespace Microsoft.PowerShell
{
	// Token: 0x0200045E RID: 1118
	public sealed class DeserializingTypeConverter : PSTypeConverter
	{
		// Token: 0x060030F7 RID: 12535 RVA: 0x0010B204 File Offset: 0x00109404
		static DeserializingTypeConverter()
		{
			DeserializingTypeConverter.converter.Add(typeof(PSPrimitiveDictionary), new Func<PSObject, object>(DeserializingTypeConverter.RehydratePrimitiveHashtable));
			DeserializingTypeConverter.converter.Add(typeof(SwitchParameter), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateSwitchParameter));
			DeserializingTypeConverter.converter.Add(typeof(PSListModifier), new Func<PSObject, object>(DeserializingTypeConverter.RehydratePSListModifier));
			DeserializingTypeConverter.converter.Add(typeof(PSCredential), new Func<PSObject, object>(DeserializingTypeConverter.RehydratePSCredential));
			DeserializingTypeConverter.converter.Add(typeof(PSSenderInfo), new Func<PSObject, object>(DeserializingTypeConverter.RehydratePSSenderInfo));
			DeserializingTypeConverter.converter.Add(typeof(CultureInfo), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateCultureInfo));
			DeserializingTypeConverter.converter.Add(typeof(ParameterSetMetadata), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateParameterSetMetadata));
			DeserializingTypeConverter.converter.Add(typeof(X509Certificate2), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateX509Certificate2));
			DeserializingTypeConverter.converter.Add(typeof(X500DistinguishedName), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateX500DistinguishedName));
			DeserializingTypeConverter.converter.Add(typeof(IPAddress), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateIPAddress));
			DeserializingTypeConverter.converter.Add(typeof(MailAddress), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateMailAddress));
			DeserializingTypeConverter.converter.Add(typeof(DirectorySecurity), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateObjectSecurity<DirectorySecurity>));
			DeserializingTypeConverter.converter.Add(typeof(FileSecurity), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateObjectSecurity<FileSecurity>));
			DeserializingTypeConverter.converter.Add(typeof(RegistrySecurity), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateObjectSecurity<RegistrySecurity>));
			DeserializingTypeConverter.converter.Add(typeof(ExtendedTypeDefinition), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateExtendedTypeDefinition));
			DeserializingTypeConverter.converter.Add(typeof(FormatViewDefinition), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateFormatViewDefinition));
			DeserializingTypeConverter.converter.Add(typeof(PSControl), new Func<PSObject, object>(DeserializingTypeConverter.RehydratePSControl));
			DeserializingTypeConverter.converter.Add(typeof(DisplayEntry), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateDisplayEntry));
			DeserializingTypeConverter.converter.Add(typeof(TableControlColumnHeader), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateTableControlColumnHeader));
			DeserializingTypeConverter.converter.Add(typeof(TableControlRow), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateTableControlRow));
			DeserializingTypeConverter.converter.Add(typeof(TableControlColumn), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateTableControlColumn));
			DeserializingTypeConverter.converter.Add(typeof(ListControlEntry), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateListControlEntry));
			DeserializingTypeConverter.converter.Add(typeof(ListControlEntryItem), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateListControlEntryItem));
			DeserializingTypeConverter.converter.Add(typeof(WideControlEntryItem), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateWideControlEntryItem));
			DeserializingTypeConverter.converter.Add(typeof(CompletionResult), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateCompletionResult));
			DeserializingTypeConverter.converter.Add(typeof(ModuleSpecification), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateModuleSpecification));
			DeserializingTypeConverter.converter.Add(typeof(CommandCompletion), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateCommandCompletion));
			DeserializingTypeConverter.converter.Add(typeof(JobStateInfo), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateJobStateInfo));
			DeserializingTypeConverter.converter.Add(typeof(JobStateEventArgs), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateJobStateEventArgs));
			DeserializingTypeConverter.converter.Add(typeof(PSSessionOption), new Func<PSObject, object>(DeserializingTypeConverter.RehydratePSSessionOption));
			DeserializingTypeConverter.converter.Add(typeof(LineBreakpoint), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateLineBreakpoint));
			DeserializingTypeConverter.converter.Add(typeof(CommandBreakpoint), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateCommandBreakpoint));
			DeserializingTypeConverter.converter.Add(typeof(VariableBreakpoint), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateVariableBreakpoint));
			DeserializingTypeConverter.converter.Add(typeof(BreakpointUpdatedEventArgs), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateBreakpointUpdatedEventArgs));
			DeserializingTypeConverter.converter.Add(typeof(DebuggerCommand), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateDebuggerCommand));
			DeserializingTypeConverter.converter.Add(typeof(DebuggerCommandResults), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateDebuggerCommandResults));
			DeserializingTypeConverter.converter.Add(typeof(DebuggerStopEventArgs), new Func<PSObject, object>(DeserializingTypeConverter.RehydrateDebuggerStopEventArgs));
		}

		// Token: 0x060030F8 RID: 12536 RVA: 0x0010B6BC File Offset: 0x001098BC
		public override bool CanConvertFrom(PSObject sourceValue, Type destinationType)
		{
			foreach (Type type in DeserializingTypeConverter.converter.Keys)
			{
				if (Deserializer.IsDeserializedInstanceOfType(sourceValue, type))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060030F9 RID: 12537 RVA: 0x0010B71C File Offset: 0x0010991C
		public override object ConvertFrom(PSObject sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase)
		{
			if (destinationType == null)
			{
				throw PSTraceSource.NewArgumentNullException("destinationType");
			}
			if (sourceValue == null)
			{
				throw new PSInvalidCastException("InvalidCastWhenRehydratingFromNull", PSTraceSource.NewArgumentNullException("sourceValue"), ExtendedTypeSystem.InvalidCastFromNull, new object[]
				{
					destinationType.ToString()
				});
			}
			foreach (KeyValuePair<Type, Func<PSObject, object>> keyValuePair in DeserializingTypeConverter.converter)
			{
				Type key = keyValuePair.Key;
				Func<PSObject, object> value = keyValuePair.Value;
				if (Deserializer.IsDeserializedInstanceOfType(sourceValue, key))
				{
					return DeserializingTypeConverter.ConvertFrom(sourceValue, value);
				}
			}
			throw new PSInvalidCastException("InvalidCastEnumFromTypeNotAString", null, ExtendedTypeSystem.InvalidCastException, new object[]
			{
				sourceValue,
				destinationType
			});
		}

		// Token: 0x060030FA RID: 12538 RVA: 0x0010B7F8 File Offset: 0x001099F8
		private static object ConvertFrom(PSObject o, Func<PSObject, object> converter)
		{
			object obj = converter(o);
			bool flag = false;
			PSObject psobject = PSObject.AsPSObject(obj);
			foreach (PSMemberInfo psmemberInfo in o.InstanceMembers)
			{
				if (psmemberInfo.MemberType == (psmemberInfo.MemberType & (PSMemberTypes.AliasProperty | PSMemberTypes.CodeProperty | PSMemberTypes.Property | PSMemberTypes.NoteProperty | PSMemberTypes.ScriptProperty | PSMemberTypes.PropertySet | PSMemberTypes.MemberSet)) && psobject.Members[psmemberInfo.Name] == null)
				{
					psobject.InstanceMembers.Add(psmemberInfo);
					flag = true;
				}
			}
			if (flag)
			{
				return psobject;
			}
			return obj;
		}

		// Token: 0x060030FB RID: 12539 RVA: 0x0010B898 File Offset: 0x00109A98
		public override bool CanConvertTo(object sourceValue, Type destinationType)
		{
			return false;
		}

		// Token: 0x060030FC RID: 12540 RVA: 0x0010B89B File Offset: 0x00109A9B
		public override object ConvertTo(object sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase)
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x060030FD RID: 12541 RVA: 0x0010B8A2 File Offset: 0x00109AA2
		public override bool CanConvertFrom(object sourceValue, Type destinationType)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060030FE RID: 12542 RVA: 0x0010B8A9 File Offset: 0x00109AA9
		public override bool CanConvertTo(PSObject sourceValue, Type destinationType)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060030FF RID: 12543 RVA: 0x0010B8B0 File Offset: 0x00109AB0
		public override object ConvertFrom(object sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06003100 RID: 12544 RVA: 0x0010B8B7 File Offset: 0x00109AB7
		public override object ConvertTo(PSObject sourceValue, Type destinationType, IFormatProvider formatProvider, bool ignoreCase)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06003101 RID: 12545 RVA: 0x0010B8BE File Offset: 0x00109ABE
		private static T GetPropertyValue<T>(PSObject pso, string propertyName)
		{
			return DeserializingTypeConverter.GetPropertyValue<T>(pso, propertyName, DeserializingTypeConverter.RehydrationFlags.NullValueBad);
		}

		// Token: 0x06003102 RID: 12546 RVA: 0x0010B8C8 File Offset: 0x00109AC8
		internal static T GetPropertyValue<T>(PSObject pso, string propertyName, DeserializingTypeConverter.RehydrationFlags flags)
		{
			PSPropertyInfo pspropertyInfo = pso.Properties[propertyName];
			if (pspropertyInfo == null && DeserializingTypeConverter.RehydrationFlags.MissingPropertyOk == (flags & DeserializingTypeConverter.RehydrationFlags.MissingPropertyOk))
			{
				return default(T);
			}
			object value = pspropertyInfo.Value;
			if (value == null && DeserializingTypeConverter.RehydrationFlags.NullValueOk == (flags & DeserializingTypeConverter.RehydrationFlags.NullValueOk))
			{
				return default(T);
			}
			return (T)((object)LanguagePrimitives.ConvertTo(value, typeof(T), CultureInfo.InvariantCulture));
		}

		// Token: 0x06003103 RID: 12547 RVA: 0x0010B92C File Offset: 0x00109B2C
		private static ListType RehydrateList<ListType, ItemType>(PSObject pso, string propertyName, DeserializingTypeConverter.RehydrationFlags flags) where ListType : IList, new()
		{
			ArrayList propertyValue = DeserializingTypeConverter.GetPropertyValue<ArrayList>(pso, propertyName, flags);
			if (propertyValue != null)
			{
				ListType result = (default(ListType) == null) ? Activator.CreateInstance<ListType>() : default(ListType);
				foreach (object valueToConvert in propertyValue)
				{
					ItemType itemType = (ItemType)((object)LanguagePrimitives.ConvertTo(valueToConvert, typeof(ItemType), CultureInfo.InvariantCulture));
					result.Add(itemType);
				}
				return result;
			}
			if (DeserializingTypeConverter.RehydrationFlags.NullValueMeansEmptyList != (flags & DeserializingTypeConverter.RehydrationFlags.NullValueMeansEmptyList))
			{
				return default(ListType);
			}
			if (default(ListType) != null)
			{
				return default(ListType);
			}
			return Activator.CreateInstance<ListType>();
		}

		// Token: 0x06003104 RID: 12548 RVA: 0x0010BA10 File Offset: 0x00109C10
		private static object RehydratePrimitiveHashtable(PSObject pso)
		{
			Hashtable other = (Hashtable)LanguagePrimitives.ConvertTo(pso, typeof(Hashtable), CultureInfo.InvariantCulture);
			return new PSPrimitiveDictionary(other);
		}

		// Token: 0x06003105 RID: 12549 RVA: 0x0010BA3E File Offset: 0x00109C3E
		private static object RehydrateSwitchParameter(PSObject pso)
		{
			return DeserializingTypeConverter.GetPropertyValue<SwitchParameter>(pso, "IsPresent");
		}

		// Token: 0x06003106 RID: 12550 RVA: 0x0010BA50 File Offset: 0x00109C50
		private static CultureInfo RehydrateCultureInfo(PSObject pso)
		{
			string name = pso.ToString();
			return new CultureInfo(name);
		}

		// Token: 0x06003107 RID: 12551 RVA: 0x0010BA6C File Offset: 0x00109C6C
		private static PSListModifier RehydratePSListModifier(PSObject pso)
		{
			Hashtable hashtable = new Hashtable();
			PSPropertyInfo pspropertyInfo = pso.Properties["Add"];
			if (pspropertyInfo != null && pspropertyInfo.Value != null)
			{
				hashtable.Add("Add", pspropertyInfo.Value);
			}
			PSPropertyInfo pspropertyInfo2 = pso.Properties["Remove"];
			if (pspropertyInfo2 != null && pspropertyInfo2.Value != null)
			{
				hashtable.Add("Remove", pspropertyInfo2.Value);
			}
			PSPropertyInfo pspropertyInfo3 = pso.Properties["Replace"];
			if (pspropertyInfo3 != null && pspropertyInfo3.Value != null)
			{
				hashtable.Add("Replace", pspropertyInfo3.Value);
			}
			return new PSListModifier(hashtable);
		}

		// Token: 0x06003108 RID: 12552 RVA: 0x0010BB0C File Offset: 0x00109D0C
		private static CompletionResult RehydrateCompletionResult(PSObject pso)
		{
			string propertyValue = DeserializingTypeConverter.GetPropertyValue<string>(pso, "CompletionText");
			string propertyValue2 = DeserializingTypeConverter.GetPropertyValue<string>(pso, "ListItemText");
			string propertyValue3 = DeserializingTypeConverter.GetPropertyValue<string>(pso, "ToolTip");
			CompletionResultType propertyValue4 = DeserializingTypeConverter.GetPropertyValue<CompletionResultType>(pso, "ResultType");
			return new CompletionResult(propertyValue, propertyValue2, propertyValue4, propertyValue3);
		}

		// Token: 0x06003109 RID: 12553 RVA: 0x0010BB54 File Offset: 0x00109D54
		private static ModuleSpecification RehydrateModuleSpecification(PSObject pso)
		{
			return new ModuleSpecification
			{
				Name = DeserializingTypeConverter.GetPropertyValue<string>(pso, "Name"),
				Guid = DeserializingTypeConverter.GetPropertyValue<Guid?>(pso, "Guid"),
				Version = DeserializingTypeConverter.GetPropertyValue<Version>(pso, "Version"),
				MaximumVersion = DeserializingTypeConverter.GetPropertyValue<string>(pso, "MaximumVersion"),
				RequiredVersion = DeserializingTypeConverter.GetPropertyValue<Version>(pso, "RequiredVersion")
			};
		}

		// Token: 0x0600310A RID: 12554 RVA: 0x0010BBC0 File Offset: 0x00109DC0
		private static CommandCompletion RehydrateCommandCompletion(PSObject pso)
		{
			Collection<CompletionResult> collection = new Collection<CompletionResult>();
			foreach (object obj in DeserializingTypeConverter.GetPropertyValue<ArrayList>(pso, "CompletionMatches"))
			{
				collection.Add((CompletionResult)obj);
			}
			int propertyValue = DeserializingTypeConverter.GetPropertyValue<int>(pso, "CurrentMatchIndex");
			int propertyValue2 = DeserializingTypeConverter.GetPropertyValue<int>(pso, "ReplacementIndex");
			int propertyValue3 = DeserializingTypeConverter.GetPropertyValue<int>(pso, "ReplacementLength");
			return new CommandCompletion(collection, propertyValue, propertyValue2, propertyValue3);
		}

		// Token: 0x0600310B RID: 12555 RVA: 0x0010BC5C File Offset: 0x00109E5C
		private static JobStateInfo RehydrateJobStateInfo(PSObject pso)
		{
			JobState propertyValue = DeserializingTypeConverter.GetPropertyValue<JobState>(pso, "State");
			Exception reason = null;
			object obj = null;
			PSPropertyInfo pspropertyInfo = pso.Properties["Reason"];
			string text = string.Empty;
			if (pspropertyInfo != null)
			{
				obj = pspropertyInfo.Value;
			}
			if (obj != null)
			{
				if (Deserializer.IsDeserializedInstanceOfType(obj, typeof(Exception)))
				{
					text = (PSObject.AsPSObject(obj).Properties["Message"].Value as string);
				}
				else if (obj is Exception)
				{
					reason = (Exception)obj;
				}
				else
				{
					text = obj.ToString();
				}
				if (!string.IsNullOrEmpty(text))
				{
					try
					{
						reason = (Exception)LanguagePrimitives.ConvertTo(text, typeof(Exception), CultureInfo.InvariantCulture);
					}
					catch (Exception)
					{
						reason = null;
					}
				}
			}
			return new JobStateInfo(propertyValue, reason);
		}

		// Token: 0x0600310C RID: 12556 RVA: 0x0010BD30 File Offset: 0x00109F30
		internal static JobStateEventArgs RehydrateJobStateEventArgs(PSObject pso)
		{
			JobStateInfo jobStateInfo = DeserializingTypeConverter.RehydrateJobStateInfo(PSObject.AsPSObject(pso.Properties["JobStateInfo"].Value));
			JobStateInfo previousJobStateInfo = null;
			PSPropertyInfo pspropertyInfo = pso.Properties["PreviousJobStateInfo"];
			if (pspropertyInfo != null && pspropertyInfo.Value != null)
			{
				previousJobStateInfo = DeserializingTypeConverter.RehydrateJobStateInfo(PSObject.AsPSObject(pspropertyInfo.Value));
			}
			return new JobStateEventArgs(jobStateInfo, previousJobStateInfo);
		}

		// Token: 0x0600310D RID: 12557 RVA: 0x0010BD94 File Offset: 0x00109F94
		internal static PSSessionOption RehydratePSSessionOption(PSObject pso)
		{
			return new PSSessionOption
			{
				ApplicationArguments = DeserializingTypeConverter.GetPropertyValue<PSPrimitiveDictionary>(pso, "ApplicationArguments"),
				CancelTimeout = DeserializingTypeConverter.GetPropertyValue<TimeSpan>(pso, "CancelTimeout"),
				Culture = DeserializingTypeConverter.GetPropertyValue<CultureInfo>(pso, "Culture"),
				IdleTimeout = DeserializingTypeConverter.GetPropertyValue<TimeSpan>(pso, "IdleTimeout"),
				MaximumConnectionRedirectionCount = DeserializingTypeConverter.GetPropertyValue<int>(pso, "MaximumConnectionRedirectionCount"),
				MaximumReceivedDataSizePerCommand = DeserializingTypeConverter.GetPropertyValue<int?>(pso, "MaximumReceivedDataSizePerCommand"),
				MaximumReceivedObjectSize = DeserializingTypeConverter.GetPropertyValue<int?>(pso, "MaximumReceivedObjectSize"),
				NoCompression = DeserializingTypeConverter.GetPropertyValue<bool>(pso, "NoCompression"),
				NoEncryption = DeserializingTypeConverter.GetPropertyValue<bool>(pso, "NoEncryption"),
				NoMachineProfile = DeserializingTypeConverter.GetPropertyValue<bool>(pso, "NoMachineProfile"),
				OpenTimeout = DeserializingTypeConverter.GetPropertyValue<TimeSpan>(pso, "OpenTimeout"),
				OperationTimeout = DeserializingTypeConverter.GetPropertyValue<TimeSpan>(pso, "OperationTimeout"),
				OutputBufferingMode = DeserializingTypeConverter.GetPropertyValue<OutputBufferingMode>(pso, "OutputBufferingMode"),
				MaxConnectionRetryCount = DeserializingTypeConverter.GetPropertyValue<int>(pso, "MaxConnectionRetryCount"),
				ProxyAccessType = DeserializingTypeConverter.GetPropertyValue<ProxyAccessType>(pso, "ProxyAccessType"),
				ProxyAuthentication = DeserializingTypeConverter.GetPropertyValue<AuthenticationMechanism>(pso, "ProxyAuthentication"),
				ProxyCredential = DeserializingTypeConverter.GetPropertyValue<PSCredential>(pso, "ProxyCredential"),
				SkipCACheck = DeserializingTypeConverter.GetPropertyValue<bool>(pso, "SkipCACheck"),
				SkipCNCheck = DeserializingTypeConverter.GetPropertyValue<bool>(pso, "SkipCNCheck"),
				SkipRevocationCheck = DeserializingTypeConverter.GetPropertyValue<bool>(pso, "SkipRevocationCheck"),
				UICulture = DeserializingTypeConverter.GetPropertyValue<CultureInfo>(pso, "UICulture"),
				UseUTF16 = DeserializingTypeConverter.GetPropertyValue<bool>(pso, "UseUTF16"),
				IncludePortInSPN = DeserializingTypeConverter.GetPropertyValue<bool>(pso, "IncludePortInSPN")
			};
		}

		// Token: 0x0600310E RID: 12558 RVA: 0x0010BF30 File Offset: 0x0010A130
		internal static LineBreakpoint RehydrateLineBreakpoint(PSObject pso)
		{
			string propertyValue = DeserializingTypeConverter.GetPropertyValue<string>(pso, "Script");
			int propertyValue2 = DeserializingTypeConverter.GetPropertyValue<int>(pso, "Line");
			int propertyValue3 = DeserializingTypeConverter.GetPropertyValue<int>(pso, "Column");
			int propertyValue4 = DeserializingTypeConverter.GetPropertyValue<int>(pso, "Id");
			bool propertyValue5 = DeserializingTypeConverter.GetPropertyValue<bool>(pso, "Enabled");
			ScriptBlock action = DeserializingTypeConverter.RehydrateScriptBlock(DeserializingTypeConverter.GetPropertyValue<string>(pso, "Action", DeserializingTypeConverter.RehydrationFlags.MissingPropertyOk));
			LineBreakpoint lineBreakpoint = new LineBreakpoint(propertyValue, propertyValue2, propertyValue3, action, propertyValue4);
			lineBreakpoint.SetEnabled(propertyValue5);
			return lineBreakpoint;
		}

		// Token: 0x0600310F RID: 12559 RVA: 0x0010BFA8 File Offset: 0x0010A1A8
		internal static CommandBreakpoint RehydrateCommandBreakpoint(PSObject pso)
		{
			string propertyValue = DeserializingTypeConverter.GetPropertyValue<string>(pso, "Script", DeserializingTypeConverter.RehydrationFlags.MissingPropertyOk);
			string propertyValue2 = DeserializingTypeConverter.GetPropertyValue<string>(pso, "Command");
			int propertyValue3 = DeserializingTypeConverter.GetPropertyValue<int>(pso, "Id");
			bool propertyValue4 = DeserializingTypeConverter.GetPropertyValue<bool>(pso, "Enabled");
			WildcardPattern command = new WildcardPattern(propertyValue2, WildcardOptions.Compiled | WildcardOptions.IgnoreCase);
			ScriptBlock action = DeserializingTypeConverter.RehydrateScriptBlock(DeserializingTypeConverter.GetPropertyValue<string>(pso, "Action", DeserializingTypeConverter.RehydrationFlags.MissingPropertyOk));
			CommandBreakpoint commandBreakpoint = new CommandBreakpoint(propertyValue, command, propertyValue2, action, propertyValue3);
			commandBreakpoint.SetEnabled(propertyValue4);
			return commandBreakpoint;
		}

		// Token: 0x06003110 RID: 12560 RVA: 0x0010C01C File Offset: 0x0010A21C
		internal static VariableBreakpoint RehydrateVariableBreakpoint(PSObject pso)
		{
			string propertyValue = DeserializingTypeConverter.GetPropertyValue<string>(pso, "Script", DeserializingTypeConverter.RehydrationFlags.MissingPropertyOk);
			string propertyValue2 = DeserializingTypeConverter.GetPropertyValue<string>(pso, "Variable");
			int propertyValue3 = DeserializingTypeConverter.GetPropertyValue<int>(pso, "Id");
			bool propertyValue4 = DeserializingTypeConverter.GetPropertyValue<bool>(pso, "Enabled");
			VariableAccessMode propertyValue5 = DeserializingTypeConverter.GetPropertyValue<VariableAccessMode>(pso, "AccessMode");
			ScriptBlock action = DeserializingTypeConverter.RehydrateScriptBlock(DeserializingTypeConverter.GetPropertyValue<string>(pso, "Action", DeserializingTypeConverter.RehydrationFlags.MissingPropertyOk));
			VariableBreakpoint variableBreakpoint = new VariableBreakpoint(propertyValue, propertyValue2, propertyValue5, action, propertyValue3);
			variableBreakpoint.SetEnabled(propertyValue4);
			return variableBreakpoint;
		}

		// Token: 0x06003111 RID: 12561 RVA: 0x0010C094 File Offset: 0x0010A294
		internal static BreakpointUpdatedEventArgs RehydrateBreakpointUpdatedEventArgs(PSObject pso)
		{
			Breakpoint propertyValue = DeserializingTypeConverter.GetPropertyValue<Breakpoint>(pso, "Breakpoint");
			BreakpointUpdateType propertyValue2 = DeserializingTypeConverter.GetPropertyValue<BreakpointUpdateType>(pso, "UpdateType");
			int propertyValue3 = DeserializingTypeConverter.GetPropertyValue<int>(pso, "BreakpointCount");
			return new BreakpointUpdatedEventArgs(propertyValue, propertyValue2, propertyValue3);
		}

		// Token: 0x06003112 RID: 12562 RVA: 0x0010C0D0 File Offset: 0x0010A2D0
		internal static DebuggerCommand RehydrateDebuggerCommand(PSObject pso)
		{
			string propertyValue = DeserializingTypeConverter.GetPropertyValue<string>(pso, "Command");
			bool propertyValue2 = DeserializingTypeConverter.GetPropertyValue<bool>(pso, "RepeatOnEnter");
			bool propertyValue3 = DeserializingTypeConverter.GetPropertyValue<bool>(pso, "ExecutedByDebugger");
			DebuggerResumeAction? propertyValue4 = DeserializingTypeConverter.GetPropertyValue<DebuggerResumeAction?>(pso, "ResumeAction", DeserializingTypeConverter.RehydrationFlags.NullValueOk);
			return new DebuggerCommand(propertyValue, propertyValue4, propertyValue2, propertyValue3);
		}

		// Token: 0x06003113 RID: 12563 RVA: 0x0010C118 File Offset: 0x0010A318
		internal static DebuggerCommandResults RehydrateDebuggerCommandResults(PSObject pso)
		{
			DebuggerResumeAction? propertyValue = DeserializingTypeConverter.GetPropertyValue<DebuggerResumeAction?>(pso, "ResumeAction", DeserializingTypeConverter.RehydrationFlags.NullValueOk);
			bool propertyValue2 = DeserializingTypeConverter.GetPropertyValue<bool>(pso, "EvaluatedByDebugger");
			return new DebuggerCommandResults(propertyValue, propertyValue2);
		}

		// Token: 0x06003114 RID: 12564 RVA: 0x0010C148 File Offset: 0x0010A348
		internal static DebuggerStopEventArgs RehydrateDebuggerStopEventArgs(PSObject pso)
		{
			PSObject propertyValue = DeserializingTypeConverter.GetPropertyValue<PSObject>(pso, "SerializedInvocationInfo", DeserializingTypeConverter.RehydrationFlags.NullValueOk | DeserializingTypeConverter.RehydrationFlags.MissingPropertyOk);
			InvocationInfo invocationInfo = (propertyValue != null) ? new InvocationInfo(propertyValue) : null;
			DebuggerResumeAction propertyValue2 = DeserializingTypeConverter.GetPropertyValue<DebuggerResumeAction>(pso, "ResumeAction");
			Collection<Breakpoint> collection = new Collection<Breakpoint>();
			foreach (object obj in DeserializingTypeConverter.GetPropertyValue<ArrayList>(pso, "Breakpoints"))
			{
				Breakpoint breakpoint = obj as Breakpoint;
				if (breakpoint != null)
				{
					collection.Add(breakpoint);
				}
			}
			return new DebuggerStopEventArgs(invocationInfo, collection, propertyValue2);
		}

		// Token: 0x06003115 RID: 12565 RVA: 0x0010C1EC File Offset: 0x0010A3EC
		private static ScriptBlock RehydrateScriptBlock(string script)
		{
			if (!string.IsNullOrEmpty(script))
			{
				return ScriptBlock.Create(script);
			}
			return null;
		}

		// Token: 0x06003116 RID: 12566 RVA: 0x0010C200 File Offset: 0x0010A400
		private static PSCredential RehydratePSCredential(PSObject pso)
		{
			string propertyValue = DeserializingTypeConverter.GetPropertyValue<string>(pso, "UserName");
			SecureString propertyValue2 = DeserializingTypeConverter.GetPropertyValue<SecureString>(pso, "Password");
			if (string.IsNullOrEmpty(propertyValue))
			{
				return PSCredential.Empty;
			}
			return new PSCredential(propertyValue, propertyValue2);
		}

		// Token: 0x06003117 RID: 12567 RVA: 0x0010C23C File Offset: 0x0010A43C
		internal static PSSenderInfo RehydratePSSenderInfo(PSObject pso)
		{
			PSObject propertyValue = DeserializingTypeConverter.GetPropertyValue<PSObject>(pso, "UserInfo");
			PSObject propertyValue2 = DeserializingTypeConverter.GetPropertyValue<PSObject>(propertyValue, "Identity");
			PSObject propertyValue3 = DeserializingTypeConverter.GetPropertyValue<PSObject>(propertyValue2, "CertificateDetails");
			PSCertificateDetails cert = (propertyValue3 == null) ? null : new PSCertificateDetails(DeserializingTypeConverter.GetPropertyValue<string>(propertyValue3, "Subject"), DeserializingTypeConverter.GetPropertyValue<string>(propertyValue3, "IssuerName"), DeserializingTypeConverter.GetPropertyValue<string>(propertyValue3, "IssuerThumbprint"));
			PSIdentity identity = new PSIdentity(DeserializingTypeConverter.GetPropertyValue<string>(propertyValue2, "AuthenticationType"), DeserializingTypeConverter.GetPropertyValue<bool>(propertyValue2, "IsAuthenticated"), DeserializingTypeConverter.GetPropertyValue<string>(propertyValue2, "Name"), cert);
			PSPrincipal userPrincipal = new PSPrincipal(identity, WindowsIdentity.GetCurrent());
			return new PSSenderInfo(userPrincipal, DeserializingTypeConverter.GetPropertyValue<string>(pso, "ConnectionString"))
			{
				ClientTimeZone = TimeZone.CurrentTimeZone,
				ApplicationArguments = DeserializingTypeConverter.GetPropertyValue<PSPrimitiveDictionary>(pso, "ApplicationArguments")
			};
		}

		// Token: 0x06003118 RID: 12568 RVA: 0x0010C308 File Offset: 0x0010A508
		private static X509Certificate2 RehydrateX509Certificate2(PSObject pso)
		{
			byte[] propertyValue = DeserializingTypeConverter.GetPropertyValue<byte[]>(pso, "RawData");
			return new X509Certificate2(propertyValue);
		}

		// Token: 0x06003119 RID: 12569 RVA: 0x0010C328 File Offset: 0x0010A528
		private static X500DistinguishedName RehydrateX500DistinguishedName(PSObject pso)
		{
			byte[] propertyValue = DeserializingTypeConverter.GetPropertyValue<byte[]>(pso, "RawData");
			return new X500DistinguishedName(propertyValue);
		}

		// Token: 0x0600311A RID: 12570 RVA: 0x0010C348 File Offset: 0x0010A548
		private static IPAddress RehydrateIPAddress(PSObject pso)
		{
			string ipString = pso.ToString();
			return IPAddress.Parse(ipString);
		}

		// Token: 0x0600311B RID: 12571 RVA: 0x0010C364 File Offset: 0x0010A564
		private static MailAddress RehydrateMailAddress(PSObject pso)
		{
			string address = pso.ToString();
			return new MailAddress(address);
		}

		// Token: 0x0600311C RID: 12572 RVA: 0x0010C380 File Offset: 0x0010A580
		private static T RehydrateObjectSecurity<T>(PSObject pso) where T : ObjectSecurity, new()
		{
			string propertyValue = DeserializingTypeConverter.GetPropertyValue<string>(pso, "SDDL");
			T result = Activator.CreateInstance<T>();
			result.SetSecurityDescriptorSddlForm(propertyValue);
			return result;
		}

		// Token: 0x0600311D RID: 12573 RVA: 0x0010C3B0 File Offset: 0x0010A5B0
		public static uint GetParameterSetMetadataFlags(PSObject instance)
		{
			if (instance == null)
			{
				throw PSTraceSource.NewArgumentNullException("instance");
			}
			ParameterSetMetadata parameterSetMetadata = instance.BaseObject as ParameterSetMetadata;
			if (parameterSetMetadata == null)
			{
				throw PSTraceSource.NewArgumentNullException("instance");
			}
			return (uint)parameterSetMetadata.Flags;
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x0010C3EC File Offset: 0x0010A5EC
		public static PSObject GetInvocationInfo(PSObject instance)
		{
			if (instance == null)
			{
				throw PSTraceSource.NewArgumentNullException("instance");
			}
			DebuggerStopEventArgs debuggerStopEventArgs = instance.BaseObject as DebuggerStopEventArgs;
			if (debuggerStopEventArgs == null)
			{
				throw PSTraceSource.NewArgumentNullException("instance");
			}
			if (debuggerStopEventArgs.InvocationInfo == null)
			{
				return null;
			}
			PSObject psobject = new PSObject();
			debuggerStopEventArgs.InvocationInfo.ToPSObjectForRemoting(psobject);
			return psobject;
		}

		// Token: 0x0600311F RID: 12575 RVA: 0x0010C440 File Offset: 0x0010A640
		private static ParameterSetMetadata RehydrateParameterSetMetadata(PSObject pso)
		{
			int propertyValue = DeserializingTypeConverter.GetPropertyValue<int>(pso, "Position");
			uint propertyValue2 = DeserializingTypeConverter.GetPropertyValue<uint>(pso, "Flags");
			string propertyValue3 = DeserializingTypeConverter.GetPropertyValue<string>(pso, "HelpMessage");
			return new ParameterSetMetadata(propertyValue, (ParameterSetMetadata.ParameterFlags)propertyValue2, propertyValue3);
		}

		// Token: 0x06003120 RID: 12576 RVA: 0x0010C47C File Offset: 0x0010A67C
		private static DisplayEntry RehydrateDisplayEntry(PSObject deserializedDisplayEntry)
		{
			return new DisplayEntry
			{
				Value = DeserializingTypeConverter.GetPropertyValue<string>(deserializedDisplayEntry, "Value"),
				ValueType = DeserializingTypeConverter.GetPropertyValue<DisplayEntryValueType>(deserializedDisplayEntry, "ValueType")
			};
		}

		// Token: 0x06003121 RID: 12577 RVA: 0x0010C4B4 File Offset: 0x0010A6B4
		private static WideControlEntryItem RehydrateWideControlEntryItem(PSObject deserializedEntryItem)
		{
			return new WideControlEntryItem
			{
				DisplayEntry = DeserializingTypeConverter.GetPropertyValue<DisplayEntry>(deserializedEntryItem, "DisplayEntry"),
				SelectedBy = DeserializingTypeConverter.RehydrateList<List<string>, string>(deserializedEntryItem, "SelectedBy", DeserializingTypeConverter.RehydrationFlags.NullValueOk)
			};
		}

		// Token: 0x06003122 RID: 12578 RVA: 0x0010C4EC File Offset: 0x0010A6EC
		private static ListControlEntryItem RehydrateListControlEntryItem(PSObject deserializedEntryItem)
		{
			return new ListControlEntryItem
			{
				DisplayEntry = DeserializingTypeConverter.GetPropertyValue<DisplayEntry>(deserializedEntryItem, "DisplayEntry"),
				Label = DeserializingTypeConverter.GetPropertyValue<string>(deserializedEntryItem, "Label", DeserializingTypeConverter.RehydrationFlags.NullValueOk)
			};
		}

		// Token: 0x06003123 RID: 12579 RVA: 0x0010C524 File Offset: 0x0010A724
		private static ListControlEntry RehydrateListControlEntry(PSObject deserializedEntry)
		{
			return new ListControlEntry
			{
				Items = DeserializingTypeConverter.RehydrateList<List<ListControlEntryItem>, ListControlEntryItem>(deserializedEntry, "Items", DeserializingTypeConverter.RehydrationFlags.NullValueBad),
				SelectedBy = DeserializingTypeConverter.RehydrateList<List<string>, string>(deserializedEntry, "SelectedBy", DeserializingTypeConverter.RehydrationFlags.NullValueOk)
			};
		}

		// Token: 0x06003124 RID: 12580 RVA: 0x0010C55C File Offset: 0x0010A75C
		private static TableControlColumnHeader RehydrateTableControlColumnHeader(PSObject deserializedHeader)
		{
			return new TableControlColumnHeader
			{
				Alignment = DeserializingTypeConverter.GetPropertyValue<Alignment>(deserializedHeader, "Alignment"),
				Label = DeserializingTypeConverter.GetPropertyValue<string>(deserializedHeader, "Label", DeserializingTypeConverter.RehydrationFlags.NullValueOk),
				Width = DeserializingTypeConverter.GetPropertyValue<int>(deserializedHeader, "Width")
			};
		}

		// Token: 0x06003125 RID: 12581 RVA: 0x0010C5A4 File Offset: 0x0010A7A4
		private static TableControlColumn RehydrateTableControlColumn(PSObject deserializedColumn)
		{
			return new TableControlColumn
			{
				Alignment = DeserializingTypeConverter.GetPropertyValue<Alignment>(deserializedColumn, "Alignment"),
				DisplayEntry = DeserializingTypeConverter.GetPropertyValue<DisplayEntry>(deserializedColumn, "DisplayEntry")
			};
		}

		// Token: 0x06003126 RID: 12582 RVA: 0x0010C5DC File Offset: 0x0010A7DC
		private static TableControlRow RehydrateTableControlRow(PSObject deserializedRow)
		{
			return new TableControlRow
			{
				Columns = DeserializingTypeConverter.RehydrateList<List<TableControlColumn>, TableControlColumn>(deserializedRow, "Columns", DeserializingTypeConverter.RehydrationFlags.NullValueBad)
			};
		}

		// Token: 0x06003127 RID: 12583 RVA: 0x0010C604 File Offset: 0x0010A804
		private static PSControl RehydratePSControl(PSObject deserializedControl)
		{
			if (Deserializer.IsDeserializedInstanceOfType(deserializedControl, typeof(TableControl)))
			{
				return new TableControl
				{
					Headers = DeserializingTypeConverter.RehydrateList<List<TableControlColumnHeader>, TableControlColumnHeader>(deserializedControl, "Headers", DeserializingTypeConverter.RehydrationFlags.NullValueBad),
					Rows = DeserializingTypeConverter.RehydrateList<List<TableControlRow>, TableControlRow>(deserializedControl, "Rows", DeserializingTypeConverter.RehydrationFlags.NullValueBad)
				};
			}
			if (Deserializer.IsDeserializedInstanceOfType(deserializedControl, typeof(ListControl)))
			{
				return new ListControl
				{
					Entries = DeserializingTypeConverter.RehydrateList<List<ListControlEntry>, ListControlEntry>(deserializedControl, "Entries", DeserializingTypeConverter.RehydrationFlags.NullValueBad)
				};
			}
			if (Deserializer.IsDeserializedInstanceOfType(deserializedControl, typeof(WideControl)))
			{
				return new WideControl
				{
					Alignment = DeserializingTypeConverter.GetPropertyValue<Alignment>(deserializedControl, "Alignment"),
					Columns = DeserializingTypeConverter.GetPropertyValue<uint>(deserializedControl, "Columns"),
					Entries = DeserializingTypeConverter.RehydrateList<List<WideControlEntryItem>, WideControlEntryItem>(deserializedControl, "Entries", DeserializingTypeConverter.RehydrationFlags.NullValueBad)
				};
			}
			throw PSTraceSource.NewArgumentException("pso");
		}

		// Token: 0x06003128 RID: 12584 RVA: 0x0010C6D4 File Offset: 0x0010A8D4
		public static Guid GetFormatViewDefinitionInstanceId(PSObject instance)
		{
			if (instance == null)
			{
				throw PSTraceSource.NewArgumentNullException("instance");
			}
			FormatViewDefinition formatViewDefinition = instance.BaseObject as FormatViewDefinition;
			if (formatViewDefinition == null)
			{
				throw PSTraceSource.NewArgumentNullException("instance");
			}
			return formatViewDefinition.InstanceId;
		}

		// Token: 0x06003129 RID: 12585 RVA: 0x0010C710 File Offset: 0x0010A910
		private static FormatViewDefinition RehydrateFormatViewDefinition(PSObject deserializedViewDefinition)
		{
			string propertyValue = DeserializingTypeConverter.GetPropertyValue<string>(deserializedViewDefinition, "Name");
			Guid propertyValue2 = DeserializingTypeConverter.GetPropertyValue<Guid>(deserializedViewDefinition, "InstanceId");
			PSControl propertyValue3 = DeserializingTypeConverter.GetPropertyValue<PSControl>(deserializedViewDefinition, "Control");
			return new FormatViewDefinition(propertyValue, propertyValue3, propertyValue2);
		}

		// Token: 0x0600312A RID: 12586 RVA: 0x0010C74C File Offset: 0x0010A94C
		private static ExtendedTypeDefinition RehydrateExtendedTypeDefinition(PSObject deserializedTypeDefinition)
		{
			string propertyValue = DeserializingTypeConverter.GetPropertyValue<string>(deserializedTypeDefinition, "TypeName");
			List<FormatViewDefinition> viewDefinitions = DeserializingTypeConverter.RehydrateList<List<FormatViewDefinition>, FormatViewDefinition>(deserializedTypeDefinition, "FormatViewDefinition", DeserializingTypeConverter.RehydrationFlags.NullValueBad);
			return new ExtendedTypeDefinition(propertyValue, viewDefinitions);
		}

		// Token: 0x04001A37 RID: 6711
		private static readonly Dictionary<Type, Func<PSObject, object>> converter = new Dictionary<Type, Func<PSObject, object>>();

		// Token: 0x0200045F RID: 1119
		[Flags]
		internal enum RehydrationFlags
		{
			// Token: 0x04001A39 RID: 6713
			NullValueBad = 0,
			// Token: 0x04001A3A RID: 6714
			NullValueOk = 1,
			// Token: 0x04001A3B RID: 6715
			NullValueMeansEmptyList = 3,
			// Token: 0x04001A3C RID: 6716
			MissingPropertyBad = 0,
			// Token: 0x04001A3D RID: 6717
			MissingPropertyOk = 4
		}
	}
}
