using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security;
using System.Text;
using Microsoft.PowerShell.Commands;
using Microsoft.Win32;

namespace System.Management.Automation
{
	// Token: 0x0200084B RID: 2123
	internal static class PSSnapInReader
	{
		// Token: 0x060051AC RID: 20908 RVA: 0x001B2C7C File Offset: 0x001B0E7C
		internal static Collection<PSSnapInInfo> ReadAll()
		{
			Collection<PSSnapInInfo> collection = new Collection<PSSnapInInfo>();
			RegistryKey monadRootKey = PSSnapInReader.GetMonadRootKey();
			string[] subKeyNames = monadRootKey.GetSubKeyNames();
			if (subKeyNames == null)
			{
				return collection;
			}
			Collection<string> collection2 = new Collection<string>();
			foreach (string text in subKeyNames)
			{
				string text2 = PSVersionInfo.GetRegisteryVersionKeyForSnapinDiscovery(text);
				if (string.IsNullOrEmpty(text2))
				{
					text2 = text;
				}
				if (!collection2.Contains(text2))
				{
					collection2.Add(text2);
				}
			}
			foreach (string text3 in collection2)
			{
				if (!string.IsNullOrEmpty(text3) && PSSnapInReader.MeetsVersionFormat(text3))
				{
					Collection<PSSnapInInfo> collection3 = null;
					try
					{
						collection3 = PSSnapInReader.ReadAll(monadRootKey, text3);
					}
					catch (SecurityException)
					{
					}
					catch (ArgumentException)
					{
					}
					if (collection3 != null)
					{
						foreach (PSSnapInInfo item in collection3)
						{
							collection.Add(item);
						}
					}
				}
			}
			return collection;
		}

