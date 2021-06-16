using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000707 RID: 1799
	internal sealed class LocalVariables
	{
		// Token: 0x060049EF RID: 18927 RVA: 0x00185638 File Offset: 0x00183838
		internal LocalVariables()
		{
		}

		// Token: 0x060049F0 RID: 18928 RVA: 0x0018564C File Offset: 0x0018384C
		public LocalDefinition DefineLocal(ParameterExpression variable, int start)
		{
			LocalVariable localVariable = new LocalVariable(this._localCount++, false, false);
			this._maxLocalCount = Math.Max(this._localCount, this._maxLocalCount);
			LocalVariables.VariableScope variableScope;
			LocalVariables.VariableScope variableScope2;
			if (this._variables.TryGetValue(variable, out variableScope))
			{
				variableScope2 = new LocalVariables.VariableScope(localVariable, start, variableScope);
				if (variableScope.ChildScopes == null)
				{
					variableScope.ChildScopes = new List<LocalVariables.VariableScope>();
				}
				variableScope.ChildScopes.Add(variableScope2);
			}
			else
			{
				variableScope2 = new LocalVariables.VariableScope(localVariable, start, null);
			}
			this._variables[variable] = variableScope2;
			return new LocalDefinition(localVariable.Index, variable);
		}

		// Token: 0x060049F1 RID: 18929 RVA: 0x001856E8 File Offset: 0x001838E8
		public void UndefineLocal(LocalDefinition definition, int end)
		{
			LocalVariables.VariableScope variableScope = this._variables[definition.Parameter];
			variableScope.Stop = end;
			if (variableScope.Parent != null)
			{
				this._variables[definition.Parameter] = variableScope.Parent;
			}
			else
			{
				this._variables.Remove(definition.Parameter);
			}
			this._localCount--;
		}

		// Token: 0x060049F2 RID: 18930 RVA: 0x00185754 File Offset: 0x00183954
		internal void Box(ParameterExpression variable, InstructionList instructions)
		{
			LocalVariables.VariableScope variableScope = this._variables[variable];
			LocalVariable variable2 = variableScope.Variable;
			this._variables[variable].Variable.IsBoxed = true;
			int num = 0;
			int num2 = variableScope.Start;
			while (num2 < variableScope.Stop && num2 < instructions.Count)
			{
				if (variableScope.ChildScopes != null && variableScope.ChildScopes[num].Start == num2)
				{
					LocalVariables.VariableScope variableScope2 = variableScope.ChildScopes[num];
					num2 = variableScope2.Stop;
					num++;
				}
				else
				{
					instructions.SwitchToBoxed(variable2.Index, num2);
				}
				num2++;
			}
		}

		// Token: 0x17000F91 RID: 3985
		// (get) Token: 0x060049F3 RID: 18931 RVA: 0x001857F2 File Offset: 0x001839F2
		public int LocalCount
		{
			get
			{
				return this._maxLocalCount;
			}
		}

		// Token: 0x060049F4 RID: 18932 RVA: 0x001857FC File Offset: 0x001839FC
		public int GetOrDefineLocal(ParameterExpression var)
		{
			int localIndex = this.GetLocalIndex(var);
			if (localIndex == -1)
			{
				return this.DefineLocal(var, 0).Index;
			}
			return localIndex;
		}

		// Token: 0x060049F5 RID: 18933 RVA: 0x00185828 File Offset: 0x00183A28
		public int GetLocalIndex(ParameterExpression var)
		{
			LocalVariables.VariableScope variableScope;
			if (!this._variables.TryGetValue(var, out variableScope))
			{
				return -1;
			}
			return variableScope.Variable.Index;
		}

		// Token: 0x060049F6 RID: 18934 RVA: 0x00185854 File Offset: 0x00183A54
		public bool TryGetLocalOrClosure(ParameterExpression var, out LocalVariable local)
		{
			LocalVariables.VariableScope variableScope;
			if (this._variables.TryGetValue(var, out variableScope))
			{
				local = variableScope.Variable;
				return true;
			}
			if (this._closureVariables != null && this._closureVariables.TryGetValue(var, out local))
			{
				return true;
			}
			local = null;
			return false;
		}

		// Token: 0x060049F7 RID: 18935 RVA: 0x00185898 File Offset: 0x00183A98
		internal Dictionary<ParameterExpression, LocalVariable> CopyLocals()
		{
			Dictionary<ParameterExpression, LocalVariable> dictionary = new Dictionary<ParameterExpression, LocalVariable>(this._variables.Count);
			foreach (KeyValuePair<ParameterExpression, LocalVariables.VariableScope> keyValuePair in this._variables)
			{
				dictionary[keyValuePair.Key] = keyValuePair.Value.Variable;
			}
			return dictionary;
		}

		// Token: 0x060049F8 RID: 18936 RVA: 0x0018590C File Offset: 0x00183B0C
		internal bool ContainsVariable(ParameterExpression variable)
		{
			return this._variables.ContainsKey(variable);
		}

		// Token: 0x17000F92 RID: 3986
		// (get) Token: 0x060049F9 RID: 18937 RVA: 0x0018591A File Offset: 0x00183B1A
		internal Dictionary<ParameterExpression, LocalVariable> ClosureVariables
		{
			get
			{
				return this._closureVariables;
			}
		}

		// Token: 0x060049FA RID: 18938 RVA: 0x00185924 File Offset: 0x00183B24
		internal LocalVariable AddClosureVariable(ParameterExpression variable)
		{
			if (this._closureVariables == null)
			{
				this._closureVariables = new Dictionary<ParameterExpression, LocalVariable>();
			}
			LocalVariable localVariable = new LocalVariable(this._closureVariables.Count, true, false);
			this._closureVariables.Add(variable, localVariable);
			return localVariable;
		}

		// Token: 0x040023D5 RID: 9173
		private readonly HybridReferenceDictionary<ParameterExpression, LocalVariables.VariableScope> _variables = new HybridReferenceDictionary<ParameterExpression, LocalVariables.VariableScope>();

		// Token: 0x040023D6 RID: 9174
		private Dictionary<ParameterExpression, LocalVariable> _closureVariables;

		// Token: 0x040023D7 RID: 9175
		private int _localCount;

		// Token: 0x040023D8 RID: 9176
		private int _maxLocalCount;

		// Token: 0x02000708 RID: 1800
		private sealed class VariableScope
		{
			// Token: 0x060049FB RID: 18939 RVA: 0x00185965 File Offset: 0x00183B65
			public VariableScope(LocalVariable variable, int start, LocalVariables.VariableScope parent)
			{
				this.Variable = variable;
				this.Start = start;
				this.Parent = parent;
			}

			// Token: 0x040023D9 RID: 9177
			public readonly int Start;

			// Token: 0x040023DA RID: 9178
			public int Stop = int.MaxValue;

			// Token: 0x040023DB RID: 9179
			public readonly LocalVariable Variable;

			// Token: 0x040023DC RID: 9180
			public readonly LocalVariables.VariableScope Parent;

			// Token: 0x040023DD RID: 9181
			public List<LocalVariables.VariableScope> ChildScopes;
		}
	}
}
