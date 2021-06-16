using System;
using System.Collections.Generic;

namespace System.Management.Automation.Language
{
	// Token: 0x02000544 RID: 1348
	public abstract class AttributeBaseAst : Ast
	{
		// Token: 0x06003822 RID: 14370 RVA: 0x0012C772 File Offset: 0x0012A972
		protected AttributeBaseAst(IScriptExtent extent, ITypeName typeName) : base(extent)
		{
			if (typeName == null)
			{
				throw PSTraceSource.NewArgumentNullException("typeName");
			}
			this.TypeName = typeName;
		}

		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x06003823 RID: 14371 RVA: 0x0012C790 File Offset: 0x0012A990
		// (set) Token: 0x06003824 RID: 14372 RVA: 0x0012C798 File Offset: 0x0012A998
		public ITypeName TypeName { get; private set; }

		// Token: 0x06003825 RID: 14373
		internal abstract Attribute GetAttribute();

		// Token: 0x06003826 RID: 14374 RVA: 0x0012C7A1 File Offset: 0x0012A9A1
		internal override IEnumerable<PSTypeName> GetInferredType(CompletionContext context)
		{
			return Ast.EmptyPSTypeNameArray;
		}
	}
}
