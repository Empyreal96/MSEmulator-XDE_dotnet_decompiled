using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Text;
using Microsoft.PowerShell;

namespace System.Management.Automation.Language
{
	// Token: 0x0200099C RID: 2460
	internal class PseudoParameterBinder
	{
		// Token: 0x06005AAE RID: 23214 RVA: 0x001E6EBC File Offset: 0x001E50BC
		internal PseudoBindingInfo DoPseudoParameterBinding(CommandAst command, Type pipeArgumentType, CommandParameterAst paramAstAtCursor, PseudoParameterBinder.BindingType bindingType)
		{
			if (command == null)
			{
				throw PSTraceSource.NewArgumentNullException("command");
			}
			this.InitializeMembers();
			this._commandAst = command;
			this._commandElements = command.CommandElements;
			Collection<AstParameterArgumentPair> unboundArguments = new Collection<AstParameterArgumentPair>();
			ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
			this.SetTemporaryDefaultHost(executionContextFromTLS);
			PSLanguageMode? pslanguageMode = null;
			try
			{
				if (ExecutionContext.HasEverUsedConstrainedLanguage)
				{
					pslanguageMode = new PSLanguageMode?(executionContextFromTLS.LanguageMode);
					executionContextFromTLS.LanguageMode = PSLanguageMode.ConstrainedLanguage;
				}
				this._bindingEffective = this.PrepareCommandElements(executionContextFromTLS);
			}
			finally
			{
				if (pslanguageMode != null)
				{
					executionContextFromTLS.LanguageMode = pslanguageMode.Value;
				}
				this.RestoreHost(executionContextFromTLS);
			}
			if (this._bindingEffective && (this._isPipelineInputExpected || pipeArgumentType != null))
			{
				this._pipelineInputType = pipeArgumentType;
			}
			this._bindingEffective = this.ParseParameterArguments(paramAstAtCursor);
			if (this._bindingEffective)
			{
				unboundArguments = this.BindNamedParameters();
				this._bindingEffective = (this._currentParameterSetFlag != 0U);
				unboundArguments = this.BindPositionalParameter(unboundArguments, this._currentParameterSetFlag, this._defaultParameterSetFlag, bindingType);
				if (!this._function)
				{
					unboundArguments = this.BindRemainingParameters(unboundArguments);
					this.BindPipelineParameters();
				}
				bool flag = this._currentParameterSetFlag != 0U && this._currentParameterSetFlag != uint.MaxValue;
				bool flag2 = this._currentParameterSetFlag != 0U && (this._currentParameterSetFlag & this._currentParameterSetFlag - 1U) == 0U;
				if (bindingType != PseudoParameterBinder.BindingType.ParameterCompletion && flag && !flag2)
				{
					CmdletParameterBinderController.ResolveParameterSetAmbiguityBasedOnMandatoryParameters(this._boundParameters, this._unboundParameters, null, ref this._currentParameterSetFlag, null);
				}
			}
			if (this._bindingEffective)
			{
				return new PseudoBindingInfo(this._commandInfo, this._currentParameterSetFlag, this._defaultParameterSetFlag, this._boundParameters, this._unboundParameters, this._boundArguments, this._boundPositionalParameter, this._arguments, this._parametersNotFound, this._ambiguousParameters, this._bindingExceptions, this._duplicateParameters, unboundArguments);
			}
			if (this._bindableParameters == null)
			{
				return null;
			}
			this._unboundParameters.Clear();
			this._unboundParameters.AddRange(this._bindableParameters.BindableParameters.Values);
			return new PseudoBindingInfo(this._commandInfo, this._defaultParameterSetFlag, this._arguments, this._unboundParameters);
		}

		// Token: 0x06005AAF RID: 23215 RVA: 0x001E70E0 File Offset: 0x001E52E0
		private void SetTemporaryDefaultHost(ExecutionContext executionContext)
		{
			if (executionContext.EngineHostInterface.IsHostRefSet)
			{
				this._restoreHost = executionContext.EngineHostInterface.ExternalHost;
				executionContext.EngineHostInterface.RevertHostRef();
			}
			executionContext.EngineHostInterface.SetHostRef(new DefaultHost(CultureInfo.CurrentCulture, CultureInfo.CurrentUICulture));
		}

		// Token: 0x06005AB0 RID: 23216 RVA: 0x001E7130 File Offset: 0x001E5330
		private void RestoreHost(ExecutionContext executionContext)
		{
			executionContext.EngineHostInterface.RevertHostRef();
			if (this._restoreHost != null)
			{
				executionContext.EngineHostInterface.SetHostRef(this._restoreHost);
				this._restoreHost = null;
			}
		}

