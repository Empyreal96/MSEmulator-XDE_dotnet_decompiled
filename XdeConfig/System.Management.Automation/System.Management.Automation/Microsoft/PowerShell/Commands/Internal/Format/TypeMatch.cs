using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x0200096B RID: 2411
	internal sealed class TypeMatch
	{
		// Token: 0x170011DB RID: 4571
		// (get) Token: 0x0600585F RID: 22623 RVA: 0x001CBC7F File Offset: 0x001C9E7F
		private static PSTraceSource ActiveTracer
		{
			get
			{
				return TypeMatch.activeTracer ?? TypeMatch.classTracer;
			}
		}

		// Token: 0x06005860 RID: 22624 RVA: 0x001CBC8F File Offset: 0x001C9E8F
		internal static void SetTracer(PSTraceSource t)
		{
			TypeMatch.activeTracer = t;
		}

		// Token: 0x06005861 RID: 22625 RVA: 0x001CBC97 File Offset: 0x001C9E97
		internal static void ResetTracer()
		{
			TypeMatch.activeTracer = TypeMatch.classTracer;
		}

		// Token: 0x06005862 RID: 22626 RVA: 0x001CBCA3 File Offset: 0x001C9EA3
		internal TypeMatch(MshExpressionFactory expressionFactory, TypeInfoDataBase db, Collection<string> typeNames)
		{
			this._expressionFactory = expressionFactory;
			this._db = db;
			this._typeNameHierarchy = typeNames;
			this._useInheritance = true;
		}

		// Token: 0x06005863 RID: 22627 RVA: 0x001CBCD9 File Offset: 0x001C9ED9
		internal TypeMatch(MshExpressionFactory expressionFactory, TypeInfoDataBase db, Collection<string> typeNames, bool useInheritance)
		{
			this._expressionFactory = expressionFactory;
			this._db = db;
			this._typeNameHierarchy = typeNames;
			this._useInheritance = useInheritance;
		}

		// Token: 0x06005864 RID: 22628 RVA: 0x001CBD10 File Offset: 0x001C9F10
		internal bool PerfectMatch(TypeMatchItem item)
		{
			int num = this.ComputeBestMatch(item.AppliesTo, item.CurrentObject);
			if (num == -1)
			{
				return false;
			}
			if (this._bestMatchIndex == -1 || num < this._bestMatchIndex)
			{
				this._bestMatchIndex = num;
				this._bestMatchItem = item;
			}
			return this._bestMatchIndex == 0;
		}

		// Token: 0x170011DC RID: 4572
		// (get) Token: 0x06005865 RID: 22629 RVA: 0x001CBD5F File Offset: 0x001C9F5F
		internal object BestMatch
		{
			get
			{
				if (this._bestMatchItem == null)
				{
					return null;
				}
				return this._bestMatchItem.Item;
			}
		}

		// Token: 0x06005866 RID: 22630 RVA: 0x001CBD78 File Offset: 0x001C9F78
		private int ComputeBestMatch(AppliesTo appliesTo, PSObject currentObject)
		{
			int num = -1;
			foreach (TypeOrGroupReference typeOrGroupReference in appliesTo.referenceList)
			{
				MshExpression ex = null;
				if (typeOrGroupReference.conditionToken != null)
				{
					ex = this._expressionFactory.CreateFromExpressionToken(typeOrGroupReference.conditionToken);
				}
				int num2 = -1;
				TypeReference typeReference = typeOrGroupReference as TypeReference;
				if (typeReference != null)
				{
					num2 = this.MatchTypeIndex(typeReference.name, currentObject, ex);
				}
				else
				{
					TypeGroupReference typeGroupReference = typeOrGroupReference as TypeGroupReference;
					TypeGroupDefinition typeGroupDefinition = DisplayDataQuery.FindGroupDefinition(this._db, typeGroupReference.name);
					if (typeGroupDefinition != null)
					{
						num2 = this.ComputeBestMatchInGroup(typeGroupDefinition, currentObject, ex);
					}
				}
				if (num2 == 0)
				{
					return num2;
				}
				if (num == -1 || num < num2)
				{
					num = num2;
				}
			}
			return num;
		}

		// Token: 0x06005867 RID: 22631 RVA: 0x001CBE4C File Offset: 0x001CA04C
		private int ComputeBestMatchInGroup(TypeGroupDefinition tgd, PSObject currentObject, MshExpression ex)
		{
			int num = -1;
			int num2 = 0;
			foreach (TypeReference typeReference in tgd.typeReferenceList)
			{
				int num3 = this.MatchTypeIndex(typeReference.name, currentObject, ex);
				if (num3 == 0)
				{
					return num3;
				}
				if (num == -1 || num < num3)
				{
					num = num3;
				}
				num2++;
			}
			return num;
		}

		// Token: 0x06005868 RID: 22632 RVA: 0x001CBEC8 File Offset: 0x001CA0C8
		private int MatchTypeIndex(string typeName, PSObject currentObject, MshExpression ex)
		{
			if (string.IsNullOrEmpty(typeName))
			{
				return -1;
			}
			int num = 0;
			foreach (string a in this._typeNameHierarchy)
			{
				if (string.Equals(a, typeName, StringComparison.OrdinalIgnoreCase) && this.MatchCondition(currentObject, ex))
				{
					return num;
				}
				if (num == 0 && !this._useInheritance)
				{
					break;
				}
				num++;
			}
			return -1;
		}

		// Token: 0x06005869 RID: 22633 RVA: 0x001CBF44 File Offset: 0x001CA144
		private bool MatchCondition(PSObject currentObject, MshExpression ex)
		{
			if (ex == null)
			{
				return true;
			}
			MshExpressionResult mshExpressionResult;
			bool result = DisplayCondition.Evaluate(currentObject, ex, out mshExpressionResult);
			if (mshExpressionResult != null && mshExpressionResult.Exception != null)
			{
				this._failedResultsList.Add(mshExpressionResult);
			}
			return result;
		}

		// Token: 0x04002F61 RID: 12129
		private const int BestMatchIndexUndefined = -1;

		// Token: 0x04002F62 RID: 12130
		private const int BestMatchIndexPerfect = 0;

		// Token: 0x04002F63 RID: 12131
		[TraceSource("TypeMatch", "F&O TypeMatch")]
		private static readonly PSTraceSource classTracer = PSTraceSource.GetTracer("TypeMatch", "F&O TypeMatch");

		// Token: 0x04002F64 RID: 12132
		private static PSTraceSource activeTracer = null;

		// Token: 0x04002F65 RID: 12133
		private MshExpressionFactory _expressionFactory;

		// Token: 0x04002F66 RID: 12134
		private TypeInfoDataBase _db;

		// Token: 0x04002F67 RID: 12135
		private Collection<string> _typeNameHierarchy;

		// Token: 0x04002F68 RID: 12136
		private bool _useInheritance;

		// Token: 0x04002F69 RID: 12137
		private List<MshExpressionResult> _failedResultsList = new List<MshExpressionResult>();

		// Token: 0x04002F6A RID: 12138
		private int _bestMatchIndex = -1;

		// Token: 0x04002F6B RID: 12139
		private TypeMatchItem _bestMatchItem;
	}
}
