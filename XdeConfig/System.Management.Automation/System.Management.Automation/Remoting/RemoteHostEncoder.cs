using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Host;
using System.Reflection;
using System.Security;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002E2 RID: 738
	internal class RemoteHostEncoder
	{
		// Token: 0x06002335 RID: 9013 RVA: 0x000C62C0 File Offset: 0x000C44C0
		private static bool IsKnownType(Type type)
		{
			TypeSerializationInfo typeSerializationInfo = KnownTypes.GetTypeSerializationInfo(type);
			return typeSerializationInfo != null;
		}

		// Token: 0x06002336 RID: 9014 RVA: 0x000C62DC File Offset: 0x000C44DC
		private static bool IsEncodingAllowedForClassOrStruct(Type type)
		{
			return type == typeof(KeyInfo) || type == typeof(Coordinates) || type == typeof(Size) || type == typeof(BufferCell) || type == typeof(Rectangle) || type == typeof(ProgressRecord) || type == typeof(FieldDescription) || type == typeof(ChoiceDescription) || type == typeof(HostInfo) || type == typeof(HostDefaultData) || type == typeof(RemoteSessionCapability);
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x000C63B8 File Offset: 0x000C45B8
		private static PSObject EncodeClassOrStruct(object obj)
		{
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (FieldInfo fieldInfo in fields)
			{
				object value = fieldInfo.GetValue(obj);
				if (value != null)
				{
					object value2 = RemoteHostEncoder.EncodeObject(value);
					psobject.Properties.Add(new PSNoteProperty(fieldInfo.Name, value2));
				}
			}
			return psobject;
		}

		// Token: 0x06002338 RID: 9016 RVA: 0x000C6424 File Offset: 0x000C4624
		private static object DecodeClassOrStruct(PSObject psObject, Type type)
		{
			object uninitializedObject = ClrFacade.GetUninitializedObject(type);
			foreach (PSPropertyInfo pspropertyInfo in psObject.Properties)
			{
				FieldInfo field = type.GetField(pspropertyInfo.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (pspropertyInfo.Value == null)
				{
					throw RemoteHostExceptions.NewDecodingFailedException();
				}
				object obj = RemoteHostEncoder.DecodeObject(pspropertyInfo.Value, field.FieldType);
				if (obj == null)
				{
					throw RemoteHostExceptions.NewDecodingFailedException();
				}
				field.SetValue(uninitializedObject, obj);
			}
			return uninitializedObject;
		}

		// Token: 0x06002339 RID: 9017 RVA: 0x000C64B8 File Offset: 0x000C46B8
		private static bool IsCollection(Type type)
		{
			return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Collection<>));
		}

		// Token: 0x0600233A RID: 9018 RVA: 0x000C64DE File Offset: 0x000C46DE
		private static bool IsGenericIEnumerableOfInt(Type type)
		{
			return type.Equals(typeof(IEnumerable<int>));
		}

		// Token: 0x0600233B RID: 9019 RVA: 0x000C64F0 File Offset: 0x000C46F0
		private static PSObject EncodeCollection(IList collection)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in collection)
			{
				arrayList.Add(RemoteHostEncoder.EncodeObject(obj));
			}
			return new PSObject(arrayList);
		}

		// Token: 0x0600233C RID: 9020 RVA: 0x000C6554 File Offset: 0x000C4754
		private static IList DecodeCollection(PSObject psObject, Type collectionType)
		{
			Type[] genericArguments = collectionType.GetGenericArguments();
			Type type = genericArguments[0];
			ArrayList arrayList = RemoteHostEncoder.SafelyGetBaseObject<ArrayList>(psObject);
			IList list = (IList)Activator.CreateInstance(collectionType);
			foreach (object obj in arrayList)
			{
				list.Add(RemoteHostEncoder.DecodeObject(obj, type));
			}
			return list;
		}

		// Token: 0x0600233D RID: 9021 RVA: 0x000C65D4 File Offset: 0x000C47D4
		private static bool IsDictionary(Type type)
		{
			return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Dictionary<, >));
		}

		// Token: 0x0600233E RID: 9022 RVA: 0x000C65FC File Offset: 0x000C47FC
		private static PSObject EncodeDictionary(IDictionary dictionary)
		{
			if (RemoteHostEncoder.IsObjectDictionaryType(dictionary.GetType()))
			{
				return RemoteHostEncoder.EncodeObjectDictionary(dictionary);
			}
			Hashtable hashtable = new Hashtable();
			foreach (object obj in dictionary.Keys)
			{
				hashtable.Add(RemoteHostEncoder.EncodeObject(obj), RemoteHostEncoder.EncodeObject(dictionary[obj]));
			}
			return new PSObject(hashtable);
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x000C6684 File Offset: 0x000C4884
		private static IDictionary DecodeDictionary(PSObject psObject, Type dictionaryType)
		{
			if (RemoteHostEncoder.IsObjectDictionaryType(dictionaryType))
			{
				return RemoteHostEncoder.DecodeObjectDictionary(psObject, dictionaryType);
			}
			Type[] genericArguments = dictionaryType.GetGenericArguments();
			Type type = genericArguments[0];
			Type type2 = genericArguments[1];
			Hashtable hashtable = RemoteHostEncoder.SafelyGetBaseObject<Hashtable>(psObject);
			IDictionary dictionary = (IDictionary)Activator.CreateInstance(dictionaryType);
			foreach (object obj in hashtable.Keys)
			{
				dictionary.Add(RemoteHostEncoder.DecodeObject(obj, type), RemoteHostEncoder.DecodeObject(hashtable[obj], type2));
			}
			return dictionary;
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x000C672C File Offset: 0x000C492C
		private static PSObject EncodePSObject(PSObject psObject)
		{
			return psObject;
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x000C672F File Offset: 0x000C492F
		private static PSObject DecodePSObject(object obj)
		{
			if (obj is PSObject)
			{
				return (PSObject)obj;
			}
			return new PSObject(obj);
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x000C6748 File Offset: 0x000C4948
		private static PSObject EncodeException(Exception exception)
		{
			IContainsErrorRecord containsErrorRecord = exception as IContainsErrorRecord;
			ErrorRecord errorRecord;
			if (containsErrorRecord == null)
			{
				errorRecord = new ErrorRecord(exception, "RemoteHostExecutionException", ErrorCategory.NotSpecified, null);
			}
			else
			{
				errorRecord = containsErrorRecord.ErrorRecord;
				errorRecord = new ErrorRecord(errorRecord, exception);
			}
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			errorRecord.ToPSObjectForRemoting(psobject);
			return psobject;
		}

		// Token: 0x06002343 RID: 9027 RVA: 0x000C6790 File Offset: 0x000C4990
		private static Exception DecodeException(PSObject psObject)
		{
			ErrorRecord errorRecord = ErrorRecord.FromPSObjectForRemoting(psObject);
			if (errorRecord == null)
			{
				throw RemoteHostExceptions.NewDecodingErrorForErrorRecordException();
			}
			return errorRecord.Exception;
		}

		// Token: 0x06002344 RID: 9028 RVA: 0x000C67B4 File Offset: 0x000C49B4
		private static FieldDescription UpcastFieldDescriptionSubclassAndDropAttributes(FieldDescription fieldDescription1)
		{
			FieldDescription fieldDescription2 = new FieldDescription(fieldDescription1.Name);
			fieldDescription2.Label = fieldDescription1.Label;
			fieldDescription2.HelpMessage = fieldDescription1.HelpMessage;
			fieldDescription2.IsMandatory = fieldDescription1.IsMandatory;
			fieldDescription2.DefaultValue = fieldDescription1.DefaultValue;
			fieldDescription2.SetParameterTypeName(fieldDescription1.ParameterTypeName);
			fieldDescription2.SetParameterTypeFullName(fieldDescription1.ParameterTypeFullName);
			fieldDescription2.SetParameterAssemblyFullName(fieldDescription1.ParameterAssemblyFullName);
			return fieldDescription2;
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x000C6824 File Offset: 0x000C4A24
		internal static object EncodeObject(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			Type type = obj.GetType();
			if (obj is PSObject)
			{
				return RemoteHostEncoder.EncodePSObject((PSObject)obj);
			}
			if (obj is ProgressRecord)
			{
				return ((ProgressRecord)obj).ToPSObjectForRemoting();
			}
			if (RemoteHostEncoder.IsKnownType(type))
			{
				return obj;
			}
			if (type.GetTypeInfo().IsEnum)
			{
				return (int)obj;
			}
			if (obj is CultureInfo)
			{
				return obj.ToString();
			}
			if (obj is Exception)
			{
				return RemoteHostEncoder.EncodeException((Exception)obj);
			}
			if (type == typeof(object[]))
			{
				return RemoteHostEncoder.EncodeObjectArray((object[])obj);
			}
			if (type.IsArray)
			{
				return RemoteHostEncoder.EncodeArray((Array)obj);
			}
			if (obj is IList && RemoteHostEncoder.IsCollection(type))
			{
				return RemoteHostEncoder.EncodeCollection((IList)obj);
			}
			if (obj is IDictionary && RemoteHostEncoder.IsDictionary(type))
			{
				return RemoteHostEncoder.EncodeDictionary((IDictionary)obj);
			}
			if (type.IsSubclassOf(typeof(FieldDescription)) || type == typeof(FieldDescription))
			{
				return RemoteHostEncoder.EncodeClassOrStruct(RemoteHostEncoder.UpcastFieldDescriptionSubclassAndDropAttributes((FieldDescription)obj));
			}
			if (RemoteHostEncoder.IsEncodingAllowedForClassOrStruct(type))
			{
				return RemoteHostEncoder.EncodeClassOrStruct(obj);
			}
			if (obj is RemoteHostCall)
			{
				return ((RemoteHostCall)obj).Encode();
			}
			if (obj is RemoteHostResponse)
			{
				return ((RemoteHostResponse)obj).Encode();
			}
			if (obj is SecureString)
			{
				return obj;
			}
			if (obj is PSCredential)
			{
				return obj;
			}
			if (RemoteHostEncoder.IsGenericIEnumerableOfInt(type))
			{
				return RemoteHostEncoder.EncodeCollection((IList)obj);
			}
			throw RemoteHostExceptions.NewRemoteHostDataEncodingNotSupportedException(type);
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x000C69B0 File Offset: 0x000C4BB0
		internal static object DecodeObject(object obj, Type type)
		{
			if (obj == null)
			{
				return obj;
			}
			if (type == typeof(PSObject))
			{
				return RemoteHostEncoder.DecodePSObject(obj);
			}
			if (type == typeof(ProgressRecord))
			{
				return ProgressRecord.FromPSObjectForRemoting(PSObject.AsPSObject(obj));
			}
			if (RemoteHostEncoder.IsKnownType(type))
			{
				return obj;
			}
			if (obj is SecureString)
			{
				return obj;
			}
			if (obj is PSCredential)
			{
				return obj;
			}
			if (obj is PSObject && type == typeof(PSCredential))
			{
				PSObject psobject = (PSObject)obj;
				PSCredential result = null;
				try
				{
					result = new PSCredential((string)psobject.Properties["UserName"].Value, (SecureString)psobject.Properties["Password"].Value);
				}
				catch (GetValueException)
				{
					result = null;
				}
				return result;
			}
			if (obj is int && type.GetTypeInfo().IsEnum)
			{
				return Enum.ToObject(type, (int)obj);
			}
			if (obj is string && type == typeof(CultureInfo))
			{
				return new CultureInfo((string)obj);
			}
			if (obj is PSObject && type == typeof(Exception))
			{
				return RemoteHostEncoder.DecodeException((PSObject)obj);
			}
			if (obj is PSObject && type == typeof(object[]))
			{
				return RemoteHostEncoder.DecodeObjectArray((PSObject)obj);
			}
			if (obj is PSObject && type.IsArray)
			{
				return RemoteHostEncoder.DecodeArray((PSObject)obj, type);
			}
			if (obj is PSObject && RemoteHostEncoder.IsCollection(type))
			{
				return RemoteHostEncoder.DecodeCollection((PSObject)obj, type);
			}
			if (obj is PSObject && RemoteHostEncoder.IsDictionary(type))
			{
				return RemoteHostEncoder.DecodeDictionary((PSObject)obj, type);
			}
			if (obj is PSObject && RemoteHostEncoder.IsEncodingAllowedForClassOrStruct(type))
			{
				return RemoteHostEncoder.DecodeClassOrStruct((PSObject)obj, type);
			}
			if (obj is PSObject && RemoteHostEncoder.IsGenericIEnumerableOfInt(type))
			{
				return RemoteHostEncoder.DecodeCollection((PSObject)obj, typeof(Collection<int>));
			}
			if (obj is PSObject && type == typeof(RemoteHostCall))
			{
				return RemoteHostCall.Decode((PSObject)obj);
			}
			if (obj is PSObject && type == typeof(RemoteHostResponse))
			{
				return RemoteHostResponse.Decode((PSObject)obj);
			}
			throw RemoteHostExceptions.NewRemoteHostDataDecodingNotSupportedException(type);
		}

		// Token: 0x06002347 RID: 9031 RVA: 0x000C6C10 File Offset: 0x000C4E10
		internal static void EncodeAndAddAsProperty(PSObject psObject, string propertyName, object propertyValue)
		{
			if (propertyValue == null)
			{
				return;
			}
			psObject.Properties.Add(new PSNoteProperty(propertyName, RemoteHostEncoder.EncodeObject(propertyValue)));
		}

		// Token: 0x06002348 RID: 9032 RVA: 0x000C6C30 File Offset: 0x000C4E30
		internal static object DecodePropertyValue(PSObject psObject, string propertyName, Type propertyValueType)
		{
			ReadOnlyPSMemberInfoCollection<PSPropertyInfo> readOnlyPSMemberInfoCollection = psObject.Properties.Match(propertyName);
			if (readOnlyPSMemberInfoCollection.Count == 0)
			{
				return null;
			}
			return RemoteHostEncoder.DecodeObject(readOnlyPSMemberInfoCollection[0].Value, propertyValueType);
		}

		// Token: 0x06002349 RID: 9033 RVA: 0x000C6C68 File Offset: 0x000C4E68
		private static PSObject EncodeObjectArray(object[] objects)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in objects)
			{
				arrayList.Add(RemoteHostEncoder.EncodeObjectWithType(obj));
			}
			return new PSObject(arrayList);
		}

		// Token: 0x0600234A RID: 9034 RVA: 0x000C6CA4 File Offset: 0x000C4EA4
		private static object[] DecodeObjectArray(PSObject psObject)
		{
			ArrayList arrayList = RemoteHostEncoder.SafelyGetBaseObject<ArrayList>(psObject);
			object[] array = new object[arrayList.Count];
			for (int i = 0; i < arrayList.Count; i++)
			{
				array[i] = RemoteHostEncoder.DecodeObjectWithType(arrayList[i]);
			}
			return array;
		}

		// Token: 0x0600234B RID: 9035 RVA: 0x000C6CE8 File Offset: 0x000C4EE8
		private static PSObject EncodeObjectWithType(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			psobject.Properties.Add(new PSNoteProperty("T", obj.GetType().ToString()));
			psobject.Properties.Add(new PSNoteProperty("V", RemoteHostEncoder.EncodeObject(obj)));
			return psobject;
		}

		// Token: 0x0600234C RID: 9036 RVA: 0x000C6D3C File Offset: 0x000C4F3C
		private static object DecodeObjectWithType(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			PSObject psObject = RemoteHostEncoder.SafelyCastObject<PSObject>(obj);
			string valueToConvert = RemoteHostEncoder.SafelyGetPropertyValue<string>(psObject, "T");
			Type type = LanguagePrimitives.ConvertTo<Type>(valueToConvert);
			object obj2 = RemoteHostEncoder.SafelyGetPropertyValue<object>(psObject, "V");
			return RemoteHostEncoder.DecodeObject(obj2, type);
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x000C6D7C File Offset: 0x000C4F7C
		private static bool ArrayIsZeroBased(Array array)
		{
			int rank = array.Rank;
			for (int i = 0; i < rank; i++)
			{
				if (array.GetLowerBound(i) != 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x000C6DA8 File Offset: 0x000C4FA8
		private static PSObject EncodeArray(Array array)
		{
			Type type = array.GetType();
			type.GetElementType();
			int rank = array.Rank;
			int[] array2 = new int[rank];
			for (int i = 0; i < rank; i++)
			{
				array2[i] = array.GetUpperBound(i) + 1;
			}
			Indexer indexer = new Indexer(array2);
			ArrayList arrayList = new ArrayList();
			foreach (object obj in indexer)
			{
				int[] indices = (int[])obj;
				object value = array.GetValue(indices);
				arrayList.Add(RemoteHostEncoder.EncodeObject(value));
			}
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			psobject.Properties.Add(new PSNoteProperty("mae", arrayList));
			psobject.Properties.Add(new PSNoteProperty("mal", array2));
			return psobject;
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x000C6E94 File Offset: 0x000C5094
		private static Array DecodeArray(PSObject psObject, Type type)
		{
			Type elementType = type.GetElementType();
			PSObject psObject2 = RemoteHostEncoder.SafelyGetPropertyValue<PSObject>(psObject, "mae");
			ArrayList arrayList = RemoteHostEncoder.SafelyGetBaseObject<ArrayList>(psObject2);
			PSObject psObject3 = RemoteHostEncoder.SafelyGetPropertyValue<PSObject>(psObject, "mal");
			ArrayList arrayList2 = RemoteHostEncoder.SafelyGetBaseObject<ArrayList>(psObject3);
			int[] lengths = (int[])arrayList2.ToArray(typeof(int));
			Indexer indexer = new Indexer(lengths);
			Array array = Array.CreateInstance(elementType, lengths);
			int num = 0;
			foreach (object obj in indexer)
			{
				int[] indices = (int[])obj;
				object value = RemoteHostEncoder.DecodeObject(arrayList[num++], elementType);
				array.SetValue(value, indices);
			}
			return array;
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x000C6F68 File Offset: 0x000C5168
		private static bool IsObjectDictionaryType(Type dictionaryType)
		{
			if (!RemoteHostEncoder.IsDictionary(dictionaryType))
			{
				return false;
			}
			Type[] genericArguments = dictionaryType.GetGenericArguments();
			if (genericArguments.Length != 2)
			{
				return false;
			}
			Type left = genericArguments[1];
			return left == typeof(object);
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x000C6FA4 File Offset: 0x000C51A4
		private static PSObject EncodeObjectDictionary(IDictionary dictionary)
		{
			Hashtable hashtable = new Hashtable();
			foreach (object obj in dictionary.Keys)
			{
				hashtable.Add(RemoteHostEncoder.EncodeObject(obj), RemoteHostEncoder.EncodeObjectWithType(dictionary[obj]));
			}
			return new PSObject(hashtable);
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x000C7018 File Offset: 0x000C5218
		private static IDictionary DecodeObjectDictionary(PSObject psObject, Type dictionaryType)
		{
			Type[] genericArguments = dictionaryType.GetGenericArguments();
			Type type = genericArguments[0];
			Type type2 = genericArguments[1];
			Hashtable hashtable = RemoteHostEncoder.SafelyGetBaseObject<Hashtable>(psObject);
			IDictionary dictionary = (IDictionary)Activator.CreateInstance(dictionaryType);
			foreach (object obj in hashtable.Keys)
			{
				dictionary.Add(RemoteHostEncoder.DecodeObject(obj, type), RemoteHostEncoder.DecodeObjectWithType(hashtable[obj]));
			}
			return dictionary;
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x000C70AC File Offset: 0x000C52AC
		private static T SafelyGetBaseObject<T>(PSObject psObject)
		{
			if (psObject == null || psObject.BaseObject == null || !(psObject.BaseObject is T))
			{
				throw RemoteHostExceptions.NewDecodingFailedException();
			}
			return (T)((object)psObject.BaseObject);
		}

		// Token: 0x06002354 RID: 9044 RVA: 0x000C70D7 File Offset: 0x000C52D7
		private static T SafelyCastObject<T>(object obj)
		{
			if (obj is T)
			{
				return (T)((object)obj);
			}
			throw RemoteHostExceptions.NewDecodingFailedException();
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x000C70F0 File Offset: 0x000C52F0
		private static T SafelyGetPropertyValue<T>(PSObject psObject, string key)
		{
			PSPropertyInfo pspropertyInfo = psObject.Properties[key];
			if (pspropertyInfo == null || pspropertyInfo.Value == null || !(pspropertyInfo.Value is T))
			{
				throw RemoteHostExceptions.NewDecodingFailedException();
			}
			return (T)((object)pspropertyInfo.Value);
		}
	}
}
