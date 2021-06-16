using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A76 RID: 2678
	internal sealed class ComTypeLibDesc : IDynamicMetaObjectProvider
	{
		// Token: 0x06006A9F RID: 27295 RVA: 0x00217AE9 File Offset: 0x00215CE9
		private ComTypeLibDesc()
		{
			this._enums = new Dictionary<string, ComTypeEnumDesc>();
			this._classes = new LinkedList<ComTypeClassDesc>();
		}

		// Token: 0x06006AA0 RID: 27296 RVA: 0x00217B08 File Offset: 0x00215D08
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "<type library {0}>", new object[]
			{
				this._typeLibName
			});
		}

		// Token: 0x17001DBD RID: 7613
		// (get) Token: 0x06006AA1 RID: 27297 RVA: 0x00217B35 File Offset: 0x00215D35
		public string Documentation
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x06006AA2 RID: 27298 RVA: 0x00217B3C File Offset: 0x00215D3C
		DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter)
		{
			return new TypeLibMetaObject(parameter, this);
		}

		// Token: 0x06006AA3 RID: 27299 RVA: 0x00217B48 File Offset: 0x00215D48
		public static ComTypeLibInfo CreateFromGuid(Guid typeLibGuid)
		{
			ITypeLib typeLib = UnsafeMethods.LoadRegTypeLib(ref typeLibGuid, -1, -1, 0);
			return new ComTypeLibInfo(ComTypeLibDesc.GetFromTypeLib(typeLib));
		}

		// Token: 0x06006AA4 RID: 27300 RVA: 0x00217B6C File Offset: 0x00215D6C
		public static ComTypeLibInfo CreateFromObject(object rcw)
		{
			if (!Marshal.IsComObject(rcw))
			{
				throw new ArgumentException("COM object is expected.");
			}
			ITypeInfo itypeInfoFromIDispatch = ComRuntimeHelpers.GetITypeInfoFromIDispatch(rcw as IDispatch, true);
			ITypeLib typeLib;
			int num;
			itypeInfoFromIDispatch.GetContainingTypeLib(out typeLib, out num);
			return new ComTypeLibInfo(ComTypeLibDesc.GetFromTypeLib(typeLib));
		}

		// Token: 0x06006AA5 RID: 27301 RVA: 0x00217BB0 File Offset: 0x00215DB0
		internal static ComTypeLibDesc GetFromTypeLib(ITypeLib typeLib)
		{
			System.Runtime.InteropServices.ComTypes.TYPELIBATTR typeAttrForTypeLib = ComRuntimeHelpers.GetTypeAttrForTypeLib(typeLib);
			ComTypeLibDesc comTypeLibDesc;
			lock (ComTypeLibDesc._CachedTypeLibDesc)
			{
				if (ComTypeLibDesc._CachedTypeLibDesc.TryGetValue(typeAttrForTypeLib.guid, out comTypeLibDesc))
				{
					return comTypeLibDesc;
				}
			}
			comTypeLibDesc = new ComTypeLibDesc();
			comTypeLibDesc._typeLibName = ComRuntimeHelpers.GetNameOfLib(typeLib);
			comTypeLibDesc._typeLibAttributes = typeAttrForTypeLib;
			int typeInfoCount = typeLib.GetTypeInfoCount();
			for (int i = 0; i < typeInfoCount; i++)
			{
				System.Runtime.InteropServices.ComTypes.TYPEKIND typekind;
				typeLib.GetTypeInfoType(i, out typekind);
				ITypeInfo typeInfo;
				typeLib.GetTypeInfo(i, out typeInfo);
				if (typekind == System.Runtime.InteropServices.ComTypes.TYPEKIND.TKIND_COCLASS)
				{
					ComTypeClassDesc value = new ComTypeClassDesc(typeInfo, comTypeLibDesc);
					comTypeLibDesc._classes.AddLast(value);
				}
				else if (typekind == System.Runtime.InteropServices.ComTypes.TYPEKIND.TKIND_ENUM)
				{
					ComTypeEnumDesc comTypeEnumDesc = new ComTypeEnumDesc(typeInfo, comTypeLibDesc);
					comTypeLibDesc._enums.Add(comTypeEnumDesc.TypeName, comTypeEnumDesc);
				}
				else if (typekind == System.Runtime.InteropServices.ComTypes.TYPEKIND.TKIND_ALIAS)
				{
					System.Runtime.InteropServices.ComTypes.TYPEATTR typeAttrForTypeInfo = ComRuntimeHelpers.GetTypeAttrForTypeInfo(typeInfo);
					if (typeAttrForTypeInfo.tdescAlias.vt == 29)
					{
						string key;
						string text;
						ComRuntimeHelpers.GetInfoFromType(typeInfo, out key, out text);
						ITypeInfo typeInfo2;
						typeInfo.GetRefTypeInfo(typeAttrForTypeInfo.tdescAlias.lpValue.ToInt32(), out typeInfo2);
						if (ComRuntimeHelpers.GetTypeAttrForTypeInfo(typeInfo2).typekind == System.Runtime.InteropServices.ComTypes.TYPEKIND.TKIND_ENUM)
						{
							ComTypeEnumDesc value2 = new ComTypeEnumDesc(typeInfo2, comTypeLibDesc);
							comTypeLibDesc._enums.Add(key, value2);
						}
					}
				}
			}
			lock (ComTypeLibDesc._CachedTypeLibDesc)
			{
				ComTypeLibDesc._CachedTypeLibDesc.Add(typeAttrForTypeLib.guid, comTypeLibDesc);
			}
			return comTypeLibDesc;
		}

		// Token: 0x06006AA6 RID: 27302 RVA: 0x00217D50 File Offset: 0x00215F50
		public object GetTypeLibObjectDesc(string member)
		{
			foreach (ComTypeClassDesc comTypeClassDesc in this._classes)
			{
				if (member == comTypeClassDesc.TypeName)
				{
					return comTypeClassDesc;
				}
			}
			ComTypeEnumDesc result;
			if (this._enums != null && this._enums.TryGetValue(member, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06006AA7 RID: 27303 RVA: 0x00217DCC File Offset: 0x00215FCC
		public string[] GetMemberNames()
		{
			string[] array = new string[this._enums.Count + this._classes.Count];
			int num = 0;
			foreach (ComTypeClassDesc comTypeClassDesc in this._classes)
			{
				array[num++] = comTypeClassDesc.TypeName;
			}
			foreach (KeyValuePair<string, ComTypeEnumDesc> keyValuePair in this._enums)
			{
				array[num++] = keyValuePair.Key;
			}
			return array;
		}

		// Token: 0x06006AA8 RID: 27304 RVA: 0x00217E90 File Offset: 0x00216090
		internal bool HasMember(string member)
		{
			foreach (ComTypeClassDesc comTypeClassDesc in this._classes)
			{
				if (member == comTypeClassDesc.TypeName)
				{
					return true;
				}
			}
			return this._enums.ContainsKey(member);
		}

		// Token: 0x17001DBE RID: 7614
		// (get) Token: 0x06006AA9 RID: 27305 RVA: 0x00217F04 File Offset: 0x00216104
		public Guid Guid
		{
			get
			{
				return this._typeLibAttributes.guid;
			}
		}

		// Token: 0x17001DBF RID: 7615
		// (get) Token: 0x06006AAA RID: 27306 RVA: 0x00217F11 File Offset: 0x00216111
		public short VersionMajor
		{
			get
			{
				return this._typeLibAttributes.wMajorVerNum;
			}
		}

		// Token: 0x17001DC0 RID: 7616
		// (get) Token: 0x06006AAB RID: 27307 RVA: 0x00217F1E File Offset: 0x0021611E
		public short VersionMinor
		{
			get
			{
				return this._typeLibAttributes.wMinorVerNum;
			}
		}

		// Token: 0x17001DC1 RID: 7617
		// (get) Token: 0x06006AAC RID: 27308 RVA: 0x00217F2B File Offset: 0x0021612B
		public string Name
		{
			get
			{
				return this._typeLibName;
			}
		}

		// Token: 0x06006AAD RID: 27309 RVA: 0x00217F34 File Offset: 0x00216134
		internal ComTypeClassDesc GetCoClassForInterface(string itfName)
		{
			foreach (ComTypeClassDesc comTypeClassDesc in this._classes)
			{
				if (comTypeClassDesc.Implements(itfName, false))
				{
					return comTypeClassDesc;
				}
			}
			return null;
		}

		// Token: 0x0400331C RID: 13084
		private LinkedList<ComTypeClassDesc> _classes;

		// Token: 0x0400331D RID: 13085
		private Dictionary<string, ComTypeEnumDesc> _enums;

		// Token: 0x0400331E RID: 13086
		private string _typeLibName;

		// Token: 0x0400331F RID: 13087
		private System.Runtime.InteropServices.ComTypes.TYPELIBATTR _typeLibAttributes;

		// Token: 0x04003320 RID: 13088
		private static Dictionary<Guid, ComTypeLibDesc> _CachedTypeLibDesc = new Dictionary<Guid, ComTypeLibDesc>();
	}
}
