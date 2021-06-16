using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Automation.Language;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading;
using Microsoft.PowerShell.Commands.Internal;
using Microsoft.Win32.SafeHandles;

namespace System.Management.Automation
{
	// Token: 0x020008D0 RID: 2256
	internal static class ClrFacade
	{
		// Token: 0x06005536 RID: 21814 RVA: 0x001C1567 File Offset: 0x001BF767
		internal static MemberInfo[] GetMethods(Type targetType, string methodName)
		{
			return targetType.GetMember(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod);
		}

		// Token: 0x06005537 RID: 21815 RVA: 0x001C1575 File Offset: 0x001BF775
		internal static FileVersionInfo GetProcessModuleFileVersionInfo(ProcessModule processModule)
		{
			return processModule.FileVersionInfo;
		}

		// Token: 0x06005538 RID: 21816 RVA: 0x001C157D File Offset: 0x001BF77D
		internal static SafeHandle GetSafeProcessHandle(Process process)
		{
			return new Microsoft.PowerShell.Commands.Internal.SafeProcessHandle(process.Handle);
		}

		// Token: 0x06005539 RID: 21817 RVA: 0x001C158A File Offset: 0x001BF78A
		internal static IntPtr GetRawProcessHandle(Process process)
		{
			return process.Handle;
		}

		// Token: 0x0600553A RID: 21818 RVA: 0x001C1592 File Offset: 0x001BF792
		internal static StringDictionary GetProcessEnvironment(ProcessStartInfo startInfo)
		{
			return startInfo.EnvironmentVariables;
		}

		// Token: 0x0600553B RID: 21819 RVA: 0x001C159A File Offset: 0x001BF79A
		internal static int SizeOf<T>()
		{
			return Marshal.SizeOf(typeof(T));
		}

		// Token: 0x0600553C RID: 21820 RVA: 0x001C15AB File Offset: 0x001BF7AB
		internal static void DestroyStructure<T>(IntPtr ptr)
		{
			Marshal.DestroyStructure(ptr, typeof(T));
		}

		// Token: 0x0600553D RID: 21821 RVA: 0x001C15BD File Offset: 0x001BF7BD
		internal static T PtrToStructure<T>(IntPtr ptr)
		{
			return (T)((object)Marshal.PtrToStructure(ptr, typeof(T)));
		}

		// Token: 0x0600553E RID: 21822 RVA: 0x001C15D4 File Offset: 0x001BF7D4
		internal static void StructureToPtr<T>(T structure, IntPtr ptr, bool deleteOld)
		{
			Marshal.StructureToPtr(structure, ptr, deleteOld);
		}

		// Token: 0x0600553F RID: 21823 RVA: 0x001C15E3 File Offset: 0x001BF7E3
		internal static void ZeroFreeCoTaskMemUnicode(IntPtr unmanagedStr)
		{
			Marshal.ZeroFreeCoTaskMemUnicode(unmanagedStr);
		}

		// Token: 0x06005540 RID: 21824 RVA: 0x001C15EB File Offset: 0x001BF7EB
		internal static IntPtr SecureStringToCoTaskMemUnicode(SecureString s)
		{
			return Marshal.SecureStringToCoTaskMemUnicode(s);
		}

		// Token: 0x06005541 RID: 21825 RVA: 0x001C15F3 File Offset: 0x001BF7F3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static string GetAssemblyLocation(Assembly assembly)
		{
			return assembly.Location;
		}

		// Token: 0x06005542 RID: 21826 RVA: 0x001C15FB File Offset: 0x001BF7FB
		internal static AssemblyName GetAssemblyName(string assemblyPath)
		{
			return AssemblyName.GetAssemblyName(assemblyPath);
		}

		// Token: 0x06005543 RID: 21827 RVA: 0x001C1603 File Offset: 0x001BF803
		internal static IEnumerable<Assembly> GetAssemblies(TypeResolutionState typeResolutionState, TypeName typeName)
		{
			return ClrFacade.GetAssemblies(null);
		}

