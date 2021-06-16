using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace System.Management.Automation.Language
{
	// Token: 0x02000582 RID: 1410
	public sealed class ArrayTypeName : ITypeName, ISupportsTypeCaching
	{
		// Token: 0x06003A72 RID: 14962 RVA: 0x0013455C File Offset: 0x0013275C
		public ArrayTypeName(IScriptExtent extent, ITypeName elementType, int rank)
		{
			if (extent == null || elementType == null)
			{
				throw PSTraceSource.NewArgumentNullException((extent == null) ? "extent" : "name");
			}
			if (rank <= 0)
			{
				throw PSTraceSource.NewArgumentException("rank");
			}
			this._extent = extent;
			this.Rank = rank;
			this.ElementType = elementType;
		}

		// Token: 0x06003A73 RID: 14963 RVA: 0x001345B0 File Offset: 0x001327B0
		private string GetName(bool includeAssemblyName)
		{
			StringBuilder stringBuilder = new StringBuilder();
			try
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				stringBuilder.Append(this.ElementType.Name);
				stringBuilder.Append('[');
				if (this.Rank > 1)
				{
					stringBuilder.Append(',', this.Rank - 1);
				}
				stringBuilder.Append(']');
				if (includeAssemblyName)
				{
					string assemblyName = this.ElementType.AssemblyName;
					if (assemblyName != null)
					{
						stringBuilder.Append(',');
						stringBuilder.Append(assemblyName);
					}
				}
			}
			catch (InsufficientExecutionStackException)
			{
				throw new ScriptCallDepthException();
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x06003A74 RID: 14964 RVA: 0x00134648 File Offset: 0x00132848
		public string FullName
		{
			get
			{
				if (this._cachedFullName == null)
				{
					Interlocked.CompareExchange<string>(ref this._cachedFullName, this.GetName(true), null);
				}
				return this._cachedFullName;
			}
		}

		// Token: 0x17000D0A RID: 3338
		// (get) Token: 0x06003A75 RID: 14965 RVA: 0x0013466C File Offset: 0x0013286C
		public string Name
		{
			get
			{
				return this.GetName(false);
			}
		}

		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x06003A76 RID: 14966 RVA: 0x00134675 File Offset: 0x00132875
		public string AssemblyName
		{
			get
			{
				return this.ElementType.AssemblyName;
			}
		}

		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x06003A77 RID: 14967 RVA: 0x00134682 File Offset: 0x00132882
		public bool IsArray
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x06003A78 RID: 14968 RVA: 0x00134685 File Offset: 0x00132885
		public bool IsGeneric
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x06003A79 RID: 14969 RVA: 0x00134688 File Offset: 0x00132888
		// (set) Token: 0x06003A7A RID: 14970 RVA: 0x00134690 File Offset: 0x00132890
		public ITypeName ElementType { get; private set; }

		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x06003A7B RID: 14971 RVA: 0x00134699 File Offset: 0x00132899
		// (set) Token: 0x06003A7C RID: 14972 RVA: 0x001346A1 File Offset: 0x001328A1
		public int Rank { get; private set; }

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x06003A7D RID: 14973 RVA: 0x001346AA File Offset: 0x001328AA
		public IScriptExtent Extent
		{
			get
			{
				return this._extent;
			}
		}

		// Token: 0x06003A7E RID: 14974 RVA: 0x001346B4 File Offset: 0x001328B4
		public Type GetReflectionType()
		{
			try
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				if (this._cachedType == null)
				{
					Type reflectionType = this.ElementType.GetReflectionType();
					if (reflectionType != null)
					{
						Type type = (this.Rank == 1) ? reflectionType.MakeArrayType() : reflectionType.MakeArrayType(this.Rank);
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
				}
			}
			catch (InsufficientExecutionStackException)
			{
				throw new ScriptCallDepthException();
			}
			return this._cachedType;
		}

		// Token: 0x06003A7F RID: 14975 RVA: 0x00134750 File Offset: 0x00132950
		public Type GetReflectionAttributeType()
		{
			return null;
		}

		// Token: 0x06003A80 RID: 14976 RVA: 0x00134753 File Offset: 0x00132953
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x06003A81 RID: 14977 RVA: 0x0013475C File Offset: 0x0013295C
		public override bool Equals(object obj)
		{
			ArrayTypeName arrayTypeName = obj as ArrayTypeName;
			return arrayTypeName != null && this.ElementType.Equals(arrayTypeName.ElementType) && this.Rank == arrayTypeName.Rank;
		}

		// Token: 0x06003A82 RID: 14978 RVA: 0x00134798 File Offset: 0x00132998
		public override int GetHashCode()
		{
			return Utils.CombineHashCodes(this.ElementType.GetHashCode(), this.Rank.GetHashCode());
		}

		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x06003A83 RID: 14979 RVA: 0x001347C3 File Offset: 0x001329C3
		// (set) Token: 0x06003A84 RID: 14980 RVA: 0x001347CB File Offset: 0x001329CB
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

		// Token: 0x04001D63 RID: 7523
		private string _cachedFullName;

		// Token: 0x04001D64 RID: 7524
		private Type _cachedType;

		// Token: 0x04001D65 RID: 7525
		private readonly IScriptExtent _extent;
	}
}