		// Token: 0x06005AB1 RID: 23217 RVA: 0x001E7178 File Offset: 0x001E5378
		private void InitializeMembers()
		{
			List<string> supportedCommonParameters = new List<string>
			{
				"Verbose",
				"Debug",
				"ErrorAction",
				"WarningAction",
				"InformationAction"
			};
			this.ignoredWorkflowParameters = new List<string>(Cmdlet.CommonParameters.Concat(Cmdlet.OptionalCommonParameters));
			this.ignoredWorkflowParameters.RemoveAll((string item) => supportedCommonParameters.Contains(item, StringComparer.OrdinalIgnoreCase));
			this._function = false;
			this._commandName = null;
			this._currentParameterSetFlag = uint.MaxValue;
			this._defaultParameterSetFlag = 0U;
			this._bindableParameters = null;
			this._arguments = (this._arguments ?? new Collection<AstParameterArgumentPair>());
			this._boundParameters = (this._boundParameters ?? new Dictionary<string, MergedCompiledCommandParameter>(StringComparer.OrdinalIgnoreCase));
			this._boundArguments = (this._boundArguments ?? new Dictionary<string, AstParameterArgumentPair>(StringComparer.OrdinalIgnoreCase));
			this._unboundParameters = (this._unboundParameters ?? new List<MergedCompiledCommandParameter>());
			this._boundPositionalParameter = (this._boundPositionalParameter ?? new Collection<string>());
			this._bindingExceptions = (this._bindingExceptions ?? new Dictionary<CommandParameterAst, ParameterBindingException>());
			this._arguments.Clear();
			this._boundParameters.Clear();
			this._unboundParameters.Clear();
			this._boundArguments.Clear();
			this._boundPositionalParameter.Clear();
			this._bindingExceptions.Clear();
			this._pipelineInputType = null;
			this._bindingEffective = true;
			this._isPipelineInputExpected = false;
			this._parametersNotFound = (this._parametersNotFound ?? new Collection<CommandParameterAst>());
			this._ambiguousParameters = (this._ambiguousParameters ?? new Collection<CommandParameterAst>());
			this._duplicateParameters = (this._duplicateParameters ?? new Collection<AstParameterArgumentPair>());
			this._parametersNotFound.Clear();
			this._ambiguousParameters.Clear();
			this._duplicateParameters.Clear();
		}

