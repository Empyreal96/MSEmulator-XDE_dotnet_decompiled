using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Management.Automation.Host;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x0200004F RID: 79
	internal class CmdletParameterBinderController : ParameterBinderController
	{
		// Token: 0x0600043F RID: 1087 RVA: 0x0000FBE0 File Offset: 0x0000DDE0
		internal CmdletParameterBinderController(Cmdlet cmdlet, CommandMetadata commandMetadata, ParameterBinderBase parameterBinder) : base(cmdlet.MyInvocation, cmdlet.Context, parameterBinder)
		{
			if (cmdlet == null)
			{
				throw PSTraceSource.NewArgumentNullException("cmdlet");
			}
			if (commandMetadata == null)
			{
				throw PSTraceSource.NewArgumentNullException("commandMetadata");
			}
			this.Command = cmdlet;
			this._commandRuntime = (MshCommandRuntime)cmdlet.CommandRuntime;
			this._commandMetadata = commandMetadata;
			if (commandMetadata.ImplementsDynamicParameters)
			{
				base.UnboundParameters = base.BindableParameters.ReplaceMetadata(commandMetadata.StaticCommandParameterMetadata);
				return;
			}
			this._bindableParameters = commandMetadata.StaticCommandParameterMetadata;
			base.UnboundParameters = new List<MergedCompiledCommandParameter>(this._bindableParameters.BindableParameters.Values);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0000FCB0 File Offset: 0x0000DEB0
		internal void BindCommandLineParameters(Collection<CommandParameterInternal> arguments)
		{
			CmdletParameterBinderController._tracer.WriteLine("Argument count: {0}", new object[]
			{
				arguments.Count
			});
			this.BindCommandLineParametersNoValidation(arguments);
			bool flag = !this._commandRuntime.IsClosed || !this._commandRuntime.InputPipe.Empty;
			int num;
			if (!flag)
			{
				num = this.ValidateParameterSets(false, true);
			}
			else
			{
				num = this.ValidateParameterSets(true, false);
			}
			if (num == 1 && !base.DefaultParameterBindingInUse)
			{
				this.ApplyDefaultParameterBinding("Mandatory Checking", false);
			}
			if (num > 1 && flag)
			{
				uint num2 = this.FilterParameterSetsTakingNoPipelineInput();
				if (num2 != this._currentParameterSetFlag)
				{
					this._currentParameterSetFlag = num2;
					num = this.ValidateParameterSets(true, false);
				}
			}
			using (ParameterBinderBase.bindingTracer.TraceScope("MANDATORY PARAMETER CHECK on cmdlet [{0}]", new object[]
			{
				this._commandMetadata.Name
			}))
			{
				try
				{
					bool promptForMandatory = this.Command.CommandInfo.Visibility == SessionStateEntryVisibility.Public;
					Collection<MergedCompiledCommandParameter> collection;
					this.HandleUnboundMandatoryParameters(num, true, promptForMandatory, flag, out collection);
					if (base.DefaultParameterBinder is ScriptParameterBinder)
					{
						base.BindUnboundScriptParameters();
					}
				}
				catch (ParameterBindingException pbex)
				{
					if (!base.DefaultParameterBindingInUse)
					{
						throw;
					}
					base.ThrowElaboratedBindingException(pbex);
				}
			}
			if (!flag)
			{
				this.VerifyParameterSetSelected();
			}
			this._prePipelineProcessingParameterSetFlags = this._currentParameterSetFlag;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0000FE20 File Offset: 0x0000E020
		internal void BindCommandLineParametersNoValidation(Collection<CommandParameterInternal> arguments)
		{
			PSScriptCmdlet psscriptCmdlet = this.Command as PSScriptCmdlet;
			if (psscriptCmdlet != null)
			{
				psscriptCmdlet.PrepareForBinding(((ScriptParameterBinder)base.DefaultParameterBinder).LocalScope, base.CommandLineParameters);
			}
			foreach (CommandParameterInternal item in arguments)
			{
				base.UnboundArguments.Add(item);
			}
			CommandMetadata commandMetadata = this._commandMetadata;
			this._warningSet.Clear();
			this._allDefaultParameterValuePairs = this.GetDefaultParameterValuePairs(true);
			base.DefaultParameterBindingInUse = false;
			base.BoundDefaultParameters.Clear();
			base.ReparseUnboundArguments();
			using (ParameterBinderBase.bindingTracer.TraceScope("BIND NAMED cmd line args [{0}]", new object[]
			{
				this._commandMetadata.Name
			}))
			{
				base.UnboundArguments = this.BindParameters(this._currentParameterSetFlag, base.UnboundArguments);
			}
			ParameterBindingException ex;
			ParameterBindingException ex2;
			using (ParameterBinderBase.bindingTracer.TraceScope("BIND POSITIONAL cmd line args [{0}]", new object[]
			{
				this._commandMetadata.Name
			}))
			{
				base.UnboundArguments = base.BindPositionalParameters(base.UnboundArguments, this._currentParameterSetFlag, commandMetadata.DefaultParameterSetFlag, out ex);
				ex2 = ex;
			}
			this.ApplyDefaultParameterBinding("POSITIONAL BIND", false);
			this.ValidateParameterSets(true, false);
			this.HandleCommandLineDynamicParameters(out ex);
			this.ApplyDefaultParameterBinding("DYNAMIC BIND", true);
			if (ex2 == null)
			{
				ex2 = ex;
			}
			this.HandleRemainingArguments();
			this.VerifyArgumentsProcessed(ex2);
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0000FFD4 File Offset: 0x0000E1D4
		private uint FilterParameterSetsTakingNoPipelineInput()
		{
			uint num = 0U;
			bool flag = false;
			foreach (KeyValuePair<MergedCompiledCommandParameter, CmdletParameterBinderController.DelayedScriptBlockArgument> keyValuePair in this._delayBindScriptBlocks)
			{
				num |= keyValuePair.Key.Parameter.ParameterSetFlags;
			}
			foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter in base.UnboundParameters)
			{
				if (mergedCompiledCommandParameter.Parameter.IsPipelineParameterInSomeParameterSet)
				{
					IEnumerable<ParameterSetSpecificMetadata> matchingParameterSetData = mergedCompiledCommandParameter.Parameter.GetMatchingParameterSetData(this._currentParameterSetFlag);
					foreach (ParameterSetSpecificMetadata parameterSetSpecificMetadata in matchingParameterSetData)
					{
						if (parameterSetSpecificMetadata.ValueFromPipeline || parameterSetSpecificMetadata.ValueFromPipelineByPropertyName)
						{
							if (parameterSetSpecificMetadata.ParameterSetFlag == 0U && parameterSetSpecificMetadata.IsInAllSets)
							{
								num = 0U;
								flag = true;
								break;
							}
							num |= parameterSetSpecificMetadata.ParameterSetFlag;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			if (num != 0U)
			{
				return this._currentParameterSetFlag & num;
			}
			return this._currentParameterSetFlag;
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0001011C File Offset: 0x0000E31C
		private void ApplyDefaultParameterBinding(string bindingStage, bool isDynamic)
		{
			if (!this._useDefaultParameterBinding)
			{
				return;
			}
			if (isDynamic)
			{
				this._allDefaultParameterValuePairs = this.GetDefaultParameterValuePairs(false);
			}
			Dictionary<MergedCompiledCommandParameter, object> qualifiedParameterValuePairs = this.GetQualifiedParameterValuePairs(this._currentParameterSetFlag, this._allDefaultParameterValuePairs);
			if (qualifiedParameterValuePairs != null)
			{
				bool flag = false;
				using (ParameterBinderBase.bindingTracer.TraceScope("BIND DEFAULT <parameter, value> pairs after [{0}] for [{1}]", new object[]
				{
					bindingStage,
					this._commandMetadata.Name
				}))
				{
					flag = this.BindDefaultParameters(this._currentParameterSetFlag, qualifiedParameterValuePairs);
					if (flag && !base.DefaultParameterBindingInUse)
					{
						base.DefaultParameterBindingInUse = true;
					}
				}
				CmdletParameterBinderController._tracer.WriteLine("BIND DEFAULT after [{0}] result [{1}]", new object[]
				{
					bindingStage,
					flag
				});
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x000101EC File Offset: 0x0000E3EC
		private bool BindDefaultParameters(uint validParameterSetFlag, Dictionary<MergedCompiledCommandParameter, object> defaultParameterValues)
		{
			bool flag = false;
			foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter in defaultParameterValues.Keys)
			{
				object obj = defaultParameterValues[mergedCompiledCommandParameter];
				string name = mergedCompiledCommandParameter.Parameter.Name;
				try
				{
					ScriptBlock scriptBlock = obj as ScriptBlock;
					if (scriptBlock != null)
					{
						PSObject psobject = this.WrapBindingState();
						Collection<PSObject> collection = scriptBlock.Invoke(new object[]
						{
							psobject
						});
						if (collection == null || collection.Count == 0)
						{
							continue;
						}
						if (collection.Count == 1)
						{
							obj = collection[0];
						}
						else
						{
							obj = collection;
						}
					}
					CommandParameterInternal argument = CommandParameterInternal.CreateParameterWithArgument(PositionUtilities.EmptyExtent, name, "-" + name + ":", PositionUtilities.EmptyExtent, obj, false, false);
					bool flag2 = this.BindParameter(validParameterSetFlag, argument, mergedCompiledCommandParameter, ParameterBindingFlags.ShouldCoerceType | ParameterBindingFlags.DelayBindScriptBlock);
					if (flag2 && !flag)
					{
						flag = true;
					}
					if (flag2)
					{
						base.BoundDefaultParameters.Add(name);
					}
				}
				catch (ParameterBindingException ex)
				{
					if (!this._warningSet.Contains(this._commandMetadata.Name + ":::" + name))
					{
						string text = string.Format(CultureInfo.InvariantCulture, ParameterBinderStrings.FailToBindDefaultParameter, new object[]
						{
							LanguagePrimitives.IsNull(obj) ? "null" : obj.ToString(),
							name,
							ex.Message
						});
						this._commandRuntime.WriteWarning(text);
						this._warningSet.Add(this._commandMetadata.Name + ":::" + name);
					}
				}
			}
			return flag;
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x000103BC File Offset: 0x0000E5BC
		private PSObject WrapBindingState()
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			HashSet<string> value = base.DefaultParameterBinder.CommandLineParameters.CopyBoundPositionalParameters();
			HashSet<string> hashSet2 = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (string item in base.BoundParameters.Keys)
			{
				hashSet.Add(item);
			}
			foreach (string item2 in base.BoundDefaultParameters)
			{
				hashSet2.Add(item2);
			}
			return new PSObject
			{
				Properties = 
				{
					new PSNoteProperty("BoundParameters", hashSet),
					new PSNoteProperty("BoundPositionalParameters", value),
					new PSNoteProperty("BoundDefaultParameters", hashSet2)
				}
			};
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x000104D0 File Offset: 0x0000E6D0
		private Dictionary<MergedCompiledCommandParameter, object> GetQualifiedParameterValuePairs(uint currentParameterSetFlag, Dictionary<MergedCompiledCommandParameter, object> availableParameterValuePairs)
		{
			if (availableParameterValuePairs == null)
			{
				return null;
			}
			Dictionary<MergedCompiledCommandParameter, object> dictionary = new Dictionary<MergedCompiledCommandParameter, object>();
			uint num = uint.MaxValue;
			foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter in availableParameterValuePairs.Keys)
			{
				if (((mergedCompiledCommandParameter.Parameter.ParameterSetFlags & currentParameterSetFlag) != 0U || mergedCompiledCommandParameter.Parameter.IsInAllSets) && !base.BoundArguments.ContainsKey(mergedCompiledCommandParameter.Parameter.Name))
				{
					if (mergedCompiledCommandParameter.Parameter.ParameterSetFlags != 0U)
					{
						num &= mergedCompiledCommandParameter.Parameter.ParameterSetFlags;
						if (num == 0U)
						{
							return null;
						}
					}
					dictionary.Add(mergedCompiledCommandParameter, availableParameterValuePairs[mergedCompiledCommandParameter]);
				}
			}
			if (dictionary.Count > 0)
			{
				return dictionary;
			}
			return null;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0001059C File Offset: 0x0000E79C
		private List<string> GetAliasOfCurrentCmdlet()
		{
			List<string> list = base.Context.SessionState.Internal.GetAliasesByCommandName(this._commandMetadata.Name).ToList<string>();
			if (list.Count <= 0)
			{
				return null;
			}
			return list;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x000105DC File Offset: 0x0000E7DC
		private bool MatchAnyAlias(string aliasName)
		{
			if (this._aliasList == null)
			{
				return false;
			}
			bool result = false;
			WildcardPattern wildcardPattern = new WildcardPattern(aliasName, WildcardOptions.IgnoreCase);
			foreach (string input in this._aliasList)
			{
				if (wildcardPattern.IsMatch(input))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x0001064C File Offset: 0x0000E84C
		// (set) Token: 0x0600044A RID: 1098 RVA: 0x00010654 File Offset: 0x0000E854
		internal IDictionary DefaultParameterValues { get; set; }

		// Token: 0x0600044B RID: 1099 RVA: 0x00010660 File Offset: 0x0000E860
		private Dictionary<MergedCompiledCommandParameter, object> GetDefaultParameterValuePairs(bool needToGetAlias)
		{
			if (this.DefaultParameterValues == null)
			{
				this._useDefaultParameterBinding = false;
				return null;
			}
			Dictionary<MergedCompiledCommandParameter, object> dictionary = new Dictionary<MergedCompiledCommandParameter, object>();
			if (needToGetAlias && this.DefaultParameterValues.Count > 0)
			{
				this._aliasList = this.GetAliasOfCurrentCmdlet();
			}
			this._useDefaultParameterBinding = true;
			string name = this._commandMetadata.Name;
			IDictionary<string, MergedCompiledCommandParameter> bindableParameters = base.BindableParameters.BindableParameters;
			IDictionary<string, MergedCompiledCommandParameter> aliasedParameters = base.BindableParameters.AliasedParameters;
			HashSet<MergedCompiledCommandParameter> hashSet = new HashSet<MergedCompiledCommandParameter>();
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			List<object> list = new List<object>();
			foreach (object obj in this.DefaultParameterValues)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = dictionaryEntry.Key as string;
				if (text != null)
				{
					text = text.Trim();
					string text2 = null;
					string text3 = null;
					if (!DefaultParameterDictionary.CheckKeyIsValid(text, ref text2, ref text3))
					{
						if (text.Equals("Disabled", StringComparison.OrdinalIgnoreCase) && LanguagePrimitives.IsTrue(dictionaryEntry.Value))
						{
							this._useDefaultParameterBinding = false;
							return null;
						}
						if (!text.Equals("Disabled", StringComparison.OrdinalIgnoreCase))
						{
							list.Add(dictionaryEntry.Key);
						}
					}
					else if (WildcardPattern.ContainsWildcardCharacters(text))
					{
						dictionary2.Add(text2 + ":::" + text3, dictionaryEntry.Value);
					}
					else if (text2.Equals(name, StringComparison.OrdinalIgnoreCase) || this.MatchAnyAlias(text2))
					{
						this.GetDefaultParameterValuePairsHelper(text2, text3, dictionaryEntry.Value, bindableParameters, aliasedParameters, dictionary, hashSet);
					}
				}
			}
			foreach (KeyValuePair<string, object> keyValuePair in dictionary2)
			{
				string key = keyValuePair.Key;
				string text4 = key.Substring(0, key.IndexOf(":::", StringComparison.OrdinalIgnoreCase));
				string text5 = key.Substring(key.IndexOf(":::", StringComparison.OrdinalIgnoreCase) + ":::".Length);
				WildcardPattern wildcardPattern = new WildcardPattern(text4, WildcardOptions.IgnoreCase);
				if (wildcardPattern.IsMatch(name) || this.MatchAnyAlias(text4))
				{
					if (!WildcardPattern.ContainsWildcardCharacters(text5))
					{
						this.GetDefaultParameterValuePairsHelper(text4, text5, keyValuePair.Value, bindableParameters, aliasedParameters, dictionary, hashSet);
					}
					else
					{
						WildcardPattern namePattern = MemberMatch.GetNamePattern(text5);
						List<MergedCompiledCommandParameter> list2 = new List<MergedCompiledCommandParameter>();
						foreach (KeyValuePair<string, MergedCompiledCommandParameter> keyValuePair2 in bindableParameters)
						{
							if (namePattern.IsMatch(keyValuePair2.Key))
							{
								list2.Add(keyValuePair2.Value);
							}
						}
						foreach (KeyValuePair<string, MergedCompiledCommandParameter> keyValuePair3 in aliasedParameters)
						{
							if (namePattern.IsMatch(keyValuePair3.Key))
							{
								list2.Add(keyValuePair3.Value);
							}
						}
						if (list2.Count > 1)
						{
							if (!this._warningSet.Contains(text4 + ":::" + text5))
							{
								this._commandRuntime.WriteWarning(string.Format(CultureInfo.InvariantCulture, ParameterBinderStrings.MultipleParametersMatched, new object[]
								{
									text5
								}));
								this._warningSet.Add(text4 + ":::" + text5);
							}
						}
						else if (list2.Count == 1)
						{
							if (!dictionary.ContainsKey(list2[0]))
							{
								dictionary.Add(list2[0], keyValuePair.Value);
							}
							else if (!keyValuePair.Value.Equals(dictionary[list2[0]]))
							{
								if (!this._warningSet.Contains(text4 + ":::" + text5))
								{
									this._commandRuntime.WriteWarning(string.Format(CultureInfo.InvariantCulture, ParameterBinderStrings.DifferentValuesAssignedToSingleParameter, new object[]
									{
										text5
									}));
									this._warningSet.Add(text4 + ":::" + text5);
								}
								hashSet.Add(list2[0]);
							}
						}
					}
				}
			}
			if (list.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (object obj2 in list)
				{
					if (this.DefaultParameterValues.Contains(obj2))
					{
						this.DefaultParameterValues.Remove(obj2);
					}
					stringBuilder.Append(obj2.ToString() + ", ");
				}
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
				string format = (list.Count > 1) ? ParameterBinderStrings.MultipleKeysInBadFormat : ParameterBinderStrings.SingleKeyInBadFormat;
				this._commandRuntime.WriteWarning(string.Format(CultureInfo.InvariantCulture, format, new object[]
				{
					stringBuilder
				}));
			}
			foreach (MergedCompiledCommandParameter key2 in hashSet)
			{
				if (dictionary.ContainsKey(key2))
				{
					dictionary.Remove(key2);
				}
			}
			if (dictionary.Count > 0)
			{
				return dictionary;
			}
			return null;
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00010C30 File Offset: 0x0000EE30
		private void GetDefaultParameterValuePairsHelper(string cmdletName, string paramName, object paramValue, IDictionary<string, MergedCompiledCommandParameter> bindableParameters, IDictionary<string, MergedCompiledCommandParameter> bindableAlias, Dictionary<MergedCompiledCommandParameter, object> result, HashSet<MergedCompiledCommandParameter> parametersToRemove)
		{
			bool flag = false;
			MergedCompiledCommandParameter mergedCompiledCommandParameter;
			if (bindableParameters.TryGetValue(paramName, out mergedCompiledCommandParameter))
			{
				if (!result.ContainsKey(mergedCompiledCommandParameter))
				{
					result.Add(mergedCompiledCommandParameter, paramValue);
					return;
				}
				if (!paramValue.Equals(result[mergedCompiledCommandParameter]))
				{
					flag = true;
					parametersToRemove.Add(mergedCompiledCommandParameter);
				}
			}
			else if (bindableAlias.TryGetValue(paramName, out mergedCompiledCommandParameter))
			{
				if (!result.ContainsKey(mergedCompiledCommandParameter))
				{
					result.Add(mergedCompiledCommandParameter, paramValue);
					return;
				}
				if (!paramValue.Equals(result[mergedCompiledCommandParameter]))
				{
					flag = true;
					parametersToRemove.Add(mergedCompiledCommandParameter);
				}
			}
			if (flag && !this._warningSet.Contains(cmdletName + ":::" + paramName))
			{
				this._commandRuntime.WriteWarning(string.Format(CultureInfo.InvariantCulture, ParameterBinderStrings.DifferentValuesAssignedToSingleParameter, new object[]
				{
					paramName
				}));
				this._warningSet.Add(cmdletName + ":::" + paramName);
			}
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00010D14 File Offset: 0x0000EF14
		private void VerifyArgumentsProcessed(ParameterBindingException originalBindingException)
		{
			if (base.UnboundArguments.Count > 0)
			{
				CommandParameterInternal commandParameterInternal = base.UnboundArguments[0];
				Type typeSpecified = null;
				object argumentValue = commandParameterInternal.ArgumentValue;
				if (argumentValue != null && argumentValue != UnboundParameter.Value)
				{
					typeSpecified = argumentValue.GetType();
				}
				ParameterBindingException ex;
				if (commandParameterInternal.ParameterNameSpecified)
				{
					ex = new ParameterBindingException(ErrorCategory.InvalidArgument, this.Command.MyInvocation, base.GetParameterErrorExtent(commandParameterInternal), commandParameterInternal.ParameterName, null, typeSpecified, ParameterBinderStrings.NamedParameterNotFound, "NamedParameterNotFound", new object[0]);
				}
				else if (originalBindingException != null)
				{
					ex = originalBindingException;
				}
				else
				{
					string parameterName = "$null";
					if (commandParameterInternal.ArgumentValue != null)
					{
						try
						{
							parameterName = commandParameterInternal.ArgumentValue.ToString();
						}
						catch (Exception ex2)
						{
							CommandProcessorBase.CheckForSevereException(ex2);
							ex = new ParameterBindingArgumentTransformationException(ex2, ErrorCategory.InvalidData, base.InvocationInfo, null, null, null, commandParameterInternal.ArgumentValue.GetType(), ParameterBinderStrings.ParameterArgumentTransformationErrorMessageOnly, "ParameterArgumentTransformationErrorMessageOnly", new object[]
							{
								ex2.Message
							});
							if (!base.DefaultParameterBindingInUse)
							{
								throw ex;
							}
							base.ThrowElaboratedBindingException(ex);
						}
					}
					ex = new ParameterBindingException(ErrorCategory.InvalidArgument, this.Command.MyInvocation, null, parameterName, null, typeSpecified, ParameterBinderStrings.PositionalParameterNotFound, "PositionalParameterNotFound", new object[0]);
				}
				if (!base.DefaultParameterBindingInUse)
				{
					throw ex;
				}
				base.ThrowElaboratedBindingException(ex);
			}
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00010E60 File Offset: 0x0000F060
		private void VerifyParameterSetSelected()
		{
			if (base.BindableParameters.ParameterSetCount > 1 && this._currentParameterSetFlag == 4294967295U)
			{
				if ((this._currentParameterSetFlag & this._commandMetadata.DefaultParameterSetFlag) != 0U && this._commandMetadata.DefaultParameterSetFlag != 4294967295U)
				{
					ParameterBinderBase.bindingTracer.WriteLine("{0} valid parameter sets, using the DEFAULT PARAMETER SET: [{0}]", new object[]
					{
						base.BindableParameters.ParameterSetCount,
						this._commandMetadata.DefaultParameterSetName
					});
					this._currentParameterSetFlag = this._commandMetadata.DefaultParameterSetFlag;
					return;
				}
				ParameterBinderBase.bindingTracer.TraceError("ERROR: {0} valid parameter sets, but NOT DEFAULT PARAMETER SET.", new object[]
				{
					base.BindableParameters.ParameterSetCount
				});
				this.ThrowAmbiguousParameterSetException(this._currentParameterSetFlag, base.BindableParameters);
			}
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00010F34 File Offset: 0x0000F134
		private bool RestoreParameter(CommandParameterInternal argumentToBind, MergedCompiledCommandParameter parameter)
		{
			switch (parameter.BinderAssociation)
			{
			case ParameterBinderAssociation.DeclaredFormalParameters:
				base.DefaultParameterBinder.BindParameter(argumentToBind.ParameterName, argumentToBind.ArgumentValue);
				break;
			case ParameterBinderAssociation.DynamicParameters:
				if (this._dynamicParameterBinder != null)
				{
					this._dynamicParameterBinder.BindParameter(argumentToBind.ParameterName, argumentToBind.ArgumentValue);
				}
				break;
			case ParameterBinderAssociation.CommonParameters:
				this.CommonParametersBinder.BindParameter(argumentToBind.ParameterName, argumentToBind.ArgumentValue);
				break;
			case ParameterBinderAssociation.ShouldProcessParameters:
				this.ShouldProcessParametersBinder.BindParameter(argumentToBind.ParameterName, argumentToBind.ArgumentValue);
				break;
			case ParameterBinderAssociation.TransactionParameters:
				this.TransactionParametersBinder.BindParameter(argumentToBind.ParameterName, argumentToBind.ArgumentValue);
				break;
			case ParameterBinderAssociation.PagingParameters:
				this.PagingParametersBinder.BindParameter(argumentToBind.ParameterName, argumentToBind.ArgumentValue);
				break;
			}
			return true;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0001100C File Offset: 0x0000F20C
		private Collection<CommandParameterInternal> BindParameters(uint parameterSets, Collection<CommandParameterInternal> arguments)
		{
			Collection<CommandParameterInternal> collection = new Collection<CommandParameterInternal>();
			foreach (CommandParameterInternal commandParameterInternal in arguments)
			{
				if (!commandParameterInternal.ParameterNameSpecified)
				{
					collection.Add(commandParameterInternal);
				}
				else
				{
					MergedCompiledCommandParameter matchingParameter = base.BindableParameters.GetMatchingParameter(commandParameterInternal.ParameterName, false, true, new InvocationInfo(base.InvocationInfo.MyCommand, commandParameterInternal.ParameterExtent));
					if (matchingParameter != null)
					{
						if (base.BoundParameters.ContainsKey(matchingParameter.Parameter.Name))
						{
							ParameterBindingException ex = new ParameterBindingException(ErrorCategory.InvalidArgument, base.InvocationInfo, base.GetParameterErrorExtent(commandParameterInternal), commandParameterInternal.ParameterName, null, null, ParameterBinderStrings.ParameterAlreadyBound, "ParameterAlreadyBound", new object[0]);
							throw ex;
						}
						if ((matchingParameter.Parameter.ParameterSetFlags & parameterSets) == 0U && !matchingParameter.Parameter.IsInAllSets)
						{
							string parameterSetName = base.BindableParameters.GetParameterSetName(parameterSets);
							ParameterBindingException ex2 = new ParameterBindingException(ErrorCategory.InvalidArgument, this.Command.MyInvocation, null, commandParameterInternal.ParameterName, null, null, ParameterBinderStrings.ParameterNotInParameterSet, "ParameterNotInParameterSet", new object[]
							{
								parameterSetName
							});
							if (!base.DefaultParameterBindingInUse)
							{
								throw ex2;
							}
							base.ThrowElaboratedBindingException(ex2);
						}
						try
						{
							this.BindParameter(parameterSets, commandParameterInternal, matchingParameter, ParameterBindingFlags.ShouldCoerceType | ParameterBindingFlags.DelayBindScriptBlock);
							continue;
						}
						catch (ParameterBindingException pbex)
						{
							if (!base.DefaultParameterBindingInUse)
							{
								throw;
							}
							base.ThrowElaboratedBindingException(pbex);
							continue;
						}
					}
					if (commandParameterInternal.ParameterName.Equals("-%", StringComparison.Ordinal))
					{
						base.DefaultParameterBinder.CommandLineParameters.SetImplicitUsingParameters(commandParameterInternal.ArgumentValue);
					}
					else
					{
						collection.Add(commandParameterInternal);
					}
				}
			}
			return collection;
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x000111D8 File Offset: 0x0000F3D8
		private static bool IsParameterScriptBlockBindable(MergedCompiledCommandParameter parameter)
		{
			bool flag = false;
			Type type = parameter.Parameter.Type;
			if (type == typeof(object))
			{
				flag = true;
			}
			else if (type == typeof(ScriptBlock))
			{
				flag = true;
			}
			else if (type.IsSubclassOf(typeof(ScriptBlock)))
			{
				flag = true;
			}
			else
			{
				ParameterCollectionTypeInformation collectionTypeInformation = parameter.Parameter.CollectionTypeInformation;
				if (collectionTypeInformation.ParameterCollectionType != ParameterCollectionType.NotCollection)
				{
					if (collectionTypeInformation.ElementType == typeof(object))
					{
						flag = true;
					}
					else if (collectionTypeInformation.ElementType == typeof(ScriptBlock))
					{
						flag = true;
					}
					else if (collectionTypeInformation.ElementType.IsSubclassOf(typeof(ScriptBlock)))
					{
						flag = true;
					}
				}
			}
			CmdletParameterBinderController._tracer.WriteLine("IsParameterScriptBlockBindable: result = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x000112BC File Offset: 0x0000F4BC
		internal override Collection<CommandParameterInternal> BindParameters(Collection<CommandParameterInternal> parameters)
		{
			return this.BindParameters(uint.MaxValue, parameters);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x000112C8 File Offset: 0x0000F4C8
		internal override bool BindParameter(uint parameterSets, CommandParameterInternal argument, MergedCompiledCommandParameter parameter, ParameterBindingFlags flags)
		{
			bool flag = true;
			if ((flags & ParameterBindingFlags.DelayBindScriptBlock) != ParameterBindingFlags.None && parameter.Parameter.DoesParameterSetTakePipelineInput(parameterSets) && argument.ArgumentSpecified)
			{
				object argumentValue = argument.ArgumentValue;
				if ((argumentValue is ScriptBlock || argumentValue is CmdletParameterBinderController.DelayedScriptBlockArgument) && !CmdletParameterBinderController.IsParameterScriptBlockBindable(parameter))
				{
					if (this._commandRuntime.IsClosed && this._commandRuntime.InputPipe.Empty)
					{
						ParameterBindingException ex = new ParameterBindingException(ErrorCategory.MetadataError, this.Command.MyInvocation, base.GetErrorExtent(argument), parameter.Parameter.Name, parameter.Parameter.Type, null, ParameterBinderStrings.ScriptBlockArgumentNoInput, "ScriptBlockArgumentNoInput", new object[0]);
						throw ex;
					}
					ParameterBinderBase.bindingTracer.WriteLine("Adding ScriptBlock to delay-bind list for parameter '{0}'", new object[]
					{
						parameter.Parameter.Name
					});
					CmdletParameterBinderController.DelayedScriptBlockArgument delayedScriptBlockArgument = argumentValue as CmdletParameterBinderController.DelayedScriptBlockArgument;
					if (delayedScriptBlockArgument == null)
					{
						delayedScriptBlockArgument = new CmdletParameterBinderController.DelayedScriptBlockArgument
						{
							_argument = argument,
							_parameterBinder = this
						};
					}
					if (!this._delayBindScriptBlocks.ContainsKey(parameter))
					{
						this._delayBindScriptBlocks.Add(parameter, delayedScriptBlockArgument);
					}
					if (parameter.Parameter.ParameterSetFlags != 0U)
					{
						this._currentParameterSetFlag &= parameter.Parameter.ParameterSetFlags;
					}
					base.UnboundParameters.Remove(parameter);
					if (!base.BoundParameters.ContainsKey(parameter.Parameter.Name))
					{
						base.BoundParameters.Add(parameter.Parameter.Name, parameter);
					}
					if (!base.BoundArguments.ContainsKey(parameter.Parameter.Name))
					{
						base.BoundArguments.Add(parameter.Parameter.Name, argument);
					}
					if (base.DefaultParameterBinder.RecordBoundParameters && !base.DefaultParameterBinder.CommandLineParameters.ContainsKey(parameter.Parameter.Name))
					{
						base.DefaultParameterBinder.CommandLineParameters.Add(parameter.Parameter.Name, delayedScriptBlockArgument);
					}
					flag = false;
				}
			}
			bool result = false;
			if (flag)
			{
				try
				{
					result = this.BindParameter(argument, parameter, flags);
				}
				catch (Exception innerException)
				{
					bool flag2 = true;
					if ((flags & ParameterBindingFlags.ShouldCoerceType) == ParameterBindingFlags.None)
					{
						while (innerException != null)
						{
							if (innerException is PSInvalidCastException)
							{
								flag2 = false;
								break;
							}
							innerException = innerException.InnerException;
						}
					}
					if (flag2)
					{
						throw;
					}
				}
			}
			return result;
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00011518 File Offset: 0x0000F718
		private bool BindParameter(CommandParameterInternal argument, MergedCompiledCommandParameter parameter, ParameterBindingFlags flags)
		{
			bool flag = false;
			switch (parameter.BinderAssociation)
			{
			case ParameterBinderAssociation.DeclaredFormalParameters:
				flag = base.DefaultParameterBinder.BindParameter(argument, parameter.Parameter, flags);
				break;
			case ParameterBinderAssociation.DynamicParameters:
				if (this._dynamicParameterBinder != null)
				{
					flag = this._dynamicParameterBinder.BindParameter(argument, parameter.Parameter, flags);
				}
				break;
			case ParameterBinderAssociation.CommonParameters:
				flag = this.CommonParametersBinder.BindParameter(argument, parameter.Parameter, flags);
				break;
			case ParameterBinderAssociation.ShouldProcessParameters:
				flag = this.ShouldProcessParametersBinder.BindParameter(argument, parameter.Parameter, flags);
				break;
			case ParameterBinderAssociation.TransactionParameters:
				flag = this.TransactionParametersBinder.BindParameter(argument, parameter.Parameter, flags);
				break;
			case ParameterBinderAssociation.PagingParameters:
				flag = this.PagingParametersBinder.BindParameter(argument, parameter.Parameter, flags);
				break;
			}
			if (flag && (flags & ParameterBindingFlags.IsDefaultValue) == ParameterBindingFlags.None)
			{
				if (parameter.Parameter.ParameterSetFlags != 0U)
				{
					this._currentParameterSetFlag &= parameter.Parameter.ParameterSetFlags;
				}
				base.UnboundParameters.Remove(parameter);
				if (!base.BoundParameters.ContainsKey(parameter.Parameter.Name))
				{
					base.BoundParameters.Add(parameter.Parameter.Name, parameter);
				}
				if (!base.BoundArguments.ContainsKey(parameter.Parameter.Name))
				{
					base.BoundArguments.Add(parameter.Parameter.Name, argument);
				}
				if (parameter.Parameter.ObsoleteAttribute != null && (flags & ParameterBindingFlags.IsDefaultValue) == ParameterBindingFlags.None && !this.BoundObsoleteParameterNames.Contains(parameter.Parameter.Name))
				{
					string message = string.Format(CultureInfo.InvariantCulture, ParameterBinderStrings.UseOfDeprecatedParameterWarning, new object[]
					{
						parameter.Parameter.Name,
						parameter.Parameter.ObsoleteAttribute.Message
					});
					WarningRecord item = new WarningRecord("ParameterObsolete", message);
					this.BoundObsoleteParameterNames.Add(parameter.Parameter.Name);
					if (this.ObsoleteParameterWarningList == null)
					{
						this.ObsoleteParameterWarningList = new List<WarningRecord>();
					}
					this.ObsoleteParameterWarningList.Add(item);
				}
			}
			return flag;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0001172C File Offset: 0x0000F92C
		private void HandleRemainingArguments()
		{
			if (base.UnboundArguments.Count > 0)
			{
				MergedCompiledCommandParameter mergedCompiledCommandParameter = null;
				foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter2 in base.UnboundParameters)
				{
					ParameterSetSpecificMetadata parameterSetData = mergedCompiledCommandParameter2.Parameter.GetParameterSetData(this._currentParameterSetFlag);
					if (parameterSetData != null && parameterSetData.ValueFromRemainingArguments)
					{
						if (mergedCompiledCommandParameter != null)
						{
							ParameterBindingException ex = new ParameterBindingException(ErrorCategory.MetadataError, this.Command.MyInvocation, null, mergedCompiledCommandParameter2.Parameter.Name, mergedCompiledCommandParameter2.Parameter.Type, null, ParameterBinderStrings.AmbiguousParameterSet, "AmbiguousParameterSet", new object[0]);
							if (!base.DefaultParameterBindingInUse)
							{
								throw ex;
							}
							base.ThrowElaboratedBindingException(ex);
						}
						mergedCompiledCommandParameter = mergedCompiledCommandParameter2;
					}
				}
				if (mergedCompiledCommandParameter != null)
				{
					using (ParameterBinderBase.bindingTracer.TraceScope("BIND REMAININGARGUMENTS cmd line args to param: [{0}]", new object[]
					{
						mergedCompiledCommandParameter.Parameter.Name
					}))
					{
						List<object> list = new List<object>();
						foreach (CommandParameterInternal commandParameterInternal in base.UnboundArguments)
						{
							if (commandParameterInternal.ParameterNameSpecified)
							{
								list.Add(commandParameterInternal.ParameterText);
							}
							if (commandParameterInternal.ArgumentSpecified)
							{
								object argumentValue = commandParameterInternal.ArgumentValue;
								if (argumentValue != AutomationNull.Value && argumentValue != UnboundParameter.Value)
								{
									list.Add(argumentValue);
								}
							}
						}
						IScriptExtent argumentExtent = (base.UnboundArguments.Count == 1) ? base.UnboundArguments[0].ArgumentExtent : PositionUtilities.EmptyExtent;
						CommandParameterInternal commandParameterInternal2 = CommandParameterInternal.CreateParameterWithArgument(PositionUtilities.EmptyExtent, mergedCompiledCommandParameter.Parameter.Name, "-" + mergedCompiledCommandParameter.Parameter.Name + ":", argumentExtent, list, false, false);
						try
						{
							this.BindParameter(commandParameterInternal2, mergedCompiledCommandParameter, ParameterBindingFlags.ShouldCoerceType);
						}
						catch (ParameterBindingException pbex)
						{
							if (list.Count == 1 && list[0] is object[])
							{
								commandParameterInternal2.SetArgumentValue(base.UnboundArguments[0].ArgumentExtent, list[0]);
								this.BindParameter(commandParameterInternal2, mergedCompiledCommandParameter, ParameterBindingFlags.ShouldCoerceType);
							}
							else
							{
								if (!base.DefaultParameterBindingInUse)
								{
									throw;
								}
								base.ThrowElaboratedBindingException(pbex);
							}
						}
						base.UnboundArguments.Clear();
					}
				}
			}
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x000119DC File Offset: 0x0000FBDC
		private void HandleCommandLineDynamicParameters(out ParameterBindingException outgoingBindingException)
		{
			outgoingBindingException = null;
			if (this._commandMetadata.ImplementsDynamicParameters)
			{
				using (ParameterBinderBase.bindingTracer.TraceScope("BIND cmd line args to DYNAMIC parameters.", new object[0]))
				{
					CmdletParameterBinderController._tracer.WriteLine("The Cmdlet supports the dynamic parameter interface", new object[0]);
					IDynamicParameters dynamicParameters = this.Command as IDynamicParameters;
					if (dynamicParameters != null)
					{
						if (this._dynamicParameterBinder == null)
						{
							CmdletParameterBinderController._tracer.WriteLine("Getting the bindable object from the Cmdlet", new object[0]);
							PSScriptCmdlet psscriptCmdlet = this.Command as PSScriptCmdlet;
							if (psscriptCmdlet != null)
							{
								psscriptCmdlet.PrepareForBinding(((ScriptParameterBinder)base.DefaultParameterBinder).LocalScope, base.CommandLineParameters);
							}
							object dynamicParameters2;
							try
							{
								dynamicParameters2 = dynamicParameters.GetDynamicParameters();
							}
							catch (Exception ex)
							{
								CommandProcessorBase.CheckForSevereException(ex);
								if (ex is ProviderInvocationException)
								{
									throw;
								}
								ParameterBindingException ex2 = new ParameterBindingException(ex, ErrorCategory.InvalidArgument, this.Command.MyInvocation, null, null, null, null, ParameterBinderStrings.GetDynamicParametersException, "GetDynamicParametersException", new object[]
								{
									ex.Message
								});
								throw ex2;
							}
							if (dynamicParameters2 != null)
							{
								ParameterBinderBase.bindingTracer.WriteLine("DYNAMIC parameter object: [{0}]", new object[]
								{
									dynamicParameters2.GetType()
								});
								CmdletParameterBinderController._tracer.WriteLine("Creating a new parameter binder for the dynamic parameter object", new object[0]);
								RuntimeDefinedParameterDictionary runtimeDefinedParameterDictionary = dynamicParameters2 as RuntimeDefinedParameterDictionary;
								InternalParameterMetadata parameterMetadata;
								if (runtimeDefinedParameterDictionary != null)
								{
									parameterMetadata = InternalParameterMetadata.Get(runtimeDefinedParameterDictionary, true, true);
									this._dynamicParameterBinder = new RuntimeDefinedParameterBinder(runtimeDefinedParameterDictionary, this.Command, base.CommandLineParameters);
								}
								else
								{
									parameterMetadata = InternalParameterMetadata.Get(dynamicParameters2.GetType(), base.Context, true);
									this._dynamicParameterBinder = new ReflectionParameterBinder(dynamicParameters2, this.Command, base.CommandLineParameters);
								}
								Collection<MergedCompiledCommandParameter> collection = base.BindableParameters.AddMetadataForBinder(parameterMetadata, ParameterBinderAssociation.DynamicParameters);
								foreach (MergedCompiledCommandParameter item in collection)
								{
									base.UnboundParameters.Add(item);
								}
								this._commandMetadata.DefaultParameterSetFlag = base.BindableParameters.GenerateParameterSetMappingFromMetadata(this._commandMetadata.DefaultParameterSetName);
							}
						}
						if (this._dynamicParameterBinder == null)
						{
							CmdletParameterBinderController._tracer.WriteLine("No dynamic parameter object was returned from the Cmdlet", new object[0]);
						}
						else if (base.UnboundArguments.Count > 0)
						{
							using (ParameterBinderBase.bindingTracer.TraceScope("BIND NAMED args to DYNAMIC parameters", new object[0]))
							{
								base.ReparseUnboundArguments();
								base.UnboundArguments = this.BindParameters(this._currentParameterSetFlag, base.UnboundArguments);
							}
							using (ParameterBinderBase.bindingTracer.TraceScope("BIND POSITIONAL args to DYNAMIC parameters", new object[0]))
							{
								base.UnboundArguments = base.BindPositionalParameters(base.UnboundArguments, this._currentParameterSetFlag, this._commandMetadata.DefaultParameterSetFlag, out outgoingBindingException);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00011D28 File Offset: 0x0000FF28
		private Collection<MergedCompiledCommandParameter> GetMissingMandatoryParameters(int validParameterSetCount, bool isPipelineInputExpected)
		{
			Collection<MergedCompiledCommandParameter> collection = new Collection<MergedCompiledCommandParameter>();
			uint defaultParameterSetFlag = this._commandMetadata.DefaultParameterSetFlag;
			uint num = 0U;
			Dictionary<uint, ParameterSetPromptingData> dictionary = new Dictionary<uint, ParameterSetPromptingData>();
			bool flag = false;
			bool flag2 = false;
			foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter in base.UnboundParameters)
			{
				if (mergedCompiledCommandParameter.Parameter.IsMandatoryInSomeParameterSet)
				{
					IEnumerable<ParameterSetSpecificMetadata> matchingParameterSetData = mergedCompiledCommandParameter.Parameter.GetMatchingParameterSetData(this._currentParameterSetFlag);
					uint num2 = 0U;
					bool flag3 = false;
					foreach (ParameterSetSpecificMetadata parameterSetMetadata in matchingParameterSetData)
					{
						uint num3 = this.NewParameterSetPromptingData(dictionary, mergedCompiledCommandParameter, parameterSetMetadata, defaultParameterSetFlag, isPipelineInputExpected);
						if (num3 != 0U)
						{
							flag = true;
							flag3 = true;
							if (num3 != 4294967295U)
							{
								num2 |= (this._currentParameterSetFlag & num3);
								num |= (this._currentParameterSetFlag & num2);
							}
							else
							{
								flag2 = true;
							}
						}
					}
					if (!isPipelineInputExpected && flag3)
					{
						collection.Add(mergedCompiledCommandParameter);
					}
				}
			}
			if (flag && isPipelineInputExpected)
			{
				if (num == 0U)
				{
					num = this._currentParameterSetFlag;
				}
				if (flag2)
				{
					uint num4 = base.BindableParameters.AllParameterSetFlags;
					if (num4 == 0U)
					{
						num4 = uint.MaxValue;
					}
					num = (this._currentParameterSetFlag & num4);
				}
				if (validParameterSetCount > 1 && defaultParameterSetFlag != 0U && (defaultParameterSetFlag & num) == 0U && (defaultParameterSetFlag & this._currentParameterSetFlag) != 0U)
				{
					uint num5 = 0U;
					foreach (ParameterSetPromptingData parameterSetPromptingData in dictionary.Values)
					{
						if ((parameterSetPromptingData.ParameterSet & this._currentParameterSetFlag) != 0U && (parameterSetPromptingData.ParameterSet & defaultParameterSetFlag) == 0U && !parameterSetPromptingData.IsAllSet && parameterSetPromptingData.PipelineableMandatoryParameters.Count > 0)
						{
							num5 = parameterSetPromptingData.ParameterSet;
							break;
						}
					}
					if (num5 == 0U)
					{
						num = (this._currentParameterSetFlag & ~num);
						this._currentParameterSetFlag = num;
						if (this._currentParameterSetFlag == defaultParameterSetFlag)
						{
							this.Command.SetParameterSetName(this.CurrentParameterSetName);
						}
						else
						{
							this._parameterSetToBePrioritizedInPipelingBinding = defaultParameterSetFlag;
						}
					}
				}
				int num6 = CmdletParameterBinderController.ValidParameterSetCount(num);
				if (num6 == 0)
				{
					this.ThrowAmbiguousParameterSetException(this._currentParameterSetFlag, base.BindableParameters);
				}
				else
				{
					if (num6 == 1)
					{
						using (Dictionary<uint, ParameterSetPromptingData>.ValueCollection.Enumerator enumerator4 = dictionary.Values.GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								ParameterSetPromptingData parameterSetPromptingData2 = enumerator4.Current;
								if ((parameterSetPromptingData2.ParameterSet & num) != 0U || parameterSetPromptingData2.IsAllSet)
								{
									foreach (MergedCompiledCommandParameter item in parameterSetPromptingData2.NonpipelineableMandatoryParameters.Keys)
									{
										collection.Add(item);
									}
								}
							}
							return collection;
						}
					}
					if (this._parameterSetToBePrioritizedInPipelingBinding == 0U)
					{
						bool flag4 = false;
						if (defaultParameterSetFlag != 0U && (num & defaultParameterSetFlag) != 0U)
						{
							bool flag5 = false;
							foreach (ParameterSetPromptingData parameterSetPromptingData3 in dictionary.Values)
							{
								if (!parameterSetPromptingData3.IsAllSet && !parameterSetPromptingData3.IsDefaultSet && parameterSetPromptingData3.PipelineableMandatoryParameters.Count > 0 && parameterSetPromptingData3.NonpipelineableMandatoryParameters.Count == 0)
								{
									flag5 = true;
									break;
								}
							}
							bool flag6 = false;
							foreach (ParameterSetPromptingData parameterSetPromptingData4 in dictionary.Values)
							{
								if (!parameterSetPromptingData4.IsAllSet && !parameterSetPromptingData4.IsDefaultSet && parameterSetPromptingData4.PipelineableMandatoryByPropertyNameParameters.Count > 0)
								{
									flag6 = true;
									break;
								}
							}
							ParameterSetPromptingData parameterSetPromptingData5;
							if (dictionary.TryGetValue(defaultParameterSetFlag, out parameterSetPromptingData5))
							{
								bool flag7 = parameterSetPromptingData5.PipelineableMandatoryParameters.Count > 0;
								bool flag8 = parameterSetPromptingData5.PipelineableMandatoryByPropertyNameParameters.Count > 0;
								if (flag8 && !flag6)
								{
									flag4 = true;
								}
								else if (flag7 && !flag5)
								{
									flag4 = true;
								}
							}
							if (!flag4 && !flag5)
							{
								flag4 = true;
							}
							ParameterSetPromptingData parameterSetPromptingData6;
							if (!flag4 && dictionary.TryGetValue(4294967295U, out parameterSetPromptingData6) && parameterSetPromptingData6.NonpipelineableMandatoryParameters.Count > 0)
							{
								flag4 = true;
							}
							if (flag4)
							{
								num = defaultParameterSetFlag;
								this._currentParameterSetFlag = defaultParameterSetFlag;
								this.Command.SetParameterSetName(this.CurrentParameterSetName);
								foreach (ParameterSetPromptingData parameterSetPromptingData7 in dictionary.Values)
								{
									if ((parameterSetPromptingData7.ParameterSet & num) != 0U || parameterSetPromptingData7.IsAllSet)
									{
										foreach (MergedCompiledCommandParameter item2 in parameterSetPromptingData7.NonpipelineableMandatoryParameters.Keys)
										{
											collection.Add(item2);
										}
									}
								}
							}
						}
						if (!flag4)
						{
							uint num7 = 0U;
							uint num8 = 0U;
							bool flag9 = false;
							bool flag10 = false;
							foreach (ParameterSetPromptingData parameterSetPromptingData8 in dictionary.Values)
							{
								if ((parameterSetPromptingData8.ParameterSet & num) != 0U && !parameterSetPromptingData8.IsAllSet && parameterSetPromptingData8.PipelineableMandatoryByValueParameters.Count > 0)
								{
									if (flag9)
									{
										flag10 = true;
										num7 = 0U;
										break;
									}
									num7 = parameterSetPromptingData8.ParameterSet;
									flag9 = true;
								}
							}
							bool flag11 = false;
							bool flag12 = false;
							foreach (ParameterSetPromptingData parameterSetPromptingData9 in dictionary.Values)
							{
								if ((parameterSetPromptingData9.ParameterSet & num) != 0U && !parameterSetPromptingData9.IsAllSet && parameterSetPromptingData9.PipelineableMandatoryByPropertyNameParameters.Count > 0)
								{
									if (flag11)
									{
										flag12 = true;
										num8 = 0U;
										break;
									}
									num8 = parameterSetPromptingData9.ParameterSet;
									flag11 = true;
								}
							}
							uint num9 = 0U;
							if (flag9 && flag11 && num7 == num8)
							{
								num9 = num7;
							}
							if (flag9 ^ flag11)
							{
								num9 = (flag9 ? num7 : num8);
							}
							if (num9 != 0U)
							{
								num = num9;
								uint num10 = 0U;
								bool chosenSetContainsNonpipelineableMandatoryParameters = false;
								foreach (ParameterSetPromptingData parameterSetPromptingData10 in dictionary.Values)
								{
									if ((parameterSetPromptingData10.ParameterSet & num) != 0U || parameterSetPromptingData10.IsAllSet)
									{
										if (!parameterSetPromptingData10.IsAllSet)
										{
											chosenSetContainsNonpipelineableMandatoryParameters = (parameterSetPromptingData10.NonpipelineableMandatoryParameters.Count > 0);
										}
										using (Dictionary<MergedCompiledCommandParameter, ParameterSetSpecificMetadata>.KeyCollection.Enumerator enumerator5 = parameterSetPromptingData10.NonpipelineableMandatoryParameters.Keys.GetEnumerator())
										{
											while (enumerator5.MoveNext())
											{
												MergedCompiledCommandParameter item3 = enumerator5.Current;
												collection.Add(item3);
											}
											continue;
										}
									}
									num10 |= parameterSetPromptingData10.ParameterSet;
								}
								this.PreservePotentialParameterSets(num9, num10, chosenSetContainsNonpipelineableMandatoryParameters);
							}
							else
							{
								bool flag13 = false;
								uint num11 = 0U;
								foreach (ParameterSetPromptingData parameterSetPromptingData11 in dictionary.Values)
								{
									if (((parameterSetPromptingData11.ParameterSet & num) != 0U || parameterSetPromptingData11.IsAllSet) && parameterSetPromptingData11.NonpipelineableMandatoryParameters.Count > 0)
									{
										flag13 = true;
										if (!parameterSetPromptingData11.IsAllSet)
										{
											num11 |= parameterSetPromptingData11.ParameterSet;
										}
									}
								}
								if (flag13)
								{
									if (num7 != 0U)
									{
										num = num7;
										uint num12 = 0U;
										bool chosenSetContainsNonpipelineableMandatoryParameters2 = false;
										foreach (ParameterSetPromptingData parameterSetPromptingData12 in dictionary.Values)
										{
											if ((parameterSetPromptingData12.ParameterSet & num) != 0U || parameterSetPromptingData12.IsAllSet)
											{
												if (!parameterSetPromptingData12.IsAllSet)
												{
													chosenSetContainsNonpipelineableMandatoryParameters2 = (parameterSetPromptingData12.NonpipelineableMandatoryParameters.Count > 0);
												}
												using (Dictionary<MergedCompiledCommandParameter, ParameterSetSpecificMetadata>.KeyCollection.Enumerator enumerator5 = parameterSetPromptingData12.NonpipelineableMandatoryParameters.Keys.GetEnumerator())
												{
													while (enumerator5.MoveNext())
													{
														MergedCompiledCommandParameter item4 = enumerator5.Current;
														collection.Add(item4);
													}
													continue;
												}
											}
											num12 |= parameterSetPromptingData12.ParameterSet;
										}
										this.PreservePotentialParameterSets(num7, num12, chosenSetContainsNonpipelineableMandatoryParameters2);
									}
									else
									{
										if (!flag10 && !flag12)
										{
											this.ThrowAmbiguousParameterSetException(this._currentParameterSetFlag, base.BindableParameters);
										}
										if (num11 != 0U)
										{
											this.IgnoreOtherMandatoryParameterSets(num11);
											if (this._currentParameterSetFlag == 0U)
											{
												this.ThrowAmbiguousParameterSetException(this._currentParameterSetFlag, base.BindableParameters);
											}
											if (CmdletParameterBinderController.ValidParameterSetCount(this._currentParameterSetFlag) == 1)
											{
												this.Command.SetParameterSetName(this.CurrentParameterSetName);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return collection;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00012630 File Offset: 0x00010830
		private void PreservePotentialParameterSets(uint chosenMandatorySet, uint otherMandatorySetsToBeIgnored, bool chosenSetContainsNonpipelineableMandatoryParameters)
		{
			if (chosenSetContainsNonpipelineableMandatoryParameters)
			{
				this._currentParameterSetFlag = chosenMandatorySet;
				this.Command.SetParameterSetName(this.CurrentParameterSetName);
				return;
			}
			this.IgnoreOtherMandatoryParameterSets(otherMandatorySetsToBeIgnored);
			this.Command.SetParameterSetName(this.CurrentParameterSetName);
			if (this._currentParameterSetFlag != chosenMandatorySet)
			{
				this._parameterSetToBePrioritizedInPipelingBinding = chosenMandatorySet;
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00012684 File Offset: 0x00010884
		private void IgnoreOtherMandatoryParameterSets(uint otherMandatorySetsToBeIgnored)
		{
			if (otherMandatorySetsToBeIgnored == 0U)
			{
				return;
			}
			if (this._currentParameterSetFlag == 4294967295U)
			{
				uint allParameterSetFlags = base.BindableParameters.AllParameterSetFlags;
				this._currentParameterSetFlag = (allParameterSetFlags & ~otherMandatorySetsToBeIgnored);
				return;
			}
			this._currentParameterSetFlag &= ~otherMandatorySetsToBeIgnored;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x000126C4 File Offset: 0x000108C4
		private uint NewParameterSetPromptingData(Dictionary<uint, ParameterSetPromptingData> promptingData, MergedCompiledCommandParameter parameter, ParameterSetSpecificMetadata parameterSetMetadata, uint defaultParameterSet, bool pipelineInputExpected)
		{
			uint num = 0U;
			uint num2 = parameterSetMetadata.ParameterSetFlag;
			if (num2 == 0U)
			{
				num2 = uint.MaxValue;
			}
			bool isDefaultSet = defaultParameterSet != 0U && (defaultParameterSet & num2) != 0U;
			bool flag = false;
			if (parameterSetMetadata.IsMandatory)
			{
				num |= num2;
				flag = true;
			}
			bool flag2 = false;
			if (pipelineInputExpected && (parameterSetMetadata.ValueFromPipeline || parameterSetMetadata.ValueFromPipelineByPropertyName))
			{
				flag2 = true;
			}
			if (flag)
			{
				ParameterSetPromptingData parameterSetPromptingData;
				if (!promptingData.TryGetValue(num2, out parameterSetPromptingData))
				{
					parameterSetPromptingData = new ParameterSetPromptingData(num2, isDefaultSet);
					promptingData.Add(num2, parameterSetPromptingData);
				}
				if (flag2)
				{
					parameterSetPromptingData.PipelineableMandatoryParameters[parameter] = parameterSetMetadata;
					if (parameterSetMetadata.ValueFromPipeline)
					{
						parameterSetPromptingData.PipelineableMandatoryByValueParameters[parameter] = parameterSetMetadata;
					}
					if (parameterSetMetadata.ValueFromPipelineByPropertyName)
					{
						parameterSetPromptingData.PipelineableMandatoryByPropertyNameParameters[parameter] = parameterSetMetadata;
					}
				}
				else
				{
					parameterSetPromptingData.NonpipelineableMandatoryParameters[parameter] = parameterSetMetadata;
				}
			}
			return num;
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0001278C File Offset: 0x0001098C
		private int ValidateParameterSets(bool prePipelineInput, bool setDefault)
		{
			int num = CmdletParameterBinderController.ValidParameterSetCount(this._currentParameterSetFlag);
			if (num == 0 && this._currentParameterSetFlag != 4294967295U)
			{
				this.ThrowAmbiguousParameterSetException(this._currentParameterSetFlag, base.BindableParameters);
			}
			else if (num > 1)
			{
				uint defaultParameterSetFlag = this._commandMetadata.DefaultParameterSetFlag;
				bool flag = defaultParameterSetFlag != 0U;
				bool flag2 = this._currentParameterSetFlag == uint.MaxValue;
				bool flag3 = this._currentParameterSetFlag == defaultParameterSetFlag;
				if (flag2 && !flag)
				{
					num = 1;
				}
				else if ((!prePipelineInput && flag3) || (flag && (this._currentParameterSetFlag & defaultParameterSetFlag) != 0U))
				{
					string parameterSetName = base.BindableParameters.GetParameterSetName(defaultParameterSetFlag);
					this.Command.SetParameterSetName(parameterSetName);
					if (setDefault)
					{
						this._currentParameterSetFlag = this._commandMetadata.DefaultParameterSetFlag;
						num = 1;
					}
				}
				else if (!prePipelineInput || !this.AtLeastOneUnboundValidParameterSetTakesPipelineInput(this._currentParameterSetFlag))
				{
					int num2 = this.ResolveParameterSetAmbiguityBasedOnMandatoryParameters();
					if (num2 != 1)
					{
						this.ThrowAmbiguousParameterSetException(this._currentParameterSetFlag, base.BindableParameters);
					}
					num = num2;
				}
			}
			else
			{
				if (this._currentParameterSetFlag == 4294967295U)
				{
					num = ((base.BindableParameters.ParameterSetCount > 0) ? base.BindableParameters.ParameterSetCount : 1);
					if (!prePipelineInput || !this.AtLeastOneUnboundValidParameterSetTakesPipelineInput(this._currentParameterSetFlag))
					{
						if (this._commandMetadata.DefaultParameterSetFlag != 0U)
						{
							if (setDefault)
							{
								this._currentParameterSetFlag = this._commandMetadata.DefaultParameterSetFlag;
								num = 1;
							}
						}
						else if (num > 1)
						{
							int num3 = this.ResolveParameterSetAmbiguityBasedOnMandatoryParameters();
							if (num3 != 1)
							{
								this.ThrowAmbiguousParameterSetException(this._currentParameterSetFlag, base.BindableParameters);
							}
							num = num3;
						}
					}
				}
				this.Command.SetParameterSetName(this.CurrentParameterSetName);
			}
			return num;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00012921 File Offset: 0x00010B21
		private int ResolveParameterSetAmbiguityBasedOnMandatoryParameters()
		{
			return CmdletParameterBinderController.ResolveParameterSetAmbiguityBasedOnMandatoryParameters(base.BoundParameters, base.UnboundParameters, base.BindableParameters, ref this._currentParameterSetFlag, this.Command);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00012974 File Offset: 0x00010B74
		internal static int ResolveParameterSetAmbiguityBasedOnMandatoryParameters(Dictionary<string, MergedCompiledCommandParameter> boundParameters, ICollection<MergedCompiledCommandParameter> unboundParameters, MergedCommandParameterMetadata bindableParameters, ref uint _currentParameterSetFlag, Cmdlet command)
		{
			uint num = _currentParameterSetFlag;
			IEnumerable<ParameterSetSpecificMetadata> enumerable = boundParameters.Values.Concat(unboundParameters).SelectMany((MergedCompiledCommandParameter p) => p.Parameter.ParameterSetData.Values);
			uint num2 = 0U;
			foreach (ParameterSetSpecificMetadata parameterSetSpecificMetadata in enumerable)
			{
				num2 |= parameterSetSpecificMetadata.ParameterSetFlag;
			}
			num &= num2;
			IEnumerable<ParameterSetSpecificMetadata> enumerable2 = from p in unboundParameters.SelectMany((MergedCompiledCommandParameter p) => p.Parameter.ParameterSetData.Values)
			where p.IsMandatory
			select p;
			foreach (ParameterSetSpecificMetadata parameterSetSpecificMetadata2 in enumerable2)
			{
				num &= ~parameterSetSpecificMetadata2.ParameterSetFlag;
			}
			int num3 = CmdletParameterBinderController.ValidParameterSetCount(num);
			if (num3 == 1)
			{
				_currentParameterSetFlag = num;
				if (command != null)
				{
					string parameterSetName = bindableParameters.GetParameterSetName(_currentParameterSetFlag);
					command.SetParameterSetName(parameterSetName);
				}
				return num3;
			}
			return -1;
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00012AB0 File Offset: 0x00010CB0
		private void ThrowAmbiguousParameterSetException(uint parameterSetFlags, MergedCommandParameterMetadata bindableParameters)
		{
			ParameterBindingException ex = new ParameterBindingException(ErrorCategory.InvalidArgument, this.Command.MyInvocation, null, null, null, null, ParameterBinderStrings.AmbiguousParameterSet, "AmbiguousParameterSet", new object[0]);
			uint num = 1U;
			while (parameterSetFlags != 0U)
			{
				uint num2 = parameterSetFlags & 1U;
				if (num2 == 1U)
				{
					string parameterSetName = bindableParameters.GetParameterSetName(num);
					if (!string.IsNullOrEmpty(parameterSetName))
					{
						ParameterBinderBase.bindingTracer.WriteLine("Remaining valid parameter set: {0}", new object[]
						{
							parameterSetName
						});
					}
				}
				parameterSetFlags >>= 1;
				num <<= 1;
			}
			if (!base.DefaultParameterBindingInUse)
			{
				throw ex;
			}
			base.ThrowElaboratedBindingException(ex);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00012B3C File Offset: 0x00010D3C
		private bool AtLeastOneUnboundValidParameterSetTakesPipelineInput(uint validParameterSetFlags)
		{
			bool result = false;
			foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter in base.UnboundParameters)
			{
				if (mergedCompiledCommandParameter.Parameter.DoesParameterSetTakePipelineInput(validParameterSetFlags))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00012BA0 File Offset: 0x00010DA0
		internal bool HandleUnboundMandatoryParameters(out Collection<MergedCompiledCommandParameter> missingMandatoryParameters)
		{
			return this.HandleUnboundMandatoryParameters(CmdletParameterBinderController.ValidParameterSetCount(this._currentParameterSetFlag), false, false, false, out missingMandatoryParameters);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00012BB8 File Offset: 0x00010DB8
		internal bool HandleUnboundMandatoryParameters(int validParameterSetCount, bool processMissingMandatory, bool promptForMandatory, bool isPipelineInputExpected, out Collection<MergedCompiledCommandParameter> missingMandatoryParameters)
		{
			bool result = true;
			missingMandatoryParameters = this.GetMissingMandatoryParameters(validParameterSetCount, isPipelineInputExpected);
			if (missingMandatoryParameters.Count > 0)
			{
				if (processMissingMandatory)
				{
					if (base.Context.EngineHostInterface == null || !promptForMandatory)
					{
						ParameterBinderBase.bindingTracer.WriteLine("ERROR: host does not support prompting for missing mandatory parameters", new object[0]);
						string parameterName = CmdletParameterBinderController.BuildMissingParamsString(missingMandatoryParameters);
						ParameterBindingException ex = new ParameterBindingException(ErrorCategory.InvalidArgument, this.Command.MyInvocation, null, parameterName, null, null, ParameterBinderStrings.MissingMandatoryParameter, "MissingMandatoryParameter", new object[0]);
						throw ex;
					}
					Collection<FieldDescription> fieldDescriptionList = this.CreatePromptDataStructures(missingMandatoryParameters);
					Dictionary<string, PSObject> dictionary = this.PromptForMissingMandatoryParameters(fieldDescriptionList, missingMandatoryParameters);
					using (ParameterBinderBase.bindingTracer.TraceScope("BIND PROMPTED mandatory parameter args", new object[0]))
					{
						foreach (KeyValuePair<string, PSObject> keyValuePair in dictionary)
						{
							CommandParameterInternal argument = CommandParameterInternal.CreateParameterWithArgument(PositionUtilities.EmptyExtent, keyValuePair.Key, "-" + keyValuePair.Key + ":", PositionUtilities.EmptyExtent, keyValuePair.Value, false, false);
							result = this.BindParameter(argument, ParameterBindingFlags.ShouldCoerceType | ParameterBindingFlags.ThrowOnParameterNotFound);
						}
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00012D08 File Offset: 0x00010F08
		private Dictionary<string, PSObject> PromptForMissingMandatoryParameters(Collection<FieldDescription> fieldDescriptionList, Collection<MergedCompiledCommandParameter> missingMandatoryParameters)
		{
			Dictionary<string, PSObject> dictionary = null;
			Exception ex = null;
			try
			{
				ParameterBinderBase.bindingTracer.WriteLine("PROMPTING for missing mandatory parameters using the host", new object[0]);
				string promptMessage = ParameterBinderStrings.PromptMessage;
				InvocationInfo myInvocation = this.Command.MyInvocation;
				string caption = StringUtil.Format(ParameterBinderStrings.PromptCaption, myInvocation.MyCommand.Name, myInvocation.PipelinePosition);
				dictionary = base.Context.EngineHostInterface.UI.Prompt(caption, promptMessage, fieldDescriptionList);
			}
			catch (NotImplementedException ex2)
			{
				ex = ex2;
			}
			catch (HostException ex3)
			{
				ex = ex3;
			}
			catch (PSInvalidOperationException ex4)
			{
				ex = ex4;
			}
			if (ex != null)
			{
				ParameterBinderBase.bindingTracer.WriteLine("ERROR: host does not support prompting for missing mandatory parameters", new object[0]);
				string parameterName = CmdletParameterBinderController.BuildMissingParamsString(missingMandatoryParameters);
				ParameterBindingException ex5 = new ParameterBindingException(ErrorCategory.InvalidArgument, this.Command.MyInvocation, null, parameterName, null, null, ParameterBinderStrings.MissingMandatoryParameter, "MissingMandatoryParameter", new object[0]);
				throw ex5;
			}
			if (dictionary == null || dictionary.Count == 0)
			{
				ParameterBinderBase.bindingTracer.WriteLine("ERROR: still missing mandatory parameters after PROMPTING", new object[0]);
				string parameterName2 = CmdletParameterBinderController.BuildMissingParamsString(missingMandatoryParameters);
				ParameterBindingException ex6 = new ParameterBindingException(ErrorCategory.InvalidArgument, this.Command.MyInvocation, null, parameterName2, null, null, ParameterBinderStrings.MissingMandatoryParameter, "MissingMandatoryParameter", new object[0]);
				throw ex6;
			}
			return dictionary;
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00012E5C File Offset: 0x0001105C
		internal static string BuildMissingParamsString(Collection<MergedCompiledCommandParameter> missingMandatoryParameters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter in missingMandatoryParameters)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " {0}", new object[]
				{
					mergedCompiledCommandParameter.Parameter.Name
				});
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00012ED0 File Offset: 0x000110D0
		private Collection<FieldDescription> CreatePromptDataStructures(Collection<MergedCompiledCommandParameter> missingMandatoryParameters)
		{
			StringBuilder usedHotKeys = new StringBuilder();
			Collection<FieldDescription> collection = new Collection<FieldDescription>();
			foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter in missingMandatoryParameters)
			{
				ParameterSetSpecificMetadata parameterSetData = mergedCompiledCommandParameter.Parameter.GetParameterSetData(this._currentParameterSetFlag);
				FieldDescription fieldDescription = new FieldDescription(mergedCompiledCommandParameter.Parameter.Name);
				string text = null;
				try
				{
					text = parameterSetData.GetHelpMessage(this.Command);
				}
				catch (InvalidOperationException)
				{
				}
				catch (ArgumentException)
				{
				}
				if (!string.IsNullOrEmpty(text))
				{
					fieldDescription.HelpMessage = text;
				}
				fieldDescription.SetParameterType(mergedCompiledCommandParameter.Parameter.Type);
				fieldDescription.Label = CmdletParameterBinderController.BuildLabel(mergedCompiledCommandParameter.Parameter.Name, usedHotKeys);
				foreach (ValidateArgumentsAttribute item in mergedCompiledCommandParameter.Parameter.ValidationAttributes)
				{
					fieldDescription.Attributes.Add(item);
				}
				foreach (ArgumentTransformationAttribute item2 in mergedCompiledCommandParameter.Parameter.ArgumentTransformationAttributes)
				{
					fieldDescription.Attributes.Add(item2);
				}
				fieldDescription.IsMandatory = true;
				collection.Add(fieldDescription);
			}
			return collection;
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x000130A4 File Offset: 0x000112A4
		private static string BuildLabel(string parameterName, StringBuilder usedHotKeys)
		{
			bool flag = false;
			StringBuilder stringBuilder = new StringBuilder(parameterName);
			string text = usedHotKeys.ToString();
			for (int i = 0; i < parameterName.Length; i++)
			{
				if (char.IsUpper(parameterName[i]) && text.IndexOf(parameterName[i]) == -1)
				{
					stringBuilder.Insert(i, '&');
					usedHotKeys.Append(parameterName[i]);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				for (int j = 0; j < parameterName.Length; j++)
				{
					if (char.IsLower(parameterName[j]) && text.IndexOf(parameterName[j]) == -1)
					{
						stringBuilder.Insert(j, '&');
						usedHotKeys.Append(parameterName[j]);
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				for (int k = 0; k < parameterName.Length; k++)
				{
					if (!char.IsLetter(parameterName[k]) && text.IndexOf(parameterName[k]) == -1)
					{
						stringBuilder.Insert(k, '&');
						usedHotKeys.Append(parameterName[k]);
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				stringBuilder.Insert(0, '&');
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x000131CC File Offset: 0x000113CC
		internal string CurrentParameterSetName
		{
			get
			{
				string parameterSetName = base.BindableParameters.GetParameterSetName(this._currentParameterSetFlag);
				CmdletParameterBinderController._tracer.WriteLine("CurrentParameterSetName = {0}", new object[]
				{
					parameterSetName
				});
				return parameterSetName;
			}
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00013208 File Offset: 0x00011408
		internal bool BindPipelineParameters(PSObject inputToOperateOn)
		{
			bool flag5;
			try
			{
				using (ParameterBinderBase.bindingTracer.TraceScope("BIND PIPELINE object to parameters: [{0}]", new object[]
				{
					this._commandMetadata.Name
				}))
				{
					bool flag2;
					bool flag = this.InvokeAndBindDelayBindScriptBlock(inputToOperateOn, out flag2);
					bool flag3 = !flag2 || flag;
					bool flag4 = false;
					if (flag3)
					{
						flag4 = this.BindPipelineParametersPrivate(inputToOperateOn);
					}
					flag5 = ((flag2 && flag) || flag4);
				}
			}
			catch (ParameterBindingException)
			{
				this.RestoreDefaultParameterValues(base.ParametersBoundThroughPipelineInput);
				throw;
			}
			try
			{
				this.VerifyParameterSetSelected();
			}
			catch (ParameterBindingException)
			{
				this.RestoreDefaultParameterValues(base.ParametersBoundThroughPipelineInput);
				throw;
			}
			if (!flag5)
			{
				this.RestoreDefaultParameterValues(base.ParametersBoundThroughPipelineInput);
			}
			return flag5;
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x000132DC File Offset: 0x000114DC
		private bool BindPipelineParametersPrivate(PSObject inputToOperateOn)
		{
			ConsolidatedString internalTypeNames;
			ParameterBinderBase.bindingTracer.WriteLine("PIPELINE object TYPE = [{0}]", new object[]
			{
				(inputToOperateOn == null || inputToOperateOn == AutomationNull.Value) ? "null" : (((internalTypeNames = inputToOperateOn.InternalTypeNames).Count > 0 && internalTypeNames[0] != null) ? internalTypeNames[0] : inputToOperateOn.BaseObject.GetType().FullName)
			});
			bool result = false;
			ParameterBinderBase.bindingTracer.WriteLine("RESTORING pipeline parameter's original values", new object[0]);
			this.RestoreDefaultParameterValues(base.ParametersBoundThroughPipelineInput);
			base.ParametersBoundThroughPipelineInput.Clear();
			this._currentParameterSetFlag = this._prePipelineProcessingParameterSetFlags;
			uint validParameterSets = this._currentParameterSetFlag;
			bool flag = this._parameterSetToBePrioritizedInPipelingBinding != 0U;
			int num = flag ? 2 : 1;
			if (flag)
			{
				validParameterSets = this._parameterSetToBePrioritizedInPipelingBinding;
			}
			for (int i = 0; i < num; i++)
			{
				for (CmdletParameterBinderController.CurrentlyBinding currentlyBinding = CmdletParameterBinderController.CurrentlyBinding.ValueFromPipelineNoCoercion; currentlyBinding <= CmdletParameterBinderController.CurrentlyBinding.ValueFromPipelineByPropertyNameWithCoercion; currentlyBinding++)
				{
					bool flag2 = this.BindUnboundParametersForBindingState(inputToOperateOn, currentlyBinding, validParameterSets);
					if (flag2)
					{
						if (!flag || i == 1)
						{
							this.ValidateParameterSets(true, true);
							validParameterSets = this._currentParameterSetFlag;
						}
						result = true;
					}
				}
				if (flag && i == 0)
				{
					if (this._currentParameterSetFlag == this._parameterSetToBePrioritizedInPipelingBinding)
					{
						break;
					}
					validParameterSets = (this._currentParameterSetFlag & ~this._parameterSetToBePrioritizedInPipelingBinding);
				}
			}
			this.ValidateParameterSets(false, true);
			if (!base.DefaultParameterBindingInUse)
			{
				this.ApplyDefaultParameterBinding("PIPELINE BIND", false);
			}
			return result;
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00013438 File Offset: 0x00011638
		private bool BindUnboundParametersForBindingState(PSObject inputToOperateOn, CmdletParameterBinderController.CurrentlyBinding currentlyBinding, uint validParameterSets)
		{
			bool flag = false;
			uint defaultParameterSetFlag = this._commandMetadata.DefaultParameterSetFlag;
			if (defaultParameterSetFlag != 0U && (validParameterSets & defaultParameterSetFlag) != 0U)
			{
				flag = this.BindUnboundParametersForBindingStateInParameterSet(inputToOperateOn, currentlyBinding, defaultParameterSetFlag);
				if (!flag)
				{
					validParameterSets &= ~defaultParameterSetFlag;
				}
			}
			if (!flag)
			{
				flag = this.BindUnboundParametersForBindingStateInParameterSet(inputToOperateOn, currentlyBinding, validParameterSets);
			}
			CmdletParameterBinderController._tracer.WriteLine("aParameterWasBound = {0}", new object[]
			{
				flag
			});
			return flag;
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0001349C File Offset: 0x0001169C
		private bool BindUnboundParametersForBindingStateInParameterSet(PSObject inputToOperateOn, CmdletParameterBinderController.CurrentlyBinding currentlyBinding, uint validParameterSets)
		{
			bool result = false;
			for (int i = base.UnboundParameters.Count - 1; i >= 0; i--)
			{
				MergedCompiledCommandParameter mergedCompiledCommandParameter = base.UnboundParameters[i];
				if (mergedCompiledCommandParameter.Parameter.IsPipelineParameterInSomeParameterSet && ((validParameterSets & mergedCompiledCommandParameter.Parameter.ParameterSetFlags) != 0U || mergedCompiledCommandParameter.Parameter.IsInAllSets))
				{
					IEnumerable<ParameterSetSpecificMetadata> matchingParameterSetData = mergedCompiledCommandParameter.Parameter.GetMatchingParameterSetData(validParameterSets);
					bool flag = false;
					foreach (ParameterSetSpecificMetadata parameterSetSpecificMetadata in matchingParameterSetData)
					{
						if (currentlyBinding == CmdletParameterBinderController.CurrentlyBinding.ValueFromPipelineNoCoercion && parameterSetSpecificMetadata.ValueFromPipeline)
						{
							flag = this.BindValueFromPipeline(inputToOperateOn, mergedCompiledCommandParameter, ParameterBindingFlags.None);
						}
						else if (currentlyBinding == CmdletParameterBinderController.CurrentlyBinding.ValueFromPipelineByPropertyNameNoCoercion && parameterSetSpecificMetadata.ValueFromPipelineByPropertyName && inputToOperateOn != null)
						{
							flag = this.BindValueFromPipelineByPropertyName(inputToOperateOn, mergedCompiledCommandParameter, ParameterBindingFlags.None);
						}
						else if (currentlyBinding == CmdletParameterBinderController.CurrentlyBinding.ValueFromPipelineWithCoercion && parameterSetSpecificMetadata.ValueFromPipeline)
						{
							flag = this.BindValueFromPipeline(inputToOperateOn, mergedCompiledCommandParameter, ParameterBindingFlags.ShouldCoerceType);
						}
						else if (currentlyBinding == CmdletParameterBinderController.CurrentlyBinding.ValueFromPipelineByPropertyNameWithCoercion && parameterSetSpecificMetadata.ValueFromPipelineByPropertyName && inputToOperateOn != null)
						{
							flag = this.BindValueFromPipelineByPropertyName(inputToOperateOn, mergedCompiledCommandParameter, ParameterBindingFlags.ShouldCoerceType);
						}
						if (flag)
						{
							result = true;
							break;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x000135C8 File Offset: 0x000117C8
		private bool BindValueFromPipeline(PSObject inputToOperateOn, MergedCompiledCommandParameter parameter, ParameterBindingFlags flags)
		{
			bool result = false;
			ParameterBinderBase.bindingTracer.WriteLine(((flags & ParameterBindingFlags.ShouldCoerceType) != ParameterBindingFlags.None) ? "Parameter [{0}] PIPELINE INPUT ValueFromPipeline WITH COERCION" : "Parameter [{0}] PIPELINE INPUT ValueFromPipeline NO COERCION", new object[]
			{
				parameter.Parameter.Name
			});
			ParameterBindingException ex = null;
			try
			{
				result = this.BindPipelineParameter(inputToOperateOn, parameter, flags);
			}
			catch (ParameterBindingArgumentTransformationException ex2)
			{
				PSInvalidCastException ex3;
				if (ex2.InnerException is ArgumentTransformationMetadataException)
				{
					ex3 = (ex2.InnerException.InnerException as PSInvalidCastException);
				}
				else
				{
					ex3 = (ex2.InnerException as PSInvalidCastException);
				}
				if (ex3 == null)
				{
					ex = ex2;
				}
				result = false;
			}
			catch (ParameterBindingValidationException ex4)
			{
				ex = ex4;
			}
			catch (ParameterBindingParameterDefaultValueException ex5)
			{
				ex = ex5;
			}
			catch (ParameterBindingException)
			{
				result = false;
			}
			if (ex != null)
			{
				if (!base.DefaultParameterBindingInUse)
				{
					throw ex;
				}
				base.ThrowElaboratedBindingException(ex);
			}
			return result;
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x000136AC File Offset: 0x000118AC
		private bool BindValueFromPipelineByPropertyName(PSObject inputToOperateOn, MergedCompiledCommandParameter parameter, ParameterBindingFlags flags)
		{
			bool result = false;
			ParameterBinderBase.bindingTracer.WriteLine(((flags & ParameterBindingFlags.ShouldCoerceType) != ParameterBindingFlags.None) ? "Parameter [{0}] PIPELINE INPUT ValueFromPipelineByPropertyName WITH COERCION" : "Parameter [{0}] PIPELINE INPUT ValueFromPipelineByPropertyName NO COERCION", new object[]
			{
				parameter.Parameter.Name
			});
			PSMemberInfo psmemberInfo = inputToOperateOn.Properties[parameter.Parameter.Name];
			if (psmemberInfo == null)
			{
				foreach (string name in parameter.Parameter.Aliases)
				{
					psmemberInfo = inputToOperateOn.Properties[name];
					if (psmemberInfo != null)
					{
						break;
					}
				}
			}
			if (psmemberInfo != null)
			{
				ParameterBindingException ex = null;
				try
				{
					result = this.BindPipelineParameter(psmemberInfo.Value, parameter, flags);
				}
				catch (ParameterBindingArgumentTransformationException ex2)
				{
					ex = ex2;
				}
				catch (ParameterBindingValidationException ex3)
				{
					ex = ex3;
				}
				catch (ParameterBindingParameterDefaultValueException ex4)
				{
					ex = ex4;
				}
				catch (ParameterBindingException)
				{
					result = false;
				}
				if (ex != null)
				{
					if (!base.DefaultParameterBindingInUse)
					{
						throw ex;
					}
					base.ThrowElaboratedBindingException(ex);
				}
			}
			return result;
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x000137D4 File Offset: 0x000119D4
		private bool InvokeAndBindDelayBindScriptBlock(PSObject inputToOperateOn, out bool thereWasSomethingToBind)
		{
			thereWasSomethingToBind = false;
			bool result = true;
			foreach (KeyValuePair<MergedCompiledCommandParameter, CmdletParameterBinderController.DelayedScriptBlockArgument> keyValuePair in this._delayBindScriptBlocks)
			{
				thereWasSomethingToBind = true;
				CommandParameterInternal argument = keyValuePair.Value._argument;
				MergedCompiledCommandParameter key = keyValuePair.Key;
				ScriptBlock scriptBlock = argument.ArgumentValue as ScriptBlock;
				Collection<PSObject> collection = null;
				Exception ex = null;
				using (ParameterBinderBase.bindingTracer.TraceScope("Invoking delay-bind ScriptBlock", new object[0]))
				{
					if (keyValuePair.Value._parameterBinder == this)
					{
						try
						{
							collection = scriptBlock.DoInvoke(inputToOperateOn, inputToOperateOn, new object[0]);
							keyValuePair.Value._evaluatedArgument = collection;
							goto IL_A7;
						}
						catch (RuntimeException ex2)
						{
							ex = ex2;
							goto IL_A7;
						}
					}
					collection = keyValuePair.Value._evaluatedArgument;
					IL_A7:;
				}
				if (ex != null)
				{
					ParameterBindingException ex3 = new ParameterBindingException(ex, ErrorCategory.InvalidArgument, this.Command.MyInvocation, base.GetErrorExtent(argument), key.Parameter.Name, null, null, ParameterBinderStrings.ScriptBlockArgumentInvocationFailed, "ScriptBlockArgumentInvocationFailed", new object[]
					{
						ex.Message
					});
					throw ex3;
				}
				if (collection == null || collection.Count == 0)
				{
					ParameterBindingException ex4 = new ParameterBindingException(ex, ErrorCategory.InvalidArgument, this.Command.MyInvocation, base.GetErrorExtent(argument), key.Parameter.Name, null, null, ParameterBinderStrings.ScriptBlockArgumentNoOutput, "ScriptBlockArgumentNoOutput", new object[0]);
					throw ex4;
				}
				object value = collection;
				if (collection.Count == 1)
				{
					value = collection[0];
				}
				CommandParameterInternal argument2 = CommandParameterInternal.CreateParameterWithArgument(argument.ParameterExtent, argument.ParameterName, "-" + argument.ParameterName + ":", argument.ArgumentExtent, value, false, false);
				if (!this.BindParameter(argument2, key, ParameterBindingFlags.ShouldCoerceType))
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x000139F0 File Offset: 0x00011BF0
		private static int ValidParameterSetCount(uint parameterSetFlags)
		{
			int num = 0;
			if (parameterSetFlags == 4294967295U)
			{
				num = 1;
			}
			else
			{
				while (parameterSetFlags != 0U)
				{
					num += (int)(parameterSetFlags & 1U);
					parameterSetFlags >>= 1;
				}
			}
			return num;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00013A18 File Offset: 0x00011C18
		internal object GetDefaultParameterValue(string name)
		{
			MergedCompiledCommandParameter matchingParameter = base.BindableParameters.GetMatchingParameter(name, false, true, null);
			object result = null;
			try
			{
				switch (matchingParameter.BinderAssociation)
				{
				case ParameterBinderAssociation.DeclaredFormalParameters:
					result = base.DefaultParameterBinder.GetDefaultParameterValue(name);
					break;
				case ParameterBinderAssociation.DynamicParameters:
					if (this._dynamicParameterBinder != null)
					{
						result = this._dynamicParameterBinder.GetDefaultParameterValue(name);
					}
					break;
				case ParameterBinderAssociation.CommonParameters:
					result = this.CommonParametersBinder.GetDefaultParameterValue(name);
					break;
				case ParameterBinderAssociation.ShouldProcessParameters:
					result = this.ShouldProcessParametersBinder.GetDefaultParameterValue(name);
					break;
				}
			}
			catch (GetValueException ex)
			{
				ParameterBindingParameterDefaultValueException ex2 = new ParameterBindingParameterDefaultValueException(ex, ErrorCategory.ReadError, this.Command.MyInvocation, null, name, null, null, "ParameterBinderStrings", "GetDefaultValueFailed", new object[]
				{
					ex.Message
				});
				throw ex2;
			}
			return result;
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x00013AE8 File Offset: 0x00011CE8
		// (set) Token: 0x06000471 RID: 1137 RVA: 0x00013AF0 File Offset: 0x00011CF0
		internal Cmdlet Command { get; private set; }

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000472 RID: 1138 RVA: 0x00013AF9 File Offset: 0x00011CF9
		// (set) Token: 0x06000473 RID: 1139 RVA: 0x00013B01 File Offset: 0x00011D01
		internal List<WarningRecord> ObsoleteParameterWarningList { get; private set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000474 RID: 1140 RVA: 0x00013B0C File Offset: 0x00011D0C
		private HashSet<string> BoundObsoleteParameterNames
		{
			get
			{
				HashSet<string> result;
				if ((result = this._boundObsoleteParameterNames) == null)
				{
					result = (this._boundObsoleteParameterNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase));
				}
				return result;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x00013B38 File Offset: 0x00011D38
		internal ReflectionParameterBinder ShouldProcessParametersBinder
		{
			get
			{
				if (this._shouldProcessParameterBinder == null)
				{
					ShouldProcessParameters target = new ShouldProcessParameters(this._commandRuntime);
					this._shouldProcessParameterBinder = new ReflectionParameterBinder(target, this.Command, base.CommandLineParameters);
				}
				return this._shouldProcessParameterBinder;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000476 RID: 1142 RVA: 0x00013B78 File Offset: 0x00011D78
		internal ReflectionParameterBinder PagingParametersBinder
		{
			get
			{
				if (this._pagingParameterBinder == null)
				{
					PagingParameters target = new PagingParameters(this._commandRuntime);
					this._pagingParameterBinder = new ReflectionParameterBinder(target, this.Command, base.CommandLineParameters);
				}
				return this._pagingParameterBinder;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x00013BB8 File Offset: 0x00011DB8
		internal ReflectionParameterBinder TransactionParametersBinder
		{
			get
			{
				if (this._transactionParameterBinder == null)
				{
					TransactionParameters target = new TransactionParameters(this._commandRuntime);
					this._transactionParameterBinder = new ReflectionParameterBinder(target, this.Command, base.CommandLineParameters);
				}
				return this._transactionParameterBinder;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x00013BF8 File Offset: 0x00011DF8
		internal ReflectionParameterBinder CommonParametersBinder
		{
			get
			{
				if (this._commonParametersBinder == null)
				{
					CommonParameters target = new CommonParameters(this._commandRuntime);
					this._commonParametersBinder = new ReflectionParameterBinder(target, this.Command, base.CommandLineParameters);
				}
				return this._commonParametersBinder;
			}
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00013C38 File Offset: 0x00011E38
		private bool BindPipelineParameter(object parameterValue, MergedCompiledCommandParameter parameter, ParameterBindingFlags flags)
		{
			bool flag = false;
			if (parameterValue != AutomationNull.Value)
			{
				CmdletParameterBinderController._tracer.WriteLine("Adding PipelineParameter name={0}; value={1}", new object[]
				{
					parameter.Parameter.Name,
					parameterValue ?? "null"
				});
				this.BackupDefaultParameter(parameter);
				CommandParameterInternal argument = CommandParameterInternal.CreateParameterWithArgument(PositionUtilities.EmptyExtent, parameter.Parameter.Name, "-" + parameter.Parameter.Name + ":", PositionUtilities.EmptyExtent, parameterValue, false, false);
				flags &= ~ParameterBindingFlags.DelayBindScriptBlock;
				flag = this.BindParameter(this._currentParameterSetFlag, argument, parameter, flags);
				if (flag)
				{
					base.ParametersBoundThroughPipelineInput.Add(parameter);
				}
			}
			return flag;
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00013CEC File Offset: 0x00011EEC
		protected override void SaveDefaultScriptParameterValue(string name, object value)
		{
			this._defaultParameterValues.Add(name, CommandParameterInternal.CreateParameterWithArgument(PositionUtilities.EmptyExtent, name, "-" + name + ":", PositionUtilities.EmptyExtent, value, false, false));
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00013D28 File Offset: 0x00011F28
		private void BackupDefaultParameter(MergedCompiledCommandParameter parameter)
		{
			if (!this._defaultParameterValues.ContainsKey(parameter.Parameter.Name))
			{
				object defaultParameterValue = this.GetDefaultParameterValue(parameter.Parameter.Name);
				this._defaultParameterValues.Add(parameter.Parameter.Name, CommandParameterInternal.CreateParameterWithArgument(PositionUtilities.EmptyExtent, parameter.Parameter.Name, "-" + parameter.Parameter.Name + ":", PositionUtilities.EmptyExtent, defaultParameterValue, false, false));
			}
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00013DAC File Offset: 0x00011FAC
		private void RestoreDefaultParameterValues(IEnumerable<MergedCompiledCommandParameter> parameters)
		{
			if (parameters == null)
			{
				throw PSTraceSource.NewArgumentNullException("parameters");
			}
			foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter in parameters)
			{
				if (mergedCompiledCommandParameter != null)
				{
					CommandParameterInternal commandParameterInternal = null;
					if (this._defaultParameterValues.TryGetValue(mergedCompiledCommandParameter.Parameter.Name, out commandParameterInternal))
					{
						Exception ex = null;
						try
						{
							this.RestoreParameter(commandParameterInternal, mergedCompiledCommandParameter);
						}
						catch (SetValueException ex2)
						{
							ex = ex2;
						}
						if (ex != null)
						{
							Type typeSpecified = (commandParameterInternal.ArgumentValue == null) ? null : commandParameterInternal.ArgumentValue.GetType();
							ParameterBindingException ex3 = new ParameterBindingException(ex, ErrorCategory.WriteError, base.InvocationInfo, base.GetErrorExtent(commandParameterInternal), mergedCompiledCommandParameter.Parameter.Name, mergedCompiledCommandParameter.Parameter.Type, typeSpecified, ParameterBinderStrings.ParameterBindingFailed, "ParameterBindingFailed", new object[]
							{
								ex.Message
							});
							throw ex3;
						}
						if (base.BoundParameters.ContainsKey(mergedCompiledCommandParameter.Parameter.Name))
						{
							base.BoundParameters.Remove(mergedCompiledCommandParameter.Parameter.Name);
						}
						if (!base.UnboundParameters.Contains(mergedCompiledCommandParameter))
						{
							base.UnboundParameters.Add(mergedCompiledCommandParameter);
						}
						if (base.BoundArguments.ContainsKey(mergedCompiledCommandParameter.Parameter.Name))
						{
							base.BoundArguments.Remove(mergedCompiledCommandParameter.Parameter.Name);
						}
					}
					else
					{
						if (!base.BoundParameters.ContainsKey(mergedCompiledCommandParameter.Parameter.Name))
						{
							base.BoundParameters.Add(mergedCompiledCommandParameter.Parameter.Name, mergedCompiledCommandParameter);
						}
						base.UnboundParameters.Remove(mergedCompiledCommandParameter);
					}
				}
			}
		}

		// Token: 0x0400018C RID: 396
		private const string Separator = ":::";

		// Token: 0x0400018D RID: 397
		[TraceSource("ParameterBinderController", "Controls the interaction between the command processor and the parameter binder(s).")]
		private static readonly PSTraceSource _tracer = PSTraceSource.GetTracer("ParameterBinderController", "Controls the interaction between the command processor and the parameter binder(s).");

		// Token: 0x0400018E RID: 398
		private List<string> _aliasList;

		// Token: 0x0400018F RID: 399
		private readonly HashSet<string> _warningSet = new HashSet<string>();

		// Token: 0x04000190 RID: 400
		private Dictionary<MergedCompiledCommandParameter, object> _allDefaultParameterValuePairs;

		// Token: 0x04000191 RID: 401
		private bool _useDefaultParameterBinding = true;

		// Token: 0x04000192 RID: 402
		private uint _parameterSetToBePrioritizedInPipelingBinding;

		// Token: 0x04000193 RID: 403
		private readonly CommandMetadata _commandMetadata;

		// Token: 0x04000194 RID: 404
		private readonly MshCommandRuntime _commandRuntime;

		// Token: 0x04000195 RID: 405
		private HashSet<string> _boundObsoleteParameterNames;

		// Token: 0x04000196 RID: 406
		private ParameterBinderBase _dynamicParameterBinder;

		// Token: 0x04000197 RID: 407
		private ReflectionParameterBinder _shouldProcessParameterBinder;

		// Token: 0x04000198 RID: 408
		private ReflectionParameterBinder _pagingParameterBinder;

		// Token: 0x04000199 RID: 409
		private ReflectionParameterBinder _transactionParameterBinder;

		// Token: 0x0400019A RID: 410
		private ReflectionParameterBinder _commonParametersBinder;

		// Token: 0x0400019B RID: 411
		private readonly Dictionary<MergedCompiledCommandParameter, CmdletParameterBinderController.DelayedScriptBlockArgument> _delayBindScriptBlocks = new Dictionary<MergedCompiledCommandParameter, CmdletParameterBinderController.DelayedScriptBlockArgument>();

		// Token: 0x0400019C RID: 412
		private readonly Dictionary<string, CommandParameterInternal> _defaultParameterValues = new Dictionary<string, CommandParameterInternal>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x02000050 RID: 80
		private enum CurrentlyBinding
		{
			// Token: 0x040001A4 RID: 420
			ValueFromPipelineNoCoercion,
			// Token: 0x040001A5 RID: 421
			ValueFromPipelineByPropertyNameNoCoercion,
			// Token: 0x040001A6 RID: 422
			ValueFromPipelineWithCoercion,
			// Token: 0x040001A7 RID: 423
			ValueFromPipelineByPropertyNameWithCoercion
		}

		// Token: 0x02000051 RID: 81
		private class DelayedScriptBlockArgument
		{
			// Token: 0x06000481 RID: 1153 RVA: 0x00013F9A File Offset: 0x0001219A
			public override string ToString()
			{
				return this._argument.ArgumentValue.ToString();
			}

			// Token: 0x040001A8 RID: 424
			internal CmdletParameterBinderController _parameterBinder;

			// Token: 0x040001A9 RID: 425
			internal CommandParameterInternal _argument;

			// Token: 0x040001AA RID: 426
			internal Collection<PSObject> _evaluatedArgument;
		}
	}
}
