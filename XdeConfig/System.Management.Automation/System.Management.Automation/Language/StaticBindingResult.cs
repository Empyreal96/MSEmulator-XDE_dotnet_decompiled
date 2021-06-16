using System;
using System.Collections.Generic;
using System.Globalization;

namespace System.Management.Automation.Language
{
	// Token: 0x02000997 RID: 2455
	public class StaticBindingResult
	{
		// Token: 0x06005A87 RID: 23175 RVA: 0x001E6526 File Offset: 0x001E4726
		internal StaticBindingResult(CommandAst commandAst, PseudoBindingInfo bindingInfo)
		{
			this.boundParameters = new Dictionary<string, ParameterBindingResult>(StringComparer.OrdinalIgnoreCase);
			this.bindingExceptions = new Dictionary<string, StaticBindingError>(StringComparer.OrdinalIgnoreCase);
			if (bindingInfo == null)
			{
				this.CreateBindingResultForSyntacticBind(commandAst);
				return;
			}
			this.CreateBindingResultForSuccessfulBind(commandAst, bindingInfo);
		}

		// Token: 0x06005A88 RID: 23176 RVA: 0x001E6564 File Offset: 0x001E4764
		private void CreateBindingResultForSuccessfulBind(CommandAst commandAst, PseudoBindingInfo bindingInfo)
		{
			this.bindingInfo = bindingInfo;
			bool flag = bindingInfo.ValidParameterSetsFlags != uint.MaxValue;
			bool flag2 = bindingInfo.DefaultParameterSetFlag != 0U && (bindingInfo.ValidParameterSetsFlags & bindingInfo.DefaultParameterSetFlag) == bindingInfo.DefaultParameterSetFlag;
			bool flag3 = bindingInfo.ValidParameterSetsFlags != 0U && (bindingInfo.ValidParameterSetsFlags & bindingInfo.ValidParameterSetsFlags - 1U) == 0U;
			if (flag && !flag2 && !flag3)
			{
				ParameterBindingException exception = new ParameterBindingException(ErrorCategory.InvalidArgument, null, null, null, null, null, ParameterBinderStrings.AmbiguousParameterSet, "AmbiguousParameterSet", new object[0]);
				this.bindingExceptions.Add(commandAst.CommandElements[0].Extent.Text, new StaticBindingError(commandAst.CommandElements[0], exception));
			}
			if (bindingInfo.DuplicateParameters != null)
			{
				foreach (AstParameterArgumentPair astParameterArgumentPair in bindingInfo.DuplicateParameters)
				{
					this.AddDuplicateParameterBindingException(astParameterArgumentPair.Parameter);
				}
			}
			if (bindingInfo.ParametersNotFound != null)
			{
				foreach (CommandParameterAst commandParameterAst in bindingInfo.ParametersNotFound)
				{
					ParameterBindingException exception2 = new ParameterBindingException(ErrorCategory.InvalidArgument, null, commandParameterAst.ErrorPosition, commandParameterAst.ParameterName, null, null, ParameterBinderStrings.NamedParameterNotFound, "NamedParameterNotFound", new object[0]);
					this.bindingExceptions.Add(commandParameterAst.ParameterName, new StaticBindingError(commandParameterAst, exception2));
				}
			}
			if (bindingInfo.AmbiguousParameters != null)
			{
				foreach (CommandParameterAst commandParameterAst2 in bindingInfo.AmbiguousParameters)
				{
					ParameterBindingException exception3 = bindingInfo.BindingExceptions[commandParameterAst2];
					this.bindingExceptions.Add(commandParameterAst2.ParameterName, new StaticBindingError(commandParameterAst2, exception3));
				}
			}
			if (bindingInfo.UnboundArguments != null)
			{
				foreach (AstParameterArgumentPair astParameterArgumentPair2 in bindingInfo.UnboundArguments)
				{
					AstPair astPair = astParameterArgumentPair2 as AstPair;
					ParameterBindingException exception4 = new ParameterBindingException(ErrorCategory.InvalidArgument, null, astPair.Argument.Extent, astPair.Argument.Extent.Text, null, null, ParameterBinderStrings.PositionalParameterNotFound, "PositionalParameterNotFound", new object[0]);
					this.bindingExceptions.Add(astPair.Argument.Extent.Text, new StaticBindingError(astPair.Argument, exception4));
				}
			}
			if (bindingInfo.BoundParameters != null)
			{
				foreach (KeyValuePair<string, MergedCompiledCommandParameter> keyValuePair in bindingInfo.BoundParameters)
				{
					CompiledCommandParameter parameter = keyValuePair.Value.Parameter;
					CommandElementAst commandElementAst = null;
					object obj = null;
					AstPair astPair2 = bindingInfo.BoundArguments[keyValuePair.Key] as AstPair;
					if (astPair2 != null)
					{
						commandElementAst = astPair2.Argument;
					}
					AstArrayPair astArrayPair = bindingInfo.BoundArguments[keyValuePair.Key] as AstArrayPair;
					if (astArrayPair != null)
					{
						List<ExpressionAst> list = new List<ExpressionAst>();
						ExpressionAst[] argument = astArrayPair.Argument;
						int i = 0;
						while (i < argument.Length)
						{
							ExpressionAst expressionAst = argument[i];
							ArrayLiteralAst arrayLiteralAst = expressionAst as ArrayLiteralAst;
							if (arrayLiteralAst != null)
							{
								using (IEnumerator<ExpressionAst> enumerator6 = arrayLiteralAst.Elements.GetEnumerator())
								{
									while (enumerator6.MoveNext())
									{
										ExpressionAst expressionAst2 = enumerator6.Current;
										list.Add((ExpressionAst)expressionAst2.Copy());
									}
									goto IL_36F;
								}
								goto IL_35C;
							}
							goto IL_35C;
							IL_36F:
							i++;
							continue;
							IL_35C:
							list.Add((ExpressionAst)expressionAst.Copy());
							goto IL_36F;
						}
						IScriptExtent extent = list[0].Extent;
						ArrayLiteralAst arrayLiteralAst2 = new ArrayLiteralAst(extent, list);
						commandElementAst = arrayLiteralAst2;
					}
					if (parameter.Type == typeof(SwitchParameter))
					{
						if (commandElementAst != null && string.Equals("$false", commandElementAst.Extent.Text, StringComparison.OrdinalIgnoreCase))
						{
							continue;
						}
						obj = true;
					}
					if (commandElementAst != null || obj != null)
					{
						this.boundParameters.Add(keyValuePair.Key, new ParameterBindingResult(parameter, commandElementAst, obj));
					}
					else
					{
						bool flag4 = false;
						foreach (ParameterSetSpecificMetadata parameterSetSpecificMetadata in parameter.GetMatchingParameterSetData(bindingInfo.ValidParameterSetsFlags))
						{
							if (parameterSetSpecificMetadata.ValueFromPipeline)
							{
								flag4 = true;
								break;
							}
						}
						if (!flag4)
						{
							ParameterBindingException exception5 = new ParameterBindingException(ErrorCategory.InvalidArgument, null, commandAst.CommandElements[0].Extent, parameter.Name, parameter.Type, null, ParameterBinderStrings.MissingArgument, "MissingArgument", new object[0]);
							this.bindingExceptions.Add(commandAst.CommandElements[0].Extent.Text, new StaticBindingError(commandAst.CommandElements[0], exception5));
						}
					}
				}
			}
		}