		// Token: 0x06005AB2 RID: 23218 RVA: 0x001E738C File Offset: 0x001E558C
		private bool PrepareCommandElements(ExecutionContext context)
		{
			int i = 0;
			bool dotSource = this._commandAst.InvocationOperator == TokenKind.Dot;
			CommandProcessorBase commandProcessorBase = null;
			string text = null;
			bool flag = false;
			try
			{
				commandProcessorBase = (this.PrepareFromAst(context, out text) ?? context.CreateCommand(text, dotSource));
			}
			catch (RuntimeException e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				if (!this._commandAst.IsInWorkflow() || text == null || !CompletionCompleters.PseudoWorkflowCommands.Contains(text, StringComparer.OrdinalIgnoreCase))
				{
					return false;
				}
				flag = true;
			}
			CommandProcessor commandProcessor = commandProcessorBase as CommandProcessor;
			ScriptCommandProcessorBase scriptCommandProcessorBase = commandProcessorBase as ScriptCommandProcessorBase;
			bool flag2 = commandProcessor != null && commandProcessor.CommandInfo.ImplementsDynamicParameters;
			List<object> list = flag2 ? new List<object>(this._commandElements.Count) : null;
			if (commandProcessor != null || scriptCommandProcessorBase != null || flag)
			{
				for (i++; i < this._commandElements.Count; i++)
				{
					CommandParameterAst commandParameterAst = this._commandElements[i] as CommandParameterAst;
					if (commandParameterAst != null)
					{
						if (list != null)
						{
							list.Add(commandParameterAst.Extent.Text);
						}
						AstPair item = (commandParameterAst.Argument != null) ? new AstPair(commandParameterAst) : new AstPair(commandParameterAst, null);
						this._arguments.Add(item);
					}
					else
					{
						StringConstantExpressionAst stringConstantExpressionAst = this._commandElements[i] as StringConstantExpressionAst;
						if (stringConstantExpressionAst == null || !stringConstantExpressionAst.Value.Trim().Equals("-", StringComparison.OrdinalIgnoreCase))
						{
							ExpressionAst expressionAst = this._commandElements[i] as ExpressionAst;
							if (expressionAst != null)
							{
								if (list != null)
								{
									list.Add(expressionAst.Extent.Text);
								}
								this._arguments.Add(new AstPair(null, expressionAst));
							}
						}
					}
				}
			}
			if (commandProcessor != null)
			{
				this._function = false;
				if (flag2)
				{
					ParameterBinderController.AddArgumentsToCommandProcessor(commandProcessor, list.ToArray());
					bool flag3 = false;
					bool flag4 = false;
					do
					{
						CommandProcessorBase currentCommandProcessor = context.CurrentCommandProcessor;
						try
						{
							context.CurrentCommandProcessor = commandProcessor;
							commandProcessor.SetCurrentScopeToExecutionScope();
							if (!flag3)
							{
								commandProcessor.CmdletParameterBinderController.BindCommandLineParametersNoValidation(commandProcessor.arguments);
							}
							else
							{
								flag4 = true;
								commandProcessor.CmdletParameterBinderController.ClearUnboundArguments();
								commandProcessor.CmdletParameterBinderController.BindCommandLineParametersNoValidation(new Collection<CommandParameterInternal>());
							}
						}
						catch (ParameterBindingException ex)
						{
							if (ex.ErrorId == "MissingArgument" || ex.ErrorId == "AmbiguousParameter")
							{
								flag3 = true;
							}
						}
						catch (Exception e2)
						{
							CommandProcessorBase.CheckForSevereException(e2);
						}
						finally
						{
							context.CurrentCommandProcessor = currentCommandProcessor;
							commandProcessor.RestorePreviousScope();
						}
					}
					while (flag3 && !flag4);
				}
				this._commandInfo = commandProcessor.CommandInfo;
				this._commandName = commandProcessor.CommandInfo.Name;
				this._bindableParameters = commandProcessor.CmdletParameterBinderController.BindableParameters;
				this._defaultParameterSetFlag = commandProcessor.CommandInfo.CommandMetadata.DefaultParameterSetFlag;
			}
			else if (scriptCommandProcessorBase != null)
			{
				this._function = true;
				this._commandInfo = scriptCommandProcessorBase.CommandInfo;
				this._commandName = scriptCommandProcessorBase.CommandInfo.Name;
				this._bindableParameters = scriptCommandProcessorBase.ScriptParameterBinderController.BindableParameters;
				this._defaultParameterSetFlag = 0U;
			}
			else if (!flag)
			{
				return false;
			}
			if (this._commandAst.IsInWorkflow())
			{
				Type type = Type.GetType("Microsoft.PowerShell.Workflow.AstToWorkflowConverter, Microsoft.PowerShell.Activities, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
				if (type != null)
				{
					Dictionary<string, Type> dictionary = (Dictionary<string, Type>)type.GetMethod("GetActivityParameters").Invoke(null, new object[]
					{
						this._commandAst
					});
					if (dictionary != null)
					{
						bool flag5 = dictionary.ContainsKey("PSComputerName") && !dictionary.ContainsKey("ComputerName");
						List<MergedCompiledCommandParameter> list2 = new List<MergedCompiledCommandParameter>();
						Collection<Attribute> attributes = new Collection<Attribute>
						{
							new ParameterAttribute()
						};
						foreach (KeyValuePair<string, Type> keyValuePair in dictionary)
						{
							if (flag || !this._bindableParameters.BindableParameters.ContainsKey(keyValuePair.Key))
							{
								Type actualActivityParameterType = PseudoParameterBinder.GetActualActivityParameterType(keyValuePair.Value);
								RuntimeDefinedParameter runtimeDefinedParameter = new RuntimeDefinedParameter(keyValuePair.Key, actualActivityParameterType, attributes);
								CompiledCommandParameter parameter = new CompiledCommandParameter(runtimeDefinedParameter, false)
								{
									IsInAllSets = true
								};
								MergedCompiledCommandParameter item2 = new MergedCompiledCommandParameter(parameter, ParameterBinderAssociation.DeclaredFormalParameters);
								list2.Add(item2);
							}
						}
						if (list2.Any<MergedCompiledCommandParameter>())
						{
							MergedCommandParameterMetadata mergedCommandParameterMetadata = new MergedCommandParameterMetadata();
							if (!flag)
							{
								mergedCommandParameterMetadata.ReplaceMetadata(this._bindableParameters);
							}
							foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter in list2)
							{
								mergedCommandParameterMetadata.BindableParameters.Add(mergedCompiledCommandParameter.Parameter.Name, mergedCompiledCommandParameter);
							}
							this._bindableParameters = mergedCommandParameterMetadata;
						}
						bool flag6 = false;
						foreach (string key in this.ignoredWorkflowParameters)
						{
							if (this._bindableParameters.BindableParameters.ContainsKey(key) && !dictionary.ContainsKey(key))
							{
								if (!flag6)
								{
									this._bindableParameters.ResetReadOnly();
									flag6 = true;
								}
								this._bindableParameters.BindableParameters.Remove(key);
							}
						}
						if (this._bindableParameters.BindableParameters.ContainsKey("ComputerName") && flag5)
						{
							if (!flag6)
							{
								this._bindableParameters.ResetReadOnly();
								flag6 = true;
							}
							this._bindableParameters.BindableParameters.Remove("ComputerName");
							string text2 = (from aliasPair in this._bindableParameters.AliasedParameters
							where string.Equals("ComputerName", aliasPair.Value.Parameter.Name)
							select aliasPair.Key).FirstOrDefault<string>();
							if (text2 != null)
							{
								this._bindableParameters.AliasedParameters.Remove(text2);
							}
						}
					}
				}
			}
			this._unboundParameters.AddRange(this._bindableParameters.BindableParameters.Values);
			CommandBaseAst commandBaseAst = null;
			PipelineAst pipelineAst = this._commandAst.Parent as PipelineAst;
			if (pipelineAst.PipelineElements.Count > 1)
			{
				foreach (CommandBaseAst commandBaseAst2 in pipelineAst.PipelineElements)
				{
					if (commandBaseAst2.GetHashCode() == this._commandAst.GetHashCode())
					{
						this._isPipelineInputExpected = (commandBaseAst != null);
						if (this._isPipelineInputExpected)
						{
							this._pipelineInputType = typeof(object);
							break;
						}
						break;
					}
					else
					{
						commandBaseAst = commandBaseAst2;
					}
				}
			}
			return true;
		}

