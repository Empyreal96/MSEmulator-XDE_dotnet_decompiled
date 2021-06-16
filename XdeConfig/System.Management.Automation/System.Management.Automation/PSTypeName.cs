using System;
using System.Management.Automation.Language;

namespace System.Management.Automation
{
	// Token: 0x02000058 RID: 88
	public class PSTypeName
	{
		// Token: 0x060004D8 RID: 1240 RVA: 0x000168B4 File Offset: 0x00014AB4
		public PSTypeName(Type type)
		{
			this._type = type;
			if (this._type != null)
			{
				this._name = this._type.FullName;
			}
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x000168E2 File Offset: 0x00014AE2
		public PSTypeName(string name)
		{
			this._name = name;
			this._type = null;
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x000168F8 File Offset: 0x00014AF8
		public PSTypeName(TypeDefinitionAst typeDefinitionAst)
		{
			if (typeDefinitionAst == null)
			{
				throw PSTraceSource.NewArgumentNullException("typeDefinitionAst");
			}
			this.TypeDefinitionAst = typeDefinitionAst;
			this._name = typeDefinitionAst.Name;
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00016924 File Offset: 0x00014B24
		public PSTypeName(ITypeName typeName)
		{
			if (typeName == null)
			{
				throw PSTraceSource.NewArgumentNullException("typeName");
			}
			this._type = typeName.GetReflectionType();
			if (this._type != null)
			{
				this._name = this._type.FullName;
				return;
			}
			TypeName typeName2 = typeName as TypeName;
			if (typeName2 != null && typeName2._typeDefinitionAst != null)
			{
				this.TypeDefinitionAst = typeName2._typeDefinitionAst;
				this._name = this.TypeDefinitionAst.Name;
				return;
			}
			this._type = null;
			this._name = typeName.FullName;
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x000169B4 File Offset: 0x00014BB4
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x000169BC File Offset: 0x00014BBC
		public Type Type
		{
			get
			{
				if (!this._typeWasCalculated)
				{
					if (this._type == null)
					{
						if (this.TypeDefinitionAst != null)
						{
							this._type = this.TypeDefinitionAst.Type;
						}
						else
						{
							TypeResolver.TryResolveType(this._name, out this._type);
						}
					}
					if (this._type == null && this._name != null && this._name.StartsWith("[", StringComparison.OrdinalIgnoreCase) && this._name.EndsWith("]", StringComparison.OrdinalIgnoreCase))
					{
						string typeName = this._name.Substring(1, this._name.Length - 2);
						TypeResolver.TryResolveType(typeName, out this._type);
					}
					this._typeWasCalculated = true;
				}
				return this._type;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x00016A7F File Offset: 0x00014C7F
		// (set) Token: 0x060004DF RID: 1247 RVA: 0x00016A87 File Offset: 0x00014C87
		public TypeDefinitionAst TypeDefinitionAst { get; private set; }

		// Token: 0x060004E0 RID: 1248 RVA: 0x00016A90 File Offset: 0x00014C90
		public override string ToString()
		{
			return this._name ?? string.Empty;
		}

		// Token: 0x040001D0 RID: 464
		private readonly string _name;

		// Token: 0x040001D1 RID: 465
		private Type _type;

		// Token: 0x040001D2 RID: 466
		private bool _typeWasCalculated;
	}
}
