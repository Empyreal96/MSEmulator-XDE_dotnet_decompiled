using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.CodeAnalysis
{
	// Token: 0x0200064B RID: 1611
	internal sealed class FusionAssemblyIdentity
	{
		// Token: 0x06004596 RID: 17814 RVA: 0x00176488 File Offset: 0x00174688
		private static int CreateAssemblyNameObject(out FusionAssemblyIdentity.IAssemblyName ppEnum, string szAssemblyName, uint dwFlags, IntPtr pvReserved)
		{
			int result;
			lock (FusionAssemblyIdentity.s_assemblyIdentityGate)
			{
				result = FusionAssemblyIdentity.RealCreateAssemblyNameObject(out ppEnum, szAssemblyName, dwFlags, pvReserved);
			}
			return result;
		}

		// Token: 0x06004597 RID: 17815
		[DllImport("clr", CharSet = CharSet.Unicode, EntryPoint = "CreateAssemblyNameObject")]
		private static extern int RealCreateAssemblyNameObject(out FusionAssemblyIdentity.IAssemblyName ppEnum, [MarshalAs(UnmanagedType.LPWStr)] string szAssemblyName, uint dwFlags, IntPtr pvReserved);

		// Token: 0x06004598 RID: 17816 RVA: 0x001764CC File Offset: 0x001746CC
		internal unsafe static string GetDisplayName(FusionAssemblyIdentity.IAssemblyName nameObject, FusionAssemblyIdentity.ASM_DISPLAYF displayFlags)
		{
			uint num = 0U;
			int displayName = nameObject.GetDisplayName(null, ref num, displayFlags);
			if (displayName == 0)
			{
				return string.Empty;
			}
			if (displayName != -2147024774)
			{
				throw Marshal.GetExceptionForHR(displayName);
			}
			byte[] array = new byte[num * 2U];
			fixed (byte* ptr = array)
			{
				displayName = nameObject.GetDisplayName(ptr, ref num, displayFlags);
				if (displayName != 0)
				{
					throw Marshal.GetExceptionForHR(displayName);
				}
				return Marshal.PtrToStringUni((IntPtr)((void*)ptr), (int)(num - 1U));
			}
		}

		// Token: 0x06004599 RID: 17817 RVA: 0x00176550 File Offset: 0x00174750
		internal unsafe static byte[] GetPropertyBytes(FusionAssemblyIdentity.IAssemblyName nameObject, FusionAssemblyIdentity.PropertyId propertyId)
		{
			uint num = 0U;
			int property = nameObject.GetProperty(propertyId, null, ref num);
			if (property == 0)
			{
				return null;
			}
			if (property != -2147024774)
			{
				throw Marshal.GetExceptionForHR(property);
			}
			byte[] array = new byte[num];
			fixed (byte* ptr = array)
			{
				property = nameObject.GetProperty(propertyId, (void*)ptr, ref num);
				if (property != 0)
				{
					throw Marshal.GetExceptionForHR(property);
				}
			}
			return array;
		}

		// Token: 0x0600459A RID: 17818 RVA: 0x001765BC File Offset: 0x001747BC
		internal unsafe static string GetPropertyString(FusionAssemblyIdentity.IAssemblyName nameObject, FusionAssemblyIdentity.PropertyId propertyId)
		{
			byte[] propertyBytes = FusionAssemblyIdentity.GetPropertyBytes(nameObject, propertyId);
			if (propertyBytes == null)
			{
				return null;
			}
			fixed (byte* ptr = propertyBytes)
			{
				return Marshal.PtrToStringUni((IntPtr)((void*)ptr), propertyBytes.Length / 2 - 1);
			}
		}

		// Token: 0x0600459B RID: 17819 RVA: 0x00176604 File Offset: 0x00174804
		internal static Version GetVersion(FusionAssemblyIdentity.IAssemblyName nameObject)
		{
			uint num;
			uint num2;
			int version = nameObject.GetVersion(out num, out num2);
			if (version != 0)
			{
				return null;
			}
			return new Version((int)(num >> 16), (int)(num & 65535U), (int)(num2 >> 16), (int)(num2 & 65535U));
		}

		// Token: 0x0600459C RID: 17820 RVA: 0x0017663C File Offset: 0x0017483C
		internal unsafe static uint? GetPropertyWord(FusionAssemblyIdentity.IAssemblyName nameObject, FusionAssemblyIdentity.PropertyId propertyId)
		{
			uint num = 4U;
			uint value;
			int property = nameObject.GetProperty(propertyId, (void*)(&value), ref num);
			if (property != 0)
			{
				throw Marshal.GetExceptionForHR(property);
			}
			if (num == 0U)
			{
				return null;
			}
			return new uint?(value);
		}

		// Token: 0x0600459D RID: 17821 RVA: 0x00176675 File Offset: 0x00174875
		internal static string GetCulture(FusionAssemblyIdentity.IAssemblyName nameObject)
		{
			return FusionAssemblyIdentity.GetPropertyString(nameObject, FusionAssemblyIdentity.PropertyId.CULTURE);
		}

		// Token: 0x0600459E RID: 17822 RVA: 0x00176680 File Offset: 0x00174880
		internal static ProcessorArchitecture GetProcessorArchitecture(FusionAssemblyIdentity.IAssemblyName nameObject)
		{
			uint? propertyWord = FusionAssemblyIdentity.GetPropertyWord(nameObject, FusionAssemblyIdentity.PropertyId.ARCHITECTURE);
			if (propertyWord == null)
			{
				return ProcessorArchitecture.None;
			}
			return (ProcessorArchitecture)propertyWord.GetValueOrDefault();
		}

		// Token: 0x0600459F RID: 17823 RVA: 0x001766A8 File Offset: 0x001748A8
		internal static FusionAssemblyIdentity.IAssemblyName ToAssemblyNameObject(string displayName)
		{
			if (displayName.IndexOf('\0') >= 0)
			{
				return null;
			}
			FusionAssemblyIdentity.IAssemblyName result;
			int num = FusionAssemblyIdentity.CreateAssemblyNameObject(out result, displayName, 1U, IntPtr.Zero);
			if (num != 0)
			{
				return null;
			}
			return result;
		}

		// Token: 0x060045A0 RID: 17824 RVA: 0x001766D8 File Offset: 0x001748D8
		internal static FusionAssemblyIdentity.IAssemblyName GetBestMatch(IEnumerable<FusionAssemblyIdentity.IAssemblyName> candidates, string preferredCultureOpt)
		{
			FusionAssemblyIdentity.IAssemblyName assemblyName = null;
			Version version = null;
			string text = null;
			foreach (FusionAssemblyIdentity.IAssemblyName assemblyName2 in candidates)
			{
				if (assemblyName != null)
				{
					Version version2 = FusionAssemblyIdentity.GetVersion(assemblyName2);
					if (version == null)
					{
						version = FusionAssemblyIdentity.GetVersion(assemblyName);
					}
					int num = version.CompareTo(version2);
					if (num == 0)
					{
						if (preferredCultureOpt != null)
						{
							string culture = FusionAssemblyIdentity.GetCulture(assemblyName2);
							if (text == null)
							{
								text = FusionAssemblyIdentity.GetCulture(assemblyName2);
							}
							if (StringComparer.OrdinalIgnoreCase.Equals(culture, preferredCultureOpt) || (culture.Length == 0 && !StringComparer.OrdinalIgnoreCase.Equals(text, preferredCultureOpt)))
							{
								assemblyName = assemblyName2;
								version = version2;
								text = culture;
							}
						}
					}
					else if (num < 0)
					{
						assemblyName = assemblyName2;
						version = version2;
					}
				}
				else
				{
					assemblyName = assemblyName2;
				}
			}
			return assemblyName;
		}

		// Token: 0x04002254 RID: 8788
		private const int ERROR_INSUFFICIENT_BUFFER = -2147024774;

		// Token: 0x04002255 RID: 8789
		private const int FUSION_E_INVALID_NAME = -2146234297;

		// Token: 0x04002256 RID: 8790
		private static object s_assemblyIdentityGate = new object();

		// Token: 0x0200064C RID: 1612
		[Flags]
		internal enum ASM_DISPLAYF
		{
			// Token: 0x04002258 RID: 8792
			VERSION = 1,
			// Token: 0x04002259 RID: 8793
			CULTURE = 2,
			// Token: 0x0400225A RID: 8794
			PUBLIC_KEY_TOKEN = 4,
			// Token: 0x0400225B RID: 8795
			PUBLIC_KEY = 8,
			// Token: 0x0400225C RID: 8796
			CUSTOM = 16,
			// Token: 0x0400225D RID: 8797
			PROCESSORARCHITECTURE = 32,
			// Token: 0x0400225E RID: 8798
			LANGUAGEID = 64,
			// Token: 0x0400225F RID: 8799
			RETARGET = 128,
			// Token: 0x04002260 RID: 8800
			CONFIG_MASK = 256,
			// Token: 0x04002261 RID: 8801
			MVID = 512,
			// Token: 0x04002262 RID: 8802
			CONTENT_TYPE = 1024,
			// Token: 0x04002263 RID: 8803
			FULL = 1191
		}

		// Token: 0x0200064D RID: 1613
		internal enum PropertyId
		{
			// Token: 0x04002265 RID: 8805
			PUBLIC_KEY,
			// Token: 0x04002266 RID: 8806
			PUBLIC_KEY_TOKEN,
			// Token: 0x04002267 RID: 8807
			HASH_VALUE,
			// Token: 0x04002268 RID: 8808
			NAME,
			// Token: 0x04002269 RID: 8809
			MAJOR_VERSION,
			// Token: 0x0400226A RID: 8810
			MINOR_VERSION,
			// Token: 0x0400226B RID: 8811
			BUILD_NUMBER,
			// Token: 0x0400226C RID: 8812
			REVISION_NUMBER,
			// Token: 0x0400226D RID: 8813
			CULTURE,
			// Token: 0x0400226E RID: 8814
			PROCESSOR_ID_ARRAY,
			// Token: 0x0400226F RID: 8815
			OSINFO_ARRAY,
			// Token: 0x04002270 RID: 8816
			HASH_ALGID,
			// Token: 0x04002271 RID: 8817
			ALIAS,
			// Token: 0x04002272 RID: 8818
			CODEBASE_URL,
			// Token: 0x04002273 RID: 8819
			CODEBASE_LASTMOD,
			// Token: 0x04002274 RID: 8820
			NULL_PUBLIC_KEY,
			// Token: 0x04002275 RID: 8821
			NULL_PUBLIC_KEY_TOKEN,
			// Token: 0x04002276 RID: 8822
			CUSTOM,
			// Token: 0x04002277 RID: 8823
			NULL_CUSTOM,
			// Token: 0x04002278 RID: 8824
			MVID,
			// Token: 0x04002279 RID: 8825
			FILE_MAJOR_VERSION,
			// Token: 0x0400227A RID: 8826
			FILE_MINOR_VERSION,
			// Token: 0x0400227B RID: 8827
			FILE_BUILD_NUMBER,
			// Token: 0x0400227C RID: 8828
			FILE_REVISION_NUMBER,
			// Token: 0x0400227D RID: 8829
			RETARGET,
			// Token: 0x0400227E RID: 8830
			SIGNATURE_BLOB,
			// Token: 0x0400227F RID: 8831
			CONFIG_MASK,
			// Token: 0x04002280 RID: 8832
			ARCHITECTURE,
			// Token: 0x04002281 RID: 8833
			CONTENT_TYPE,
			// Token: 0x04002282 RID: 8834
			MAX_PARAMS
		}

		// Token: 0x0200064E RID: 1614
		private static class CANOF
		{
			// Token: 0x04002283 RID: 8835
			public const uint PARSE_DISPLAY_NAME = 1U;

			// Token: 0x04002284 RID: 8836
			public const uint SET_DEFAULT_VALUES = 2U;
		}

		// Token: 0x0200064F RID: 1615
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("CD193BC0-B4BC-11d2-9833-00C04FC31D2E")]
		[ComImport]
		internal interface IAssemblyName
		{
			// Token: 0x060045A3 RID: 17827
			unsafe void SetProperty(FusionAssemblyIdentity.PropertyId id, void* data, uint size);

			// Token: 0x060045A4 RID: 17828
			[PreserveSig]
			unsafe int GetProperty(FusionAssemblyIdentity.PropertyId id, void* data, ref uint size);

			// Token: 0x060045A5 RID: 17829
			[PreserveSig]
			int Finalize();

			// Token: 0x060045A6 RID: 17830
			[PreserveSig]
			unsafe int GetDisplayName(byte* buffer, ref uint characterCount, FusionAssemblyIdentity.ASM_DISPLAYF dwDisplayFlags);

			// Token: 0x060045A7 RID: 17831
			[PreserveSig]
			int __BindToObject();

			// Token: 0x060045A8 RID: 17832
			[PreserveSig]
			int __GetName();

			// Token: 0x060045A9 RID: 17833
			[PreserveSig]
			int GetVersion(out uint versionHi, out uint versionLow);

			// Token: 0x060045AA RID: 17834
			[PreserveSig]
			int IsEqual(FusionAssemblyIdentity.IAssemblyName pName, uint dwCmpFlags);

			// Token: 0x060045AB RID: 17835
			[PreserveSig]
			int Clone(out FusionAssemblyIdentity.IAssemblyName pName);
		}

		// Token: 0x02000650 RID: 1616
		[Guid("7c23ff90-33af-11d3-95da-00a024a85b51")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface IApplicationContext
		{
		}
	}
}
