using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;

namespace System.Management.Automation
{
	// Token: 0x020000F9 RID: 249
	public sealed class CallStackFrame
	{
		// Token: 0x06000DD2 RID: 3538 RVA: 0x0004B780 File Offset: 0x00049980
		public CallStackFrame(InvocationInfo invocationInfo) : this(null, invocationInfo)
		{
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0004B78C File Offset: 0x0004998C
		internal CallStackFrame(FunctionContext functionContext, InvocationInfo invocationInfo)
		{
			if (invocationInfo == null)
			{
				throw new PSArgumentNullException("invocationInfo");
			}
			if (functionContext != null)
			{
				this.InvocationInfo = invocationInfo;
				this._functionContext = functionContext;
				this.Position = functionContext.CurrentPosition;
				return;
			}
			this.InvocationInfo = invocationInfo;
			this.Position = invocationInfo.ScriptPosition;
			this._functionContext = new FunctionContext();
			this._functionContext._functionName = invocationInfo.ScriptName;
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000DD4 RID: 3540 RVA: 0x0004B7FA File Offset: 0x000499FA
		public string ScriptName
		{
			get
			{
				return this.Position.File;
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000DD5 RID: 3541 RVA: 0x0004B807 File Offset: 0x00049A07
		public int ScriptLineNumber
		{
			get
			{
				return this.Position.StartLineNumber;
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000DD6 RID: 3542 RVA: 0x0004B814 File Offset: 0x00049A14
		// (set) Token: 0x06000DD7 RID: 3543 RVA: 0x0004B81C File Offset: 0x00049A1C
		public InvocationInfo InvocationInfo { get; private set; }

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000DD8 RID: 3544 RVA: 0x0004B825 File Offset: 0x00049A25
		// (set) Token: 0x06000DD9 RID: 3545 RVA: 0x0004B82D File Offset: 0x00049A2D
		public IScriptExtent Position { get; private set; }

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000DDA RID: 3546 RVA: 0x0004B836 File Offset: 0x00049A36
		public string FunctionName
		{
			get
			{
				return this._functionContext._functionName;
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000DDB RID: 3547 RVA: 0x0004B843 File Offset: 0x00049A43
		internal FunctionContext FunctionContext
		{
			get
			{
				return this._functionContext;
			}
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x0004B84B File Offset: 0x00049A4B
		public string GetScriptLocation()
		{
			if (string.IsNullOrEmpty(this.ScriptName))
			{
				return DebuggerStrings.NoFile;
			}
			return StringUtil.Format(DebuggerStrings.LocationFormat, Path.GetFileName(this.ScriptName), this.ScriptLineNumber);
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x0004B890 File Offset: 0x00049A90
		public Dictionary<string, PSVariable> GetFrameVariables()
		{
			Dictionary<string, PSVariable> result = new Dictionary<string, PSVariable>(StringComparer.OrdinalIgnoreCase);
			if (this._functionContext._executionContext == null)
			{
				return result;
			}
			SessionStateScope sessionStateScope = this._functionContext._executionContext.EngineSessionState.CurrentScope;
			while (sessionStateScope != null && sessionStateScope.LocalsTuple != this._functionContext._localsTuple)
			{
				if (sessionStateScope.DottedScopes != null)
				{
					if ((from s in sessionStateScope.DottedScopes
					where s == this._functionContext._localsTuple
					select s).Any<MutableTuple>())
					{
						MutableTuple[] array = sessionStateScope.DottedScopes.ToArray();
						for (int i = 0; i < array.Length; i++)
						{
							if (array[i] == this._functionContext._localsTuple)
							{
								IL_B3:
								while (i < array.Length)
								{
									array[i].GetVariableTable(result, true);
									i++;
								}
								goto IL_C8;
							}
						}
						goto IL_B3;
					}
				}
				sessionStateScope = sessionStateScope.Parent;
			}
			IL_C8:
			this._functionContext._localsTuple.GetVariableTable(result, true);
			return result;
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0004B978 File Offset: 0x00049B78
		public override string ToString()
		{
			return StringUtil.Format(DebuggerStrings.StackTraceFormat, new object[]
			{
				this.FunctionName,
				this.ScriptName ?? DebuggerStrings.NoFile,
				this.ScriptLineNumber
			});
		}

		// Token: 0x0400062C RID: 1580
		private readonly FunctionContext _functionContext;
	}
}
