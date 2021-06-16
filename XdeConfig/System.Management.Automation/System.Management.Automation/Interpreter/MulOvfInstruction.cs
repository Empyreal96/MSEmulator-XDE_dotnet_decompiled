using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000714 RID: 1812
	internal abstract class MulOvfInstruction : Instruction
	{
		// Token: 0x17000F95 RID: 3989
		// (get) Token: 0x06004A21 RID: 18977 RVA: 0x00186455 File Offset: 0x00184655
		public override int ConsumedStack
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000F96 RID: 3990
		// (get) Token: 0x06004A22 RID: 18978 RVA: 0x00186458 File Offset: 0x00184658
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06004A23 RID: 18979 RVA: 0x0018645B File Offset: 0x0018465B
		private MulOvfInstruction()
		{
		}

		// Token: 0x06004A24 RID: 18980 RVA: 0x00186464 File Offset: 0x00184664
		public static Instruction Create(Type type)
		{
			switch (type.GetTypeCode())
			{
			case TypeCode.Int16:
			{
				Instruction result;
				if ((result = MulOvfInstruction._Int16) == null)
				{
					result = (MulOvfInstruction._Int16 = new MulOvfInstruction.MulOvfInt16());
				}
				return result;
			}
			case TypeCode.UInt16:
			{
				Instruction result2;
				if ((result2 = MulOvfInstruction._UInt16) == null)
				{
					result2 = (MulOvfInstruction._UInt16 = new MulOvfInstruction.MulOvfUInt16());
				}
				return result2;
			}
			case TypeCode.Int32:
			{
				Instruction result3;
				if ((result3 = MulOvfInstruction._Int32) == null)
				{
					result3 = (MulOvfInstruction._Int32 = new MulOvfInstruction.MulOvfInt32());
				}
				return result3;
			}
			case TypeCode.UInt32:
			{
				Instruction result4;
				if ((result4 = MulOvfInstruction._UInt32) == null)
				{
					result4 = (MulOvfInstruction._UInt32 = new MulOvfInstruction.MulOvfUInt32());
				}
				return result4;
			}
			case TypeCode.Int64:
			{
				Instruction result5;
				if ((result5 = MulOvfInstruction._Int64) == null)
				{
					result5 = (MulOvfInstruction._Int64 = new MulOvfInstruction.MulOvfInt64());
				}
				return result5;
			}
			case TypeCode.UInt64:
			{
				Instruction result6;
				if ((result6 = MulOvfInstruction._UInt64) == null)
				{
					result6 = (MulOvfInstruction._UInt64 = new MulOvfInstruction.MulOvfUInt64());
				}
				return result6;
			}
			case TypeCode.Single:
			{
				Instruction result7;
				if ((result7 = MulOvfInstruction._Single) == null)
				{
					result7 = (MulOvfInstruction._Single = new MulOvfInstruction.MulOvfSingle());
				}
				return result7;
			}
			case TypeCode.Double:
			{
				Instruction result8;
				if ((result8 = MulOvfInstruction._Double) == null)
				{
					result8 = (MulOvfInstruction._Double = new MulOvfInstruction.MulOvfDouble());
				}
				return result8;
			}
			default:
				throw Assert.Unreachable;
			}
		}

		// Token: 0x06004A25 RID: 18981 RVA: 0x00186552 File Offset: 0x00184752
		public override string ToString()
		{
			return "MulOvf()";
		}

		// Token: 0x040023F5 RID: 9205
		private static Instruction _Int16;

		// Token: 0x040023F6 RID: 9206
		private static Instruction _Int32;

		// Token: 0x040023F7 RID: 9207
		private static Instruction _Int64;

		// Token: 0x040023F8 RID: 9208
		private static Instruction _UInt16;

		// Token: 0x040023F9 RID: 9209
		private static Instruction _UInt32;

		// Token: 0x040023FA RID: 9210
		private static Instruction _UInt64;

		// Token: 0x040023FB RID: 9211
		private static Instruction _Single;

		// Token: 0x040023FC RID: 9212
		private static Instruction _Double;

		// Token: 0x02000715 RID: 1813
		internal sealed class MulOvfInt32 : MulOvfInstruction
		{
			// Token: 0x06004A26 RID: 18982 RVA: 0x0018655C File Offset: 0x0018475C
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = ScriptingRuntimeHelpers.Int32ToObject(checked((int)obj * (int)obj2));
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000716 RID: 1814
		internal sealed class MulOvfInt16 : MulOvfInstruction
		{
			// Token: 0x06004A28 RID: 18984 RVA: 0x001865C4 File Offset: 0x001847C4
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = checked((short)obj * (short)obj2);
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000717 RID: 1815
		internal sealed class MulOvfInt64 : MulOvfInstruction
		{
			// Token: 0x06004A2A RID: 18986 RVA: 0x0018662C File Offset: 0x0018482C
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = checked((long)obj * (long)obj2);
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000718 RID: 1816
		internal sealed class MulOvfUInt16 : MulOvfInstruction
		{
			// Token: 0x06004A2C RID: 18988 RVA: 0x00186694 File Offset: 0x00184894
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = checked((ushort)obj * (ushort)obj2);
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000719 RID: 1817
		internal sealed class MulOvfUInt32 : MulOvfInstruction
		{
			// Token: 0x06004A2E RID: 18990 RVA: 0x001866FC File Offset: 0x001848FC
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = checked((uint)obj * (uint)obj2);
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x0200071A RID: 1818
		internal sealed class MulOvfUInt64 : MulOvfInstruction
		{
			// Token: 0x06004A30 RID: 18992 RVA: 0x00186764 File Offset: 0x00184964
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (ulong)((long)(checked((short)obj * (short)obj2)));
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x0200071B RID: 1819
		internal sealed class MulOvfSingle : MulOvfInstruction
		{
			// Token: 0x06004A32 RID: 18994 RVA: 0x001867CC File Offset: 0x001849CC
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (float)obj * (float)obj2;
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x0200071C RID: 1820
		internal sealed class MulOvfDouble : MulOvfInstruction
		{
			// Token: 0x06004A34 RID: 18996 RVA: 0x00186834 File Offset: 0x00184A34
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (double)obj * (double)obj2;
				frame.StackIndex--;
				return 1;
			}
		}
	}
}
