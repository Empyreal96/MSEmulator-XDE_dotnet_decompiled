using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200065B RID: 1627
	internal abstract class AddOvfInstruction : Instruction
	{
		// Token: 0x17000EBC RID: 3772
		// (get) Token: 0x060045CD RID: 17869 RVA: 0x00176C6D File Offset: 0x00174E6D
		public override int ConsumedStack
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000EBD RID: 3773
		// (get) Token: 0x060045CE RID: 17870 RVA: 0x00176C70 File Offset: 0x00174E70
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060045CF RID: 17871 RVA: 0x00176C73 File Offset: 0x00174E73
		private AddOvfInstruction()
		{
		}

		// Token: 0x060045D0 RID: 17872 RVA: 0x00176C7C File Offset: 0x00174E7C
		public static Instruction Create(Type type)
		{
			switch (type.GetTypeCode())
			{
			case TypeCode.Int16:
			{
				Instruction result;
				if ((result = AddOvfInstruction._Int16) == null)
				{
					result = (AddOvfInstruction._Int16 = new AddOvfInstruction.AddOvfInt16());
				}
				return result;
			}
			case TypeCode.UInt16:
			{
				Instruction result2;
				if ((result2 = AddOvfInstruction._UInt16) == null)
				{
					result2 = (AddOvfInstruction._UInt16 = new AddOvfInstruction.AddOvfUInt16());
				}
				return result2;
			}
			case TypeCode.Int32:
			{
				Instruction result3;
				if ((result3 = AddOvfInstruction._Int32) == null)
				{
					result3 = (AddOvfInstruction._Int32 = new AddOvfInstruction.AddOvfInt32());
				}
				return result3;
			}
			case TypeCode.UInt32:
			{
				Instruction result4;
				if ((result4 = AddOvfInstruction._UInt32) == null)
				{
					result4 = (AddOvfInstruction._UInt32 = new AddOvfInstruction.AddOvfUInt32());
				}
				return result4;
			}
			case TypeCode.Int64:
			{
				Instruction result5;
				if ((result5 = AddOvfInstruction._Int64) == null)
				{
					result5 = (AddOvfInstruction._Int64 = new AddOvfInstruction.AddOvfInt64());
				}
				return result5;
			}
			case TypeCode.UInt64:
			{
				Instruction result6;
				if ((result6 = AddOvfInstruction._UInt64) == null)
				{
					result6 = (AddOvfInstruction._UInt64 = new AddOvfInstruction.AddOvfUInt64());
				}
				return result6;
			}
			case TypeCode.Single:
			{
				Instruction result7;
				if ((result7 = AddOvfInstruction._Single) == null)
				{
					result7 = (AddOvfInstruction._Single = new AddOvfInstruction.AddOvfSingle());
				}
				return result7;
			}
			case TypeCode.Double:
			{
				Instruction result8;
				if ((result8 = AddOvfInstruction._Double) == null)
				{
					result8 = (AddOvfInstruction._Double = new AddOvfInstruction.AddOvfDouble());
				}
				return result8;
			}
			default:
				throw Assert.Unreachable;
			}
		}

		// Token: 0x060045D1 RID: 17873 RVA: 0x00176D6A File Offset: 0x00174F6A
		public override string ToString()
		{
			return "AddOvf()";
		}

		// Token: 0x0400228E RID: 8846
		private static Instruction _Int16;

		// Token: 0x0400228F RID: 8847
		private static Instruction _Int32;

		// Token: 0x04002290 RID: 8848
		private static Instruction _Int64;

		// Token: 0x04002291 RID: 8849
		private static Instruction _UInt16;

		// Token: 0x04002292 RID: 8850
		private static Instruction _UInt32;

		// Token: 0x04002293 RID: 8851
		private static Instruction _UInt64;

		// Token: 0x04002294 RID: 8852
		private static Instruction _Single;

		// Token: 0x04002295 RID: 8853
		private static Instruction _Double;

		// Token: 0x0200065C RID: 1628
		internal sealed class AddOvfInt32 : AddOvfInstruction
		{
			// Token: 0x060045D2 RID: 17874 RVA: 0x00176D74 File Offset: 0x00174F74
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = ScriptingRuntimeHelpers.Int32ToObject(checked((int)obj + (int)obj2));
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x0200065D RID: 1629
		internal sealed class AddOvfInt16 : AddOvfInstruction
		{
			// Token: 0x060045D4 RID: 17876 RVA: 0x00176DDC File Offset: 0x00174FDC
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = checked((short)obj + (short)obj2);
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x0200065E RID: 1630
		internal sealed class AddOvfInt64 : AddOvfInstruction
		{
			// Token: 0x060045D6 RID: 17878 RVA: 0x00176E44 File Offset: 0x00175044
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = checked((long)obj + (long)obj2);
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x0200065F RID: 1631
		internal sealed class AddOvfUInt16 : AddOvfInstruction
		{
			// Token: 0x060045D8 RID: 17880 RVA: 0x00176EAC File Offset: 0x001750AC
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = checked((ushort)obj + (ushort)obj2);
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000660 RID: 1632
		internal sealed class AddOvfUInt32 : AddOvfInstruction
		{
			// Token: 0x060045DA RID: 17882 RVA: 0x00176F14 File Offset: 0x00175114
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = checked((uint)obj + (uint)obj2);
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000661 RID: 1633
		internal sealed class AddOvfUInt64 : AddOvfInstruction
		{
			// Token: 0x060045DC RID: 17884 RVA: 0x00176F7C File Offset: 0x0017517C
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (ulong)((long)(checked((short)obj + (short)obj2)));
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000662 RID: 1634
		internal sealed class AddOvfSingle : AddOvfInstruction
		{
			// Token: 0x060045DE RID: 17886 RVA: 0x00176FE4 File Offset: 0x001751E4
			public override int Run(InterpretedFrame frame)
			{
				object obj = frame.Data[frame.StackIndex - 2];
				object obj2 = frame.Data[frame.StackIndex - 1];
				frame.Data[frame.StackIndex - 2] = (float)obj + (float)obj2;
				frame.StackIndex--;
				return 1;
			}
		}

		// Token: 0x02000663 RID: 1635
		internal sealed class AddOvfDouble : AddOvfInstruction
		{
			// Token: 0x060045E0 RID: 17888 RVA: 0x0017704C File Offset: 0x0017524C
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
