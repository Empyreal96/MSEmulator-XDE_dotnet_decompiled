using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace System.Management.Automation
{
	// Token: 0x0200081B RID: 2075
	internal sealed class ArgumentTypeConverterAttribute : ArgumentTransformationAttribute
	{
		// Token: 0x06004FBF RID: 20415 RVA: 0x001A7447 File Offset: 0x001A5647
		internal ArgumentTypeConverterAttribute(params Type[] types)
		{
			this._convertTypes = types;
		}

		// Token: 0x1700102B RID: 4139
		// (get) Token: 0x06004FC0 RID: 20416 RVA: 0x001A7456 File Offset: 0x001A5656
		internal Type TargetType
		{
			get
			{
				if (this._convertTypes != null)
				{
					return this._convertTypes.LastOrDefault<Type>();
				}
				return null;
			}
		}

		// Token: 0x06004FC1 RID: 20417 RVA: 0x001A746D File Offset: 0x001A566D
		public override object Transform(EngineIntrinsics engineIntrinsics, object inputData)
		{
			return this.Transform(engineIntrinsics, inputData, false, false);
		}

		// Token: 0x06004FC2 RID: 20418 RVA: 0x001A747C File Offset: 0x001A567C
		internal object Transform(EngineIntrinsics engineIntrinsics, object inputData, bool bindingParameters, bool bindingScriptCmdlet)
		{
			if (this._convertTypes == null)
			{
				return inputData;
			}
			object obj = inputData;
			try
			{
				for (int i = 0; i < this._convertTypes.Length; i++)
				{
					if (bindingParameters)
					{
						if (this._convertTypes[i].Equals(typeof(PSReference)))
						{
							PSObject psobject = obj as PSObject;
							object obj2;
							if (psobject != null)
							{
								obj2 = psobject.BaseObject;
							}
							else
							{
								obj2 = obj;
							}
							if (!(obj2 is PSReference))
							{
								throw new PSInvalidCastException("InvalidCastExceptionReferenceTypeExpected", null, ExtendedTypeSystem.ReferenceTypeExpected, new object[0]);
							}
						}
						else
						{
							PSObject psobject2 = obj as PSObject;
							object obj3;
							if (psobject2 != null)
							{
								obj3 = psobject2.BaseObject;
							}
							else
							{
								obj3 = obj;
							}
							PSReference psreference = obj3 as PSReference;
							if (psreference != null)
							{
								obj = psreference.Value;
							}
							if (bindingScriptCmdlet && this._convertTypes[i] == typeof(string))
							{
								obj3 = PSObject.Base(obj);
								if (obj3 != null && obj3.GetType().IsArray)
								{
									throw new PSInvalidCastException("InvalidCastFromAnyTypeToString", null, ExtendedTypeSystem.InvalidCastCannotRetrieveString, new object[0]);
								}
							}
						}
					}
					if (LanguagePrimitives.IsBoolOrSwitchParameterType(this._convertTypes[i]))
					{
						ArgumentTypeConverterAttribute.CheckBoolValue(obj, this._convertTypes[i]);
					}
					if (bindingScriptCmdlet)
					{
						ParameterCollectionTypeInformation parameterCollectionTypeInformation = new ParameterCollectionTypeInformation(this._convertTypes[i]);
						if (parameterCollectionTypeInformation.ParameterCollectionType != ParameterCollectionType.NotCollection && LanguagePrimitives.IsBoolOrSwitchParameterType(parameterCollectionTypeInformation.ElementType))
						{
							IList ilist = ParameterBinderBase.GetIList(obj);
							if (ilist != null)
							{
								using (IEnumerator enumerator = ilist.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										object value = enumerator.Current;
										ArgumentTypeConverterAttribute.CheckBoolValue(value, parameterCollectionTypeInformation.ElementType);
									}
									goto IL_18F;
								}
							}
							ArgumentTypeConverterAttribute.CheckBoolValue(obj, parameterCollectionTypeInformation.ElementType);
						}
					}
					IL_18F:
					obj = LanguagePrimitives.ConvertTo(obj, this._convertTypes[i], CultureInfo.InvariantCulture);
					if (!bindingScriptCmdlet && !bindingParameters && this._convertTypes[i] == typeof(ActionPreference))
					{
						ActionPreference actionPreference = (ActionPreference)obj;
						if (actionPreference == ActionPreference.Suspend)
						{
							throw new PSInvalidCastException("InvalidActionPreference", null, ErrorPackage.UnsupportedPreferenceVariable, new object[]
							{
								actionPreference
							});
						}
					}
				}
			}
			catch (PSInvalidCastException ex)
			{
				throw new ArgumentTransformationMetadataException(ex.Message, ex);
			}
			return obj;
		}

		// Token: 0x06004FC3 RID: 20419 RVA: 0x001A76DC File Offset: 0x001A58DC
		private static void CheckBoolValue(object value, Type boolType)
		{
			if (value != null)
			{
				Type type = value.GetType();
				if (type == typeof(PSObject))
				{
					type = ((PSObject)value).BaseObject.GetType();
				}
				if (!LanguagePrimitives.IsNumeric(type.GetTypeCode()) && !LanguagePrimitives.IsBoolOrSwitchParameterType(type))
				{
					ArgumentTypeConverterAttribute.ThrowPSInvalidBooleanArgumentCastException(type, boolType);
					return;
				}
			}
			else if ((!boolType.GetTypeInfo().IsGenericType || !(boolType.GetGenericTypeDefinition() == typeof(Nullable<>))) && LanguagePrimitives.IsBooleanType(boolType))
			{
				ArgumentTypeConverterAttribute.ThrowPSInvalidBooleanArgumentCastException(null, boolType);
			}
		}

		// Token: 0x06004FC4 RID: 20420 RVA: 0x001A776C File Offset: 0x001A596C
		internal static void ThrowPSInvalidBooleanArgumentCastException(Type resultType, Type convertType)
		{
			throw new PSInvalidCastException("InvalidCastExceptionUnsupportedParameterType", null, ExtendedTypeSystem.InvalidCastExceptionForBooleanArgumentValue, new object[]
			{
				resultType,
				convertType
			});
		}

		// Token: 0x040028D7 RID: 10455
		private Type[] _convertTypes;
	}
}
