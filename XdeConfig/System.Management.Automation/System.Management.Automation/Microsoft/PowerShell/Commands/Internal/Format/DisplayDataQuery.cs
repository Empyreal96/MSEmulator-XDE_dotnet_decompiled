using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation;
using System.Text;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x0200096C RID: 2412
	internal static class DisplayDataQuery
	{
		// Token: 0x170011DD RID: 4573
		// (get) Token: 0x0600586B RID: 22635 RVA: 0x001CBF94 File Offset: 0x001CA194
		private static PSTraceSource ActiveTracer
		{
			get
			{
				return DisplayDataQuery.activeTracer ?? DisplayDataQuery.classTracer;
			}
		}

		// Token: 0x0600586C RID: 22636 RVA: 0x001CBFA4 File Offset: 0x001CA1A4
		internal static void SetTracer(PSTraceSource t)
		{
			DisplayDataQuery.activeTracer = t;
		}

		// Token: 0x0600586D RID: 22637 RVA: 0x001CBFAC File Offset: 0x001CA1AC
		internal static void ResetTracer()
		{
			DisplayDataQuery.activeTracer = DisplayDataQuery.classTracer;
		}

		// Token: 0x0600586E RID: 22638 RVA: 0x001CBFB8 File Offset: 0x001CA1B8
		internal static EnumerableExpansion GetEnumerableExpansionFromType(MshExpressionFactory expressionFactory, TypeInfoDataBase db, Collection<string> typeNames)
		{
			TypeMatch typeMatch = new TypeMatch(expressionFactory, db, typeNames);
			foreach (EnumerableExpansionDirective enumerableExpansionDirective in db.defaultSettingsSection.enumerableExpansionDirectiveList)
			{
				if (typeMatch.PerfectMatch(new TypeMatchItem(enumerableExpansionDirective, enumerableExpansionDirective.appliesTo)))
				{
					return enumerableExpansionDirective.enumerableExpansion;
				}
			}
			if (typeMatch.BestMatch != null)
			{
				return ((EnumerableExpansionDirective)typeMatch.BestMatch).enumerableExpansion;
			}
			Collection<string> collection = Deserializer.MaskDeserializationPrefix(typeNames);
			if (collection != null)
			{
				return DisplayDataQuery.GetEnumerableExpansionFromType(expressionFactory, db, collection);
			}
			return EnumerableExpansion.EnumOnly;
		}

		// Token: 0x0600586F RID: 22639 RVA: 0x001CC064 File Offset: 0x001CA264
		internal static FormatShape GetShapeFromType(MshExpressionFactory expressionFactory, TypeInfoDataBase db, Collection<string> typeNames)
		{
			ShapeSelectionDirectives shapeSelectionDirectives = db.defaultSettingsSection.shapeSelectionDirectives;
			TypeMatch typeMatch = new TypeMatch(expressionFactory, db, typeNames);
			foreach (FormatShapeSelectionOnType formatShapeSelectionOnType in shapeSelectionDirectives.formatShapeSelectionOnTypeList)
			{
				if (typeMatch.PerfectMatch(new TypeMatchItem(formatShapeSelectionOnType, formatShapeSelectionOnType.appliesTo)))
				{
					return formatShapeSelectionOnType.formatShape;
				}
			}
			if (typeMatch.BestMatch != null)
			{
				return ((FormatShapeSelectionOnType)typeMatch.BestMatch).formatShape;
			}
			Collection<string> collection = Deserializer.MaskDeserializationPrefix(typeNames);
			if (collection != null)
			{
				return DisplayDataQuery.GetShapeFromType(expressionFactory, db, collection);
			}
			return FormatShape.Undefined;
		}

		// Token: 0x06005870 RID: 22640 RVA: 0x001CC118 File Offset: 0x001CA318
		internal static FormatShape GetShapeFromPropertyCount(TypeInfoDataBase db, int propertyCount)
		{
			if (propertyCount <= db.defaultSettingsSection.shapeSelectionDirectives.PropertyCountForTable)
			{
				return FormatShape.Table;
			}
			return FormatShape.List;
		}

		// Token: 0x06005871 RID: 22641 RVA: 0x001CC130 File Offset: 0x001CA330
		internal static ViewDefinition GetViewByShapeAndType(MshExpressionFactory expressionFactory, TypeInfoDataBase db, FormatShape shape, Collection<string> typeNames, string viewName)
		{
			if (shape == FormatShape.Undefined)
			{
				return DisplayDataQuery.GetDefaultView(expressionFactory, db, typeNames);
			}
			Type typeFromHandle;
			if (shape == FormatShape.Table)
			{
				typeFromHandle = typeof(TableControlBody);
			}
			else if (shape == FormatShape.List)
			{
				typeFromHandle = typeof(ListControlBody);
			}
			else if (shape == FormatShape.Wide)
			{
				typeFromHandle = typeof(WideControlBody);
			}
			else
			{
				if (shape != FormatShape.Complex)
				{
					return null;
				}
				typeFromHandle = typeof(ComplexControlBody);
			}
			return DisplayDataQuery.GetView(expressionFactory, db, typeFromHandle, typeNames, viewName);
		}

		// Token: 0x06005872 RID: 22642 RVA: 0x001CC19C File Offset: 0x001CA39C
		internal static ViewDefinition GetOutOfBandView(MshExpressionFactory expressionFactory, TypeInfoDataBase db, Collection<string> typeNames)
		{
			TypeMatch typeMatch = new TypeMatch(expressionFactory, db, typeNames);
			foreach (ViewDefinition viewDefinition in db.viewDefinitionsSection.viewDefinitionList)
			{
				if (DisplayDataQuery.IsOutOfBandView(viewDefinition) && typeMatch.PerfectMatch(new TypeMatchItem(viewDefinition, viewDefinition.appliesTo)))
				{
					return viewDefinition;
				}
			}
			ViewDefinition viewDefinition2 = typeMatch.BestMatch as ViewDefinition;
			if (viewDefinition2 == null)
			{
				Collection<string> collection = Deserializer.MaskDeserializationPrefix(typeNames);
				if (collection != null)
				{
					viewDefinition2 = DisplayDataQuery.GetOutOfBandView(expressionFactory, db, collection);
				}
			}
			return viewDefinition2;
		}

		// Token: 0x06005873 RID: 22643 RVA: 0x001CC240 File Offset: 0x001CA440
		private static ViewDefinition GetView(MshExpressionFactory expressionFactory, TypeInfoDataBase db, Type mainControlType, Collection<string> typeNames, string viewName)
		{
			TypeMatch typeMatch = new TypeMatch(expressionFactory, db, typeNames);
			foreach (ViewDefinition viewDefinition in db.viewDefinitionsSection.viewDefinitionList)
			{
				if (viewDefinition == null || mainControlType != viewDefinition.mainControl.GetType())
				{
					DisplayDataQuery.ActiveTracer.WriteLine("NOT MATCH {0}  NAME: {1}", new object[]
					{
						ControlBase.GetControlShapeName(viewDefinition.mainControl),
						(viewDefinition != null) ? viewDefinition.name : string.Empty
					});
				}
				else if (DisplayDataQuery.IsOutOfBandView(viewDefinition))
				{
					DisplayDataQuery.ActiveTracer.WriteLine("NOT MATCH OutOfBand {0}  NAME: {1}", new object[]
					{
						ControlBase.GetControlShapeName(viewDefinition.mainControl),
						viewDefinition.name
					});
				}
				else if (viewDefinition.appliesTo == null)
				{
					DisplayDataQuery.ActiveTracer.WriteLine("NOT MATCH {0}  NAME: {1}  No applicable types", new object[]
					{
						ControlBase.GetControlShapeName(viewDefinition.mainControl),
						viewDefinition.name
					});
				}
				else if (viewName != null && !string.Equals(viewDefinition.name, viewName, StringComparison.OrdinalIgnoreCase))
				{
					DisplayDataQuery.ActiveTracer.WriteLine("NOT MATCH {0}  NAME: {1}", new object[]
					{
						ControlBase.GetControlShapeName(viewDefinition.mainControl),
						viewDefinition.name
					});
				}
				else
				{
					try
					{
						TypeMatch.SetTracer(DisplayDataQuery.ActiveTracer);
						if (typeMatch.PerfectMatch(new TypeMatchItem(viewDefinition, viewDefinition.appliesTo)))
						{
							DisplayDataQuery.TraceHelper(viewDefinition, true);
							return viewDefinition;
						}
					}
					finally
					{
						TypeMatch.ResetTracer();
					}
					DisplayDataQuery.TraceHelper(viewDefinition, false);
				}
			}
			ViewDefinition viewDefinition2 = DisplayDataQuery.GetBestMatch(typeMatch);
			if (viewDefinition2 == null)
			{
				Collection<string> collection = Deserializer.MaskDeserializationPrefix(typeNames);
				if (collection != null)
				{
					viewDefinition2 = DisplayDataQuery.GetView(expressionFactory, db, mainControlType, collection, viewName);
				}
			}
			return viewDefinition2;
		}

		// Token: 0x06005874 RID: 22644 RVA: 0x001CC440 File Offset: 0x001CA640
		private static void TraceHelper(ViewDefinition vd, bool isMatched)
		{
			if ((DisplayDataQuery.ActiveTracer.Options & PSTraceSourceOptions.WriteLine) != PSTraceSourceOptions.None)
			{
				foreach (TypeOrGroupReference typeOrGroupReference in vd.appliesTo.referenceList)
				{
					StringBuilder stringBuilder = new StringBuilder();
					TypeReference typeReference = typeOrGroupReference as TypeReference;
					stringBuilder.Append(isMatched ? "MATCH FOUND" : "NOT MATCH");
					if (typeReference != null)
					{
						stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " {0} NAME: {1}  TYPE: {2}", new object[]
						{
							ControlBase.GetControlShapeName(vd.mainControl),
							vd.name,
							typeReference.name
						});
					}
					else
					{
						TypeGroupReference typeGroupReference = typeOrGroupReference as TypeGroupReference;
						stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " {0} NAME: {1}  GROUP: {2}", new object[]
						{
							ControlBase.GetControlShapeName(vd.mainControl),
							vd.name,
							typeGroupReference.name
						});
					}
					DisplayDataQuery.ActiveTracer.WriteLine(stringBuilder.ToString(), new object[0]);
				}
			}
		}

		// Token: 0x06005875 RID: 22645 RVA: 0x001CC570 File Offset: 0x001CA770
		private static ViewDefinition GetBestMatch(TypeMatch match)
		{
			ViewDefinition viewDefinition = match.BestMatch as ViewDefinition;
			if (viewDefinition != null)
			{
				DisplayDataQuery.TraceHelper(viewDefinition, true);
			}
			return viewDefinition;
		}

		// Token: 0x06005876 RID: 22646 RVA: 0x001CC594 File Offset: 0x001CA794
		private static ViewDefinition GetDefaultView(MshExpressionFactory expressionFactory, TypeInfoDataBase db, Collection<string> typeNames)
		{
			TypeMatch typeMatch = new TypeMatch(expressionFactory, db, typeNames);
			foreach (ViewDefinition viewDefinition in db.viewDefinitionsSection.viewDefinitionList)
			{
				if (viewDefinition != null)
				{
					if (DisplayDataQuery.IsOutOfBandView(viewDefinition))
					{
						DisplayDataQuery.ActiveTracer.WriteLine("NOT MATCH OutOfBand {0}  NAME: {1}", new object[]
						{
							ControlBase.GetControlShapeName(viewDefinition.mainControl),
							viewDefinition.name
						});
					}
					else if (viewDefinition.appliesTo == null)
					{
						DisplayDataQuery.ActiveTracer.WriteLine("NOT MATCH {0}  NAME: {1}  No applicable types", new object[]
						{
							ControlBase.GetControlShapeName(viewDefinition.mainControl),
							viewDefinition.name
						});
					}
					else
					{
						try
						{
							TypeMatch.SetTracer(DisplayDataQuery.ActiveTracer);
							if (typeMatch.PerfectMatch(new TypeMatchItem(viewDefinition, viewDefinition.appliesTo)))
							{
								DisplayDataQuery.TraceHelper(viewDefinition, true);
								return viewDefinition;
							}
						}
						finally
						{
							TypeMatch.ResetTracer();
						}
						DisplayDataQuery.TraceHelper(viewDefinition, false);
					}
				}
			}
			ViewDefinition viewDefinition2 = DisplayDataQuery.GetBestMatch(typeMatch);
			if (viewDefinition2 == null)
			{
				Collection<string> collection = Deserializer.MaskDeserializationPrefix(typeNames);
				if (collection != null)
				{
					viewDefinition2 = DisplayDataQuery.GetDefaultView(expressionFactory, db, collection);
				}
			}
			return viewDefinition2;
		}

		// Token: 0x06005877 RID: 22647 RVA: 0x001CC6DC File Offset: 0x001CA8DC
		private static bool IsOutOfBandView(ViewDefinition vd)
		{
			return (vd.mainControl is ComplexControlBody || vd.mainControl is ListControlBody) && vd.outOfBand;
		}

		// Token: 0x06005878 RID: 22648 RVA: 0x001CC700 File Offset: 0x001CA900
		internal static AppliesTo GetAllApplicableTypes(TypeInfoDataBase db, AppliesTo appliesTo)
		{
			Hashtable hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
			foreach (TypeOrGroupReference typeOrGroupReference in appliesTo.referenceList)
			{
				TypeReference typeReference = typeOrGroupReference as TypeReference;
				if (typeReference != null)
				{
					if (!hashtable.ContainsKey(typeReference.name))
					{
						hashtable.Add(typeReference.name, null);
					}
				}
				else
				{
					TypeGroupReference typeGroupReference = typeOrGroupReference as TypeGroupReference;
					if (typeGroupReference != null)
					{
						TypeGroupDefinition typeGroupDefinition = DisplayDataQuery.FindGroupDefinition(db, typeGroupReference.name);
						if (typeGroupDefinition != null)
						{
							foreach (TypeReference typeReference2 in typeGroupDefinition.typeReferenceList)
							{
								if (!hashtable.ContainsKey(typeReference2.name))
								{
									hashtable.Add(typeReference2.name, null);
								}
							}
						}
					}
				}
			}
			AppliesTo appliesTo2 = new AppliesTo();
			foreach (object obj in hashtable)
			{
				appliesTo2.AddAppliesToType(((DictionaryEntry)obj).Key as string);
			}
			return appliesTo2;
		}

		// Token: 0x06005879 RID: 22649 RVA: 0x001CC85C File Offset: 0x001CAA5C
		internal static TypeGroupDefinition FindGroupDefinition(TypeInfoDataBase db, string groupName)
		{
			foreach (TypeGroupDefinition typeGroupDefinition in db.typeGroupSection.typeGroupDefinitionList)
			{
				if (string.Equals(typeGroupDefinition.name, groupName, StringComparison.OrdinalIgnoreCase))
				{
					return typeGroupDefinition;
				}
			}
			return null;
		}

		// Token: 0x0600587A RID: 22650 RVA: 0x001CC8C4 File Offset: 0x001CAAC4
		internal static ControlBody ResolveControlReference(TypeInfoDataBase db, List<ControlDefinition> viewControlDefinitionList, ControlReference controlReference)
		{
			ControlBody controlBody = DisplayDataQuery.ResolveControlReferenceInList(controlReference, viewControlDefinitionList);
			if (controlBody != null)
			{
				return controlBody;
			}
			return DisplayDataQuery.ResolveControlReferenceInList(controlReference, db.formatControlDefinitionHolder.controlDefinitionList);
		}

		// Token: 0x0600587B RID: 22651 RVA: 0x001CC8F0 File Offset: 0x001CAAF0
		private static ControlBody ResolveControlReferenceInList(ControlReference controlReference, List<ControlDefinition> controlDefinitionList)
		{
			foreach (ControlDefinition controlDefinition in controlDefinitionList)
			{
				if (!(controlDefinition.controlBody.GetType() != controlReference.controlType) && string.Compare(controlReference.name, controlDefinition.name, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return controlDefinition.controlBody;
				}
			}
			return null;
		}

		// Token: 0x04002F6C RID: 12140
		[TraceSource("DisplayDataQuery", "DisplayDataQuery")]
		private static readonly PSTraceSource classTracer = PSTraceSource.GetTracer("DisplayDataQuery", "DisplayDataQuery");

		// Token: 0x04002F6D RID: 12141
		private static PSTraceSource activeTracer = null;
	}
}
