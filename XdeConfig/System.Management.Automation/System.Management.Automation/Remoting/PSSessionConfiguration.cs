using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Tracing;
using System.Reflection;
using System.Security;
using Microsoft.Win32;

namespace System.Management.Automation.Remoting
{
	// Token: 0x0200036A RID: 874
	public abstract class PSSessionConfiguration : IDisposable
	{
		// Token: 0x06002B1C RID: 11036
		public abstract InitialSessionState GetInitialSessionState(PSSenderInfo senderInfo);

		// Token: 0x06002B1D RID: 11037 RVA: 0x000ECFAC File Offset: 0x000EB1AC
		public virtual InitialSessionState GetInitialSessionState(PSSessionConfigurationData sessionConfigurationData, PSSenderInfo senderInfo, string configProviderId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002B1E RID: 11038 RVA: 0x000ECFB3 File Offset: 0x000EB1B3
		public virtual int? GetMaximumReceivedObjectSize(PSSenderInfo senderInfo)
		{
			return new int?(10485760);
		}

		// Token: 0x06002B1F RID: 11039 RVA: 0x000ECFBF File Offset: 0x000EB1BF
		public virtual int? GetMaximumReceivedDataSizePerCommand(PSSenderInfo senderInfo)
		{
			return new int?(52428800);
		}

		// Token: 0x06002B20 RID: 11040 RVA: 0x000ECFCB File Offset: 0x000EB1CB
		public virtual PSPrimitiveDictionary GetApplicationPrivateData(PSSenderInfo senderInfo)
		{
			return null;
		}

		// Token: 0x06002B21 RID: 11041 RVA: 0x000ECFCE File Offset: 0x000EB1CE
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x000ECFDD File Offset: 0x000EB1DD
		protected virtual void Dispose(bool isDisposing)
		{
		}

		// Token: 0x06002B23 RID: 11043 RVA: 0x000ECFE0 File Offset: 0x000EB1E0
		internal static ConfigurationDataFromXML LoadEndPointConfiguration(string shellId, string initializationParameters)
		{
			ConfigurationDataFromXML result = null;
			if (!PSSessionConfiguration.ssnStateProviders.ContainsKey(initializationParameters))
			{
				PSSessionConfiguration.LoadRSConfigProvider(shellId, initializationParameters);
			}
			lock (PSSessionConfiguration.syncObject)
			{
				if (!PSSessionConfiguration.ssnStateProviders.TryGetValue(initializationParameters, out result))
				{
					throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.NonExistentInitialSessionStateProvider, new object[]
					{
						shellId
					});
				}
			}
			return result;
		}

		// Token: 0x06002B24 RID: 11044 RVA: 0x000ED058 File Offset: 0x000EB258
		private static void LoadRSConfigProvider(string shellId, string initializationParameters)
		{
			ConfigurationDataFromXML configurationDataFromXML = ConfigurationDataFromXML.Create(initializationParameters);
			Type endPointConfigurationType = PSSessionConfiguration.LoadAndAnalyzeAssembly(shellId, configurationDataFromXML.ApplicationBase, configurationDataFromXML.AssemblyName, configurationDataFromXML.EndPointConfigurationTypeName);
			configurationDataFromXML.EndPointConfigurationType = endPointConfigurationType;
			lock (PSSessionConfiguration.syncObject)
			{
				if (!PSSessionConfiguration.ssnStateProviders.ContainsKey(initializationParameters))
				{
					PSSessionConfiguration.ssnStateProviders.Add(initializationParameters, configurationDataFromXML);
				}
			}
		}

