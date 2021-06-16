using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Management.Automation.Language
{
	// Token: 0x0200099B RID: 2459
	internal sealed class PseudoBindingInfo
	{
		// Token: 0x06005A9D RID: 23197 RVA: 0x001E6D94 File Offset: 0x001E4F94
		internal PseudoBindingInfo(CommandInfo commandInfo, uint validParameterSetsFlags, uint defaultParameterSetFalg, Dictionary<string, MergedCompiledCommandParameter> boundParameters, List<MergedCompiledCommandParameter> unboundParameters, Dictionary<string, AstParameterArgumentPair> boundArguments, Collection<string> boundPositionalParameter, Collection<AstParameterArgumentPair> allParsedArguments, Collection<CommandParameterAst> parametersNotFound, Collection<CommandParameterAst> ambiguousParameters, Dictionary<CommandParameterAst, ParameterBindingException> bindingExceptions, Collection<AstParameterArgumentPair> duplicateParameters, Collection<AstParameterArgumentPair> unboundArguments)
		{
			this._commandInfo = commandInfo;
			this._infoType = PseudoBindingInfoType.PseudoBindingSucceed;
			this._validParameterSetsFlags = validParameterSetsFlags;
			this._defaultParameterSetFlag = defaultParameterSetFalg;
			this._boundParameters = boundParameters;
			this._unboundParameters = unboundParameters;
			this._boundArguments = boundArguments;
			this._boundPositionalParameter = boundPositionalParameter;
			this._allParsedArguments = allParsedArguments;
			this._parametersNotFound = parametersNotFound;
			this._ambiguousParameters = ambiguousParameters;
			this._bindingExceptions = bindingExceptions;
			this._duplicateParameters = duplicateParameters;
			this._unboundArguments = unboundArguments;
		}

		// Token: 0x06005A9E RID: 23198 RVA: 0x001E6E13 File Offset: 0x001E5013
		internal PseudoBindingInfo(CommandInfo commandInfo, uint defaultParameterSetFlag, Collection<AstParameterArgumentPair> allParsedArguments, List<MergedCompiledCommandParameter> unboundParameters)
		{
			this._commandInfo = commandInfo;
			this._infoType = PseudoBindingInfoType.PseudoBindingFail;
			this._defaultParameterSetFlag = defaultParameterSetFlag;
			this._allParsedArguments = allParsedArguments;
			this._unboundParameters = unboundParameters;
		}

		// Token: 0x1700121D RID: 4637
		// (get) Token: 0x06005A9F RID: 23199 RVA: 0x001E6E3F File Offset: 0x001E503F
		internal string CommandName
		{
			get
			{
				return this._commandInfo.Name;
			}
		}

		// Token: 0x1700121E RID: 4638
		// (get) Token: 0x06005AA0 RID: 23200 RVA: 0x001E6E4C File Offset: 0x001E504C
		internal CommandInfo CommandInfo
		{
			get
			{
				return this._commandInfo;
			}
		}

		// Token: 0x1700121F RID: 4639
		// (get) Token: 0x06005AA1 RID: 23201 RVA: 0x001E6E54 File Offset: 0x001E5054
		internal PseudoBindingInfoType InfoType
		{
			get
			{
				return this._infoType;
			}
		}

		// Token: 0x17001220 RID: 4640
		// (get) Token: 0x06005AA2 RID: 23202 RVA: 0x001E6E5C File Offset: 0x001E505C
		internal uint ValidParameterSetsFlags
		{
			get
			{
				return this._validParameterSetsFlags;
			}
		}

		// Token: 0x17001221 RID: 4641
		// (get) Token: 0x06005AA3 RID: 23203 RVA: 0x001E6E64 File Offset: 0x001E5064
		internal uint DefaultParameterSetFlag
		{
			get
			{
				return this._defaultParameterSetFlag;
			}
		}

		// Token: 0x17001222 RID: 4642
		// (get) Token: 0x06005AA4 RID: 23204 RVA: 0x001E6E6C File Offset: 0x001E506C
		internal Dictionary<string, MergedCompiledCommandParameter> BoundParameters
		{
			get
			{
				return this._boundParameters;
			}
		}

		// Token: 0x17001223 RID: 4643
		// (get) Token: 0x06005AA5 RID: 23205 RVA: 0x001E6E74 File Offset: 0x001E5074
		internal List<MergedCompiledCommandParameter> UnboundParameters
		{
			get
			{
				return this._unboundParameters;
			}
		}

		// Token: 0x17001224 RID: 4644
		// (get) Token: 0x06005AA6 RID: 23206 RVA: 0x001E6E7C File Offset: 0x001E507C
		internal Dictionary<string, AstParameterArgumentPair> BoundArguments
		{
			get
			{
				return this._boundArguments;
			}
		}

		// Token: 0x17001225 RID: 4645
		// (get) Token: 0x06005AA7 RID: 23207 RVA: 0x001E6E84 File Offset: 0x001E5084
		internal Collection<AstParameterArgumentPair> UnboundArguments
		{
			get
			{
				return this._unboundArguments;
			}
		}

		// Token: 0x17001226 RID: 4646
		// (get) Token: 0x06005AA8 RID: 23208 RVA: 0x001E6E8C File Offset: 0x001E508C
		internal Collection<string> BoundPositionalParameter
		{
			get
			{
				return this._boundPositionalParameter;
			}
		}

		// Token: 0x17001227 RID: 4647
		// (get) Token: 0x06005AA9 RID: 23209 RVA: 0x001E6E94 File Offset: 0x001E5094
		internal Collection<AstParameterArgumentPair> AllParsedArguments
		{
			get
			{
				return this._allParsedArguments;
			}
		}

		// Token: 0x17001228 RID: 4648
		// (get) Token: 0x06005AAA RID: 23210 RVA: 0x001E6E9C File Offset: 0x001E509C
		internal Collection<CommandParameterAst> ParametersNotFound
		{
			get
			{
				return this._parametersNotFound;
			}
		}

		// Token: 0x17001229 RID: 4649
		// (get) Token: 0x06005AAB RID: 23211 RVA: 0x001E6EA4 File Offset: 0x001E50A4
		internal Collection<CommandParameterAst> AmbiguousParameters
		{
			get
			{
				return this._ambiguousParameters;
			}
		}

		// Token: 0x1700122A RID: 4650
		// (get) Token: 0x06005AAC RID: 23212 RVA: 0x001E6EAC File Offset: 0x001E50AC
		internal Dictionary<CommandParameterAst, ParameterBindingException> BindingExceptions
		{
			get
			{
				return this._bindingExceptions;
			}
		}

		// Token: 0x1700122B RID: 4651
		// (get) Token: 0x06005AAD RID: 23213 RVA: 0x001E6EB4 File Offset: 0x001E50B4
		internal Collection<AstParameterArgumentPair> DuplicateParameters
		{
			get
			{
				return this._duplicateParameters;
			}
		}

		// Token: 0x0400306F RID: 12399
		private readonly CommandInfo _commandInfo;

		// Token: 0x04003070 RID: 12400
		private readonly PseudoBindingInfoType _infoType;

		// Token: 0x04003071 RID: 12401
		private readonly uint _validParameterSetsFlags;

		// Token: 0x04003072 RID: 12402
		private readonly uint _defaultParameterSetFlag;

		// Token: 0x04003073 RID: 12403
		private readonly Dictionary<string, MergedCompiledCommandParameter> _boundParameters;

		// Token: 0x04003074 RID: 12404
		private readonly Dictionary<string, AstParameterArgumentPair> _boundArguments;

		// Token: 0x04003075 RID: 12405
		private readonly List<MergedCompiledCommandParameter> _unboundParameters;

		// Token: 0x04003076 RID: 12406
		private readonly Collection<string> _boundPositionalParameter;

		// Token: 0x04003077 RID: 12407
		private readonly Collection<AstParameterArgumentPair> _allParsedArguments;

		// Token: 0x04003078 RID: 12408
		private readonly Collection<CommandParameterAst> _parametersNotFound;

		// Token: 0x04003079 RID: 12409
		private readonly Collection<CommandParameterAst> _ambiguousParameters;

		// Token: 0x0400307A RID: 12410
		private readonly Dictionary<CommandParameterAst, ParameterBindingException> _bindingExceptions;

		// Token: 0x0400307B RID: 12411
		private readonly Collection<AstParameterArgumentPair> _duplicateParameters;

		// Token: 0x0400307C RID: 12412
		private readonly Collection<AstParameterArgumentPair> _unboundArguments;
	}
}
