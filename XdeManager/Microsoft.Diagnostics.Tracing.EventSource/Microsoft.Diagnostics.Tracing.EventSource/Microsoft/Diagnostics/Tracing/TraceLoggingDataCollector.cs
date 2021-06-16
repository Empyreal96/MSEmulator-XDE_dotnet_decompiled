using System;
using System.Security;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000076 RID: 118
	[SecuritySafeCritical]
	internal class TraceLoggingDataCollector
	{
		// Token: 0x060002DB RID: 731 RVA: 0x0000F715 File Offset: 0x0000D915
		private TraceLoggingDataCollector()
		{
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000F71D File Offset: 0x0000D91D
		public int BeginBufferedArray()
		{
			return DataCollector.ThreadInstance.BeginBufferedArray();
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000F729 File Offset: 0x0000D929
		public void EndBufferedArray(int bookmark, int count)
		{
			DataCollector.ThreadInstance.EndBufferedArray(bookmark, count);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000F737 File Offset: 0x0000D937
		public TraceLoggingDataCollector AddGroup()
		{
			return this;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000F73A File Offset: 0x0000D93A
		public unsafe void AddScalar(bool value)
		{
			DataCollector.ThreadInstance.AddScalar((void*)(&value), 1);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000F74A File Offset: 0x0000D94A
		public unsafe void AddScalar(sbyte value)
		{
			DataCollector.ThreadInstance.AddScalar((void*)(&value), 1);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000F75A File Offset: 0x0000D95A
		public unsafe void AddScalar(byte value)
		{
			DataCollector.ThreadInstance.AddScalar((void*)(&value), 1);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000F76A File Offset: 0x0000D96A
		public unsafe void AddScalar(short value)
		{
			DataCollector.ThreadInstance.AddScalar((void*)(&value), 2);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000F77A File Offset: 0x0000D97A
		public unsafe void AddScalar(ushort value)
		{
			DataCollector.ThreadInstance.AddScalar((void*)(&value), 2);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000F78A File Offset: 0x0000D98A
		public unsafe void AddScalar(int value)
		{
			DataCollector.ThreadInstance.AddScalar((void*)(&value), 4);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000F79A File Offset: 0x0000D99A
		public unsafe void AddScalar(uint value)
		{
			DataCollector.ThreadInstance.AddScalar((void*)(&value), 4);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000F7AA File Offset: 0x0000D9AA
		public unsafe void AddScalar(long value)
		{
			DataCollector.ThreadInstance.AddScalar((void*)(&value), 8);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000F7BA File Offset: 0x0000D9BA
		public unsafe void AddScalar(ulong value)
		{
			DataCollector.ThreadInstance.AddScalar((void*)(&value), 8);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000F7CA File Offset: 0x0000D9CA
		public unsafe void AddScalar(IntPtr value)
		{
			DataCollector.ThreadInstance.AddScalar((void*)(&value), IntPtr.Size);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000F7DE File Offset: 0x0000D9DE
		public unsafe void AddScalar(UIntPtr value)
		{
			DataCollector.ThreadInstance.AddScalar((void*)(&value), UIntPtr.Size);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000F7F2 File Offset: 0x0000D9F2
		public unsafe void AddScalar(float value)
		{
			DataCollector.ThreadInstance.AddScalar((void*)(&value), 4);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000F802 File Offset: 0x0000DA02
		public unsafe void AddScalar(double value)
		{
			DataCollector.ThreadInstance.AddScalar((void*)(&value), 8);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000F812 File Offset: 0x0000DA12
		public unsafe void AddScalar(char value)
		{
			DataCollector.ThreadInstance.AddScalar((void*)(&value), 2);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000F822 File Offset: 0x0000DA22
		public unsafe void AddScalar(Guid value)
		{
			DataCollector.ThreadInstance.AddScalar((void*)(&value), 16);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000F833 File Offset: 0x0000DA33
		public void AddBinary(string value)
		{
			DataCollector.ThreadInstance.AddBinary(value, (value == null) ? 0 : checked(value.Length * 2));
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000F84E File Offset: 0x0000DA4E
		public void AddBinary(byte[] value)
		{
			DataCollector.ThreadInstance.AddBinary(value, (value == null) ? 0 : value.Length);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000F864 File Offset: 0x0000DA64
		public void AddArray(bool[] value)
		{
			DataCollector.ThreadInstance.AddArray(value, (value == null) ? 0 : value.Length, 1);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000F87B File Offset: 0x0000DA7B
		public void AddArray(sbyte[] value)
		{
			DataCollector.ThreadInstance.AddArray(value, (value == null) ? 0 : value.Length, 1);
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000F892 File Offset: 0x0000DA92
		public void AddArray(short[] value)
		{
			DataCollector.ThreadInstance.AddArray(value, (value == null) ? 0 : value.Length, 2);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000F8A9 File Offset: 0x0000DAA9
		public void AddArray(ushort[] value)
		{
			DataCollector.ThreadInstance.AddArray(value, (value == null) ? 0 : value.Length, 2);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000F8C0 File Offset: 0x0000DAC0
		public void AddArray(int[] value)
		{
			DataCollector.ThreadInstance.AddArray(value, (value == null) ? 0 : value.Length, 4);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000F8D7 File Offset: 0x0000DAD7
		public void AddArray(uint[] value)
		{
			DataCollector.ThreadInstance.AddArray(value, (value == null) ? 0 : value.Length, 4);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000F8EE File Offset: 0x0000DAEE
		public void AddArray(long[] value)
		{
			DataCollector.ThreadInstance.AddArray(value, (value == null) ? 0 : value.Length, 8);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000F905 File Offset: 0x0000DB05
		public void AddArray(ulong[] value)
		{
			DataCollector.ThreadInstance.AddArray(value, (value == null) ? 0 : value.Length, 8);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000F91C File Offset: 0x0000DB1C
		public void AddArray(IntPtr[] value)
		{
			DataCollector.ThreadInstance.AddArray(value, (value == null) ? 0 : value.Length, IntPtr.Size);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000F937 File Offset: 0x0000DB37
		public void AddArray(UIntPtr[] value)
		{
			DataCollector.ThreadInstance.AddArray(value, (value == null) ? 0 : value.Length, UIntPtr.Size);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000F952 File Offset: 0x0000DB52
		public void AddArray(float[] value)
		{
			DataCollector.ThreadInstance.AddArray(value, (value == null) ? 0 : value.Length, 4);
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000F969 File Offset: 0x0000DB69
		public void AddArray(double[] value)
		{
			DataCollector.ThreadInstance.AddArray(value, (value == null) ? 0 : value.Length, 8);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000F980 File Offset: 0x0000DB80
		public void AddArray(char[] value)
		{
			DataCollector.ThreadInstance.AddArray(value, (value == null) ? 0 : value.Length, 2);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000F997 File Offset: 0x0000DB97
		public void AddArray(Guid[] value)
		{
			DataCollector.ThreadInstance.AddArray(value, (value == null) ? 0 : value.Length, 16);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000F9AF File Offset: 0x0000DBAF
		public void AddCustom(byte[] value)
		{
			DataCollector.ThreadInstance.AddArray(value, (value == null) ? 0 : value.Length, 1);
		}

		// Token: 0x04000152 RID: 338
		internal static readonly TraceLoggingDataCollector Instance = new TraceLoggingDataCollector();
	}
}
