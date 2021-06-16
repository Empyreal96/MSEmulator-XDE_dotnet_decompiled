using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Management.Automation.Remoting.Client;
using System.Reflection;
using System.Security;
using Microsoft.Win32;

namespace System.Management.Automation
{
	// Token: 0x020000CA RID: 202
	internal class PSVersionInfo
	{
		// Token: 0x06000B58 RID: 2904 RVA: 0x00041A90 File Offset: 0x0003FC90
		static PSVersionInfo()
		{
			PSVersionInfo._psVersionTable = new Hashtable(StringComparer.OrdinalIgnoreCase);
			PSVersionInfo._psVersionTable["PSVersion"] = PSVersionInfo._psV5Version;
			PSVersionInfo._psVersionTable["CLRVersion"] = Environment.Version;
			PSVersionInfo._psVersionTable["BuildVersion"] = PSVersionInfo.GetBuildVersion();
			PSVersionInfo._psVersionTable["PSCompatibleVersions"] = new Version[]
			{
				PSVersionInfo._psV1Version,
				PSVersionInfo._psV2Version,
				PSVersionInfo._psV3Version,
				PSVersionInfo._psV4Version,
				PSVersionInfo._psV5Version
			};
			PSVersionInfo._psVersionTable["SerializationVersion"] = new Version("1.1.0.1");
			PSVersionInfo._psVersionTable["PSRemotingProtocolVersion"] = RemotingConstants.ProtocolVersion;
			PSVersionInfo._psVersionTable["WSManStackVersion"] = PSVersionInfo.GetWSManStackVersion();
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x00041BB0 File Offset: 0x0003FDB0
		internal static Hashtable GetPSVersionTable()
		{
			return PSVersionInfo._psVersionTable;
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x00041BB8 File Offset: 0x0003FDB8
		internal static Version GetBuildVersion()
		{
			string assemblyLocation = ClrFacade.GetAssemblyLocation(typeof(PSVersionInfo).GetTypeInfo().Assembly);
			string fileVersion = FileVersionInfo.GetVersionInfo(assemblyLocation).FileVersion;
			return new Version(fileVersion);
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00041BF4 File Offset: 0x0003FDF4
		private static Version GetWSManStackVersion()
		{
			Version version = null;
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\WSMAN"))
				{
					if (registryKey != null)
					{
						object value = registryKey.GetValue("ServiceStackVersion");
						string text = (value != null) ? ((string)value) : null;
						if (!string.IsNullOrEmpty(text))
						{
							version = new Version(text.Trim());
						}
					}
				}
			}
			catch (ObjectDisposedException)
			{
			}
			catch (SecurityException)
			{
			}
			catch (ArgumentException)
			{
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (FormatException)
			{
			}
			catch (OverflowException)
			{
			}
			catch (InvalidCastException)
			{
			}
			return version ?? WSManNativeApi.WSMAN_STACK_VERSION;
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000B5C RID: 2908 RVA: 0x00041CE4 File Offset: 0x0003FEE4
		internal static Version PSVersion
		{
			get
			{
				return (Version)PSVersionInfo.GetPSVersionTable()["PSVersion"];
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000B5D RID: 2909 RVA: 0x00041CFA File Offset: 0x0003FEFA
		internal static Version CLRVersion
		{
			get
			{
				return (Version)PSVersionInfo.GetPSVersionTable()["CLRVersion"];
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000B5E RID: 2910 RVA: 0x00041D10 File Offset: 0x0003FF10
		internal static Version BuildVersion
		{
			get
			{
				return (Version)PSVersionInfo.GetPSVersionTable()["BuildVersion"];
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000B5F RID: 2911 RVA: 0x00041D26 File Offset: 0x0003FF26
		internal static Version[] PSCompatibleVersions
		{
			get
			{
				return (Version[])PSVersionInfo.GetPSVersionTable()["PSCompatibleVersions"];
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000B60 RID: 2912 RVA: 0x00041D3C File Offset: 0x0003FF3C
		internal static Version SerializationVersion
		{
			get
			{
				return (Version)PSVersionInfo.GetPSVersionTable()["SerializationVersion"];
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x00041D52 File Offset: 0x0003FF52
		internal static string RegistryVersion1Key
		{
			get
			{
				return "1";
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000B62 RID: 2914 RVA: 0x00041D59 File Offset: 0x0003FF59
		internal static string RegistryVersionKey
		{
			get
			{
				return "3";
			}
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x00041D60 File Offset: 0x0003FF60
		internal static string GetRegisteryVersionKeyForSnapinDiscovery(string majorVersion)
		{
			int num = 0;
			LanguagePrimitives.TryConvertTo<int>(majorVersion, out num);
			if (num >= 1 && num <= PSVersionInfo.PSVersion.Major)
			{
				return "1";
			}
			return null;
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000B64 RID: 2916 RVA: 0x00041D90 File Offset: 0x0003FF90
		internal static string FeatureVersionString
		{
			get
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", new object[]
				{
					PSVersionInfo.PSVersion.Major,
					PSVersionInfo.PSVersion.Minor
				});
			}
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x00041DD8 File Offset: 0x0003FFD8
		internal static bool IsValidPSVersion(Version version)
		{
			if (version.Major == PSVersionInfo._psV5Version.Major)
			{
				return version.Minor == PSVersionInfo._psV5Version.Minor;
			}
			if (version.Major == PSVersionInfo._psV4Version.Major)
			{
				return version.Minor == PSVersionInfo._psV4Version.Minor;
			}
			if (version.Major == PSVersionInfo._psV3Version.Major)
			{
				return version.Minor == PSVersionInfo._psV3Version.Minor;
			}
			if (version.Major == PSVersionInfo._psV2Version.Major)
			{
				return version.Minor == PSVersionInfo._psV2Version.Minor;
			}
			return version.Major == PSVersionInfo._psV1Version.Major && version.Minor == PSVersionInfo._psV1Version.Minor;
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000B66 RID: 2918 RVA: 0x00041E9F File Offset: 0x0004009F
		internal static Version PSV4Version
		{
			get
			{
				return PSVersionInfo._psV4Version;
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000B67 RID: 2919 RVA: 0x00041EA6 File Offset: 0x000400A6
		internal static Version PSV5Version
		{
			get
			{
				return PSVersionInfo._psV5Version;
			}
		}

		// Token: 0x04000509 RID: 1289
		internal const string PSVersionTableName = "PSVersionTable";

		// Token: 0x0400050A RID: 1290
		internal const string PSRemotingProtocolVersionName = "PSRemotingProtocolVersion";

		// Token: 0x0400050B RID: 1291
		internal const string PSVersionName = "PSVersion";

		// Token: 0x0400050C RID: 1292
		internal const string SerializationVersionName = "SerializationVersion";

		// Token: 0x0400050D RID: 1293
		internal const string WSManStackVersionName = "WSManStackVersion";

		// Token: 0x0400050E RID: 1294
		private static Hashtable _psVersionTable = null;

		// Token: 0x0400050F RID: 1295
		private static Version _psV1Version = new Version(1, 0);

		// Token: 0x04000510 RID: 1296
		private static Version _psV2Version = new Version(2, 0);

		// Token: 0x04000511 RID: 1297
		private static Version _psV3Version = new Version(3, 0);

		// Token: 0x04000512 RID: 1298
		private static Version _psV4Version = new Version(4, 0);

		// Token: 0x04000513 RID: 1299
		private static Version _psV5Version = new Version(5, 0, 10586, 0);
	}
}
