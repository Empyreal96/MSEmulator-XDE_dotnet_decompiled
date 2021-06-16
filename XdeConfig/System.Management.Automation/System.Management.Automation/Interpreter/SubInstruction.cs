using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000734 RID: 1844
	internal abstract class SubInstruction : Instruction
	{
		// Token: 0x17000FA4 RID: 4004
		// (get) Token: 0x06004A85 RID: 19077 RVA: 0x0018766B File Offset: 0x0018586B
		public override int ConsumedStack
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000FA5 RID: 4005
		// (get) Token: 0x06004A86 RID: 19078 RVA: 0x0018766E File Offset: 0x0018586E
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06004A87 RID: 19079 RVA: 0x00187671 File Offset: 0x00185871
		private SubInstruction()
		{
		}

		// Token: 0x06004A88 RID: 19080 RVA: 0x0018767C File Offset: 0x0018587C
		public static Instruction Create(Type type)
		{
			switch (type.GetTypeCode())
			{
			case TypeCode.Int16:
			{
				Instruction result;
				if ((result = SubInstruction._Int16) == null)
				{
					result = (SubInstruction._Int16 = new SubInstruction.SubInt16());
				}
				return result;
			}
			case TypeCode.UInt16:
			{
				Instruction result2;
				if ((result2 = SubInstruction._UInt16) == null)
				{
					result2 = (SubInstruction._UInt16 = new SubInstruction.SubUInt16());
				}
				return result2;
			}
			case TypeCode.Int32:
			{
				Instruction result3;
				if ((result3 = SubInstruction._Int32) == null)
				{
					result3 = (SubInstruction._Int32 = new SubInstruction.SubInt32());
				}
				return result3;
			}
			case TypeCode.UInt32:
			{
				Instruction result4;
				if ((result4 = SubInstruction._UInt32) == null)
				{
					result4 = (SubInstruction._UInt32 = new SubInstruction.SubUInt32());
				}
				return result4;
			}
			case TypeCode.Int64:
			{
				Instruction result5;
				if ((result5 = SubInstruction._Int64) == null)
				{
					result5 = (SubInstruction._Int64 = new SubInstruction.SubInt64());
				}
				return result5;
			}
			case TypeCode.UInt64:
			{
				Instruction result6;
				if ((result6 = SubInstruction._UInt64) == null)
				{
					result6 = (SubInstruction._UInt64 = new SubInstruction.SubUInt64());
				}
				return result6;
			}
			case TypeCode.Single:
			{
				Instruction result7;
				if ((result7 = SubInstruction._Single) == null)
				{
					result7 = (SubInstruction._Single = new SubInstruction.SubSingle());
				}
				return result7;
			}
			case TypeCode.Double:
			{
				Instruction result8;
				if ((result8 = SubInstruction._Double) == null)
				{
					result8 = (SubInstruction._Double = new SubInstruction.SubDouble());
				}
				return result8;
			}
			default:
				throw Assert.Unreachable;
			}
		}

		// Token: 0x06004A89 RID: 19081 RVA: 0x0018776A File Offset: 0x0018596A
		public override string ToString()
		{
			return "Sub()";
		}

		// Token: 0x04002413 RID: 9235
		private static Instruction _Int16;

		// Token: 0x04002414 RID: 9236
		private static Instruction _Int32;

		// Token: 0x04002415 RID: 9237
		private static Instruction _Int64;

		// Token: 0x04002416 RID: 9238
		private static Instruction _UInt16;

		// Token: 0x04002417 RID: 9239
		private static Instruction _UInt32;

		// Token: 0x04002418 RID: 9240
		private static Instruction _UInt64;

		// Token: 0x04002419 RID: 9241
		private static Instruction _Single;

		// Token: 0x0400241A RID: 9242
		private static Instruction _Double;

		// Token: 0x02000735 RID: 1845
		internal sealed class SubInt32 : SubInstruction
		{
			// Token: 0x06004A8A RID: 19082 RVA: 0x00187774 File Offset: 0x00185974
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = ScriptingRuntimeHelpers.Int32ToObject((int)obj - (int)obj2);
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000736 RID: 1846
		internal sealed class SubInt16 : SubInstruction
		{
			// Token: 0x06004A8C RID: 19084 RVA: 0x001877DC File Offset: 0x001859DC
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (short)obj - (short)obj2;
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000737 RID: 1847
		internal sealed class SubInt64 : SubInstruction
		{
			// Token: 0x06004A8E RID: 19086 RVA: 0x00187844 File Offset: 0x00185A44
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (long)obj - (long)obj2;
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000738 RID: 1848
		internal sealed class SubUInt16 : SubInstruction
		{
			// Token: 0x06004A90 RID: 19088 RVA: 0x001878AC File Offset: 0x00185AAC
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (ushort)obj - (ushort)obj2;
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000739 RID: 1849
		internal sealed class SubUInt32 : SubInstruction
		{
			// Token: 0x06004A92 RID: 19090 RVA: 0x00187914 File Offset: 0x00185B14
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (uint)obj - (uint)obj2;
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x0200073A RID: 1850
		internal sealed class SubUInt64 : SubInstruction
		{
			// Token: 0x06004A94 RID: 19092 RVA: 0x0018797C File Offset: 0x00185B7C
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (ulong)((long)((short)obj - (short)obj2));
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x0200073B RID: 1851
		internal sealed class SubSingle : SubInstruction
		{
			// Token: 0x06004A96 RID: 19094 RVA: 0x001879E4 File Offset: 0x00185BE4
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (float)obj - (float)obj2;
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x0200073C RID: 1852
		internal sealed class SubDouble : SubInstruction
		{
			// Token: 0x06004A98 RID: 19096 RVA: 0x00187A4C File Offset: 0x00185C4C
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
