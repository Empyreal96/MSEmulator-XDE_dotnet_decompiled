using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Options;
using Microsoft.PowerShell;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x020000B8 RID: 184
	internal class RemoteDiscoveryHelper
	{
		// Token: 0x06000A18 RID: 2584 RVA: 0x0003C0EC File Offset: 0x0003A2EC
		private static Collection<string> RehydrateHashtableKeys(PSObject pso, string propertyName)
		{
			DeserializingTypeConverter.RehydrationFlags flags = DeserializingTypeConverter.RehydrationFlags.NullValueOk | DeserializingTypeConverter.RehydrationFlags.MissingPropertyOk;
			Hashtable propertyValue = DeserializingTypeConverter.GetPropertyValue<Hashtable>(pso, propertyName, flags);
			if (propertyValue == null)
			{
				return new Collection<string>();
			}
			List<string> list = (from object k in propertyValue.Keys
			where k != null
			select k.ToString() into s
			where s != null
			select s).ToList<string>();
			return new Collection<string>(list);
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0003C18C File Offset: 0x0003A38C
		internal static PSModuleInfo RehydratePSModuleInfo(PSObject deserializedModuleInfo)
		{
			DeserializingTypeConverter.RehydrationFlags flags = DeserializingTypeConverter.RehydrationFlags.NullValueOk | DeserializingTypeConverter.RehydrationFlags.MissingPropertyOk;
			string propertyValue = DeserializingTypeConverter.GetPropertyValue<string>(deserializedModuleInfo, "Name", flags);
			string propertyValue2 = DeserializingTypeConverter.GetPropertyValue<string>(deserializedModuleInfo, "Path", flags);
			PSModuleInfo psmoduleInfo = new PSModuleInfo(propertyValue, propertyValue2, null, null);
			psmoduleInfo.SetGuid(DeserializingTypeConverter.GetPropertyValue<Guid>(deserializedModuleInfo, "Guid", flags));
			psmoduleInfo.SetModuleType(DeserializingTypeConverter.GetPropertyValue<ModuleType>(deserializedModuleInfo, "ModuleType", flags));
			psmoduleInfo.SetVersion(DeserializingTypeConverter.GetPropertyValue<Version>(deserializedModuleInfo, "Version", flags));
			psmoduleInfo.SetHelpInfoUri(DeserializingTypeConverter.GetPropertyValue<string>(deserializedModuleInfo, "HelpInfoUri", flags));
			psmoduleInfo.AccessMode = DeserializingTypeConverter.GetPropertyValue<ModuleAccessMode>(deserializedModuleInfo, "AccessMode", flags);
			psmoduleInfo.Author = DeserializingTypeConverter.GetPropertyValue<string>(deserializedModuleInfo, "Author", flags);
			psmoduleInfo.ClrVersion = DeserializingTypeConverter.GetPropertyValue<Version>(deserializedModuleInfo, "ClrVersion", flags);
			psmoduleInfo.CompanyName = DeserializingTypeConverter.GetPropertyValue<string>(deserializedModuleInfo, "CompanyName", flags);
			psmoduleInfo.Copyright = DeserializingTypeConverter.GetPropertyValue<string>(deserializedModuleInfo, "Copyright", flags);
			psmoduleInfo.Description = DeserializingTypeConverter.GetPropertyValue<string>(deserializedModuleInfo, "Description", flags);
			psmoduleInfo.DotNetFrameworkVersion = DeserializingTypeConverter.GetPropertyValue<Version>(deserializedModuleInfo, "DotNetFrameworkVersion", flags);
			psmoduleInfo.PowerShellHostName = DeserializingTypeConverter.GetPropertyValue<string>(deserializedModuleInfo, "PowerShellHostName", flags);
			psmoduleInfo.PowerShellHostVersion = DeserializingTypeConverter.GetPropertyValue<Version>(deserializedModuleInfo, "PowerShellHostVersion", flags);
			psmoduleInfo.PowerShellVersion = DeserializingTypeConverter.GetPropertyValue<Version>(deserializedModuleInfo, "PowerShellVersion", flags);
			psmoduleInfo.ProcessorArchitecture = DeserializingTypeConverter.GetPropertyValue<ProcessorArchitecture>(deserializedModuleInfo, "ProcessorArchitecture", flags);
			psmoduleInfo.DeclaredAliasExports = RemoteDiscoveryHelper.RehydrateHashtableKeys(deserializedModuleInfo, "ExportedAliases");
			psmoduleInfo.DeclaredCmdletExports = RemoteDiscoveryHelper.RehydrateHashtableKeys(deserializedModuleInfo, "ExportedCmdlets");
			psmoduleInfo.DeclaredFunctionExports = RemoteDiscoveryHelper.RehydrateHashtableKeys(deserializedModuleInfo, "ExportedFunctions");
			psmoduleInfo.DeclaredVariableExports = RemoteDiscoveryHelper.RehydrateHashtableKeys(deserializedModuleInfo, "ExportedVariables");
			string[] propertyValue3 = DeserializingTypeConverter.GetPropertyValue<string[]>(deserializedModuleInfo, "Tags", flags);
			if (propertyValue3 != null && propertyValue3.Any<string>())
			{
				foreach (string tag in propertyValue3)
				{
					psmoduleInfo.AddToTags(tag);
				}
			}
			psmoduleInfo.ReleaseNotes = DeserializingTypeConverter.GetPropertyValue<string>(deserializedModuleInfo, "ReleaseNotes", flags);
			psmoduleInfo.ProjectUri = DeserializingTypeConverter.GetPropertyValue<Uri>(deserializedModuleInfo, "ProjectUri", flags);
			psmoduleInfo.LicenseUri = DeserializingTypeConverter.GetPropertyValue<Uri>(deserializedModuleInfo, "LicenseUri", flags);
			psmoduleInfo.IconUri = DeserializingTypeConverter.GetPropertyValue<Uri>(deserializedModuleInfo, "IconUri", flags);
			psmoduleInfo.RepositorySourceLocation = DeserializingTypeConverter.GetPropertyValue<Uri>(deserializedModuleInfo, "RepositorySourceLocation", flags);
			return psmoduleInfo;
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0003C42C File Offset: 0x0003A62C
		private static EventHandler<DataAddedEventArgs> GetStreamForwarder<T>(Action<T> forwardingAction, bool swallowInvalidOperationExceptions = false)
		{
			return delegate(object sender, DataAddedEventArgs eventArgs)
			{
				PSDataCollection<T> psdataCollection = (PSDataCollection<T>)sender;
				foreach (T obj in psdataCollection.ReadAll())
				{
					try
					{
						forwardingAction(obj);
					}
					catch (InvalidOperationException)
					{
						if (!swallowInvalidOperationExceptions)
						{
							throw;
						}
					}
				}
			};
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x0003CEBC File Offset: 0x0003B0BC
		private static IEnumerable<PSObject> InvokeTopLevelPowerShell(PowerShell powerShell, CancellationToken cancellationToken, PSCmdlet cmdlet, PSInvocationSettings invocationSettings, string errorMessageTemplate)
		{
			RemoteDiscoveryHelper.<>c__DisplayClass16 CS$<>8__locals1 = new RemoteDiscoveryHelper.<>c__DisplayClass16();
			CS$<>8__locals1.powerShell = powerShell;
			CS$<>8__locals1.errorMessageTemplate = errorMessageTemplate;
			using (BlockingCollection<Func<PSCmdlet, IEnumerable<PSObject>>> mergedOutput = new BlockingCollection<Func<PSCmdlet, IEnumerable<PSObject>>>(RemoteDiscoveryHelper.BlockingCollectionCapacity))
			{
				PSDataCollection<PSObject> asyncOutput = new PSDataCollection<PSObject>();
				EventHandler<DataAddedEventArgs> outputHandler = RemoteDiscoveryHelper.GetStreamForwarder<PSObject>(delegate(PSObject output)
				{
					mergedOutput.Add((PSCmdlet _) => new PSObject[]
					{
						output
					});
				}, true);
				EventHandler<DataAddedEventArgs> errorHandler = RemoteDiscoveryHelper.GetStreamForwarder<ErrorRecord>(delegate(ErrorRecord errorRecord)
				{
					mergedOutput.Add(delegate(PSCmdlet c)
					{
						errorRecord = RemoteDiscoveryHelper.GetErrorRecordForRemotePipelineInvocation(errorRecord, CS$<>8__locals1.errorMessageTemplate);
						RemoteDiscoveryHelper.HandleErrorFromPipeline(c, errorRecord, CS$<>8__locals1.powerShell);
						return Enumerable.Empty<PSObject>();
					});
				}, true);
				EventHandler<DataAddedEventArgs> warningHandler = RemoteDiscoveryHelper.GetStreamForwarder<WarningRecord>(delegate(WarningRecord warningRecord)
				{
					mergedOutput.Add(delegate(PSCmdlet c)
					{
						c.WriteWarning(warningRecord.Message);
						return Enumerable.Empty<PSObject>();
					});
				}, true);
				EventHandler<DataAddedEventArgs> verboseHandler = RemoteDiscoveryHelper.GetStreamForwarder<VerboseRecord>(delegate(VerboseRecord verboseRecord)
				{
					mergedOutput.Add(delegate(PSCmdlet c)
					{
						c.WriteVerbose(verboseRecord.Message);
						return Enumerable.Empty<PSObject>();
					});
				}, true);
				EventHandler<DataAddedEventArgs> debugHandler = RemoteDiscoveryHelper.GetStreamForwarder<DebugRecord>(delegate(DebugRecord debugRecord)
				{
					mergedOutput.Add(delegate(PSCmdlet c)
					{
						c.WriteDebug(debugRecord.Message);
						return Enumerable.Empty<PSObject>();
					});
				}, true);
				EventHandler<DataAddedEventArgs> informationHandler = RemoteDiscoveryHelper.GetStreamForwarder<InformationRecord>(delegate(InformationRecord informationRecord)
				{
					mergedOutput.Add(delegate(PSCmdlet c)
					{
						c.WriteInformation(informationRecord);
						return Enumerable.Empty<PSObject>();
					});
				}, true);
				asyncOutput.DataAdded += outputHandler;
				CS$<>8__locals1.powerShell.Streams.Error.DataAdded += errorHandler;
				CS$<>8__locals1.powerShell.Streams.Warning.DataAdded += warningHandler;
				CS$<>8__locals1.powerShell.Streams.Verbose.DataAdded += verboseHandler;
				CS$<>8__locals1.powerShell.Streams.Debug.DataAdded += debugHandler;
				CS$<>8__locals1.powerShell.Streams.Information.DataAdded += informationHandler;
				try
				{
					IAsyncResult asyncResult = CS$<>8__locals1.powerShell.BeginInvoke<PSObject, PSObject>(null, asyncOutput, invocationSettings, delegate(IAsyncResult param0)
					{
						try
						{
							mergedOutput.CompleteAdding();
						}
						catch (InvalidOperationException)
						{
						}
					}, null);
					using (cancellationToken.Register(new Action(CS$<>8__locals1.powerShell.Stop)))
					{
						try
						{
							foreach (Func<PSCmdlet, IEnumerable<PSObject>> mergedOutputItem in mergedOutput.GetConsumingEnumerable())
							{
								foreach (PSObject outputObject in mergedOutputItem(cmdlet))
								{
									yield return outputObject;
								}
							}
						}
						finally
						{
							mergedOutput.CompleteAdding();
							CS$<>8__locals1.powerShell.EndInvoke(asyncResult);
						}
					}
				}
				finally
				{
					asyncOutput.DataAdded -= outputHandler;
					CS$<>8__locals1.powerShell.Streams.Error.DataAdded -= errorHandler;
					CS$<>8__locals1.powerShell.Streams.Warning.DataAdded -= warningHandler;
					CS$<>8__locals1.powerShell.Streams.Verbose.DataAdded -= verboseHandler;
					CS$<>8__locals1.powerShell.Streams.Debug.DataAdded -= debugHandler;
					CS$<>8__locals1.powerShell.Streams.Information.DataAdded -= informationHandler;
				}
			}
			yield break;
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0003D254 File Offset: 0x0003B454
		private static IEnumerable<PSObject> InvokeNestedPowerShell(PowerShell powerShell, CancellationToken cancellationToken, PSCmdlet cmdlet, PSInvocationSettings invocationSettings, string errorMessageTemplate)
		{
			EventHandler<DataAddedEventArgs> errorHandler = RemoteDiscoveryHelper.GetStreamForwarder<ErrorRecord>(delegate(ErrorRecord errorRecord)
			{
				errorRecord = RemoteDiscoveryHelper.GetErrorRecordForRemotePipelineInvocation(errorRecord, errorMessageTemplate);
				RemoteDiscoveryHelper.HandleErrorFromPipeline(cmdlet, errorRecord, powerShell);
			}, false);
			powerShell.Streams.Error.DataAdded += errorHandler;
			try
			{
				using (cancellationToken.Register(new Action(powerShell.Stop)))
				{
					foreach (PSObject outputObject in powerShell.Invoke<PSObject>(null, invocationSettings))
					{
						yield return outputObject;
					}
				}
			}
			finally
			{
				powerShell.Streams.Error.DataAdded -= errorHandler;
			}
			yield break;
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x0003D2AC File Offset: 0x0003B4AC
		private static void CopyParameterFromCmdletToPowerShell(Cmdlet cmdlet, PowerShell powerShell, string parameterName)
		{
			object value;
			if (!cmdlet.MyInvocation.BoundParameters.TryGetValue(parameterName, out value))
			{
				return;
			}
			CommandParameter item = new CommandParameter(parameterName, value);
			foreach (Command command in powerShell.Commands.Commands)
			{
				if (!command.Parameters.Any((CommandParameter existingParameter) => existingParameter.Name.Equals(parameterName, StringComparison.OrdinalIgnoreCase)))
				{
					command.Parameters.Add(item);
				}
			}
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x0003D364 File Offset: 0x0003B564
		internal static ErrorRecord GetErrorRecordForProcessingOfCimModule(Exception innerException, string moduleName)
		{
			string message = string.Format(CultureInfo.InvariantCulture, Modules.RemoteDiscoveryFailedToProcessRemoteModule, new object[]
			{
				moduleName,
				innerException.Message
			});
			Exception exception = new InvalidOperationException(message, innerException);
			return new ErrorRecord(exception, innerException.GetType().Name, ErrorCategory.NotSpecified, moduleName);
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x0003D3B4 File Offset: 0x0003B5B4
		private static ErrorRecord GetErrorRecordForRemoteDiscoveryProvider(Exception innerException)
		{
			CimException ex = innerException as CimException;
			if (ex != null && (ex.NativeErrorCode == NativeErrorCode.InvalidNamespace || ex.NativeErrorCode == NativeErrorCode.InvalidClass || ex.NativeErrorCode == NativeErrorCode.MethodNotFound || ex.NativeErrorCode == NativeErrorCode.MethodNotAvailable))
			{
				string message = string.Format(CultureInfo.InvariantCulture, Modules.RemoteDiscoveryProviderNotFound, new object[]
				{
					innerException.Message
				});
				Exception exception = new InvalidOperationException(message, innerException);
				return new ErrorRecord(exception, "DiscoveryProviderNotFound", ErrorCategory.NotImplemented, null);
			}
			string message2 = string.Format(CultureInfo.InvariantCulture, Modules.RemoteDiscoveryFailureFromDiscoveryProvider, new object[]
			{
				innerException.Message
			});
			Exception exception2 = new InvalidOperationException(message2, innerException);
			return new ErrorRecord(exception2, "DiscoveryProviderFailure", ErrorCategory.NotSpecified, null);
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x0003D470 File Offset: 0x0003B670
		private static ErrorRecord GetErrorRecordForRemotePipelineInvocation(Exception innerException, string errorMessageTemplate)
		{
			string message = string.Format(CultureInfo.InvariantCulture, errorMessageTemplate, new object[]
			{
				innerException.Message
			});
			Exception exception = new InvalidOperationException(message, innerException);
			RemoteException ex = innerException as RemoteException;
			ErrorRecord errorRecord = (ex != null) ? ex.ErrorRecord : null;
			string errorId = (errorRecord != null) ? errorRecord.FullyQualifiedErrorId : innerException.GetType().Name;
			ErrorCategory errorCategory = (errorRecord != null) ? errorRecord.CategoryInfo.Category : ErrorCategory.NotSpecified;
			return new ErrorRecord(exception, errorId, errorCategory, null);
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x0003D4F4 File Offset: 0x0003B6F4
		private static ErrorRecord GetErrorRecordForRemotePipelineInvocation(ErrorRecord innerErrorRecord, string errorMessageTemplate)
		{
			string text;
			if (innerErrorRecord.ErrorDetails != null && innerErrorRecord.ErrorDetails.Message != null)
			{
				text = innerErrorRecord.ErrorDetails.Message;
			}
			else if (innerErrorRecord.Exception != null && innerErrorRecord.Exception.Message != null)
			{
				text = innerErrorRecord.Exception.Message;
			}
			else
			{
				text = innerErrorRecord.ToString();
			}
			string message = string.Format(CultureInfo.InvariantCulture, errorMessageTemplate, new object[]
			{
				text
			});
			ErrorRecord errorRecord = new ErrorRecord(innerErrorRecord, null);
			ErrorDetails errorDetails = new ErrorDetails(message);
			errorRecord.ErrorDetails = errorDetails;
			return errorRecord;
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x0003D800 File Offset: 0x0003BA00
		private static IEnumerable<T> EnumerateWithCatch<T>(IEnumerable<T> enumerable, Action<Exception> exceptionHandler)
		{
			IEnumerator<T> enumerator = null;
			try
			{
				enumerator = enumerable.GetEnumerator();
			}
			catch (Exception obj)
			{
				exceptionHandler(obj);
			}
			if (enumerator != null)
			{
				using (enumerator)
				{
					bool gotResults = false;
					for (;;)
					{
						try
						{
							gotResults = false;
							gotResults = enumerator.MoveNext();
						}
						catch (Exception obj2)
						{
							exceptionHandler(obj2);
						}
						if (gotResults)
						{
							T currentItem = default(T);
							bool gotCurrentItem = false;
							try
							{
								currentItem = enumerator.Current;
								gotCurrentItem = true;
							}
							catch (Exception obj3)
							{
								exceptionHandler(obj3);
							}
							if (!gotCurrentItem)
							{
								break;
							}
							yield return currentItem;
						}
						if (!gotResults)
						{
							goto Block_11;
						}
					}
					yield break;
					Block_11:;
				}
			}
			yield break;
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x0003D824 File Offset: 0x0003BA24
		private static void HandleErrorFromPipeline(Cmdlet cmdlet, ErrorRecord errorRecord, PowerShell powerShell)
		{
			if (!cmdlet.MyInvocation.ExpectingInput && ((powerShell.Runspace != null && powerShell.Runspace.RunspaceStateInfo.State != RunspaceState.Opened) || (powerShell.RunspacePool != null && powerShell.RunspacePool.RunspacePoolStateInfo.State != RunspacePoolState.Opened)))
			{
				cmdlet.ThrowTerminatingError(errorRecord);
			}
			cmdlet.WriteError(errorRecord);
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x0003D8B8 File Offset: 0x0003BAB8
		internal static IEnumerable<PSObject> InvokePowerShell(PowerShell powerShell, CancellationToken cancellationToken, PSCmdlet cmdlet, string errorMessageTemplate)
		{
			RemoteDiscoveryHelper.CopyParameterFromCmdletToPowerShell(cmdlet, powerShell, "ErrorAction");
			RemoteDiscoveryHelper.CopyParameterFromCmdletToPowerShell(cmdlet, powerShell, "WarningAction");
			RemoteDiscoveryHelper.CopyParameterFromCmdletToPowerShell(cmdlet, powerShell, "InformationAction");
			RemoteDiscoveryHelper.CopyParameterFromCmdletToPowerShell(cmdlet, powerShell, "Verbose");
			RemoteDiscoveryHelper.CopyParameterFromCmdletToPowerShell(cmdlet, powerShell, "Debug");
			PSInvocationSettings invocationSettings = new PSInvocationSettings
			{
				Host = cmdlet.Host
			};
			IEnumerable<PSObject> enumerable = powerShell.IsNested ? RemoteDiscoveryHelper.InvokeNestedPowerShell(powerShell, cancellationToken, cmdlet, invocationSettings, errorMessageTemplate) : RemoteDiscoveryHelper.InvokeTopLevelPowerShell(powerShell, cancellationToken, cmdlet, invocationSettings, errorMessageTemplate);
			return RemoteDiscoveryHelper.EnumerateWithCatch<PSObject>(enumerable, delegate(Exception exception)
			{
				ErrorRecord errorRecordForRemotePipelineInvocation = RemoteDiscoveryHelper.GetErrorRecordForRemotePipelineInvocation(exception, errorMessageTemplate);
				RemoteDiscoveryHelper.HandleErrorFromPipeline(cmdlet, errorRecordForRemotePipelineInvocation, powerShell);
			});
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x0003D9BC File Offset: 0x0003BBBC
		private static T GetPropertyValue<T>(CimInstance cimInstance, string propertyName, T defaultValue)
		{
			CimProperty cimProperty = cimInstance.CimInstanceProperties[propertyName];
			if (cimProperty == null)
			{
				return defaultValue;
			}
			object value = cimProperty.Value;
			if (value is T)
			{
				return (T)((object)value);
			}
			if (value is string)
			{
				string s = (string)value;
				try
				{
					if (typeof(T) == typeof(bool))
					{
						return (T)((object)XmlConvert.ToBoolean(s));
					}
					if (typeof(T) == typeof(ushort))
					{
						return (T)((object)ushort.Parse(s, CultureInfo.InvariantCulture));
					}
					if (typeof(T) == typeof(byte[]))
					{
						byte[] array = Convert.FromBase64String(s);
						byte[] bytes = BitConverter.GetBytes(array.Length + 4);
						if (BitConverter.IsLittleEndian)
						{
							Array.Reverse(bytes);
						}
						return (T)((object)bytes.Concat(array).ToArray<byte>());
					}
				}
				catch (Exception)
				{
					return defaultValue;
				}
				return defaultValue;
			}
			return defaultValue;
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0003DDE0 File Offset: 0x0003BFE0
		internal static IEnumerable<RemoteDiscoveryHelper.CimModule> GetCimModules(CimSession cimSession, Uri resourceUri, string cimNamespace, IEnumerable<string> moduleNamePatterns, bool onlyManifests, Cmdlet cmdlet, CancellationToken cancellationToken)
		{
			IEnumerable<string> enumerable;
			if ((enumerable = moduleNamePatterns) == null)
			{
				enumerable = (IEnumerable<string>)new string[]
				{
					"*"
				};
			}
			moduleNamePatterns = enumerable;
			HashSet<string> alreadyEmittedNamesOfCimModules = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			IEnumerable<RemoteDiscoveryHelper.CimModule> remoteModules = moduleNamePatterns.SelectMany((string moduleNamePattern) => RemoteDiscoveryHelper.GetCimModules(cimSession, resourceUri, cimNamespace, moduleNamePattern, onlyManifests, cmdlet, cancellationToken));
			foreach (RemoteDiscoveryHelper.CimModule remoteModule in remoteModules)
			{
				if (!alreadyEmittedNamesOfCimModules.Contains(remoteModule.ModuleName))
				{
					alreadyEmittedNamesOfCimModules.Add(remoteModule.ModuleName);
					yield return remoteModule;
				}
			}
			yield break;
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0003DEDC File Offset: 0x0003C0DC
		private static IEnumerable<RemoteDiscoveryHelper.CimModule> GetCimModules(CimSession cimSession, Uri resourceUri, string cimNamespace, string moduleNamePattern, bool onlyManifests, Cmdlet cmdlet, CancellationToken cancellationToken)
		{
			WildcardPattern wildcardPattern = new WildcardPattern(moduleNamePattern, WildcardOptions.IgnoreCase | WildcardOptions.CultureInvariant);
			string optionValue = WildcardPatternToDosWildcardParser.Parse(wildcardPattern);
			CimOperationOptions options = new CimOperationOptions
			{
				CancellationToken = new CancellationToken?(cancellationToken)
			};
			options.SetCustomOption("PS_ModuleNamePattern", optionValue, false);
			if (resourceUri != null)
			{
				options.ResourceUri = resourceUri;
			}
			if (string.IsNullOrEmpty(cimNamespace) && resourceUri == null)
			{
				cimNamespace = "root/Microsoft/Windows/Powershellv3";
			}
			IEnumerable<CimInstance> source = cimSession.EnumerateInstances(cimNamespace, "PS_Module", options);
			IEnumerable<RemoteDiscoveryHelper.CimModule> enumerable = from cimInstance in source
			select new RemoteDiscoveryHelper.CimModule(cimInstance) into cimModule
			where wildcardPattern.IsMatch(cimModule.ModuleName)
			select cimModule;
			if (!onlyManifests)
			{
				enumerable = enumerable.Select(delegate(RemoteDiscoveryHelper.CimModule cimModule)
				{
					cimModule.FetchAllModuleFiles(cimSession, cimNamespace, options);
					return cimModule;
				});
			}
			return RemoteDiscoveryHelper.EnumerateWithCatch<RemoteDiscoveryHelper.CimModule>(enumerable, delegate(Exception exception)
			{
				ErrorRecord errorRecordForRemoteDiscoveryProvider = RemoteDiscoveryHelper.GetErrorRecordForRemoteDiscoveryProvider(exception);
				if (!cmdlet.MyInvocation.ExpectingInput && (-1 != errorRecordForRemoteDiscoveryProvider.FullyQualifiedErrorId.IndexOf("DiscoveryProviderNotFound", StringComparison.OrdinalIgnoreCase) || cancellationToken.IsCancellationRequested || exception is OperationCanceledException || !cimSession.TestConnection()))
				{
					cmdlet.ThrowTerminatingError(errorRecordForRemoteDiscoveryProvider);
				}
				cmdlet.WriteError(errorRecordForRemoteDiscoveryProvider);
			});
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0003E025 File Offset: 0x0003C225
		internal static Hashtable RewriteManifest(Hashtable originalManifest)
		{
			return RemoteDiscoveryHelper.RewriteManifest(originalManifest, null, null, null);
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x0003E030 File Offset: 0x0003C230
		internal static Hashtable RewriteManifest(Hashtable originalManifest, IEnumerable<string> nestedModules, IEnumerable<string> typesToProcess, IEnumerable<string> formatsToProcess)
		{
			nestedModules = (nestedModules ?? ((IEnumerable<string>)new string[0]));
			typesToProcess = (typesToProcess ?? ((IEnumerable<string>)new string[0]));
			formatsToProcess = (formatsToProcess ?? ((IEnumerable<string>)new string[0]));
			Hashtable hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
			hashtable["NestedModules"] = nestedModules;
			hashtable["TypesToProcess"] = typesToProcess;
			hashtable["FormatsToProcess"] = formatsToProcess;
			if (originalManifest.ContainsKey("PrivateData"))
			{
				hashtable["PrivateData"] = originalManifest["PrivateData"];
			}
			foreach (object obj in originalManifest)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				if (RemoteDiscoveryHelper.ManifestEntriesToKeepAsString.Contains(dictionaryEntry.Key as string, StringComparer.OrdinalIgnoreCase))
				{
					string value = (string)LanguagePrimitives.ConvertTo(dictionaryEntry.Value, typeof(string), CultureInfo.InvariantCulture);
					hashtable[dictionaryEntry.Key] = value;
				}
				else if (RemoteDiscoveryHelper.ManifestEntriesToKeepAsStringArray.Contains(dictionaryEntry.Key as string, StringComparer.OrdinalIgnoreCase))
				{
					string[] value2 = (string[])LanguagePrimitives.ConvertTo(dictionaryEntry.Value, typeof(string[]), CultureInfo.InvariantCulture);
					hashtable[dictionaryEntry.Key] = value2;
				}
			}
			return hashtable;
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0003E1B0 File Offset: 0x0003C3B0
		private static CimCredential GetCimCredentials(PasswordAuthenticationMechanism authenticationMechanism, PSCredential credential)
		{
			NetworkCredential networkCredential = credential.GetNetworkCredential();
			return new CimCredential(authenticationMechanism, networkCredential.Domain, networkCredential.UserName, credential.Password);
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x0003E1DC File Offset: 0x0003C3DC
		private static Exception GetExceptionWhenAuthenticationRequiresCredential(string authentication)
		{
			string message = string.Format(CultureInfo.InvariantCulture, RemotingErrorIdStrings.AuthenticationMechanismRequiresCredential, new object[]
			{
				authentication
			});
			throw new ArgumentException(message);
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x0003E20C File Offset: 0x0003C40C
		private static CimCredential GetCimCredentials(string authentication, PSCredential credential)
		{
			if (authentication == null || authentication.Equals("Default", StringComparison.OrdinalIgnoreCase))
			{
				if (credential == null)
				{
					return null;
				}
				return RemoteDiscoveryHelper.GetCimCredentials(PasswordAuthenticationMechanism.Default, credential);
			}
			else if (authentication.Equals("Basic", StringComparison.OrdinalIgnoreCase))
			{
				if (credential == null)
				{
					throw RemoteDiscoveryHelper.GetExceptionWhenAuthenticationRequiresCredential(authentication);
				}
				return RemoteDiscoveryHelper.GetCimCredentials(PasswordAuthenticationMechanism.Basic, credential);
			}
			else if (authentication.Equals("Negotiate", StringComparison.OrdinalIgnoreCase))
			{
				if (credential == null)
				{
					return new CimCredential(ImpersonatedAuthenticationMechanism.Negotiate);
				}
				return RemoteDiscoveryHelper.GetCimCredentials(PasswordAuthenticationMechanism.Negotiate, credential);
			}
			else if (authentication.Equals("CredSSP", StringComparison.OrdinalIgnoreCase))
			{
				if (credential == null)
				{
					throw RemoteDiscoveryHelper.GetExceptionWhenAuthenticationRequiresCredential(authentication);
				}
				return RemoteDiscoveryHelper.GetCimCredentials(PasswordAuthenticationMechanism.CredSsp, credential);
			}
			else if (authentication.Equals("Digest", StringComparison.OrdinalIgnoreCase))
			{
				if (credential == null)
				{
					throw RemoteDiscoveryHelper.GetExceptionWhenAuthenticationRequiresCredential(authentication);
				}
				return RemoteDiscoveryHelper.GetCimCredentials(PasswordAuthenticationMechanism.Digest, credential);
			}
			else
			{
				if (!authentication.Equals("Kerberos", StringComparison.OrdinalIgnoreCase))
				{
					throw new ArgumentOutOfRangeException("authentication");
				}
				if (credential == null)
				{
					return new CimCredential(ImpersonatedAuthenticationMechanism.Kerberos);
				}
				return RemoteDiscoveryHelper.GetCimCredentials(PasswordAuthenticationMechanism.Kerberos, credential);
			}
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x0003E2E4 File Offset: 0x0003C4E4
		internal static CimSession CreateCimSession(string computerName, PSCredential credential, string authentication, CancellationToken cancellationToken, PSCmdlet cmdlet)
		{
			CimSessionOptions cimSessionOptions = new CimSessionOptions();
			CimCredential cimCredentials = RemoteDiscoveryHelper.GetCimCredentials(authentication, credential);
			if (cimCredentials != null)
			{
				cimSessionOptions.AddDestinationCredentials(cimCredentials);
			}
			return CimSession.Create(computerName, cimSessionOptions);
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x0003E314 File Offset: 0x0003C514
		internal static Hashtable ConvertCimModuleFileToManifestHashtable(RemoteDiscoveryHelper.CimModuleFile cimModuleFile, string temporaryModuleManifestPath, ModuleCmdletBase cmdlet, ref bool containedErrors)
		{
			ScriptBlockAst scriptBlockAst = null;
			if (!containedErrors)
			{
				Token[] array;
				ParseError[] array2;
				scriptBlockAst = Parser.ParseInput(cimModuleFile.FileData, temporaryModuleManifestPath, out array, out array2);
				if (scriptBlockAst == null || (array2 != null && array2.Length > 0))
				{
					containedErrors = true;
				}
			}
			Hashtable result = null;
			if (!containedErrors)
			{
				ScriptBlock scriptBlock = new ScriptBlock(scriptBlockAst, false);
				result = cmdlet.LoadModuleManifestData(temporaryModuleManifestPath, scriptBlock, ModuleCmdletBase.ModuleManifestMembers, (ModuleCmdletBase.ManifestProcessingFlags)0, ref containedErrors);
			}
			return result;
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x0003E368 File Offset: 0x0003C568
		internal static string GetModulePath(string remoteModuleName, Version remoteModuleVersion, string computerName, Runspace localRunspace)
		{
			computerName = (computerName ?? string.Empty);
			string text = Regex.Replace(remoteModuleName, "[^a-zA-Z0-9]", "");
			string text2 = Regex.Replace(computerName, "[^a-zA-Z0-9]", "");
			string path = string.Format(CultureInfo.InvariantCulture, "remoteIpMoProxy_{0}_{1}_{2}_{3}", new object[]
			{
				text.Substring(0, Math.Min(text.Length, 100)),
				remoteModuleVersion,
				text2.Substring(0, Math.Min(text2.Length, 100)),
				localRunspace.InstanceId
			});
			return Path.Combine(Path.GetTempPath(), path);
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0003E40E File Offset: 0x0003C60E
		internal static void AssociatePSModuleInfoWithSession(PSModuleInfo moduleInfo, CimSession cimSession, Uri resourceUri, string cimNamespace)
		{
			RemoteDiscoveryHelper.AssociatePSModuleInfoWithSession(moduleInfo, new Tuple<CimSession, Uri, string>(cimSession, resourceUri, cimNamespace));
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0003E41E File Offset: 0x0003C61E
		internal static void AssociatePSModuleInfoWithSession(PSModuleInfo moduleInfo, PSSession psSession)
		{
			RemoteDiscoveryHelper.AssociatePSModuleInfoWithSession(moduleInfo, psSession);
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0003E427 File Offset: 0x0003C627
		private static void AssociatePSModuleInfoWithSession(PSModuleInfo moduleInfo, object weaklyTypedSession)
		{
			RemoteDiscoveryHelper._moduleInfoToSession.Add(moduleInfo, weaklyTypedSession);
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x0003E438 File Offset: 0x0003C638
		internal static void DispatchModuleInfoProcessing(PSModuleInfo moduleInfo, Action localAction, Action<CimSession, Uri, string> cimSessionAction, Action<PSSession> psSessionAction)
		{
			object obj;
			if (!RemoteDiscoveryHelper._moduleInfoToSession.TryGetValue(moduleInfo, out obj))
			{
				localAction();
				return;
			}
			Tuple<CimSession, Uri, string> tuple = obj as Tuple<CimSession, Uri, string>;
			if (tuple != null)
			{
				cimSessionAction(tuple.Item1, tuple.Item2, tuple.Item3);
				return;
			}
			PSSession pssession = obj as PSSession;
			if (pssession != null)
			{
				psSessionAction(pssession);
			}
		}

		// Token: 0x04000490 RID: 1168
		private const string DiscoveryProviderNotFoundErrorId = "DiscoveryProviderNotFound";

		// Token: 0x04000491 RID: 1169
		private const string DiscoveryProviderNamespace = "root/Microsoft/Windows/Powershellv3";

		// Token: 0x04000492 RID: 1170
		private const string DiscoveryProviderModuleClass = "PS_Module";

		// Token: 0x04000493 RID: 1171
		private const string DiscoveryProviderFileClass = "PS_ModuleFile";

		// Token: 0x04000494 RID: 1172
		private const string DiscoveryProviderAssociationClass = "PS_ModuleToModuleFile";

		// Token: 0x04000495 RID: 1173
		private static readonly int BlockingCollectionCapacity = 1000;

		// Token: 0x04000496 RID: 1174
		private static readonly string[] ManifestEntriesToKeepAsString = new string[]
		{
			"GUID",
			"Author",
			"CompanyName",
			"Copyright",
			"ModuleVersion",
			"Description",
			"HelpInfoURI"
		};

		// Token: 0x04000497 RID: 1175
		private static readonly string[] ManifestEntriesToKeepAsStringArray = new string[]
		{
			"FunctionsToExport",
			"VariablesToExport",
			"AliasesToExport",
			"CmdletsToExport"
		};

		// Token: 0x04000498 RID: 1176
		private static readonly ConditionalWeakTable<PSModuleInfo, object> _moduleInfoToSession = new ConditionalWeakTable<PSModuleInfo, object>();

		// Token: 0x020000B9 RID: 185
		internal enum CimFileCode
		{
			// Token: 0x0400049E RID: 1182
			Unknown,
			// Token: 0x0400049F RID: 1183
			PsdV1,
			// Token: 0x040004A0 RID: 1184
			TypesV1,
			// Token: 0x040004A1 RID: 1185
			FormatV1,
			// Token: 0x040004A2 RID: 1186
			CmdletizationV1
		}

		// Token: 0x020000BA RID: 186
		internal abstract class CimModuleFile
		{
			// Token: 0x170002C0 RID: 704
			// (get) Token: 0x06000A3A RID: 2618 RVA: 0x0003E52C File Offset: 0x0003C72C
			public RemoteDiscoveryHelper.CimFileCode FileCode
			{
				get
				{
					if (this.FileName.EndsWith(".psd1", StringComparison.OrdinalIgnoreCase))
					{
						return RemoteDiscoveryHelper.CimFileCode.PsdV1;
					}
					if (this.FileName.EndsWith(".cdxml", StringComparison.OrdinalIgnoreCase))
					{
						return RemoteDiscoveryHelper.CimFileCode.CmdletizationV1;
					}
					if (this.FileName.EndsWith(".types.ps1xml", StringComparison.OrdinalIgnoreCase))
					{
						return RemoteDiscoveryHelper.CimFileCode.TypesV1;
					}
					if (this.FileName.EndsWith(".format.ps1xml", StringComparison.OrdinalIgnoreCase))
					{
						return RemoteDiscoveryHelper.CimFileCode.FormatV1;
					}
					return RemoteDiscoveryHelper.CimFileCode.Unknown;
				}
			}

			// Token: 0x170002C1 RID: 705
			// (get) Token: 0x06000A3B RID: 2619
			public abstract string FileName { get; }

			// Token: 0x170002C2 RID: 706
			// (get) Token: 0x06000A3C RID: 2620
			internal abstract byte[] RawFileDataCore { get; }

			// Token: 0x170002C3 RID: 707
			// (get) Token: 0x06000A3D RID: 2621 RVA: 0x0003E58E File Offset: 0x0003C78E
			public byte[] RawFileData
			{
				get
				{
					return this.RawFileDataCore.Skip(4).ToArray<byte>();
				}
			}

			// Token: 0x170002C4 RID: 708
			// (get) Token: 0x06000A3E RID: 2622 RVA: 0x0003E5A4 File Offset: 0x0003C7A4
			public string FileData
			{
				get
				{
					if (this._fileData == null)
					{
						using (MemoryStream memoryStream = new MemoryStream(this.RawFileData))
						{
							using (StreamReader streamReader = new StreamReader(memoryStream, true))
							{
								this._fileData = streamReader.ReadToEnd();
							}
						}
					}
					return this._fileData;
				}
			}

			// Token: 0x040004A3 RID: 1187
			private string _fileData;
		}

		// Token: 0x020000BB RID: 187
		internal class CimModule
		{
			// Token: 0x06000A40 RID: 2624 RVA: 0x0003E61C File Offset: 0x0003C81C
			internal CimModule(CimInstance baseObject)
			{
				this._baseObject = baseObject;
			}

			// Token: 0x170002C5 RID: 709
			// (get) Token: 0x06000A41 RID: 2625 RVA: 0x0003E62C File Offset: 0x0003C82C
			public string ModuleName
			{
				get
				{
					string propertyValue = RemoteDiscoveryHelper.GetPropertyValue<string>(this._baseObject, "ModuleName", string.Empty);
					return Path.GetFileName(propertyValue);
				}
			}

			// Token: 0x170002C6 RID: 710
			// (get) Token: 0x06000A42 RID: 2626 RVA: 0x0003E658 File Offset: 0x0003C858
			public bool IsPsCimModule
			{
				get
				{
					ushort propertyValue = RemoteDiscoveryHelper.GetPropertyValue<ushort>(this._baseObject, "ModuleType", 0);
					RemoteDiscoveryHelper.CimModule.DiscoveredModuleType discoveredModuleType = (RemoteDiscoveryHelper.CimModule.DiscoveredModuleType)propertyValue;
					return discoveredModuleType == RemoteDiscoveryHelper.CimModule.DiscoveredModuleType.Cim;
				}
			}

			// Token: 0x170002C7 RID: 711
			// (get) Token: 0x06000A43 RID: 2627 RVA: 0x0003E680 File Offset: 0x0003C880
			public RemoteDiscoveryHelper.CimModuleFile MainManifest
			{
				get
				{
					byte[] propertyValue = RemoteDiscoveryHelper.GetPropertyValue<byte[]>(this._baseObject, "moduleManifestFileData", new byte[0]);
					return new RemoteDiscoveryHelper.CimModule.CimModuleManifestFile(this.ModuleName + ".psd1", propertyValue);
				}
			}

			// Token: 0x170002C8 RID: 712
			// (get) Token: 0x06000A44 RID: 2628 RVA: 0x0003E6BA File Offset: 0x0003C8BA
			public IEnumerable<RemoteDiscoveryHelper.CimModuleFile> ModuleFiles
			{
				get
				{
					return this._moduleFiles;
				}
			}

			// Token: 0x06000A45 RID: 2629 RVA: 0x0003E6CC File Offset: 0x0003C8CC
			internal void FetchAllModuleFiles(CimSession cimSession, string cimNamespace, CimOperationOptions operationOptions)
			{
				IEnumerable<CimInstance> source = cimSession.EnumerateAssociatedInstances(cimNamespace, this._baseObject, "PS_ModuleToModuleFile", "PS_ModuleFile", "Antecedent", "Dependent", operationOptions);
				IEnumerable<RemoteDiscoveryHelper.CimModuleFile> source2 = from i in source
				select new RemoteDiscoveryHelper.CimModule.CimModuleImplementationFile(i);
				this._moduleFiles = source2.ToList<RemoteDiscoveryHelper.CimModuleFile>();
			}

			// Token: 0x040004A4 RID: 1188
			private readonly CimInstance _baseObject;

			// Token: 0x040004A5 RID: 1189
			private List<RemoteDiscoveryHelper.CimModuleFile> _moduleFiles;

			// Token: 0x020000BC RID: 188
			private enum DiscoveredModuleType : ushort
			{
				// Token: 0x040004A8 RID: 1192
				Unknown,
				// Token: 0x040004A9 RID: 1193
				Cim
			}

			// Token: 0x020000BD RID: 189
			private class CimModuleManifestFile : RemoteDiscoveryHelper.CimModuleFile
			{
				// Token: 0x06000A47 RID: 2631 RVA: 0x0003E72C File Offset: 0x0003C92C
				internal CimModuleManifestFile(string fileName, byte[] rawFileData)
				{
					this._fileName = fileName;
					this._rawFileData = rawFileData;
				}

				// Token: 0x170002C9 RID: 713
				// (get) Token: 0x06000A48 RID: 2632 RVA: 0x0003E742 File Offset: 0x0003C942
				public override string FileName
				{
					get
					{
						return this._fileName;
					}
				}

				// Token: 0x170002CA RID: 714
				// (get) Token: 0x06000A49 RID: 2633 RVA: 0x0003E74A File Offset: 0x0003C94A
				internal override byte[] RawFileDataCore
				{
					get
					{
						return this._rawFileData;
					}
				}

				// Token: 0x040004AA RID: 1194
				private readonly string _fileName;

				// Token: 0x040004AB RID: 1195
				private readonly byte[] _rawFileData;
			}

			// Token: 0x020000BE RID: 190
			private class CimModuleImplementationFile : RemoteDiscoveryHelper.CimModuleFile
			{
				// Token: 0x06000A4A RID: 2634 RVA: 0x0003E752 File Offset: 0x0003C952
				internal CimModuleImplementationFile(CimInstance baseObject)
				{
					this._baseObject = baseObject;
				}

				// Token: 0x170002CB RID: 715
				// (get) Token: 0x06000A4B RID: 2635 RVA: 0x0003E764 File Offset: 0x0003C964
				public override string FileName
				{
					get
					{
						string propertyValue = RemoteDiscoveryHelper.GetPropertyValue<string>(this._baseObject, "FileName", string.Empty);
						return Path.GetFileName(propertyValue);
					}
				}

				// Token: 0x170002CC RID: 716
				// (get) Token: 0x06000A4C RID: 2636 RVA: 0x0003E78D File Offset: 0x0003C98D
				internal override byte[] RawFileDataCore
				{
					get
					{
						return RemoteDiscoveryHelper.GetPropertyValue<byte[]>(this._baseObject, "FileData", new byte[0]);
					}
				}

				// Token: 0x040004AC RID: 1196
				private readonly CimInstance _baseObject;
			}
		}
	}
}