		// Token: 0x060051AD RID: 20909 RVA: 0x001B2DAC File Offset: 0x001B0FAC
		private static bool MeetsVersionFormat(string version)
		{
			bool result = true;
			try
			{
				LanguagePrimitives.ConvertTo(version, typeof(int), CultureInfo.InvariantCulture);
			}
			catch (PSInvalidCastException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060051AE RID: 20910 RVA: 0x001B2DEC File Offset: 0x001B0FEC
		internal static Collection<PSSnapInInfo> ReadAll(string psVersion)
		{
			if (string.IsNullOrEmpty(psVersion))
			{
				throw PSTraceSource.NewArgumentNullException("psVersion");
			}
			RegistryKey monadRootKey = PSSnapInReader.GetMonadRootKey();
			return PSSnapInReader.ReadAll(monadRootKey, psVersion);
		}

		// Token: 0x060051AF RID: 20911 RVA: 0x001B2E1C File Offset: 0x001B101C
		private static Collection<PSSnapInInfo> ReadAll(RegistryKey monadRootKey, string psVersion)
		{
			Collection<PSSnapInInfo> collection = new Collection<PSSnapInInfo>();
			RegistryKey versionRootKey = PSSnapInReader.GetVersionRootKey(monadRootKey, psVersion);
			RegistryKey mshSnapinRootKey = PSSnapInReader.GetMshSnapinRootKey(versionRootKey, psVersion);
			string[] subKeyNames = mshSnapinRootKey.GetSubKeyNames();
			foreach (string text in subKeyNames)
			{
				if (!string.IsNullOrEmpty(text))
				{
					try
					{
						collection.Add(PSSnapInReader.ReadOne(mshSnapinRootKey, text));
					}
					catch (SecurityException)
					{
					}
					catch (ArgumentException)
					{
					}
				}
			}
			return collection;
		}

		// Token: 0x060051B0 RID: 20912 RVA: 0x001B2EA0 File Offset: 0x001B10A0
		internal static PSSnapInInfo Read(string psVersion, string mshsnapinId)
		{
			if (string.IsNullOrEmpty(psVersion))
			{
				throw PSTraceSource.NewArgumentNullException("psVersion");
			}
			if (string.IsNullOrEmpty(mshsnapinId))
			{
				throw PSTraceSource.NewArgumentNullException("mshsnapinId");
			}
			PSSnapInInfo.VerifyPSSnapInFormatThrowIfError(mshsnapinId);
			RegistryKey monadRootKey = PSSnapInReader.GetMonadRootKey();
			RegistryKey versionRootKey = PSSnapInReader.GetVersionRootKey(monadRootKey, psVersion);
			RegistryKey mshSnapinRootKey = PSSnapInReader.GetMshSnapinRootKey(versionRootKey, psVersion);
			return PSSnapInReader.ReadOne(mshSnapinRootKey, mshsnapinId);
		}

		// Token: 0x060051B1 RID: 20913 RVA: 0x001B2EF8 File Offset: 0x001B10F8
		private static PSSnapInInfo ReadOne(RegistryKey mshSnapInRoot, string mshsnapinId)
		{
			RegistryKey registryKey = mshSnapInRoot.OpenSubKey(mshsnapinId);
			if (registryKey == null)
			{
				PSSnapInReader._mshsnapinTracer.TraceError("Error opening registry key {0}\\{1}.", new object[]
				{
					mshSnapInRoot.Name,
					mshsnapinId
				});
				throw PSTraceSource.NewArgumentException("mshsnapinId", MshSnapinInfo.MshSnapinDoesNotExist, new object[]
				{
					mshsnapinId
				});
			}
			string applicationBase = PSSnapInReader.ReadStringValue(registryKey, "ApplicationBase", true);
			string assemblyName = PSSnapInReader.ReadStringValue(registryKey, "AssemblyName", true);
			string moduleName = PSSnapInReader.ReadStringValue(registryKey, "ModuleName", true);
			Version psVersion = PSSnapInReader.ReadVersionValue(registryKey, "PowerShellVersion", true);
			Version version = PSSnapInReader.ReadVersionValue(registryKey, "Version", false);
			string text = PSSnapInReader.ReadStringValue(registryKey, "Description", false);
			if (text == null)
			{
				PSSnapInReader._mshsnapinTracer.WriteLine("No description is specified for mshsnapin {0}. Using empty string for description.", new object[]
				{
					mshsnapinId
				});
				text = string.Empty;
			}
			string text2 = PSSnapInReader.ReadStringValue(registryKey, "Vendor", false);
			if (text2 == null)
			{
				PSSnapInReader._mshsnapinTracer.WriteLine("No vendor is specified for mshsnapin {0}. Using empty string for description.", new object[]
				{
					mshsnapinId
				});
				text2 = string.Empty;
			}
			bool logPipelineExecutionDetails = false;
			string text3 = PSSnapInReader.ReadStringValue(registryKey, "LogPipelineExecutionDetails", false);
			if (!string.IsNullOrEmpty(text3) && string.Compare("1", text3, StringComparison.OrdinalIgnoreCase) == 0)
			{
				logPipelineExecutionDetails = true;
			}
			string text4 = PSSnapInReader.ReadStringValue(registryKey, "CustomPSSnapInType", false);
			if (string.IsNullOrEmpty(text4))
			{
				text4 = null;
			}
			Collection<string> types = PSSnapInReader.ReadMultiStringValue(registryKey, "Types", false);
			Collection<string> formats = PSSnapInReader.ReadMultiStringValue(registryKey, "Formats", false);
			PSSnapInReader._mshsnapinTracer.WriteLine("Successfully read registry values for mshsnapin {0}. Constructing PSSnapInInfo object.", new object[]
			{
				mshsnapinId
			});
			return new PSSnapInInfo(mshsnapinId, false, applicationBase, assemblyName, moduleName, psVersion, version, types, formats, text, text2, text4)
			{
				LogPipelineExecutionDetails = logPipelineExecutionDetails
			};
		}

		// Token: 0x060051B2 RID: 20914 RVA: 0x001B30B0 File Offset: 0x001B12B0
		private static Collection<string> ReadMultiStringValue(RegistryKey mshsnapinKey, string name, bool mandatory)
		{
			object value = mshsnapinKey.GetValue(name);
			if (value == null)
			{
				if (mandatory)
				{
					PSSnapInReader._mshsnapinTracer.TraceError("Mandatory property {0} not specified for registry key {1}", new object[]
					{
						name,
						mshsnapinKey.Name
					});
					throw PSTraceSource.NewArgumentException("name", MshSnapinInfo.MandatoryValueNotPresent, new object[]
					{
						name,
						mshsnapinKey.Name
					});
				}
				return null;
			}
			else
			{
				string[] array = value as string[];
				if (array == null)
				{
					string text = value as string;
					if (text != null)
					{
						array = new string[]
						{
							text
						};
					}
				}
				if (array != null)
				{
					PSSnapInReader._mshsnapinTracer.WriteLine("Successfully read property {0} from {1}", new object[]
					{
						name,
						mshsnapinKey.Name
					});
					return new Collection<string>(array);
				}
				if (mandatory)
				{
					PSSnapInReader._mshsnapinTracer.TraceError("Cannot get string/multi-string value for mandatory property {0} in registry key {1}", new object[]
					{
						name,
						mshsnapinKey.Name
					});
					throw PSTraceSource.NewArgumentException("name", MshSnapinInfo.MandatoryValueNotInCorrectFormatMultiString, new object[]
					{
						name,
						mshsnapinKey.Name
					});
				}
				return null;
			}
		}

		// Token: 0x060051B3 RID: 20915 RVA: 0x001B31C0 File Offset: 0x001B13C0
		internal static string ReadStringValue(RegistryKey mshsnapinKey, string name, bool mandatory)
		{
			object value = mshsnapinKey.GetValue(name);
			if (value == null && mandatory)
			{
				PSSnapInReader._mshsnapinTracer.TraceError("Mandatory property {0} not specified for registry key {1}", new object[]
				{
					name,
					mshsnapinKey.Name
				});
				throw PSTraceSource.NewArgumentException("name", MshSnapinInfo.MandatoryValueNotPresent, new object[]
				{
					name,
					mshsnapinKey.Name
				});
			}
			string text = value as string;
			if (string.IsNullOrEmpty(text) && mandatory)
			{
				PSSnapInReader._mshsnapinTracer.TraceError("Value is null or empty for mandatory property {0} in {1}", new object[]
				{
					name,
					mshsnapinKey.Name
				});
				throw PSTraceSource.NewArgumentException("name", MshSnapinInfo.MandatoryValueNotInCorrectFormat, new object[]
				{
					name,
					mshsnapinKey.Name
				});
			}
			PSSnapInReader._mshsnapinTracer.WriteLine("Successfully read value {0} for property {1} from {2}", new object[]
			{
				text,
				name,
				mshsnapinKey.Name
			});
			return text;
		}

		// Token: 0x060051B4 RID: 20916 RVA: 0x001B32B8 File Offset: 0x001B14B8
		internal static Version ReadVersionValue(RegistryKey mshsnapinKey, string name, bool mandatory)
		{
			string text = PSSnapInReader.ReadStringValue(mshsnapinKey, name, mandatory);
			if (text == null)
			{
				PSSnapInReader._mshsnapinTracer.TraceError("Cannot read value for property {0} in registry key {1}", new object[]
				{
					name,
					mshsnapinKey.ToString()
				});
				return null;
			}
			Version version;
			try
			{
				version = new Version(text);
			}
			catch (ArgumentOutOfRangeException)
			{
				PSSnapInReader._mshsnapinTracer.TraceError("Cannot convert value {0} to version format", new object[]
				{
					text
				});
				throw PSTraceSource.NewArgumentException("name", MshSnapinInfo.VersionValueInCorrect, new object[]
				{
					name,
					mshsnapinKey.Name
				});
			}
			catch (ArgumentException)
			{
				PSSnapInReader._mshsnapinTracer.TraceError("Cannot convert value {0} to version format", new object[]
				{
					text
				});
				throw PSTraceSource.NewArgumentException("name", MshSnapinInfo.VersionValueInCorrect, new object[]
				{
					name,
					mshsnapinKey.Name
				});
			}
			catch (OverflowException)
			{
				PSSnapInReader._mshsnapinTracer.TraceError("Cannot convert value {0} to version format", new object[]
				{
					text
				});
				throw PSTraceSource.NewArgumentException("name", MshSnapinInfo.VersionValueInCorrect, new object[]
				{
					name,
					mshsnapinKey.Name
				});
			}
			catch (FormatException)
			{
				PSSnapInReader._mshsnapinTracer.TraceError("Cannot convert value {0} to version format", new object[]
				{
					text
				});
				throw PSTraceSource.NewArgumentException("name", MshSnapinInfo.VersionValueInCorrect, new object[]
				{
					name,
					mshsnapinKey.Name
				});
			}
			PSSnapInReader._mshsnapinTracer.WriteLine("Successfully converted string {0} to version format.", new object[]
			{
				version.ToString()
			});
			return version;
		}

		// Token: 0x060051B5 RID: 20917 RVA: 0x001B3478 File Offset: 0x001B1678
		private static void ReadRegistryInfo(out Version assemblyVersion, out string publicKeyToken, out string culture, out string architecture, out string applicationBase, out Version psVersion)
		{
			applicationBase = Utils.GetApplicationBase(Utils.DefaultPowerShellShellID);
			psVersion = PSVersionInfo.PSVersion;
			Assembly assembly = typeof(PSSnapInReader).GetTypeInfo().Assembly;
			assemblyVersion = assembly.GetName().Version;
			byte[] publicKeyToken2 = assembly.GetName().GetPublicKeyToken();
			if (publicKeyToken2.Length == 0)
			{
				throw PSTraceSource.NewArgumentException("PublicKeyToken", MshSnapinInfo.PublicKeyTokenAccessFailed, new object[0]);
			}
			publicKeyToken = PSSnapInReader.ConvertByteArrayToString(publicKeyToken2);
			culture = "neutral";
			architecture = "MSIL";
		}

		// Token: 0x060051B6 RID: 20918 RVA: 0x001B34FC File Offset: 0x001B16FC
		internal static string ConvertByteArrayToString(byte[] tokens)
		{
			StringBuilder stringBuilder = new StringBuilder(tokens.Length * 2);
			foreach (byte b in tokens)
			{
				stringBuilder.Append(b.ToString("x2", CultureInfo.InvariantCulture));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060051B7 RID: 20919 RVA: 0x001B3548 File Offset: 0x001B1748
		internal static PSSnapInInfo ReadCoreEngineSnapIn()
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			Version version;
			Version psVersion;
			PSSnapInReader.ReadRegistryInfo(out version, out text, out text2, out text3, out text4, out psVersion);
			Collection<string> types = new Collection<string>(new string[]
			{
				"types.ps1xml",
				"typesv3.ps1xml"
			});
			Collection<string> formats = new Collection<string>(new string[]
			{
				"Certificate.format.ps1xml",
				"DotNetTypes.format.ps1xml",
				"FileSystem.format.ps1xml",
				"Help.format.ps1xml",
				"HelpV3.format.ps1xml",
				"PowerShellCore.format.ps1xml",
				"PowerShellTrace.format.ps1xml",
				"Registry.format.ps1xml"
			});
			string assemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, Version={1}, Culture={2}, PublicKeyToken={3}, ProcessorArchitecture={4}", new object[]
			{
				PSSnapInReader.CoreSnapin.AssemblyName,
				version,
				text2,
				text,
				text3
			});
			string moduleName = Path.Combine(text4, PSSnapInReader.CoreSnapin.AssemblyName + ".dll");
			PSSnapInInfo pssnapInInfo = new PSSnapInInfo(PSSnapInReader.CoreSnapin.PSSnapInName, true, text4, assemblyName, moduleName, psVersion, version, types, formats, null, PSSnapInReader.CoreSnapin.Description, PSSnapInReader.CoreSnapin.DescriptionIndirect, null, null, PSSnapInReader.CoreSnapin.VendorIndirect, null);
			PSSnapInReader.SetSnapInLoggingInformation(pssnapInInfo);
			return pssnapInInfo;
		}

		// Token: 0x060051B8 RID: 20920 RVA: 0x001B3690 File Offset: 0x001B1890
		internal static Collection<PSSnapInInfo> ReadEnginePSSnapIns()
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			Version version;
			Version psVersion;
			PSSnapInReader.ReadRegistryInfo(out version, out text, out text2, out text3, out text4, out psVersion);
			Collection<string> collection = new Collection<string>(new string[]
			{
				"Certificate.format.ps1xml",
				"DotNetTypes.format.ps1xml",
				"FileSystem.format.ps1xml",
				"Help.format.ps1xml",
				"HelpV3.format.ps1xml",
				"PowerShellCore.format.ps1xml",
				"PowerShellTrace.format.ps1xml",
				"Registry.format.ps1xml"
			});
			Collection<string> collection2 = new Collection<string>(new string[]
			{
				"types.ps1xml",
				"typesv3.ps1xml"
			});
			Collection<PSSnapInInfo> collection3 = new Collection<PSSnapInInfo>();
			for (int i = 0; i < PSSnapInReader.DefaultMshSnapins.Count; i++)
			{
				PSSnapInReader.DefaultPSSnapInInformation defaultPSSnapInInformation = PSSnapInReader.DefaultMshSnapins[i];
				string assemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, Version={1}, Culture={2}, PublicKeyToken={3}, ProcessorArchitecture={4}", new object[]
				{
					defaultPSSnapInInformation.AssemblyName,
					version.ToString(),
					text2,
					text,
					text3
				});
				Collection<string> formats = null;
				Collection<string> types = null;
				if (defaultPSSnapInInformation.AssemblyName.Equals("System.Management.Automation", StringComparison.OrdinalIgnoreCase))
				{
					formats = collection;
					types = collection2;
				}
				else if (defaultPSSnapInInformation.AssemblyName.Equals("Microsoft.PowerShell.Commands.Diagnostics", StringComparison.OrdinalIgnoreCase))
				{
					types = new Collection<string>(new string[]
					{
						"GetEvent.types.ps1xml"
					});
					formats = new Collection<string>(new string[]
					{
						"Event.Format.ps1xml",
						"Diagnostics.Format.ps1xml"
					});
				}
				else if (defaultPSSnapInInformation.AssemblyName.Equals("Microsoft.WSMan.Management", StringComparison.OrdinalIgnoreCase))
				{
					formats = new Collection<string>(new string[]
					{
						"WSMan.format.ps1xml"
					});
				}
				string text5 = Path.Combine(text4, defaultPSSnapInInformation.AssemblyName + ".dll");
				if (File.Exists(text5))
				{
					text5 = Path.Combine(text4, defaultPSSnapInInformation.AssemblyName + ".dll");
				}
				else
				{
					text5 = defaultPSSnapInInformation.AssemblyName;
				}
				PSSnapInInfo pssnapInInfo = new PSSnapInInfo(defaultPSSnapInInformation.PSSnapInName, true, text4, assemblyName, text5, psVersion, version, types, formats, null, defaultPSSnapInInformation.Description, defaultPSSnapInInformation.DescriptionIndirect, null, null, defaultPSSnapInInformation.VendorIndirect, null);
				PSSnapInReader.SetSnapInLoggingInformation(pssnapInInfo);
				collection3.Add(pssnapInInfo);
			}
			return collection3;
		}

