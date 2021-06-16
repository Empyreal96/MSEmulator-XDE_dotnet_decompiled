using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000083 RID: 131
	public abstract class JsonContract
	{
		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x0001C8F7 File Offset: 0x0001AAF7
		public Type UnderlyingType { get; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060006A2 RID: 1698 RVA: 0x0001C8FF File Offset: 0x0001AAFF
		// (set) Token: 0x060006A3 RID: 1699 RVA: 0x0001C907 File Offset: 0x0001AB07
		public Type CreatedType
		{
			get
			{
				return this._createdType;
			}
			set
			{
				this._createdType = value;
				this.IsSealed = this._createdType.IsSealed();
				this.IsInstantiable = (!this._createdType.IsInterface() && !this._createdType.IsAbstract());
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060006A4 RID: 1700 RVA: 0x0001C945 File Offset: 0x0001AB45
		// (set) Token: 0x060006A5 RID: 1701 RVA: 0x0001C94D File Offset: 0x0001AB4D
		public bool? IsReference { get; set; }

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060006A6 RID: 1702 RVA: 0x0001C956 File Offset: 0x0001AB56
		// (set) Token: 0x060006A7 RID: 1703 RVA: 0x0001C95E File Offset: 0x0001AB5E
		public JsonConverter Converter { get; set; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060006A8 RID: 1704 RVA: 0x0001C967 File Offset: 0x0001AB67
		// (set) Token: 0x060006A9 RID: 1705 RVA: 0x0001C96F File Offset: 0x0001AB6F
		public JsonConverter InternalConverter { get; internal set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060006AA RID: 1706 RVA: 0x0001C978 File Offset: 0x0001AB78
		public IList<SerializationCallback> OnDeserializedCallbacks
		{
			get
			{
				if (this._onDeserializedCallbacks == null)
				{
					this._onDeserializedCallbacks = new List<SerializationCallback>();
				}
				return this._onDeserializedCallbacks;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x0001C993 File Offset: 0x0001AB93
		public IList<SerializationCallback> OnDeserializingCallbacks
		{
			get
			{
				if (this._onDeserializingCallbacks == null)
				{
					this._onDeserializingCallbacks = new List<SerializationCallback>();
				}
				return this._onDeserializingCallbacks;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060006AC RID: 1708 RVA: 0x0001C9AE File Offset: 0x0001ABAE
		public IList<SerializationCallback> OnSerializedCallbacks
		{
			get
			{
				if (this._onSerializedCallbacks == null)
				{
					this._onSerializedCallbacks = new List<SerializationCallback>();
				}
				return this._onSerializedCallbacks;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x0001C9C9 File Offset: 0x0001ABC9
		public IList<SerializationCallback> OnSerializingCallbacks
		{
			get
			{
				if (this._onSerializingCallbacks == null)
				{
					this._onSerializingCallbacks = new List<SerializationCallback>();
				}
				return this._onSerializingCallbacks;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x0001C9E4 File Offset: 0x0001ABE4
		public IList<SerializationErrorCallback> OnErrorCallbacks
		{
			get
			{
				if (this._onErrorCallbacks == null)
				{
					this._onErrorCallbacks = new List<SerializationErrorCallback>();
				}
				return this._onErrorCallbacks;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x0001C9FF File Offset: 0x0001ABFF
		// (set) Token: 0x060006B0 RID: 1712 RVA: 0x0001CA07 File Offset: 0x0001AC07
		public Func<object> DefaultCreator { get; set; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x0001CA10 File Offset: 0x0001AC10
		// (set) Token: 0x060006B2 RID: 1714 RVA: 0x0001CA18 File Offset: 0x0001AC18
		public bool DefaultCreatorNonPublic { get; set; }

		// Token: 0x060006B3 RID: 1715 RVA: 0x0001CA24 File Offset: 0x0001AC24
		internal JsonContract(Type underlyingType)
		{
			ValidationUtils.ArgumentNotNull(underlyingType, "underlyingType");
			this.UnderlyingType = underlyingType;
			underlyingType = ReflectionUtils.EnsureNotByRefType(underlyingType);
			this.IsNullable = ReflectionUtils.IsNullable(underlyingType);
			this.NonNullableUnderlyingType = ((this.IsNullable && ReflectionUtils.IsNullableType(underlyingType)) ? Nullable.GetUnderlyingType(underlyingType) : underlyingType);
			this.CreatedType = this.NonNullableUnderlyingType;
			this.IsConvertable = ConvertUtils.IsConvertible(this.NonNullableUnderlyingType);
			this.IsEnum = this.NonNullableUnderlyingType.IsEnum();
			this.InternalReadType = ReadType.Read;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0001CAB4 File Offset: 0x0001ACB4
		internal void InvokeOnSerializing(object o, StreamingContext context)
		{
			if (this._onSerializingCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onSerializingCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0001CB08 File Offset: 0x0001AD08
		internal void InvokeOnSerialized(object o, StreamingContext context)
		{
			if (this._onSerializedCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onSerializedCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0001CB5C File Offset: 0x0001AD5C
		internal void InvokeOnDeserializing(object o, StreamingContext context)
		{
			if (this._onDeserializingCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onDeserializingCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0001CBB0 File Offset: 0x0001ADB0
		internal void InvokeOnDeserialized(object o, StreamingContext context)
		{
			if (this._onDeserializedCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onDeserializedCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0001CC0C File Offset: 0x0001AE0C
		internal void InvokeOnError(object o, StreamingContext context, ErrorContext errorContext)
		{
			if (this._onErrorCallbacks != null)
			{
				foreach (SerializationErrorCallback serializationErrorCallback in this._onErrorCallbacks)
				{
					serializationErrorCallback(o, context, errorContext);
				}
			}
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0001CC64 File Offset: 0x0001AE64
		internal static SerializationCallback CreateSerializationCallback(MethodInfo callbackMethodInfo)
		{
			return delegate(object o, StreamingContext context)
			{
				callbackMethodInfo.Invoke(o, new object[]
				{
					context
				});
			};
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0001CC7D File Offset: 0x0001AE7D
		internal static SerializationErrorCallback CreateSerializationErrorCallback(MethodInfo callbackMethodInfo)
		{
			return delegate(object o, StreamingContext context, ErrorContext econtext)
			{
				callbackMethodInfo.Invoke(o, new object[]
				{
					context,
					econtext
				});
			};
		}

		// Token: 0x04000249 RID: 585
		internal bool IsNullable;

		// Token: 0x0400024A RID: 586
		internal bool IsConvertable;

		// Token: 0x0400024B RID: 587
		internal bool IsEnum;

		// Token: 0x0400024C RID: 588
		internal Type NonNullableUnderlyingType;

		// Token: 0x0400024D RID: 589
		internal ReadType InternalReadType;

		// Token: 0x0400024E RID: 590
		internal JsonContractType ContractType;

		// Token: 0x0400024F RID: 591
		internal bool IsReadOnlyOrFixedSize;

		// Token: 0x04000250 RID: 592
		internal bool IsSealed;

		// Token: 0x04000251 RID: 593
		internal bool IsInstantiable;

		// Token: 0x04000252 RID: 594
		private List<SerializationCallback> _onDeserializedCallbacks;

		// Token: 0x04000253 RID: 595
		private IList<SerializationCallback> _onDeserializingCallbacks;

		// Token: 0x04000254 RID: 596
		private IList<SerializationCallback> _onSerializedCallbacks;

		// Token: 0x04000255 RID: 597
		private IList<SerializationCallback> _onSerializingCallbacks;

		// Token: 0x04000256 RID: 598
		private IList<SerializationErrorCallback> _onErrorCallbacks;

		// Token: 0x04000257 RID: 599
		private Type _createdType;
	}
}