		// Token: 0x06005AB3 RID: 23219 RVA: 0x001E7A84 File Offset: 0x001E5C84
		private CommandProcessorBase PrepareFromAst(ExecutionContext context, out string resolvedCommandName)
		{
			ExportVisitor exportVisitor = new ExportVisitor();
			Ast ast = this._commandAst;
			while (ast.Parent != null)
			{
				ast = ast.Parent;
			}
			ast.Visit(exportVisitor);
			resolvedCommandName = this._commandAst.GetCommandName();
			CommandProcessorBase result = null;
			int num = 0;
			string text;
			while (exportVisitor.DiscoveredAliases.TryGetValue(resolvedCommandName, out text))
			{
				num++;
				if (num > 5)
				{
					break;
				}
				resolvedCommandName = text;
			}
			FunctionDefinitionAst functionDefinitionAst;
			if (exportVisitor.DiscoveredFunctions.TryGetValue(resolvedCommandName, out functionDefinitionAst))
			{
				ScriptBlock scriptblock = functionDefinitionAst.IsWorkflow ? PseudoParameterBinder.CreateFakeScriptBlockForWorkflow(functionDefinitionAst) : new ScriptBlock(functionDefinitionAst, functionDefinitionAst.IsFilter);
				result = CommandDiscovery.CreateCommandProcessorForScript(scriptblock, context, true, context.EngineSessionState);
			}
			return result;
		}

		// Token: 0x06005AB4 RID: 23220 RVA: 0x001E7B4C File Offset: 0x001E5D4C
		private static ScriptBlock CreateFakeScriptBlockForWorkflow(FunctionDefinitionAst functionDefinitionAst)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			ParamBlockAst paramBlock = functionDefinitionAst.Body.ParamBlock;
			if (paramBlock != null)
			{
				IEnumerable<AttributeAst> enumerable = from attribute in paramBlock.Attributes
				where typeof(OutputTypeAttribute).Equals(attribute.TypeName.GetReflectionAttributeType())
				select attribute;
				foreach (AttributeAst attributeAst in enumerable)
				{
					stringBuilder.Append(attributeAst.Extent.Text);
				}
			}
			ReadOnlyCollection<ParameterAst> parameters = ((IParameterMetadataProvider)functionDefinitionAst).Parameters;
			if (parameters != null)
			{
				bool flag = true;
				foreach (ParameterAst parameterAst in parameters)
				{
					if (!flag)
					{
						stringBuilder2.Append(", ");
					}
					flag = false;
					stringBuilder2.Append(parameterAst.Extent.Text);
				}
				if (!flag)
				{
					stringBuilder2.Append(", ");
				}
			}
			Token[] array;
			ParseError[] array2;
			ScriptBlockAst scriptBlockAst = Parser.ParseInput(string.Format(CultureInfo.InvariantCulture, "\r\n                [CmdletBinding()]\r\n                {0}\r\n                param (\r\n                    {1}\r\n                    [hashtable[]] $PSParameterCollection,\r\n                    [string[]] $PSComputerName,\r\n                    [ValidateNotNullOrEmpty()] $PSCredential,\r\n                    [uint32] $PSConnectionRetryCount,\r\n                    [uint32] $PSConnectionRetryIntervalSec,\r\n                    [ValidateRange(1, 2147483)][uint32] $PSRunningTimeoutSec,\r\n                    [ValidateRange(1, 2147483)][uint32] $PSElapsedTimeoutSec,\r\n                    [bool] $PSPersist,\r\n                    [ValidateNotNullOrEmpty()] [System.Management.Automation.Runspaces.AuthenticationMechanism] $PSAuthentication,\r\n                    [ValidateNotNullOrEmpty()][System.Management.AuthenticationLevel] $PSAuthenticationLevel,\r\n                    [ValidateNotNullOrEmpty()] [string] $PSApplicationName,\r\n                    [uint32] $PSPort,\r\n                    [switch] $PSUseSSL,\r\n                    [ValidateNotNullOrEmpty()] [string] $PSConfigurationName,\r\n                    [ValidateNotNullOrEmpty()][string[]] $PSConnectionURI,\r\n                    [switch] $PSAllowRedirection,\r\n                    [ValidateNotNullOrEmpty()][System.Management.Automation.Remoting.PSSessionOption] $PSSessionOption,\r\n                    [ValidateNotNullOrEmpty()] [string] $PSCertificateThumbprint,\r\n                    [hashtable] $PSPrivateMetadata,\r\n                    [switch] $AsJob,\r\n                    [string] $JobName,\r\n                    [Parameter(ValueFromPipeline=$true)]$InputObject\r\n                    )\r\n", new object[]
			{
				stringBuilder.ToString(),
				stringBuilder2.ToString()
			}), out array, out array2);
			return scriptBlockAst.GetScriptBlock();
		}