		// Token: 0x060051B9 RID: 20921 RVA: 0x001B38E0 File Offset: 0x001B1AE0
		private static void SetSnapInLoggingInformation(PSSnapInInfo psSnapInInfo)
		{
			IEnumerable<string> moduleOrSnapinNames;
			ModuleCmdletBase.ModuleLoggingGroupPolicyStatus moduleLoggingInformation = ModuleCmdletBase.GetModuleLoggingInformation(out moduleOrSnapinNames);
			if (moduleLoggingInformation != ModuleCmdletBase.ModuleLoggingGroupPolicyStatus.Undefined)
			{
				PSSnapInReader.SetSnapInLoggingInformation(psSnapInInfo, moduleLoggingInformation, moduleOrSnapinNames);
			}
		}

		// Token: 0x060051BA RID: 20922 RVA: 0x001B3900 File Offset: 0x001B1B00
		private static void SetSnapInLoggingInformation(PSSnapInInfo psSnapInInfo, ModuleCmdletBase.ModuleLoggingGroupPolicyStatus status, IEnumerable<string> moduleOrSnapinNames)
		{
			if ((status & ModuleCmdletBase.ModuleLoggingGroupPolicyStatus.Enabled) != ModuleCmdletBase.ModuleLoggingGroupPolicyStatus.Undefined && moduleOrSnapinNames != null)
			{
				foreach (string text in moduleOrSnapinNames)
				{
					if (string.Equals(psSnapInInfo.Name, text, StringComparison.OrdinalIgnoreCase))
					{
						psSnapInInfo.LogPipelineExecutionDetails = true;
					}
					else if (WildcardPattern.ContainsWildcardCharacters(text))
					{
						WildcardPattern wildcardPattern = new WildcardPattern(text, WildcardOptions.IgnoreCase);
						if (wildcardPattern.IsMatch(psSnapInInfo.Name))
						{
							psSnapInInfo.LogPipelineExecutionDetails = true;
						}
					}
				}
			}
		}

