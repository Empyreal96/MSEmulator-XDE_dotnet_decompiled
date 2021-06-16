using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation.Language
{
	// Token: 0x02000537 RID: 1335
	public abstract class Ast
	{
		// Token: 0x06003779 RID: 14201 RVA: 0x0012A5AF File Offset: 0x001287AF
		protected Ast(IScriptExtent extent)
		{
			if (extent == null)
			{
				throw PSTraceSource.NewArgumentNullException("extent");
			}
			this.Extent = extent;
		}

		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x0600377A RID: 14202 RVA: 0x0012A5CC File Offset: 0x001287CC
		// (set) Token: 0x0600377B RID: 14203 RVA: 0x0012A5D4 File Offset: 0x001287D4
		public IScriptExtent Extent { get; private set; }

		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x0600377C RID: 14204 RVA: 0x0012A5DD File Offset: 0x001287DD
		// (set) Token: 0x0600377D RID: 14205 RVA: 0x0012A5E5 File Offset: 0x001287E5
		public Ast Parent { get; private set; }

		// Token: 0x0600377E RID: 14206 RVA: 0x0012A5EE File Offset: 0x001287EE
		public object Visit(ICustomAstVisitor astVisitor)
		{
			if (astVisitor == null)
			{
				throw PSTraceSource.NewArgumentNullException("astVisitor");
			}
			return this.Accept(astVisitor);
		}

		// Token: 0x0600377F RID: 14207 RVA: 0x0012A605 File Offset: 0x00128805
		public void Visit(AstVisitor astVisitor)
		{
			if (astVisitor == null)
			{
				throw PSTraceSource.NewArgumentNullException("astVisitor");
			}
			this.InternalVisit(astVisitor);
		}

		// Token: 0x06003780 RID: 14208 RVA: 0x0012A61D File Offset: 0x0012881D
		public IEnumerable<Ast> FindAll(Func<Ast, bool> predicate, bool searchNestedScriptBlocks)
		{
			if (predicate == null)
			{
				throw PSTraceSource.NewArgumentNullException("predicate");
			}
			return AstSearcher.FindAll(this, predicate, searchNestedScriptBlocks);
		}

		// Token: 0x06003781 RID: 14209 RVA: 0x0012A635 File Offset: 0x00128835
		public Ast Find(Func<Ast, bool> predicate, bool searchNestedScriptBlocks)
		{
			if (predicate == null)
			{
				throw PSTraceSource.NewArgumentNullException("predicate");
			}
			return AstSearcher.FindFirst(this, predicate, searchNestedScriptBlocks);
		}

		// Token: 0x06003782 RID: 14210 RVA: 0x0012A64D File Offset: 0x0012884D
		public override string ToString()
		{
			return this.Extent.Text;
		}

		// Token: 0x06003783 RID: 14211
		public abstract Ast Copy();

		// Token: 0x06003784 RID: 14212 RVA: 0x0012A65C File Offset: 0x0012885C
		public object SafeGetValue()
		{
			object safeValue;
			try
			{
				ExecutionContext context = null;
				if (Runspace.DefaultRunspace != null)
				{
					context = Runspace.DefaultRunspace.ExecutionContext;
				}
				safeValue = GetSafeValueVisitor.GetSafeValue(this, context, false);
			}
			catch
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, AutomationExceptions.CantConvertWithDynamicExpression, new object[]
				{
					this.Extent.Text
				}));
			}
			return safeValue;
		}

		// Token: 0x06003785 RID: 14213 RVA: 0x0012A6C8 File Offset: 0x001288C8
		internal static T[] CopyElements<T>(ReadOnlyCollection<T> elements) where T : Ast
		{
			if (elements == null || elements.Count == 0)
			{
				return null;
			}
			T[] array = new T[elements.Count];
			for (int i = 0; i < array.Count<T>(); i++)
			{
				T[] array2 = array;
				int num = i;
				T t = elements[i];
				array2[num] = (T)((object)t.Copy());
			}
			return array;
		}

		// Token: 0x06003786 RID: 14214 RVA: 0x0012A724 File Offset: 0x00128924
		internal static T CopyElement<T>(T element) where T : Ast
		{
			if (element == null)
			{
				return default(T);
			}
			return (T)((object)element.Copy());
		}

		// Token: 0x06003787 RID: 14215 RVA: 0x0012A758 File Offset: 0x00128958
		internal void SetParents<T>(ReadOnlyCollection<T> children) where T : Ast
		{
			for (int i = 0; i < children.Count; i++)
			{
				T t = children[i];
				this.SetParent(t);
			}
		}

		// Token: 0x06003788 RID: 14216 RVA: 0x0012A78C File Offset: 0x0012898C
		internal void SetParents<T1, T2>(ReadOnlyCollection<Tuple<T1, T2>> children) where T1 : Ast where T2 : Ast
		{
			for (int i = 0; i < children.Count; i++)
			{
				Tuple<T1, T2> tuple = children[i];
				this.SetParent(tuple.Item1);
				this.SetParent(tuple.Item2);
			}
		}

		// Token: 0x06003789 RID: 14217 RVA: 0x0012A7D4 File Offset: 0x001289D4
		internal void SetParent(Ast child)
		{
			if (child.Parent != null)
			{
				throw new InvalidOperationException(ParserStrings.AstIsReused);
			}
			child.Parent = this;
		}

		// Token: 0x0600378A RID: 14218 RVA: 0x0012A7F0 File Offset: 0x001289F0
		internal void ClearParent()
		{
			this.Parent = null;
		}

		// Token: 0x0600378B RID: 14219
		internal abstract object Accept(ICustomAstVisitor visitor);

		// Token: 0x0600378C RID: 14220
		internal abstract AstVisitAction InternalVisit(AstVisitor visitor);

		// Token: 0x0600378D RID: 14221
		internal abstract IEnumerable<PSTypeName> GetInferredType(CompletionContext context);

		// Token: 0x0600378E RID: 14222 RVA: 0x0012A7FC File Offset: 0x001289FC
		internal bool IsInWorkflow()
		{
			Ast ast = this;
			bool flag = false;
			while (ast != null && !flag)
			{
				ScriptBlockAst scriptBlockAst = ast as ScriptBlockAst;
				if (scriptBlockAst != null)
				{
					FunctionDefinitionAst functionDefinitionAst = scriptBlockAst.Parent as FunctionDefinitionAst;
					if (functionDefinitionAst != null)
					{
						flag = true;
						if (functionDefinitionAst.IsWorkflow)
						{
							return true;
						}
					}
				}
				CommandAst commandAst = ast as CommandAst;
				if (commandAst != null && string.Equals(TokenKind.InlineScript.Text(), commandAst.GetCommandName(), StringComparison.OrdinalIgnoreCase) && this != commandAst)
				{
					return false;
				}
				ast = ast.Parent;
			}
			return false;
		}

		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x0600378F RID: 14223 RVA: 0x0012A86E File Offset: 0x00128A6E
		// (set) Token: 0x06003790 RID: 14224 RVA: 0x0012A876 File Offset: 0x00128A76
		internal bool HasSuspiciousContent { get; set; }

		// Token: 0x06003791 RID: 14225 RVA: 0x0012A880 File Offset: 0x00128A80
		internal static ConfigurationDefinitionAst GetAncestorConfigurationDefinitionAstAndDynamicKeywordStatementAst(Ast ast, out DynamicKeywordStatementAst keywordAst)
		{
			keywordAst = Ast.GetAncestorAst<DynamicKeywordStatementAst>(ast);
			return (keywordAst != null) ? Ast.GetAncestorAst<ConfigurationDefinitionAst>(keywordAst) : Ast.GetAncestorAst<ConfigurationDefinitionAst>(ast);
		}

		// Token: 0x06003792 RID: 14226 RVA: 0x0012A8AC File Offset: 0x00128AAC
		internal static HashtableAst GetAncestorHashtableAst(Ast ast, out Ast lastChildOfHashtable)
		{
			HashtableAst hashtableAst = null;
			lastChildOfHashtable = null;
			while (ast != null)
			{
				hashtableAst = (ast as HashtableAst);
				if (hashtableAst != null)
				{
					break;
				}
				lastChildOfHashtable = ast;
				ast = ast.Parent;
			}
			return hashtableAst;
		}

		// Token: 0x06003793 RID: 14227 RVA: 0x0012A8DC File Offset: 0x00128ADC
		internal static TypeDefinitionAst GetAncestorTypeDefinitionAst(Ast ast)
		{
			TypeDefinitionAst typeDefinitionAst = null;
			while (ast != null)
			{
				typeDefinitionAst = (ast as TypeDefinitionAst);
				if (typeDefinitionAst != null)
				{
					break;
				}
				FunctionDefinitionAst functionDefinitionAst = ast as FunctionDefinitionAst;
				if (functionDefinitionAst != null && !(functionDefinitionAst.Parent is FunctionMemberAst))
				{
					break;
				}
				ast = ast.Parent;
			}
			return typeDefinitionAst;
		}

		// Token: 0x06003794 RID: 14228 RVA: 0x0012A91C File Offset: 0x00128B1C
		internal static T GetAncestorAst<T>(Ast ast) where T : Ast
		{
			T t = default(T);
			for (Ast ast2 = ast; ast2 != null; ast2 = ast2.Parent)
			{
				t = (ast2 as T);
				if (t != null)
				{
					break;
				}
			}
			return t;
		}

		// Token: 0x04001C67 RID: 7271
		internal static PSTypeName[] EmptyPSTypeNameArray = new PSTypeName[0];
	}
}
