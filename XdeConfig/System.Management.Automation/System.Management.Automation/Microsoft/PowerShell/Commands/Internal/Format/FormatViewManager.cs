using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Text;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004B0 RID: 1200
	internal sealed class FormatViewManager
	{
		// Token: 0x06003570 RID: 13680 RVA: 0x00122E9C File Offset: 0x0012109C
		private static string PSObjectTypeName(PSObject so)
		{
			if (so != null)
			{
				ConsolidatedString internalTypeNames = so.InternalTypeNames;
				if (internalTypeNames.Count > 0)
				{
					return internalTypeNames[0];
				}
			}
			return "";
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x00122ECC File Offset: 0x001210CC
		internal void Initialize(TerminatingErrorContext errorContext, MshExpressionFactory expressionFactory, TypeInfoDataBase db, PSObject so, FormatShape shape, FormattingCommandLineParameters parameters)
		{
			ViewDefinition viewDefinition = null;
			try
			{
				DisplayDataQuery.SetTracer(FormatViewManager.formatViewBindingTracer);
				ConsolidatedString internalTypeNames = so.InternalTypeNames;
				if (shape == FormatShape.Undefined)
				{
					using (FormatViewManager.formatViewBindingTracer.TraceScope("FINDING VIEW  TYPE: {0}", new object[]
					{
						FormatViewManager.PSObjectTypeName(so)
					}))
					{
						viewDefinition = DisplayDataQuery.GetViewByShapeAndType(expressionFactory, db, shape, internalTypeNames, null);
					}
					if (viewDefinition != null)
					{
						this.viewGenerator = FormatViewManager.SelectViewGeneratorFromViewDefinition(errorContext, expressionFactory, db, viewDefinition, parameters);
						FormatViewManager.formatViewBindingTracer.WriteLine("An applicable view has been found", new object[0]);
						FormatViewManager.PrepareViewForRemoteObjects(this.ViewGenerator, so);
					}
					else
					{
						FormatViewManager.formatViewBindingTracer.WriteLine("No applicable view has been found", new object[0]);
						this.viewGenerator = FormatViewManager.SelectViewGeneratorFromProperties(shape, so, errorContext, expressionFactory, db, null);
						FormatViewManager.PrepareViewForRemoteObjects(this.ViewGenerator, so);
					}
				}
				else if (parameters != null && parameters.mshParameterList.Count > 0)
				{
					this.viewGenerator = FormatViewManager.SelectViewGeneratorFromProperties(shape, so, errorContext, expressionFactory, db, parameters);
				}
				else
				{
					if (parameters != null && !string.IsNullOrEmpty(parameters.viewName))
					{
						using (FormatViewManager.formatViewBindingTracer.TraceScope("FINDING VIEW NAME: {0}  TYPE: {1}", new object[]
						{
							parameters.viewName,
							FormatViewManager.PSObjectTypeName(so)
						}))
						{
							viewDefinition = DisplayDataQuery.GetViewByShapeAndType(expressionFactory, db, shape, internalTypeNames, parameters.viewName);
						}
						if (viewDefinition != null)
						{
							this.viewGenerator = FormatViewManager.SelectViewGeneratorFromViewDefinition(errorContext, expressionFactory, db, viewDefinition, parameters);
							FormatViewManager.formatViewBindingTracer.WriteLine("An applicable view has been found", new object[0]);
							return;
						}
						FormatViewManager.formatViewBindingTracer.WriteLine("No applicable view has been found", new object[0]);
						FormatViewManager.ProcessUnknownViewName(errorContext, parameters.viewName, so, db, shape);
					}
					using (FormatViewManager.formatViewBindingTracer.TraceScope("FINDING VIEW {0} TYPE: {1}", new object[]
					{
						shape,
						FormatViewManager.PSObjectTypeName(so)
					}))
					{
						viewDefinition = DisplayDataQuery.GetViewByShapeAndType(expressionFactory, db, shape, internalTypeNames, null);
					}
					if (viewDefinition != null)
					{
						this.viewGenerator = FormatViewManager.SelectViewGeneratorFromViewDefinition(errorContext, expressionFactory, db, viewDefinition, parameters);
						FormatViewManager.formatViewBindingTracer.WriteLine("An applicable view has been found", new object[0]);
						FormatViewManager.PrepareViewForRemoteObjects(this.ViewGenerator, so);
					}
					else
					{
						FormatViewManager.formatViewBindingTracer.WriteLine("No applicable view has been found", new object[0]);
						this.viewGenerator = FormatViewManager.SelectViewGeneratorFromProperties(shape, so, errorContext, expressionFactory, db, parameters);
						FormatViewManager.PrepareViewForRemoteObjects(this.ViewGenerator, so);
					}
				}
			}
			finally
			{
				DisplayDataQuery.ResetTracer();
			}
		}

		// Token: 0x06003572 RID: 13682 RVA: 0x001231B4 File Offset: 0x001213B4
		private static void PrepareViewForRemoteObjects(ViewGenerator viewGenerator, PSObject so)
		{
			if (PSObjectHelper.ShouldShowComputerNameProperty(so))
			{
				viewGenerator.PrepareForRemoteObjects(so);
			}
		}

		// Token: 0x06003573 RID: 13683 RVA: 0x001231C8 File Offset: 0x001213C8
		private static void ProcessUnknownViewName(TerminatingErrorContext errorContext, string viewName, PSObject so, TypeInfoDataBase db, FormatShape formatShape)
		{
			string message = null;
			bool flag = false;
			string text = null;
			string text2 = ", ";
			StringBuilder stringBuilder = new StringBuilder();
			if (so != null && so.BaseObject != null && db != null && db.viewDefinitionsSection != null && db.viewDefinitionsSection.viewDefinitionList != null && db.viewDefinitionsSection.viewDefinitionList.Count > 0)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				string a = so.BaseObject.GetType().ToString();
				Type type = null;
				if (formatShape == FormatShape.Table)
				{
					type = typeof(TableControlBody);
					text = "Table";
				}
				else if (formatShape == FormatShape.List)
				{
					type = typeof(ListControlBody);
					text = "List";
				}
				else if (formatShape == FormatShape.Wide)
				{
					type = typeof(WideControlBody);
					text = "Wide";
				}
				else if (formatShape == FormatShape.Complex)
				{
					type = typeof(ComplexControlBody);
					text = "Custom";
				}
				if (type != null)
				{
					foreach (ViewDefinition viewDefinition in db.viewDefinitionsSection.viewDefinitionList)
					{
						if (viewDefinition.mainControl != null)
						{
							foreach (TypeOrGroupReference typeOrGroupReference in viewDefinition.appliesTo.referenceList)
							{
								if (!string.IsNullOrEmpty(typeOrGroupReference.name) && string.Equals(a, typeOrGroupReference.name, StringComparison.OrdinalIgnoreCase))
								{
									if (viewDefinition.mainControl.GetType() == type)
									{
										stringBuilder2.Append(viewDefinition.name);
										stringBuilder2.Append(text2);
									}
									else if (string.Equals(viewName, viewDefinition.name, StringComparison.OrdinalIgnoreCase))
									{
										string value = null;
										if (viewDefinition.mainControl.GetType() == typeof(TableControlBody))
										{
											value = "Format-Table";
										}
										else if (viewDefinition.mainControl.GetType() == typeof(ListControlBody))
										{
											value = "Format-List";
										}
										else if (viewDefinition.mainControl.GetType() == typeof(WideControlBody))
										{
											value = "Format-Wide";
										}
										else if (viewDefinition.mainControl.GetType() == typeof(ComplexControlBody))
										{
											value = "Format-Custom";
										}
										if (stringBuilder.Length == 0)
										{
											string value2 = StringUtil.Format(FormatAndOut_format_xxx.SuggestValidViewNamePrefix, new object[0]);
											stringBuilder.Append(value2);
										}
										else
										{
											stringBuilder.Append(", ");
										}
										stringBuilder.Append(value);
									}
								}
							}
						}
					}
				}
				if (stringBuilder2.Length > 0)
				{
					stringBuilder2.Remove(stringBuilder2.Length - text2.Length, text2.Length);
					message = StringUtil.Format(FormatAndOut_format_xxx.InvalidViewNameError, new object[]
					{
						viewName,
						text,
						stringBuilder2.ToString()
					});
					flag = true;
				}
			}
			if (!flag)
			{
				StringBuilder stringBuilder3 = new StringBuilder();
				if (stringBuilder.Length > 0)
				{
					stringBuilder3.Append(StringUtil.Format(FormatAndOut_format_xxx.UnknownViewNameErrorSuffix, viewName, text));
					stringBuilder3.Append(stringBuilder.ToString());
				}
				else
				{
					stringBuilder3.Append(StringUtil.Format(FormatAndOut_format_xxx.UnknownViewNameError, viewName));
					stringBuilder3.Append(StringUtil.Format(FormatAndOut_format_xxx.NonExistingViewNameError, text, so.BaseObject.GetType()));
				}
				message = stringBuilder3.ToString();
			}
			errorContext.ThrowTerminatingError(new ErrorRecord(new PipelineStoppedException(), "FormatViewNotFound", ErrorCategory.ObjectNotFound, viewName)
			{
				ErrorDetails = new ErrorDetails(message)
			});
		}

		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x06003574 RID: 13684 RVA: 0x001235A4 File Offset: 0x001217A4
		internal ViewGenerator ViewGenerator
		{
			get
			{
				return this.viewGenerator;
			}
		}

		// Token: 0x06003575 RID: 13685 RVA: 0x001235AC File Offset: 0x001217AC
		private static ViewGenerator SelectViewGeneratorFromViewDefinition(TerminatingErrorContext errorContext, MshExpressionFactory expressionFactory, TypeInfoDataBase db, ViewDefinition view, FormattingCommandLineParameters parameters)
		{
			ViewGenerator viewGenerator = null;
			if (view.mainControl is TableControlBody)
			{
				viewGenerator = new TableViewGenerator();
			}
			else if (view.mainControl is ListControlBody)
			{
				viewGenerator = new ListViewGenerator();
			}
			else if (view.mainControl is WideControlBody)
			{
				viewGenerator = new WideViewGenerator();
			}
			else if (view.mainControl is ComplexControlBody)
			{
				viewGenerator = new ComplexViewGenerator();
			}
			viewGenerator.Initialize(errorContext, expressionFactory, db, view, parameters);
			return viewGenerator;
		}

		// Token: 0x06003576 RID: 13686 RVA: 0x0012361C File Offset: 0x0012181C
		private static ViewGenerator SelectViewGeneratorFromProperties(FormatShape shape, PSObject so, TerminatingErrorContext errorContext, MshExpressionFactory expressionFactory, TypeInfoDataBase db, FormattingCommandLineParameters parameters)
		{
			if (shape == FormatShape.Undefined && parameters == null)
			{
				ConsolidatedString internalTypeNames = so.InternalTypeNames;
				shape = DisplayDataQuery.GetShapeFromType(expressionFactory, db, internalTypeNames);
				if (shape == FormatShape.Undefined)
				{
					List<MshExpression> defaultPropertySet = PSObjectHelper.GetDefaultPropertySet(so);
					if (defaultPropertySet.Count == 0)
					{
						foreach (MshResolvedExpressionParameterAssociation mshResolvedExpressionParameterAssociation in AssociationManager.ExpandAll(so))
						{
							defaultPropertySet.Add(mshResolvedExpressionParameterAssociation.ResolvedExpression);
						}
					}
					shape = DisplayDataQuery.GetShapeFromPropertyCount(db, defaultPropertySet.Count);
				}
			}
			ViewGenerator viewGenerator = null;
			if (shape == FormatShape.Table)
			{
				viewGenerator = new TableViewGenerator();
			}
			else if (shape == FormatShape.List)
			{
				viewGenerator = new ListViewGenerator();
			}
			else if (shape == FormatShape.Wide)
			{
				viewGenerator = new WideViewGenerator();
			}
			else if (shape == FormatShape.Complex)
			{
				viewGenerator = new ComplexViewGenerator();
			}
			viewGenerator.Initialize(errorContext, expressionFactory, so, db, parameters);
			return viewGenerator;
		}

		// Token: 0x04001B44 RID: 6980
		[TraceSource("FormatViewBinding", "Format view binding")]
		private static PSTraceSource formatViewBindingTracer = PSTraceSource.GetTracer("FormatViewBinding", "Format view binding", false);

		// Token: 0x04001B45 RID: 6981
		private ViewGenerator viewGenerator;
	}
}
