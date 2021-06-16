using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation.Language
{
	// Token: 0x02000574 RID: 1396
	public class ConfigurationDefinitionAst : StatementAst
	{
		// Token: 0x060039BB RID: 14779 RVA: 0x001313F0 File Offset: 0x0012F5F0
		public ConfigurationDefinitionAst(IScriptExtent extent, ScriptBlockExpressionAst body, ConfigurationType type, ExpressionAst instanceName) : base(extent)
		{
			if (extent == null)
			{
				throw PSTraceSource.NewArgumentNullException("extent");
			}
			if (body == null)
			{
				throw PSTraceSource.NewArgumentNullException("body");
			}
			if (instanceName == null)
			{
				throw PSTraceSource.NewArgumentNullException("instanceName");
			}
			this.Body = body;
			base.SetParent(body);
			this.ConfigurationType = type;
			this.InstanceName = instanceName;
			base.SetParent(instanceName);
		}

		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x060039BC RID: 14780 RVA: 0x00131454 File Offset: 0x0012F654
		// (set) Token: 0x060039BD RID: 14781 RVA: 0x0013145C File Offset: 0x0012F65C
		public ScriptBlockExpressionAst Body { get; private set; }

		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x060039BE RID: 14782 RVA: 0x00131465 File Offset: 0x0012F665
		// (set) Token: 0x060039BF RID: 14783 RVA: 0x0013146D File Offset: 0x0012F66D
		public ConfigurationType ConfigurationType { get; private set; }

		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x060039C0 RID: 14784 RVA: 0x00131476 File Offset: 0x0012F676
		// (set) Token: 0x060039C1 RID: 14785 RVA: 0x0013147E File Offset: 0x0012F67E
		public ExpressionAst InstanceName { get; private set; }

		// Token: 0x060039C2 RID: 14786 RVA: 0x00131494 File Offset: 0x0012F694
		public override Ast Copy()
		{
			ScriptBlockExpressionAst body = Ast.CopyElement<ScriptBlockExpressionAst>(this.Body);
			ExpressionAst instanceName = Ast.CopyElement<ExpressionAst>(this.InstanceName);
			ConfigurationDefinitionAst configurationDefinitionAst = new ConfigurationDefinitionAst(base.Extent, body, this.ConfigurationType, instanceName);
			configurationDefinitionAst.LCurlyToken = this.LCurlyToken;
			configurationDefinitionAst.ConfigurationToken = this.ConfigurationToken;
			ConfigurationDefinitionAst configurationDefinitionAst2 = configurationDefinitionAst;
			IEnumerable<AttributeAst> customAttributes;
			if (this.CustomAttributes != null)
			{
				customAttributes = from e in this.CustomAttributes
				select (AttributeAst)e.Copy();
			}
			else
			{
				customAttributes = null;
			}
			configurationDefinitionAst2.CustomAttributes = customAttributes;
			return configurationDefinitionAst;
		}

		// Token: 0x060039C3 RID: 14787 RVA: 0x0013151F File Offset: 0x0012F71F
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return this.Body.GetInferredType(context);
		}

		// Token: 0x060039C4 RID: 14788 RVA: 0x00131530 File Offset: 0x0012F730
		internal override object Accept(ICustomAstVisitor visitor)
		{
			ICustomAstVisitor2 customAstVisitor = visitor as ICustomAstVisitor2;
			if (customAstVisitor == null)
			{
				return null;
			}
			return customAstVisitor.VisitConfigurationDefinition(this);
		}

		// Token: 0x060039C5 RID: 14789 RVA: 0x00131550 File Offset: 0x0012F750
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = AstVisitAction.Continue;
			AstVisitor2 astVisitor = visitor as AstVisitor2;
			if (astVisitor != null)
			{
				astVisitAction = astVisitor.VisitConfigurationDefinition(this);
				if (astVisitAction == AstVisitAction.SkipChildren)
				{
					return visitor.CheckForPostAction(this, AstVisitAction.Continue);
				}
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.InstanceName.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				this.Body.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x060039C6 RID: 14790 RVA: 0x001315A6 File Offset: 0x0012F7A6
		// (set) Token: 0x060039C7 RID: 14791 RVA: 0x001315AE File Offset: 0x0012F7AE
		internal Token LCurlyToken { get; set; }

		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x060039C8 RID: 14792 RVA: 0x001315B7 File Offset: 0x0012F7B7
		// (set) Token: 0x060039C9 RID: 14793 RVA: 0x001315BF File Offset: 0x0012F7BF
		internal Token ConfigurationToken { get; set; }

		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x060039CA RID: 14794 RVA: 0x001315C8 File Offset: 0x0012F7C8
		// (set) Token: 0x060039CB RID: 14795 RVA: 0x001315D0 File Offset: 0x0012F7D0
		internal IEnumerable<AttributeAst> CustomAttributes { get; set; }

		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x060039CC RID: 14796 RVA: 0x001315D9 File Offset: 0x0012F7D9
		// (set) Token: 0x060039CD RID: 14797 RVA: 0x001315E1 File Offset: 0x0012F7E1
		internal List<DynamicKeyword> DefinedKeywords { get; set; }

		// Token: 0x060039CE RID: 14798 RVA: 0x00131660 File Offset: 0x0012F860
		internal PipelineAst GenerateSetItemPipelineAst()
		{
			Collection<CommandElementAst> collection = new Collection<CommandElementAst>
			{
				new StringConstantExpressionAst(base.Extent, "PSDesiredStateConfiguration\\Configuration", StringConstantType.BareWord),
				new CommandParameterAst(this.LCurlyToken.Extent, "ArgsToBody", new VariableExpressionAst(this.LCurlyToken.Extent, "toBody", false), this.LCurlyToken.Extent),
				new CommandParameterAst(this.LCurlyToken.Extent, "Name", (ExpressionAst)this.InstanceName.Copy(), this.LCurlyToken.Extent)
			};
			ReadOnlyCollection<StatementAst> statements = this.Body.ScriptBlock.EndBlock.Statements;
			List<Tuple<string[], ModuleSpecification[], Version>> resourceModulePairsToImport = new List<Tuple<string[], ModuleSpecification[], Version>>();
			List<StatementAst> statements2 = (from stm in statements
			where !ConfigurationDefinitionAst.IsImportCommand(stm, resourceModulePairsToImport)
			select (StatementAst)stm.Copy()).ToList<StatementAst>();
			collection.Add(new CommandParameterAst(PositionUtilities.EmptyExtent, "ResourceModuleTuplesToImport", new ConstantExpressionAst(PositionUtilities.EmptyExtent, resourceModulePairsToImport), PositionUtilities.EmptyExtent));
			IScriptExtent extent = this.Body.Extent;
			IEnumerable<AttributeAst> attributes;
			if (this.CustomAttributes != null)
			{
				attributes = (from att in this.CustomAttributes
				select (AttributeAst)att.Copy()).ToList<AttributeAst>();
			}
			else
			{
				attributes = null;
			}
			ScriptBlockAst scriptBlock = new ScriptBlockAst(extent, attributes, null, new StatementBlockAst(this.Body.Extent, statements2, null), false, false);
			ScriptBlockExpressionAst argument = new ScriptBlockExpressionAst(this.Body.Extent, scriptBlock);
			collection.Add(new CommandParameterAst(this.LCurlyToken.Extent, "Body", argument, this.LCurlyToken.Extent));
			collection.Add(new CommandParameterAst(this.LCurlyToken.Extent, "Outputpath", new VariableExpressionAst(this.LCurlyToken.Extent, "OutputPath", false), this.LCurlyToken.Extent));
			collection.Add(new CommandParameterAst(this.LCurlyToken.Extent, "ConfigurationData", new VariableExpressionAst(this.LCurlyToken.Extent, "ConfigurationData", false), this.LCurlyToken.Extent));
			collection.Add(new CommandParameterAst(this.LCurlyToken.Extent, "InstanceName", new VariableExpressionAst(this.LCurlyToken.Extent, "InstanceName", false), this.LCurlyToken.Extent));
			List<AttributeAst> attributes2 = (from attribAst in ConfigurationDefinitionAst.ConfigurationBuildInParameterAttribAsts
			select (AttributeAst)attribAst.Copy()).ToList<AttributeAst>();
			List<ParameterAst> list = (from paramAst in ConfigurationDefinitionAst.ConfigurationBuildInParameters
			select (ParameterAst)paramAst.Copy()).ToList<ParameterAst>();
			if (this.Body.ScriptBlock.ParamBlock != null)
			{
				list.AddRange(from parameterAst in this.Body.ScriptBlock.ParamBlock.Parameters
				select (ParameterAst)parameterAst.Copy());
			}
			ParamBlockAst paramBlock = new ParamBlockAst(base.Extent, attributes2, list);
			CommandAst commandAst = new CommandAst(base.Extent, collection, TokenKind.Unknown, null);
			PipelineAst item = new PipelineAst(base.Extent, commandAst);
			List<StatementAst> list2 = (from statement in ConfigurationDefinitionAst.ConfigurationExtraParameterStatements
			select (StatementAst)statement.Copy()).ToList<StatementAst>();
			list2.Add(item);
			StatementBlockAst statements3 = new StatementBlockAst(base.Extent, list2, null);
			IScriptExtent extent2 = this.Body.Extent;
			IEnumerable<AttributeAst> attributes3;
			if (this.CustomAttributes != null)
			{
				attributes3 = (from att in this.CustomAttributes
				select (AttributeAst)att.Copy()).ToList<AttributeAst>();
			}
			else
			{
				attributes3 = null;
			}
			ScriptBlockAst scriptBlock2 = new ScriptBlockAst(extent2, attributes3, paramBlock, statements3, false, true);
			ScriptBlockExpressionAst argument2 = new ScriptBlockExpressionAst(base.Extent, scriptBlock2);
			StringConstantExpressionAst left = new StringConstantExpressionAst(base.Extent, "function:\\", StringConstantType.BareWord);
			BinaryExpressionAst argument3 = new BinaryExpressionAst(base.Extent, left, TokenKind.Plus, (ExpressionAst)this.InstanceName.Copy(), base.Extent);
			Collection<CommandElementAst> commandElements = new Collection<CommandElementAst>
			{
				new StringConstantExpressionAst(base.Extent, "set-item", StringConstantType.BareWord),
				new CommandParameterAst(base.Extent, "Path", argument3, base.Extent),
				new CommandParameterAst(base.Extent, "Value", argument2, base.Extent)
			};
			CommandAst commandAst2 = new CommandAst(base.Extent, commandElements, TokenKind.Unknown, null);
			PipelineAst pipelineAst = new PipelineAst(base.Extent, commandAst2);
			base.SetParent(pipelineAst);
			return pipelineAst;
		}

		// Token: 0x060039CF RID: 14799 RVA: 0x00131B28 File Offset: 0x0012FD28
		private static bool IsImportCommand(StatementAst stmt, List<Tuple<string[], ModuleSpecification[], Version>> resourceModulePairsToImport)
		{
			DynamicKeywordStatementAst dynamicKeywordStatementAst = stmt as DynamicKeywordStatementAst;
			if (dynamicKeywordStatementAst == null || !dynamicKeywordStatementAst.Keyword.Keyword.Equals("Import-DscResource", StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			CommandAst commandAst = new CommandAst(dynamicKeywordStatementAst.Extent, Ast.CopyElements<CommandElementAst>(dynamicKeywordStatementAst.CommandElements), TokenKind.Unknown, null);
			StaticBindingResult staticBindingResult = StaticParameterBinder.BindCommand(commandAst, false);
			ParameterBindingResult parameterBindingResult = null;
			ParameterBindingResult parameterBindingResult2 = null;
			ParameterBindingResult parameterBindingResult3 = null;
			foreach (KeyValuePair<string, ParameterBindingResult> keyValuePair in staticBindingResult.BoundParameters)
			{
				string key = keyValuePair.Key;
				ParameterBindingResult value = keyValuePair.Value;
				if (key.Length <= "Name".Length && key.Equals("Name".Substring(0, key.Length), StringComparison.OrdinalIgnoreCase))
				{
					parameterBindingResult2 = value;
				}
				if (key.Length <= "ModuleName".Length && key.Equals("ModuleName".Substring(0, key.Length), StringComparison.OrdinalIgnoreCase))
				{
					parameterBindingResult = value;
				}
				else if (key.Length <= "ModuleVersion".Length && key.Equals("ModuleVersion".Substring(0, key.Length), StringComparison.OrdinalIgnoreCase))
				{
					parameterBindingResult3 = value;
				}
			}
			string[] item = new string[]
			{
				"*"
			};
			ModuleSpecification[] array = null;
			Version version = null;
			if (parameterBindingResult2 != null)
			{
				object valueToConvert;
				IsConstantValueVisitor.IsConstant(parameterBindingResult2.Value, out valueToConvert, true, true);
				item = LanguagePrimitives.ConvertTo<string[]>(valueToConvert);
			}
			if (parameterBindingResult != null)
			{
				object valueToConvert2;
				IsConstantValueVisitor.IsConstant(parameterBindingResult.Value, out valueToConvert2, true, true);
				array = LanguagePrimitives.ConvertTo<ModuleSpecification[]>(valueToConvert2);
			}
			if (parameterBindingResult3 != null)
			{
				object text;
				IsConstantValueVisitor.IsConstant(parameterBindingResult3.Value, out text, true, true);
				if (text is double)
				{
					text = parameterBindingResult3.Value.Extent.Text;
				}
				version = LanguagePrimitives.ConvertTo<Version>(text);
				if (array != null && array.Length == 1)
				{
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = new ModuleSpecification(new Hashtable
						{
							{
								"ModuleName",
								array[i].Name
							},
							{
								"ModuleVersion",
								version
							}
						});
					}
				}
			}
			resourceModulePairsToImport.Add(new Tuple<string[], ModuleSpecification[], Version>(item, array, version));
			return true;
		}

		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x060039D0 RID: 14800 RVA: 0x00131D70 File Offset: 0x0012FF70
		private static IEnumerable<ParameterAst> ConfigurationBuildInParameters
		{
			get
			{
				if (ConfigurationDefinitionAst._configurationBuildInParameters == null)
				{
					ConfigurationDefinitionAst._configurationBuildInParameters = new List<ParameterAst>();
					ScriptBlock scriptBlock = ScriptBlock.Create("\r\n                        [cmdletbinding()]\r\n                        param(\r\n                            [string]\r\n                                $InstanceName,\r\n                            [string[]]\r\n                                $DependsOn,\r\n                            [string]\r\n                                $OutputPath,\r\n                            [hashtable]\r\n                            [Microsoft.PowerShell.DesiredStateConfiguration.ArgumentToConfigurationDataTransformation()]\r\n                               $ConfigurationData\r\n                        )");
					ScriptBlockAst scriptBlockAst = scriptBlock.Ast as ScriptBlockAst;
					if (scriptBlockAst != null)
					{
						foreach (ParameterAst parameterAst in scriptBlockAst.ParamBlock.Parameters)
						{
							ConfigurationDefinitionAst._configurationBuildInParameters.Add((ParameterAst)parameterAst.Copy());
						}
					}
				}
				return ConfigurationDefinitionAst._configurationBuildInParameters;
			}
		}

		// Token: 0x17000CD6 RID: 3286
		// (get) Token: 0x060039D1 RID: 14801 RVA: 0x00131E00 File Offset: 0x00130000
		private static IEnumerable<AttributeAst> ConfigurationBuildInParameterAttribAsts
		{
			get
			{
				if (ConfigurationDefinitionAst._configurationBuildInParameterAttrAsts == null)
				{
					ConfigurationDefinitionAst._configurationBuildInParameterAttrAsts = new List<AttributeAst>();
					ScriptBlock scriptBlock = ScriptBlock.Create("\r\n                        [cmdletbinding()]\r\n                        param(\r\n                            [string]\r\n                                $InstanceName,\r\n                            [string[]]\r\n                                $DependsOn,\r\n                            [string]\r\n                                $OutputPath,\r\n                            [hashtable]\r\n                            [Microsoft.PowerShell.DesiredStateConfiguration.ArgumentToConfigurationDataTransformation()]\r\n                               $ConfigurationData\r\n                        )");
					ScriptBlockAst scriptBlockAst = scriptBlock.Ast as ScriptBlockAst;
					if (scriptBlockAst != null)
					{
						if (ConfigurationDefinitionAst._configurationBuildInParameters == null)
						{
							ConfigurationDefinitionAst._configurationBuildInParameters = new List<ParameterAst>();
							foreach (ParameterAst parameterAst in scriptBlockAst.ParamBlock.Parameters)
							{
								ConfigurationDefinitionAst._configurationBuildInParameters.Add((ParameterAst)parameterAst.Copy());
							}
						}
						foreach (AttributeAst attributeAst in scriptBlockAst.ParamBlock.Attributes)
						{
							ConfigurationDefinitionAst._configurationBuildInParameterAttrAsts.Add((AttributeAst)attributeAst.Copy());
						}
					}
				}
				return ConfigurationDefinitionAst._configurationBuildInParameterAttrAsts;
			}
		}

		// Token: 0x17000CD7 RID: 3287
		// (get) Token: 0x060039D2 RID: 14802 RVA: 0x00131F00 File Offset: 0x00130100
		private static IEnumerable<StatementAst> ConfigurationExtraParameterStatements
		{
			get
			{
				if (ConfigurationDefinitionAst._configurationExtraParameterStatements == null)
				{
					ConfigurationDefinitionAst._configurationExtraParameterStatements = new List<StatementAst>();
					ScriptBlock scriptBlock = ScriptBlock.Create("\r\n                        Import-Module Microsoft.PowerShell.Management -Verbose:$false\r\n                        Import-Module PSDesiredStateConfiguration -Verbose:$false\r\n                        $toBody = @{}+$PSBoundParameters\r\n                        $toBody.Remove(\"OutputPath\")\r\n                        $toBody.Remove(\"ConfigurationData\")\r\n                        $ConfigurationData = $psboundparameters[\"ConfigurationData\"]\r\n                        $Outputpath = $psboundparameters[\"Outputpath\"]");
					ScriptBlockAst scriptBlockAst = scriptBlock.Ast as ScriptBlockAst;
					if (scriptBlockAst != null)
					{
						foreach (StatementAst statementAst in scriptBlockAst.EndBlock.Statements)
						{
							ConfigurationDefinitionAst._configurationExtraParameterStatements.Add((StatementAst)statementAst.Copy());
						}
					}
				}
				return ConfigurationDefinitionAst._configurationExtraParameterStatements;
			}
		}

		// Token: 0x04001D2D RID: 7469
		private const string ConfigurationBuildInParametersStr = "\r\n                        [cmdletbinding()]\r\n                        param(\r\n                            [string]\r\n                                $InstanceName,\r\n                            [string[]]\r\n                                $DependsOn,\r\n                            [string]\r\n                                $OutputPath,\r\n                            [hashtable]\r\n                            [Microsoft.PowerShell.DesiredStateConfiguration.ArgumentToConfigurationDataTransformation()]\r\n                               $ConfigurationData\r\n                        )";

		// Token: 0x04001D2E RID: 7470
		private static List<ParameterAst> _configurationBuildInParameters;

		// Token: 0x04001D2F RID: 7471
		private static List<AttributeAst> _configurationBuildInParameterAttrAsts;

		// Token: 0x04001D30 RID: 7472
		private static List<StatementAst> _configurationExtraParameterStatements;
	}
}
