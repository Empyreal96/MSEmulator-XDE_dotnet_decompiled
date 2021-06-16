using System;
using System.Linq;
using System.Threading;

namespace System.Management.Automation.Language
{
	// Token: 0x02000580 RID: 1408
	public sealed class TypeName : ITypeName, ISupportsTypeCaching
	{
		// Token: 0x06003A4B RID: 14923 RVA: 0x00133C40 File Offset: 0x00131E40
		public TypeName(IScriptExtent extent, string name)
		{
			if (extent == null || string.IsNullOrEmpty(name))
			{
				throw PSTraceSource.NewArgumentNullException((extent == null) ? "extent" : "name");
			}
			char c = name[0];
			if (c == '[' || c == ']' || c == ',')
			{
				throw PSTraceSource.NewArgumentException("name");
			}
			int num = name.IndexOf('`');
			if (num != -1)
			{
				name = name.Replace("``", "`");
			}
			this._extent = extent;
			this._name = name;
		}

		// Token: 0x06003A4C RID: 14924 RVA: 0x00133CC2 File Offset: 0x00131EC2
		public TypeName(IScriptExtent extent, string name, string assembly) : this(extent, name)
		{
			if (string.IsNullOrEmpty(assembly))
			{
				throw PSTraceSource.NewArgumentNullException("assembly");
			}
			this.AssemblyName = assembly;
		}

		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x06003A4D RID: 14925 RVA: 0x00133CE6 File Offset: 0x00131EE6
		public string FullName
		{
			get
			{
				if (this.AssemblyName == null)
				{
					return this._name;
				}
				return this._name + "," + this.AssemblyName;
			}
		}

		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x06003A4E RID: 14926 RVA: 0x00133D0D File Offset: 0x00131F0D
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x06003A4F RID: 14927 RVA: 0x00133D15 File Offset: 0x00131F15
		// (set) Token: 0x06003A50 RID: 14928 RVA: 0x00133D1D File Offset: 0x00131F1D
		public string AssemblyName { get; internal set; }

		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x06003A51 RID: 14929 RVA: 0x00133D26 File Offset: 0x00131F26
		public bool IsArray
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x06003A52 RID: 14930 RVA: 0x00133D29 File Offset: 0x00131F29
		public bool IsGeneric
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x06003A53 RID: 14931 RVA: 0x00133D2C File Offset: 0x00131F2C
		public IScriptExtent Extent
		{
			get
			{
				return this._extent;
			}
		}

		// Token: 0x06003A54 RID: 14932 RVA: 0x00133D34 File Offset: 0x00131F34
		internal bool HasDefaultCtor()
		{
			if (this._typeDefinitionAst == null)
			{
				Type reflectionType = this.GetReflectionType();
				return !(reflectionType == null) && reflectionType.HasDefaultCtor();
			}
			bool flag = false;
			foreach (MemberAst memberAst in this._typeDefinitionAst.Members)
			{
				FunctionMemberAst functionMemberAst = memberAst as FunctionMemberAst;
				if (functionMemberAst != null && functionMemberAst.IsConstructor)
				{
					if (!functionMemberAst.Parameters.Any<ParameterAst>())
					{
						return true;
					}
					flag = true;
				}
			}
			return !flag;
		}

		// Token: 0x06003A55 RID: 14933 RVA: 0x00133DD4 File Offset: 0x00131FD4
		public Type GetReflectionType()
		{
			if (this._type == null)
			{
				Exception ex;
				Type type = (this._typeDefinitionAst != null) ? this._typeDefinitionAst.Type : TypeResolver.ResolveTypeName(this, out ex);
				if (type != null)
				{
					try
					{
						RuntimeTypeHandle typeHandle = type.TypeHandle;
					}
					catch (NotSupportedException)
					{
						return type;
					}
					Interlocked.CompareExchange<Type>(ref this._type, type, null);
				}
			}
			return this._type;
		}

		// Token: 0x06003A56 RID: 14934 RVA: 0x00133E4C File Offset: 0x0013204C
		public Type GetReflectionAttributeType()
		{
			Type type = this.GetReflectionType();
			if (type == null || !typeof(Attribute).IsAssignableFrom(type))
			{
				TypeName typeName = new TypeName(this._extent, this.FullName + "Attribute");
				type = typeName.GetReflectionType();
				if (type != null && !typeof(Attribute).IsAssignableFrom(type))
				{
					type = null;
				}
			}
			return type;
		}

		// Token: 0x06003A57 RID: 14935 RVA: 0x00133EBC File Offset: 0x001320BC
		internal void SetTypeDefinition(TypeDefinitionAst typeDefinitionAst)
		{
			this._typeDefinitionAst = typeDefinitionAst;
		}

		// Token: 0x06003A58 RID: 14936 RVA: 0x00133EC5 File Offset: 0x001320C5
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x06003A59 RID: 14937 RVA: 0x00133ED0 File Offset: 0x001320D0
		public override bool Equals(object obj)
		{
			TypeName typeName = obj as TypeName;
			if (typeName == null)
			{
				return false;
			}
			if (!this._name.Equals(typeName._name, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			if (this.AssemblyName == null)
			{
				return typeName.AssemblyName == null;
			}
			return typeName.AssemblyName != null && this.AssemblyName.Equals(typeName.AssemblyName, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06003A5A RID: 14938 RVA: 0x00133F30 File Offset: 0x00132130
		public override int GetHashCode()
		{
			StringComparer ordinalIgnoreCase = StringComparer.OrdinalIgnoreCase;
			int hashCode = ordinalIgnoreCase.GetHashCode(this._name);
			if (this.AssemblyName == null)
			{
				return hashCode;
			}
			return Utils.CombineHashCodes(hashCode, ordinalIgnoreCase.GetHashCode(this.AssemblyName));
		}

		// Token: 0x06003A5B RID: 14939 RVA: 0x00133F6C File Offset: 0x0013216C
		internal bool IsType(Type type)
		{
			string fullName = type.FullName;
			if (fullName.Equals(this.Name, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			int num = fullName.LastIndexOf('.');
			return num >= 0 && fullName.Substring(num + 1).Equals(this.Name, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x06003A5C RID: 14940 RVA: 0x00133FB5 File Offset: 0x001321B5
		// (set) Token: 0x06003A5D RID: 14941 RVA: 0x00133FBD File Offset: 0x001321BD
		Type ISupportsTypeCaching.CachedType
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		// Token: 0x04001D58 RID: 7512
		internal readonly string _name;

		// Token: 0x04001D59 RID: 7513
		internal Type _type;

		// Token: 0x04001D5A RID: 7514
		internal readonly IScriptExtent _extent;

		// Token: 0x04001D5B RID: 7515
		internal TypeDefinitionAst _typeDefinitionAst;
	}
}
