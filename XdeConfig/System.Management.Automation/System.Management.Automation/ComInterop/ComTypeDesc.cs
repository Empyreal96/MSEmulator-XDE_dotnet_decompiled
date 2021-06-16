using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A73 RID: 2675
	internal class ComTypeDesc : ComTypeLibMemberDesc
	{
		// Token: 0x06006A77 RID: 27255 RVA: 0x002172AA File Offset: 0x002154AA
		internal ComTypeDesc(ITypeInfo typeInfo, ComType memberType, ComTypeLibDesc typeLibDesc) : base(memberType)
		{
			if (typeInfo != null)
			{
				ComRuntimeHelpers.GetInfoFromType(typeInfo, out this._typeName, out this._documentation);
			}
			this._typeLibDesc = typeLibDesc;
		}

		// Token: 0x06006A78 RID: 27256 RVA: 0x002172D0 File Offset: 0x002154D0
		internal static ComTypeDesc FromITypeInfo(ITypeInfo typeInfo, TYPEATTR typeAttr)
		{
			if (typeAttr.typekind == TYPEKIND.TKIND_COCLASS)
			{
				return new ComTypeClassDesc(typeInfo, null);
			}
			if (typeAttr.typekind == TYPEKIND.TKIND_ENUM)
			{
				return new ComTypeEnumDesc(typeInfo, null);
			}
			if (typeAttr.typekind == TYPEKIND.TKIND_DISPATCH || typeAttr.typekind == TYPEKIND.TKIND_INTERFACE)
			{
				return new ComTypeDesc(typeInfo, ComType.Interface, null);
			}
			throw new InvalidOperationException("Attempting to wrap an unsupported enum type.");
		}

		// Token: 0x06006A79 RID: 27257 RVA: 0x0021732C File Offset: 0x0021552C
		internal static ComTypeDesc CreateEmptyTypeDesc()
		{
			return new ComTypeDesc(null, ComType.Interface, null)
			{
				_funcs = new Hashtable(),
				_puts = new Hashtable(),
				_putRefs = new Hashtable(),
				_events = ComTypeDesc._EmptyEventsDict
			};
		}

		// Token: 0x17001DB2 RID: 7602
		// (get) Token: 0x06006A7A RID: 27258 RVA: 0x0021736F File Offset: 0x0021556F
		internal static Dictionary<string, ComEventDesc> EmptyEvents
		{
			get
			{
				return ComTypeDesc._EmptyEventsDict;
			}
		}

		// Token: 0x17001DB3 RID: 7603
		// (get) Token: 0x06006A7B RID: 27259 RVA: 0x00217376 File Offset: 0x00215576
		// (set) Token: 0x06006A7C RID: 27260 RVA: 0x0021737E File Offset: 0x0021557E
		internal Hashtable Funcs
		{
			get
			{
				return this._funcs;
			}
			set
			{
				this._funcs = value;
			}
		}

		// Token: 0x17001DB4 RID: 7604
		// (get) Token: 0x06006A7D RID: 27261 RVA: 0x00217387 File Offset: 0x00215587
		// (set) Token: 0x06006A7E RID: 27262 RVA: 0x0021738F File Offset: 0x0021558F
		internal Hashtable Puts
		{
			get
			{
				return this._puts;
			}
			set
			{
				this._puts = value;
			}
		}

		// Token: 0x17001DB5 RID: 7605
		// (set) Token: 0x06006A7F RID: 27263 RVA: 0x00217398 File Offset: 0x00215598
		internal Hashtable PutRefs
		{
			set
			{
				this._putRefs = value;
			}
		}

		// Token: 0x17001DB6 RID: 7606
		// (get) Token: 0x06006A80 RID: 27264 RVA: 0x002173A1 File Offset: 0x002155A1
		// (set) Token: 0x06006A81 RID: 27265 RVA: 0x002173A9 File Offset: 0x002155A9
		internal Dictionary<string, ComEventDesc> Events
		{
			get
			{
				return this._events;
			}
			set
			{
				this._events = value;
			}
		}

		// Token: 0x06006A82 RID: 27266 RVA: 0x002173B2 File Offset: 0x002155B2
		internal bool TryGetFunc(string name, out ComMethodDesc method)
		{
			name = name.ToUpper(CultureInfo.InvariantCulture);
			if (this._funcs.ContainsKey(name))
			{
				method = (this._funcs[name] as ComMethodDesc);
				return true;
			}
			method = null;
			return false;
		}

		// Token: 0x06006A83 RID: 27267 RVA: 0x002173E8 File Offset: 0x002155E8
		internal void AddFunc(string name, ComMethodDesc method)
		{
			name = name.ToUpper(CultureInfo.InvariantCulture);
			lock (this._funcs)
			{
				this._funcs[name] = method;
			}
		}

		// Token: 0x06006A84 RID: 27268 RVA: 0x0021743C File Offset: 0x0021563C
		internal bool TryGetPut(string name, out ComMethodDesc method)
		{
			name = name.ToUpper(CultureInfo.InvariantCulture);
			if (this._puts.ContainsKey(name))
			{
				method = (this._puts[name] as ComMethodDesc);
				return true;
			}
			method = null;
			return false;
		}

		// Token: 0x06006A85 RID: 27269 RVA: 0x00217474 File Offset: 0x00215674
		internal void AddPut(string name, ComMethodDesc method)
		{
			name = name.ToUpper(CultureInfo.InvariantCulture);
			lock (this._puts)
			{
				this._puts[name] = method;
			}
		}

		// Token: 0x06006A86 RID: 27270 RVA: 0x002174C8 File Offset: 0x002156C8
		internal bool TryGetPutRef(string name, out ComMethodDesc method)
		{
			name = name.ToUpper(CultureInfo.InvariantCulture);
			if (this._putRefs.ContainsKey(name))
			{
				method = (this._putRefs[name] as ComMethodDesc);
				return true;
			}
			method = null;
			return false;
		}

		// Token: 0x06006A87 RID: 27271 RVA: 0x00217500 File Offset: 0x00215700
		internal void AddPutRef(string name, ComMethodDesc method)
		{
			name = name.ToUpper(CultureInfo.InvariantCulture);
			lock (this._putRefs)
			{
				this._putRefs[name] = method;
			}
		}

		// Token: 0x06006A88 RID: 27272 RVA: 0x00217554 File Offset: 0x00215754
		internal bool TryGetEvent(string name, out ComEventDesc @event)
		{
			name = name.ToUpper(CultureInfo.InvariantCulture);
			return this._events.TryGetValue(name, out @event);
		}

		// Token: 0x06006A89 RID: 27273 RVA: 0x00217570 File Offset: 0x00215770
		internal string[] GetMemberNames(bool dataOnly)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			lock (this._funcs)
			{
				foreach (object obj in this._funcs.Values)
				{
					ComMethodDesc comMethodDesc = (ComMethodDesc)obj;
					if (!dataOnly || comMethodDesc.IsDataMember)
					{
						dictionary.Add(comMethodDesc.Name, null);
					}
				}
			}
			if (!dataOnly)
			{
				lock (this._puts)
				{
					foreach (object obj2 in this._puts.Values)
					{
						ComMethodDesc comMethodDesc2 = (ComMethodDesc)obj2;
						if (!dictionary.ContainsKey(comMethodDesc2.Name))
						{
							dictionary.Add(comMethodDesc2.Name, null);
						}
					}
				}
				lock (this._putRefs)
				{
					foreach (object obj3 in this._putRefs.Values)
					{
						ComMethodDesc comMethodDesc3 = (ComMethodDesc)obj3;
						if (!dictionary.ContainsKey(comMethodDesc3.Name))
						{
							dictionary.Add(comMethodDesc3.Name, null);
						}
					}
				}
				if (this._events != null && this._events.Count > 0)
				{
					foreach (string key in this._events.Keys)
					{
						if (!dictionary.ContainsKey(key))
						{
							dictionary.Add(key, null);
						}
					}
				}
			}
			string[] array = new string[dictionary.Keys.Count];
			dictionary.Keys.CopyTo(array, 0);
			return array;
		}

		// Token: 0x17001DB7 RID: 7607
		// (get) Token: 0x06006A8A RID: 27274 RVA: 0x002177D8 File Offset: 0x002159D8
		public string TypeName
		{
			get
			{
				return this._typeName;
			}
		}

		// Token: 0x17001DB8 RID: 7608
		// (get) Token: 0x06006A8B RID: 27275 RVA: 0x002177E0 File Offset: 0x002159E0
		internal string Documentation
		{
			get
			{
				return this._documentation;
			}
		}

		// Token: 0x17001DB9 RID: 7609
		// (get) Token: 0x06006A8C RID: 27276 RVA: 0x002177E8 File Offset: 0x002159E8
		public ComTypeLibDesc TypeLib
		{
			get
			{
				return this._typeLibDesc;
			}
		}

		// Token: 0x17001DBA RID: 7610
		// (get) Token: 0x06006A8D RID: 27277 RVA: 0x002177F0 File Offset: 0x002159F0
		// (set) Token: 0x06006A8E RID: 27278 RVA: 0x002177F8 File Offset: 0x002159F8
		internal Guid Guid
		{
			get
			{
				return this._guid;
			}
			set
			{
				this._guid = value;
			}
		}

		// Token: 0x17001DBB RID: 7611
		// (get) Token: 0x06006A8F RID: 27279 RVA: 0x00217801 File Offset: 0x00215A01
		internal ComMethodDesc GetItem
		{
			get
			{
				return this._getItem;
			}
		}

		// Token: 0x06006A90 RID: 27280 RVA: 0x00217809 File Offset: 0x00215A09
		internal void EnsureGetItem(ComMethodDesc candidate)
		{
			Interlocked.CompareExchange<ComMethodDesc>(ref this._getItem, candidate, null);
		}

		// Token: 0x17001DBC RID: 7612
		// (get) Token: 0x06006A91 RID: 27281 RVA: 0x00217819 File Offset: 0x00215A19
		internal ComMethodDesc SetItem
		{
			get
			{
				return this._setItem;
			}
		}

		// Token: 0x06006A92 RID: 27282 RVA: 0x00217821 File Offset: 0x00215A21
		internal void EnsureSetItem(ComMethodDesc candidate)
		{
			Interlocked.CompareExchange<ComMethodDesc>(ref this._setItem, candidate, null);
		}

		// Token: 0x0400330C RID: 13068
		private string _typeName;

		// Token: 0x0400330D RID: 13069
		private string _documentation;

		// Token: 0x0400330E RID: 13070
		private Guid _guid;

		// Token: 0x0400330F RID: 13071
		private Hashtable _funcs;

		// Token: 0x04003310 RID: 13072
		private Hashtable _puts;

		// Token: 0x04003311 RID: 13073
		private Hashtable _putRefs;

		// Token: 0x04003312 RID: 13074
		private ComMethodDesc _getItem;

		// Token: 0x04003313 RID: 13075
		private ComMethodDesc _setItem;

		// Token: 0x04003314 RID: 13076
		private Dictionary<string, ComEventDesc> _events;

		// Token: 0x04003315 RID: 13077
		private readonly ComTypeLibDesc _typeLibDesc;

		// Token: 0x04003316 RID: 13078
		private static readonly Dictionary<string, ComEventDesc> _EmptyEventsDict = new Dictionary<string, ComEventDesc>();
	}
}
