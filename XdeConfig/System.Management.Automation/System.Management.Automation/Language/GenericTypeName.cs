using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace System.Management.Automation.Language
{
	// Token: 0x02000581 RID: 1409
	public sealed class GenericTypeName : ITypeName, ISupportsTypeCaching
	{
		// Token: 0x06003A5E RID: 14942 RVA: 0x00133FC8 File Offset: 0x001321C8
		public GenericTypeName(IScriptExtent extent, ITypeName genericTypeName, IEnumerable<ITypeName> genericArguments)
		{
			if (genericTypeName == null || extent == null)
			{
				throw PSTraceSource.NewArgumentNullException((extent == null) ? "extent" : "genericTypeName");
			}
			if (genericArguments == null)
			{
				throw PSTraceSource.NewArgumentException("genericArguments");
			}
			this._extent = extent;
			this.TypeName = genericTypeName;
			this.GenericArguments = new ReadOnlyCollection<ITypeName>(genericArguments.ToArray<ITypeName>());
			if (this.GenericArguments.Count == 0)
			{
				throw PSTraceSource.NewArgumentException("genericArguments");
			}
		}

		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x06003A5F RID: 14943 RVA: 0x0013403C File Offset: 0x0013223C
		public string FullName
		{
			get
			{
				if (this._cachedFullName == null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(this.TypeName.Name);
					stringBuilder.Append('[');
					bool flag = true;
					for (int i = 0; i < this.GenericArguments.Count; i++)
					{
						ITypeName typeName = this.GenericArguments[i];
						if (!flag)
						{
							stringBuilder.Append(',');
						}
						flag = false;
						stringBuilder.Append(typeName.FullName);
					}
					stringBuilder.Append(']');
					string assemblyName = this.TypeName.AssemblyName;
					if (assemblyName != null)
					{
						stringBuilder.Append(',');
						stringBuilder.Append(assemblyName);
					}
					Interlocked.CompareExchange<string>(ref this._cachedFullName, stringBuilder.ToString(), null);
				}
				return this._cachedFullName;
			}
		}

		// Token: 0x17000D01 RID: 3329
		// (get) Token: 0x06003A60 RID: 14944 RVA: 0x001340FC File Offset: 0x001322FC
		public string Name
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(this.TypeName.Name);
				stringBuilder.Append('[');
				bool flag = true;
				for (int i = 0; i < this.GenericArguments.Count; i++)
				{
					ITypeName typeName = this.GenericArguments[i];
					if (!flag)
					{
						stringBuilder.Append(',');
					}
					flag = false;
					stringBuilder.Append(typeName.Name);
				}
				stringBuilder.Append(']');
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000D02 RID: 3330
		// (get) Token: 0x06003A61 RID: 14945 RVA: 0x00134179 File Offset: 0x00132379
		public string AssemblyName
		{
			get
			{
				return this.TypeName.AssemblyName;
			}
		}

		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x06003A62 RID: 14946 RVA: 0x00134186 File Offset: 0x00132386
		public bool IsArray
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x06003A63 RID: 14947 RVA: 0x00134189 File Offset: 0x00132389
		public bool IsGeneric
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x06003A64 RID: 14948 RVA: 0x0013418C File Offset: 0x0013238C
		// (set) Token: 0x06003A65 RID: 14949 RVA: 0x00134194 File Offset: 0x00132394
		public ITypeName TypeName { get; private set; }

		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x06003A66 RID: 14950 RVA: 0x0013419D File Offset: 0x0013239D
		// (set) Token: 0x06003A67 RID: 14951 RVA: 0x001341A5 File Offset: 0x001323A5
		public ReadOnlyCollection<ITypeName> GenericArguments { get; private set; }

		// Token: 0x17000D07 RID: 3335
		// (get) Token: 0x06003A68 RID: 14952 RVA: 0x001341AE File Offset: 0x001323AE
		public IScriptExtent Extent
		{
			get
			{
				return this._extent;
			}
		}

		// Token: 0x06003A69 RID: 14953 RVA: 0x001341B8 File Offset: 0x001323B8
		public Type GetReflectionType()
		{
			if (this._cachedType == null)
			{
				Type genericType = this.GetGenericType(this.TypeName.GetReflectionType());
				if (genericType != null && genericType.GetTypeInfo().ContainsGenericParameters)
				{
					List<Type> list = new List<Type>();
					foreach (ITypeName typeName in this.GenericArguments)
					{
						Type reflectionType = typeName.GetReflectionType();
						if (reflectionType == null)
						{
							return null;
						}
						list.Add(reflectionType);
					}
					try
					{
						Type type = genericType.MakeGenericType(list.ToArray());
						try
						{
							RuntimeTypeHandle typeHandle = type.TypeHandle;
						}
						catch (NotSupportedException)
						{
							return type;
						}
						Interlocked.CompareExchange<Type>(ref this._cachedType, type, null);
					}
					catch (Exception e)
					{
						CommandProcessorBase.CheckForSevereException(e);
					}
				}
			}
			return this._cachedType;
		}

		// Token: 0x06003A6A RID: 14954 RVA: 0x001342C0 File Offset: 0x001324C0
		internal Type GetGenericType(Type generic)
		{
			if ((generic == null || !generic.GetTypeInfo().ContainsGenericParameters) && this.TypeName.FullName.IndexOf("`", StringComparison.OrdinalIgnoreCase) == -1)
			{
				TypeName typeName = new TypeName(this._extent, string.Format(CultureInfo.InvariantCulture, "{0}`{1}", new object[]
				{
					this.TypeName.FullName,
					this.GenericArguments.Count
				}));
				generic = typeName.GetReflectionType();
			}
			return generic;
		}

		// Token: 0x06003A6B RID: 14955 RVA: 0x00134354 File Offset: 0x00132554
		public Type GetReflectionAttributeType()
		{
			Type type = this.GetReflectionType();
			if (type == null)
			{
				Type type2 = this.TypeName.GetReflectionAttributeType();
				TypeInfo typeInfo = (type2 != null) ? type2.GetTypeInfo() : null;
				if ((typeInfo == null || !typeInfo.ContainsGenericParameters) && this.TypeName.FullName.IndexOf("`", StringComparison.OrdinalIgnoreCase) == -1)
				{
					TypeName typeName = new TypeName(this._extent, string.Format(CultureInfo.InvariantCulture, "{0}Attribute`{1}", new object[]
					{
						this.TypeName.FullName,
						this.GenericArguments.Count
					}));
					type2 = typeName.GetReflectionType();
					typeInfo = ((type2 != null) ? type2.GetTypeInfo() : null);
				}
				if (typeInfo != null && typeInfo.ContainsGenericParameters)
				{
					type = typeInfo.MakeGenericType((from arg in this.GenericArguments
					select arg.GetReflectionType()).ToArray<Type>());
					Interlocked.CompareExchange<Type>(ref this._cachedType, type, null);
				}
			}
			return type;
		}

		// Token: 0x06003A6C RID: 14956 RVA: 0x00134473 File Offset: 0x00132673
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x06003A6D RID: 14957 RVA: 0x0013447C File Offset: 0x0013267C
		public override bool Equals(object obj)
		{
			GenericTypeName genericTypeName = obj as GenericTypeName;
			if (genericTypeName == null)
			{
				return false;
			}
			if (!this.TypeName.Equals(genericTypeName.TypeName))
			{
				return false;
			}
			if (this.GenericArguments.Count != genericTypeName.GenericArguments.Count)
			{
				return false;
			}
			int count = this.GenericArguments.Count;
			for (int i = 0; i < count; i++)
			{
				if (!this.GenericArguments[i].Equals(genericTypeName.GenericArguments[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003A6E RID: 14958 RVA: 0x00134500 File Offset: 0x00132700
		public override int GetHashCode()
		{
			int num = this.TypeName.GetHashCode();
			int count = this.GenericArguments.Count;
			for (int i = 0; i < count; i++)
			{
				num = Utils.CombineHashCodes(num, this.GenericArguments[i].GetHashCode());
			}
			return num;
		}

		// Token: 0x17000D08 RID: 3336
		// (get) Token: 0x06003A6F RID: 14959 RVA: 0x0013454A File Offset: 0x0013274A
		// (set) Token: 0x06003A70 RID: 14960 RVA: 0x00134552 File Offset: 0x00132752
		Type ISupportsTypeCaching.CachedType
		{
			get
			{
				return this._cachedType;
			}
			set
			{
				this._cachedType = value;
			}
		}

		// Token: 0x04001D5D RID: 7517
		private string _cachedFullName;

		// Token: 0x04001D5E RID: 7518
		private Type _cachedType;

		// Token: 0x04001D5F RID: 7519
		private readonly IScriptExtent _extent;
	}
}
