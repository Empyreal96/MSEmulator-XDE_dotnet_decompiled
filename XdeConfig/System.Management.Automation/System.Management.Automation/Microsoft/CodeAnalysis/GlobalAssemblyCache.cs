using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.CodeAnalysis
{
	// Token: 0x02000646 RID: 1606
	internal static class GlobalAssemblyCache
	{
		// Token: 0x06004589 RID: 17801
		[DllImport("clr", CharSet = CharSet.Auto)]
		private static extern int CreateAssemblyEnum(out GlobalAssemblyCache.IAssemblyEnum ppEnum, FusionAssemblyIdentity.IApplicationContext pAppCtx, FusionAssemblyIdentity.IAssemblyName pName, GlobalAssemblyCache.ASM_CACHE dwFlags, IntPtr pvReserved);

		// Token: 0x0600458A RID: 17802
		[DllImport("clr", CharSet = CharSet.Auto, PreserveSig = false)]
		private static extern void CreateAssemblyCache(out GlobalAssemblyCache.IAssemblyCache ppAsmCache, uint dwReserved);

		// Token: 0x0600458B RID: 17803 RVA: 0x00176330 File Offset: 0x00174530
		internal static IEnumerable<FusionAssemblyIdentity.IAssemblyName> GetAssemblyObjects(FusionAssemblyIdentity.IAssemblyName partialNameFilter, ProcessorArchitecture[] architectureFilter)
		{
			FusionAssemblyIdentity.IApplicationContext applicationContext = null;
			GlobalAssemblyCache.IAssemblyEnum enumerator;
			int hr = GlobalAssemblyCache.CreateAssemblyEnum(out enumerator, applicationContext, partialNameFilter, GlobalAssemblyCache.ASM_CACHE.GAC, IntPtr.Zero);
			if (hr != 1)
			{
				if (hr != 0)
				{
					Exception exceptionForHR = Marshal.GetExceptionForHR(hr);
					if (!(exceptionForHR is FileNotFoundException))
					{
						if (exceptionForHR != null)
						{
							throw exceptionForHR;
						}
						throw new ArgumentException("Invalid assembly name");
					}
				}
				else
				{
					for (;;)
					{
						FusionAssemblyIdentity.IAssemblyName nameObject;
						hr = enumerator.GetNextAssembly(out applicationContext, out nameObject, 0U);
						if (hr != 0)
						{
							break;
						}
						if (architectureFilter != null)
						{
							ProcessorArchitecture processorArchitecture = FusionAssemblyIdentity.GetProcessorArchitecture(nameObject);
							if (!architectureFilter.Contains(processorArchitecture))
							{
								continue;
							}
						}
						yield return nameObject;
					}
					if (hr < 0)
					{
						Marshal.ThrowExceptionForHR(hr);
					}
				}
			}
			yield break;
		}

		// Token: 0x0600458C RID: 17804 RVA: 0x00176354 File Offset: 0x00174554
		public unsafe static string ResolvePartialName(string displayName, out string location, ProcessorArchitecture[] architectureFilter = null, CultureInfo preferredCulture = null)
		{
			if (displayName == null)
			{
				throw new ArgumentNullException("displayName");
			}
			location = null;
			FusionAssemblyIdentity.IAssemblyName assemblyName = FusionAssemblyIdentity.ToAssemblyNameObject(displayName);
			if (assemblyName == null)
			{
				return null;
			}
			IEnumerable<FusionAssemblyIdentity.IAssemblyName> assemblyObjects = GlobalAssemblyCache.GetAssemblyObjects(assemblyName, architectureFilter);
			string preferredCultureOpt = (preferredCulture != null && !preferredCulture.IsNeutralCulture) ? preferredCulture.Name : null;
			FusionAssemblyIdentity.IAssemblyName bestMatch = FusionAssemblyIdentity.GetBestMatch(assemblyObjects, preferredCultureOpt);
			if (bestMatch == null)
			{
				return null;
			}
			string displayName2 = FusionAssemblyIdentity.GetDisplayName(bestMatch, FusionAssemblyIdentity.ASM_DISPLAYF.FULL);
			fixed (char* ptr = new char[260])
			{
				GlobalAssemblyCache.ASSEMBLY_INFO assembly_INFO = new GlobalAssemblyCache.ASSEMBLY_INFO
				{
					cbAssemblyInfo = (uint)Marshal.SizeOf(typeof(GlobalAssemblyCache.ASSEMBLY_INFO)),
					pszCurrentAssemblyPathBuf = ptr,
					cchBuf = 260U
				};
				GlobalAssemblyCache.IAssemblyCache assemblyCache;
				GlobalAssemblyCache.CreateAssemblyCache(out assemblyCache, 0U);
				assemblyCache.QueryAssemblyInfo(0U, displayName2, ref assembly_INFO);
				string text = Marshal.PtrToStringUni((IntPtr)((void*)assembly_INFO.pszCurrentAssemblyPathBuf), (int)(assembly_INFO.cchBuf - 1U));
				location = text;
			}
			return displayName2;
		}

		// Token: 0x04002242 RID: 8770
		private const int MAX_PATH = 260;

		// Token: 0x04002243 RID: 8771
		private const int S_OK = 0;

		// Token: 0x04002244 RID: 8772
		private const int S_FALSE = 1;

		// Token: 0x04002245 RID: 8773
		public static readonly ProcessorArchitecture[] CurrentArchitectures = (IntPtr.Size == 4) ? new ProcessorArchitecture[]
		{
			ProcessorArchitecture.None,
			ProcessorArchitecture.MSIL,
			ProcessorArchitecture.X86
		} : new ProcessorArchitecture[]
		{
			ProcessorArchitecture.None,
			ProcessorArchitecture.MSIL,
			ProcessorArchitecture.Amd64
		};

		// Token: 0x02000647 RID: 1607
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("21b8916c-f28e-11d2-a473-00c04f8ef448")]
		[ComImport]
		private interface IAssemblyEnum
		{
			// Token: 0x0600458E RID: 17806
			[PreserveSig]
			int GetNextAssembly(out FusionAssemblyIdentity.IApplicationContext ppAppCtx, out FusionAssemblyIdentity.IAssemblyName ppName, uint dwFlags);

			// Token: 0x0600458F RID: 17807
			[PreserveSig]
			int Reset();

			// Token: 0x06004590 RID: 17808
			[PreserveSig]
			int Clone(out GlobalAssemblyCache.IAssemblyEnum ppEnum);
		}

		// Token: 0x02000648 RID: 1608
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("e707dcde-d1cd-11d2-bab9-00c04f8eceae")]
		[ComImport]
		private interface IAssemblyCache
		{
			// Token: 0x06004591 RID: 17809
			void UninstallAssembly();

			// Token: 0x06004592 RID: 17810
			void QueryAssemblyInfo(uint dwFlags, [MarshalAs(UnmanagedType.LPWStr)] string pszAssemblyName, ref GlobalAssemblyCache.ASSEMBLY_INFO pAsmInfo);

			// Token: 0x06004593 RID: 17811
			void CreateAssemblyCacheItem();

			// Token: 0x06004594 RID: 17812
			void CreateAssemblyScavenger();

			// Token: 0x06004595 RID: 17813
			void InstallAssembly();
		}

		// Token: 0x02000649 RID: 1609
		private struct ASSEMBLY_INFO
		{
			// Token: 0x04002246 RID: 8774
			public uint cbAssemblyInfo;

			// Token: 0x04002247 RID: 8775
			public uint dwAssemblyFlags;

			// Token: 0x04002248 RID: 8776
			public ulong uliAssemblySizeInKB;

			// Token: 0x04002249 RID: 8777
			public unsafe char* pszCurrentAssemblyPathBuf;

			// Token: 0x0400224A RID: 8778
			public uint cchBuf;
		}

		// Token: 0x0200064A RID: 1610
		private enum ASM_CACHE
		{
			// Token: 0x0400224C RID: 8780
			ZAP = 1,
			// Token: 0x0400224D RID: 8781
			GAC,
			// Token: 0x0400224E RID: 8782
			DOWNLOAD = 4,
			// Token: 0x0400224F RID: 8783
			ROOT = 8,
			// Token: 0x04002250 RID: 8784
			GAC_MSIL = 16,
			// Token: 0x04002251 RID: 8785
			GAC_32 = 32,
			// Token: 0x04002252 RID: 8786
			GAC_64 = 64,
			// Token: 0x04002253 RID: 8787
			ROOT_EX = 128
		}
	}
}