		// Token: 0x06002B25 RID: 11045 RVA: 0x000ED0D4 File Offset: 0x000EB2D4
		private static Type LoadAndAnalyzeAssembly(string shellId, string applicationBase, string assemblyName, string typeToLoad)
		{
			if ((string.IsNullOrEmpty(assemblyName) && !string.IsNullOrEmpty(typeToLoad)) || (!string.IsNullOrEmpty(assemblyName) && string.IsNullOrEmpty(typeToLoad)))
			{
				throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.TypeNeedsAssembly, new object[]
				{
					"assemblyname",
					"pssessionconfigurationtypename",
					"InitializationParameters"
				});
			}
			Assembly assembly = null;
			if (!string.IsNullOrEmpty(assemblyName))
			{
				PSEtwLog.LogAnalyticVerbose(PSEventId.LoadingPSCustomShellAssembly, PSOpcode.Connect, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
				{
					assemblyName,
					shellId
				});
				assembly = PSSessionConfiguration.LoadSsnStateProviderAssembly(applicationBase, assemblyName);
				if (null == assembly)
				{
					throw PSTraceSource.NewArgumentException("assemblyName", RemotingErrorIdStrings.UnableToLoadAssembly, new object[]
					{
						assemblyName,
						"InitializationParameters"
					});
				}
			}
			if (null != assembly)
			{
				try
				{
					PSEtwLog.LogAnalyticVerbose(PSEventId.LoadingPSCustomShellType, PSOpcode.Connect, PSTask.None, (PSKeyword)4611686018427387912UL, new object[]
					{
						typeToLoad,
						shellId
					});
					Type type = assembly.GetType(typeToLoad, true, true);
					if (null == type)
					{
						throw PSTraceSource.NewArgumentException("typeToLoad", RemotingErrorIdStrings.UnableToLoadType, new object[]
						{
							typeToLoad,
							"InitializationParameters"
						});
					}
					return type;
				}
				catch (ReflectionTypeLoadException)
				{
				}
				catch (TypeLoadException)
				{
				}
				catch (ArgumentException)
				{
				}
				catch (MissingMethodException)
				{
				}
				catch (InvalidCastException)
				{
				}
				catch (TargetInvocationException)
				{
				}
				throw PSTraceSource.NewArgumentException("typeToLoad", RemotingErrorIdStrings.UnableToLoadType, new object[]
				{
					typeToLoad,
					"InitializationParameters"
				});
			}
			return typeof(DefaultRemotePowerShellConfiguration);
		}

		// Token: 0x06002B26 RID: 11046 RVA: 0x000ED2A0 File Offset: 0x000EB4A0
		private static Assembly LoadSsnStateProviderAssembly(string applicationBase, string assemblyName)
		{
			string currentDirectory = string.Empty;
			if (!string.IsNullOrEmpty(applicationBase))
			{
				try
				{
					currentDirectory = Directory.GetCurrentDirectory();
					Directory.SetCurrentDirectory(applicationBase);
				}
				catch (ArgumentException ex)
				{
					PSSessionConfiguration.tracer.TraceWarning("Not able to change curent working directory to {0}: {1}", new object[]
					{
						applicationBase,
						ex.Message
					});
				}
				catch (PathTooLongException ex2)
				{
					PSSessionConfiguration.tracer.TraceWarning("Not able to change curent working directory to {0}: {1}", new object[]
					{
						applicationBase,
						ex2.Message
					});
				}
				catch (FileNotFoundException ex3)
				{
					PSSessionConfiguration.tracer.TraceWarning("Not able to change curent working directory to {0}: {1}", new object[]
					{
						applicationBase,
						ex3.Message
					});
				}
				catch (IOException ex4)
				{
					PSSessionConfiguration.tracer.TraceWarning("Not able to change curent working directory to {0}: {1}", new object[]
					{
						applicationBase,
						ex4.Message
					});
				}
				catch (SecurityException ex5)
				{
					PSSessionConfiguration.tracer.TraceWarning("Not able to change curent working directory to {0}: {1}", new object[]
					{
						applicationBase,
						ex5.Message
					});
				}
				catch (UnauthorizedAccessException ex6)
				{
					PSSessionConfiguration.tracer.TraceWarning("Not able to change curent working directory to {0}: {1}", new object[]
					{
						applicationBase,
						ex6.Message
					});
				}
			}
			Assembly assembly = null;
			try
			{
				try
				{
					assembly = Assembly.Load(new AssemblyName(assemblyName));
				}
				catch (FileLoadException ex7)
				{
					PSSessionConfiguration.tracer.TraceWarning("Not able to load assembly {0}: {1}", new object[]
					{
						assemblyName,
						ex7.Message
					});
				}
				catch (BadImageFormatException ex8)
				{
					PSSessionConfiguration.tracer.TraceWarning("Not able to load assembly {0}: {1}", new object[]
					{
						assemblyName,
						ex8.Message
					});
				}
				catch (FileNotFoundException ex9)
				{
					PSSessionConfiguration.tracer.TraceWarning("Not able to load assembly {0}: {1}", new object[]
					{
						assemblyName,
						ex9.Message
					});
				}
				if (null != assembly)
				{
					return assembly;
				}
				PSSessionConfiguration.tracer.WriteLine("Loading assembly from path {0}", new object[]
				{
					applicationBase
				});
				try
				{
					assembly = ClrFacade.LoadFrom(assemblyName);
				}
				catch (FileLoadException ex10)
				{
					PSSessionConfiguration.tracer.TraceWarning("Not able to load assembly {0}: {1}", new object[]
					{
						assemblyName,
						ex10.Message
					});
				}
				catch (BadImageFormatException ex11)
				{
					PSSessionConfiguration.tracer.TraceWarning("Not able to load assembly {0}: {1}", new object[]
					{
						assemblyName,
						ex11.Message
					});
				}
				catch (FileNotFoundException ex12)
				{
					PSSessionConfiguration.tracer.TraceWarning("Not able to load assembly {0}: {1}", new object[]
					{
						assemblyName,
						ex12.Message
					});
				}
			}
			finally
			{
				if (!string.IsNullOrEmpty(applicationBase))
				{
					Directory.SetCurrentDirectory(currentDirectory);
				}
			}
			return assembly;
		}

