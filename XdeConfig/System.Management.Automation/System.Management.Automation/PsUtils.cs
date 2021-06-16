using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Automation.Language;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using Microsoft.Management.Infrastructure;
using Microsoft.PowerShell.Commands;
using Microsoft.Win32;

namespace System.Management.Automation
{
	// Token: 0x0200089C RID: 2204
	internal static class PsUtils
	{
		// Token: 0x0600546C RID: 21612 RVA: 0x001BDF1C File Offset: 0x001BC11C
		internal static ProcessModule GetMainModule(Process targetProcess)
		{
			int num = 0;
			ProcessModule processModule = null;
			while (processModule == null)
			{
				try
				{
					processModule = targetProcess.MainModule;
				}
				catch (Win32Exception ex)
				{
					if (ex.NativeErrorCode == 5)
					{
						throw;
					}
					num++;
					Thread.Sleep(100);
					if (num == 5)
					{
						throw;
					}
				}
			}
			return processModule;
		}

		// Token: 0x0600546D RID: 21613 RVA: 0x001BDF90 File Offset: 0x001BC190
		internal static Process GetParentProcess(Process current)
		{
			string queryExpression = string.Format(CultureInfo.CurrentCulture, "Select * From Win32_Process Where Handle='{0}'", new object[]
			{
				current.Id
			});
			Process result;
			using (CimSession cimSession = CimSession.Create(null))
			{
				IEnumerable<CimInstance> source = cimSession.QueryInstances("root/cimv2", "WQL", queryExpression);
				int num = (from cimProcess in source
				select Convert.ToInt32(cimProcess.CimInstanceProperties["ParentProcessId"].Value, CultureInfo.CurrentCulture)).FirstOrDefault<int>();
				if (num == 0)
				{
					result = null;
				}
				else
				{
					try
					{
						Process processById = Process.GetProcessById(num);
						if (processById.StartTime <= current.StartTime)
						{
							result = processById;
						}
						else
						{
							result = null;
						}
					}
					catch (ArgumentException)
					{
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x0600546E RID: 21614 RVA: 0x001BE068 File Offset: 0x001BC268
		internal static ProcessorArchitecture GetProcessorArchitecture(out bool isRunningOnArm)
		{
			PsUtils.NativeMethods.SYSTEM_INFO system_INFO = default(PsUtils.NativeMethods.SYSTEM_INFO);
			PsUtils.NativeMethods.GetSystemInfo(ref system_INFO);
			isRunningOnArm = false;
			ushort wProcessorArchitecture = system_INFO.wProcessorArchitecture;
			ProcessorArchitecture result;
			if (wProcessorArchitecture != 0)
			{
				switch (wProcessorArchitecture)
				{
				case 5:
					result = ProcessorArchitecture.None;
					isRunningOnArm = true;
					return result;
				case 6:
					return ProcessorArchitecture.IA64;
				case 9:
					return ProcessorArchitecture.Amd64;
				}
				result = ProcessorArchitecture.None;
			}
			else
			{
				result = ProcessorArchitecture.X86;
			}
			return result;
		}

		// Token: 0x0600546F RID: 21615 RVA: 0x001BE0C8 File Offset: 0x001BC2C8
		internal static bool IsRunningOnProcessorArchitectureARM()
		{
			PsUtils.NativeMethods.SYSTEM_INFO system_INFO = default(PsUtils.NativeMethods.SYSTEM_INFO);
			PsUtils.NativeMethods.GetSystemInfo(ref system_INFO);
			return system_INFO.wProcessorArchitecture == 5;
		}

		// Token: 0x06005470 RID: 21616 RVA: 0x001BE0F0 File Offset: 0x001BC2F0
		internal static string GetHostName()
		{
			IPGlobalProperties ipglobalProperties = IPGlobalProperties.GetIPGlobalProperties();
			string text = ipglobalProperties.HostName;
			if (!string.IsNullOrEmpty(ipglobalProperties.DomainName))
			{
				text = text + "." + ipglobalProperties.DomainName;
			}
			return text;
		}

		// Token: 0x06005471 RID: 21617 RVA: 0x001BE12A File Offset: 0x001BC32A
		internal static uint GetNativeThreadId()
		{
			return PsUtils.NativeMethods.GetCurrentThreadId();
		}

		// Token: 0x06005472 RID: 21618 RVA: 0x001BE134 File Offset: 0x001BC334
		internal static string GetUsingExpressionKey(UsingExpressionAst usingAst)
		{
			string text = usingAst.ToString();
			if (usingAst.SubExpression is VariableExpressionAst)
			{
				text = text.ToLowerInvariant();
			}
			return StringToBase64Converter.StringToBase64String(text);
		}

		// Token: 0x06005473 RID: 21619 RVA: 0x001BE164 File Offset: 0x001BC364
		internal static Hashtable EvaluatePowerShellDataFileAsModuleManifest(string parameterName, string psDataFilePath, ExecutionContext context, bool skipPathValidation)
		{
			return PsUtils.EvaluatePowerShellDataFile(parameterName, psDataFilePath, context, ModuleCmdletBase.PermittedCmdlets, new string[]
			{
				"PSScriptRoot"
			}, true, skipPathValidation);
		}

		// Token: 0x06005474 RID: 21620 RVA: 0x001BE190 File Offset: 0x001BC390
		internal static Hashtable EvaluatePowerShellDataFile(string parameterName, string psDataFilePath, ExecutionContext context, IEnumerable<string> allowedCommands, IEnumerable<string> allowedVariables, bool allowEnvironmentVariables, bool skipPathValidation)
		{
			if (!skipPathValidation && string.IsNullOrEmpty(parameterName))
			{
				throw PSTraceSource.NewArgumentNullException("parameterName");
			}
			if (string.IsNullOrEmpty(psDataFilePath))
			{
				throw PSTraceSource.NewArgumentNullException("psDataFilePath");
			}
			if (context == null)
			{
				throw PSTraceSource.NewArgumentNullException("context");
			}
			string path;
			if (skipPathValidation)
			{
				path = psDataFilePath;
			}
			else
			{
				bool flag = true;
				string extension = Path.GetExtension(psDataFilePath);
				if (string.IsNullOrEmpty(extension) || !".psd1".Equals(extension, StringComparison.OrdinalIgnoreCase))
				{
					flag = false;
				}
				ProviderInfo providerInfo;
				Collection<string> resolvedProviderPathFromPSPath = context.SessionState.Path.GetResolvedProviderPathFromPSPath(psDataFilePath, out providerInfo);
				if (providerInfo == null || !"FileSystem".Equals(providerInfo.Name, StringComparison.OrdinalIgnoreCase))
				{
					flag = false;
				}
				if (resolvedProviderPathFromPSPath.Count != 1)
				{
					flag = false;
				}
				if (!flag)
				{
					throw PSTraceSource.NewArgumentException(parameterName, ParserStrings.CannotResolvePowerShellDataFilePath, new object[]
					{
						psDataFilePath
					});
				}
				path = resolvedProviderPathFromPSPath[0];
			}
			object obj;
			try
			{
				string fileName = Path.GetFileName(path);
				ExternalScriptInfo externalScriptInfo = new ExternalScriptInfo(fileName, path, context);
				ScriptBlock scriptBlock = externalScriptInfo.ScriptBlock;
				scriptBlock.CheckRestrictedLanguage(allowedCommands, allowedVariables, allowEnvironmentVariables);
				object variableValue = context.GetVariableValue(SpecialVariables.PSScriptRootVarPath);
				try
				{
					context.SetVariable(SpecialVariables.PSScriptRootVarPath, Path.GetDirectoryName(path));
					obj = PSObject.Base(scriptBlock.InvokeReturnAsIs(new object[0]));
				}
				finally
				{
					context.SetVariable(SpecialVariables.PSScriptRootVarPath, variableValue);
				}
			}
			catch (RuntimeException ex)
			{
				throw PSTraceSource.NewInvalidOperationException(ex, ParserStrings.CannotLoadPowerShellDataFile, new object[]
				{
					psDataFilePath,
					ex.Message
				});
			}
			Hashtable hashtable = obj as Hashtable;
			if (hashtable == null)
			{
				throw PSTraceSource.NewInvalidOperationException(ParserStrings.InvalidPowerShellDataFile, new object[]
				{
					psDataFilePath
				});
			}
			return hashtable;
		}

		// Token: 0x06005475 RID: 21621 RVA: 0x001BE33C File Offset: 0x001BC53C
		internal static Hashtable GetModuleManifestProperties(string psDataFilePath, string[] keys)
		{
			string input = File.ReadAllText(psDataFilePath);
			ParseError[] array;
			ScriptBlockAst scriptBlockAst = new Parser().Parse(psDataFilePath, input, null, out array);
			if (array.Length > 0)
			{
				ParseException ex = new ParseException(array);
				throw PSTraceSource.NewInvalidOperationException(ex, ParserStrings.CannotLoadPowerShellDataFile, new object[]
				{
					psDataFilePath,
					ex.Message
				});
			}
			string text;
			string text2;
			PipelineAst simplePipeline = scriptBlockAst.GetSimplePipeline(false, out text, out text2);
			if (simplePipeline != null)
			{
				HashtableAst hashtableAst = simplePipeline.GetPureExpression() as HashtableAst;
				if (hashtableAst != null)
				{
					Hashtable hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
					foreach (Tuple<ExpressionAst, StatementAst> tuple in hashtableAst.KeyValuePairs)
					{
						StringConstantExpressionAst stringConstantExpressionAst = tuple.Item1 as StringConstantExpressionAst;
						if (stringConstantExpressionAst != null && keys.Contains(stringConstantExpressionAst.Value, StringComparer.OrdinalIgnoreCase))
						{
							try
							{
								object value = tuple.Item2.SafeGetValue();
								hashtable[stringConstantExpressionAst.Value] = value;
							}
							catch
							{
								throw PSTraceSource.NewInvalidOperationException(ParserStrings.InvalidPowerShellDataFile, new object[]
								{
									psDataFilePath
								});
							}
						}
					}
					return hashtable;
				}
			}
			throw PSTraceSource.NewInvalidOperationException(ParserStrings.InvalidPowerShellDataFile, new object[]
			{
				psDataFilePath
			});
		}

		// Token: 0x04002B41 RID: 11073
		internal static string ArmArchitecture = "ARM";

		// Token: 0x04002B42 RID: 11074
		internal static readonly string[] ManifestModuleVersionPropertyName = new string[]
		{
			"ModuleVersion"
		};

		// Token: 0x0200089D RID: 2205
		internal static class FrameworkRegistryInstallation
		{
			// Token: 0x06005478 RID: 21624 RVA: 0x001BE4BC File Offset: 0x001BC6BC
			private static bool GetRegistryNames(int majorVersion, int minorVersion, out string installKeyName, out string installValueName, out string spKeyName, out string spValueName)
			{
				installKeyName = null;
				spKeyName = null;
				installValueName = null;
				spValueName = "SP";
				if (majorVersion == 1 && minorVersion == 1)
				{
					installKeyName = "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v1.1.4322";
					spKeyName = installKeyName;
					installValueName = "Install";
					return true;
				}
				if (majorVersion == 2 && minorVersion == 0)
				{
					installKeyName = "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v2.0.50727";
					spKeyName = installKeyName;
					installValueName = "Install";
					return true;
				}
				if (majorVersion == 3 && minorVersion == 0)
				{
					installKeyName = "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v3.0\\Setup";
					spKeyName = "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v3.0";
					installValueName = "InstallSuccess";
					return true;
				}
				if (majorVersion == 3 && minorVersion == 5)
				{
					installKeyName = "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v3.5";
					spKeyName = installKeyName;
					installValueName = "Install";
					return true;
				}
				if (majorVersion == 4 && minorVersion == 0)
				{
					installKeyName = "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Client";
					spKeyName = installKeyName;
					installValueName = "Install";
					spValueName = "Servicing";
					return true;
				}
				if (majorVersion == 4 && minorVersion == 5)
				{
					installKeyName = "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full";
					installValueName = "Release";
					return true;
				}
				return false;
			}

			// Token: 0x06005479 RID: 21625 RVA: 0x001BE590 File Offset: 0x001BC790
			private static int? GetRegistryKeyValueInt(RegistryKey key, string valueName)
			{
				int? result;
				try
				{
					object value = key.GetValue(valueName);
					if (value is int)
					{
						result = new int?((int)value);
					}
					else
					{
						result = null;
					}
				}
				catch (ObjectDisposedException)
				{
					result = null;
				}
				catch (SecurityException)
				{
					result = null;
				}
				catch (IOException)
				{
					result = null;
				}
				catch (UnauthorizedAccessException)
				{
					result = null;
				}
				return result;
			}

			// Token: 0x0600547A RID: 21626 RVA: 0x001BE634 File Offset: 0x001BC834
			private static RegistryKey GetRegistryKeySubKey(RegistryKey key, string subKeyName)
			{
				RegistryKey result;
				try
				{
					result = key.OpenSubKey(subKeyName);
				}
				catch (ObjectDisposedException)
				{
					result = null;
				}
				catch (SecurityException)
				{
					result = null;
				}
				catch (ArgumentException)
				{
					result = null;
				}
				return result;
			}

			// Token: 0x0600547B RID: 21627 RVA: 0x001BE684 File Offset: 0x001BC884
			internal static bool CanCheckFrameworkInstallation(Version version, out int majorVersion, out int minorVersion, out int minimumSpVersion)
			{
				majorVersion = -1;
				minorVersion = -1;
				minimumSpVersion = -1;
				if (version == PsUtils.FrameworkRegistryInstallation.V4_5_00)
				{
					majorVersion = 4;
					minorVersion = 5;
					minimumSpVersion = 0;
					return true;
				}
				if (version == PsUtils.FrameworkRegistryInstallation.V4_0 || version == PsUtils.FrameworkRegistryInstallation.V4_0_00)
				{
					majorVersion = 4;
					minorVersion = 0;
					minimumSpVersion = 0;
					return true;
				}
				if (version == PsUtils.FrameworkRegistryInstallation.V3_5 || version == PsUtils.FrameworkRegistryInstallation.V3_5_00)
				{
					majorVersion = 3;
					minorVersion = 5;
					minimumSpVersion = 0;
					return true;
				}
				if (version == PsUtils.FrameworkRegistryInstallation.V3_5sp1)
				{
					majorVersion = 3;
					minorVersion = 5;
					minimumSpVersion = 1;
					return true;
				}
				if (version == PsUtils.FrameworkRegistryInstallation.V3_0 || version == PsUtils.FrameworkRegistryInstallation.V3_0_00)
				{
					majorVersion = 3;
					minorVersion = 0;
					minimumSpVersion = 0;
					return true;
				}
				if (version == PsUtils.FrameworkRegistryInstallation.V3_0sp1)
				{
					majorVersion = 3;
					minorVersion = 0;
					minimumSpVersion = 1;
					return true;
				}
				if (version == PsUtils.FrameworkRegistryInstallation.V3_0sp2)
				{
					majorVersion = 3;
					minorVersion = 0;
					minimumSpVersion = 2;
					return true;
				}
				if (version == PsUtils.FrameworkRegistryInstallation.V2_0 || version == PsUtils.FrameworkRegistryInstallation.V2_0_00)
				{
					majorVersion = 2;
					minorVersion = 0;
					minimumSpVersion = 0;
					return true;
				}
				if (version == PsUtils.FrameworkRegistryInstallation.V2_0sp1)
				{
					majorVersion = 2;
					minorVersion = 0;
					minimumSpVersion = 1;
					return true;
				}
				if (version == PsUtils.FrameworkRegistryInstallation.V2_0sp2)
				{
					majorVersion = 2;
					minorVersion = 0;
					minimumSpVersion = 2;
					return true;
				}
				if (version == PsUtils.FrameworkRegistryInstallation.V1_1 || version == PsUtils.FrameworkRegistryInstallation.V1_1_00)
				{
					majorVersion = 1;
					minorVersion = 1;
					minimumSpVersion = 0;
					return true;
				}
				if (version == PsUtils.FrameworkRegistryInstallation.V1_1sp1 || version == PsUtils.FrameworkRegistryInstallation.V1_1sp1Server)
				{
					majorVersion = 1;
					minorVersion = 1;
					minimumSpVersion = 1;
					return true;
				}
				return false;
			}

			// Token: 0x0600547C RID: 21628 RVA: 0x001BE80C File Offset: 0x001BCA0C
			internal static bool IsFrameworkInstalled(Version version)
			{
				int majorVersion;
				int minorVersion;
				int minimumSPVersion;
				return PsUtils.FrameworkRegistryInstallation.CanCheckFrameworkInstallation(version, out majorVersion, out minorVersion, out minimumSPVersion) && PsUtils.FrameworkRegistryInstallation.IsFrameworkInstalled(majorVersion, minorVersion, minimumSPVersion);
			}

			// Token: 0x0600547D RID: 21629 RVA: 0x001BE834 File Offset: 0x001BCA34
			internal static bool IsFrameworkInstalled(int majorVersion, int minorVersion, int minimumSPVersion)
			{
				string subKeyName;
				string valueName;
				string subKeyName2;
				string valueName2;
				if (!PsUtils.FrameworkRegistryInstallation.GetRegistryNames(majorVersion, minorVersion, out subKeyName, out valueName, out subKeyName2, out valueName2))
				{
					return false;
				}
				RegistryKey registryKeySubKey = PsUtils.FrameworkRegistryInstallation.GetRegistryKeySubKey(Registry.LocalMachine, subKeyName);
				if (registryKeySubKey == null)
				{
					return false;
				}
				int? registryKeyValueInt = PsUtils.FrameworkRegistryInstallation.GetRegistryKeyValueInt(registryKeySubKey, valueName);
				if (registryKeyValueInt == null)
				{
					return false;
				}
				if (majorVersion != 4 && minorVersion != 5 && registryKeyValueInt != 1)
				{
					return false;
				}
				if (minimumSPVersion > 0)
				{
					RegistryKey registryKeySubKey2 = PsUtils.FrameworkRegistryInstallation.GetRegistryKeySubKey(Registry.LocalMachine, subKeyName2);
					if (registryKeySubKey2 == null)
					{
						return false;
					}
					int? registryKeyValueInt2 = PsUtils.FrameworkRegistryInstallation.GetRegistryKeyValueInt(registryKeySubKey2, valueName2);
					if (registryKeyValueInt2 == null)
					{
						return false;
					}
					if (registryKeyValueInt2 < minimumSPVersion)
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x04002B44 RID: 11076
			private static Version V4_0 = new Version(4, 0, 30319, 0);

			// Token: 0x04002B45 RID: 11077
			private static Version V3_5 = new Version(3, 5, 21022, 8);

			// Token: 0x04002B46 RID: 11078
			private static Version V3_5sp1 = new Version(3, 5, 30729, 1);

			// Token: 0x04002B47 RID: 11079
			private static Version V3_0 = new Version(3, 0, 4506, 30);

			// Token: 0x04002B48 RID: 11080
			private static Version V3_0sp1 = new Version(3, 0, 4506, 648);

			// Token: 0x04002B49 RID: 11081
			private static Version V3_0sp2 = new Version(3, 0, 4506, 2152);

			// Token: 0x04002B4A RID: 11082
			private static Version V2_0 = new Version(2, 0, 50727, 42);

			// Token: 0x04002B4B RID: 11083
			private static Version V2_0sp1 = new Version(2, 0, 50727, 1433);

			// Token: 0x04002B4C RID: 11084
			private static Version V2_0sp2 = new Version(2, 0, 50727, 3053);

			// Token: 0x04002B4D RID: 11085
			private static Version V1_1 = new Version(1, 1, 4322, 573);

			// Token: 0x04002B4E RID: 11086
			private static Version V1_1sp1 = new Version(1, 1, 4322, 2032);

			// Token: 0x04002B4F RID: 11087
			private static Version V1_1sp1Server = new Version(1, 1, 4322, 2300);

			// Token: 0x04002B50 RID: 11088
			private static Version V4_5_00 = new Version(4, 5, 0, 0);

			// Token: 0x04002B51 RID: 11089
			private static Version V4_0_00 = new Version(4, 0, 0, 0);

			// Token: 0x04002B52 RID: 11090
			private static Version V3_5_00 = new Version(3, 5, 0, 0);

			// Token: 0x04002B53 RID: 11091
			private static Version V3_0_00 = new Version(3, 0, 0, 0);

			// Token: 0x04002B54 RID: 11092
			private static Version V2_0_00 = new Version(2, 0, 0, 0);

			// Token: 0x04002B55 RID: 11093
			private static Version V1_1_00 = new Version(1, 1, 0, 0);

			// Token: 0x04002B56 RID: 11094
			internal static Dictionary<Version, HashSet<Version>> CompatibleNetFrameworkVersions = new Dictionary<Version, HashSet<Version>>
			{
				{
					PsUtils.FrameworkRegistryInstallation.V1_1_00,
					new HashSet<Version>
					{
						PsUtils.FrameworkRegistryInstallation.V4_5_00,
						PsUtils.FrameworkRegistryInstallation.V4_0_00,
						PsUtils.FrameworkRegistryInstallation.V3_5_00,
						PsUtils.FrameworkRegistryInstallation.V3_0_00,
						PsUtils.FrameworkRegistryInstallation.V2_0_00
					}
				},
				{
					PsUtils.FrameworkRegistryInstallation.V2_0_00,
					new HashSet<Version>
					{
						PsUtils.FrameworkRegistryInstallation.V4_5_00,
						PsUtils.FrameworkRegistryInstallation.V4_0_00,
						PsUtils.FrameworkRegistryInstallation.V3_5_00,
						PsUtils.FrameworkRegistryInstallation.V3_0_00
					}
				},
				{
					PsUtils.FrameworkRegistryInstallation.V3_0_00,
					new HashSet<Version>
					{
						PsUtils.FrameworkRegistryInstallation.V4_5_00,
						PsUtils.FrameworkRegistryInstallation.V4_0_00,
						PsUtils.FrameworkRegistryInstallation.V3_5_00
					}
				},
				{
					PsUtils.FrameworkRegistryInstallation.V3_5_00,
					new HashSet<Version>
					{
						PsUtils.FrameworkRegistryInstallation.V4_5_00,
						PsUtils.FrameworkRegistryInstallation.V4_0_00
					}
				},
				{
					PsUtils.FrameworkRegistryInstallation.V4_0_00,
					new HashSet<Version>
					{
						PsUtils.FrameworkRegistryInstallation.V4_5_00
					}
				},
				{
					PsUtils.FrameworkRegistryInstallation.V4_5_00,
					new HashSet<Version>()
				}
			};

			// Token: 0x04002B57 RID: 11095
			internal static Version KnownHighestNetFrameworkVersion = new Version(4, 5);
		}

		// Token: 0x0200089E RID: 2206
		private static class NativeMethods
		{
			// Token: 0x0600547F RID: 21631
			[DllImport("kernel32.dll")]
			internal static extern void GetSystemInfo(ref PsUtils.NativeMethods.SYSTEM_INFO lpSystemInfo);

			// Token: 0x06005480 RID: 21632
			[DllImport("kernel32.dll")]
			internal static extern uint GetCurrentThreadId();

			// Token: 0x04002B58 RID: 11096
			internal const ushort PROCESSOR_ARCHITECTURE_INTEL = 0;

			// Token: 0x04002B59 RID: 11097
			internal const ushort PROCESSOR_ARCHITECTURE_ARM = 5;

			// Token: 0x04002B5A RID: 11098
			internal const ushort PROCESSOR_ARCHITECTURE_IA64 = 6;

			// Token: 0x04002B5B RID: 11099
			internal const ushort PROCESSOR_ARCHITECTURE_AMD64 = 9;

			// Token: 0x04002B5C RID: 11100
			internal const ushort PROCESSOR_ARCHITECTURE_UNKNOWN = 65535;

			// Token: 0x0200089F RID: 2207
			internal struct SYSTEM_INFO
			{
				// Token: 0x04002B5D RID: 11101
				public ushort wProcessorArchitecture;

				// Token: 0x04002B5E RID: 11102
				public ushort wReserved;

				// Token: 0x04002B5F RID: 11103
				public uint dwPageSize;

				// Token: 0x04002B60 RID: 11104
				public IntPtr lpMinimumApplicationAddress;

				// Token: 0x04002B61 RID: 11105
				public IntPtr lpMaximumApplicationAddress;

				// Token: 0x04002B62 RID: 11106
				public UIntPtr dwActiveProcessorMask;

				// Token: 0x04002B63 RID: 11107
				public uint dwNumberOfProcessors;

				// Token: 0x04002B64 RID: 11108
				public uint dwProcessorType;

				// Token: 0x04002B65 RID: 11109
				public uint dwAllocationGranularity;

				// Token: 0x04002B66 RID: 11110
				public ushort wProcessorLevel;

				// Token: 0x04002B67 RID: 11111
				public ushort wProcessorRevision;
			}
		}
	}
}
