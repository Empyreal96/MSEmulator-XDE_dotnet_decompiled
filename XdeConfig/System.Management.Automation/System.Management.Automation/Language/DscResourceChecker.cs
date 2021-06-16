using System;

namespace System.Management.Automation.Language
{
	// Token: 0x020005C3 RID: 1475
	internal static class DscResourceChecker
	{
		// Token: 0x06003F52 RID: 16210 RVA: 0x0014EEC4 File Offset: 0x0014D0C4
		internal static void CheckType(Parser parser, TypeDefinitionAst typeDefinitionAst, AttributeAst dscResourceAttributeAst)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			foreach (MemberAst memberAst in typeDefinitionAst.Members)
			{
				FunctionMemberAst functionMemberAst = memberAst as FunctionMemberAst;
				if (functionMemberAst != null)
				{
					DscResourceChecker.CheckSet(functionMemberAst, ref flag);
					DscResourceChecker.CheckGet(parser, functionMemberAst, ref flag3);
					DscResourceChecker.CheckTest(functionMemberAst, ref flag2);
					if (functionMemberAst.IsConstructor && !functionMemberAst.IsStatic)
					{
						if (functionMemberAst.Parameters.Count == 0)
						{
							flag4 = true;
						}
						else
						{
							flag5 = true;
						}
					}
				}
				else
				{
					PropertyMemberAst propertyMemberAst = (PropertyMemberAst)memberAst;
					DscResourceChecker.CheckKey(parser, propertyMemberAst, ref flag6);
				}
			}
			if (typeDefinitionAst.BaseTypes != null && (!flag || !flag3 || !flag2 || !flag6))
			{
				DscResourceChecker.LookupRequiredMembers(parser, typeDefinitionAst, ref flag, ref flag3, ref flag2, ref flag6);
			}
			string name = typeDefinitionAst.Name;
			if (!flag)
			{
				parser.ReportError(dscResourceAttributeAst.Extent, () => ParserStrings.DscResourceMissingSetMethod, name);
			}
			if (!flag3)
			{
				parser.ReportError(dscResourceAttributeAst.Extent, () => ParserStrings.DscResourceMissingGetMethod, name);
			}
			if (!flag2)
			{
				parser.ReportError(dscResourceAttributeAst.Extent, () => ParserStrings.DscResourceMissingTestMethod, name);
			}
			if (!flag4 && flag5)
			{
				parser.ReportError(dscResourceAttributeAst.Extent, () => ParserStrings.DscResourceMissingDefaultConstructor, name);
			}
			if (!flag6)
			{
				parser.ReportError(dscResourceAttributeAst.Extent, () => ParserStrings.DscResourceMissingKeyProperty, name);
			}
		}

		// Token: 0x06003F53 RID: 16211 RVA: 0x0014F0AC File Offset: 0x0014D2AC
		private static void LookupRequiredMembers(Parser parser, TypeDefinitionAst typeDefinitionAst, ref bool hasSet, ref bool hasGet, ref bool hasTest, ref bool hasKey)
		{
			if (typeDefinitionAst == null)
			{
				return;
			}
			if (hasSet && hasGet && hasTest && hasKey)
			{
				return;
			}
			foreach (TypeConstraintAst typeConstraintAst in typeDefinitionAst.BaseTypes)
			{
				TypeName typeName = typeConstraintAst.TypeName as TypeName;
				if (typeName != null)
				{
					TypeDefinitionAst typeDefinitionAst2 = typeName._typeDefinitionAst;
					if (typeDefinitionAst2 != null && typeDefinitionAst2.IsClass)
					{
						foreach (MemberAst memberAst in typeDefinitionAst2.Members)
						{
							FunctionMemberAst functionMemberAst = memberAst as FunctionMemberAst;
							if (functionMemberAst != null)
							{
								DscResourceChecker.CheckSet(functionMemberAst, ref hasSet);
								DscResourceChecker.CheckGet(parser, functionMemberAst, ref hasGet);
								DscResourceChecker.CheckTest(functionMemberAst, ref hasTest);
							}
							else
							{
								PropertyMemberAst propertyMemberAst = (PropertyMemberAst)memberAst;
								DscResourceChecker.CheckKey(parser, propertyMemberAst, ref hasKey);
							}
						}
						if (typeDefinitionAst2.BaseTypes != null && (!hasSet || !hasGet || !hasTest || !hasKey))
						{
							DscResourceChecker.LookupRequiredMembers(parser, typeDefinitionAst2, ref hasSet, ref hasGet, ref hasTest, ref hasKey);
						}
					}
				}
			}
		}