		// Token: 0x06002B27 RID: 11047 RVA: 0x000ED694 File Offset: 0x000EB894
		private static RegistryKey GetConfigurationProvidersRegistryKey()
		{
			try
			{
				RegistryKey monadRootKey = PSSnapInReader.GetMonadRootKey();
				RegistryKey versionRootKey = PSSnapInReader.GetVersionRootKey(monadRootKey, Utils.GetCurrentMajorVersion());
				return versionRootKey.OpenSubKey("PSConfigurationProviders");
			}
			catch (ArgumentException)
			{
			}
			catch (SecurityException)
			{
			}
			return null;
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x000ED6E8 File Offset: 0x000EB8E8
		private static string ReadStringValue(RegistryKey registryKey, string name, bool mandatory)
		{
			object value = registryKey.GetValue(name);
			if (value == null && mandatory)
			{
				PSSessionConfiguration.tracer.TraceError("Mandatory property {0} not specified for registry key {1}", new object[]
				{
					name,
					registryKey.Name
				});
				throw PSTraceSource.NewArgumentException("name", RemotingErrorIdStrings.MandatoryValueNotPresent, new object[]
				{
					name,
					registryKey.Name
				});
			}
			string text = value as string;
			if (string.IsNullOrEmpty(text) && mandatory)
			{
				PSSessionConfiguration.tracer.TraceError("Value is null or empty for mandatory property {0} in {1}", new object[]
				{
					name,
					registryKey.Name
				});
				throw PSTraceSource.NewArgumentException("name", RemotingErrorIdStrings.MandatoryValueNotInCorrectFormat, new object[]
				{
					name,
					registryKey.Name
				});
			}
			return text;
		}

		// Token: 0x0400158E RID: 5518
		private const string configProvidersKeyName = "PSConfigurationProviders";

		// Token: 0x0400158F RID: 5519
		private const string configProviderApplicationBaseKeyName = "ApplicationBase";

		// Token: 0x04001590 RID: 5520
		private const string configProviderAssemblyNameKeyName = "AssemblyName";

		// Token: 0x04001591 RID: 5521
		[TraceSource("ServerRemoteSession", "ServerRemoteSession")]
		private static readonly PSTraceSource tracer = PSTraceSource.GetTracer("ServerRemoteSession", "ServerRemoteSession");

		// Token: 0x04001592 RID: 5522
		private static Dictionary<string, ConfigurationDataFromXML> ssnStateProviders = new Dictionary<string, ConfigurationDataFromXML>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04001593 RID: 5523
		private static object syncObject = new object();
	}
}
