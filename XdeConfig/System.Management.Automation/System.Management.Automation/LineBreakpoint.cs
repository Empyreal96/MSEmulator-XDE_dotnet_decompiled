using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;

namespace System.Management.Automation
{
	// Token: 0x020000E1 RID: 225
	public class LineBreakpoint : Breakpoint
	{
		// Token: 0x06000C9D RID: 3229 RVA: 0x00045FA4 File Offset: 0x000441A4
		internal LineBreakpoint(string script, int line, ScriptBlock action) : base(script, action)
		{
			this.Line = line;
			this.Column = 0;
			this.SequencePointIndex = -1;
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x00045FC3 File Offset: 0x000441C3
		internal LineBreakpoint(string script, int line, int column, ScriptBlock action) : base(script, action)
		{
			this.Line = line;
			this.Column = column;
			this.SequencePointIndex = -1;
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x00045FE3 File Offset: 0x000441E3
		internal LineBreakpoint(string script, int line, int column, ScriptBlock action, int id) : base(script, action, id)
		{
			this.Line = line;
			this.Column = column;
			this.SequencePointIndex = -1;
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x00046005 File Offset: 0x00044205
		// (set) Token: 0x06000CA1 RID: 3233 RVA: 0x0004600D File Offset: 0x0004420D
		public int Column { get; private set; }

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x00046016 File Offset: 0x00044216
		// (set) Token: 0x06000CA3 RID: 3235 RVA: 0x0004601E File Offset: 0x0004421E
		public int Line { get; private set; }

		// Token: 0x06000CA4 RID: 3236 RVA: 0x00046028 File Offset: 0x00044228
		public override string ToString()
		{
			if (this.Column != 0)
			{
				return StringUtil.Format(DebuggerStrings.StatementBreakpointString, new object[]
				{
					base.Script,
					this.Line,
					this.Column
				});
			}
			return StringUtil.Format(DebuggerStrings.LineBreakpointString, base.Script, this.Line);
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x00046090 File Offset: 0x00044290
		// (set) Token: 0x06000CA6 RID: 3238 RVA: 0x00046098 File Offset: 0x00044298
		internal int SequencePointIndex { get; set; }

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x000460A1 File Offset: 0x000442A1
		// (set) Token: 0x06000CA8 RID: 3240 RVA: 0x000460A9 File Offset: 0x000442A9
		internal IScriptExtent[] SequencePoints { get; set; }

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x000460B2 File Offset: 0x000442B2
		// (set) Token: 0x06000CAA RID: 3242 RVA: 0x000460BA File Offset: 0x000442BA
		internal BitArray BreakpointBitArray { get; set; }

		// Token: 0x06000CAB RID: 3243 RVA: 0x000460C4 File Offset: 0x000442C4
		internal bool TrySetBreakpoint(string scriptFile, FunctionContext functionContext)
		{
			if (!scriptFile.Equals(base.Script, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			ScriptBlock scriptBlock = functionContext._scriptBlock;
			bool flag;
			if (scriptBlock != null)
			{
				Ast ast = scriptBlock.Ast;
				if (!ast.Extent.ContainsLineAndColumn(this.Line, this.Column))
				{
					return false;
				}
				IScriptExtent[] sequencePoints = functionContext._sequencePoints;
				if (sequencePoints.Length == 1 && sequencePoints[0] == scriptBlock.Ast.Extent)
				{
					return false;
				}
				flag = LineBreakpoint.CheckBreakpointInScript.IsInNestedScriptBlock(((IParameterMetadataProvider)ast).Body, this);
			}
			else
			{
				flag = false;
			}
			int sequencePointIndex;
			IScriptExtent scriptExtent = LineBreakpoint.FindSequencePoint(functionContext, this.Line, this.Column, out sequencePointIndex);
			if (scriptExtent != null && (!flag || (scriptExtent.StartLineNumber == this.Line && this.Column == 0)))
			{
				this.SetBreakpoint(functionContext, sequencePointIndex);
				return true;
			}
			if (flag)
			{
				return false;
			}
			if (scriptBlock != null)
			{
				Ast ast2 = scriptBlock.Ast;
				ScriptBlockAst body = ((IParameterMetadataProvider)ast2).Body;
				if ((body.DynamicParamBlock == null || body.DynamicParamBlock.Extent.IsAfter(this.Line, this.Column)) && (body.BeginBlock == null || body.BeginBlock.Extent.IsAfter(this.Line, this.Column)) && (body.ProcessBlock == null || body.ProcessBlock.Extent.IsAfter(this.Line, this.Column)) && (body.EndBlock == null || body.EndBlock.Extent.IsAfter(this.Line, this.Column)))
				{
					this.SetBreakpoint(functionContext, 0);
					return true;
				}
			}
			if (this.Column == 0 && LineBreakpoint.FindSequencePoint(functionContext, this.Line + 1, 0, out sequencePointIndex) != null)
			{
				this.SetBreakpoint(functionContext, sequencePointIndex);
				return true;
			}
			return false;
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x00046278 File Offset: 0x00044478
		private static IScriptExtent FindSequencePoint(FunctionContext functionContext, int line, int column, out int sequencePointIndex)
		{
			IScriptExtent[] sequencePoints = functionContext._sequencePoints;
			for (int i = 0; i < sequencePoints.Length; i++)
			{
				IScriptExtent scriptExtent = sequencePoints[i];
				if (scriptExtent.ContainsLineAndColumn(line, column))
				{
					sequencePointIndex = i;
					return scriptExtent;
				}
			}
			sequencePointIndex = -1;
			return null;
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x000462B1 File Offset: 0x000444B1
		private void SetBreakpoint(FunctionContext functionContext, int sequencePointIndex)
		{
			this.BreakpointBitArray = functionContext._breakPoints;
			this.SequencePoints = functionContext._sequencePoints;
			this.SequencePointIndex = sequencePointIndex;
			this.BreakpointBitArray.Set(this.SequencePointIndex, true);
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x000462F8 File Offset: 0x000444F8
		internal override void RemoveSelf(ScriptDebugger debugger)
		{
			if (this.SequencePoints != null)
			{
				List<LineBreakpoint> boundBreakpoints = debugger.GetBoundBreakpoints(this.SequencePoints);
				if (boundBreakpoints != null)
				{
					boundBreakpoints.Remove(this);
					if (boundBreakpoints.All((LineBreakpoint breakpoint) => breakpoint.SequencePointIndex != this.SequencePointIndex))
					{
						this.BreakpointBitArray.Set(this.SequencePointIndex, false);
					}
				}
			}
			debugger.RemoveLineBreakpoint(this);
		}

		// Token: 0x020000E2 RID: 226
		private class CheckBreakpointInScript : AstVisitor
		{
			// Token: 0x06000CB0 RID: 3248 RVA: 0x0004635C File Offset: 0x0004455C
			public static bool IsInNestedScriptBlock(Ast ast, LineBreakpoint breakpoint)
			{
				LineBreakpoint.CheckBreakpointInScript checkBreakpointInScript = new LineBreakpoint.CheckBreakpointInScript
				{
					_breakpoint = breakpoint
				};
				ast.InternalVisit(checkBreakpointInScript);
				return checkBreakpointInScript._result;
			}

			// Token: 0x06000CB1 RID: 3249 RVA: 0x00046386 File Offset: 0x00044586
			public override AstVisitAction VisitFunctionDefinition(FunctionDefinitionAst functionDefinitionAst)
			{
				if (functionDefinitionAst.Extent.ContainsLineAndColumn(this._breakpoint.Line, this._breakpoint.Column))
				{
					this._result = true;
					return AstVisitAction.StopVisit;
				}
				return AstVisitAction.SkipChildren;
			}

			// Token: 0x06000CB2 RID: 3250 RVA: 0x000463B5 File Offset: 0x000445B5
			public override AstVisitAction VisitScriptBlockExpression(ScriptBlockExpressionAst scriptBlockExpressionAst)
			{
				if (scriptBlockExpressionAst.Extent.ContainsLineAndColumn(this._breakpoint.Line, this._breakpoint.Column))
				{
					this._result = true;
					return AstVisitAction.StopVisit;
				}
				return AstVisitAction.SkipChildren;
			}

			// Token: 0x040005A0 RID: 1440
			private LineBreakpoint _breakpoint;

			// Token: 0x040005A1 RID: 1441
			private bool _result;
		}
	}
}