		// Token: 0x060051BB RID: 20923 RVA: 0x001B3988 File Offset: 0x001B1B88
		internal static RegistryKey GetMonadRootKey()
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\PowerShell");
			if (registryKey == null)
			{
				throw PSTraceSource.NewArgumentException("monad", MshSnapinInfo.MonadRootRegistryAccessFailed, new object[0]);
			}
			return registryKey;
		}

		// Token: 0x060051BC RID: 20924 RVA: 0x001B39C0 File Offset: 0x001B1BC0
		internal static RegistryKey GetPSEngineKey(string psVersion)
		{
			RegistryKey monadRootKey = PSSnapInReader.GetMonadRootKey();
			PSSnapInReader.GetVersionRootKey(monadRootKey, psVersion);
			RegistryKey registryKey = monadRootKey.OpenSubKey(psVersion);
			if (registryKey == null)
			{
				throw PSTraceSource.NewArgumentException("monad", MshSnapinInfo.MonadEngineRegistryAccessFailed, new object[0]);
			}
			RegistryKey registryKey2 = registryKey.OpenSubKey("PowerShellEngine");
			if (registryKey2 == null)
			{
				throw PSTraceSource.NewArgumentException("monad", MshSnapinInfo.MonadEngineRegistryAccessFailed, new object[0]);
			}
			return registryKey2;
		}

