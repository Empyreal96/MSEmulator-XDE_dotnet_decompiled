using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000084 RID: 132
	public class JsonDictionaryContract : JsonContainerContract
	{
		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x0001CC96 File Offset: 0x0001AE96
		// (set) Token: 0x060006BC RID: 1724 RVA: 0x0001CC9E File Offset: 0x0001AE9E
		public Func<string, string> DictionaryKeyResolver { get; set; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060006BD RID: 1725 RVA: 0x0001CCA7 File Offset: 0x0001AEA7
		public Type DictionaryKeyType { get; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x0001CCAF File Offset: 0x0001AEAF
		public Type DictionaryValueType { get; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060006BF RID: 1727 RVA: 0x0001CCB7 File Offset: 0x0001AEB7
		// (set) Token: 0x060006C0 RID: 1728 RVA: 0x0001CCBF File Offset: 0x0001AEBF
		internal JsonContract KeyContract { get; set; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x0001CCC8 File Offset: 0x0001AEC8
		internal bool ShouldCreateWrapper { get; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060006C2 RID: 1730 RVA: 0x0001CCD0 File Offset: 0x0001AED0
		internal ObjectConstructor<object> ParameterizedCreator
		{
			get
			{
				if (this._parameterizedCreator == null)
				{
					this._parameterizedCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(this._parameterizedConstructor);
				}
				return this._parameterizedCreator;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x0001CCF6 File Offset: 0x0001AEF6
		// (set) Token: 0x060006C4 RID: 1732 RVA: 0x0001CCFE File Offset: 0x0001AEFE
		public ObjectConstructor<object> OverrideCreator
		{
			get
			{
				return this._overrideCreator;
			}
			set
			{
				this._overrideCreator = value;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x0001CD07 File Offset: 0x0001AF07
		// (set) Token: 0x060006C6 RID: 1734 RVA: 0x0001CD0F File Offset: 0x0001AF0F
		public bool HasParameterizedCreator { get; set; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x0001CD18 File Offset: 0x0001AF18
		internal bool HasParameterizedCreatorInternal
		{
			get
			{
				return this.HasParameterizedCreator || this._parameterizedCreator != null || this._parameterizedConstructor != null;
			}
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0001CD38 File Offset: 0x0001AF38
		public JsonDictionaryContract(Type underlyingType) : base(underlyingType)
		{
			this.ContractType = JsonContractType.Dictionary;
			Type type;
			Type type2;
			if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(IDictionary<, >), out this._genericCollectionDefinitionType))
			{
				type = this._genericCollectionDefinitionType.GetGenericArguments()[0];
				type2 = this._genericCollectionDefinitionType.GetGenericArguments()[1];
				if (ReflectionUtils.IsGenericDefinition(base.UnderlyingType, typeof(IDictionary<, >)))
				{
					base.CreatedType = typeof(Dictionary<, >).MakeGenericType(new Type[]
					{
						type,
						type2
					});
				}
				else if (underlyingType.IsGenericType() && underlyingType.GetGenericTypeDefinition().FullName == "System.Collections.Concurrent.ConcurrentDictionary`2")
				{
					this.ShouldCreateWrapper = 1;
				}
				this.IsReadOnlyOrFixedSize = ReflectionUtils.InheritsGenericDefinition(underlyingType, typeof(ReadOnlyDictionary<, >));
			}
			else if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(IReadOnlyDictionary<, >), out this._genericCollectionDefinitionType))
			{
				type = this._genericCollectionDefinitionType.GetGenericArguments()[0];
				type2 = this._genericCollectionDefinitionType.GetGenericArguments()[1];
				if (ReflectionUtils.IsGenericDefinition(base.UnderlyingType, typeof(IReadOnlyDictionary<, >)))
				{
					base.CreatedType = typeof(ReadOnlyDictionary<, >).MakeGenericType(new Type[]
					{
						type,
						type2
					});
				}
				this.IsReadOnlyOrFixedSize = true;
			}
			else
			{
				ReflectionUtils.GetDictionaryKeyValueTypes(base.UnderlyingType, out type, out type2);
				if (base.UnderlyingType == typeof(IDictionary))
				{
					base.CreatedType = typeof(Dictionary<object, object>);
				}
			}
			if (type != null && type2 != null)
			{
				this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(base.CreatedType, typeof(KeyValuePair<, >).MakeGenericType(new Type[]
				{
					type,
					type2
				}), typeof(IDictionary<, >).MakeGenericType(new Type[]
				{
					type,
					type2
				}));
				if (!this.HasParameterizedCreatorInternal && underlyingType.Name == "FSharpMap`2")
				{
					FSharpUtils.EnsureInitialized(underlyingType.Assembly());
					this._parameterizedCreator = FSharpUtils.CreateMap(type, type2);
				}
			}
			if (!typeof(IDictionary).IsAssignableFrom(base.CreatedType))
			{
				this.ShouldCreateWrapper = 1;
			}
			this.DictionaryKeyType = type;
			this.DictionaryValueType = type2;
			Type createdType;
			ObjectConstructor<object> parameterizedCreator;
			if (ImmutableCollectionsUtils.TryBuildImmutableForDictionaryContract(underlyingType, this.DictionaryKeyType, this.DictionaryValueType, out createdType, out parameterizedCreator))
			{
				base.CreatedType = createdType;
				this._parameterizedCreator = parameterizedCreator;
				this.IsReadOnlyOrFixedSize = true;
			}
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0001CFA0 File Offset: 0x0001B1A0
		internal IWrappedDictionary CreateWrapper(object dictionary)
		{
			if (this._genericWrapperCreator == null)
			{
				this._genericWrapperType = typeof(DictionaryWrapper<, >).MakeGenericType(new Type[]
				{
					this.DictionaryKeyType,
					this.DictionaryValueType
				});
				ConstructorInfo constructor = this._genericWrapperType.GetConstructor(new Type[]
				{
					this._genericCollectionDefinitionType
				});
				this._genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor(constructor);
			}
			return (IWrappedDictionary)this._genericWrapperCreator(new object[]
			{
				dictionary
			});
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0001D028 File Offset: 0x0001B228
		internal IDictionary CreateTemporaryDictionary()
		{
			if (this._genericTemporaryDictionaryCreator == null)
			{
				Type type = typeof(Dictionary<, >).MakeGenericType(new Type[]
				{
					this.DictionaryKeyType ?? typeof(object),
					this.DictionaryValueType ?? typeof(object)
				});
				this._genericTemporaryDictionaryCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(type);
			}
			return (IDictionary)this._genericTemporaryDictionaryCreator();
		}

		// Token: 0x04000262 RID: 610
		private readonly Type _genericCollectionDefinitionType;

		// Token: 0x04000263 RID: 611
		private Type _genericWrapperType;

		// Token: 0x04000264 RID: 612
		private ObjectConstructor<object> _genericWrapperCreator;

		// Token: 0x04000265 RID: 613
		private Func<object> _genericTemporaryDictionaryCreator;

		// Token: 0x04000267 RID: 615
		private readonly ConstructorInfo _parameterizedConstructor;

		// Token: 0x04000268 RID: 616
		private ObjectConstructor<object> _overrideCreator;

		// Token: 0x04000269 RID: 617
		private ObjectConstructor<object> _parameterizedCreator;
	}
}
