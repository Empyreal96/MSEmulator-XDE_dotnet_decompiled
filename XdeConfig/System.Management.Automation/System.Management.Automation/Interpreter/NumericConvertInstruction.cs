using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200072B RID: 1835
	internal abstract class NumericConvertInstruction : Instruction
	{
		// Token: 0x06004A55 RID: 19029 RVA: 0x00186C77 File Offset: 0x00184E77
		protected NumericConvertInstruction(TypeCode from, TypeCode to)
		{
			this._from = from;
			this._to = to;
		}

		// Token: 0x17000F99 RID: 3993
		// (get) Token: 0x06004A56 RID: 19030 RVA: 0x00186C8D File Offset: 0x00184E8D
		public override int ConsumedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F9A RID: 3994
		// (get) Token: 0x06004A57 RID: 19031 RVA: 0x00186C90 File Offset: 0x00184E90
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06004A58 RID: 19032 RVA: 0x00186C94 File Offset: 0x00184E94
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.InstructionName,
				"(",
				this._from,
				"->",
				this._to,
				")"
			});
		}

		// Token: 0x0400240A RID: 9226
		internal readonly TypeCode _from;

		// Token: 0x0400240B RID: 9227
		internal readonly TypeCode _to;

		// Token: 0x0200072C RID: 1836
		internal sealed class Unchecked : NumericConvertInstruction
		{
			// Token: 0x17000F9B RID: 3995
			// (get) Token: 0x06004A59 RID: 19033 RVA: 0x00186CEB File Offset: 0x00184EEB
			public override string InstructionName
			{
				get
				{
					return "UncheckedConvert";
				}
			}

			// Token: 0x06004A5A RID: 19034 RVA: 0x00186CF2 File Offset: 0x00184EF2
			public Unchecked(TypeCode from, TypeCode to) : base(from, to)
			{
			}

			// Token: 0x06004A5B RID: 19035 RVA: 0x00186CFC File Offset: 0x00184EFC
			public override int Run(InterpretedFrame frame)
			{
				frame.Push(this.Convert(frame.Pop()));
				return 1;
			}

			// Token: 0x06004A5C RID: 19036 RVA: 0x00186D14 File Offset: 0x00184F14
			private object Convert(object obj)
			{
				switch (this._from)
				{
				case TypeCode.Char:
					return this.ConvertInt32((int)((char)obj));
				case TypeCode.SByte:
					return this.ConvertInt32((int)((sbyte)obj));
				case TypeCode.Byte:
					return this.ConvertInt32((int)((byte)obj));
				case TypeCode.Int16:
					return this.ConvertInt32((int)((short)obj));
				case TypeCode.UInt16:
					return this.ConvertInt32((int)((ushort)obj));
				case TypeCode.Int32:
					return this.ConvertInt32((int)obj);
				case TypeCode.UInt32:
					return this.ConvertInt64((long)((ulong)((uint)obj)));
				case TypeCode.Int64:
					return this.ConvertInt64((long)obj);
				case TypeCode.UInt64:
					return this.ConvertUInt64((ulong)obj);
				case TypeCode.Single:
					return this.ConvertDouble((double)((float)obj));
				case TypeCode.Double:
					return this.ConvertDouble((double)obj);
				default:
					throw Assert.Unreachable;
				}
			}

			// Token: 0x06004A5D RID: 19037 RVA: 0x00186DF8 File Offset: 0x00184FF8
			private object ConvertInt32(int obj)
			{
				switch (this._to)
				{
				case TypeCode.Char:
					return (char)obj;
				case TypeCode.SByte:
					return (sbyte)obj;
				case TypeCode.Byte:
					return (byte)obj;
				case TypeCode.Int16:
					return (short)obj;
				case TypeCode.UInt16:
					return (ushort)obj;
				case TypeCode.Int32:
					return obj;
				case TypeCode.UInt32:
					return (uint)obj;
				case TypeCode.Int64:
					return (long)obj;
				case TypeCode.UInt64:
					return (ulong)((long)obj);
				case TypeCode.Single:
					return (float)obj;
				case TypeCode.Double:
					return (double)obj;
				default:
					throw Assert.Unreachable;
				}
			}

			// Token: 0x06004A5E RID: 19038 RVA: 0x00186EA0 File Offset: 0x001850A0
			private object ConvertInt64(long obj)
			{
				switch (this._to)
				{
				case TypeCode.Char:
					return (char)obj;
				case TypeCode.SByte:
					return (sbyte)obj;
				case TypeCode.Byte:
					return (byte)obj;
				case TypeCode.Int16:
					return (short)obj;
				case TypeCode.UInt16:
					return (ushort)obj;
				case TypeCode.Int32:
					return (int)obj;
				case TypeCode.UInt32:
					return (uint)obj;
				case TypeCode.Int64:
					return obj;
				case TypeCode.UInt64:
					return (ulong)obj;
				case TypeCode.Single:
					return (float)obj;
				case TypeCode.Double:
					return (double)obj;
				default:
					throw Assert.Unreachable;
				}
			}

			// Token: 0x06004A5F RID: 19039 RVA: 0x00186F48 File Offset: 0x00185148
			private object ConvertUInt64(ulong obj)
			{
				switch (this._to)
				{
				case TypeCode.Char:
					return (char)obj;
				case TypeCode.SByte:
					return (sbyte)obj;
				case TypeCode.Byte:
					return (byte)obj;
				case TypeCode.Int16:
					return (short)obj;
				case TypeCode.UInt16:
					return (ushort)obj;
				case TypeCode.Int32:
					return (int)obj;
				case TypeCode.UInt32:
					return (uint)obj;
				case TypeCode.Int64:
					return (long)obj;
				case TypeCode.UInt64:
					return obj;
				case TypeCode.Single:
					return obj;
				case TypeCode.Double:
					return obj;
				default:
					throw Assert.Unreachable;
				}
			}

			// Token: 0x06004A60 RID: 19040 RVA: 0x00186FF0 File Offset: 0x001851F0
			private object ConvertDouble(double obj)
			{
				switch (this._to)
				{
				case TypeCode.Char:
					return (char)obj;
				case TypeCode.SByte:
					return (sbyte)obj;
				case TypeCode.Byte:
					return (byte)obj;
				case TypeCode.Int16:
					return (short)obj;
				case TypeCode.UInt16:
					return (ushort)obj;
				case TypeCode.Int32:
					return (int)obj;
				case TypeCode.UInt32:
					return (uint)obj;
				case TypeCode.Int64:
					return (long)obj;
				case TypeCode.UInt64:
					return (ulong)obj;
				case TypeCode.Single:
					return (float)obj;
				case TypeCode.Double:
					return obj;
				default:
					throw Assert.Unreachable;
				}
			}
		}

		// Token: 0x0200072D RID: 1837
		internal sealed class Checked : NumericConvertInstruction
		{
			// Token: 0x17000F9C RID: 3996
			// (get) Token: 0x06004A61 RID: 19041 RVA: 0x00187097 File Offset: 0x00185297
			public override string InstructionName
			{
				get
				{
					return "CheckedConvert";
				}
			}

			// Token: 0x06004A62 RID: 19042 RVA: 0x0018709E File Offset: 0x0018529E
			public Checked(TypeCode from, TypeCode to) : base(from, to)
			{
			}

			// Token: 0x06004A63 RID: 19043 RVA: 0x001870A8 File Offset: 0x001852A8
			public override int Run(InterpretedFrame frame)
			{
				frame.Push(this.Convert(frame.Pop()));
				return 1;
			}

			// Token: 0x06004A64 RID: 19044 RVA: 0x001870C0 File Offset: 0x001852C0
			private object Convert(object obj)
			{
				switch (this._from)
				{
				case TypeCode.Char:
					return this.ConvertInt32((int)((char)obj));
				case TypeCode.SByte:
					return this.ConvertInt32((int)((sbyte)obj));
				case TypeCode.Byte:
					return this.ConvertInt32((int)((byte)obj));
				case TypeCode.Int16:
					return this.ConvertInt32((int)((short)obj));
				case TypeCode.UInt16:
					return this.ConvertInt32((int)((ushort)obj));
				case TypeCode.Int32:
					return this.ConvertInt32((int)obj);
				case TypeCode.UInt32:
					return this.ConvertInt64((long)((ulong)((uint)obj)));
				case TypeCode.Int64:
					return this.ConvertInt64((long)obj);
				case TypeCode.UInt64:
					return this.ConvertUInt64((ulong)obj);
				case TypeCode.Single:
					return this.ConvertDouble((double)((float)obj));
				case TypeCode.Double:
					return this.ConvertDouble((double)obj);
				default:
					throw Assert.Unreachable;
				}
			}

			// Token: 0x06004A65 RID: 19045 RVA: 0x001871A4 File Offset: 0x001853A4
			private object ConvertInt32(int obj)
			{
				checked
				{
					switch (this._to)
					{
					case TypeCode.Char:
						return (char)obj;
					case TypeCode.SByte:
						return (sbyte)obj;
					case TypeCode.Byte:
						return (byte)obj;
					case TypeCode.Int16:
						return (short)obj;
					case TypeCode.UInt16:
						return (ushort)obj;
					case TypeCode.Int32:
						return obj;
					case TypeCode.UInt32:
						return (uint)obj;
					case TypeCode.Int64:
						return unchecked((long)obj);
					case TypeCode.UInt64:
						return (ulong)obj;
					case TypeCode.Single:
						return (float)obj;
					case TypeCode.Double:
						return (double)obj;
					default:
						throw Assert.Unreachable;
					}
				}
			}

			// Token: 0x06004A66 RID: 19046 RVA: 0x0018724C File Offset: 0x0018544C
			private object ConvertInt64(long obj)
			{
				checked
				{
					switch (this._to)
					{
					case TypeCode.Char:
						return (char)obj;
					case TypeCode.SByte:
						return (sbyte)obj;
					case TypeCode.Byte:
						return (byte)obj;
					case TypeCode.Int16:
						return (short)obj;
					case TypeCode.UInt16:
						return (ushort)obj;
					case TypeCode.Int32:
						return (int)obj;
					case TypeCode.UInt32:
						return (uint)obj;
					case TypeCode.Int64:
						return obj;
					case TypeCode.UInt64:
						return (ulong)obj;
					case TypeCode.Single:
						return (float)obj;
					case TypeCode.Double:
						return (double)obj;
					default:
						throw Assert.Unreachable;
					}
				}
			}

			// Token: 0x06004A67 RID: 19047 RVA: 0x001872F4 File Offset: 0x001854F4
			private object ConvertUInt64(ulong obj)
			{
				checked
				{
					switch (this._to)
					{
					case TypeCode.Char:
						return (char)obj;
					case TypeCode.SByte:
						return (sbyte)obj;
					case TypeCode.Byte:
						return (byte)obj;
					case TypeCode.Int16:
						return (short)obj;
					case TypeCode.UInt16:
						return (ushort)obj;
					case TypeCode.Int32:
						return (int)obj;
					case TypeCode.UInt32:
						return (uint)obj;
					case TypeCode.Int64:
						return (long)obj;
					case TypeCode.UInt64:
						return obj;
					case TypeCode.Single:
						return obj;
					case TypeCode.Double:
						return obj;
					default:
						throw Assert.Unreachable;
					}
				}
			}

			// Token: 0x06004A68 RID: 19048 RVA: 0x0018739C File Offset: 0x0018559C
			private object ConvertDouble(double obj)
			{
				checked
				{
					switch (this._to)
					{
					case TypeCode.Char:
						return (char)obj;
					case TypeCode.SByte:
						return (sbyte)obj;
					case TypeCode.Byte:
						return (byte)obj;
					case TypeCode.Int16:
						return (short)obj;
					case TypeCode.UInt16:
						return (ushort)obj;
					case TypeCode.Int32:
						return (int)obj;
					case TypeCode.UInt32:
						return (uint)obj;
					case TypeCode.Int64:
						return (long)obj;
					case TypeCode.UInt64:
						return (ulong)obj;
					case TypeCode.Single:
						return (float)obj;
					case TypeCode.Double:
						return obj;
					default:
						throw Assert.Unreachable;
					}
				}
			}
		}
	}
}
