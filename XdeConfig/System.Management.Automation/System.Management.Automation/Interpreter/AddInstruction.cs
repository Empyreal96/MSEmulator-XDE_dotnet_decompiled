using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000652 RID: 1618
	internal abstract class AddInstruction : Instruction
	{
		// Token: 0x17000EBA RID: 3770
		// (get) Token: 0x060045B8 RID: 17848 RVA: 0x00176827 File Offset: 0x00174A27
		public override int ConsumedStack
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000EBB RID: 3771
		// (get) Token: 0x060045B9 RID: 17849 RVA: 0x0017682A File Offset: 0x00174A2A
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060045BA RID: 17850 RVA: 0x0017682D File Offset: 0x00174A2D
		private AddInstruction()
		{
		}

		// Token: 0x060045BB RID: 17851 RVA: 0x00176838 File Offset: 0x00174A38
		public static Instruction Create(Type type)
		{
			switch (type.GetTypeCode())
			{
			case TypeCode.Int16:
			{
				Instruction result;
				if ((result = AddInstruction._Int16) == null)
				{
					result = (AddInstruction._Int16 = new AddInstruction.AddInt16());
				}
				return result;
			}
			case TypeCode.UInt16:
			{
				Instruction result2;
				if ((result2 = AddInstruction._UInt16) == null)
				{
					result2 = (AddInstruction._UInt16 = new AddInstruction.AddUInt16());
				}
				return result2;
			}
			case TypeCode.Int32:
			{
				Instruction result3;
				if ((result3 = AddInstruction._Int32) == null)
				{
					result3 = (AddInstruction._Int32 = new AddInstruction.AddInt32());
				}
				return result3;
			}
			case TypeCode.UInt32:
			{
				Instruction result4;
				if ((result4 = AddInstruction._UInt32) == null)
				{
					result4 = (AddInstruction._UInt32 = new AddInstruction.AddUInt32());
				}
				return result4;
			}
			case TypeCode.Int64:
			{
				Instruction result5;
				if ((result5 = AddInstruction._Int64) == null)
				{
					result5 = (AddInstruction._Int64 = new AddInstruction.AddInt64());
				}
				return result5;
			}
			case TypeCode.UInt64:
			{
				Instruction result6;
				if ((result6 = AddInstruction._UInt64) == null)
				{
					result6 = (AddInstruction._UInt64 = new AddInstruction.AddUInt64());
				}
				return result6;
			}
			case TypeCode.Single:
			{
				Instruction result7;
				if ((result7 = AddInstruction._Single) == null)
				{
					result7 = (AddInstruction._Single = new AddInstruction.AddSingle());
				}
				return result7;
			}
			case TypeCode.Double:
			{
				Instruction result8;
				if ((result8 = AddInstruction._Double) == null)
				{
					result8 = (AddInstruction._Double = new AddInstruction.AddDouble());
				}
				return result8;
			}
			default:
				throw Assert.Unreachable;
			}
		}

		// Token: 0x060045BC RID: 17852 RVA: 0x00176926 File Offset: 0x00174B26
		public override string ToString()
		{
			return "Add()";
		}

		// Token: 0x04002286 RID: 8838
		private static Instruction _Int16;

		// Token: 0x04002287 RID: 8839
		private static Instruction _Int32;

		// Token: 0x04002288 RID: 8840
		private static Instruction _Int64;

		// Token: 0x04002289 RID: 8841
		private static Instruction _UInt16;

		// Token: 0x0400228A RID: 8842
		private static Instruction _UInt32;

		// Token: 0x0400228B RID: 8843
		private static Instruction _UInt64;

		// Token: 0x0400228C RID: 8844
		private static Instruction _Single;

		// Token: 0x0400228D RID: 8845
		private static Instruction _Double;

		// Token: 0x02000653 RID: 1619
		internal sealed class AddInt32 : AddInstruction
		{
			// Token: 0x060045BD RID: 17853 RVA: 0x00176930 File Offset: 0x00174B30
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = ScriptingRuntimeHelpers.Int32ToObject((int)obj + (int)obj2);
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000654 RID: 1620
		internal sealed class AddInt16 : AddInstruction
		{
			// Token: 0x060045BF RID: 17855 RVA: 0x00176998 File Offset: 0x00174B98
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (short)obj + (short)obj2;
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000655 RID: 1621
		internal sealed class AddInt64 : AddInstruction
		{
			// Token: 0x060045C1 RID: 17857 RVA: 0x00176A00 File Offset: 0x00174C00
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (long)obj + (long)obj2;
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000656 RID: 1622
		internal sealed class AddUInt16 : AddInstruction
		{
			// Token: 0x060045C3 RID: 17859 RVA: 0x00176A68 File Offset: 0x00174C68
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (ushort)obj + (ushort)obj2;
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000657 RID: 1623
		internal sealed class AddUInt32 : AddInstruction
		{
			// Token: 0x060045C5 RID: 17861 RVA: 0x00176AD0 File Offset: 0x00174CD0
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (uint)obj + (uint)obj2;
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000658 RID: 1624
		internal sealed class AddUInt64 : AddInstruction
		{
			// Token: 0x060045C7 RID: 17863 RVA: 0x00176B38 File Offset: 0x00174D38
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (ulong)((long)((short)obj + (short)obj2));
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000659 RID: 1625
		internal sealed class AddSingle : AddInstruction
		{
			// Token: 0x060045C9 RID: 17865 RVA: 0x00176BA0 File Offset: 0x00174DA0
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (float)obj + (float)obj2;
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x0200065A RID: 1626
		internal sealed class AddDouble : AddInstruction
		{
			// Token: 0x060045CB RID: 17867 RVA: 0x00176C08 File Offset: 0x00174E08
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (double)obj + (double)obj2;
				frame.StackIndex--;
				return 1;
			}
		}
	}
}
