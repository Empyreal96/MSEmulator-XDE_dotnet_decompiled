using System;
using System.Runtime.InteropServices;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000024 RID: 36
	public static class StructUtils
	{
		// Token: 0x0600018A RID: 394 RVA: 0x00003EDC File Offset: 0x000020DC
		public static T GetStructFromBytes<T>(byte[] buffer)
		{
			GCHandle gchandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			T result;
			try
			{
				result = (T)((object)Marshal.PtrToStructure(gchandle.AddrOfPinnedObject(), typeof(T)));
			}
			finally
			{
				gchandle.Free();
			}
			return result;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00003F28 File Offset: 0x00002128
		public static byte[] GetStructBytes(object obj)
		{
			byte[] array = new byte[Marshal.SizeOf(obj)];
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			try
			{
				Marshal.StructureToPtr(obj, gchandle.AddrOfPinnedObject(), false);
			}
			finally
			{
				gchandle.Free();
			}
			return array;
		}
	}
}
