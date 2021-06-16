using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.Management.Automation.Language
{
	// Token: 0x0200057B RID: 1403
	public class MemberExpressionAst : ExpressionAst, ISupportsAssignment
	{
		// Token: 0x06003A2A RID: 14890 RVA: 0x00132E7C File Offset: 0x0013107C
		public MemberExpressionAst(IScriptExtent extent, ExpressionAst expression, CommandElementAst member, bool @static) : base(extent)
		{
			if (expression == null || member == null)
			{
				throw PSTraceSource.NewArgumentNullException((expression == null) ? "expression" : "member");
			}
			this.Expression = expression;
			base.SetParent(expression);
			this.Member = member;
			base.SetParent(member);
			this.Static = @static;
		}

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x06003A2B RID: 14891 RVA: 0x00132ECF File Offset: 0x001310CF
		// (set) Token: 0x06003A2C RID: 14892 RVA: 0x00132ED7 File Offset: 0x001310D7
		public ExpressionAst Expression { get; private set; }

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x06003A2D RID: 14893 RVA: 0x00132EE0 File Offset: 0x001310E0
		// (set) Token: 0x06003A2E RID: 14894 RVA: 0x00132EE8 File Offset: 0x001310E8
		public CommandElementAst Member { get; private set; }

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x06003A2F RID: 14895 RVA: 0x00132EF1 File Offset: 0x001310F1
		// (set) Token: 0x06003A30 RID: 14896 RVA: 0x00132EF9 File Offset: 0x001310F9
		public bool Static { get; private set; }

		// Token: 0x06003A31 RID: 14897 RVA: 0x00132F04 File Offset: 0x00131104
		public override Ast Copy()
		{
			ExpressionAst expression = Ast.CopyElement<ExpressionAst>(this.Expression);
			CommandElementAst member = Ast.CopyElement<CommandElementAst>(this.Member);
			return new MemberExpressionAst(base.Extent, expression, member, this.Static);
		}

		// Token: 0x06003A32 RID: 14898 RVA: 0x001339D0 File Offset: 0x00131BD0
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			StringConstantExpressionAst memberAsStringConst = this.Member as StringConstantExpressionAst;
			if (memberAsStringConst != null)
			{
				PSTypeName[] exprType;
				if (this.Static)
				{
					TypeExpressionAst typeExpressionAst = this.Expression as TypeExpressionAst;
					if (typeExpressionAst == null)
					{
						goto IL_86E;
					}
					Type reflectionType = typeExpressionAst.TypeName.GetReflectionType();
					if (reflectionType == null)
					{
						TypeName typeName = typeExpressionAst.TypeName as TypeName;
						if (typeName == null || typeName._typeDefinitionAst == null)
						{
							goto IL_86E;
						}
						exprType = new PSTypeName[]
						{
							new PSTypeName(typeName._typeDefinitionAst)
						};
					}
					else
					{
						exprType = new PSTypeName[]
						{
							new PSTypeName(reflectionType)
						};
					}
				}
				else
				{
					exprType = this.Expression.GetInferredType(context).ToArray<PSTypeName>();
					if (exprType.Length == 0)
					{
						goto IL_86E;
					}
				}
				bool maybeWantDefaultCtor = this.Static && this is InvokeMemberExpressionAst && memberAsStringConst.Value.Equals("new", StringComparison.OrdinalIgnoreCase);
				List<string> memberNameList = new List<string>
				{
					memberAsStringConst.Value
				};
				foreach (PSTypeName type in exprType)
				{
					IEnumerable<object> members = CompletionCompleters.GetMembersByInferredType(type, context, this.Static, null);
					for (int i = 0; i < memberNameList.Count; i++)
					{
						string memberName = memberNameList[i];
						foreach (object member in members)
						{
							PropertyInfo propertyInfo = member as PropertyInfo;
							if (propertyInfo != null)
							{
								if (propertyInfo.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase) && !(this is InvokeMemberExpressionAst))
								{
									yield return new PSTypeName(propertyInfo.PropertyType);
									break;
								}
							}
							else
							{
								FieldInfo fieldInfo = member as FieldInfo;
								if (fieldInfo != null)
								{
									if (fieldInfo.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase) && !(this is InvokeMemberExpressionAst))
									{
										yield return new PSTypeName(fieldInfo.FieldType);
										break;
									}
								}
								else
								{
									DotNetAdapter.MethodCacheEntry methodCacheEntry = member as DotNetAdapter.MethodCacheEntry;
									if (methodCacheEntry != null)
									{
										if (methodCacheEntry[0].method.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase))
										{
											maybeWantDefaultCtor = false;
											if (this is InvokeMemberExpressionAst)
											{
												foreach (MethodInformation method in methodCacheEntry.methodInformationStructures)
												{
													MethodInfo methodInfo = method.method as MethodInfo;
													if (methodInfo != null && !methodInfo.ReturnType.GetTypeInfo().ContainsGenericParameters)
													{
														yield return new PSTypeName(methodInfo.ReturnType);
													}
												}
												break;
											}
											yield return new PSTypeName(typeof(PSMethod));
											break;
										}
									}
									else
									{
										MemberAst memberAst = member as MemberAst;
										if (memberAst != null)
										{
											if (memberAst.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase))
											{
												if (this is InvokeMemberExpressionAst)
												{
													FunctionMemberAst functionMemberAst = memberAst as FunctionMemberAst;
													if (functionMemberAst != null && !functionMemberAst.IsReturnTypeVoid())
													{
														yield return new PSTypeName(functionMemberAst.ReturnType.TypeName);
													}
												}
												else
												{
													PropertyMemberAst propertyMemberAst = memberAst as PropertyMemberAst;
													if (propertyMemberAst != null)
													{
														if (propertyMemberAst.PropertyType != null)
														{
															yield return new PSTypeName(propertyMemberAst.PropertyType.TypeName);
														}
														else
														{
															yield return new PSTypeName(typeof(object));
														}
													}
													else
													{
														yield return new PSTypeName(typeof(PSMethod));
													}
												}
											}
										}
										else
										{
											PSMemberInfo memberInfo = member as PSMemberInfo;
											if (memberInfo != null && memberInfo.Name.Equals(memberName, StringComparison.OrdinalIgnoreCase))
											{
												PSNoteProperty noteProperty = member as PSNoteProperty;
												if (noteProperty != null)
												{
													yield return new PSTypeName(noteProperty.Value.GetType());
													break;
												}
												PSAliasProperty aliasProperty = member as PSAliasProperty;
												if (aliasProperty != null)
												{
													memberNameList.Add(aliasProperty.ReferencedMemberName);
													break;
												}
												PSCodeProperty codeProperty = member as PSCodeProperty;
												if (codeProperty != null)
												{
													if (codeProperty.GetterCodeReference != null)
													{
														yield return new PSTypeName(codeProperty.GetterCodeReference.ReturnType);
														break;
													}
													break;
												}
												else
												{
													ScriptBlock scriptBlock = null;
													PSScriptProperty scriptProperty = member as PSScriptProperty;
													if (scriptProperty != null)
													{
														scriptBlock = scriptProperty.GetterScript;
													}
													PSScriptMethod scriptMethod = member as PSScriptMethod;
													if (scriptMethod != null)
													{
														scriptBlock = scriptMethod.Script;
													}
													if (scriptBlock != null)
													{
														foreach (PSTypeName t in scriptBlock.OutputType)
														{
															yield return t;
														}
														break;
													}
													break;
												}
											}
										}
									}
								}
							}
						}
					}
					if (maybeWantDefaultCtor)
					{
						yield return type;
					}
				}
			}
			IL_86E:
			yield break;
		}

		// Token: 0x06003A33 RID: 14899 RVA: 0x001339F4 File Offset: 0x00131BF4
		internal override object Accept(ICustomAstVisitor visitor)
		{
			return visitor.VisitMemberExpression(this);
		}

		// Token: 0x06003A34 RID: 14900 RVA: 0x00133A00 File Offset: 0x00131C00
		internal override AstVisitAction InternalVisit(AstVisitor visitor)
		{
			AstVisitAction astVisitAction = visitor.VisitMemberExpression(this);
			if (astVisitAction == AstVisitAction.SkipChildren)
			{
				return visitor.CheckForPostAction(this, AstVisitAction.Continue);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.Expression.InternalVisit(visitor);
			}
			if (astVisitAction == AstVisitAction.Continue)
			{
				astVisitAction = this.Member.InternalVisit(visitor);
			}
			return visitor.CheckForPostAction(this, astVisitAction);
		}

		// Token: 0x06003A35 RID: 14901 RVA: 0x00133A4C File Offset: 0x00131C4C
		IAssignableValue ISupportsAssignment.GetAssignableValue()
		{
			return new MemberAssignableValue
			{
				MemberExpression = this
			};
		}
	}
}
