using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006CE RID: 1742
	internal sealed class InstructionFactory<T> : InstructionFactory
	{
		// Token: 0x060047EE RID: 18414 RVA: 0x0017D298 File Offset: 0x0017B498
		private InstructionFactory()
		{
		}

		// Token: 0x060047EF RID: 18415 RVA: 0x0017D2A0 File Offset: 0x0017B4A0
		protected internal override Instruction GetArrayItem()
		{
			Instruction result;
			if ((result = this._getArrayItem) == null)
			{
				result = (this._getArrayItem = new GetArrayItemInstruction<T>());
			}
			return result;
		}

		// Token: 0x060047F0 RID: 18416 RVA: 0x0017D2C8 File Offset: 0x0017B4C8
		protected internal override Instruction SetArrayItem()
		{
			Instruction result;
			if ((result = this._setArrayItem) == null)
			{
				result = (this._setArrayItem = new SetArrayItemInstruction<T>());
			}
			return result;
		}

		// Token: 0x060047F1 RID: 18417 RVA: 0x0017D2F0 File Offset: 0x0017B4F0
		protected internal override Instruction TypeIs()
		{
			Instruction result;
			if ((result = this._typeIs) == null)
			{
				result = (this._typeIs = new TypeIsInstruction<T>());
			}
			return result;
		}

		// Token: 0x060047F2 RID: 18418 RVA: 0x0017D318 File Offset: 0x0017B518
		protected internal override Instruction TypeAs()
		{
			Instruction result;
			if ((result = this._typeAs) == null)
			{
				result = (this._typeAs = new TypeAsInstruction<T>());
			}
			return result;
		}

		// Token: 0x060047F3 RID: 18419 RVA: 0x0017D340 File Offset: 0x0017B540
		protected internal override Instruction DefaultValue()
		{
			Instruction result;
			if ((result = this._defaultValue) == null)
			{
				result = (this._defaultValue = new DefaultValueInstruction<T>());
			}
			return result;
		}

		// Token: 0x060047F4 RID: 18420 RVA: 0x0017D368 File Offset: 0x0017B568
		protected internal override Instruction NewArray()
		{
			Instruction result;
			if ((result = this._newArray) == null)
			{
				result = (this._newArray = new NewArrayInstruction<T>());
			}
			return result;
		}

		// Token: 0x060047F5 RID: 18421 RVA: 0x0017D38D File Offset: 0x0017B58D
		protected internal override Instruction NewArrayInit(int elementCount)
		{
			return new NewArrayInitInstruction<T>(elementCount);
		}

		// Token: 0x04002322 RID: 8994
		public static readonly InstructionFactory Factory = new InstructionFactory<T>();

		// Token: 0x04002323 RID: 8995
		private Instruction _getArrayItem;

		// Token: 0x04002324 RID: 8996
		private Instruction _setArrayItem;

		// Token: 0x04002325 RID: 8997
		private Instruction _typeIs;

		// Token: 0x04002326 RID: 8998
		private Instruction _defaultValue;

		// Token: 0x04002327 RID: 8999
		private Instruction _newArray;

		// Token: 0x04002328 RID: 9000
		private Instruction _typeAs;
	}
}
