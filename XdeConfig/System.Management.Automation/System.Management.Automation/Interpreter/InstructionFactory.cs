using System;
using System.Collections.Generic;
using System.Numerics;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006CD RID: 1741
	internal abstract class InstructionFactory
	{
		// Token: 0x060047E5 RID: 18405 RVA: 0x0017D0B0 File Offset: 0x0017B2B0
		internal static InstructionFactory GetFactory(Type type)
		{
			if (InstructionFactory._factories == null)
			{
				InstructionFactory._factories = new Dictionary<Type, InstructionFactory>
				{
					{
						typeof(object),
						InstructionFactory<object>.Factory
					},
					{
						typeof(bool),
						InstructionFactory<bool>.Factory
					},
					{
						typeof(byte),
						InstructionFactory<byte>.Factory
					},
					{
						typeof(sbyte),
						InstructionFactory<sbyte>.Factory
					},
					{
						typeof(short),
						InstructionFactory<short>.Factory
					},
					{
						typeof(ushort),
						InstructionFactory<ushort>.Factory
					},
					{
						typeof(int),
						InstructionFactory<int>.Factory
					},
					{
						typeof(uint),
						InstructionFactory<uint>.Factory
					},
					{
						typeof(long),
						InstructionFactory<long>.Factory
					},
					{
						typeof(ulong),
						InstructionFactory<ulong>.Factory
					},
					{
						typeof(float),
						InstructionFactory<float>.Factory
					},
					{
						typeof(double),
						InstructionFactory<double>.Factory
					},
					{
						typeof(char),
						InstructionFactory<char>.Factory
					},
					{
						typeof(string),
						InstructionFactory<string>.Factory
					},
					{
						typeof(BigInteger),
						InstructionFactory<BigInteger>.Factory
					}
				};
			}
			InstructionFactory result;
			lock (InstructionFactory._factories)
			{
				InstructionFactory instructionFactory;
				if (!InstructionFactory._factories.TryGetValue(type, out instructionFactory))
				{
					instructionFactory = (InstructionFactory)typeof(InstructionFactory<>).MakeGenericType(new Type[]
					{
						type
					}).GetField("Factory").GetValue(null);
					InstructionFactory._factories[type] = instructionFactory;
				}
				result = instructionFactory;
			}
			return result;
		}

		// Token: 0x060047E6 RID: 18406
		protected internal abstract Instruction GetArrayItem();

		// Token: 0x060047E7 RID: 18407
		protected internal abstract Instruction SetArrayItem();

		// Token: 0x060047E8 RID: 18408
		protected internal abstract Instruction TypeIs();

		// Token: 0x060047E9 RID: 18409
		protected internal abstract Instruction TypeAs();

		// Token: 0x060047EA RID: 18410
		protected internal abstract Instruction DefaultValue();

		// Token: 0x060047EB RID: 18411
		protected internal abstract Instruction NewArray();

		// Token: 0x060047EC RID: 18412
		protected internal abstract Instruction NewArrayInit(int elementCount);

		// Token: 0x04002321 RID: 8993
		private static Dictionary<Type, InstructionFactory> _factories;
	}
}
