using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006BF RID: 1727
	internal abstract class GreaterThanInstruction : Instruction
	{
		// Token: 0x17000F4C RID: 3916
		// (get) Token: 0x060047C4 RID: 18372 RVA: 0x0017CCBE File Offset: 0x0017AEBE
		public override int ConsumedStack
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000F4D RID: 3917
		// (get) Token: 0x060047C5 RID: 18373 RVA: 0x0017CCC1 File Offset: 0x0017AEC1
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060047C6 RID: 18374 RVA: 0x0017CCC4 File Offset: 0x0017AEC4
		private GreaterThanInstruction()
		{
		}

		// Token: 0x060047C7 RID: 18375 RVA: 0x0017CCCC File Offset: 0x0017AECC
		public static Instruction Create(Type type)
		{
			switch (type.GetTypeCode())
			{
			case TypeCode.Char:
			{
				Instruction result;
				if ((result = GreaterThanInstruction._Char) == null)
				{
					result = (GreaterThanInstruction._Char = new GreaterThanInstruction.GreaterThanChar());
				}
				return result;
			}
			case TypeCode.SByte:
			{
				Instruction result2;
				if ((result2 = GreaterThanInstruction._SByte) == null)
				{
					result2 = (GreaterThanInstruction._SByte = new GreaterThanInstruction.GreaterThanSByte());
				}
				return result2;
			}
			case TypeCode.Byte:
			{
				Instruction result3;
				if ((result3 = GreaterThanInstruction._Byte) == null)
				{
					result3 = (GreaterThanInstruction._Byte = new GreaterThanInstruction.GreaterThanByte());
				}
				return result3;
			}
			case TypeCode.Int16:
			{
				Instruction result4;
				if ((result4 = GreaterThanInstruction._Int16) == null)
				{
					result4 = (GreaterThanInstruction._Int16 = new GreaterThanInstruction.GreaterThanInt16());
				}
				return result4;
			}
			case TypeCode.UInt16:
			{
				Instruction result5;
				if ((result5 = GreaterThanInstruction._UInt16) == null)
				{
					result5 = (GreaterThanInstruction._UInt16 = new GreaterThanInstruction.GreaterThanUInt16());
				}
				return result5;
			}
			case TypeCode.Int32:
			{
				Instruction result6;
				if ((result6 = GreaterThanInstruction._Int32) == null)
				{
					result6 = (GreaterThanInstruction._Int32 = new GreaterThanInstruction.GreaterThanInt32());
				}
				return result6;
			}
			case TypeCode.UInt32:
			{
				Instruction result7;
				if ((result7 = GreaterThanInstruction._UInt32) == null)
				{
					result7 = (GreaterThanInstruction._UInt32 = new GreaterThanInstruction.GreaterThanUInt32());
				}
				return result7;
			}
			case TypeCode.Int64:
			{
				Instruction result8;
				if ((result8 = GreaterThanInstruction._Int64) == null)
				{
					result8 = (GreaterThanInstruction._Int64 = new GreaterThanInstruction.GreaterThanInt64());
				}
				return result8;
			}
			case TypeCode.UInt64:
			{
				Instruction result9;
				if ((result9 = GreaterThanInstruction._UInt64) == null)
				{
					result9 = (GreaterThanInstruction._UInt64 = new GreaterThanInstruction.GreaterThanUInt64());
				}
				return result9;
			}
			case TypeCode.Single:
			{
				Instruction result10;
				if ((result10 = GreaterThanInstruction._Single) == null)
				{
					result10 = (GreaterThanInstruction._Single = new GreaterThanInstruction.GreaterThanSingle());
				}
				return result10;
			}
			case TypeCode.Double:
			{
				Instruction result11;
				if ((result11 = GreaterThanInstruction._Double) == null)
				{
					result11 = (GreaterThanInstruction._Double = new GreaterThanInstruction.GreaterThanDouble());
				}
				return result11;
			}
			default:
				throw Assert.Unreachable;
			}
		}

		// Token: 0x060047C8 RID: 18376 RVA: 0x0017CE05 File Offset: 0x0017B005
		public override string ToString()
		{
			return "GreaterThan()";
		}

		// Token: 0x04002315 RID: 8981
		private static Instruction _SByte;

		// Token: 0x04002316 RID: 8982
		private static Instruction _Int16;

		// Token: 0x04002317 RID: 8983
		private static Instruction _Char;

		// Token: 0x04002318 RID: 8984
		private static Instruction _Int32;

		// Token: 0x04002319 RID: 8985
		private static Instruction _Int64;

		// Token: 0x0400231A RID: 8986
		private static Instruction _Byte;

		// Token: 0x0400231B RID: 8987
		private static Instruction _UInt16;

		// Token: 0x0400231C RID: 8988
		private static Instruction _UInt32;

		// Token: 0x0400231D RID: 8989
		private static Instruction _UInt64;

		// Token: 0x0400231E RID: 8990
		private static Instruction _Single;

		// Token: 0x0400231F RID: 8991
		private static Instruction _Double;

		// Token: 0x020006C0 RID: 1728
		internal sealed class GreaterThanSByte : GreaterThanInstruction
		{
			// Token: 0x060047C9 RID: 18377 RVA: 0x0017CE0C File Offset: 0x0017B00C
			public override int Run(InterpretedFrame frame)
			{
				sbyte b = (sbyte)frame.Pop();
				frame.Push((sbyte)frame.Pop() > b);
				return 1;
			}
		}

		// Token: 0x020006C1 RID: 1729
		internal sealed class GreaterThanInt16 : GreaterThanInstruction
		{
			// Token: 0x060047CB RID: 18379 RVA: 0x0017CE44 File Offset: 0x0017B044
			public override int Run(InterpretedFrame frame)
			{
				short num = (short)frame.Pop();
				frame.Push((short)frame.Pop() > num);
				return 1;
			}
		}

		// Token: 0x020006C2 RID: 1730
		internal sealed class GreaterThanChar : GreaterThanInstruction
		{
			// Token: 0x060047CD RID: 18381 RVA: 0x0017CE7C File Offset: 0x0017B07C
			public override int Run(InterpretedFrame frame)
			{
				char c = (char)frame.Pop();
				frame.Push((char)frame.Pop() > c);
				return 1;
			}
		}

		// Token: 0x020006C3 RID: 1731
		internal sealed class GreaterThanInt32 : GreaterThanInstruction
		{
			// Token: 0x060047CF RID: 18383 RVA: 0x0017CEB4 File Offset: 0x0017B0B4
			public override int Run(InterpretedFrame frame)
			{
				int num = (int)frame.Pop();
				frame.Push((int)frame.Pop() > num);
				return 1;
			}
		}

		// Token: 0x020006C4 RID: 1732
		internal sealed class GreaterThanInt64 : GreaterThanInstruction
		{
			// Token: 0x060047D1 RID: 18385 RVA: 0x0017CEEC File Offset: 0x0017B0EC
			public override int Run(InterpretedFrame frame)
			{
				long num = (long)frame.Pop();
				frame.Push((long)frame.Pop() > num);
				return 1;
			}
		}

		// Token: 0x020006C5 RID: 1733
		internal sealed class GreaterThanByte : GreaterThanInstruction
		{
			// Token: 0x060047D3 RID: 18387 RVA: 0x0017CF24 File Offset: 0x0017B124
			public override int Run(InterpretedFrame frame)
			{
				byte b = (byte)frame.Pop();
				frame.Push((byte)frame.Pop() > b);
				return 1;
			}
		}

		// Token: 0x020006C6 RID: 1734
		internal sealed class GreaterThanUInt16 : GreaterThanInstruction
		{
			// Token: 0x060047D5 RID: 18389 RVA: 0x0017CF5C File Offset: 0x0017B15C
			public override int Run(InterpretedFrame frame)
			{
				ushort num = (ushort)frame.Pop();
				frame.Push((ushort)frame.Pop() > num);
				return 1;
			}
		}

		// Token: 0x020006C7 RID: 1735
		internal sealed class GreaterThanUInt32 : GreaterThanInstruction
		{
			// Token: 0x060047D7 RID: 18391 RVA: 0x0017CF94 File Offset: 0x0017B194
			public override int Run(InterpretedFrame frame)
			{
				uint num = (uint)frame.Pop();
				frame.Push((uint)frame.Pop() > num);
				return 1;
			}
		}

		// Token: 0x020006C8 RID: 1736
		internal sealed class GreaterThanUInt64 : GreaterThanInstruction
		{
			// Token: 0x060047D9 RID: 18393 RVA: 0x0017CFCC File Offset: 0x0017B1CC
			public override int Run(InterpretedFrame frame)
			{
				ulong num = (ulong)frame.Pop();
				frame.Push((ulong)frame.Pop() > num);
				return 1;
			}
		}

		// Token: 0x020006C9 RID: 1737
		internal sealed class GreaterThanSingle : GreaterThanInstruction
		{
			// Token: 0x060047DB RID: 18395 RVA: 0x0017D004 File Offset: 0x0017B204
			public override int Run(InterpretedFrame frame)
			{
				float num = (float)frame.Pop();
				frame.Push((float)frame.Pop() > num);
				return 1;
			}
		}

		// Token: 0x020006CA RID: 1738
		internal sealed class GreaterThanDouble : GreaterThanInstruction
		{
			// Token: 0x060047DD RID: 18397 RVA: 0x0017D03C File Offset: 0x0017B23C
			public override int Run(InterpretedFrame frame)
			{
				double num = (double)frame.Pop();
				frame.Push((double)frame.Pop() > num);
				return 1;
			}
		}
	}
}