		// Token: 0x06005544 RID: 21828 RVA: 0x001C1625 File Offset: 0x001BF825
		internal static IEnumerable<Assembly> GetAssemblies(string namespaceQualifiedTypeName = null)
		{
			return from a in AppDomain.CurrentDomain.GetAssemblies()
			where !a.GetCustomAttributes(typeof(DynamicClassImplementationAssemblyAttribute)).Any<Attribute>()
			select a;
		}

		// Token: 0x06005545 RID: 21829 RVA: 0x001C1653 File Offset: 0x001BF853
		internal static Assembly LoadFrom(string assemblyPath)
		{
			return Assembly.LoadFrom(assemblyPath);
		}

		// Token: 0x06005546 RID: 21830 RVA: 0x001C165B File Offset: 0x001BF85B
		internal static void CreateEnumType(EnumBuilder enumBuilder)
		{
			enumBuilder.CreateTypeInfo();
		}

		// Token: 0x06005547 RID: 21831 RVA: 0x001C1664 File Offset: 0x001BF864
		internal static object[] GetCustomAttributes<T>(Assembly assembly)
		{
			return assembly.GetCustomAttributes(typeof(T), false);
		}

		// Token: 0x06005548 RID: 21832 RVA: 0x001C1677 File Offset: 0x001BF877
		internal static Encoding GetDefaultEncoding()
		{
			if (ClrFacade._defaultEncoding == null)
			{
				ClrFacade._defaultEncoding = Encoding.Default;
			}
			return ClrFacade._defaultEncoding;
		}

		// Token: 0x06005549 RID: 21833 RVA: 0x001C1698 File Offset: 0x001BF898
		internal static Encoding GetOEMEncoding()
		{
			if (ClrFacade._oemEncoding == null)
			{
				uint oemcp = ClrFacade.NativeMethods.GetOEMCP();
				ClrFacade._oemEncoding = Encoding.GetEncoding((int)oemcp);
			}
			return ClrFacade._oemEncoding;
		}

		// Token: 0x0600554A RID: 21834 RVA: 0x001C16C8 File Offset: 0x001BF8C8
		internal static SecurityZone GetFileSecurityZone(string filePath)
		{
			return ClrFacade.MapSecurityZoneWithUrlmon(filePath);
		}

		// Token: 0x0600554B RID: 21835 RVA: 0x001C16D0 File Offset: 0x001BF8D0
		private static SecurityZone MapSecurityZoneWithUrlmon(string filePath)
		{
			object obj = null;
			int num = ClrFacade.NativeMethods.CoInternetCreateSecurityManager(null, out obj, 0);
			if (num != 0)
			{
				throw new Win32Exception(num);
			}
			SecurityZone result;
			try
			{
				ClrFacade.NativeMethods.IInternetSecurityManager internetSecurityManager = (ClrFacade.NativeMethods.IInternetSecurityManager)obj;
				uint num2;
				if (internetSecurityManager.MapUrlToZone(filePath, out num2, 0U) == 0)
				{
					SecurityZone securityZone;
					result = (LanguagePrimitives.TryConvertTo<SecurityZone>(num2, out securityZone) ? securityZone : SecurityZone.NoZone);
				}
				else
				{
					result = SecurityZone.NoZone;
				}
			}
			finally
			{
				if (obj != null)
				{
					Marshal.ReleaseComObject(obj);
				}
			}
			return result;
		}

		// Token: 0x0600554C RID: 21836 RVA: 0x001C1744 File Offset: 0x001BF944
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool IsTransparentProxy(object obj)
		{
			return RemotingServices.IsTransparentProxy(obj);
		}

		// Token: 0x0600554D RID: 21837 RVA: 0x001C174C File Offset: 0x001BF94C
		internal static DirectoryInfo GetParent(string path)
		{
			return Directory.GetParent(path);
		}

		// Token: 0x0600554E RID: 21838 RVA: 0x001C1754 File Offset: 0x001BF954
		internal static string ToDmtfDateTime(DateTime date)
		{
			return ManagementDateTimeConverter.ToDmtfDateTime(date);
		}

		// Token: 0x0600554F RID: 21839 RVA: 0x001C175C File Offset: 0x001BF95C
		internal static bool Is64BitOperatingSystem()
		{
			return Environment.Is64BitOperatingSystem;
		}

