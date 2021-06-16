using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Management.Automation.Language;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x0200004E RID: 78
	[DebuggerDisplay("InvocationInfo = {InvocationInfo}")]
	internal abstract class ParameterBinderController
	{
		// Token: 0x06000419 RID: 1049 RVA: 0x0000EBEC File Offset: 0x0000CDEC
		internal ParameterBinderController(InvocationInfo invocationInfo, ExecutionContext context, ParameterBinderBase parameterBinder)
		{
			this.DefaultParameterBinder = parameterBinder;
			this._context = context;
			this._invocationInfo = invocationInfo;
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x0000EC6E File Offset: 0x0000CE6E
		internal ExecutionContext Context
		{
			get
			{
				return this._context;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x0000EC76 File Offset: 0x0000CE76
		// (set) Token: 0x0600041C RID: 1052 RVA: 0x0000EC7E File Offset: 0x0000CE7E
		internal ParameterBinderBase DefaultParameterBinder { get; private set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x0000EC87 File Offset: 0x0000CE87
		internal InvocationInfo InvocationInfo
		{
			get
			{
				return this._invocationInfo;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x0000EC8F File Offset: 0x0000CE8F
		internal MergedCommandParameterMetadata BindableParameters
		{
			get
			{
				return this._bindableParameters;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x0000EC97 File Offset: 0x0000CE97
		// (set) Token: 0x06000420 RID: 1056 RVA: 0x0000EC9F File Offset: 0x0000CE9F
		protected List<MergedCompiledCommandParameter> UnboundParameters { get; set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x0000ECA8 File Offset: 0x0000CEA8
		protected Dictionary<string, MergedCompiledCommandParameter> BoundParameters
		{
			get
			{
				return this._boundParameters;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x0000ECB0 File Offset: 0x0000CEB0
		internal CommandLineParameters CommandLineParameters
		{
			get
			{
				return this.DefaultParameterBinder.CommandLineParameters;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x0000ECBD File Offset: 0x0000CEBD
		// (set) Token: 0x06000424 RID: 1060 RVA: 0x0000ECC5 File Offset: 0x0000CEC5
		protected bool DefaultParameterBindingInUse
		{
			get
			{
				return this._defaultParameterBindingInUse;
			}
			set
			{
				this._defaultParameterBindingInUse = value;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x0000ECCE File Offset: 0x0000CECE
		protected Collection<string> BoundDefaultParameters
		{
			get
			{
				return this._boundDefaultParameters;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x0000ECD6 File Offset: 0x0000CED6
		// (set) Token: 0x06000427 RID: 1063 RVA: 0x0000ECDE File Offset: 0x0000CEDE
		protected Collection<CommandParameterInternal> UnboundArguments
		{
			get
			{
				return this._unboundArguments;
			}
			set
			{
				this._unboundArguments = value;
			}
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0000ECE7 File Offset: 0x0000CEE7
		internal void ClearUnboundArguments()
		{
			this._unboundArguments.Clear();
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x0000ECF4 File Offset: 0x0000CEF4
		protected Dictionary<string, CommandParameterInternal> BoundArguments
		{
			get
			{
				return this._boundArguments;
			}
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000ECFC File Offset: 0x0000CEFC
		internal void ReparseUnboundArguments()
		{
			Collection<CommandParameterInternal> collection = new Collection<CommandParameterInternal>();
			for (int i = 0; i < this._unboundArguments.Count; i++)
			{
				CommandParameterInternal commandParameterInternal = this._unboundArguments[i];
				if (!commandParameterInternal.ParameterNameSpecified || commandParameterInternal.ArgumentSpecified)
				{
					collection.Add(commandParameterInternal);
				}
				else
				{
					string parameterName = commandParameterInternal.ParameterName;
					MergedCompiledCommandParameter matchingParameter = this._bindableParameters.GetMatchingParameter(parameterName, false, true, new InvocationInfo(this.InvocationInfo.MyCommand, commandParameterInternal.ParameterExtent));
					if (matchingParameter == null)
					{
						collection.Add(commandParameterInternal);
					}
					else if (ParameterBinderController.IsSwitchAndSetValue(parameterName, commandParameterInternal, matchingParameter.Parameter))
					{
						collection.Add(commandParameterInternal);
					}
					else
					{
						if (this._unboundArguments.Count - 1 <= i)
						{
							ParameterBindingException ex = new ParameterBindingException(ErrorCategory.InvalidArgument, this.InvocationInfo, this.GetParameterErrorExtent(commandParameterInternal), matchingParameter.Parameter.Name, matchingParameter.Parameter.Type, null, ParameterBinderStrings.MissingArgument, "MissingArgument", new object[0]);
							throw ex;
						}
						CommandParameterInternal commandParameterInternal2 = this._unboundArguments[i + 1];
						if (commandParameterInternal2.ParameterNameSpecified)
						{
							if (this._bindableParameters.GetMatchingParameter(commandParameterInternal2.ParameterName, false, true, new InvocationInfo(this.InvocationInfo.MyCommand, commandParameterInternal2.ParameterExtent)) != null || commandParameterInternal2.ParameterAndArgumentSpecified)
							{
								ParameterBindingException ex2 = new ParameterBindingException(ErrorCategory.InvalidArgument, this.InvocationInfo, this.GetParameterErrorExtent(commandParameterInternal), matchingParameter.Parameter.Name, matchingParameter.Parameter.Type, null, ParameterBinderStrings.MissingArgument, "MissingArgument", new object[0]);
								throw ex2;
							}
							i++;
							commandParameterInternal.ParameterName = matchingParameter.Parameter.Name;
							commandParameterInternal.SetArgumentValue(commandParameterInternal2.ArgumentExtent, commandParameterInternal2.ParameterText);
							collection.Add(commandParameterInternal);
						}
						else
						{
							i++;
							commandParameterInternal.ParameterName = matchingParameter.Parameter.Name;
							commandParameterInternal.SetArgumentValue(commandParameterInternal2.ArgumentExtent, commandParameterInternal2.ArgumentValue);
							collection.Add(commandParameterInternal);
						}
					}
				}
			}
			this._unboundArguments = collection;
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000EF08 File Offset: 0x0000D108
		private static bool IsSwitchAndSetValue(string argumentName, CommandParameterInternal argument, CompiledCommandParameter matchingParameter)
		{
			bool result = false;
			if (matchingParameter.Type == typeof(SwitchParameter))
			{
				argument.ParameterName = argumentName;
				argument.SetArgumentValue(PositionUtilities.EmptyExtent, SwitchParameter.Present);
				result = true;
			}
			return result;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0000EF50 File Offset: 0x0000D150
		internal static bool ArgumentLooksLikeParameter(string arg)
		{
			bool result = false;
			if (!string.IsNullOrEmpty(arg))
			{
				result = arg[0].IsDash();
			}
			return result;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0000EF78 File Offset: 0x0000D178
		internal static void AddArgumentsToCommandProcessor(CommandProcessorBase commandProcessor, object[] arguments)
		{
			if (arguments != null && arguments.Length > 0)
			{
				PSBoundParametersDictionary psboundParametersDictionary = arguments[0] as PSBoundParametersDictionary;
				if (psboundParametersDictionary != null && arguments.Length == 1)
				{
					using (Dictionary<string, object>.Enumerator enumerator = psboundParametersDictionary.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<string, object> keyValuePair = enumerator.Current;
							CommandParameterInternal parameter = CommandParameterInternal.CreateParameterWithArgument(PositionUtilities.EmptyExtent, keyValuePair.Key, keyValuePair.Key, PositionUtilities.EmptyExtent, keyValuePair.Value, false, false);
							commandProcessor.AddParameter(parameter);
						}
						return;
					}
				}
				for (int i = 0; i < arguments.Length; i++)
				{
					string text = arguments[i] as string;
					CommandParameterInternal parameter2;
					if (ParameterBinderController.ArgumentLooksLikeParameter(text))
					{
						int num = text.IndexOf(':');
						if (num != -1 && num != text.Length - 1)
						{
							parameter2 = CommandParameterInternal.CreateParameterWithArgument(PositionUtilities.EmptyExtent, text.Substring(1, num - 1), text, PositionUtilities.EmptyExtent, text.Substring(num + 1).Trim(), false, false);
						}
						else if (i == arguments.Length - 1 || text[text.Length - 1] != ':')
						{
							parameter2 = CommandParameterInternal.CreateParameter(PositionUtilities.EmptyExtent, text.Substring(1), text);
						}
						else
						{
							parameter2 = CommandParameterInternal.CreateParameterWithArgument(PositionUtilities.EmptyExtent, text.Substring(1, text.Length - 2), text, PositionUtilities.EmptyExtent, arguments[i + 1], false, false);
							i++;
						}
					}
					else
					{
						parameter2 = CommandParameterInternal.CreateArgument(PositionUtilities.EmptyExtent, arguments[i], false, false);
					}
					commandProcessor.AddParameter(parameter2);
				}
			}
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000F10C File Offset: 0x0000D30C
		internal virtual bool BindParameter(CommandParameterInternal argument, ParameterBindingFlags flags)
		{
			bool result = false;
			MergedCompiledCommandParameter matchingParameter = this.BindableParameters.GetMatchingParameter(argument.ParameterName, (flags & ParameterBindingFlags.ThrowOnParameterNotFound) != ParameterBindingFlags.None, true, new InvocationInfo(this.InvocationInfo.MyCommand, argument.ParameterExtent));
			if (matchingParameter != null)
			{
				if (this.BoundParameters.ContainsKey(matchingParameter.Parameter.Name))
				{
					ParameterBindingException ex = new ParameterBindingException(ErrorCategory.InvalidArgument, this.InvocationInfo, this.GetParameterErrorExtent(argument), argument.ParameterName, null, null, ParameterBinderStrings.ParameterAlreadyBound, "ParameterAlreadyBound", new object[0]);
					throw ex;
				}
				flags &= ~ParameterBindingFlags.DelayBindScriptBlock;
				result = this.BindParameter(this._currentParameterSetFlag, argument, matchingParameter, flags);
			}
			return result;
		}

		// Token: 0x0600042F RID: 1071
		internal abstract Collection<CommandParameterInternal> BindParameters(Collection<CommandParameterInternal> parameters);

		// Token: 0x06000430 RID: 1072 RVA: 0x0000F1B0 File Offset: 0x0000D3B0
		internal virtual bool BindParameter(uint parameterSets, CommandParameterInternal argument, MergedCompiledCommandParameter parameter, ParameterBindingFlags flags)
		{
			bool flag = false;
			ParameterBinderAssociation binderAssociation = parameter.BinderAssociation;
			if (binderAssociation == ParameterBinderAssociation.DeclaredFormalParameters)
			{
				flag = this.DefaultParameterBinder.BindParameter(argument, parameter.Parameter, flags);
			}
			if (flag && (flags & ParameterBindingFlags.IsDefaultValue) == ParameterBindingFlags.None)
			{
				this.UnboundParameters.Remove(parameter);
				this.BoundParameters.Add(parameter.Parameter.Name, parameter);
			}
			return flag;
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000F210 File Offset: 0x0000D410
		internal Collection<CommandParameterInternal> BindPositionalParameters(Collection<CommandParameterInternal> unboundArguments, uint validParameterSets, uint defaultParameterSet, out ParameterBindingException outgoingBindingException)
		{
			Collection<CommandParameterInternal> collection = new Collection<CommandParameterInternal>();
			outgoingBindingException = null;
			if (unboundArguments.Count > 0)
			{
				List<CommandParameterInternal> list = new List<CommandParameterInternal>(unboundArguments);
				SortedDictionary<int, Dictionary<MergedCompiledCommandParameter, PositionalCommandParameter>> sortedDictionary;
				try
				{
					sortedDictionary = ParameterBinderController.EvaluateUnboundPositionalParameters(this.UnboundParameters, this._currentParameterSetFlag);
				}
				catch (InvalidOperationException)
				{
					ParameterBindingException ex = new ParameterBindingException(ErrorCategory.InvalidArgument, this.InvocationInfo, null, null, null, null, ParameterBinderStrings.AmbiguousPositionalParameterNoName, "AmbiguousPositionalParameterNoName", new object[0]);
					throw ex;
				}
				if (sortedDictionary.Count > 0)
				{
					int num = 0;
					foreach (Dictionary<MergedCompiledCommandParameter, PositionalCommandParameter> dictionary in sortedDictionary.Values)
					{
						if (dictionary.Count != 0)
						{
							CommandParameterInternal nextPositionalArgument = ParameterBinderController.GetNextPositionalArgument(list, collection, ref num);
							if (nextPositionalArgument == null)
							{
								break;
							}
							bool flag = false;
							if (defaultParameterSet != 0U && (validParameterSets & defaultParameterSet) != 0U)
							{
								flag = this.BindPositionalParametersInSet(defaultParameterSet, dictionary, nextPositionalArgument, ParameterBindingFlags.DelayBindScriptBlock, out outgoingBindingException);
							}
							if (!flag)
							{
								flag = this.BindPositionalParametersInSet(validParameterSets, dictionary, nextPositionalArgument, ParameterBindingFlags.DelayBindScriptBlock, out outgoingBindingException);
							}
							if (!flag && defaultParameterSet != 0U && (validParameterSets & defaultParameterSet) != 0U)
							{
								flag = this.BindPositionalParametersInSet(defaultParameterSet, dictionary, nextPositionalArgument, ParameterBindingFlags.ShouldCoerceType | ParameterBindingFlags.DelayBindScriptBlock, out outgoingBindingException);
							}
							if (!flag)
							{
								flag = this.BindPositionalParametersInSet(validParameterSets, dictionary, nextPositionalArgument, ParameterBindingFlags.ShouldCoerceType | ParameterBindingFlags.DelayBindScriptBlock, out outgoingBindingException);
							}
							if (!flag)
							{
								collection.Add(nextPositionalArgument);
							}
							else if (validParameterSets != this._currentParameterSetFlag)
							{
								validParameterSets = this._currentParameterSetFlag;
								ParameterBinderController.UpdatePositionalDictionary(sortedDictionary, validParameterSets);
							}
						}
					}
					for (int i = num; i < list.Count; i++)
					{
						collection.Add(list[i]);
					}
				}
				else
				{
					collection = unboundArguments;
				}
			}
			return collection;
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000F3A0 File Offset: 0x0000D5A0
		internal static void UpdatePositionalDictionary(SortedDictionary<int, Dictionary<MergedCompiledCommandParameter, PositionalCommandParameter>> positionalParameterDictionary, uint validParameterSets)
		{
			foreach (Dictionary<MergedCompiledCommandParameter, PositionalCommandParameter> dictionary in positionalParameterDictionary.Values)
			{
				Collection<MergedCompiledCommandParameter> collection = new Collection<MergedCompiledCommandParameter>();
				foreach (PositionalCommandParameter positionalCommandParameter in dictionary.Values)
				{
					Collection<ParameterSetSpecificMetadata> parameterSetData = positionalCommandParameter.ParameterSetData;
					for (int i = parameterSetData.Count - 1; i >= 0; i--)
					{
						if ((parameterSetData[i].ParameterSetFlag & validParameterSets) == 0U && !parameterSetData[i].IsInAllSets)
						{
							parameterSetData.RemoveAt(i);
						}
					}
					if (parameterSetData.Count == 0)
					{
						collection.Add(positionalCommandParameter.Parameter);
					}
				}
				foreach (MergedCompiledCommandParameter key in collection)
				{
					dictionary.Remove(key);
				}
			}
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000F4D0 File Offset: 0x0000D6D0
		private bool BindPositionalParametersInSet(uint validParameterSets, Dictionary<MergedCompiledCommandParameter, PositionalCommandParameter> nextPositionalParameters, CommandParameterInternal argument, ParameterBindingFlags flags, out ParameterBindingException bindingException)
		{
			bool result = false;
			bindingException = null;
			foreach (PositionalCommandParameter positionalCommandParameter in nextPositionalParameters.Values)
			{
				foreach (ParameterSetSpecificMetadata parameterSetSpecificMetadata in positionalCommandParameter.ParameterSetData)
				{
					if ((validParameterSets & parameterSetSpecificMetadata.ParameterSetFlag) != 0U || parameterSetSpecificMetadata.IsInAllSets)
					{
						bool flag = false;
						string name = positionalCommandParameter.Parameter.Parameter.Name;
						ParameterBindingException ex = null;
						try
						{
							CommandParameterInternal argument2 = CommandParameterInternal.CreateParameterWithArgument(PositionUtilities.EmptyExtent, name, "-" + name + ":", argument.ArgumentExtent, argument.ArgumentValue, false, false);
							flag = this.BindParameter(validParameterSets, argument2, positionalCommandParameter.Parameter, flags);
						}
						catch (ParameterBindingArgumentTransformationException ex2)
						{
							ex = ex2;
						}
						catch (ParameterBindingValidationException ex3)
						{
							if (ex3.SwallowException)
							{
								flag = false;
								bindingException = ex3;
							}
							else
							{
								ex = ex3;
							}
						}
						catch (ParameterBindingParameterDefaultValueException ex4)
						{
							ex = ex4;
						}
						catch (ParameterBindingException ex5)
						{
							flag = false;
							bindingException = ex5;
						}
						if (ex != null)
						{
							if (!this.DefaultParameterBindingInUse)
							{
								throw ex;
							}
							this.ThrowElaboratedBindingException(ex);
						}
						if (flag)
						{
							result = true;
							this.CommandLineParameters.MarkAsBoundPositionally(name);
							break;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000F6B0 File Offset: 0x0000D8B0
		protected void ThrowElaboratedBindingException(ParameterBindingException pbex)
		{
			if (pbex == null)
			{
				throw PSTraceSource.NewArgumentNullException("pbex");
			}
			string message = pbex.Message;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text in this.BoundDefaultParameters)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " -{0}", new object[]
				{
					text
				});
			}
			string resourceString = ParameterBinderStrings.DefaultBindingErrorElaborationSingle;
			if (this.BoundDefaultParameters.Count > 1)
			{
				resourceString = ParameterBinderStrings.DefaultBindingErrorElaborationMultiple;
			}
			ParameterBindingException ex = new ParameterBindingException(pbex.InnerException, pbex, resourceString, new object[]
			{
				message,
				stringBuilder
			});
			throw ex;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000F778 File Offset: 0x0000D978
		private static CommandParameterInternal GetNextPositionalArgument(List<CommandParameterInternal> unboundArgumentsCollection, Collection<CommandParameterInternal> nonPositionalArguments, ref int unboundArgumentsIndex)
		{
			CommandParameterInternal result = null;
			while (unboundArgumentsIndex < unboundArgumentsCollection.Count)
			{
				CommandParameterInternal commandParameterInternal = unboundArgumentsCollection[unboundArgumentsIndex++];
				if (!commandParameterInternal.ParameterNameSpecified)
				{
					result = commandParameterInternal;
					break;
				}
				nonPositionalArguments.Add(commandParameterInternal);
				if (unboundArgumentsCollection.Count - 1 >= unboundArgumentsIndex)
				{
					commandParameterInternal = unboundArgumentsCollection[unboundArgumentsIndex];
					if (!commandParameterInternal.ParameterNameSpecified)
					{
						nonPositionalArguments.Add(commandParameterInternal);
						unboundArgumentsIndex++;
					}
				}
			}
			return result;
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0000F7E4 File Offset: 0x0000D9E4
		internal static SortedDictionary<int, Dictionary<MergedCompiledCommandParameter, PositionalCommandParameter>> EvaluateUnboundPositionalParameters(ICollection<MergedCompiledCommandParameter> unboundParameters, uint validParameterSetFlag)
		{
			SortedDictionary<int, Dictionary<MergedCompiledCommandParameter, PositionalCommandParameter>> result = new SortedDictionary<int, Dictionary<MergedCompiledCommandParameter, PositionalCommandParameter>>();
			if (unboundParameters.Count > 0)
			{
				foreach (MergedCompiledCommandParameter mergedCompiledCommandParameter in unboundParameters)
				{
					bool flag = (mergedCompiledCommandParameter.Parameter.ParameterSetFlags & validParameterSetFlag) != 0U || mergedCompiledCommandParameter.Parameter.IsInAllSets;
					if (flag)
					{
						IEnumerable<ParameterSetSpecificMetadata> matchingParameterSetData = mergedCompiledCommandParameter.Parameter.GetMatchingParameterSetData(validParameterSetFlag);
						foreach (ParameterSetSpecificMetadata parameterSetSpecificMetadata in matchingParameterSetData)
						{
							if (!parameterSetSpecificMetadata.ValueFromRemainingArguments)
							{
								int position = parameterSetSpecificMetadata.Position;
								if (position != -2147483648)
								{
									ParameterBinderController.AddNewPosition(result, position, mergedCompiledCommandParameter, parameterSetSpecificMetadata);
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000F8D0 File Offset: 0x0000DAD0
		private static void AddNewPosition(SortedDictionary<int, Dictionary<MergedCompiledCommandParameter, PositionalCommandParameter>> result, int positionInParameterSet, MergedCompiledCommandParameter parameter, ParameterSetSpecificMetadata parameterSetData)
		{
			if (!result.ContainsKey(positionInParameterSet))
			{
				result.Add(positionInParameterSet, new Dictionary<MergedCompiledCommandParameter, PositionalCommandParameter>
				{
					{
						parameter,
						new PositionalCommandParameter(parameter)
						{
							ParameterSetData = 
							{
								parameterSetData
							}
						}
					}
				});
				return;
			}
			Dictionary<MergedCompiledCommandParameter, PositionalCommandParameter> dictionary = result[positionInParameterSet];
			if (ParameterBinderController.ContainsPositionalParameterInSet(dictionary, parameter, parameterSetData.ParameterSetFlag))
			{
				throw PSTraceSource.NewInvalidOperationException();
			}
			if (dictionary.ContainsKey(parameter))
			{
				dictionary[parameter].ParameterSetData.Add(parameterSetData);
				return;
			}
			dictionary.Add(parameter, new PositionalCommandParameter(parameter)
			{
				ParameterSetData = 
				{
					parameterSetData
				}
			});
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0000F964 File Offset: 0x0000DB64
		private static bool ContainsPositionalParameterInSet(Dictionary<MergedCompiledCommandParameter, PositionalCommandParameter> positionalCommandParameters, MergedCompiledCommandParameter parameter, uint parameterSet)
		{
			bool flag = false;
			foreach (KeyValuePair<MergedCompiledCommandParameter, PositionalCommandParameter> keyValuePair in positionalCommandParameters)
			{
				if (keyValuePair.Key != parameter)
				{
					foreach (ParameterSetSpecificMetadata parameterSetSpecificMetadata in keyValuePair.Value.ParameterSetData)
					{
						if ((parameterSetSpecificMetadata.ParameterSetFlag & parameterSet) != 0U || parameterSetSpecificMetadata.ParameterSetFlag == parameterSet)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x0000FA14 File Offset: 0x0000DC14
		internal Collection<MergedCompiledCommandParameter> ParametersBoundThroughPipelineInput
		{
			get
			{
				return this._parametersBoundThroughPipelineInput;
			}
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0000FA1C File Offset: 0x0000DC1C
		internal void BindUnboundScriptParameters()
		{
			foreach (MergedCompiledCommandParameter parameter in this.UnboundParameters)
			{
				this.BindUnboundScriptParameterWithDefaultValue(parameter);
			}
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0000FA70 File Offset: 0x0000DC70
		protected virtual void SaveDefaultScriptParameterValue(string name, object value)
		{
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0000FA74 File Offset: 0x0000DC74
		internal void BindUnboundScriptParameterWithDefaultValue(MergedCompiledCommandParameter parameter)
		{
			ScriptParameterBinder scriptParameterBinder = (ScriptParameterBinder)this.DefaultParameterBinder;
			ScriptBlock script = scriptParameterBinder.Script;
			if (script.RuntimeDefinedParameters.ContainsKey(parameter.Parameter.Name))
			{
				bool recordBoundParameters = scriptParameterBinder.RecordBoundParameters;
				try
				{
					scriptParameterBinder.RecordBoundParameters = false;
					RuntimeDefinedParameter runtimeDefinedParameter = script.RuntimeDefinedParameters[parameter.Parameter.Name];
					IDictionary implicitUsingParameters = null;
					if (this.DefaultParameterBinder.CommandLineParameters != null)
					{
						implicitUsingParameters = this.DefaultParameterBinder.CommandLineParameters.GetImplicitUsingParameters();
					}
					object defaultScriptParameterValue = scriptParameterBinder.GetDefaultScriptParameterValue(runtimeDefinedParameter, implicitUsingParameters);
					this.SaveDefaultScriptParameterValue(parameter.Parameter.Name, defaultScriptParameterValue);
					CommandParameterInternal argument = CommandParameterInternal.CreateParameterWithArgument(PositionUtilities.EmptyExtent, parameter.Parameter.Name, "-" + parameter.Parameter.Name + ":", PositionUtilities.EmptyExtent, defaultScriptParameterValue, false, false);
					ParameterBindingFlags parameterBindingFlags = ParameterBindingFlags.IsDefaultValue;
					if (runtimeDefinedParameter.IsSet)
					{
						parameterBindingFlags |= ParameterBindingFlags.ShouldCoerceType;
					}
					this.BindParameter(uint.MaxValue, argument, parameter, parameterBindingFlags);
				}
				finally
				{
					scriptParameterBinder.RecordBoundParameters = recordBoundParameters;
				}
			}
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0000FB88 File Offset: 0x0000DD88
		protected IScriptExtent GetErrorExtent(CommandParameterInternal cpi)
		{
			IScriptExtent scriptExtent = cpi.ErrorExtent;
			if (scriptExtent == PositionUtilities.EmptyExtent)
			{
				scriptExtent = this.InvocationInfo.ScriptPosition;
			}
			return scriptExtent;
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0000FBB4 File Offset: 0x0000DDB4
		protected IScriptExtent GetParameterErrorExtent(CommandParameterInternal cpi)
		{
			IScriptExtent scriptExtent = cpi.ParameterExtent;
			if (scriptExtent == PositionUtilities.EmptyExtent)
			{
				scriptExtent = this.InvocationInfo.ScriptPosition;
			}
			return scriptExtent;
		}

		// Token: 0x0400017F RID: 383
		private readonly ExecutionContext _context;

		// Token: 0x04000180 RID: 384
		private readonly InvocationInfo _invocationInfo;

		// Token: 0x04000181 RID: 385
		protected MergedCommandParameterMetadata _bindableParameters = new MergedCommandParameterMetadata();

		// Token: 0x04000182 RID: 386
		private readonly Dictionary<string, MergedCompiledCommandParameter> _boundParameters = new Dictionary<string, MergedCompiledCommandParameter>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000183 RID: 387
		private bool _defaultParameterBindingInUse;

		// Token: 0x04000184 RID: 388
		private readonly Collection<string> _boundDefaultParameters = new Collection<string>();

		// Token: 0x04000185 RID: 389
		private Collection<CommandParameterInternal> _unboundArguments = new Collection<CommandParameterInternal>();

		// Token: 0x04000186 RID: 390
		private readonly Dictionary<string, CommandParameterInternal> _boundArguments = new Dictionary<string, CommandParameterInternal>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000187 RID: 391
		private readonly Collection<MergedCompiledCommandParameter> _parametersBoundThroughPipelineInput = new Collection<MergedCompiledCommandParameter>();

		// Token: 0x04000188 RID: 392
		internal uint _currentParameterSetFlag = uint.MaxValue;

		// Token: 0x04000189 RID: 393
		internal uint _prePipelineProcessingParameterSetFlags = uint.MaxValue;
	}
}
