using System;
using System.Reflection;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006AD RID: 1709
	internal abstract class EqualInstruction : Instruction
	{
		// Token: 0x17000F43 RID: 3907
		// (get) Token: 0x06004796 RID: 18326 RVA: 0x0017C83A File Offset: 0x0017AA3A
		public override int ConsumedStack
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x06004797 RID: 18327 RVA: 0x0017C83D File Offset: 0x0017AA3D
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06004798 RID: 18328 RVA: 0x0017C840 File Offset: 0x0017AA40
		private EqualInstruction()
		{
		}

		// Token: 0x06004799 RID: 18329 RVA: 0x0017C848 File Offset: 0x0017AA48
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
					if ((result = EqualInstruction._Reference) == null)
					{
						result = (EqualInstruction._Reference = new EqualInstruction.EqualReference());
					}
					return result;
				}
				throw new NotImplementedException();
			case TypeCode.Boolean:
			{
				Instruction result2;
				if ((result2 = EqualInstruction._Boolean) == null)
				{
					result2 = (EqualInstruction._Boolean = new EqualInstruction.EqualBoolean());
				}
				return result2;
			}
			case TypeCode.Char:
			{
				Instruction result3;
				if ((result3 = EqualInstruction._Char) == null)
				{
					result3 = (EqualInstruction._Char = new EqualInstruction.EqualChar());
				}
				return result3;
			}
			case TypeCode.SByte:
			{
				Instruction result4;
				if ((result4 = EqualInstruction._SByte) == null)
				{
					result4 = (EqualInstruction._SByte = new EqualInstruction.EqualSByte());
				}
				return result4;
			}
			case TypeCode.Byte:
			{
				Instruction result5;
				if ((result5 = EqualInstruction._Byte) == null)
				{
					result5 = (EqualInstruction._Byte = new EqualInstruction.EqualByte());
				}
				return result5;
			}
			case TypeCode.Int16:
			{
				Instruction result6;
				if ((result6 = EqualInstruction._Int16) == null)
				{
					result6 = (EqualInstruction._Int16 = new EqualInstruction.EqualInt16());
				}
				return result6;
			}
			case TypeCode.UInt16:
			{
				Instruction result7;
				if ((result7 = EqualInstruction._UInt16) == null)
				{
					result7 = (EqualInstruction._UInt16 = new EqualInstruction.EqualInt16());
				}
				return result7;
			}
			case TypeCode.Int32:
			{
				Instruction result8;
				if ((result8 = EqualInstruction._Int32) == null)
				{
					result8 = (EqualInstruction._Int32 = new EqualInstruction.EqualInt32());
				}
				return result8;
			}
			case TypeCode.UInt32:
			{
				Instruction result9;
				if ((result9 = EqualInstruction._UInt32) == null)
				{
					result9 = (EqualInstruction._UInt32 = new EqualInstruction.EqualInt32());
				}
				return result9;
			}
			case TypeCode.Int64:
			{
				Instruction result10;
				if ((result10 = EqualInstruction._Int64) == null)
				{
					result10 = (EqualInstruction._Int64 = new EqualInstruction.EqualInt64());
				}
				return result10;
			}
			case TypeCode.UInt64:
			{
				Instruction result11;
				if ((result11 = EqualInstruction._UInt64) == null)
				{
					result11 = (EqualInstruction._UInt64 = new EqualInstruction.EqualInt64());
				}
				return result11;
			}
			case TypeCode.Single:
			{
				Instruction result12;
				if ((result12 = EqualInstruction._Single) == null)
				{
					result12 = (EqualInstruction._Single = new EqualInstruction.EqualSingle());
				}
				return result12;
			}
			case TypeCode.Double:
			{
				Instruction result13;
				if ((result13 = EqualInstruction._Double) == null)
				{
					result13 = (EqualInstruction._Double = new EqualInstruction.EqualDouble());
				}
				return result13;
			}
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600479A RID: 18330 RVA: 0x0017C9DE File Offset: 0x0017ABDE
		public override string ToString()
		{
			return "Equal()";
		}

		// Token: 0x04002304 RID: 8964
		private static Instruction _Reference;

		// Token: 0x04002305 RID: 8965
		private static Instruction _Boolean;

		// Token: 0x04002306 RID: 8966
		private static Instruction _SByte;

		// Token: 0x04002307 RID: 8967
		private static Instruction _Int16;

		// Token: 0x04002308 RID: 8968
		private static Instruction _Char;

		// Token: 0x04002309 RID: 8969
		private static Instruction _Int32;

		// Token: 0x0400230A RID: 8970
		private static Instruction _Int64;

		// Token: 0x0400230B RID: 8971
		private static Instruction _Byte;

		// Token: 0x0400230C RID: 8972
		private static Instruction _UInt16;

		// Token: 0x0400230D RID: 8973
		private static Instruction _UInt32;

		// Token: 0x0400230E RID: 8974
		private static Instruction _UInt64;

		// Token: 0x0400230F RID: 8975
		private static Instruction _Single;

		// Token: 0x04002310 RID: 8976
		private static Instruction _Double;

		// Token: 0x020006AE RID: 1710
		internal sealed class EqualBoolean : EqualInstruction
		{
			// Token: 0x0600479B RID: 18331 RVA: 0x0017C9E5 File Offset: 0x0017ABE5
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((bool)frame.Pop() == (bool)frame.Pop());
				return 1;
			}
		}

		// Token: 0x020006AF RID: 1711
		internal sealed class EqualSByte : EqualInstruction
		{
			// Token: 0x0600479D RID: 18333 RVA: 0x0017CA0E File Offset: 0x0017AC0E
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((sbyte)frame.Pop() == (sbyte)frame.Pop());
				return 1;
			}
		}

		// Token: 0x020006B0 RID: 1712
		internal sealed class EqualInt16 : EqualInstruction
		{
			// Token: 0x0600479F RID: 18335 RVA: 0x0017CA37 File Offset: 0x0017AC37
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((short)frame.Pop() == (short)frame.Pop());
				return 1;
			}
		}

		// Token: 0x020006B1 RID: 1713
		internal sealed class EqualChar : EqualInstruction
		{
			// Token: 0x060047A1 RID: 18337 RVA: 0x0017CA60 File Offset: 0x0017AC60
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((char)frame.Pop() == (char)frame.Pop());
				return 1;
			}
		}

		// Token: 0x020006B2 RID: 1714
		internal sealed class EqualInt32 : EqualInstruction
		{
			// Token: 0x060047A3 RID: 18339 RVA: 0x0017CA89 File Offset: 0x0017AC89
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((int)frame.Pop() == (int)frame.Pop());
				return 1;
			}
		}

		// Token: 0x020006B3 RID: 1715
		internal sealed class EqualInt64 : EqualInstruction
		{
			// Token: 0x060047A5 RID: 18341 RVA: 0x0017CAB2 File Offset: 0x0017ACB2
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((long)frame.Pop() == (long)frame.Pop());
				return 1;
			}
		}

		// Token: 0x020006B4 RID: 1716
		internal sealed class EqualByte : EqualInstruction
		{
			// Token: 0x060047A7 RID: 18343 RVA: 0x0017CADB File Offset: 0x0017ACDB
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((byte)frame.Pop() == (byte)frame.Pop());
				return 1;
			}
		}

		// Token: 0x020006B5 RID: 1717
		internal sealed class EqualUInt16 : EqualInstruction
		{
			// Token: 0x060047A9 RID: 18345 RVA: 0x0017CB04 File Offset: 0x0017AD04
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((ushort)frame.Pop() == (ushort)frame.Pop());
				return 1;
			}
		}

		// Token: 0x020006B6 RID: 1718
		internal sealed class EqualUInt32 : EqualInstruction
		{
			// Token: 0x060047AB RID: 18347 RVA: 0x0017CB2D File Offset: 0x0017AD2D
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((uint)frame.Pop() == (uint)frame.Pop());
				return 1;
			}
		}

		// Token: 0x020006B7 RID: 1719
		internal sealed class EqualUInt64 : EqualInstruction
		{
			// Token: 0x060047AD RID: 18349 RVA: 0x0017CB56 File Offset: 0x0017AD56
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((ulong)frame.Pop() == (ulong)frame.Pop());
				return 1;
			}
		}

		// Token: 0x020006B8 RID: 1720
		internal sealed class EqualSingle : EqualInstruction
		{
			// Token: 0x060047AF RID: 18351 RVA: 0x0017CB7F File Offset: 0x0017AD7F
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((float)frame.Pop() == (float)frame.Pop());
				return 1;
			}
		}

		// Token: 0x020006B9 RID: 1721
		internal sealed class EqualDouble : EqualInstruction
		{
			// Token: 0x060047B1 RID: 18353 RVA: 0x0017CBA8 File Offset: 0x0017ADA8
			public override int Run(InterpretedFrame frame)
			{
				frame.Push((double)frame.Pop() == (double)frame.Pop());
				return 1;
			}
		}

		// Token: 0x020006BA RID: 1722
		internal sealed class EqualReference : EqualInstruction
		{
			// Token: 0x060047B3 RID: 18355 RVA: 0x0017CBD1 File Offset: 0x0017ADD1
			public override int Run(InterpretedFrame frame)
			{
				frame.Push(frame.Pop() == frame.Pop());
				return 1;
			}
		}
	}
}
