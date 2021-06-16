using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200070B RID: 1803
	internal abstract class MulInstruction : Instruction
	{
		// Token: 0x17000F93 RID: 3987
		// (get) Token: 0x06004A0C RID: 18956 RVA: 0x00186010 File Offset: 0x00184210
		public override int ConsumedStack
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000F94 RID: 3988
		// (get) Token: 0x06004A0D RID: 18957 RVA: 0x00186013 File Offset: 0x00184213
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06004A0E RID: 18958 RVA: 0x00186016 File Offset: 0x00184216
		private MulInstruction()
		{
		}

		// Token: 0x06004A0F RID: 18959 RVA: 0x00186020 File Offset: 0x00184220
		public static Instruction Create(Type type)
		{
			switch (type.GetTypeCode())
			{
			case TypeCode.Int16:
			{
				Instruction result;
				if ((result = MulInstruction._Int16) == null)
				{
					result = (MulInstruction._Int16 = new MulInstruction.MulInt16());
				}
				return result;
			}
			case TypeCode.UInt16:
			{
				Instruction result2;
				if ((result2 = MulInstruction._UInt16) == null)
				{
					result2 = (MulInstruction._UInt16 = new MulInstruction.MulUInt16());
				}
				return result2;
			}
			case TypeCode.Int32:
			{
				Instruction result3;
				if ((result3 = MulInstruction._Int32) == null)
				{
					result3 = (MulInstruction._Int32 = new MulInstruction.MulInt32());
				}
				return result3;
			}
			case TypeCode.UInt32:
			{
				Instruction result4;
				if ((result4 = MulInstruction._UInt32) == null)
				{
					result4 = (MulInstruction._UInt32 = new MulInstruction.MulUInt32());
				}
				return result4;
			}
			case TypeCode.Int64:
			{
				Instruction result5;
				if ((result5 = MulInstruction._Int64) == null)
				{
					result5 = (MulInstruction._Int64 = new MulInstruction.MulInt64());
				}
				return result5;
			}
			case TypeCode.UInt64:
			{
				Instruction result6;
				if ((result6 = MulInstruction._UInt64) == null)
				{
					result6 = (MulInstruction._UInt64 = new MulInstruction.MulUInt64());
				}
				return result6;
			}
			case TypeCode.Single:
			{
				Instruction result7;
				if ((result7 = MulInstruction._Single) == null)
				{
					result7 = (MulInstruction._Single = new MulInstruction.MulSingle());
				}
				return result7;
			}
			case TypeCode.Double:
			{
				Instruction result8;
				if ((result8 = MulInstruction._Double) == null)
				{
					result8 = (MulInstruction._Double = new MulInstruction.MulDouble());
				}
				return result8;
			}
			default:
				throw Assert.Unreachable;
			}
		}

		// Token: 0x06004A10 RID: 18960 RVA: 0x0018610E File Offset: 0x0018430E
		public override string ToString()
		{
			return "Mul()";
		}

		// Token: 0x040023ED RID: 9197
		private static Instruction _Int16;

		// Token: 0x040023EE RID: 9198
		private static Instruction _Int32;

		// Token: 0x040023EF RID: 9199
		private static Instruction _Int64;

		// Token: 0x040023F0 RID: 9200
		private static Instruction _UInt16;

		// Token: 0x040023F1 RID: 9201
		private static Instruction _UInt32;

		// Token: 0x040023F2 RID: 9202
		private static Instruction _UInt64;

		// Token: 0x040023F3 RID: 9203
		private static Instruction _Single;

		// Token: 0x040023F4 RID: 9204
		private static Instruction _Double;

		// Token: 0x0200070C RID: 1804
		internal sealed class MulInt32 : MulInstruction
		{
			// Token: 0x06004A11 RID: 18961 RVA: 0x00186118 File Offset: 0x00184318
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = ScriptingRuntimeHelpers.Int32ToObject((int)obj * (int)obj2);
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x0200070D RID: 1805
		internal sealed class MulInt16 : MulInstruction
		{
			// Token: 0x06004A13 RID: 18963 RVA: 0x00186180 File Offset: 0x00184380
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (short)obj * (short)obj2;
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x0200070E RID: 1806
		internal sealed class MulInt64 : MulInstruction
		{
			// Token: 0x06004A15 RID: 18965 RVA: 0x001861E8 File Offset: 0x001843E8
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (long)obj * (long)obj2;
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x0200070F RID: 1807
		internal sealed class MulUInt16 : MulInstruction
		{
			// Token: 0x06004A17 RID: 18967 RVA: 0x00186250 File Offset: 0x00184450
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (ushort)obj * (ushort)obj2;
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000710 RID: 1808
		internal sealed class MulUInt32 : MulInstruction
		{
			// Token: 0x06004A19 RID: 18969 RVA: 0x001862B8 File Offset: 0x001844B8
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (uint)obj * (uint)obj2;
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000711 RID: 1809
		internal sealed class MulUInt64 : MulInstruction
		{
			// Token: 0x06004A1B RID: 18971 RVA: 0x00186320 File Offset: 0x00184520
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (ulong)((long)((short)obj * (short)obj2));
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000712 RID: 1810
		internal sealed class MulSingle : MulInstruction
		{
			// Token: 0x06004A1D RID: 18973 RVA: 0x00186388 File Offset: 0x00184588
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (float)obj * (float)obj2;
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000713 RID: 1811
		internal sealed class MulDouble : MulInstruction
		{
			// Token: 0x06004A1F RID: 18975 RVA: 0x001863F0 File Offset: 0x001845F0
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
