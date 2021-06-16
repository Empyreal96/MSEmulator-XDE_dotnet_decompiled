using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation.Interpreter;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x020005F3 RID: 1523
	internal class CompiledScriptBlockData
	{
		// Token: 0x06004161 RID: 16737 RVA: 0x0015AD07 File Offset: 0x00158F07
		internal CompiledScriptBlockData(IParameterMetadataProvider ast)
		{
			this._ast = ast;
			this._syncObject = new object();
			this.Id = Guid.NewGuid();
		}

		// Token: 0x06004162 RID: 16738 RVA: 0x0015AD2C File Offset: 0x00158F2C
		internal CompiledScriptBlockData(string scriptText)
		{
			this._scriptText = scriptText;
			this._syncObject = new object();
			this.Id = Guid.NewGuid();
		}

		// Token: 0x06004163 RID: 16739 RVA: 0x0015AD54 File Offset: 0x00158F54
		internal bool Compile(bool optimized)
		{
			if (this._attributes == null)
			{
				this.InitializeMetadata();
			}
			if (optimized && this.NameToIndexMap == null)
			{
				this.CompileOptimized();
			}
			optimized = (optimized && !VariableAnalysis.AnyVariablesCouldBeAllScope(this.NameToIndexMap));
			if (!optimized && !this._compiledUnoptimized)
			{
				this.CompileUnoptimized();
			}
			else if (optimized && !this._compiledOptimized)
			{
				this.CompileOptimized();
			}
			return optimized;
		}

		// Token: 0x06004164 RID: 16740 RVA: 0x0015ADBC File Offset: 0x00158FBC
		private void InitializeMetadata()
		{
			lock (this._syncObject)
			{
				if (this._attributes == null)
				{
					CmdletBindingAttribute cmdletBindingAttribute = null;
					Attribute[] array = this.Ast.GetScriptBlockAttributes().ToArray<Attribute>();
					foreach (Attribute attribute in array)
					{
						if (attribute is CmdletBindingAttribute)
						{
							cmdletBindingAttribute = (cmdletBindingAttribute ?? ((CmdletBindingAttribute)attribute));
						}
						else if (attribute is DebuggerHiddenAttribute)
						{
							this.DebuggerHidden = true;
						}
						else if (attribute is DebuggerStepThroughAttribute || attribute is DebuggerNonUserCodeAttribute)
						{
							this.DebuggerStepThrough = true;
						}
					}
					this._usesCmdletBinding = (cmdletBindingAttribute != null);
					bool automaticPositions = cmdletBindingAttribute == null || cmdletBindingAttribute.PositionalBinding;
					RuntimeDefinedParameterDictionary parameterMetadata = this.Ast.GetParameterMetadata(automaticPositions, ref this._usesCmdletBinding);
					this._attributes = array;
					this._runtimeDefinedParameterDictionary = parameterMetadata;
				}
			}
		}

		// Token: 0x06004165 RID: 16741 RVA: 0x0015AEB4 File Offset: 0x001590B4
		private void CompileUnoptimized()
		{
			lock (this._syncObject)
			{
				if (!this._compiledUnoptimized)
				{
					this.PerformSecurityChecks();
					Compiler compiler = new Compiler();
					compiler.Compile(this, false);
					if (this.UnoptimizedDynamicParamBlockTree != null)
					{
						this.UnoptimizedDynamicParamBlock = this.CompileTree(this.UnoptimizedDynamicParamBlockTree);
					}
					if (this.UnoptimizedBeginBlockTree != null)
					{
						this.UnoptimizedBeginBlock = this.CompileTree(this.UnoptimizedBeginBlockTree);
					}
					if (this.UnoptimizedProcessBlockTree != null)
					{
						this.UnoptimizedProcessBlock = this.CompileTree(this.UnoptimizedProcessBlockTree);
					}
					if (this.UnoptimizedEndBlockTree != null)
					{
						this.UnoptimizedEndBlock = this.CompileTree(this.UnoptimizedEndBlockTree);
					}
					this._compiledUnoptimized = true;
				}
			}
		}

		// Token: 0x06004166 RID: 16742 RVA: 0x0015AF80 File Offset: 0x00159180
		private void CompileOptimized()
		{
			lock (this._syncObject)
			{
				if (!this._compiledOptimized)
				{
					this.PerformSecurityChecks();
					Compiler compiler = new Compiler();
					compiler.Compile(this, true);
					if (this.DynamicParamBlockTree != null)
					{
						this.DynamicParamBlock = this.CompileTree(this.DynamicParamBlockTree);
					}
					if (this.BeginBlockTree != null)
					{
						this.BeginBlock = this.CompileTree(this.BeginBlockTree);
					}
					if (this.ProcessBlockTree != null)
					{
						this.ProcessBlock = this.CompileTree(this.ProcessBlockTree);
					}
					if (this.EndBlockTree != null)
					{
						this.EndBlock = this.CompileTree(this.EndBlockTree);
					}
					this._compiledOptimized = true;
				}
			}
		}

		// Token: 0x06004167 RID: 16743 RVA: 0x0015B04C File Offset: 0x0015924C
		private Action<FunctionContext> CompileTree(Expression<Action<FunctionContext>> lambda)
		{
			if (this.CompileInterpretDecision == CompileInterpretChoice.AlwaysCompile)
			{
				return lambda.Compile();
			}
			int compilationThreshold = (this.CompileInterpretDecision == CompileInterpretChoice.NeverCompile) ? int.MaxValue : -1;
			Delegate @delegate = new LightCompiler(compilationThreshold).CompileTop(lambda).CreateDelegate();
			return (Action<FunctionContext>)@delegate;
		}

		// Token: 0x06004168 RID: 16744 RVA: 0x0015B094 File Offset: 0x00159294
		private void PerformSecurityChecks()
		{
			ScriptBlockAst scriptBlockAst = this.Ast as ScriptBlockAst;
			if (scriptBlockAst == null)
			{
				return;
			}
			IScriptExtent extent = scriptBlockAst.Extent;
			if (AmsiUtils.ScanContent(extent.Text, extent.File) == AmsiUtils.AmsiNativeMethods.AMSI_RESULT.AMSI_RESULT_DETECTED)
			{
				ParseError parseError = new ParseError(extent, "ScriptContainedMaliciousContent", ParserStrings.ScriptContainedMaliciousContent);
				throw new ParseException(new ParseError[]
				{
					parseError
				});
			}
			if (ScriptBlock.CheckSuspiciousContent(scriptBlockAst) != null)
			{
				this.HasSuspiciousContent = true;
			}
		}

		// Token: 0x17000DFB RID: 3579
		// (get) Token: 0x06004169 RID: 16745 RVA: 0x0015B102 File Offset: 0x00159302
		internal IParameterMetadataProvider Ast
		{
			get
			{
				return this._ast ?? this.DelayParseScriptText();
			}
		}

		// Token: 0x0600416A RID: 16746 RVA: 0x0015B114 File Offset: 0x00159314
		private IParameterMetadataProvider DelayParseScriptText()
		{
			IParameterMetadataProvider ast;
			lock (this._syncObject)
			{
				if (this._ast != null)
				{
					ast = this._ast;
				}
				else
				{
					ParseError[] array;
					this._ast = new Parser().Parse(null, this._scriptText, null, out array);
					if (array.Length != 0)
					{
						throw new ParseException(array);
					}
					this._scriptText = null;
					ast = this._ast;
				}
			}
			return ast;
		}

		// Token: 0x17000DFC RID: 3580
		// (get) Token: 0x0600416B RID: 16747 RVA: 0x0015B194 File Offset: 0x00159394
		// (set) Token: 0x0600416C RID: 16748 RVA: 0x0015B19C File Offset: 0x0015939C
		internal Expression<Action<FunctionContext>> UnoptimizedDynamicParamBlockTree { get; set; }

		// Token: 0x17000DFD RID: 3581
		// (get) Token: 0x0600416D RID: 16749 RVA: 0x0015B1A5 File Offset: 0x001593A5
		// (set) Token: 0x0600416E RID: 16750 RVA: 0x0015B1AD File Offset: 0x001593AD
		internal Expression<Action<FunctionContext>> DynamicParamBlockTree { get; set; }

		// Token: 0x17000DFE RID: 3582
		// (get) Token: 0x0600416F RID: 16751 RVA: 0x0015B1B6 File Offset: 0x001593B6
		// (set) Token: 0x06004170 RID: 16752 RVA: 0x0015B1BE File Offset: 0x001593BE
		internal Expression<Action<FunctionContext>> BeginBlockTree { get; set; }

		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x06004171 RID: 16753 RVA: 0x0015B1C7 File Offset: 0x001593C7
		// (set) Token: 0x06004172 RID: 16754 RVA: 0x0015B1CF File Offset: 0x001593CF
		internal Expression<Action<FunctionContext>> UnoptimizedBeginBlockTree { get; set; }

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x06004173 RID: 16755 RVA: 0x0015B1D8 File Offset: 0x001593D8
		// (set) Token: 0x06004174 RID: 16756 RVA: 0x0015B1E0 File Offset: 0x001593E0
		internal Expression<Action<FunctionContext>> ProcessBlockTree { get; set; }

		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x06004175 RID: 16757 RVA: 0x0015B1E9 File Offset: 0x001593E9
		// (set) Token: 0x06004176 RID: 16758 RVA: 0x0015B1F1 File Offset: 0x001593F1
		internal Expression<Action<FunctionContext>> UnoptimizedProcessBlockTree { get; set; }

		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x06004177 RID: 16759 RVA: 0x0015B1FA File Offset: 0x001593FA
		// (set) Token: 0x06004178 RID: 16760 RVA: 0x0015B202 File Offset: 0x00159402
		internal Expression<Action<FunctionContext>> EndBlockTree { get; set; }

		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x06004179 RID: 16761 RVA: 0x0015B20B File Offset: 0x0015940B
		// (set) Token: 0x0600417A RID: 16762 RVA: 0x0015B213 File Offset: 0x00159413
		internal Expression<Action<FunctionContext>> UnoptimizedEndBlockTree { get; set; }

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x0600417B RID: 16763 RVA: 0x0015B21C File Offset: 0x0015941C
		// (set) Token: 0x0600417C RID: 16764 RVA: 0x0015B224 File Offset: 0x00159424
		internal CompileInterpretChoice CompileInterpretDecision { get; set; }

		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x0600417D RID: 16765 RVA: 0x0015B22D File Offset: 0x0015942D
		// (set) Token: 0x0600417E RID: 16766 RVA: 0x0015B235 File Offset: 0x00159435
		internal Type LocalsMutableTupleType { get; set; }

		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x0600417F RID: 16767 RVA: 0x0015B23E File Offset: 0x0015943E
		// (set) Token: 0x06004180 RID: 16768 RVA: 0x0015B246 File Offset: 0x00159446
		internal Type UnoptimizedLocalsMutableTupleType { get; set; }

		// Token: 0x17000E07 RID: 3591
		// (get) Token: 0x06004181 RID: 16769 RVA: 0x0015B24F File Offset: 0x0015944F
		// (set) Token: 0x06004182 RID: 16770 RVA: 0x0015B257 File Offset: 0x00159457
		internal Dictionary<string, int> NameToIndexMap { get; set; }

		// Token: 0x17000E08 RID: 3592
		// (get) Token: 0x06004183 RID: 16771 RVA: 0x0015B260 File Offset: 0x00159460
		// (set) Token: 0x06004184 RID: 16772 RVA: 0x0015B268 File Offset: 0x00159468
		internal Action<FunctionContext> DynamicParamBlock { get; private set; }

		// Token: 0x17000E09 RID: 3593
		// (get) Token: 0x06004185 RID: 16773 RVA: 0x0015B271 File Offset: 0x00159471
		// (set) Token: 0x06004186 RID: 16774 RVA: 0x0015B279 File Offset: 0x00159479
		internal Action<FunctionContext> UnoptimizedDynamicParamBlock { get; private set; }

		// Token: 0x17000E0A RID: 3594
		// (get) Token: 0x06004187 RID: 16775 RVA: 0x0015B282 File Offset: 0x00159482
		// (set) Token: 0x06004188 RID: 16776 RVA: 0x0015B28A File Offset: 0x0015948A
		internal Action<FunctionContext> BeginBlock { get; private set; }

		// Token: 0x17000E0B RID: 3595
		// (get) Token: 0x06004189 RID: 16777 RVA: 0x0015B293 File Offset: 0x00159493
		// (set) Token: 0x0600418A RID: 16778 RVA: 0x0015B29B File Offset: 0x0015949B
		internal Action<FunctionContext> UnoptimizedBeginBlock { get; private set; }

		// Token: 0x17000E0C RID: 3596
		// (get) Token: 0x0600418B RID: 16779 RVA: 0x0015B2A4 File Offset: 0x001594A4
		// (set) Token: 0x0600418C RID: 16780 RVA: 0x0015B2AC File Offset: 0x001594AC
		internal Action<FunctionContext> ProcessBlock { get; private set; }

		// Token: 0x17000E0D RID: 3597
		// (get) Token: 0x0600418D RID: 16781 RVA: 0x0015B2B5 File Offset: 0x001594B5
		// (set) Token: 0x0600418E RID: 16782 RVA: 0x0015B2BD File Offset: 0x001594BD
		internal Action<FunctionContext> UnoptimizedProcessBlock { get; private set; }

		// Token: 0x17000E0E RID: 3598
		// (get) Token: 0x0600418F RID: 16783 RVA: 0x0015B2C6 File Offset: 0x001594C6
		// (set) Token: 0x06004190 RID: 16784 RVA: 0x0015B2CE File Offset: 0x001594CE
		internal Action<FunctionContext> EndBlock { get; private set; }

		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x06004191 RID: 16785 RVA: 0x0015B2D7 File Offset: 0x001594D7
		// (set) Token: 0x06004192 RID: 16786 RVA: 0x0015B2DF File Offset: 0x001594DF
		internal Action<FunctionContext> UnoptimizedEndBlock { get; private set; }

		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x06004193 RID: 16787 RVA: 0x0015B2E8 File Offset: 0x001594E8
		// (set) Token: 0x06004194 RID: 16788 RVA: 0x0015B2F0 File Offset: 0x001594F0
		internal IScriptExtent[] SequencePoints { get; set; }

		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x06004195 RID: 16789 RVA: 0x0015B2F9 File Offset: 0x001594F9
		// (set) Token: 0x06004196 RID: 16790 RVA: 0x0015B301 File Offset: 0x00159501
		internal bool DebuggerHidden { get; set; }

		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x06004197 RID: 16791 RVA: 0x0015B30A File Offset: 0x0015950A
		// (set) Token: 0x06004198 RID: 16792 RVA: 0x0015B312 File Offset: 0x00159512
		internal bool DebuggerStepThrough { get; set; }

		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x06004199 RID: 16793 RVA: 0x0015B31B File Offset: 0x0015951B
		// (set) Token: 0x0600419A RID: 16794 RVA: 0x0015B323 File Offset: 0x00159523
		internal Guid Id { get; private set; }

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x0600419B RID: 16795 RVA: 0x0015B32C File Offset: 0x0015952C
		// (set) Token: 0x0600419C RID: 16796 RVA: 0x0015B334 File Offset: 0x00159534
		internal bool HasLogged { get; set; }

		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x0600419D RID: 16797 RVA: 0x0015B33D File Offset: 0x0015953D
		// (set) Token: 0x0600419E RID: 16798 RVA: 0x0015B345 File Offset: 0x00159545
		internal bool HasSuspiciousContent
		{
			get
			{
				return this._hasSuspicousContent;
			}
			set
			{
				this._hasSuspicousContent = value;
			}
		}

		// Token: 0x0600419F RID: 16799 RVA: 0x0015B34E File Offset: 0x0015954E
		internal List<Attribute> GetAttributes()
		{
			if (this._attributes == null)
			{
				this.InitializeMetadata();
			}
			return this._attributes.ToList<Attribute>();
		}

		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x060041A0 RID: 16800 RVA: 0x0015B369 File Offset: 0x00159569
		internal bool UsesCmdletBinding
		{
			get
			{
				if (this._attributes != null)
				{
					return this._usesCmdletBinding;
				}
				return this.Ast.UsesCmdletBinding();
			}
		}

		// Token: 0x17000E17 RID: 3607
		// (get) Token: 0x060041A1 RID: 16801 RVA: 0x0015B385 File Offset: 0x00159585
		internal RuntimeDefinedParameterDictionary RuntimeDefinedParameters
		{
			get
			{
				if (this._runtimeDefinedParameterDictionary == null)
				{
					this.InitializeMetadata();
				}
				return this._runtimeDefinedParameterDictionary;
			}
		}

		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x060041A2 RID: 16802 RVA: 0x0015B3A8 File Offset: 0x001595A8
		internal CmdletBindingAttribute CmdletBindingAttribute
		{
			get
			{
				if (this._runtimeDefinedParameterDictionary == null)
				{
					this.InitializeMetadata();
				}
				if (!this._usesCmdletBinding)
				{
					return null;
				}
				return (CmdletBindingAttribute)this._attributes.FirstOrDefault((Attribute attr) => attr is CmdletBindingAttribute);
			}
		}

		// Token: 0x17000E19 RID: 3609
		// (get) Token: 0x060041A3 RID: 16803 RVA: 0x0015B405 File Offset: 0x00159605
		internal AliasAttribute AliasAttribute
		{
			get
			{
				return (AliasAttribute)this.Ast.GetScriptBlockAttributes().FirstOrDefault((Attribute attr) => attr is AliasAttribute);
			}
		}

		// Token: 0x17000E1A RID: 3610
		// (get) Token: 0x060041A4 RID: 16804 RVA: 0x0015B444 File Offset: 0x00159644
		internal ObsoleteAttribute ObsoleteAttribute
		{
			get
			{
				if (this._runtimeDefinedParameterDictionary == null)
				{
					this.InitializeMetadata();
				}
				return (ObsoleteAttribute)this._attributes.FirstOrDefault((Attribute attr) => attr is ObsoleteAttribute);
			}
		}

		// Token: 0x060041A5 RID: 16805 RVA: 0x0015B484 File Offset: 0x00159684
		public MergedCommandParameterMetadata GetParameterMetadata(ScriptBlock scriptBlock)
		{
			if (this._parameterMetadata == null)
			{
				lock (this._syncObject)
				{
					if (this._parameterMetadata == null)
					{
						CommandMetadata commandMetadata = new CommandMetadata(scriptBlock, "", LocalPipeline.GetExecutionContextFromTLS());
						this._parameterMetadata = commandMetadata.StaticCommandParameterMetadata;
					}
				}
			}
			return this._parameterMetadata;
		}

		// Token: 0x060041A6 RID: 16806 RVA: 0x0015B4F4 File Offset: 0x001596F4
		public override string ToString()
		{
			if (this._scriptText != null)
			{
				return this._scriptText;
			}
			ScriptBlockAst scriptBlockAst = this._ast as ScriptBlockAst;
			if (scriptBlockAst != null)
			{
				return scriptBlockAst.ToStringForSerialization();
			}
			FunctionDefinitionAst functionDefinitionAst = (FunctionDefinitionAst)this._ast;
			if (functionDefinitionAst.Parameters == null)
			{
				return functionDefinitionAst.Body.ToStringForSerialization();
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(functionDefinitionAst.GetParamTextFromParameterList(null));
			stringBuilder.Append(functionDefinitionAst.Body.ToStringForSerialization());
			return stringBuilder.ToString();
		}

		// Token: 0x040020C4 RID: 8388
		private string _scriptText;

		// Token: 0x040020C5 RID: 8389
		private IParameterMetadataProvider _ast;

		// Token: 0x040020C6 RID: 8390
		private readonly object _syncObject;

		// Token: 0x040020C7 RID: 8391
		private RuntimeDefinedParameterDictionary _runtimeDefinedParameterDictionary;

		// Token: 0x040020C8 RID: 8392
		private Attribute[] _attributes;

		// Token: 0x040020C9 RID: 8393
		private bool _usesCmdletBinding;

		// Token: 0x040020CA RID: 8394
		private bool _compiledOptimized;

		// Token: 0x040020CB RID: 8395
		private bool _compiledUnoptimized;

		// Token: 0x040020CC RID: 8396
		private bool _hasSuspicousContent;

		// Token: 0x040020CD RID: 8397
		private MergedCommandParameterMetadata _parameterMetadata;
	}
}