		// Token: 0x06005A89 RID: 23177 RVA: 0x001E6AF8 File Offset: 0x001E4CF8
		private void AddDuplicateParameterBindingException(CommandParameterAst duplicateParameter)
		{
			if (duplicateParameter == null)
			{
				return;
			}
			ParameterBindingException exception = new ParameterBindingException(ErrorCategory.InvalidArgument, null, duplicateParameter.ErrorPosition, duplicateParameter.ParameterName, null, null, ParameterBinderStrings.ParameterAlreadyBound, "ParameterAlreadyBound", new object[0]);
			if (!this.bindingExceptions.ContainsKey(duplicateParameter.ParameterName))
			{
				this.bindingExceptions.Add(duplicateParameter.ParameterName, new StaticBindingError(duplicateParameter, exception));
			}
		}

		// Token: 0x06005A8A RID: 23178 RVA: 0x001E6B5C File Offset: 0x001E4D5C
		private void CreateBindingResultForSyntacticBind(CommandAst commandAst)
		{
			bool flag = false;
			CommandParameterAst commandParameterAst = null;
			int num = 0;
			ParameterBindingResult parameterBindingResult = new ParameterBindingResult();
			foreach (CommandElementAst commandElementAst in commandAst.CommandElements)
			{
				if (!flag)
				{
					flag = true;
				}
				else
				{
					CommandParameterAst commandParameterAst2 = commandElementAst as CommandParameterAst;
					if (commandParameterAst2 != null)
					{
						if (commandParameterAst != null)
						{
							this.AddSwitch(commandParameterAst.ParameterName, parameterBindingResult);
							StaticBindingResult.ResetCurrentParameter(ref commandParameterAst, ref parameterBindingResult);
						}
						string parameterName = commandParameterAst2.ParameterName;
						parameterBindingResult.Value = commandParameterAst2;
						if (commandParameterAst2.Argument != null)
						{
							parameterBindingResult.Value = commandParameterAst2.Argument;
							this.AddBoundParameter(commandParameterAst2, parameterName, parameterBindingResult);
							StaticBindingResult.ResetCurrentParameter(ref commandParameterAst, ref parameterBindingResult);
						}
						else
						{
							commandParameterAst = commandParameterAst2;
						}
					}
					else
					{
						if (commandParameterAst != null)
						{
							parameterBindingResult.Value = commandElementAst;
							this.AddBoundParameter(commandParameterAst, commandParameterAst.ParameterName, parameterBindingResult);
						}
						else
						{
							parameterBindingResult.Value = commandElementAst;
							this.AddBoundParameter(null, num.ToString(CultureInfo.InvariantCulture), parameterBindingResult);
							num++;
						}
						StaticBindingResult.ResetCurrentParameter(ref commandParameterAst, ref parameterBindingResult);
					}
				}
			}
			if (commandParameterAst != null)
			{
				this.AddSwitch(commandParameterAst.ParameterName, parameterBindingResult);
			}
		}