		// Token: 0x060051BD RID: 20925 RVA: 0x001B3A24 File Offset: 0x001B1C24
		internal static RegistryKey GetVersionRootKey(RegistryKey rootKey, string psVersion)
		{
			string registeryVersionKeyForSnapinDiscovery = PSVersionInfo.GetRegisteryVersionKeyForSnapinDiscovery(psVersion);
			RegistryKey registryKey = rootKey.OpenSubKey(registeryVersionKeyForSnapinDiscovery);
			if (registryKey == null)
			{
				throw PSTraceSource.NewArgumentException("psVersion", MshSnapinInfo.SpecifiedVersionNotFound, new object[]
				{
					registeryVersionKeyForSnapinDiscovery
				});
			}
			return registryKey;
		}

		// Token: 0x060051BE RID: 20926 RVA: 0x001B3A60 File Offset: 0x001B1C60
		private static RegistryKey GetMshSnapinRootKey(RegistryKey versionRootKey, string psVersion)
		{
			RegistryKey registryKey = versionRootKey.OpenSubKey("PowerShellSnapIns");
			if (registryKey == null)
			{
				throw PSTraceSource.NewArgumentException("psVersion", MshSnapinInfo.NoMshSnapinPresentForVersion, new object[]
				{
					psVersion
				});
			}
			return registryKey;
		}