		// Token: 0x06005AB5 RID: 23221 RVA: 0x001E7CA8 File Offset: 0x001E5EA8
		private static Type GetActualActivityParameterType(Type parameterType)
		{
			if (parameterType.GetTypeInfo().IsGenericType)
			{
				string fullName = parameterType.GetGenericTypeDefinition().FullName;
				if (fullName.Equals("System.Activities.InArgument`1", StringComparison.Ordinal) || fullName.Equals("System.Activities.InOutArgument`1", StringComparison.Ordinal))
				{
					parameterType = parameterType.GetGenericArguments()[0];
				}
			}
			parameterType = (Nullable.GetUnderlyingType(parameterType) ?? parameterType);
			return parameterType;
		}

		// Token: 0x06005AB6 RID: 23222 RVA: 0x001E7D04 File Offset: 0x001E5F04
		private bool ParseParameterArguments(CommandParameterAst paramAstAtCursor)
		{
			if (!this._bindingEffective)
			{
				return this._bindingEffective;
			}
			Collection<AstParameterArgumentPair> collection = new Collection<AstParameterArgumentPair>();
			for (int i = 0; i < this._arguments.Count; i++)
			{
				AstParameterArgumentPair astParameterArgumentPair = this._arguments[i];
				if (!astParameterArgumentPair.ParameterSpecified || astParameterArgumentPair.ArgumentSpecified)
				{
					collection.Add(astParameterArgumentPair);
				}
				else
				{
					string parameterName = astParameterArgumentPair.ParameterName;
					MergedCompiledCommandParameter mergedCompiledCommandParameter = null;
					try
					{
						bool tryExactMatching = astParameterArgumentPair.Parameter != paramAstAtCursor;
						mergedCompiledCommandParameter = this._bindableParameters.GetMatchingParameter(parameterName, false, tryExactMatching, null);
					}
					catch (ParameterBindingException value)
					{
						if (i < this._arguments.Count - 1)
						{
							AstParameterArgumentPair astParameterArgumentPair2 = this._arguments[i + 1];
							if (!astParameterArgumentPair2.ParameterSpecified && astParameterArgumentPair2.ArgumentSpecified)
							{
								i++;
							}
						}
						this._ambiguousParameters.Add(astParameterArgumentPair.Parameter);
						this._bindingExceptions[astParameterArgumentPair.Parameter] = value;
						goto IL_258;
					}
					if (mergedCompiledCommandParameter == null)
					{
						if (i < this._arguments.Count - 1)
						{
							AstParameterArgumentPair astParameterArgumentPair3 = this._arguments[i + 1];
							if (!astParameterArgumentPair3.ParameterSpecified && astParameterArgumentPair3.ArgumentSpecified)
							{
								if (paramAstAtCursor != null)
								{
									this._arguments = null;
									return false;
								}
								i++;
								this._parametersNotFound.Add(astParameterArgumentPair.Parameter);
								goto IL_258;
							}
						}
						this._parametersNotFound.Add(astParameterArgumentPair.Parameter);
					}
					else if (mergedCompiledCommandParameter.Parameter.Type == typeof(SwitchParameter))
					{
						SwitchPair item = new SwitchPair(astParameterArgumentPair.Parameter);
						collection.Add(item);
					}
					else if (i < this._arguments.Count - 1)
					{
						AstParameterArgumentPair astParameterArgumentPair4 = this._arguments[i + 1];
						if (astParameterArgumentPair4.ParameterSpecified)
						{
							try
							{
								if (this._bindableParameters.GetMatchingParameter(astParameterArgumentPair4.ParameterName, false, true, null) == null)
								{
									AstPair item2 = new AstPair(astParameterArgumentPair.Parameter, astParameterArgumentPair4.Parameter);
									collection.Add(item2);
									i++;
								}
								else
								{
									FakePair item3 = new FakePair(astParameterArgumentPair.Parameter);
									collection.Add(item3);
								}
								goto IL_258;
							}
							catch (ParameterBindingException)
							{
								FakePair item4 = new FakePair(astParameterArgumentPair.Parameter);
								collection.Add(item4);
								goto IL_258;
							}
						}
						AstPair astPair = astParameterArgumentPair4 as AstPair;
						AstPair item5 = new AstPair(astParameterArgumentPair.Parameter, (ExpressionAst)astPair.Argument);
						collection.Add(item5);
						i++;
					}
					else
					{
						FakePair item6 = new FakePair(astParameterArgumentPair.Parameter);
						collection.Add(item6);
					}
				}
				IL_258:;
			}
			this._arguments = collection;
			return true;
		}

