using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000692 RID: 1682
	internal abstract class DivInstruction : Instruction
	{
		// Token: 0x17000F1D RID: 3869
		// (get) Token: 0x06004714 RID: 18196 RVA: 0x0017ADE5 File Offset: 0x00178FE5
		public override int ConsumedStack
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000F1E RID: 3870
		// (get) Token: 0x06004715 RID: 18197 RVA: 0x0017ADE8 File Offset: 0x00178FE8
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06004716 RID: 18198 RVA: 0x0017ADEB File Offset: 0x00178FEB
		private DivInstruction()
		{
		}

		// Token: 0x06004717 RID: 18199 RVA: 0x0017ADF4 File Offset: 0x00178FF4
		public static Instruction Create(Type type)
		{
			switch (type.GetTypeCode())
			{
			case TypeCode.Int16:
			{
				Instruction result;
				if ((result = DivInstruction._Int16) == null)
				{
					result = (DivInstruction._Int16 = new DivInstruction.DivInt16());
				}
				return result;
			}
			case TypeCode.UInt16:
			{
				Instruction result2;
				if ((result2 = DivInstruction._UInt16) == null)
				{
					result2 = (DivInstruction._UInt16 = new DivInstruction.DivUInt16());
				}
				return result2;
			}
			case TypeCode.Int32:
			{
				Instruction result3;
				if ((result3 = DivInstruction._Int32) == null)
				{
					result3 = (DivInstruction._Int32 = new DivInstruction.DivInt32());
				}
				return result3;
			}
			case TypeCode.UInt32:
			{
				Instruction result4;
				if ((result4 = DivInstruction._UInt32) == null)
				{
					result4 = (DivInstruction._UInt32 = new DivInstruction.DivUInt32());
				}
				return result4;
			}
			case TypeCode.Int64:
			{
				Instruction result5;
				if ((result5 = DivInstruction._Int64) == null)
				{
					result5 = (DivInstruction._Int64 = new DivInstruction.DivInt64());
				}
				return result5;
			}
			case TypeCode.UInt64:
			{
				Instruction result6;
				if ((result6 = DivInstruction._UInt64) == null)
				{
					result6 = (DivInstruction._UInt64 = new DivInstruction.DivUInt64());
				}
				return result6;
			}
			case TypeCode.Single:
			{
				Instruction result7;
				if ((result7 = DivInstruction._Single) == null)
				{
					result7 = (DivInstruction._Single = new DivInstruction.DivSingle());
				}
				return result7;
			}
			case TypeCode.Double:
			{
				Instruction result8;
				if ((result8 = DivInstruction._Double) == null)
				{
					result8 = (DivInstruction._Double = new DivInstruction.DivDouble());
				}
				return result8;
			}
			default:
				throw Assert.Unreachable;
			}
		}

		// Token: 0x06004718 RID: 18200 RVA: 0x0017AEE2 File Offset: 0x001790E2
		public override string ToString()
		{
			return "Div()";
		}

		// Token: 0x040022E5 RID: 8933
		private static Instruction _Int16;

		// Token: 0x040022E6 RID: 8934
		private static Instruction _Int32;

		// Token: 0x040022E7 RID: 8935
		private static Instruction _Int64;

		// Token: 0x040022E8 RID: 8936
		private static Instruction _UInt16;

		// Token: 0x040022E9 RID: 8937
		private static Instruction _UInt32;

		// Token: 0x040022EA RID: 8938
		private static Instruction _UInt64;

		// Token: 0x040022EB RID: 8939
		private static Instruction _Single;

		// Token: 0x040022EC RID: 8940
		private static Instruction _Double;

		// Token: 0x02000693 RID: 1683
		internal sealed class DivInt32 : DivInstruction
		{
			// Token: 0x06004719 RID: 18201 RVA: 0x0017AEEC File Offset: 0x001790EC
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = ScriptingRuntimeHelpers.Int32ToObject((int)obj / (int)obj2);
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000694 RID: 1684
		internal sealed class DivInt16 : DivInstruction
		{
			// Token: 0x0600471B RID: 18203 RVA: 0x0017AF54 File Offset: 0x00179154
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (short)obj / (short)obj2;
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000695 RID: 1685
		internal sealed class DivInt64 : DivInstruction
		{
			// Token: 0x0600471D RID: 18205 RVA: 0x0017AFBC File Offset: 0x001791BC
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (long)obj / (long)obj2;
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000696 RID: 1686
		internal sealed class DivUInt16 : DivInstruction
		{
			// Token: 0x0600471F RID: 18207 RVA: 0x0017B024 File Offset: 0x00179224
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (ushort)obj / (ushort)obj2;
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000697 RID: 1687
		internal sealed class DivUInt32 : DivInstruction
		{
			// Token: 0x06004721 RID: 18209 RVA: 0x0017B08C File Offset: 0x0017928C
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (uint)obj / (uint)obj2;
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000698 RID: 1688
		internal sealed class DivUInt64 : DivInstruction
		{
			// Token: 0x06004723 RID: 18211 RVA: 0x0017B0F4 File Offset: 0x001792F4
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (ulong)((long)((short)obj / (short)obj2));
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000699 RID: 1689
		internal sealed class DivSingle : DivInstruction
		{
			// Token: 0x06004725 RID: 18213 RVA: 0x0017B15C File Offset: 0x0017935C
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (float)obj / (float)obj2;
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x0200069A RID: 1690
		internal sealed class DivDouble : DivInstruction
		{
			// Token: 0x06004727 RID: 18215 RVA: 0x0017B1C4 File Offset: 0x001793C4
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (double)obj / (double)obj2;
				frame.StackIndex--;
				return 1;
			}
		}
	}
}