		// Token: 0x060051BF RID: 20927 RVA: 0x001B3A9C File Offset: 0x001B1C9C
		internal static RegistryKey GetMshSnapinKey(string mshSnapInName, string psVersion)
		{
			RegistryKey monadRootKey = PSSnapInReader.GetMonadRootKey();
			RegistryKey versionRootKey = PSSnapInReader.GetVersionRootKey(monadRootKey, psVersion);
			RegistryKey registryKey = versionRootKey.OpenSubKey("PowerShellSnapIns");
			if (registryKey == null)
			{
				throw PSTraceSource.NewArgumentException("psVersion", MshSnapinInfo.NoMshSnapinPresentForVersion, new object[]
				{
					psVersion
				});
			}
			return registryKey.OpenSubKey(mshSnapInName);
		}

		// Token: 0x170010CF RID: 4303
		// (get) Token: 0x060051C0 RID: 20928 RVA: 0x001B3AF0 File Offset: 0x001B1CF0
		private static IList<PSSnapInReader.DefaultPSSnapInInformation> DefaultMshSnapins
		{
			get
			{
				if (PSSnapInReader.defaultMshSnapins == null)
				{
					lock (PSSnapInReader._syncObject)
					{
						if (PSSnapInReader.defaultMshSnapins == null)
						{
							PSSnapInReader.defaultMshSnapins = new List<PSSnapInReader.DefaultPSSnapInInformation>
							{
								new PSSnapInReader.DefaultPSSnapInInformation("Microsoft.PowerShell.Diagnostics", "Microsoft.PowerShell.Commands.Diagnostics", null, "GetEventResources,Description", "GetEventResources,Vendor"),
								new PSSnapInReader.DefaultPSSnapInInformation("Microsoft.PowerShell.Host", "Microsoft.PowerShell.ConsoleHost", null, "HostMshSnapInResources,Description", "HostMshSnapInResources,Vendor"),
								PSSnapInReader.CoreSnapin,
								new PSSnapInReader.DefaultPSSnapInInformation("Microsoft.PowerShell.Utility", "Microsoft.PowerShell.Commands.Utility", null, "UtilityMshSnapInResources,Description", "UtilityMshSnapInResources,Vendor"),
								new PSSnapInReader.DefaultPSSnapInInformation("Microsoft.PowerShell.Management", "Microsoft.PowerShell.Commands.Management", null, "ManagementMshSnapInResources,Description", "ManagementMshSnapInResources,Vendor"),
								new PSSnapInReader.DefaultPSSnapInInformation("Microsoft.PowerShell.Security", "Microsoft.PowerShell.Security", null, "SecurityMshSnapInResources,Description", "SecurityMshSnapInResources,Vendor")
							};
							if (!RemotingCommandUtil.IsWinPEHost())
							{
								PSSnapInReader.defaultMshSnapins.Add(new PSSnapInReader.DefaultPSSnapInInformation("Microsoft.WSMan.Management", "Microsoft.WSMan.Management", null, "WsManResources,Description", "WsManResources,Vendor"));
							}
						}
					}
				}
				return PSSnapInReader.defaultMshSnapins;
			}
		}

