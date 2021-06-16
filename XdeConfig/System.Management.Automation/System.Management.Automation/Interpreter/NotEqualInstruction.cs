using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200071D RID: 1821
	internal abstract class NotEqualInstruction : Instruction
	{
		// Token: 0x17000F97 RID: 3991
		// (get) Token: 0x06004A36 RID: 18998 RVA: 0x00186899 File Offset: 0x00184A99
		public override int ConsumedStack
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000F98 RID: 3992
		// (get) Token: 0x06004A37 RID: 18999 RVA: 0x0018689C File Offset: 0x00184A9C
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06004A38 RID: 19000 RVA: 0x0018689F File Offset: 0x00184A9F
		private NotEqualInstruction()
		{
		}

		// Token: 0x06004A39 RID: 19001 RVA: 0x001868A8 File Offset: 0x00184AA8
		public static Instruction Create(Type type)
		{
			TypeInfo typeInfo = type.GetTypeInfo();
			Type type2 = typeInfo.IsEnum ? Enum.GetUnderlyingType(type) : type;
			switch (type2.GetTypeCode())
			{
			case TypeCode.Object:
				if (!typeInfo.IsValueType)
				{
					Instruction result;
					if ((result = NotEqualInstruction._Reference) == null)
					{
						result = (NotEqualInstruction._Reference = new NotEqualInstruction.NotEqualReference());
					}
					return result;
				}
				throw new NotImplementedException();
			case TypeCode.Boolean:
			{
				Instruction result2;
				if ((result2 = NotEqualInstruction._Boolean) == null)
				{
					result2 = (NotEqualInstruction._Boolean = new NotEqualInstruction.NotEqualBoolean());
				}
				return result2;
			}
			case TypeCode.Char:
			{
				Instruction result3;
				if ((result3 = NotEqualInstruction._Char) == null)
				{
					result3 = (NotEqualInstruction._Char = new NotEqualInstruction.NotEqualChar());
				}
				return result3;
			}
			case TypeCode.SByte:
			{
				Instruction result4;
				if ((result4 = NotEqualInstruction._SByte) == null)
				{
					result4 = (NotEqualInstruction._SByte = new NotEqualInstruction.NotEqualSByte());
				}
				return result4;
			}
			case TypeCode.Byte:
			{
				Instruction result5;
				if ((result5 = NotEqualInstruction._Byte) == null)
				{
					result5 = (NotEqualInstruction._Byte = new NotEqualInstruction.NotEqualByte());
				}
				return result5;
			}
			case TypeCode.Int16:
			{
				Instruction result6;
				if ((result6 = NotEqualInstruction._Int16) == null)
				{
					result6 = (NotEqualInstruction._Int16 = new NotEqualInstruction.NotEqualInt16());
				}
				return result6;
			}
			case TypeCode.UInt16:
			{
				Instruction result7;
				if ((result7 = NotEqualInstruction._UInt16) == null)
				{
					result7 = (NotEqualInstruction._UInt16 = new NotEqualInstruction.NotEqualInt16());
				}
				return result7;
			}
			case TypeCode.Int32:
			{
				Instruction result8;
				if ((result8 = NotEqualInstruction._Int32) == null)
				{
					result8 = (NotEqualInstruction._Int32 = new NotEqualInstruction.NotEqualInt32());
				}
				return result8;
			}
			case TypeCode.UInt32:
			{
				Instruction result9;
				if ((result9 = NotEqualInstruction._UInt32) == null)
				{
					result9 = (NotEqualInstruction._UInt32 = new NotEqualInstruction.NotEqualInt32());
				}
				return result9;
			}
			case TypeCode.Int64:
			{
				Instruction result10;
				if ((result10 = NotEqualInstruction._Int64) == null)
				{
					result10 = (NotEqualInstruction._Int64 = new NotEqualInstruction.NotEqualInt64());
				}
				return result10;
			}
			case TypeCode.UInt64:
			{
				Instruction result11;
				if ((result11 = NotEqualInstruction._UInt64) == null)
				{
					result11 = (NotEqualInstruction._UInt64 = new NotEqualInstruction.NotEqualInt64());
				}
				return result11;
			}
			case TypeCode.Single:
			{
				Instruction result12;
				if ((result12 = NotEqualInstruction._Single) == null)
				{
					result12 = (NotEqualInstruction._Single = new NotEqualInstruction.NotEqualSingle());
				}
				return result12;
			}
			case TypeCode.Double:
			{
				Instruction result13;
				if ((result13 = NotEqualInstruction._Double) == null)
				{
					result13 = (NotEqualInstruction._Double = new NotEqualInstruction.NotEqualDouble());
				}
				return result13;
			}
			}
			throw new NotImplementedException();
		}

		// Token: 0x06004A3A RID: 19002 RVA: 0x00186A3E File Offset: 0x00184C3E
		public override string ToString()
		{
			return "NotEqual()";
		}

		// Token: 0x040023FD RID: 9213
		private static Instruction _Reference;

		// Token: 0x040023FE RID: 9214
		private static Instruction _Boolean;

		// Token: 0x040023FF RID: 9215
		private static Instruction _SByte;

		// Token: 0x04002400 RID: 9216
		private static Instruction _Int16;

		// Token: 0x04002401 RID: 9217
		private static Instruction _Char;

		// Token: 0x04002402 RID: 9218
		private static Instruction _Int32;

		// Token: 0x04002403 RID: 9219
		private static Instruction _Int64;

		// Token: 0x04002404 RID: 9220
		private static Instruction _Byte;

		// Token: 0x04002405 RID: 9221
		private static Instruction _UInt16;

		// Token: 0x04002406 RID: 9222
		private static Instruction _UInt32;

		// Token: 0x04002407 RID: 9223
		private static Instruction _UInt64;

		// Token: 0x04002408 RID: 9224
		private static Instruction _Single;

		// Token: 0x04002409 RID: 9225
		private static Instruction _Double;

		// Token: 0x0200071E RID: 1822
		internal sealed class NotEqualBoolean : NotEqualInstruction
		{
			// Token: 0x06004A3B RID: 19003 RVA: 0x00186A45 File Offset: 0x00184C45
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((bool)frame.Pop() != (bool)frame.Pop());
				return 1;
			}
		}

		// Token: 0x0200071F RID: 1823
		internal sealed class NotEqualSByte : NotEqualInstruction
		{
			// Token: 0x06004A3D RID: 19005 RVA: 0x00186A71 File Offset: 0x00184C71
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((sbyte)frame.Pop() != (sbyte)frame.Pop());
				return 1;
			}
		}

		// Token: 0x02000720 RID: 1824
		internal sealed class NotEqualInt16 : NotEqualInstruction
		{
			// Token: 0x06004A3F RID: 19007 RVA: 0x00186A9D File Offset: 0x00184C9D
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((short)frame.Pop() != (short)frame.Pop());
				return 1;
			}
		}

		// Token: 0x02000721 RID: 1825
		internal sealed class NotEqualChar : NotEqualInstruction
		{
			// Token: 0x06004A41 RID: 19009 RVA: 0x00186AC9 File Offset: 0x00184CC9
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((char)frame.Pop() != (char)frame.Pop());
				return 1;
			}
		}

		// Token: 0x02000722 RID: 1826
		internal sealed class NotEqualInt32 : NotEqualInstruction
		{
			// Token: 0x06004A43 RID: 19011 RVA: 0x00186AF5 File Offset: 0x00184CF5
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((int)frame.Pop() != (int)frame.Pop());
				return 1;
			}
		}

		// Token: 0x02000723 RID: 1827
		internal sealed class NotEqualInt64 : NotEqualInstruction
		{
			// Token: 0x06004A45 RID: 19013 RVA: 0x00186B21 File Offset: 0x00184D21
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((long)frame.Pop() != (long)frame.Pop());
				return 1;
			}
		}

		// Token: 0x02000724 RID: 1828
		internal sealed class NotEqualByte : NotEqualInstruction
		{
			// Token: 0x06004A47 RID: 19015 RVA: 0x00186B4D File Offset: 0x00184D4D
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((byte)frame.Pop() != (byte)frame.Pop());
				return 1;
			}
		}

		// Token: 0x02000725 RID: 1829
		internal sealed class NotEqualUInt16 : NotEqualInstruction
		{
			// Token: 0x06004A49 RID: 19017 RVA: 0x00186B79 File Offset: 0x00184D79
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((ushort)frame.Pop() != (ushort)frame.Pop());
				return 1;
			}
		}

		// Token: 0x02000726 RID: 1830
		internal sealed class NotEqualUInt32 : NotEqualInstruction
		{
			// Token: 0x06004A4B RID: 19019 RVA: 0x00186BA5 File Offset: 0x00184DA5
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((uint)frame.Pop() != (uint)frame.Pop());
				return 1;
			}
		}

		// Token: 0x02000727 RID: 1831
		internal sealed class NotEqualUInt64 : NotEqualInstruction
		{
			// Token: 0x06004A4D RID: 19021 RVA: 0x00186BD1 File Offset: 0x00184DD1
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((ulong)frame.Pop() != (ulong)frame.Pop());
				return 1;
			}
		}

		// Token: 0x02000728 RID: 1832
		internal sealed class NotEqualSingle : NotEqualInstruction
		{
			// Token: 0x06004A4F RID: 19023 RVA: 0x00186BFD File Offset: 0x00184DFD
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((float)frame.Pop() != (float)frame.Pop());
				return 1;
			}
		}

		// Token: 0x02000729 RID: 1833
		internal sealed class NotEqualDouble : NotEqualInstruction
		{
			// Token: 0x06004A51 RID: 19025 RVA: 0x00186C29 File Offset: 0x00184E29
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((double)frame.Pop() != (double)frame.Pop());
				return 1;
			}
		}

		// Token: 0x0200072A RID: 1834
		internal sealed class NotEqualReference : NotEqualInstruction
		{
			// Token: 0x06004A53 RID: 19027 RVA: 0x00186C55 File Offset: 0x00184E55
			public override int Run(InterpretedFrame frame)
			{
				frame.Push(frame.Pop() != frame.Pop());
				return 1;
			}
		}
	}
}
