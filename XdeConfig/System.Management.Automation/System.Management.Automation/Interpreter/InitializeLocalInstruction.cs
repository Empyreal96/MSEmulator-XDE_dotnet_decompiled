using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006FC RID: 1788
	internal abstract class InitializeLocalInstruction : LocalAccessInstruction
	{
		// Token: 0x060049C3 RID: 18883 RVA: 0x00185214 File Offset: 0x00183414
		internal InitializeLocalInstruction(int index) : base(index)
		{
		}

		// Token: 0x020006FD RID: 1789
		internal sealed class Reference : InitializeLocalInstruction, IBoxableInstruction
		{
			// Token: 0x060049C4 RID: 18884 RVA: 0x0018521D File Offset: 0x0018341D
			internal Reference(int index) : base(index)
			{
			}

			// Token: 0x060049C5 RID: 18885 RVA: 0x00185226 File Offset: 0x00183426
			public override int Run(InterpretedFrame frame)
			{
				frame.Data[this._index] = null;
				return 1;
			}

			// Token: 0x060049C6 RID: 18886 RVA: 0x00185237 File Offset: 0x00183437
			public Instruction BoxIfIndexMatches(int index)
			{
				if (index != this._index)
				{
					return null;
				}
				return InstructionList.InitImmutableRefBox(index);
			}

			// Token: 0x17000F84 RID: 3972
			// (get) Token: 0x060049C7 RID: 18887 RVA: 0x0018524A File Offset: 0x0018344A
			public override string InstructionName
			{
				get
				{
					return "InitRef";
				}
			}
		}

		// Token: 0x020006FE RID: 1790
		internal sealed class ImmutableValue : InitializeLocalInstruction, IBoxableInstruction
		{
			// Token: 0x060049C8 RID: 18888 RVA: 0x00185251 File Offset: 0x00183451
			internal ImmutableValue(int index, object defaultValue) : base(index)
			{
				this._defaultValue = defaultValue;
			}

			// Token: 0x060049C9 RID: 18889 RVA: 0x00185261 File Offset: 0x00183461
			public override int Run(InterpretedFrame frame)
			{
				frame.Data[this._index] = this._defaultValue;
				return 1;
			}

			// Token: 0x060049CA RID: 18890 RVA: 0x00185277 File Offset: 0x00183477
			public Instruction BoxIfIndexMatches(int index)
			{
				if (index != this._index)
				{
					return null;
				}
				return new InitializeLocalInstruction.ImmutableBox(index, this._defaultValue);
			}

			// Token: 0x17000F85 RID: 3973
			// (get) Token: 0x060049CB RID: 18891 RVA: 0x00185290 File Offset: 0x00183490
			public override string InstructionName
			{
				get
				{
					return "InitImmutableValue";
				}
			}

			// Token: 0x040023CA RID: 9162
			private readonly object _defaultValue;
		}

		// Token: 0x020006FF RID: 1791
		internal sealed class ImmutableBox : InitializeLocalInstruction
		{
			// Token: 0x060049CC RID: 18892 RVA: 0x00185297 File Offset: 0x00183497
			internal ImmutableBox(int index, object defaultValue) : base(index)
			{
				this._defaultValue = defaultValue;
			}

			// Token: 0x060049CD RID: 18893 RVA: 0x001852A7 File Offset: 0x001834A7
			public override int Run(InterpretedFrame frame)
			{
				frame.Data[this._index] = new StrongBox<object>(this._defaultValue);
				return 1;
			}

			// Token: 0x17000F86 RID: 3974
			// (get) Token: 0x060049CE RID: 18894 RVA: 0x001852C2 File Offset: 0x001834C2
			public override string InstructionName
			{
				get
				{
					return "InitImmutableBox";
				}
			}

			// Token: 0x040023CB RID: 9163
			private readonly object _defaultValue;
		}

		// Token: 0x02000700 RID: 1792
		internal sealed class ParameterBox : InitializeLocalInstruction
		{
			// Token: 0x060049CF RID: 18895 RVA: 0x001852C9 File Offset: 0x001834C9
			public ParameterBox(int index) : base(index)
			{
			}

			// Token: 0x060049D0 RID: 18896 RVA: 0x001852D2 File Offset: 0x001834D2
			public override int Run(InterpretedFrame frame)
			{
				frame.Data[this._index] = new StrongBox<object>(frame.Data[this._index]);
				return 1;
			}
		}

		// Token: 0x02000701 RID: 1793
		internal sealed class Parameter : InitializeLocalInstruction, IBoxableInstruction
		{
			// Token: 0x060049D1 RID: 18897 RVA: 0x001852F4 File Offset: 0x001834F4
			internal Parameter(int index) : base(index)
			{
			}

			// Token: 0x060049D2 RID: 18898 RVA: 0x001852FD File Offset: 0x001834FD
			public override int Run(InterpretedFrame frame)
			{
				return 1;
			}

			// Token: 0x060049D3 RID: 18899 RVA: 0x00185300 File Offset: 0x00183500
			public Instruction BoxIfIndexMatches(int index)
			{
				if (index == this._index)
				{
					return InstructionList.ParameterBox(index);
				}
				return null;
			}

			// Token: 0x17000F87 RID: 3975
			// (get) Token: 0x060049D4 RID: 18900 RVA: 0x00185313 File Offset: 0x00183513
			public override string InstructionName
			{
				get
				{
					return "InitParameter";
				}
			}
		}

		// Token: 0x02000702 RID: 1794
		internal sealed class MutableValue : InitializeLocalInstruction, IBoxableInstruction
		{
			// Token: 0x060049D5 RID: 18901 RVA: 0x0018531A File Offset: 0x0018351A
			internal MutableValue(int index, Type type) : base(index)
			{
				this._type = type;
			}

			// Token: 0x060049D6 RID: 18902 RVA: 0x0018532C File Offset: 0x0018352C
			public override int Run(InterpretedFrame frame)
			{
				try
				{
					frame.Data[this._index] = Activator.CreateInstance(this._type);
				}
				catch (TargetInvocationException ex)
				{
					ExceptionHelpers.UpdateForRethrow(ex.InnerException);
					throw ex.InnerException;
				}
				return 1;
			}

			// Token: 0x060049D7 RID: 18903 RVA: 0x00185378 File Offset: 0x00183578
			public Instruction BoxIfIndexMatches(int index)
			{
				if (index != this._index)
				{
					return null;
				}
				return new InitializeLocalInstruction.MutableBox(index, this._type);
			}

			// Token: 0x17000F88 RID: 3976
			// (get) Token: 0x060049D8 RID: 18904 RVA: 0x00185391 File Offset: 0x00183591
			public override string InstructionName
			{
				get
				{
					return "InitMutableValue";
				}
			}

			// Token: 0x040023CC RID: 9164
			private readonly Type _type;
		}

		// Token: 0x02000703 RID: 1795
		internal sealed class MutableBox : InitializeLocalInstruction
		{
			// Token: 0x060049D9 RID: 18905 RVA: 0x00185398 File Offset: 0x00183598
			internal MutableBox(int index, Type type) : base(index)
			{
				this._type = type;
			}

			// Token: 0x060049DA RID: 18906 RVA: 0x001853A8 File Offset: 0x001835A8
			public override int Run(InterpretedFrame frame)
			{
				frame.Data[this._index] = new StrongBox<object>(Activator.CreateInstance(this._type));
				return 1;
			}

			// Token: 0x17000F89 RID: 3977
			// (get) Token: 0x060049DB RID: 18907 RVA: 0x001853C8 File Offset: 0x001835C8
			public override string InstructionName
			{
				get
				{
					return "InitMutableBox";
				}
			}

			// Token: 0x040023CD RID: 9165
			private readonly Type _type;
		}
	}
}