		// Token: 0x06003F54 RID: 16212 RVA: 0x0014F1DC File Offset: 0x0014D3DC
		private static void CheckGet(Parser parser, FunctionMemberAst functionMemberAst, ref bool hasGet)
		{
			if (hasGet)
			{
				return;
			}
			if (functionMemberAst.Name.Equals("Get", StringComparison.OrdinalIgnoreCase) && functionMemberAst.Parameters.Count == 0)
			{
				if (functionMemberAst.ReturnType != null)
				{
					ArrayTypeName arrayTypeName = functionMemberAst.ReturnType.TypeName as ArrayTypeName;
					TypeName typeName = ((arrayTypeName != null) ? arrayTypeName.ElementType : functionMemberAst.ReturnType.TypeName) as TypeName;
					if (typeName == null || typeName._typeDefinitionAst != functionMemberAst.Parent)
					{
						parser.ReportError(functionMemberAst.Extent, () => ParserStrings.DscResourceInvalidGetMethod, ((TypeDefinitionAst)functionMemberAst.Parent).Name);
					}
				}
				else
				{
					parser.ReportError(functionMemberAst.Extent, () => ParserStrings.DscResourceInvalidGetMethod, ((TypeDefinitionAst)functionMemberAst.Parent).Name);
				}
				hasGet = true;
			}
		}

		// Token: 0x06003F55 RID: 16213 RVA: 0x0014F2D8 File Offset: 0x0014D4D8
		private static void CheckTest(FunctionMemberAst functionMemberAst, ref bool hasTest)
		{
			if (hasTest)
			{
				return;
			}
			hasTest = (functionMemberAst.Name.Equals("Test", StringComparison.OrdinalIgnoreCase) && functionMemberAst.Parameters.Count == 0 && functionMemberAst.ReturnType != null && functionMemberAst.ReturnType.TypeName.GetReflectionType() == typeof(bool));
		}

		// Token: 0x06003F56 RID: 16214 RVA: 0x0014F336 File Offset: 0x0014D536
		private static void CheckSet(FunctionMemberAst functionMemberAst, ref bool hasSet)
		{
			if (hasSet)
			{
				return;
			}
			hasSet = (functionMemberAst.Name.Equals("Set", StringComparison.OrdinalIgnoreCase) && functionMemberAst.Parameters.Count == 0 && functionMemberAst.IsReturnTypeVoid());
		}

		// Token: 0x06003F57 RID: 16215 RVA: 0x0014F368 File Offset: 0x0014D568
		private static void CheckKey(Parser parser, PropertyMemberAst propertyMemberAst, ref bool hasKey)
		{
			foreach (AttributeAst attributeAst in propertyMemberAst.Attributes)
			{
				if (attributeAst.TypeName.GetReflectionAttributeType() == typeof(DscPropertyAttribute))
				{
					foreach (NamedAttributeArgumentAst namedAttributeArgumentAst in attributeAst.NamedArguments)
					{
						object obj;
						if (namedAttributeArgumentAst.ArgumentName.Equals("Key", StringComparison.OrdinalIgnoreCase) && IsConstantValueVisitor.IsConstant(namedAttributeArgumentAst.Argument, out obj, true, false) && LanguagePrimitives.IsTrue(obj))
						{
							hasKey = true;
							bool flag = false;
							TypeConstraintAst propertyType = propertyMemberAst.PropertyType;
							if (propertyType != null)
							{
								TypeName typeName = propertyType.TypeName as TypeName;
								if (typeName != null)
								{
									Type reflectionType = typeName.GetReflectionType();
									if (reflectionType != null)
									{
										flag = (reflectionType == typeof(string) || reflectionType.IsInteger());
									}
									else
									{
										TypeDefinitionAst typeDefinitionAst = typeName._typeDefinitionAst;
										if (typeDefinitionAst != null)
										{
											flag = typeDefinitionAst.IsEnum;
										}
									}
								}
							}
							if (!flag)
							{
								parser.ReportError(propertyMemberAst.Extent, () => ParserStrings.DscResourceInvalidKeyProperty);
							}
							return;
						}
					}
				}
			}
		}
	}
}