		// Token: 0x04002A05 RID: 10757
		private static PSSnapInReader.DefaultPSSnapInInformation CoreSnapin = new PSSnapInReader.DefaultPSSnapInInformation("Microsoft.PowerShell.Core", "System.Management.Automation", null, "CoreMshSnapInResources,Description", "CoreMshSnapInResources,Vendor");

		// Token: 0x04002A06 RID: 10758
		private static IList<PSSnapInReader.DefaultPSSnapInInformation> defaultMshSnapins = null;

		// Token: 0x04002A07 RID: 10759
		private static object _syncObject = new object();

		// Token: 0x04002A08 RID: 10760
		private static PSTraceSource _mshsnapinTracer = PSTraceSource.GetTracer("MshSnapinLoadUnload", "Loading and unloading mshsnapins", false);

		// Token: 0x0200084C RID: 2124
		private struct DefaultPSSnapInInformation
		{
			// Token: 0x060051C2 RID: 20930 RVA: 0x001B3C75 File Offset: 0x001B1E75
			public DefaultPSSnapInInformation(string sName, string sAssemblyName, string sDescription, string sDescriptionIndirect, string sVendorIndirect)
			{
				this.PSSnapInName = sName;
				this.AssemblyName = sAssemblyName;
				this.Description = sDescription;
				this.DescriptionIndirect = sDescriptionIndirect;
				this.VendorIndirect = sVendorIndirect;
			}

			// Token: 0x04002A09 RID: 10761
			public string PSSnapInName;

			// Token: 0x04002A0A RID: 10762
			public string AssemblyName;

			// Token: 0x04002A0B RID: 10763
			public string Description;

			// Token: 0x04002A0C RID: 10764
			public string DescriptionIndirect;

			// Token: 0x04002A0D RID: 10765
			public string VendorIndirect;
		}
	}
}
