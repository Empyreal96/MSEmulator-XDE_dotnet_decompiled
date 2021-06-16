using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Management.Automation.ComInterop
{
	// Token: 0x02000A86 RID: 2694
	internal sealed class IDispatchComObject : ComObject, IDynamicMetaObjectProvider
	{
		// Token: 0x06006B01 RID: 27393 RVA: 0x00218BAE File Offset: 0x00216DAE
		internal IDispatchComObject(IDispatch rcw) : base(rcw)
		{
			this._dispatchObject = rcw;
		}

		// Token: 0x06006B02 RID: 27394 RVA: 0x00218BC0 File Offset: 0x00216DC0
		public override string ToString()
		{
			ComTypeDesc comTypeDesc = this._comTypeDesc;
			string text = null;
			if (comTypeDesc != null)
			{
				text = comTypeDesc.TypeName;
			}
			if (string.IsNullOrEmpty(text))
			{
				text = "IDispatch";
			}
			return string.Format(CultureInfo.CurrentCulture, "{0} ({1})", new object[]
			{
				base.RuntimeCallableWrapper.ToString(),
				text
			});
		}

		// Token: 0x17001DCB RID: 7627
		// (get) Token: 0x06006B03 RID: 27395 RVA: 0x00218C17 File Offset: 0x00216E17
		public ComTypeDesc ComTypeDesc
		{
			get
			{
				this.EnsureScanDefinedMethods();
				return this._comTypeDesc;
			}
		}

		// Token: 0x17001DCC RID: 7628
		// (get) Token: 0x06006B04 RID: 27396 RVA: 0x00218C25 File Offset: 0x00216E25
		public IDispatch DispatchObject
		{
			get
			{
				return this._dispatchObject;
			}
		}

		// Token: 0x06006B05 RID: 27397 RVA: 0x00218C30 File Offset: 0x00216E30
		private static int GetIDsOfNames(IDispatch dispatch, string name, out int dispId)
		{
			int[] array = new int[1];
			Guid empty = Guid.Empty;
			int result = dispatch.TryGetIDsOfNames(ref empty, new string[]
			{
				name
			}, 1U, 0, array);
			dispId = array[0];
			return result;
		}

		// Token: 0x06006B06 RID: 27398 RVA: 0x00218C68 File Offset: 0x00216E68
		private static int Invoke(IDispatch dispatch, int memberDispId, out object result)
		{
			Guid empty = Guid.Empty;
			System.Runtime.InteropServices.ComTypes.DISPPARAMS dispparams = default(System.Runtime.InteropServices.ComTypes.DISPPARAMS);
			System.Runtime.InteropServices.ComTypes.EXCEPINFO excepinfo = default(System.Runtime.InteropServices.ComTypes.EXCEPINFO);
			uint num;
			return dispatch.TryInvoke(memberDispId, ref empty, 0, System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYGET, ref dispparams, out result, out excepinfo, out num);
		}

		// Token: 0x06006B07 RID: 27399 RVA: 0x00218CA4 File Offset: 0x00216EA4
		internal bool TryGetGetItem(out ComMethodDesc value)
		{
			ComMethodDesc getItem = this._comTypeDesc.GetItem;
			if (getItem != null)
			{
				value = getItem;
				return true;
			}
			return this.SlowTryGetGetItem(out value);
		}

		// Token: 0x06006B08 RID: 27400 RVA: 0x00218CCC File Offset: 0x00216ECC
		private bool SlowTryGetGetItem(out ComMethodDesc value)
		{
			this.EnsureScanDefinedMethods();
			ComMethodDesc getItem = this._comTypeDesc.GetItem;
			if (getItem == null)
			{
				string name = "[PROPERTYGET, DISPID(0)]";
				this._comTypeDesc.EnsureGetItem(new ComMethodDesc(name, 0, System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYGET));
				getItem = this._comTypeDesc.GetItem;
			}
			value = getItem;
			return true;
		}

		// Token: 0x06006B09 RID: 27401 RVA: 0x00218D18 File Offset: 0x00216F18
		internal bool TryGetSetItem(out ComMethodDesc value)
		{
			ComMethodDesc setItem = this._comTypeDesc.SetItem;
			if (setItem != null)
			{
				value = setItem;
				return true;
			}
			return this.SlowTryGetSetItem(out value);
		}

		// Token: 0x06006B0A RID: 27402 RVA: 0x00218D40 File Offset: 0x00216F40
		private bool SlowTryGetSetItem(out ComMethodDesc value)
		{
			this.EnsureScanDefinedMethods();
			ComMethodDesc setItem = this._comTypeDesc.SetItem;
			if (setItem == null)
			{
				string name = "[PROPERTYPUT, DISPID(0)]";
				this._comTypeDesc.EnsureSetItem(new ComMethodDesc(name, 0, System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYPUT));
				setItem = this._comTypeDesc.SetItem;
			}
			value = setItem;
			return true;
		}

		// Token: 0x06006B0B RID: 27403 RVA: 0x00218D8B File Offset: 0x00216F8B
		internal bool TryGetMemberMethod(string name, out ComMethodDesc method)
		{
			this.EnsureScanDefinedMethods();
			return this._comTypeDesc.TryGetFunc(name, out method);
		}

		// Token: 0x06006B0C RID: 27404 RVA: 0x00218DA0 File Offset: 0x00216FA0
		internal bool TryGetMemberEvent(string name, out ComEventDesc @event)
		{
			this.EnsureScanDefinedEvents();
			return this._comTypeDesc.TryGetEvent(name, out @event);
		}

		// Token: 0x06006B0D RID: 27405 RVA: 0x00218DB8 File Offset: 0x00216FB8
		internal bool TryGetMemberMethodExplicit(string name, out ComMethodDesc method)
		{
			this.EnsureScanDefinedMethods();
			int dispId;
			int idsOfNames = IDispatchComObject.GetIDsOfNames(this._dispatchObject, name, out dispId);
			if (idsOfNames == 0)
			{
				ComMethodDesc comMethodDesc = new ComMethodDesc(name, dispId, System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_FUNC);
				this._comTypeDesc.AddFunc(name, comMethodDesc);
				method = comMethodDesc;
				return true;
			}
			if (idsOfNames == -2147352570)
			{
				method = null;
				return false;
			}
			throw Error.CouldNotGetDispId(name, string.Format(CultureInfo.InvariantCulture, "0x{0:X})", new object[]
			{
				idsOfNames
			}));
		}

		// Token: 0x06006B0E RID: 27406 RVA: 0x00218E2C File Offset: 0x0021702C
		internal bool TryGetPropertySetterExplicit(string name, out ComMethodDesc method, Type limitType, bool holdsNull)
		{
			this.EnsureScanDefinedMethods();
			int dispId;
			int idsOfNames = IDispatchComObject.GetIDsOfNames(this._dispatchObject, name, out dispId);
			if (idsOfNames == 0)
			{
				ComMethodDesc comMethodDesc = new ComMethodDesc(name, dispId, System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYPUT);
				this._comTypeDesc.AddPut(name, comMethodDesc);
				ComMethodDesc comMethodDesc2 = new ComMethodDesc(name, dispId, System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYPUTREF);
				this._comTypeDesc.AddPutRef(name, comMethodDesc2);
				if (ComBinderHelpers.PreferPut(limitType, holdsNull))
				{
					method = comMethodDesc;
				}
				else
				{
					method = comMethodDesc2;
				}
				return true;
			}
			if (idsOfNames == -2147352570)
			{
				method = null;
				return false;
			}
			throw Error.CouldNotGetDispId(name, string.Format(CultureInfo.InvariantCulture, "0x{0:X})", new object[]
			{
				idsOfNames
			}));
		}

		// Token: 0x06006B0F RID: 27407 RVA: 0x00218EC7 File Offset: 0x002170C7
		internal override IList<string> GetMemberNames(bool dataOnly)
		{
			this.EnsureScanDefinedMethods();
			this.EnsureScanDefinedEvents();
			return this.ComTypeDesc.GetMemberNames(dataOnly);
		}

		// Token: 0x06006B10 RID: 27408 RVA: 0x00218EE4 File Offset: 0x002170E4
		internal override IList<KeyValuePair<string, object>> GetMembers(IEnumerable<string> names)
		{
			if (names == null)
			{
				names = this.GetMemberNames(true);
			}
			Type type = base.RuntimeCallableWrapper.GetType();
			List<KeyValuePair<string, object>> list = new List<KeyValuePair<string, object>>();
			foreach (string text in names)
			{
				ComMethodDesc comMethodDesc;
				if (text != null && this.ComTypeDesc.TryGetFunc(text, out comMethodDesc) && comMethodDesc.IsDataMember)
				{
					try
					{
						object value = type.InvokeMember(comMethodDesc.Name, BindingFlags.GetProperty, null, base.RuntimeCallableWrapper, new object[0], CultureInfo.InvariantCulture);
						list.Add(new KeyValuePair<string, object>(comMethodDesc.Name, value));
					}
					catch (Exception value2)
					{
						list.Add(new KeyValuePair<string, object>(comMethodDesc.Name, value2));
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06006B11 RID: 27409 RVA: 0x00218FC8 File Offset: 0x002171C8
		DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter)
		{
			this.EnsureScanDefinedMethods();
			return new IDispatchMetaObject(parameter, this);
		}

		// Token: 0x06006B12 RID: 27410 RVA: 0x00218FD8 File Offset: 0x002171D8
		private static void GetFuncDescForDescIndex(ITypeInfo typeInfo, int funcIndex, out System.Runtime.InteropServices.ComTypes.FUNCDESC funcDesc, out IntPtr funcDescHandle)
		{
			IntPtr zero = IntPtr.Zero;
			typeInfo.GetFuncDesc(funcIndex, out zero);
			if (zero == IntPtr.Zero)
			{
				throw Error.CannotRetrieveTypeInformation();
			}
			funcDesc = (System.Runtime.InteropServices.ComTypes.FUNCDESC)Marshal.PtrToStructure(zero, typeof(System.Runtime.InteropServices.ComTypes.FUNCDESC));
			funcDescHandle = zero;
		}

		// Token: 0x06006B13 RID: 27411 RVA: 0x0021902C File Offset: 0x0021722C
		private void EnsureScanDefinedEvents()
		{
			if (this._comTypeDesc != null && this._comTypeDesc.Events != null)
			{
				return;
			}
			ITypeInfo itypeInfoFromIDispatch = ComRuntimeHelpers.GetITypeInfoFromIDispatch(this._dispatchObject, true);
			if (itypeInfoFromIDispatch == null)
			{
				this._comTypeDesc = ComTypeDesc.CreateEmptyTypeDesc();
				return;
			}
			System.Runtime.InteropServices.ComTypes.TYPEATTR typeAttrForTypeInfo = ComRuntimeHelpers.GetTypeAttrForTypeInfo(itypeInfoFromIDispatch);
			if (this._comTypeDesc == null)
			{
				lock (IDispatchComObject._CacheComTypeDesc)
				{
					if (IDispatchComObject._CacheComTypeDesc.TryGetValue(typeAttrForTypeInfo.guid, out this._comTypeDesc) && this._comTypeDesc.Events != null)
					{
						return;
					}
				}
			}
			ComTypeDesc comTypeDesc = ComTypeDesc.FromITypeInfo(itypeInfoFromIDispatch, typeAttrForTypeInfo);
			Dictionary<string, ComEventDesc> dictionary = null;
			ITypeInfo coClassTypeInfo;
			if (!(base.RuntimeCallableWrapper is IConnectionPointContainer))
			{
				dictionary = ComTypeDesc.EmptyEvents;
			}
			else if ((coClassTypeInfo = IDispatchComObject.GetCoClassTypeInfo(base.RuntimeCallableWrapper, itypeInfoFromIDispatch)) == null)
			{
				dictionary = ComTypeDesc.EmptyEvents;
			}
			else
			{
				dictionary = new Dictionary<string, ComEventDesc>();
				System.Runtime.InteropServices.ComTypes.TYPEATTR typeAttrForTypeInfo2 = ComRuntimeHelpers.GetTypeAttrForTypeInfo(coClassTypeInfo);
				for (int i = 0; i < (int)typeAttrForTypeInfo2.cImplTypes; i++)
				{
					int hRef;
					coClassTypeInfo.GetRefTypeOfImplType(i, out hRef);
					ITypeInfo sourceTypeInfo;
					coClassTypeInfo.GetRefTypeInfo(hRef, out sourceTypeInfo);
					System.Runtime.InteropServices.ComTypes.IMPLTYPEFLAGS impltypeflags;
					coClassTypeInfo.GetImplTypeFlags(i, out impltypeflags);
					if ((impltypeflags & System.Runtime.InteropServices.ComTypes.IMPLTYPEFLAGS.IMPLTYPEFLAG_FSOURCE) != (System.Runtime.InteropServices.ComTypes.IMPLTYPEFLAGS)0)
					{
						IDispatchComObject.ScanSourceInterface(sourceTypeInfo, ref dictionary);
					}
				}
				if (dictionary.Count == 0)
				{
					dictionary = ComTypeDesc.EmptyEvents;
				}
			}
			lock (IDispatchComObject._CacheComTypeDesc)
			{
				ComTypeDesc comTypeDesc2;
				if (IDispatchComObject._CacheComTypeDesc.TryGetValue(typeAttrForTypeInfo.guid, out comTypeDesc2))
				{
					this._comTypeDesc = comTypeDesc2;
				}
				else
				{
					this._comTypeDesc = comTypeDesc;
					IDispatchComObject._CacheComTypeDesc.Add(typeAttrForTypeInfo.guid, this._comTypeDesc);
				}
				this._comTypeDesc.Events = dictionary;
			}
		}

		// Token: 0x06006B14 RID: 27412 RVA: 0x002191F4 File Offset: 0x002173F4
		private static void ScanSourceInterface(ITypeInfo sourceTypeInfo, ref Dictionary<string, ComEventDesc> events)
		{
			System.Runtime.InteropServices.ComTypes.TYPEATTR typeAttrForTypeInfo = ComRuntimeHelpers.GetTypeAttrForTypeInfo(sourceTypeInfo);
			for (int i = 0; i < (int)typeAttrForTypeInfo.cFuncs; i++)
			{
				IntPtr zero = IntPtr.Zero;
				try
				{
					System.Runtime.InteropServices.ComTypes.FUNCDESC funcdesc;
					IDispatchComObject.GetFuncDescForDescIndex(sourceTypeInfo, i, out funcdesc, out zero);
					if ((funcdesc.wFuncFlags & 64) == 0)
					{
						if ((funcdesc.wFuncFlags & 1) == 0)
						{
							string text = ComRuntimeHelpers.GetNameOfMethod(sourceTypeInfo, funcdesc.memid);
							text = text.ToUpper(CultureInfo.InvariantCulture);
							if (!events.ContainsKey(text))
							{
								ComEventDesc comEventDesc = new ComEventDesc();
								comEventDesc.dispid = funcdesc.memid;
								comEventDesc.sourceIID = typeAttrForTypeInfo.guid;
								events.Add(text, comEventDesc);
							}
						}
					}
				}
				finally
				{
					if (zero != IntPtr.Zero)
					{
						sourceTypeInfo.ReleaseFuncDesc(zero);
					}
				}
			}
		}

		// Token: 0x06006B15 RID: 27413 RVA: 0x002192CC File Offset: 0x002174CC
		private static ITypeInfo GetCoClassTypeInfo(object rcw, ITypeInfo typeInfo)
		{
			IProvideClassInfo provideClassInfo = rcw as IProvideClassInfo;
			if (provideClassInfo != null)
			{
				IntPtr zero = IntPtr.Zero;
				try
				{
					provideClassInfo.GetClassInfo(out zero);
					if (zero != IntPtr.Zero)
					{
						return Marshal.GetObjectForIUnknown(zero) as ITypeInfo;
					}
				}
				finally
				{
					if (zero != IntPtr.Zero)
					{
						Marshal.Release(zero);
					}
				}
			}
			ITypeLib typeLib;
			int num;
			typeInfo.GetContainingTypeLib(out typeLib, out num);
			string nameOfType = ComRuntimeHelpers.GetNameOfType(typeInfo);
			ComTypeLibDesc fromTypeLib = ComTypeLibDesc.GetFromTypeLib(typeLib);
			ComTypeClassDesc coClassForInterface = fromTypeLib.GetCoClassForInterface(nameOfType);
			if (coClassForInterface == null)
			{
				return null;
			}
			Guid guid = coClassForInterface.Guid;
			ITypeInfo result;
			typeLib.GetTypeInfoOfGuid(ref guid, out result);
			return result;
		}

		// Token: 0x06006B16 RID: 27414 RVA: 0x00219378 File Offset: 0x00217578
		private void EnsureScanDefinedMethods()
		{
			if (this._comTypeDesc != null && this._comTypeDesc.Funcs != null)
			{
				return;
			}
			ITypeInfo itypeInfoFromIDispatch = ComRuntimeHelpers.GetITypeInfoFromIDispatch(this._dispatchObject, true);
			if (itypeInfoFromIDispatch == null)
			{
				this._comTypeDesc = ComTypeDesc.CreateEmptyTypeDesc();
				return;
			}
			System.Runtime.InteropServices.ComTypes.TYPEATTR typeAttrForTypeInfo = ComRuntimeHelpers.GetTypeAttrForTypeInfo(itypeInfoFromIDispatch);
			if (this._comTypeDesc == null)
			{
				lock (IDispatchComObject._CacheComTypeDesc)
				{
					if (IDispatchComObject._CacheComTypeDesc.TryGetValue(typeAttrForTypeInfo.guid, out this._comTypeDesc) && this._comTypeDesc.Funcs != null)
					{
						return;
					}
				}
			}
			ComTypeDesc comTypeDesc = ComTypeDesc.FromITypeInfo(itypeInfoFromIDispatch, typeAttrForTypeInfo);
			ComMethodDesc candidate = null;
			ComMethodDesc comMethodDesc = null;
			Hashtable hashtable = new Hashtable((int)typeAttrForTypeInfo.cFuncs);
			Hashtable hashtable2 = new Hashtable();
			Hashtable hashtable3 = new Hashtable();
			for (int i = 0; i < (int)typeAttrForTypeInfo.cFuncs; i++)
			{
				IntPtr zero = IntPtr.Zero;
				try
				{
					System.Runtime.InteropServices.ComTypes.FUNCDESC funcDesc;
					IDispatchComObject.GetFuncDescForDescIndex(itypeInfoFromIDispatch, i, out funcDesc, out zero);
					if ((funcDesc.wFuncFlags & 1) == 0)
					{
						ComMethodDesc comMethodDesc2 = new ComMethodDesc(itypeInfoFromIDispatch, funcDesc);
						string key = comMethodDesc2.Name.ToUpper(CultureInfo.InvariantCulture);
						if ((funcDesc.invkind & System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYPUT) != (System.Runtime.InteropServices.ComTypes.INVOKEKIND)0)
						{
							if (hashtable.ContainsKey(key))
							{
								comMethodDesc2.InputType = ((ComMethodDesc)hashtable[key]).ReturnType;
							}
							hashtable2.Add(key, comMethodDesc2);
							if (comMethodDesc2.DispId == 0 && comMethodDesc == null)
							{
								comMethodDesc = comMethodDesc2;
							}
						}
						else if ((funcDesc.invkind & System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYPUTREF) != (System.Runtime.InteropServices.ComTypes.INVOKEKIND)0)
						{
							if (hashtable.ContainsKey(key))
							{
								comMethodDesc2.InputType = ((ComMethodDesc)hashtable[key]).ReturnType;
							}
							hashtable3.Add(key, comMethodDesc2);
							if (comMethodDesc2.DispId == 0 && comMethodDesc == null)
							{
								comMethodDesc = comMethodDesc2;
							}
						}
						else if (funcDesc.memid == -4)
						{
							hashtable.Add("GETENUMERATOR", comMethodDesc2);
						}
						else
						{
							if (hashtable2.ContainsKey(key))
							{
								((ComMethodDesc)hashtable2[key]).InputType = comMethodDesc2.ReturnType;
							}
							if (hashtable3.ContainsKey(key))
							{
								((ComMethodDesc)hashtable3[key]).InputType = comMethodDesc2.ReturnType;
							}
							hashtable.Add(key, comMethodDesc2);
							if (funcDesc.memid == 0)
							{
								candidate = comMethodDesc2;
							}
						}
					}
				}
				finally
				{
					if (zero != IntPtr.Zero)
					{
						itypeInfoFromIDispatch.ReleaseFuncDesc(zero);
					}
				}
			}
			lock (IDispatchComObject._CacheComTypeDesc)
			{
				ComTypeDesc comTypeDesc2;
				if (IDispatchComObject._CacheComTypeDesc.TryGetValue(typeAttrForTypeInfo.guid, out comTypeDesc2))
				{
					this._comTypeDesc = comTypeDesc2;
				}
				else
				{
					this._comTypeDesc = comTypeDesc;
					IDispatchComObject._CacheComTypeDesc.Add(typeAttrForTypeInfo.guid, this._comTypeDesc);
				}
				this._comTypeDesc.Funcs = hashtable;
				this._comTypeDesc.Puts = hashtable2;
				this._comTypeDesc.PutRefs = hashtable3;
				this._comTypeDesc.EnsureGetItem(candidate);
				this._comTypeDesc.EnsureSetItem(comMethodDesc);
			}
		}

		// Token: 0x06006B17 RID: 27415 RVA: 0x002196B8 File Offset: 0x002178B8
		internal bool TryGetPropertySetter(string name, out ComMethodDesc method, Type limitType, bool holdsNull)
		{
			this.EnsureScanDefinedMethods();
			if (ComBinderHelpers.PreferPut(limitType, holdsNull))
			{
				return this._comTypeDesc.TryGetPut(name, out method) || this._comTypeDesc.TryGetPutRef(name, out method);
			}
			return this._comTypeDesc.TryGetPutRef(name, out method) || this._comTypeDesc.TryGetPut(name, out method);
		}

		// Token: 0x04003333 RID: 13107
		private readonly IDispatch _dispatchObject;

		// Token: 0x04003334 RID: 13108
		private ComTypeDesc _comTypeDesc;

		// Token: 0x04003335 RID: 13109
		private static readonly Dictionary<Guid, ComTypeDesc> _CacheComTypeDesc = new Dictionary<Guid, ComTypeDesc>();
	}
}