		// Token: 0x06005A8B RID: 23179 RVA: 0x001E6C80 File Offset: 0x001E4E80
		private void AddBoundParameter(CommandParameterAst parameter, string parameterName, ParameterBindingResult bindingResult)
		{
			if (this.boundParameters.ContainsKey(parameterName))
			{
				this.AddDuplicateParameterBindingException(parameter);
				return;
			}
			this.boundParameters.Add(parameterName, bindingResult);
		}

		// Token: 0x06005A8C RID: 23180 RVA: 0x001E6CA5 File Offset: 0x001E4EA5
		private static void ResetCurrentParameter(ref CommandParameterAst currentParameter, ref ParameterBindingResult bindingResult)
		{
			currentParameter = null;
			bindingResult = new ParameterBindingResult();
		}

		// Token: 0x06005A8D RID: 23181 RVA: 0x001E6CB1 File Offset: 0x001E4EB1
		private void AddSwitch(string currentParameter, ParameterBindingResult bindingResult)
		{
			bindingResult.ConstantValue = true;
			this.AddBoundParameter(null, currentParameter, bindingResult);
		}

		// Token: 0x17001216 RID: 4630
		// (get) Token: 0x06005A8E RID: 23182 RVA: 0x001E6CC8 File Offset: 0x001E4EC8
		public Dictionary<string, ParameterBindingResult> BoundParameters
		{
			get
			{
				return this.boundParameters;
			}
		}

		// Token: 0x17001217 RID: 4631
		// (get) Token: 0x06005A8F RID: 23183 RVA: 0x001E6CD0 File Offset: 0x001E4ED0
		public Dictionary<string, StaticBindingError> BindingExceptions
		{
			get
			{
				return this.bindingExceptions;
			}
		}

		// Token: 0x04003064 RID: 12388
		private PseudoBindingInfo bindingInfo;

		// Token: 0x04003065 RID: 12389
		private Dictionary<string, ParameterBindingResult> boundParameters;

		// Token: 0x04003066 RID: 12390
		private Dictionary<string, StaticBindingError> bindingExceptions;
	}
}
