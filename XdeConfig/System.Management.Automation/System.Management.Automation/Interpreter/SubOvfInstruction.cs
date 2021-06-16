using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200073D RID: 1853
	internal abstract class SubOvfInstruction : Instruction
	{
		// Token: 0x17000FA6 RID: 4006
		// (get) Token: 0x06004A9A RID: 19098 RVA: 0x00187AB1 File Offset: 0x00185CB1
		public override int ConsumedStack
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000FA7 RID: 4007
		// (get) Token: 0x06004A9B RID: 19099 RVA: 0x00187AB4 File Offset: 0x00185CB4
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06004A9C RID: 19100 RVA: 0x00187AB7 File Offset: 0x00185CB7
		private SubOvfInstruction()
		{
		}

		// Token: 0x06004A9D RID: 19101 RVA: 0x00187AC0 File Offset: 0x00185CC0
		public static Instruction Create(Type type)
		{
			switch (type.GetTypeCode())
			{
			case TypeCode.Int16:
			{
				Instruction result;
				if ((result = SubOvfInstruction._Int16) == null)
				{
					result = (SubOvfInstruction._Int16 = new SubOvfInstruction.SubOvfInt16());
				}
				return result;
			}
			case TypeCode.UInt16:
			{
				Instruction result2;
				if ((result2 = SubOvfInstruction._UInt16) == null)
				{
					result2 = (SubOvfInstruction._UInt16 = new SubOvfInstruction.SubOvfUInt16());
				}
				return result2;
			}
			case TypeCode.Int32:
			{
				Instruction result3;
				if ((result3 = SubOvfInstruction._Int32) == null)
				{
					result3 = (SubOvfInstruction._Int32 = new SubOvfInstruction.SubOvfInt32());
				}
				return result3;
			}
			case TypeCode.UInt32:
			{
				Instruction result4;
				if ((result4 = SubOvfInstruction._UInt32) == null)
				{
					result4 = (SubOvfInstruction._UInt32 = new SubOvfInstruction.SubOvfUInt32());
				}
				return result4;
			}
			case TypeCode.Int64:
			{
				Instruction result5;
				if ((result5 = SubOvfInstruction._Int64) == null)
				{
					result5 = (SubOvfInstruction._Int64 = new SubOvfInstruction.SubOvfInt64());
				}
				return result5;
			}
			case TypeCode.UInt64:
			{
				Instruction result6;
				if ((result6 = SubOvfInstruction._UInt64) == null)
				{
					result6 = (SubOvfInstruction._UInt64 = new SubOvfInstruction.SubOvfUInt64());
				}
				return result6;
			}
			case TypeCode.Single:
			{
				Instruction result7;
				if ((result7 = SubOvfInstruction._Single) == null)
				{
					result7 = (SubOvfInstruction._Single = new SubOvfInstruction.SubOvfSingle());
				}
				return result7;
			}
			case TypeCode.Double:
			{
				Instruction result8;
				if ((result8 = SubOvfInstruction._Double) == null)
				{
					result8 = (SubOvfInstruction._Double = new SubOvfInstruction.SubOvfDouble());
				}
				return result8;
			}
			default:
				throw Assert.Unreachable;
			}
		}

		// Token: 0x06004A9E RID: 19102 RVA: 0x00187BAE File Offset: 0x00185DAE
		public override string ToString()
		{
			return "SubOvf()";
		}

		// Token: 0x0400241B RID: 9243
		private static Instruction _Int16;

		// Token: 0x0400241C RID: 9244
		private static Instruction _Int32;

		// Token: 0x0400241D RID: 9245
		private static Instruction _Int64;

		// Token: 0x0400241E RID: 9246
		private static Instruction _UInt16;

		// Token: 0x0400241F RID: 9247
		private static Instruction _UInt32;

		// Token: 0x04002420 RID: 9248
		private static Instruction _UInt64;

		// Token: 0x04002421 RID: 9249
		private static Instruction _Single;

		// Token: 0x04002422 RID: 9250
		private static Instruction _Double;

		// Token: 0x0200073E RID: 1854
		internal sealed class SubOvfInt32 : SubOvfInstruction
		{
			// Token: 0x06004A9F RID: 19103 RVA: 0x00187BB8 File Offset: 0x00185DB8
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = ScriptingRuntimeHelpers.Int32ToObject(checked((int)obj - (int)obj2));
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x0200073F RID: 1855
		internal sealed class SubOvfInt16 : SubOvfInstruction
		{
			// Token: 0x06004AA1 RID: 19105 RVA: 0x00187C20 File Offset: 0x00185E20
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = checked((short)obj - (short)obj2);
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000740 RID: 1856
		internal sealed class SubOvfInt64 : SubOvfInstruction
		{
			// Token: 0x06004AA3 RID: 19107 RVA: 0x00187C88 File Offset: 0x00185E88
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = checked((long)obj - (long)obj2);
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000741 RID: 1857
		internal sealed class SubOvfUInt16 : SubOvfInstruction
		{
			// Token: 0x06004AA5 RID: 19109 RVA: 0x00187CF0 File Offset: 0x00185EF0
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = checked((ushort)obj - (ushort)obj2);
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000742 RID: 1858
		internal sealed class SubOvfUInt32 : SubOvfInstruction
		{
			// Token: 0x06004AA7 RID: 19111 RVA: 0x00187D58 File Offset: 0x00185F58
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = checked((uint)obj - (uint)obj2);
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000743 RID: 1859
		internal sealed class SubOvfUInt64 : SubOvfInstruction
		{
			// Token: 0x06004AA9 RID: 19113 RVA: 0x00187DC0 File Offset: 0x00185FC0
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (ulong)((long)(checked((short)obj - (short)obj2)));
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000744 RID: 1860
		internal sealed class SubOvfSingle : SubOvfInstruction
		{
			// Token: 0x06004AAB RID: 19115 RVA: 0x00187E28 File Offset: 0x00186028
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (float)obj - (float)obj2;
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000745 RID: 1861
		internal sealed class SubOvfDouble : SubOvfInstruction
		{
			// Token: 0x06004AAD RID: 19117 RVA: 0x00187E90 File Offset: 0x00186090
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (double)obj - (double)obj2;
				frame.StackIndex--;
				return 1;
			}
		}
	}
}
