using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006D1 RID: 1745
	[DebuggerTypeProxy(typeof(InstructionList.DebugView))]
	internal sealed class InstructionList
	{
		// Token: 0x060047FC RID: 18428 RVA: 0x0017D43A File Offset: 0x0017B63A
		public void Emit(Instruction instruction)
		{
			this._instructions.Add(instruction);
			this.UpdateStackDepth(instruction);
		}

		// Token: 0x060047FD RID: 18429 RVA: 0x0017D450 File Offset: 0x0017B650
		private void UpdateStackDepth(Instruction instruction)
		{
			this._currentStackDepth -= instruction.ConsumedStack;
			this._currentStackDepth += instruction.ProducedStack;
			if (this._currentStackDepth > this._maxStackDepth)
			{
				this._maxStackDepth = this._currentStackDepth;
			}
			this._currentContinuationsDepth -= instruction.ConsumedContinuations;
			this._currentContinuationsDepth += instruction.ProducedContinuations;
			if (this._currentContinuationsDepth > this._maxContinuationDepth)
			{
				this._maxContinuationDepth = this._currentContinuationsDepth;
			}
		}

		// Token: 0x060047FE RID: 18430 RVA: 0x0017D4DD File Offset: 0x0017B6DD
		[Conditional("DEBUG")]
		public void SetDebugCookie(object cookie)
		{
		}

		// Token: 0x17000F53 RID: 3923
		// (get) Token: 0x060047FF RID: 18431 RVA: 0x0017D4DF File Offset: 0x0017B6DF
		public int Count
		{
			get
			{
				return this._instructions.Count;
			}
		}

		// Token: 0x17000F54 RID: 3924
		// (get) Token: 0x06004800 RID: 18432 RVA: 0x0017D4EC File Offset: 0x0017B6EC
		public int CurrentStackDepth
		{
			get
			{
				return this._currentStackDepth;
			}
		}

		// Token: 0x17000F55 RID: 3925
		// (get) Token: 0x06004801 RID: 18433 RVA: 0x0017D4F4 File Offset: 0x0017B6F4
		public int CurrentContinuationsDepth
		{
			get
			{
				return this._currentContinuationsDepth;
			}
		}

		// Token: 0x17000F56 RID: 3926
		// (get) Token: 0x06004802 RID: 18434 RVA: 0x0017D4FC File Offset: 0x0017B6FC
		public int MaxStackDepth
		{
			get
			{
				return this._maxStackDepth;
			}
		}

		// Token: 0x06004803 RID: 18435 RVA: 0x0017D504 File Offset: 0x0017B704
		internal Instruction GetInstruction(int index)
		{
			return this._instructions[index];
		}

		// Token: 0x06004804 RID: 18436 RVA: 0x0017D512 File Offset: 0x0017B712
		public InstructionArray ToArray()
		{
			return new InstructionArray(this._maxStackDepth, this._maxContinuationDepth, this._instructions.ToArray(), (this._objects != null) ? this._objects.ToArray() : null, this.BuildRuntimeLabels(), this._debugCookies);
		}

		// Token: 0x06004805 RID: 18437 RVA: 0x0017D552 File Offset: 0x0017B752
		public void EmitLoad(object value)
		{
			this.EmitLoad(value, null);
		}

		// Token: 0x06004806 RID: 18438 RVA: 0x0017D55C File Offset: 0x0017B75C
		public void EmitLoad(bool value)
		{
			if (value)
			{
				Instruction instruction;
				if ((instruction = InstructionList._true) == null)
				{
					instruction = (InstructionList._true = new LoadObjectInstruction(value));
				}
				this.Emit(instruction);
				return;
			}
			Instruction instruction2;
			if ((instruction2 = InstructionList._false) == null)
			{
				instruction2 = (InstructionList._false = new LoadObjectInstruction(value));
			}
			this.Emit(instruction2);
		}

		// Token: 0x06004807 RID: 18439 RVA: 0x0017D5B0 File Offset: 0x0017B7B0
		public void EmitLoad(object value, Type type)
		{
			if (value == null)
			{
				Instruction instruction;
				if ((instruction = InstructionList._null) == null)
				{
					instruction = (InstructionList._null = new LoadObjectInstruction(null));
				}
				this.Emit(instruction);
				return;
			}
			if (type == null || type.GetTypeInfo().IsValueType)
			{
				if (value is bool)
				{
					this.EmitLoad((bool)value);
					return;
				}
				if (value is int)
				{
					int num = (int)value;
					if (num >= -100 && num <= 100)
					{
						if (InstructionList._ints == null)
						{
							InstructionList._ints = new Instruction[201];
						}
						num -= -100;
						Instruction instruction2;
						if ((instruction2 = InstructionList._ints[num]) == null)
						{
							instruction2 = (InstructionList._ints[num] = new LoadObjectInstruction(value));
						}
						this.Emit(instruction2);
						return;
					}
				}
			}
			if (this._objects == null)
			{
				this._objects = new List<object>();
				if (InstructionList._loadObjectCached == null)
				{
					InstructionList._loadObjectCached = new Instruction[256];
				}
			}
			if (this._objects.Count < InstructionList._loadObjectCached.Length)
			{
				uint count = (uint)this._objects.Count;
				this._objects.Add(value);
				Instruction instruction3;
				if ((instruction3 = InstructionList._loadObjectCached[(int)((UIntPtr)count)]) == null)
				{
					instruction3 = (InstructionList._loadObjectCached[(int)((UIntPtr)count)] = new LoadCachedObjectInstruction(count));
				}
				this.Emit(instruction3);
				return;
			}
			this.Emit(new LoadObjectInstruction(value));
		}

		// Token: 0x06004808 RID: 18440 RVA: 0x0017D6E2 File Offset: 0x0017B8E2
		public void EmitDup()
		{
			this.Emit(DupInstruction.Instance);
		}

		// Token: 0x06004809 RID: 18441 RVA: 0x0017D6EF File Offset: 0x0017B8EF
		public void EmitPop()
		{
			this.Emit(PopInstruction.Instance);
		}

		// Token: 0x0600480A RID: 18442 RVA: 0x0017D6FC File Offset: 0x0017B8FC
		internal void SwitchToBoxed(int index, int instructionIndex)
		{
			IBoxableInstruction boxableInstruction = this._instructions[instructionIndex] as IBoxableInstruction;
			if (boxableInstruction != null)
			{
				Instruction instruction = boxableInstruction.BoxIfIndexMatches(index);
				if (instruction != null)
				{
					this._instructions[instructionIndex] = instruction;
				}
			}
		}

		// Token: 0x0600480B RID: 18443 RVA: 0x0017D738 File Offset: 0x0017B938
		public void EmitLoadLocal(int index)
		{
			if (InstructionList._loadLocal == null)
			{
				InstructionList._loadLocal = new Instruction[64];
			}
			if (index < InstructionList._loadLocal.Length)
			{
				Instruction instruction;
				if ((instruction = InstructionList._loadLocal[index]) == null)
				{
					instruction = (InstructionList._loadLocal[index] = new LoadLocalInstruction(index));
				}
				this.Emit(instruction);
				return;
			}
			this.Emit(new LoadLocalInstruction(index));
		}

		// Token: 0x0600480C RID: 18444 RVA: 0x0017D790 File Offset: 0x0017B990
		public void EmitLoadLocalBoxed(int index)
		{
			this.Emit(InstructionList.LoadLocalBoxed(index));
		}

		// Token: 0x0600480D RID: 18445 RVA: 0x0017D7A0 File Offset: 0x0017B9A0
		internal static Instruction LoadLocalBoxed(int index)
		{
			if (InstructionList._loadLocalBoxed == null)
			{
				InstructionList._loadLocalBoxed = new Instruction[64];
			}
			if (index < InstructionList._loadLocalBoxed.Length)
			{
				Instruction result;
				if ((result = InstructionList._loadLocalBoxed[index]) == null)
				{
					result = (InstructionList._loadLocalBoxed[index] = new LoadLocalBoxedInstruction(index));
				}
				return result;
			}
			return new LoadLocalBoxedInstruction(index);
		}

		// Token: 0x0600480E RID: 18446 RVA: 0x0017D7EC File Offset: 0x0017B9EC
		public void EmitLoadLocalFromClosure(int index)
		{
			if (InstructionList._loadLocalFromClosure == null)
			{
				InstructionList._loadLocalFromClosure = new Instruction[64];
			}
			if (index < InstructionList._loadLocalFromClosure.Length)
			{
				Instruction instruction;
				if ((instruction = InstructionList._loadLocalFromClosure[index]) == null)
				{
					instruction = (InstructionList._loadLocalFromClosure[index] = new LoadLocalFromClosureInstruction(index));
				}
				this.Emit(instruction);
				return;
			}
			this.Emit(new LoadLocalFromClosureInstruction(index));
		}

		// Token: 0x0600480F RID: 18447 RVA: 0x0017D844 File Offset: 0x0017BA44
		public void EmitLoadLocalFromClosureBoxed(int index)
		{
			if (InstructionList._loadLocalFromClosureBoxed == null)
			{
				InstructionList._loadLocalFromClosureBoxed = new Instruction[64];
			}
			if (index < InstructionList._loadLocalFromClosureBoxed.Length)
			{
				Instruction instruction;
				if ((instruction = InstructionList._loadLocalFromClosureBoxed[index]) == null)
				{
					instruction = (InstructionList._loadLocalFromClosureBoxed[index] = new LoadLocalFromClosureBoxedInstruction(index));
				}
				this.Emit(instruction);
				return;
			}
			this.Emit(new LoadLocalFromClosureBoxedInstruction(index));
		}

		// Token: 0x06004810 RID: 18448 RVA: 0x0017D89C File Offset: 0x0017BA9C
		public void EmitAssignLocal(int index)
		{
			if (InstructionList._assignLocal == null)
			{
				InstructionList._assignLocal = new Instruction[64];
			}
			if (index < InstructionList._assignLocal.Length)
			{
				Instruction instruction;
				if ((instruction = InstructionList._assignLocal[index]) == null)
				{
					instruction = (InstructionList._assignLocal[index] = new AssignLocalInstruction(index));
				}
				this.Emit(instruction);
				return;
			}
			this.Emit(new AssignLocalInstruction(index));
		}

		// Token: 0x06004811 RID: 18449 RVA: 0x0017D8F4 File Offset: 0x0017BAF4
		public void EmitStoreLocal(int index)
		{
			if (InstructionList._storeLocal == null)
			{
				InstructionList._storeLocal = new Instruction[64];
			}
			if (index < InstructionList._storeLocal.Length)
			{
				Instruction instruction;
				if ((instruction = InstructionList._storeLocal[index]) == null)
				{
					instruction = (InstructionList._storeLocal[index] = new StoreLocalInstruction(index));
				}
				this.Emit(instruction);
				return;
			}
			this.Emit(new StoreLocalInstruction(index));
		}

		// Token: 0x06004812 RID: 18450 RVA: 0x0017D94C File Offset: 0x0017BB4C
		public void EmitAssignLocalBoxed(int index)
		{
			this.Emit(InstructionList.AssignLocalBoxed(index));
		}

		// Token: 0x06004813 RID: 18451 RVA: 0x0017D95C File Offset: 0x0017BB5C
		internal static Instruction AssignLocalBoxed(int index)
		{
			if (InstructionList._assignLocalBoxed == null)
			{
				InstructionList._assignLocalBoxed = new Instruction[64];
			}
			if (index < InstructionList._assignLocalBoxed.Length)
			{
				Instruction result;
				if ((result = InstructionList._assignLocalBoxed[index]) == null)
				{
					result = (InstructionList._assignLocalBoxed[index] = new AssignLocalBoxedInstruction(index));
				}
				return result;
			}
			return new AssignLocalBoxedInstruction(index);
		}

		// Token: 0x06004814 RID: 18452 RVA: 0x0017D9A8 File Offset: 0x0017BBA8
		public void EmitStoreLocalBoxed(int index)
		{
			this.Emit(InstructionList.StoreLocalBoxed(index));
		}

		// Token: 0x06004815 RID: 18453 RVA: 0x0017D9B8 File Offset: 0x0017BBB8
		internal static Instruction StoreLocalBoxed(int index)
		{
			if (InstructionList._storeLocalBoxed == null)
			{
				InstructionList._storeLocalBoxed = new Instruction[64];
			}
			if (index < InstructionList._storeLocalBoxed.Length)
			{
				Instruction result;
				if ((result = InstructionList._storeLocalBoxed[index]) == null)
				{
					result = (InstructionList._storeLocalBoxed[index] = new StoreLocalBoxedInstruction(index));
				}
				return result;
			}
			return new StoreLocalBoxedInstruction(index);
		}

		// Token: 0x06004816 RID: 18454 RVA: 0x0017DA04 File Offset: 0x0017BC04
		public void EmitAssignLocalToClosure(int index)
		{
			if (InstructionList._assignLocalToClosure == null)
			{
				InstructionList._assignLocalToClosure = new Instruction[64];
			}
			if (index < InstructionList._assignLocalToClosure.Length)
			{
				Instruction instruction;
				if ((instruction = InstructionList._assignLocalToClosure[index]) == null)
				{
					instruction = (InstructionList._assignLocalToClosure[index] = new AssignLocalToClosureInstruction(index));
				}
				this.Emit(instruction);
				return;
			}
			this.Emit(new AssignLocalToClosureInstruction(index));
		}

		// Token: 0x06004817 RID: 18455 RVA: 0x0017DA5C File Offset: 0x0017BC5C
		public void EmitStoreLocalToClosure(int index)
		{
			this.EmitAssignLocalToClosure(index);
			this.EmitPop();
		}

		// Token: 0x06004818 RID: 18456 RVA: 0x0017DA6C File Offset: 0x0017BC6C
		public void EmitInitializeLocal(int index, Type type)
		{
			object primitiveDefaultValue = ScriptingRuntimeHelpers.GetPrimitiveDefaultValue(type);
			if (primitiveDefaultValue != null)
			{
				this.Emit(new InitializeLocalInstruction.ImmutableValue(index, primitiveDefaultValue));
				return;
			}
			if (type.GetTypeInfo().IsValueType)
			{
				this.Emit(new InitializeLocalInstruction.MutableValue(index, type));
				return;
			}
			this.Emit(InstructionList.InitReference(index));
		}

		// Token: 0x06004819 RID: 18457 RVA: 0x0017DAB8 File Offset: 0x0017BCB8
		internal void EmitInitializeParameter(int index)
		{
			this.Emit(InstructionList.Parameter(index));
		}

		// Token: 0x0600481A RID: 18458 RVA: 0x0017DAC8 File Offset: 0x0017BCC8
		internal static Instruction Parameter(int index)
		{
			if (InstructionList._parameter == null)
			{
				InstructionList._parameter = new Instruction[64];
			}
			if (index < InstructionList._parameter.Length)
			{
				Instruction result;
				if ((result = InstructionList._parameter[index]) == null)
				{
					result = (InstructionList._parameter[index] = new InitializeLocalInstruction.Parameter(index));
				}
				return result;
			}
			return new InitializeLocalInstruction.Parameter(index);
		}

		// Token: 0x0600481B RID: 18459 RVA: 0x0017DB14 File Offset: 0x0017BD14
		internal static Instruction ParameterBox(int index)
		{
			if (InstructionList._parameterBox == null)
			{
				InstructionList._parameterBox = new Instruction[64];
			}
			if (index < InstructionList._parameterBox.Length)
			{
				Instruction result;
				if ((result = InstructionList._parameterBox[index]) == null)
				{
					result = (InstructionList._parameterBox[index] = new InitializeLocalInstruction.ParameterBox(index));
				}
				return result;
			}
			return new InitializeLocalInstruction.ParameterBox(index);
		}

		// Token: 0x0600481C RID: 18460 RVA: 0x0017DB60 File Offset: 0x0017BD60
		internal static Instruction InitReference(int index)
		{
			if (InstructionList._initReference == null)
			{
				InstructionList._initReference = new Instruction[64];
			}
			if (index < InstructionList._initReference.Length)
			{
				Instruction result;
				if ((result = InstructionList._initReference[index]) == null)
				{
					result = (InstructionList._initReference[index] = new InitializeLocalInstruction.Reference(index));
				}
				return result;
			}
			return new InitializeLocalInstruction.Reference(index);
		}

		// Token: 0x0600481D RID: 18461 RVA: 0x0017DBAC File Offset: 0x0017BDAC
		internal static Instruction InitImmutableRefBox(int index)
		{
			if (InstructionList._initImmutableRefBox == null)
			{
				InstructionList._initImmutableRefBox = new Instruction[64];
			}
			if (index < InstructionList._initImmutableRefBox.Length)
			{
				Instruction result;
				if ((result = InstructionList._initImmutableRefBox[index]) == null)
				{
					result = (InstructionList._initImmutableRefBox[index] = new InitializeLocalInstruction.ImmutableBox(index, null));
				}
				return result;
			}
			return new InitializeLocalInstruction.ImmutableBox(index, null);
		}

		// Token: 0x0600481E RID: 18462 RVA: 0x0017DBFA File Offset: 0x0017BDFA
		public void EmitNewRuntimeVariables(int count)
		{
			this.Emit(new RuntimeVariablesInstruction(count));
		}

		// Token: 0x0600481F RID: 18463 RVA: 0x0017DC08 File Offset: 0x0017BE08
		public void EmitGetArrayItem(Type arrayType)
		{
			Type elementType = arrayType.GetElementType();
			TypeInfo typeInfo = elementType.GetTypeInfo();
			if (typeInfo.IsClass || typeInfo.IsInterface)
			{
				this.Emit(InstructionFactory<object>.Factory.GetArrayItem());
				return;
			}
			this.Emit(InstructionFactory.GetFactory(elementType).GetArrayItem());
		}

		// Token: 0x06004820 RID: 18464 RVA: 0x0017DC58 File Offset: 0x0017BE58
		public void EmitSetArrayItem(Type arrayType)
		{
			Type elementType = arrayType.GetElementType();
			TypeInfo typeInfo = elementType.GetTypeInfo();
			if (typeInfo.IsClass || typeInfo.IsInterface)
			{
				this.Emit(InstructionFactory<object>.Factory.SetArrayItem());
				return;
			}
			this.Emit(InstructionFactory.GetFactory(elementType).SetArrayItem());
		}

		// Token: 0x06004821 RID: 18465 RVA: 0x0017DCA5 File Offset: 0x0017BEA5
		public void EmitNewArray(Type elementType)
		{
			this.Emit(InstructionFactory.GetFactory(elementType).NewArray());
		}

		// Token: 0x06004822 RID: 18466 RVA: 0x0017DCB8 File Offset: 0x0017BEB8
		public void EmitNewArrayBounds(Type elementType, int rank)
		{
			this.Emit(new NewArrayBoundsInstruction(elementType, rank));
		}

		// Token: 0x06004823 RID: 18467 RVA: 0x0017DCC7 File Offset: 0x0017BEC7
		public void EmitNewArrayInit(Type elementType, int elementCount)
		{
			this.Emit(InstructionFactory.GetFactory(elementType).NewArrayInit(elementCount));
		}

		// Token: 0x06004824 RID: 18468 RVA: 0x0017DCDB File Offset: 0x0017BEDB
		public void EmitAdd(Type type, bool @checked)
		{
			if (@checked)
			{
				this.Emit(AddOvfInstruction.Create(type));
				return;
			}
			this.Emit(AddInstruction.Create(type));
		}

		// Token: 0x06004825 RID: 18469 RVA: 0x0017DCF9 File Offset: 0x0017BEF9
		public void EmitSub(Type type, bool @checked)
		{
			if (@checked)
			{
				this.Emit(SubOvfInstruction.Create(type));
				return;
			}
			this.Emit(SubInstruction.Create(type));
		}

		// Token: 0x06004826 RID: 18470 RVA: 0x0017DD17 File Offset: 0x0017BF17
		public void EmitMul(Type type, bool @checked)
		{
			if (@checked)
			{
				this.Emit(MulOvfInstruction.Create(type));
				return;
			}
			this.Emit(MulInstruction.Create(type));
		}

		// Token: 0x06004827 RID: 18471 RVA: 0x0017DD35 File Offset: 0x0017BF35
		public void EmitDiv(Type type)
		{
			this.Emit(DivInstruction.Create(type));
		}

		// Token: 0x06004828 RID: 18472 RVA: 0x0017DD43 File Offset: 0x0017BF43
		public void EmitEqual(Type type)
		{
			this.Emit(EqualInstruction.Create(type));
		}

		// Token: 0x06004829 RID: 18473 RVA: 0x0017DD51 File Offset: 0x0017BF51
		public void EmitNotEqual(Type type)
		{
			this.Emit(NotEqualInstruction.Create(type));
		}

		// Token: 0x0600482A RID: 18474 RVA: 0x0017DD5F File Offset: 0x0017BF5F
		public void EmitLessThan(Type type)
		{
			this.Emit(LessThanInstruction.Create(type));
		}

		// Token: 0x0600482B RID: 18475 RVA: 0x0017DD6D File Offset: 0x0017BF6D
		public void EmitLessThanOrEqual(Type type)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600482C RID: 18476 RVA: 0x0017DD74 File Offset: 0x0017BF74
		public void EmitGreaterThan(Type type)
		{
			this.Emit(GreaterThanInstruction.Create(type));
		}

		// Token: 0x0600482D RID: 18477 RVA: 0x0017DD82 File Offset: 0x0017BF82
		public void EmitGreaterThanOrEqual(Type type)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600482E RID: 18478 RVA: 0x0017DD89 File Offset: 0x0017BF89
		public void EmitNumericConvertChecked(TypeCode from, TypeCode to)
		{
			this.Emit(new NumericConvertInstruction.Checked(from, to));
		}

		// Token: 0x0600482F RID: 18479 RVA: 0x0017DD98 File Offset: 0x0017BF98
		public void EmitNumericConvertUnchecked(TypeCode from, TypeCode to)
		{
			this.Emit(new NumericConvertInstruction.Unchecked(from, to));
		}

		// Token: 0x06004830 RID: 18480 RVA: 0x0017DDA7 File Offset: 0x0017BFA7
		public void EmitNot()
		{
			this.Emit(NotInstruction.Instance);
		}

		// Token: 0x06004831 RID: 18481 RVA: 0x0017DDB4 File Offset: 0x0017BFB4
		public void EmitDefaultValue(Type type)
		{
			this.Emit(InstructionFactory.GetFactory(type).DefaultValue());
		}

		// Token: 0x06004832 RID: 18482 RVA: 0x0017DDC7 File Offset: 0x0017BFC7
		public void EmitNew(ConstructorInfo constructorInfo)
		{
			this.Emit(new NewInstruction(constructorInfo));
		}

		// Token: 0x06004833 RID: 18483 RVA: 0x0017DDD5 File Offset: 0x0017BFD5
		internal void EmitCreateDelegate(LightDelegateCreator creator)
		{
			this.Emit(new CreateDelegateInstruction(creator));
		}

		// Token: 0x06004834 RID: 18484 RVA: 0x0017DDE3 File Offset: 0x0017BFE3
		public void EmitTypeEquals()
		{
			this.Emit(TypeEqualsInstruction.Instance);
		}

		// Token: 0x06004835 RID: 18485 RVA: 0x0017DDF0 File Offset: 0x0017BFF0
		public void EmitTypeIs(Type type)
		{
			this.Emit(InstructionFactory.GetFactory(type).TypeIs());
		}

		// Token: 0x06004836 RID: 18486 RVA: 0x0017DE03 File Offset: 0x0017C003
		public void EmitTypeAs(Type type)
		{
			this.Emit(InstructionFactory.GetFactory(type).TypeAs());
		}

		// Token: 0x06004837 RID: 18487 RVA: 0x0017DE16 File Offset: 0x0017C016
		public void EmitLoadField(FieldInfo field)
		{
			this.Emit(this.GetLoadField(field));
		}

		// Token: 0x06004838 RID: 18488 RVA: 0x0017DE28 File Offset: 0x0017C028
		private Instruction GetLoadField(FieldInfo field)
		{
			Instruction result;
			lock (InstructionList._loadFields)
			{
				Instruction instruction;
				if (!InstructionList._loadFields.TryGetValue(field, out instruction))
				{
					if (field.IsStatic)
					{
						instruction = new LoadStaticFieldInstruction(field);
					}
					else
					{
						instruction = new LoadFieldInstruction(field);
					}
					InstructionList._loadFields.Add(field, instruction);
				}
				result = instruction;
			}
			return result;
		}

		// Token: 0x06004839 RID: 18489 RVA: 0x0017DE98 File Offset: 0x0017C098
		public void EmitStoreField(FieldInfo field)
		{
			if (field.IsStatic)
			{
				this.Emit(new StoreStaticFieldInstruction(field));
				return;
			}
			this.Emit(new StoreFieldInstruction(field));
		}

		// Token: 0x0600483A RID: 18490 RVA: 0x0017DEBB File Offset: 0x0017C0BB
		public void EmitCall(MethodInfo method)
		{
			this.EmitCall(method, method.GetParameters());
		}

		// Token: 0x0600483B RID: 18491 RVA: 0x0017DECA File Offset: 0x0017C0CA
		public void EmitCall(MethodInfo method, ParameterInfo[] parameters)
		{
			this.Emit(CallInstruction.Create(method, parameters));
		}

		// Token: 0x0600483C RID: 18492 RVA: 0x0017DED9 File Offset: 0x0017C0D9
		public void EmitDynamic(Type type, CallSiteBinder binder)
		{
			this.Emit(InstructionList.CreateDynamicInstruction(type, binder));
		}

		// Token: 0x0600483D RID: 18493 RVA: 0x0017DEE8 File Offset: 0x0017C0E8
		public void EmitDynamic<T0, TRet>(CallSiteBinder binder)
		{
			this.Emit(DynamicInstruction<T0, TRet>.Factory(binder));
		}

		// Token: 0x0600483E RID: 18494 RVA: 0x0017DEF6 File Offset: 0x0017C0F6
		public void EmitDynamic<T0, T1, TRet>(CallSiteBinder binder)
		{
			this.Emit(DynamicInstruction<T0, T1, TRet>.Factory(binder));
		}

		// Token: 0x0600483F RID: 18495 RVA: 0x0017DF04 File Offset: 0x0017C104
		public void EmitDynamic<T0, T1, T2, TRet>(CallSiteBinder binder)
		{
			this.Emit(DynamicInstruction<T0, T1, T2, TRet>.Factory(binder));
		}

		// Token: 0x06004840 RID: 18496 RVA: 0x0017DF12 File Offset: 0x0017C112
		public void EmitDynamic<T0, T1, T2, T3, TRet>(CallSiteBinder binder)
		{
			this.Emit(DynamicInstruction<T0, T1, T2, T3, TRet>.Factory(binder));
		}

		// Token: 0x06004841 RID: 18497 RVA: 0x0017DF20 File Offset: 0x0017C120
		public void EmitDynamic<T0, T1, T2, T3, T4, TRet>(CallSiteBinder binder)
		{
			this.Emit(DynamicInstruction<T0, T1, T2, T3, T4, TRet>.Factory(binder));
		}

		// Token: 0x06004842 RID: 18498 RVA: 0x0017DF2E File Offset: 0x0017C12E
		public void EmitDynamic<T0, T1, T2, T3, T4, T5, TRet>(CallSiteBinder binder)
		{
			this.Emit(DynamicInstruction<T0, T1, T2, T3, T4, T5, TRet>.Factory(binder));
		}

		// Token: 0x06004843 RID: 18499 RVA: 0x0017DF3C File Offset: 0x0017C13C
		public void EmitDynamic<T0, T1, T2, T3, T4, T5, T6, TRet>(CallSiteBinder binder)
		{
			this.Emit(DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, TRet>.Factory(binder));
		}

		// Token: 0x06004844 RID: 18500 RVA: 0x0017DF4A File Offset: 0x0017C14A
		public void EmitDynamic<T0, T1, T2, T3, T4, T5, T6, T7, TRet>(CallSiteBinder binder)
		{
			this.Emit(DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, TRet>.Factory(binder));
		}

		// Token: 0x06004845 RID: 18501 RVA: 0x0017DF58 File Offset: 0x0017C158
		public void EmitDynamic<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>(CallSiteBinder binder)
		{
			this.Emit(DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8, TRet>.Factory(binder));
		}

		// Token: 0x06004846 RID: 18502 RVA: 0x0017DF66 File Offset: 0x0017C166
		public void EmitDynamic<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>(CallSiteBinder binder)
		{
			this.Emit(DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>.Factory(binder));
		}

		// Token: 0x06004847 RID: 18503 RVA: 0x0017DF74 File Offset: 0x0017C174
		public void EmitDynamic<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRet>(CallSiteBinder binder)
		{
			this.Emit(DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRet>.Factory(binder));
		}

		// Token: 0x06004848 RID: 18504 RVA: 0x0017DF82 File Offset: 0x0017C182
		public void EmitDynamic<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRet>(CallSiteBinder binder)
		{
			this.Emit(DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRet>.Factory(binder));
		}

		// Token: 0x06004849 RID: 18505 RVA: 0x0017DF90 File Offset: 0x0017C190
		public void EmitDynamic<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRet>(CallSiteBinder binder)
		{
			this.Emit(DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRet>.Factory(binder));
		}

		// Token: 0x0600484A RID: 18506 RVA: 0x0017DF9E File Offset: 0x0017C19E
		public void EmitDynamic<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRet>(CallSiteBinder binder)
		{
			this.Emit(DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRet>.Factory(binder));
		}

		// Token: 0x0600484B RID: 18507 RVA: 0x0017DFAC File Offset: 0x0017C1AC
		public void EmitDynamic<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRet>(CallSiteBinder binder)
		{
			this.Emit(DynamicInstruction<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRet>.Factory(binder));
		}

		// Token: 0x0600484C RID: 18508 RVA: 0x0017DFBC File Offset: 0x0017C1BC
		internal static Instruction CreateDynamicInstruction(Type delegateType, CallSiteBinder binder)
		{
			Func<CallSiteBinder, Instruction> func;
			lock (InstructionList._factories)
			{
				if (!InstructionList._factories.TryGetValue(delegateType, out func))
				{
					if (delegateType.GetMethod("Invoke").ReturnType == typeof(void))
					{
						return new DynamicInstructionN(delegateType, CallSite.Create(delegateType, binder), true);
					}
					Type dynamicInstructionType = DynamicInstructionN.GetDynamicInstructionType(delegateType);
					if (dynamicInstructionType == null)
					{
						return new DynamicInstructionN(delegateType, CallSite.Create(delegateType, binder));
					}
					func = (Func<CallSiteBinder, Instruction>)dynamicInstructionType.GetMethod("Factory").CreateDelegate(typeof(Func<CallSiteBinder, Instruction>));
					InstructionList._factories[delegateType] = func;
				}
			}
			return func(binder);
		}

		// Token: 0x0600484D RID: 18509 RVA: 0x0017E090 File Offset: 0x0017C290
		private RuntimeLabel[] BuildRuntimeLabels()
		{
			if (this._runtimeLabelCount == 0)
			{
				return InstructionList.EmptyRuntimeLabels;
			}
			RuntimeLabel[] array = new RuntimeLabel[this._runtimeLabelCount + 1];
			foreach (BranchLabel branchLabel in this._labels)
			{
				if (branchLabel.HasRuntimeLabel)
				{
					array[branchLabel.LabelIndex] = branchLabel.ToRuntimeLabel();
				}
			}
			array[array.Length - 1] = new RuntimeLabel(int.MaxValue, 0, 0);
			return array;
		}

		// Token: 0x0600484E RID: 18510 RVA: 0x0017E134 File Offset: 0x0017C334
		public BranchLabel MakeLabel()
		{
			if (this._labels == null)
			{
				this._labels = new List<BranchLabel>();
			}
			BranchLabel branchLabel = new BranchLabel();
			this._labels.Add(branchLabel);
			return branchLabel;
		}

		// Token: 0x0600484F RID: 18511 RVA: 0x0017E167 File Offset: 0x0017C367
		internal void FixupBranch(int branchIndex, int offset)
		{
			this._instructions[branchIndex] = ((OffsetInstruction)this._instructions[branchIndex]).Fixup(offset);
		}

		// Token: 0x06004850 RID: 18512 RVA: 0x0017E18C File Offset: 0x0017C38C
		private int EnsureLabelIndex(BranchLabel label)
		{
			if (label.HasRuntimeLabel)
			{
				return label.LabelIndex;
			}
			label.LabelIndex = this._runtimeLabelCount;
			this._runtimeLabelCount++;
			return label.LabelIndex;
		}

		// Token: 0x06004851 RID: 18513 RVA: 0x0017E1C0 File Offset: 0x0017C3C0
		public int MarkRuntimeLabel()
		{
			BranchLabel label = this.MakeLabel();
			this.MarkLabel(label);
			return this.EnsureLabelIndex(label);
		}

		// Token: 0x06004852 RID: 18514 RVA: 0x0017E1E2 File Offset: 0x0017C3E2
		public void MarkLabel(BranchLabel label)
		{
			label.Mark(this);
		}

		// Token: 0x06004853 RID: 18515 RVA: 0x0017E1EB File Offset: 0x0017C3EB
		public void EmitGoto(BranchLabel label, bool hasResult, bool hasValue)
		{
			this.Emit(GotoInstruction.Create(this.EnsureLabelIndex(label), hasResult, hasValue));
		}

		// Token: 0x06004854 RID: 18516 RVA: 0x0017E201 File Offset: 0x0017C401
		private void EmitBranch(OffsetInstruction instruction, BranchLabel label)
		{
			this.Emit(instruction);
			label.AddBranch(this, this.Count - 1);
		}

		// Token: 0x06004855 RID: 18517 RVA: 0x0017E219 File Offset: 0x0017C419
		public void EmitBranch(BranchLabel label)
		{
			this.EmitBranch(new BranchInstruction(), label);
		}

		// Token: 0x06004856 RID: 18518 RVA: 0x0017E227 File Offset: 0x0017C427
		public void EmitBranch(BranchLabel label, bool hasResult, bool hasValue)
		{
			this.EmitBranch(new BranchInstruction(hasResult, hasValue), label);
		}

		// Token: 0x06004857 RID: 18519 RVA: 0x0017E237 File Offset: 0x0017C437
		public void EmitCoalescingBranch(BranchLabel leftNotNull)
		{
			this.EmitBranch(new CoalescingBranchInstruction(), leftNotNull);
		}

		// Token: 0x06004858 RID: 18520 RVA: 0x0017E245 File Offset: 0x0017C445
		public void EmitBranchTrue(BranchLabel elseLabel)
		{
			this.EmitBranch(new BranchTrueInstruction(), elseLabel);
		}

		// Token: 0x06004859 RID: 18521 RVA: 0x0017E253 File Offset: 0x0017C453
		public void EmitBranchFalse(BranchLabel elseLabel)
		{
			this.EmitBranch(new BranchFalseInstruction(), elseLabel);
		}

		// Token: 0x0600485A RID: 18522 RVA: 0x0017E261 File Offset: 0x0017C461
		public void EmitThrow()
		{
			this.Emit(ThrowInstruction.Throw);
		}

		// Token: 0x0600485B RID: 18523 RVA: 0x0017E26E File Offset: 0x0017C46E
		public void EmitThrowVoid()
		{
			this.Emit(ThrowInstruction.VoidThrow);
		}

		// Token: 0x0600485C RID: 18524 RVA: 0x0017E27B File Offset: 0x0017C47B
		public void EmitRethrow()
		{
			this.Emit(ThrowInstruction.Rethrow);
		}

		// Token: 0x0600485D RID: 18525 RVA: 0x0017E288 File Offset: 0x0017C488
		public void EmitRethrowVoid()
		{
			this.Emit(ThrowInstruction.VoidRethrow);
		}

		// Token: 0x0600485E RID: 18526 RVA: 0x0017E295 File Offset: 0x0017C495
		public void EmitEnterTryFinally(BranchLabel finallyStartLabel)
		{
			this.Emit(EnterTryCatchFinallyInstruction.CreateTryFinally(this.EnsureLabelIndex(finallyStartLabel)));
		}

		// Token: 0x0600485F RID: 18527 RVA: 0x0017E2A9 File Offset: 0x0017C4A9
		public void EmitEnterTryCatch()
		{
			this.Emit(EnterTryCatchFinallyInstruction.CreateTryCatch());
		}

		// Token: 0x06004860 RID: 18528 RVA: 0x0017E2B6 File Offset: 0x0017C4B6
		public void EmitEnterFinally(BranchLabel finallyStartLabel)
		{
			this.Emit(EnterFinallyInstruction.Create(this.EnsureLabelIndex(finallyStartLabel)));
		}

		// Token: 0x06004861 RID: 18529 RVA: 0x0017E2CA File Offset: 0x0017C4CA
		public void EmitLeaveFinally()
		{
			this.Emit(LeaveFinallyInstruction.Instance);
		}

		// Token: 0x06004862 RID: 18530 RVA: 0x0017E2D7 File Offset: 0x0017C4D7
		public void EmitLeaveFault(bool hasValue)
		{
			this.Emit(hasValue ? LeaveFaultInstruction.NonVoid : LeaveFaultInstruction.Void);
		}

		// Token: 0x06004863 RID: 18531 RVA: 0x0017E2EE File Offset: 0x0017C4EE
		public void EmitEnterExceptionHandlerNonVoid()
		{
			this.Emit(EnterExceptionHandlerInstruction.NonVoid);
		}

		// Token: 0x06004864 RID: 18532 RVA: 0x0017E2FB File Offset: 0x0017C4FB
		public void EmitEnterExceptionHandlerVoid()
		{
			this.Emit(EnterExceptionHandlerInstruction.Void);
		}

		// Token: 0x06004865 RID: 18533 RVA: 0x0017E308 File Offset: 0x0017C508
		public void EmitLeaveExceptionHandler(bool hasValue, BranchLabel tryExpressionEndLabel)
		{
			this.Emit(LeaveExceptionHandlerInstruction.Create(this.EnsureLabelIndex(tryExpressionEndLabel), hasValue));
		}

		// Token: 0x06004866 RID: 18534 RVA: 0x0017E31D File Offset: 0x0017C51D
		public void EmitSwitch(Dictionary<int, int> cases)
		{
			this.Emit(new SwitchInstruction(cases));
		}

		// Token: 0x04002330 RID: 9008
		private const int PushIntMinCachedValue = -100;

		// Token: 0x04002331 RID: 9009
		private const int PushIntMaxCachedValue = 100;

		// Token: 0x04002332 RID: 9010
		private const int CachedObjectCount = 256;

		// Token: 0x04002333 RID: 9011
		private const int LocalInstrCacheSize = 64;

		// Token: 0x04002334 RID: 9012
		private readonly List<Instruction> _instructions = new List<Instruction>();

		// Token: 0x04002335 RID: 9013
		private List<object> _objects;

		// Token: 0x04002336 RID: 9014
		private int _currentStackDepth;

		// Token: 0x04002337 RID: 9015
		private int _maxStackDepth;

		// Token: 0x04002338 RID: 9016
		private int _currentContinuationsDepth;

		// Token: 0x04002339 RID: 9017
		private int _maxContinuationDepth;

		// Token: 0x0400233A RID: 9018
		private int _runtimeLabelCount;

		// Token: 0x0400233B RID: 9019
		private List<BranchLabel> _labels;

		// Token: 0x0400233C RID: 9020
		private List<KeyValuePair<int, object>> _debugCookies;

		// Token: 0x0400233D RID: 9021
		private static Instruction _null;

		// Token: 0x0400233E RID: 9022
		private static Instruction _true;

		// Token: 0x0400233F RID: 9023
		private static Instruction _false;

		// Token: 0x04002340 RID: 9024
		private static Instruction[] _ints;

		// Token: 0x04002341 RID: 9025
		private static Instruction[] _loadObjectCached;

		// Token: 0x04002342 RID: 9026
		private static Instruction[] _loadLocal;

		// Token: 0x04002343 RID: 9027
		private static Instruction[] _loadLocalBoxed;

		// Token: 0x04002344 RID: 9028
		private static Instruction[] _loadLocalFromClosure;

		// Token: 0x04002345 RID: 9029
		private static Instruction[] _loadLocalFromClosureBoxed;

		// Token: 0x04002346 RID: 9030
		private static Instruction[] _assignLocal;

		// Token: 0x04002347 RID: 9031
		private static Instruction[] _storeLocal;

		// Token: 0x04002348 RID: 9032
		private static Instruction[] _assignLocalBoxed;

		// Token: 0x04002349 RID: 9033
		private static Instruction[] _storeLocalBoxed;

		// Token: 0x0400234A RID: 9034
		private static Instruction[] _assignLocalToClosure;

		// Token: 0x0400234B RID: 9035
		private static Instruction[] _initReference;

		// Token: 0x0400234C RID: 9036
		private static Instruction[] _initImmutableRefBox;

		// Token: 0x0400234D RID: 9037
		private static Instruction[] _parameterBox;

		// Token: 0x0400234E RID: 9038
		private static Instruction[] _parameter;

		// Token: 0x0400234F RID: 9039
		private static readonly Dictionary<FieldInfo, Instruction> _loadFields = new Dictionary<FieldInfo, Instruction>();

		// Token: 0x04002350 RID: 9040
		private static Dictionary<Type, Func<CallSiteBinder, Instruction>> _factories = new Dictionary<Type, Func<CallSiteBinder, Instruction>>();

		// Token: 0x04002351 RID: 9041
		private static readonly RuntimeLabel[] EmptyRuntimeLabels = new RuntimeLabel[]
		{
			new RuntimeLabel(int.MaxValue, 0, 0)
		};

		// Token: 0x020006D2 RID: 1746
		internal sealed class DebugView
		{
			// Token: 0x06004869 RID: 18537 RVA: 0x0017E385 File Offset: 0x0017C585
			public DebugView(InstructionList list)
			{
				this._list = list;
			}

			// Token: 0x17000F57 RID: 3927
			// (get) Token: 0x0600486A RID: 18538 RVA: 0x0017E3AC File Offset: 0x0017C5AC
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public InstructionList.DebugView.InstructionView[] A0
			{
				get
				{
					return InstructionList.DebugView.GetInstructionViews(this._list._instructions, this._list._objects, (int index) => this._list._labels[index].TargetIndex, this._list._debugCookies);
				}
			}

			// Token: 0x0600486B RID: 18539 RVA: 0x0017E3E0 File Offset: 0x0017C5E0
			internal static InstructionList.DebugView.InstructionView[] GetInstructionViews(IList<Instruction> instructions, IList<object> objects, Func<int, int> labelIndexer, IList<KeyValuePair<int, object>> debugCookies)
			{
				List<InstructionList.DebugView.InstructionView> list = new List<InstructionList.DebugView.InstructionView>();
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				IEnumerator<KeyValuePair<int, object>> enumerator = ((debugCookies != null) ? debugCookies : new KeyValuePair<int, object>[0]).GetEnumerator();
				bool flag = enumerator.MoveNext();
				for (int i = 0; i < instructions.Count; i++)
				{
					object cookie = null;
					while (flag)
					{
						KeyValuePair<int, object> keyValuePair = enumerator.Current;
						if (keyValuePair.Key != i)
						{
							break;
						}
						KeyValuePair<int, object> keyValuePair2 = enumerator.Current;
						cookie = keyValuePair2.Value;
						flag = enumerator.MoveNext();
					}
					int stackBalance = instructions[i].StackBalance;
					int continuationsBalance = instructions[i].ContinuationsBalance;
					string name = instructions[i].ToDebugString(i, cookie, labelIndexer, objects);
					list.Add(new InstructionList.DebugView.InstructionView(instructions[i], name, i, num2, num3));
					num++;
					num2 += stackBalance;
					num3 += continuationsBalance;
				}
				return list.ToArray();
			}

			// Token: 0x04002352 RID: 9042
			private readonly InstructionList _list;

			// Token: 0x020006D3 RID: 1747
			[DebuggerDisplay("{GetValue(),nq}", Name = "{GetName(),nq}", Type = "{GetDisplayType(), nq}")]
			internal struct InstructionView
			{
				// Token: 0x0600486D RID: 18541 RVA: 0x0017E4C8 File Offset: 0x0017C6C8
				internal string GetName()
				{
					return this._index + ((this._continuationsDepth == 0) ? "" : (" C(" + this._continuationsDepth + ")")) + ((this._stackDepth == 0) ? "" : (" S(" + this._stackDepth + ")"));
				}

				// Token: 0x0600486E RID: 18542 RVA: 0x0017E537 File Offset: 0x0017C737
				internal string GetValue()
				{
					return this._name;
				}

				// Token: 0x0600486F RID: 18543 RVA: 0x0017E53F File Offset: 0x0017C73F
				internal string GetDisplayType()
				{
					return this._instruction.ContinuationsBalance + "/" + this._instruction.StackBalance;
				}

				// Token: 0x06004870 RID: 18544 RVA: 0x0017E56B File Offset: 0x0017C76B
				public InstructionView(Instruction instruction, string name, int index, int stackDepth, int continuationsDepth)
				{
					this._instruction = instruction;
					this._name = name;
					this._index = index;
					this._stackDepth = stackDepth;
					this._continuationsDepth = continuationsDepth;
				}

				// Token: 0x04002353 RID: 9043
				private readonly int _index;

				// Token: 0x04002354 RID: 9044
				private readonly int _stackDepth;

				// Token: 0x04002355 RID: 9045
				private readonly int _continuationsDepth;

				// Token: 0x04002356 RID: 9046
				private readonly string _name;

				// Token: 0x04002357 RID: 9047
				private readonly Instruction _instruction;
			}
		}
	}
}
