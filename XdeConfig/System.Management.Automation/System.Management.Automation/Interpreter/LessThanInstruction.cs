using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006D9 RID: 1753
	internal abstract class LessThanInstruction : Instruction
	{
		// Token: 0x17000F68 RID: 3944
		// (get) Token: 0x060048AF RID: 18607 RVA: 0x0017F35D File Offset: 0x0017D55D
		public override int ConsumedStack
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000F69 RID: 3945
		// (get) Token: 0x060048B0 RID: 18608 RVA: 0x0017F360 File Offset: 0x0017D560
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060048B1 RID: 18609 RVA: 0x0017F363 File Offset: 0x0017D563
		private LessThanInstruction()
		{
		}

		// Token: 0x060048B2 RID: 18610 RVA: 0x0017F36C File Offset: 0x0017D56C
		public static Instruction Create(Type type)
		{
			switch (type.GetTypeCode())
			{
			case TypeCode.Char:
			{
				Instruction result;
				if ((result = LessThanInstruction._Char) == null)
				{
					result = (LessThanInstruction._Char = new LessThanInstruction.LessThanChar());
				}
				return result;
			}
			case TypeCode.SByte:
			{
				Instruction result2;
				if ((result2 = LessThanInstruction._SByte) == null)
				{
					result2 = (LessThanInstruction._SByte = new LessThanInstruction.LessThanSByte());
				}
				return result2;
			}
			case TypeCode.Byte:
			{
				Instruction result3;
				if ((result3 = LessThanInstruction._Byte) == null)
				{
					result3 = (LessThanInstruction._Byte = new LessThanInstruction.LessThanByte());
				}
				return result3;
			}
			case TypeCode.Int16:
			{
				Instruction result4;
				if ((result4 = LessThanInstruction._Int16) == null)
				{
					result4 = (LessThanInstruction._Int16 = new LessThanInstruction.LessThanInt16());
				}
				return result4;
			}
			case TypeCode.UInt16:
			{
				Instruction result5;
				if ((result5 = LessThanInstruction._UInt16) == null)
				{
					result5 = (LessThanInstruction._UInt16 = new LessThanInstruction.LessThanUInt16());
				}
				return result5;
			}
			case TypeCode.Int32:
			{
				Instruction result6;
				if ((result6 = LessThanInstruction._Int32) == null)
				{
					result6 = (LessThanInstruction._Int32 = new LessThanInstruction.LessThanInt32());
				}
				return result6;
			}
			case TypeCode.UInt32:
			{
				Instruction result7;
				if ((result7 = LessThanInstruction._UInt32) == null)
				{
					result7 = (LessThanInstruction._UInt32 = new LessThanInstruction.LessThanUInt32());
				}
				return result7;
			}
			case TypeCode.Int64:
			{
				Instruction result8;
				if ((result8 = LessThanInstruction._Int64) == null)
				{
					result8 = (LessThanInstruction._Int64 = new LessThanInstruction.LessThanInt64());
				}
				return result8;
			}
			case TypeCode.UInt64:
			{
				Instruction result9;
				if ((result9 = LessThanInstruction._UInt64) == null)
				{
					result9 = (LessThanInstruction._UInt64 = new LessThanInstruction.LessThanUInt64());
				}
				return result9;
			}
			case TypeCode.Single:
			{
				Instruction result10;
				if ((result10 = LessThanInstruction._Single) == null)
				{
					result10 = (LessThanInstruction._Single = new LessThanInstruction.LessThanSingle());
				}
				return result10;
			}
			case TypeCode.Double:
			{
				Instruction result11;
				if ((result11 = LessThanInstruction._Double) == null)
				{
					result11 = (LessThanInstruction._Double = new LessThanInstruction.LessThanDouble());
				}
				return result11;
			}
			default:
				throw Assert.Unreachable;
			}
		}

		// Token: 0x060048B3 RID: 18611 RVA: 0x0017F4A5 File Offset: 0x0017D6A5
		public override string ToString()
		{
			return "LessThan()";
		}

		// Token: 0x04002385 RID: 9093
		private static Instruction _SByte;

		// Token: 0x04002386 RID: 9094
		private static Instruction _Int16;

		// Token: 0x04002387 RID: 9095
		private static Instruction _Char;

		// Token: 0x04002388 RID: 9096
		private static Instruction _Int32;

		// Token: 0x04002389 RID: 9097
		private static Instruction _Int64;

		// Token: 0x0400238A RID: 9098
		private static Instruction _Byte;

		// Token: 0x0400238B RID: 9099
		private static Instruction _UInt16;

		// Token: 0x0400238C RID: 9100
		private static Instruction _UInt32;

		// Token: 0x0400238D RID: 9101
		private static Instruction _UInt64;

		// Token: 0x0400238E RID: 9102
		private static Instruction _Single;

		// Token: 0x0400238F RID: 9103
		private static Instruction _Double;

		// Token: 0x020006DA RID: 1754
		internal sealed class LessThanSByte : LessThanInstruction
		{
			// Token: 0x060048B4 RID: 18612 RVA: 0x0017F4AC File Offset: 0x0017D6AC
			public override int Run(InterpretedFrame frame)
			{
				sbyte b = (sbyte)frame.Pop();
				frame.Push((sbyte)frame.Pop() < b);
				return 1;
			}
		}

		// Token: 0x020006DB RID: 1755
		internal sealed class LessThanInt16 : LessThanInstruction
		{
			// Token: 0x060048B6 RID: 18614 RVA: 0x0017F4E4 File Offset: 0x0017D6E4
			public override int Run(InterpretedFrame frame)
			{
				short num = (short)frame.Pop();
				frame.Push((short)frame.Pop() < num);
				return 1;
			}
		}

		// Token: 0x020006DC RID: 1756
		internal sealed class LessThanChar : LessThanInstruction
		{
			// Token: 0x060048B8 RID: 18616 RVA: 0x0017F51C File Offset: 0x0017D71C
			public override int Run(InterpretedFrame frame)
			{
				char c = (char)frame.Pop();
				frame.Push((char)frame.Pop() < c);
				return 1;
			}
		}

		// Token: 0x020006DD RID: 1757
		internal sealed class LessThanInt32 : LessThanInstruction
		{
			// Token: 0x060048BA RID: 18618 RVA: 0x0017F554 File Offset: 0x0017D754
			public override int Run(InterpretedFrame frame)
			{
				int num = (int)frame.Pop();
				frame.Push((int)frame.Pop() < num);
				return 1;
			}
		}

		// Token: 0x020006DE RID: 1758
		internal sealed class LessThanInt64 : LessThanInstruction
		{
			// Token: 0x060048BC RID: 18620 RVA: 0x0017F58C File Offset: 0x0017D78C
			public override int Run(InterpretedFrame frame)
			{
				long num = (long)frame.Pop();
				frame.Push((long)frame.Pop() < num);
				return 1;
			}
		}

		// Token: 0x020006DF RID: 1759
		internal sealed class LessThanByte : LessThanInstruction
		{
			// Token: 0x060048BE RID: 18622 RVA: 0x0017F5C4 File Offset: 0x0017D7C4
			public override int Run(InterpretedFrame frame)
			{
				byte b = (byte)frame.Pop();
				frame.Push((byte)frame.Pop() < b);
				return 1;
			}
		}

		// Token: 0x020006E0 RID: 1760
		internal sealed class LessThanUInt16 : LessThanInstruction
		{
			// Token: 0x060048C0 RID: 18624 RVA: 0x0017F5FC File Offset: 0x0017D7FC
			public override int Run(InterpretedFrame frame)
			{
				ushort num = (ushort)frame.Pop();
				frame.Push((ushort)frame.Pop() < num);
				return 1;
			}
		}

		// Token: 0x020006E1 RID: 1761
		internal sealed class LessThanUInt32 : LessThanInstruction
		{
			// Token: 0x060048C2 RID: 18626 RVA: 0x0017F634 File Offset: 0x0017D834
			public override int Run(InterpretedFrame frame)
			{
				uint num = (uint)frame.Pop();
				frame.Push((uint)frame.Pop() < num);
				return 1;
			}
		}

		// Token: 0x020006E2 RID: 1762
		internal sealed class LessThanUInt64 : LessThanInstruction
		{
			// Token: 0x060048C4 RID: 18628 RVA: 0x0017F66C File Offset: 0x0017D86C
			public override int Run(InterpretedFrame frame)
			{
				ulong num = (ulong)frame.Pop();
				frame.Push((ulong)frame.Pop() < num);
				return 1;
			}
		}

		// Token: 0x020006E3 RID: 1763
		internal sealed class LessThanSingle : LessThanInstruction
		{
			// Token: 0x060048C6 RID: 18630 RVA: 0x0017F6A4 File Offset: 0x0017D8A4
			public override int Run(InterpretedFrame frame)
			{
				float num = (float)frame.Pop();
				frame.Push((float)frame.Pop() < num);
				return 1;
			}
		}

		// Token: 0x020006E4 RID: 1764
		internal sealed class LessThanDouble : LessThanInstruction
		{
			// Token: 0x060048C8 RID: 18632 RVA: 0x0017F6DC File Offset: 0x0017D8DC
			public override int Run(InterpretedFrame frame)
			{
				double num = (double)frame.Pop();
				frame.Push((double)frame.Pop() < num);
				return 1;
			}
		}
	}
}
