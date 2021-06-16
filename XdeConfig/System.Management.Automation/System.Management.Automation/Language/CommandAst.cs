using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Microsoft.Management.Infrastructure;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation.Language
{
	// Token: 0x0200056C RID: 1388
	public class CommandAst : CommandBaseAst
	{
		// Token: 0x06003987 RID: 14727 RVA: 0x00130108 File Offset: 0x0012E308
		public CommandAst(IScriptExtent extent, IEnumerable<CommandElementAst> commandElements, TokenKind invocationOperator, IEnumerable<RedirectionAst> redirections) : base(extent, redirections)
		{
			if (commandElements == null || !commandElements.Any<CommandElementAst>())
			{
				throw PSTraceSource.NewArgumentException("commandElements");
			}
			if (invocationOperator != TokenKind.Dot && invocationOperator != TokenKind.Ampersand && invocationOperator != TokenKind.Unknown)
			{
				throw PSTraceSource.NewArgumentException("invocationOperator");
			}
			this.CommandElements = new ReadOnlyCollection<CommandElementAst>(commandElements.ToArray<CommandElementAst>());
			base.SetParents<CommandElementAst>(this.CommandElements);
			this.InvocationOperator = invocationOperator;
		}

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x06003988 RID: 14728 RVA: 0x00130170 File Offset: 0x0012E370
		// (set) Token: 0x06003989 RID: 14729 RVA: 0x00130178 File Offset: 0x0012E378
		public ReadOnlyCollection<CommandElementAst> CommandElements { get; private set; }

		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x0600398A RID: 14730 RVA: 0x00130181 File Offset: 0x0012E381
		// (set) Token: 0x0600398B RID: 14731 RVA: 0x00130189 File Offset: 0x0012E389
		public TokenKind InvocationOperator { get; private set; }

		// Token: 0x0600398C RID: 14732 RVA: 0x00130194 File Offset: 0x0012E394
		public string GetCommandName()
		{
			StringConstantExpressionAst stringConstantExpressionAst = this.CommandElements[0] as StringConstantExpressionAst;
			if (stringConstantExpressionAst == null)
			{
				return null;
			}
			return stringConstantExpressionAst.Value;
		}

		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x0600398D RID: 14733 RVA: 0x001301BE File Offset: 0x0012E3BE
		// (set) Token: 0x0600398E RID: 14734 RVA: 0x001301C6 File Offset: 0x0012E3C6
		public DynamicKeyword DefiningKeyword { get; set; }

		// Token: 0x0600398F RID: 14735 RVA: 0x001301D0 File Offset: 0x0012E3D0
		public override Ast Copy()
		{
			CommandElementAst[] commandElements = Ast.CopyElements<CommandElementAst>(this.CommandElements);
			RedirectionAst[] redirections = Ast.CopyElements<RedirectionAst>(base.Redirections);
			return new CommandAst(base.Extent, commandElements, this.InvocationOperator, redirections)
			{
				DefiningKeyword = this.DefiningKeyword
			};
		}

		// Token: 0x06003990 RID: 14736 RVA: 0x00130B30 File Offset: 0x0012ED30
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			PseudoBindingInfo pseudoBinding = new PseudoParameterBinder().DoPseudoParameterBinding(this, null, null, PseudoParameterBinder.BindingType.ParameterCompletion);
			if (pseudoBinding != null && pseudoBinding.CommandInfo != null)
			{
				string pathParameterName = "Path";
				AstParameterArgumentPair pathArgument;
				if (!pseudoBinding.BoundArguments.TryGetValue(pathParameterName, out pathArgument))
				{
					pathParameterName = "LiteralPath";
					pseudoBinding.BoundArguments.TryGetValue(pathParameterName, out pathArgument);
				}
				CommandInfo commandInfo = pseudoBinding.CommandInfo;
				AstPair pathArgumentPair = pathArgument as AstPair;
				if (pathArgumentPair != null && pathArgumentPair.Argument is StringConstantExpressionAst)
				{
					string value = ((StringConstantExpressionAst)pathArgumentPair.Argument).Value;
					try
					{
						commandInfo = commandInfo.CreateGetCommandCopy(new string[]
						{
							"-" + pathParameterName,
							value
						});
					}
					catch (InvalidOperationException)
					{
					}
				}
				CmdletInfo cmdletInfo = commandInfo as CmdletInfo;
				if (cmdletInfo != null)
				{
					if (cmdletInfo.ImplementingType.FullName.Equals("Microsoft.PowerShell.Commands.NewObjectCommand", StringComparison.Ordinal))
					{
						AstParameterArgumentPair typeArgument;
						if (!pseudoBinding.BoundArguments.TryGetValue("TypeName", out typeArgument))
						{
							goto IL_6B1;
						}
						AstPair typeArgumentPair = typeArgument as AstPair;
						if (typeArgumentPair != null && typeArgumentPair.Argument is StringConstantExpressionAst)
						{
							yield return new PSTypeName(((StringConstantExpressionAst)typeArgumentPair.Argument).Value);
							goto IL_6B1;
						}
						goto IL_6B1;
					}
					else
					{
						if (cmdletInfo.ImplementingType.FullName.Equals("Microsoft.Management.Infrastructure.CimCmdlets.GetCimInstanceCommand", StringComparison.Ordinal) || cmdletInfo.ImplementingType.FullName.Equals("Microsoft.Management.Infrastructure.CimCmdlets.NewCimInstanceCommand", StringComparison.Ordinal))
						{
							string pseudoboundNamespace = CompletionCompleters.NativeCommandArgumentCompletion_ExtractSecondaryArgument(pseudoBinding.BoundArguments, "Namespace").FirstOrDefault<string>();
							string pseudoboundClassName = CompletionCompleters.NativeCommandArgumentCompletion_ExtractSecondaryArgument(pseudoBinding.BoundArguments, "ClassName").FirstOrDefault<string>();
							if (!string.IsNullOrWhiteSpace(pseudoboundClassName))
							{
								yield return new PSTypeName(string.Format(CultureInfo.InvariantCulture, "{0}#{1}/{2}", new object[]
								{
									typeof(CimInstance).FullName,
									pseudoboundNamespace ?? "root/cimv2",
									pseudoboundClassName
								}));
							}
							yield return new PSTypeName(typeof(CimInstance));
							goto IL_6B1;
						}
						if (cmdletInfo.ImplementingType == typeof(WhereObjectCommand) || cmdletInfo.ImplementingType.FullName.Equals("Microsoft.PowerShell.Commands.SortObjectCommand", StringComparison.Ordinal))
						{
							PipelineAst parentPipeline = base.Parent as PipelineAst;
							if (parentPipeline == null)
							{
								goto IL_6B1;
							}
							int i = 0;
							while (i < parentPipeline.PipelineElements.Count && parentPipeline.PipelineElements[i] != this)
							{
								i++;
							}
							if (i > 0)
							{
								foreach (PSTypeName typename in parentPipeline.PipelineElements[i - 1].GetInferredType(context))
								{
									yield return typename;
								}
								goto IL_6B1;
							}
							goto IL_6B1;
						}
						else if (cmdletInfo.ImplementingType == typeof(ForEachObjectCommand))
						{
							AstParameterArgumentPair argument;
							if (pseudoBinding.BoundArguments.TryGetValue("Begin", out argument))
							{
								foreach (PSTypeName type in this.GetInferredTypeFromScriptBlockParameter(argument, context))
								{
									yield return type;
								}
							}
							if (pseudoBinding.BoundArguments.TryGetValue("Process", out argument))
							{
								foreach (PSTypeName type2 in this.GetInferredTypeFromScriptBlockParameter(argument, context))
								{
									yield return type2;
								}
							}
							if (pseudoBinding.BoundArguments.TryGetValue("End", out argument))
							{
								foreach (PSTypeName type3 in this.GetInferredTypeFromScriptBlockParameter(argument, context))
								{
									yield return type3;
								}
							}
						}
					}
				}
				foreach (PSTypeName result in commandInfo.OutputType)
				{
					yield return result;
				}
			}
			IL_6B1:
			yield break;
		}

		// Token: 0x06003991 RID: 14737 RVA: 0x00130D4C File Offset: 0x0012EF4C
		private IEnumerable<PSTypeName> GetInferredTypeFromScriptBlockParameter(AstParameterArgumentPair argument, CompletionContext context)
		{
			AstPair argumentPair = argument as AstPair;
			if (argumentPair != null && argumentPair.Argument is ScriptBlockExpressionAst)
			{
				ScriptBlockExpressionAst scriptBlockExpressionAst = (ScriptBlockExpressionAst)argumentPair.Argument;
				foreach (PSTypeName type in scriptBlockExpressionAst.ScriptBlock.GetInferredType(context))
				{
					yield return type;
				}
			}
			yield break;
		}

		// Token: 0x06003992 RID: 14738 RVA: 0x00130D77 File Offset: 0x0012EF77
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitCommand(this);
		}

		// Token: 0x06003993 RID: 14739 RVA: 0x00130D80 File Offset: 0x0012EF80
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitCommand(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				for (int i = 0; i < this.CommandElements.Count; i++)
				{
					CommandElementAst commandElementAst = this.CommandElements[i];
					astVisitAction = commandElementAst.InternalVisit(visitor);
					if (astVisitAction != AstVisitAction.Continue)
					{
						break;
					}
				}
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				for (int j = 0; j < base.Redirections.Count; j++)
				{
					RedirectionAst redirectionAst = base.Redirections[j];
					if (astVisitAction == AstVisitAction.Continue)
					{
						astVisitAction = redirectionAst.InternalVisit(visitor);
					}
				}
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}
	}
}