		// Token: 0x06005AB7 RID: 23223 RVA: 0x001E7FA4 File Offset: 0x001E61A4
		private Collection<AstParameterArgumentPair> BindNamedParameters()
		{
			Collection<AstParameterArgumentPair> collection = new Collection<AstParameterArgumentPair>();
			if (!this._bindingEffective)
			{
				return collection;
			}
			foreach (AstParameterArgumentPair astParameterArgumentPair in this._arguments)
			{
				if (!astParameterArgumentPair.ParameterSpecified)
				{
					collection.Add(astParameterArgumentPair);
				}
				else
				{
					MergedCompiledCommandParameter mergedCompiledCommandParameter = null;
					try
					{
						mergedCompiledCommandParameter = this._bindableParameters.GetMatchingParameter(astParameterArgumentPair.ParameterName, false, true, null);
					}
					catch (ParameterBindingException)
					{
						this._ambiguousParameters.Add(astParameterArgumentPair.Parameter);
						continue;
					}
					if (mergedCompiledCommandParameter == null)
					{
						this._parametersNotFound.Add(astParameterArgumentPair.Parameter);
					}
					else if (this._boundParameters.ContainsKey(mergedCompiledCommandParameter.Parameter.Name))
					{
						this._duplicateParameters.Add(astParameterArgumentPair);
					}
					else
					{
						if (mergedCompiledCommandParameter.Parameter.ParameterSetFlags != 0U)
						{
							this._currentParameterSetFlag &= mergedCompiledCommandParameter.Parameter.ParameterSetFlags;
						}
						this._unboundParameters.Remove(mergedCompiledCommandParameter);
						if (!this._boundParameters.ContainsKey(mergedCompiledCommandParameter.Parameter.Name))
						{
							this._boundParameters.Add(mergedCompiledCommandParameter.Parameter.Name, mergedCompiledCommandParameter);
						}
						if (!this._boundArguments.ContainsKey(mergedCompiledCommandParameter.Parameter.Name))
						{
							this._boundArguments.Add(mergedCompiledCommandParameter.Parameter.Name, astParameterArgumentPair);
						}
					}
				}
			}
			return collection;
		}

		// Token: 0x06005AB8 RID: 23224 RVA: 0x001E813C File Offset: 0x001E633C
		private Collection<AstParameterArgumentPair> BindPositionalParameter(Collection<AstParameterArgumentPair> unboundArguments, uint validParameterSetFlags, uint defaultParameterSetFlag, PseudoParameterBinder.BindingType bindingType)
		{
			Collection<AstParameterArgumentPair> collection = new Collection<AstParameterArgumentPair>();
			if (this._bindingEffective && unboundArguments.Count > 0)
			{
				List<AstParameterArgumentPair> list = new List<AstParameterArgumentPair>(unboundArguments);
				SortedDictionary<int, Dictionary<MergedCompiledCommandParameter, PositionalCommandParameter>> sortedDictionary;
				try
				{
					sortedDictionary = ParameterBinderController.EvaluateUnboundPositionalParameters(this._unboundParameters, validParameterSetFlags);
				}
				catch (InvalidOperationException)
				{
					this._bindingEffective = false;
					return collection;
				}
				if (sortedDictionary.Count == 0)
				{
					return unboundArguments;
				}
				int num = 0;
				foreach (Dictionary<MergedCompiledCommandParameter, PositionalCommandParameter> dictionary in sortedDictionary.Values)
				{
					if (dictionary.Count != 0)
					{
						AstParameterArgumentPair nextPositionalArgument = PseudoParameterBinder.GetNextPositionalArgument(list, collection, ref num);
						if (nextPositionalArgument == null)
						{
							break;
						}
						bool flag = false;
						if (bindingType != PseudoParameterBinder.BindingType.ParameterCompletion && (validParameterSetFlags & defaultParameterSetFlag) != 0U)
						{
							flag = this.BindPseudoPositionalParameterInSet(defaultParameterSetFlag, dictionary, nextPositionalArgument, false);
						}
						if (!flag && bindingType == PseudoParameterBinder.BindingType.ArgumentBinding)
						{
							flag = this.BindPseudoPositionalParameterInSet(validParameterSetFlags, dictionary, nextPositionalArgument, false);
						}
						if (!flag)
						{
							flag = this.BindPseudoPositionalParameterInSet(validParameterSetFlags, dictionary, nextPositionalArgument, true);
						}
						if (!flag)
						{
							collection.Add(nextPositionalArgument);
						}
						else if (validParameterSetFlags != this._currentParameterSetFlag)
						{
							validParameterSetFlags = this._currentParameterSetFlag;
							ParameterBinderController.UpdatePositionalDictionary(sortedDictionary, validParameterSetFlags);
						}
					}
				}
				for (int i = num; i < list.Count; i++)
				{
					collection.Add(list[i]);
				}
				return collection;
			}
			return collection;
		}