		// Token: 0x06005550 RID: 21840 RVA: 0x001C1763 File Offset: 0x001BF963
		internal static object GetUninitializedObject(Type type)
		{
			return FormatterServices.GetUninitializedObject(type);
		}

		// Token: 0x06005551 RID: 21841 RVA: 0x001C176B File Offset: 0x001BF96B
		internal static void SetSafeWaitHandle(WaitHandle waitHandle, SafeWaitHandle value)
		{
			waitHandle.SafeWaitHandle = value;
		}

		// Token: 0x06005552 RID: 21842 RVA: 0x001C1774 File Offset: 0x001BF974
		internal static CultureInfo GetCultureInfo(string cultureName)
		{
			return CultureInfo.GetCultureInfo(cultureName);
		}

		// Token: 0x04002CB8 RID: 11448
		private static volatile Encoding _defaultEncoding;

		// Token: 0x04002CB9 RID: 11449
		private static volatile Encoding _oemEncoding;

		// Token: 0x020008D1 RID: 2257
		private static class NativeMethods
		{
			// Token: 0x06005554 RID: 21844
			[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
			internal static extern uint GetOEMCP();

			// Token: 0x06005555 RID: 21845
			[DllImport("urlmon.dll", ExactSpelling = true)]
			internal static extern int CoInternetCreateSecurityManager([MarshalAs(UnmanagedType.Interface)] object pIServiceProvider, [MarshalAs(UnmanagedType.Interface)] out object ppISecurityManager, int dwReserved);

			// Token: 0x04002CBB RID: 11451
			public const int S_OK = 0;

			// Token: 0x020008D2 RID: 2258
			[ComVisible(false)]
			[Guid("79EAC9EE-BAF9-11CE-8C82-00AA004BA90B")]
			[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
			[ComImport]
			internal interface IInternetSecurityManager
			{
				// Token: 0x06005556 RID: 21846
				[PreserveSig]
				[return: MarshalAs(UnmanagedType.I4)]
				int SetSecuritySite([In] IntPtr pSite);

				// Token: 0x06005557 RID: 21847
				[PreserveSig]
				[return: MarshalAs(UnmanagedType.I4)]
				int GetSecuritySite([Out] IntPtr pSite);

				// Token: 0x06005558 RID: 21848
				[PreserveSig]
				[return: MarshalAs(UnmanagedType.I4)]
				int MapUrlToZone([MarshalAs(UnmanagedType.LPWStr)] [In] string pwszUrl, out uint pdwZone, uint dwFlags);

				// Token: 0x06005559 RID: 21849
				[PreserveSig]
				[return: MarshalAs(UnmanagedType.I4)]
				int GetSecurityId([MarshalAs(UnmanagedType.LPWStr)] string pwszUrl, [MarshalAs(UnmanagedType.LPArray)] byte[] pbSecurityId, ref uint pcbSecurityId, uint dwReserved);

				// Token: 0x0600555A RID: 21850
				[PreserveSig]
				[return: MarshalAs(UnmanagedType.I4)]
				int ProcessUrlAction([MarshalAs(UnmanagedType.LPWStr)] [In] string pwszUrl, uint dwAction, out byte pPolicy, uint cbPolicy, byte pContext, uint cbContext, uint dwFlags, uint dwReserved);

				// Token: 0x0600555B RID: 21851
				[PreserveSig]
				[return: MarshalAs(UnmanagedType.I4)]
				int QueryCustomPolicy([MarshalAs(UnmanagedType.LPWStr)] [In] string pwszUrl, ref Guid guidKey, ref byte ppPolicy, ref uint pcbPolicy, ref byte pContext, uint cbContext, uint dwReserved);

				// Token: 0x0600555C RID: 21852
				[PreserveSig]
				[return: MarshalAs(UnmanagedType.I4)]
				int SetZoneMapping(uint dwZone, [MarshalAs(UnmanagedType.LPWStr)] [In] string lpszPattern, uint dwFlags);

				// Token: 0x0600555D RID: 21853
				[PreserveSig]
				[return: MarshalAs(UnmanagedType.I4)]
				int GetZoneMappings(uint dwZone, out IEnumString ppenumString, uint dwFlags);
			}
		}
	}
}