		// Token: 0x06005AB9 RID: 23225 RVA: 0x001E8294 File Offset: 0x001E6494
		private bool BindPseudoPositionalParameterInSet(uint validParameterSetFlag, Dictionary<MergedCompiledCommandParameter, PositionalCommandParameter> nextPositionalParameters, AstParameterArgumentPair argument, bool typeConversion)
		{
			bool flag = false;
			uint num = 0U;
			foreach (PositionalCommandParameter positionalCommandParameter in nextPositionalParameters.Values)
			{
				foreach (ParameterSetSpecificMetadata parameterSetSpecificMetadata in positionalCommandParameter.ParameterSetData)
				{
					if ((validParameterSetFlag & parameterSetSpecificMetadata.ParameterSetFlag) != 0U || parameterSetSpecificMetadata.IsInAllSets)
					{
						bool flag2 = false;
						string name = positionalCommandParameter.Parameter.Parameter.Name;
						Type type = positionalCommandParameter.Parameter.Parameter.Type;
						Type argumentType = argument.ArgumentType;
						if (argumentType == typeof(object))
						{
							flag2 = (flag = true);
						}
						else if (PseudoParameterBinder.IsTypeEquivalent(argumentType, type))
						{
							flag2 = (flag = true);
						}
						else if (typeConversion)
						{
							flag2 = (flag = true);
						}
						if (flag2)
						{
							num |= positionalCommandParameter.Parameter.Parameter.ParameterSetFlags;
							this._unboundParameters.Remove(positionalCommandParameter.Parameter);
							if (!this._boundParameters.ContainsKey(name))
							{
								this._boundParameters.Add(name, positionalCommandParameter.Parameter);
								this._boundPositionalParameter.Add(name);
							}
							if (!this._boundArguments.ContainsKey(name))
							{
								this._boundArguments.Add(name, argument);
								break;
							}
							break;
						}
					}
				}
			}
			if (flag && num != 0U)
			{
				this._currentParameterSetFlag &= num;
			}
			return flag;
		}

		// Token: 0x06005ABA RID: 23226 RVA: 0x001E8448 File Offset: 0x001E6648
		private static bool IsTypeEquivalent(Type argType, Type paramType)
		{
			bool result = false;
			if (argType == paramType)
			{
				result = true;
			}
			else if (argType.IsSubclassOf(paramType))
			{
				result = true;
			}
			else if (argType == paramType.GetElementType())
			{
				result = true;
			}
			else if (argType.IsSubclassOf(typeof(Array)) && paramType.IsSubclassOf(typeof(Array)))
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06005ABB RID: 23227 RVA: 0x001E84AC File Offset: 0x001E66AC
		private static AstParameterArgumentPair GetNextPositionalArgument(List<AstParameterArgumentPair> unboundArgumentsCollection, Collection<AstParameterArgumentPair> nonPositionalArguments, ref int unboundArgumentsIndex)
		{
			AstParameterArgumentPair result = null;
			while (unboundArgumentsIndex < unboundArgumentsCollection.Count)
			{
				AstParameterArgumentPair astParameterArgumentPair = unboundArgumentsCollection[unboundArgumentsIndex++];
				if (!astParameterArgumentPair.ParameterSpecified)
				{
					result = astParameterArgumentPair;
					break;
				}
				nonPositionalArguments.Add(astParameterArgumentPair);
			}
			return result;
		}

		// Token: 0x06005ABC RID: 23228 RVA: 0x001E84EC File Offset: 0x001E66EC
		private Collection<AstParameterArgumentPair> BindRemainingParameters(Collection<AstParameterArgumentPair> unboundArguments)
		{
			bool flag = false;
			uint num = 0U;
			if (!this._bindingEffective || unboundArguments.Count == 0)
			{
				return unboundArguments;
			}
			Collection<ExpressionAst> collection = new Collection<ExpressionAst>();
			foreach (AstParameterArgumentPair astParameterArgumentPair in unboundArguments)
			{
				AstPair astPair = astParameterArgumentPair as AstPair;
				collection.Add((ExpressionAst)astPair.Argument);
			}
			List<MergedCompiledCommandParameter> list = new List<MergedCompiledCommandParameter>(this._unboundParameters);
			foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter in list)
			{
				bool flag2 = (mergedCompiledCommandParameter.Parameter.ParameterSetFlags & this._currentParameterSetFlag) != 0U || mergedCompiledCommandParameter.Parameter.IsInAllSets;
				if (flag2)
				{
					IEnumerable<ParameterSetSpecificMetadata> matchingParameterSetData = mergedCompiledCommandParameter.Parameter.GetMatchingParameterSetData(this._currentParameterSetFlag);
					foreach (ParameterSetSpecificMetadata parameterSetSpecificMetadata in matchingParameterSetData)
					{
						if (parameterSetSpecificMetadata.ValueFromRemainingArguments)
						{
							num |= mergedCompiledCommandParameter.Parameter.ParameterSetFlags;
							string name = mergedCompiledCommandParameter.Parameter.Name;
							this._unboundParameters.Remove(mergedCompiledCommandParameter);
							if (!this._boundParameters.ContainsKey(name))
							{
								this._boundParameters.Add(name, mergedCompiledCommandParameter);
							}
							if (!this._boundArguments.ContainsKey(name))
							{
								this._boundArguments.Add(name, new AstArrayPair(name, collection));
								unboundArguments.Clear();
							}
							flag = true;
							break;
						}
					}
				}
			}
			if (flag && num != 0U)
			{
				this._currentParameterSetFlag &= num;
			}
			return unboundArguments;
		}

		// Token: 0x06005ABD RID: 23229 RVA: 0x001E86EC File Offset: 0x001E68EC
		private void BindPipelineParameters()
		{
			bool flag = false;
			uint num = 0U;
			if (!this._bindingEffective || !this._isPipelineInputExpected)
			{
				return;
			}
			List<MergedCompiledCommandParameter> list = new List<MergedCompiledCommandParameter>(this._unboundParameters);
			foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter in list)
			{
				if (mergedCompiledCommandParameter.Parameter.IsPipelineParameterInSomeParameterSet)
				{
					bool flag2 = (mergedCompiledCommandParameter.Parameter.ParameterSetFlags & this._currentParameterSetFlag) != 0U || mergedCompiledCommandParameter.Parameter.IsInAllSets;
					if (flag2)
					{
						IEnumerable<ParameterSetSpecificMetadata> matchingParameterSetData = mergedCompiledCommandParameter.Parameter.GetMatchingParameterSetData(this._currentParameterSetFlag);
						foreach (ParameterSetSpecificMetadata parameterSetSpecificMetadata in matchingParameterSetData)
						{
							if (parameterSetSpecificMetadata.ValueFromPipeline)
							{
								num |= mergedCompiledCommandParameter.Parameter.ParameterSetFlags;
								string name = mergedCompiledCommandParameter.Parameter.Name;
								this._unboundParameters.Remove(mergedCompiledCommandParameter);
								if (!this._boundParameters.ContainsKey(name))
								{
									this._boundParameters.Add(name, mergedCompiledCommandParameter);
								}
								if (!this._boundArguments.ContainsKey(name))
								{
									this._boundArguments.Add(name, new PipeObjectPair(name, this._pipelineInputType));
								}
								flag = true;
								break;
							}
						}
					}
				}
			}
			if (flag && num != 0U)
			{
				this._currentParameterSetFlag &= num;
			}
		}

		// Token: 0x0400307D RID: 12413
		private PSHost _restoreHost;

		// Token: 0x0400307E RID: 12414
		private CommandAst _commandAst;

		// Token: 0x0400307F RID: 12415
		private ReadOnlyCollection<CommandElementAst> _commandElements;

		// Token: 0x04003080 RID: 12416
		private bool _function;

		// Token: 0x04003081 RID: 12417
		private string _commandName;

		// Token: 0x04003082 RID: 12418
		private CommandInfo _commandInfo;

		// Token: 0x04003083 RID: 12419
		private uint _currentParameterSetFlag = uint.MaxValue;

		// Token: 0x04003084 RID: 12420
		private uint _defaultParameterSetFlag;

		// Token: 0x04003085 RID: 12421
		private MergedCommandParameterMetadata _bindableParameters;

		// Token: 0x04003086 RID: 12422
		private Dictionary<string, MergedCompiledCommandParameter> _boundParameters;

		// Token: 0x04003087 RID: 12423
		private Dictionary<string, AstParameterArgumentPair> _boundArguments;

		// Token: 0x04003088 RID: 12424
		private Collection<AstParameterArgumentPair> _arguments;

		// Token: 0x04003089 RID: 12425
		private Collection<string> _boundPositionalParameter;

		// Token: 0x0400308A RID: 12426
		private List<MergedCompiledCommandParameter> _unboundParameters;

		// Token: 0x0400308B RID: 12427
		private Type _pipelineInputType;

		// Token: 0x0400308C RID: 12428
		private bool _bindingEffective = true;

		// Token: 0x0400308D RID: 12429
		private bool _isPipelineInputExpected;

		// Token: 0x0400308E RID: 12430
		private Collection<CommandParameterAst> _parametersNotFound;

		// Token: 0x0400308F RID: 12431
		private Collection<CommandParameterAst> _ambiguousParameters;

		// Token: 0x04003090 RID: 12432
		private Collection<AstParameterArgumentPair> _duplicateParameters;

		// Token: 0x04003091 RID: 12433
		private Dictionary<CommandParameterAst, ParameterBindingException> _bindingExceptions;

		// Token: 0x04003092 RID: 12434
		private List<string> ignoredWorkflowParameters;

		// Token: 0x0200099D RID: 2461
		internal enum BindingType
		{
			// Token: 0x04003097 RID: 12439
			ArgumentBinding,
			// Token: 0x04003098 RID: 12440
			ArgumentCompletion,
			// Token: 0x04003099 RID: 12441
			ParameterCompletion
		}
	}
}
