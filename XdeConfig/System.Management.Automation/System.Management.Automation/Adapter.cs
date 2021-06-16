using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace System.Management.Automation
{
	// Token: 0x020000FF RID: 255
	internal abstract class Adapter
	{
		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000DF9 RID: 3577 RVA: 0x0004BBF3 File Offset: 0x00049DF3
		internal virtual bool SiteBinderCanOptimize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x0004BCEC File Offset: 0x00049EEC
		protected static IEnumerable<string> GetDotNetTypeNameHierarchy(Type type)
		{
			while (type != null)
			{
				yield return type.FullName;
				type = type.GetTypeInfo().BaseType;
			}
			yield break;
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x0004BD09 File Offset: 0x00049F09
		protected static IEnumerable<string> GetDotNetTypeNameHierarchy(object obj)
		{
			return Adapter.GetDotNetTypeNameHierarchy(obj.GetType());
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0004BD16 File Offset: 0x00049F16
		protected virtual IEnumerable<string> GetTypeNameHierarchy(object obj)
		{
			return Adapter.GetDotNetTypeNameHierarchy(obj);
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0004BD1E File Offset: 0x00049F1E
		protected virtual ConsolidatedString GetInternedTypeNameHierarchy(object obj)
		{
			return new ConsolidatedString(this.GetTypeNameHierarchy(obj));
		}

		// Token: 0x06000DFE RID: 3582
		protected abstract T GetMember<T>(object obj, string memberName) where T : PSMemberInfo;

		// Token: 0x06000DFF RID: 3583
		protected abstract PSMemberInfoInternalCollection<T> GetMembers<T>(object obj) where T : PSMemberInfo;

		// Token: 0x06000E00 RID: 3584
		protected abstract object PropertyGet(PSProperty property);

		// Token: 0x06000E01 RID: 3585
		protected abstract void PropertySet(PSProperty property, object setValue, bool convertIfPossible);

		// Token: 0x06000E02 RID: 3586
		protected abstract bool PropertyIsSettable(PSProperty property);

		// Token: 0x06000E03 RID: 3587
		protected abstract bool PropertyIsGettable(PSProperty property);

		// Token: 0x06000E04 RID: 3588
		protected abstract string PropertyType(PSProperty property, bool forDisplay);

		// Token: 0x06000E05 RID: 3589
		protected abstract string PropertyToString(PSProperty property);

		// Token: 0x06000E06 RID: 3590
		protected abstract AttributeCollection PropertyAttributes(PSProperty property);

		// Token: 0x06000E07 RID: 3591 RVA: 0x0004BD2C File Offset: 0x00049F2C
		protected virtual object MethodInvoke(PSMethod method, PSMethodInvocationConstraints invocationConstraints, object[] arguments)
		{
			return this.MethodInvoke(method, arguments);
		}

		// Token: 0x06000E08 RID: 3592
		protected abstract object MethodInvoke(PSMethod method, object[] arguments);

		// Token: 0x06000E09 RID: 3593
		protected abstract Collection<string> MethodDefinitions(PSMethod method);

		// Token: 0x06000E0A RID: 3594 RVA: 0x0004BD38 File Offset: 0x00049F38
		protected virtual string MethodToString(PSMethod method)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Collection<string> collection = this.MethodDefinitions(method);
			for (int i = 0; i < collection.Count; i++)
			{
				stringBuilder.Append(collection[i]);
				stringBuilder.Append(", ");
			}
			stringBuilder.Remove(stringBuilder.Length - 2, 2);
			return stringBuilder.ToString();
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x0004BD94 File Offset: 0x00049F94
		protected virtual string ParameterizedPropertyType(PSParameterizedProperty property)
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x0004BD9B File Offset: 0x00049F9B
		protected virtual bool ParameterizedPropertyIsSettable(PSParameterizedProperty property)
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x0004BDA2 File Offset: 0x00049FA2
		protected virtual bool ParameterizedPropertyIsGettable(PSParameterizedProperty property)
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x0004BDA9 File Offset: 0x00049FA9
		protected virtual Collection<string> ParameterizedPropertyDefinitions(PSParameterizedProperty property)
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x0004BDB0 File Offset: 0x00049FB0
		protected virtual object ParameterizedPropertyGet(PSParameterizedProperty property, object[] arguments)
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x0004BDB7 File Offset: 0x00049FB7
		protected virtual void ParameterizedPropertySet(PSParameterizedProperty property, object setValue, object[] arguments)
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x0004BDBE File Offset: 0x00049FBE
		protected virtual string ParameterizedPropertyToString(PSParameterizedProperty property)
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x0004BDC8 File Offset: 0x00049FC8
		private static Exception NewException(Exception e, string errorId, string targetErrorId, string resourceString, params object[] parameters)
		{
			object[] array = new object[parameters.Length + 1];
			for (int i = 0; i < parameters.Length; i++)
			{
				array[i + 1] = parameters[i];
			}
			Exception ex = e as TargetInvocationException;
			if (ex != null)
			{
				Exception ex2 = ex.InnerException ?? ex;
				array[0] = ex2.Message;
				return new ExtendedTypeSystemException(targetErrorId, ex2, resourceString, array);
			}
			array[0] = e.Message;
			return new ExtendedTypeSystemException(errorId, e, resourceString, array);
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x0004BE34 File Offset: 0x0004A034
		internal ConsolidatedString BaseGetTypeNameHierarchy(object obj)
		{
			ConsolidatedString internedTypeNameHierarchy;
			try
			{
				internedTypeNameHierarchy = this.GetInternedTypeNameHierarchy(obj);
			}
			catch (ExtendedTypeSystemException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw Adapter.NewException(e, "CatchFromBaseGetTypeNameHierarchy", "CatchFromBaseGetTypeNameHierarchyTI", ExtendedTypeSystem.ExceptionRetrievingTypeNameHierarchy, new object[0]);
			}
			return internedTypeNameHierarchy;
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x0004BE90 File Offset: 0x0004A090
		internal T BaseGetMember<T>(object obj, string memberName) where T : PSMemberInfo
		{
			T member;
			try
			{
				member = this.GetMember<T>(obj, memberName);
			}
			catch (ExtendedTypeSystemException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw Adapter.NewException(e, "CatchFromBaseGetMember", "CatchFromBaseGetMemberTI", ExtendedTypeSystem.ExceptionGettingMember, new object[]
				{
					memberName
				});
			}
			return member;
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0004BEF4 File Offset: 0x0004A0F4
		internal PSMemberInfoInternalCollection<T> BaseGetMembers<T>(object obj) where T : PSMemberInfo
		{
			PSMemberInfoInternalCollection<T> members;
			try
			{
				members = this.GetMembers<T>(obj);
			}
			catch (ExtendedTypeSystemException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw Adapter.NewException(e, "CatchFromBaseGetMembers", "CatchFromBaseGetMembersTI", ExtendedTypeSystem.ExceptionGettingMembers, new object[0]);
			}
			return members;
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x0004BF50 File Offset: 0x0004A150
		internal object BasePropertyGet(PSProperty property)
		{
			object result;
			try
			{
				result = this.PropertyGet(property);
			}
			catch (TargetInvocationException ex)
			{
				Exception ex2 = ex.InnerException ?? ex;
				throw new GetValueInvocationException("CatchFromBaseAdapterGetValueTI", ex2, ExtendedTypeSystem.ExceptionWhenGetting, new object[]
				{
					property.Name,
					ex2.Message
				});
			}
			catch (GetValueException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				CommandProcessorBase.CheckForSevereException(ex3);
				throw new GetValueInvocationException("CatchFromBaseAdapterGetValue", ex3, ExtendedTypeSystem.ExceptionWhenGetting, new object[]
				{
					property.Name,
					ex3.Message
				});
			}
			return result;
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x0004C008 File Offset: 0x0004A208
		internal void BasePropertySet(PSProperty property, object setValue, bool convert)
		{
			try
			{
				this.PropertySet(property, setValue, convert);
			}
			catch (TargetInvocationException ex)
			{
				Exception ex2 = ex.InnerException ?? ex;
				throw new SetValueInvocationException("CatchFromBaseAdapterSetValueTI", ex2, ExtendedTypeSystem.ExceptionWhenSetting, new object[]
				{
					property.Name,
					ex2.Message
				});
			}
			catch (SetValueException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				CommandProcessorBase.CheckForSevereException(ex3);
				throw new SetValueInvocationException("CatchFromBaseAdapterSetValue", ex3, ExtendedTypeSystem.ExceptionWhenSetting, new object[]
				{
					property.Name,
					ex3.Message
				});
			}
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x0004C0BC File Offset: 0x0004A2BC
		internal bool BasePropertyIsSettable(PSProperty property)
		{
			bool result;
			try
			{
				result = this.PropertyIsSettable(property);
			}
			catch (ExtendedTypeSystemException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw Adapter.NewException(e, "CatchFromBasePropertyIsSettable", "CatchFromBasePropertyIsSettableTI", ExtendedTypeSystem.ExceptionRetrievingPropertyWriteState, new object[]
				{
					property.Name
				});
			}
			return result;
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x0004C124 File Offset: 0x0004A324
		internal bool BasePropertyIsGettable(PSProperty property)
		{
			bool result;
			try
			{
				result = this.PropertyIsGettable(property);
			}
			catch (ExtendedTypeSystemException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw Adapter.NewException(e, "CatchFromBasePropertyIsGettable", "CatchFromBasePropertyIsGettableTI", ExtendedTypeSystem.ExceptionRetrievingPropertyReadState, new object[]
				{
					property.Name
				});
			}
			return result;
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x0004C18C File Offset: 0x0004A38C
		internal string BasePropertyType(PSProperty property)
		{
			string result;
			try
			{
				result = this.PropertyType(property, false);
			}
			catch (ExtendedTypeSystemException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw Adapter.NewException(e, "CatchFromBasePropertyType", "CatchFromBasePropertyTypeTI", ExtendedTypeSystem.ExceptionRetrievingPropertyType, new object[]
				{
					property.Name
				});
			}
			return result;
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0004C1F4 File Offset: 0x0004A3F4
		internal string BasePropertyToString(PSProperty property)
		{
			string result;
			try
			{
				result = this.PropertyToString(property);
			}
			catch (ExtendedTypeSystemException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw Adapter.NewException(e, "CatchFromBasePropertyToString", "CatchFromBasePropertyToStringTI", ExtendedTypeSystem.ExceptionRetrievingPropertyString, new object[]
				{
					property.Name
				});
			}
			return result;
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x0004C25C File Offset: 0x0004A45C
		internal AttributeCollection BasePropertyAttributes(PSProperty property)
		{
			AttributeCollection result;
			try
			{
				result = this.PropertyAttributes(property);
			}
			catch (ExtendedTypeSystemException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw Adapter.NewException(e, "CatchFromBasePropertyAttributes", "CatchFromBasePropertyAttributesTI", ExtendedTypeSystem.ExceptionRetrievingPropertyAttributes, new object[]
				{
					property.Name
				});
			}
			return result;
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x0004C2C4 File Offset: 0x0004A4C4
		internal object BaseMethodInvoke(PSMethod method, PSMethodInvocationConstraints invocationConstraints, params object[] arguments)
		{
			object result;
			try
			{
				result = this.MethodInvoke(method, invocationConstraints, arguments);
			}
			catch (TargetInvocationException ex)
			{
				Exception ex2 = ex.InnerException ?? ex;
				throw new MethodInvocationException("CatchFromBaseAdapterMethodInvokeTI", ex2, ExtendedTypeSystem.MethodInvocationException, new object[]
				{
					method.Name,
					arguments.Length,
					ex2.Message
				});
			}
			catch (FlowControlException)
			{
				throw;
			}
			catch (ScriptCallDepthException)
			{
				throw;
			}
			catch (PipelineStoppedException)
			{
				throw;
			}
			catch (MethodException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				CommandProcessorBase.CheckForSevereException(ex3);
				if (method.baseObject is SteppablePipeline && (method.Name.Equals("Begin", StringComparison.OrdinalIgnoreCase) || method.Name.Equals("Process", StringComparison.OrdinalIgnoreCase) || method.Name.Equals("End", StringComparison.OrdinalIgnoreCase)))
				{
					throw;
				}
				throw new MethodInvocationException("CatchFromBaseAdapterMethodInvoke", ex3, ExtendedTypeSystem.MethodInvocationException, new object[]
				{
					method.Name,
					arguments.Length,
					ex3.Message
				});
			}
			return result;
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x0004C40C File Offset: 0x0004A60C
		internal Collection<string> BaseMethodDefinitions(PSMethod method)
		{
			Collection<string> result;
			try
			{
				result = this.MethodDefinitions(method);
			}
			catch (ExtendedTypeSystemException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw Adapter.NewException(e, "CatchFromBaseMethodDefinitions", "CatchFromBaseMethodDefinitionsTI", ExtendedTypeSystem.ExceptionRetrievingMethodDefinitions, new object[]
				{
					method.Name
				});
			}
			return result;
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x0004C474 File Offset: 0x0004A674
		internal string BaseMethodToString(PSMethod method)
		{
			string result;
			try
			{
				result = this.MethodToString(method);
			}
			catch (ExtendedTypeSystemException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw Adapter.NewException(e, "CatchFromBaseMethodToString", "CatchFromBaseMethodToStringTI", ExtendedTypeSystem.ExceptionRetrievingMethodString, new object[]
				{
					method.Name
				});
			}
			return result;
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x0004C4DC File Offset: 0x0004A6DC
		internal string BaseParameterizedPropertyType(PSParameterizedProperty property)
		{
			string result;
			try
			{
				result = this.ParameterizedPropertyType(property);
			}
			catch (ExtendedTypeSystemException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw Adapter.NewException(e, "CatchFromBaseParameterizedPropertyType", "CatchFromBaseParameterizedPropertyTypeTI", ExtendedTypeSystem.ExceptionRetrievingParameterizedPropertytype, new object[]
				{
					property.Name
				});
			}
			return result;
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x0004C544 File Offset: 0x0004A744
		internal bool BaseParameterizedPropertyIsSettable(PSParameterizedProperty property)
		{
			bool result;
			try
			{
				result = this.ParameterizedPropertyIsSettable(property);
			}
			catch (ExtendedTypeSystemException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw Adapter.NewException(e, "CatchFromBaseParameterizedPropertyIsSettable", "CatchFromBaseParameterizedPropertyIsSettableTI", ExtendedTypeSystem.ExceptionRetrievingParameterizedPropertyWriteState, new object[]
				{
					property.Name
				});
			}
			return result;
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x0004C5AC File Offset: 0x0004A7AC
		internal bool BaseParameterizedPropertyIsGettable(PSParameterizedProperty property)
		{
			bool result;
			try
			{
				result = this.ParameterizedPropertyIsGettable(property);
			}
			catch (ExtendedTypeSystemException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw Adapter.NewException(e, "CatchFromBaseParameterizedPropertyIsGettable", "CatchFromBaseParameterizedPropertyIsGettableTI", ExtendedTypeSystem.ExceptionRetrievingParameterizedPropertyReadState, new object[]
				{
					property.Name
				});
			}
			return result;
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x0004C614 File Offset: 0x0004A814
		internal Collection<string> BaseParameterizedPropertyDefinitions(PSParameterizedProperty property)
		{
			Collection<string> result;
			try
			{
				result = this.ParameterizedPropertyDefinitions(property);
			}
			catch (ExtendedTypeSystemException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw Adapter.NewException(e, "CatchFromBaseParameterizedPropertyDefinitions", "CatchFromBaseParameterizedPropertyDefinitionsTI", ExtendedTypeSystem.ExceptionRetrievingParameterizedPropertyDefinitions, new object[]
				{
					property.Name
				});
			}
			return result;
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x0004C67C File Offset: 0x0004A87C
		internal object BaseParameterizedPropertyGet(PSParameterizedProperty property, params object[] arguments)
		{
			object result;
			try
			{
				result = this.ParameterizedPropertyGet(property, arguments);
			}
			catch (TargetInvocationException ex)
			{
				Exception ex2 = (ex.InnerException == null) ? ex : ex.InnerException;
				throw new GetValueInvocationException("CatchFromBaseAdapterParameterizedPropertyGetValueTI", ex2, ExtendedTypeSystem.ExceptionWhenGetting, new object[]
				{
					property.Name,
					ex2.Message
				});
			}
			catch (GetValueException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				CommandProcessorBase.CheckForSevereException(ex3);
				throw new GetValueInvocationException("CatchFromBaseParameterizedPropertyAdapterGetValue", ex3, ExtendedTypeSystem.ExceptionWhenGetting, new object[]
				{
					property.Name,
					ex3.Message
				});
			}
			return result;
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x0004C738 File Offset: 0x0004A938
		internal void BaseParameterizedPropertySet(PSParameterizedProperty property, object setValue, params object[] arguments)
		{
			try
			{
				this.ParameterizedPropertySet(property, setValue, arguments);
			}
			catch (TargetInvocationException ex)
			{
				Exception ex2 = ex.InnerException ?? ex;
				throw new SetValueInvocationException("CatchFromBaseAdapterParameterizedPropertySetValueTI", ex2, ExtendedTypeSystem.ExceptionWhenSetting, new object[]
				{
					property.Name,
					ex2.Message
				});
			}
			catch (SetValueException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				CommandProcessorBase.CheckForSevereException(ex3);
				throw new SetValueInvocationException("CatchFromBaseAdapterParameterizedPropertySetValue", ex3, ExtendedTypeSystem.ExceptionWhenSetting, new object[]
				{
					property.Name,
					ex3.Message
				});
			}
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x0004C7EC File Offset: 0x0004A9EC
		internal string BaseParameterizedPropertyToString(PSParameterizedProperty property)
		{
			string result;
			try
			{
				result = this.ParameterizedPropertyToString(property);
			}
			catch (ExtendedTypeSystemException)
			{
				throw;
			}
			catch (Exception e)
			{
				CommandProcessorBase.CheckForSevereException(e);
				throw Adapter.NewException(e, "CatchFromBaseParameterizedPropertyToString", "CatchFromBaseParameterizedPropertyToStringTI", ExtendedTypeSystem.ExceptionRetrievingParameterizedPropertyString, new object[]
				{
					property.Name
				});
			}
			return result;
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x0004C854 File Offset: 0x0004AA54
		private static Type GetArgumentType(object argument)
		{
			if (argument == null)
			{
				return typeof(LanguagePrimitives.Null);
			}
			PSReference psreference = argument as PSReference;
			if (psreference != null)
			{
				return Adapter.GetArgumentType(PSObject.Base(psreference.Value));
			}
			return argument.GetType();
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x0004C890 File Offset: 0x0004AA90
		internal static ConversionRank GetArgumentConversionRank(object argument, Type parameterType)
		{
			Type argumentType = Adapter.GetArgumentType(argument);
			ConversionRank conversionRank = LanguagePrimitives.GetConversionRank(argumentType, parameterType);
			if (conversionRank == ConversionRank.None)
			{
				argumentType = Adapter.GetArgumentType(PSObject.Base(argument));
				conversionRank = LanguagePrimitives.GetConversionRank(argumentType, parameterType);
			}
			return conversionRank;
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x0004C8C4 File Offset: 0x0004AAC4
		private static ParameterInformation[] ExpandParameters(int argCount, ParameterInformation[] parameters, Type elementType)
		{
			ParameterInformation[] array = new ParameterInformation[argCount];
			ParameterInformation parameterInformation = parameters[parameters.Length - 1];
			Array.Copy(parameters, array, parameters.Length - 1);
			for (int i = parameters.Length - 1; i < argCount; i++)
			{
				array[i] = new ParameterInformation(elementType, false, null, false);
			}
			return array;
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x0004C90C File Offset: 0x0004AB0C
		private static int CompareOverloadCandidates(Adapter.OverloadCandidate candidate1, Adapter.OverloadCandidate candidate2, object[] arguments)
		{
			ParameterInformation[] array = candidate1.expandedParameters ?? candidate1.parameters;
			ParameterInformation[] array2 = candidate2.expandedParameters ?? candidate2.parameters;
			int num = 0;
			int num2 = candidate1.conversionRanks.Length;
			int i = 0;
			while (i < candidate1.conversionRanks.Length)
			{
				if (candidate1.conversionRanks[i] < candidate2.conversionRanks[i])
				{
					num -= num2;
				}
				else if (candidate1.conversionRanks[i] > candidate2.conversionRanks[i])
				{
					num += num2;
				}
				else if (candidate1.conversionRanks[i] == ConversionRank.UnrelatedArrays)
				{
					Type elementType = Adapter.EffectiveArgumentType(arguments[i]).GetElementType();
					ConversionRank conversionRank = LanguagePrimitives.GetConversionRank(elementType, array[i].parameterType.GetElementType());
					ConversionRank conversionRank2 = LanguagePrimitives.GetConversionRank(elementType, array2[i].parameterType.GetElementType());
					if (conversionRank < conversionRank2)
					{
						num -= num2;
					}
					else if (conversionRank > conversionRank2)
					{
						num += num2;
					}
				}
				i++;
				num2--;
			}
			if (num == 0)
			{
				num2 = candidate1.conversionRanks.Length;
				int j = 0;
				while (j < candidate1.conversionRanks.Length)
				{
					ConversionRank conversionRank3 = candidate1.conversionRanks[j];
					ConversionRank conversionRank4 = candidate2.conversionRanks[j];
					if (conversionRank3 >= ConversionRank.NullToValue && conversionRank4 >= ConversionRank.NullToValue && conversionRank3 >= ConversionRank.NumericImplicit == conversionRank4 >= ConversionRank.NumericImplicit)
					{
						if (conversionRank3 >= ConversionRank.NumericImplicit)
						{
							num2 = -num2;
						}
						conversionRank3 = LanguagePrimitives.GetConversionRank(array[j].parameterType, array2[j].parameterType);
						conversionRank4 = LanguagePrimitives.GetConversionRank(array2[j].parameterType, array[j].parameterType);
						if (conversionRank3 < conversionRank4)
						{
							num += num2;
						}
						else if (conversionRank3 > conversionRank4)
						{
							num -= num2;
						}
					}
					j++;
					num2 = Math.Abs(num2) - 1;
				}
			}
			if (num == 0)
			{
				for (int k = 0; k < candidate1.conversionRanks.Length; k++)
				{
					if (array[k].parameterType != array2[k].parameterType)
					{
						return 0;
					}
				}
			}
			if (num == 0)
			{
				if (candidate1.expandedParameters != null && candidate2.expandedParameters != null)
				{
					if (candidate1.parameters.Length <= candidate2.parameters.Length)
					{
						return -1;
					}
					return 1;
				}
				else
				{
					if (candidate1.expandedParameters != null)
					{
						return -1;
					}
					if (candidate2.expandedParameters != null)
					{
						return 1;
					}
				}
			}
			if (num == 0)
			{
				num = Adapter.CompareTypeSpecificity(candidate1, candidate2);
			}
			if (num == 0)
			{
				if (candidate1.parameters.Length < candidate2.parameters.Length)
				{
					return 1;
				}
				if (candidate1.parameters.Length > candidate2.parameters.Length)
				{
					return -1;
				}
			}
			return num;
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x0004CB70 File Offset: 0x0004AD70
		private static Adapter.OverloadCandidate FindBestCandidate(List<Adapter.OverloadCandidate> candidates, object[] arguments)
		{
			Adapter.OverloadCandidate overloadCandidate = null;
			bool flag = false;
			for (int i = 0; i < candidates.Count; i++)
			{
				Adapter.OverloadCandidate overloadCandidate2 = candidates[i];
				if (overloadCandidate == null)
				{
					overloadCandidate = overloadCandidate2;
				}
				else
				{
					int num = Adapter.CompareOverloadCandidates(overloadCandidate, overloadCandidate2, arguments);
					if (num == 0)
					{
						flag = true;
					}
					else if (num < 0)
					{
						overloadCandidate = overloadCandidate2;
						flag = false;
					}
				}
			}
			if (!flag)
			{
				return overloadCandidate;
			}
			return null;
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x0004CBD8 File Offset: 0x0004ADD8
		private static Adapter.OverloadCandidate FindBestCandidate(List<Adapter.OverloadCandidate> candidates, object[] arguments, PSMethodInvocationConstraints invocationConstraints)
		{
			List<Adapter.OverloadCandidate> list = (from candidate in candidates
			where Adapter.IsInvocationConstraintSatisfied(candidate, invocationConstraints)
			select candidate).ToList<Adapter.OverloadCandidate>();
			if (list.Count > 0)
			{
				candidates = list;
			}
			return Adapter.FindBestCandidate(candidates, arguments);
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x0004CC20 File Offset: 0x0004AE20
		private static int CompareTypeSpecificity(Type type1, Type type2)
		{
			if (type1.IsGenericParameter || type2.IsGenericParameter)
			{
				int num = 0;
				if (type1.IsGenericParameter)
				{
					num--;
				}
				if (type2.IsGenericParameter)
				{
					num++;
				}
				return num;
			}
			if (type1.IsArray)
			{
				return Adapter.CompareTypeSpecificity(type1.GetElementType(), type2.GetElementType());
			}
			if (type1.GetTypeInfo().IsGenericType)
			{
				return Adapter.CompareTypeSpecificity(type1.GetGenericArguments(), type2.GetGenericArguments());
			}
			return 0;
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x0004CC94 File Offset: 0x0004AE94
		private static int CompareTypeSpecificity(Type[] params1, Type[] params2)
		{
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < params1.Length; i++)
			{
				int num = Adapter.CompareTypeSpecificity(params1[i], params2[i]);
				if (num > 0)
				{
					flag = true;
				}
				else if (num < 0)
				{
					flag2 = true;
				}
				if (flag && flag2)
				{
					break;
				}
			}
			if (flag && !flag2)
			{
				return 1;
			}
			if (flag2 && !flag)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x0004CCF4 File Offset: 0x0004AEF4
		private static int CompareTypeSpecificity(Adapter.OverloadCandidate candidate1, Adapter.OverloadCandidate candidate2)
		{
			if (!candidate1.method.isGeneric && !candidate2.method.isGeneric)
			{
				return 0;
			}
			Type[] @params = (from p in Adapter.GetGenericMethodDefinitionIfPossible(candidate1.method.method).GetParameters()
			select p.ParameterType).ToArray<Type>();
			Type[] params2 = (from p in Adapter.GetGenericMethodDefinitionIfPossible(candidate2.method.method).GetParameters()
			select p.ParameterType).ToArray<Type>();
			return Adapter.CompareTypeSpecificity(@params, params2);
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x0004CDA0 File Offset: 0x0004AFA0
		private static MethodBase GetGenericMethodDefinitionIfPossible(MethodBase method)
		{
			if (method.IsGenericMethod && !method.IsGenericMethodDefinition)
			{
				MethodInfo methodInfo = method as MethodInfo;
				if (methodInfo != null)
				{
					return methodInfo.GetGenericMethodDefinition();
				}
			}
			return method;
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x0004CDD8 File Offset: 0x0004AFD8
		private static bool IsInvocationTargetConstraintSatisfied(MethodInformation method, PSMethodInvocationConstraints invocationConstraints)
		{
			if (method.method == null)
			{
				return true;
			}
			Type declaringType = method.method.DeclaringType;
			TypeInfo typeInfo = declaringType.GetTypeInfo();
			if (invocationConstraints == null || invocationConstraints.MethodTargetType == null)
			{
				return !typeInfo.IsInterface;
			}
			Type methodTargetType = invocationConstraints.MethodTargetType;
			TypeInfo typeInfo2 = methodTargetType.GetTypeInfo();
			if (typeInfo2.IsInterface)
			{
				return declaringType == methodTargetType || (typeInfo.IsInterface && methodTargetType.IsSubclassOf(declaringType));
			}
			return !typeInfo.IsInterface && (methodTargetType.IsAssignableFrom(declaringType) || (typeInfo2.IsArray && declaringType == typeof(Array)));
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x0004CE88 File Offset: 0x0004B088
		private static bool IsInvocationConstraintSatisfied(Adapter.OverloadCandidate overloadCandidate, PSMethodInvocationConstraints invocationConstraints)
		{
			if (invocationConstraints == null)
			{
				return true;
			}
			if (invocationConstraints.ParameterTypes != null)
			{
				int num = 0;
				foreach (Type type in invocationConstraints.ParameterTypes)
				{
					if (type != null)
					{
						if (num >= overloadCandidate.parameters.Length)
						{
							return false;
						}
						Type parameterType = overloadCandidate.parameters[num].parameterType;
						if (parameterType != type)
						{
							return false;
						}
					}
					num++;
				}
				return true;
			}
			return true;
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x0004CF24 File Offset: 0x0004B124
		internal static MethodInformation FindBestMethod(MethodInformation[] methods, PSMethodInvocationConstraints invocationConstraints, object[] arguments, ref string errorId, ref string errorMsg, out bool expandParamsOnBest, out bool callNonVirtually)
		{
			callNonVirtually = false;
			MethodInformation methodInformation = Adapter.FindBestMethodImpl(methods, invocationConstraints, arguments, ref errorId, ref errorMsg, out expandParamsOnBest);
			if (methodInformation == null)
			{
				return null;
			}
			if (invocationConstraints != null && invocationConstraints.MethodTargetType != null && methodInformation.method != null && methodInformation.method.DeclaringType != null)
			{
				Type declaringType = methodInformation.method.DeclaringType;
				if (declaringType != invocationConstraints.MethodTargetType && declaringType.IsSubclassOf(invocationConstraints.MethodTargetType))
				{
					Type[] types = (from parameter in methodInformation.method.GetParameters()
					select parameter.ParameterType).ToArray<Type>();
					MethodInfo method = invocationConstraints.MethodTargetType.GetMethod(methodInformation.method.Name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, types, null);
					if (method != null && (method.IsPublic || method.IsFamily))
					{
						methodInformation = new MethodInformation(method, 0);
						callNonVirtually = true;
					}
				}
			}
			return methodInformation;
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x0004D05C File Offset: 0x0004B25C
		private static MethodInformation FindBestMethodImpl(MethodInformation[] methods, PSMethodInvocationConstraints invocationConstraints, object[] arguments, ref string errorId, ref string errorMsg, out bool expandParamsOnBest)
		{
			expandParamsOnBest = false;
			if (methods.Length == 1 && !methods[0].hasVarArgs && !methods[0].isGeneric && (methods[0].method == null || !methods[0].method.DeclaringType.GetTypeInfo().IsGenericTypeDefinition) && methods[0].parameters.Length == arguments.Length)
			{
				return methods[0];
			}
			Type[] array = arguments.Select(new Func<object, Type>(Adapter.EffectiveArgumentType)).ToArray<Type>();
			List<Adapter.OverloadCandidate> list = new List<Adapter.OverloadCandidate>();
			foreach (MethodInformation methodInformation in methods)
			{
				if (!(methodInformation.method != null) || !methodInformation.method.DeclaringType.GetTypeInfo().IsGenericTypeDefinition)
				{
					if (methodInformation.isGeneric)
					{
						Type[] array2 = new Type[array.Length];
						Array.Copy(array, array2, array.Length);
						if (invocationConstraints != null && invocationConstraints.ParameterTypes != null)
						{
							int num = 0;
							foreach (Type type in invocationConstraints.ParameterTypes)
							{
								if (type != null)
								{
									array2[num] = type;
								}
								num++;
							}
						}
						methodInformation = TypeInference.Infer(methodInformation, array2);
						if (methodInformation == null)
						{
							goto IL_310;
						}
					}
					if (Adapter.IsInvocationTargetConstraintSatisfied(methodInformation, invocationConstraints))
					{
						ParameterInformation[] parameters = methodInformation.parameters;
						if (arguments.Length != parameters.Length)
						{
							if (arguments.Length > parameters.Length)
							{
								if (!methodInformation.hasVarArgs)
								{
									goto IL_310;
								}
							}
							else
							{
								if (!methodInformation.hasOptional && (!methodInformation.hasVarArgs || arguments.Length + 1 != parameters.Length))
								{
									goto IL_310;
								}
								if (methodInformation.hasOptional)
								{
									int num2 = 0;
									for (int j = 0; j < parameters.Length; j++)
									{
										if (parameters[j].isOptional)
										{
											num2++;
										}
									}
									if (arguments.Length + num2 < parameters.Length)
									{
										goto IL_310;
									}
								}
							}
						}
						Adapter.OverloadCandidate overloadCandidate = new Adapter.OverloadCandidate(methodInformation, arguments.Length);
						int num3 = 0;
						while (overloadCandidate != null && num3 < parameters.Length)
						{
							ParameterInformation parameterInformation = parameters[num3];
							if (parameterInformation.isOptional && arguments.Length <= num3)
							{
								break;
							}
							if (parameterInformation.isParamArray)
							{
								Type elementType = parameterInformation.parameterType.GetElementType();
								if (parameters.Length == arguments.Length)
								{
									ConversionRank argumentConversionRank = Adapter.GetArgumentConversionRank(arguments[num3], parameterInformation.parameterType);
									ConversionRank argumentConversionRank2 = Adapter.GetArgumentConversionRank(arguments[num3], elementType);
									if (argumentConversionRank2 > argumentConversionRank)
									{
										overloadCandidate.expandedParameters = Adapter.ExpandParameters(arguments.Length, parameters, elementType);
										overloadCandidate.conversionRanks[num3] = argumentConversionRank2;
									}
									else
									{
										overloadCandidate.conversionRanks[num3] = argumentConversionRank;
									}
									if (overloadCandidate.conversionRanks[num3] == ConversionRank.None)
									{
										overloadCandidate = null;
									}
								}
								else
								{
									for (int k = num3; k < arguments.Length; k++)
									{
										overloadCandidate.conversionRanks[k] = Adapter.GetArgumentConversionRank(arguments[k], elementType);
										if (overloadCandidate.conversionRanks[k] == ConversionRank.None)
										{
											overloadCandidate = null;
											break;
										}
									}
									if (overloadCandidate != null)
									{
										overloadCandidate.expandedParameters = Adapter.ExpandParameters(arguments.Length, parameters, elementType);
									}
								}
							}
							else
							{
								overloadCandidate.conversionRanks[num3] = Adapter.GetArgumentConversionRank(arguments[num3], parameterInformation.parameterType);
								if (overloadCandidate.conversionRanks[num3] == ConversionRank.None)
								{
									overloadCandidate = null;
								}
							}
							num3++;
						}
						if (overloadCandidate != null)
						{
							list.Add(overloadCandidate);
						}
					}
				}
				IL_310:;
			}
			if (list.Count == 0)
			{
				if (methods.Length > 0)
				{
					if (methods.All((MethodInformation m) => m.method != null && m.method.DeclaringType.GetTypeInfo().IsGenericTypeDefinition && m.method.IsStatic))
					{
						errorId = "CannotInvokeStaticMethodOnUninstantiatedGenericType";
						errorMsg = string.Format(CultureInfo.InvariantCulture, ExtendedTypeSystem.CannotInvokeStaticMethodOnUninstantiatedGenericType, new object[]
						{
							methods[0].method.DeclaringType.FullName
						});
						return null;
					}
				}
				errorId = "MethodCountCouldNotFindBest";
				errorMsg = ExtendedTypeSystem.MethodArgumentCountException;
				return null;
			}
			Adapter.OverloadCandidate overloadCandidate2 = (list.Count == 1) ? list[0] : Adapter.FindBestCandidate(list, arguments, invocationConstraints);
			if (overloadCandidate2 != null)
			{
				expandParamsOnBest = (overloadCandidate2.expandedParameters != null);
				return overloadCandidate2.method;
			}
			errorId = "MethodCountCouldNotFindBest";
			errorMsg = ExtendedTypeSystem.MethodAmbiguousException;
			return null;
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x0004D460 File Offset: 0x0004B660
		internal static Type EffectiveArgumentType(object arg)
		{
			if (arg != null)
			{
				arg = PSObject.Base(arg);
				object[] array = arg as object[];
				if (array != null && array.Length > 0 && PSObject.Base(array[0]) != null)
				{
					Type type = PSObject.Base(array[0]).GetType();
					bool flag = true;
					for (int i = 1; i < array.Length; i++)
					{
						if (array[i] == null || type != PSObject.Base(array[i]).GetType())
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return type.MakeArrayType();
					}
				}
				return arg.GetType();
			}
			return typeof(LanguagePrimitives.Null);
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x0004D4EC File Offset: 0x0004B6EC
		internal static void SetReferences(object[] arguments, MethodInformation methodInformation, object[] originalArguments)
		{
			using (PSObject.memberResolution.TraceScope("Checking for possible references.", new object[0]))
			{
				ParameterInformation[] parameters = methodInformation.parameters;
				int num = 0;
				while (num < originalArguments.Length && num < parameters.Length && num < arguments.Length)
				{
					object obj = originalArguments[num];
					PSReference psreference = obj as PSReference;
					if (psreference != null)
					{
						goto IL_4C;
					}
					PSObject psobject = obj as PSObject;
					if (psobject != null)
					{
						psreference = (psobject.BaseObject as PSReference);
						if (psreference != null)
						{
							goto IL_4C;
						}
					}
					IL_92:
					num++;
					continue;
					IL_4C:
					ParameterInformation parameterInformation = parameters[num];
					if (parameterInformation.isByRef)
					{
						object obj2 = arguments[num];
						PSObject.memberResolution.WriteLine("Argument '{0}' was a reference so it will be set to \"{1}\".", new object[]
						{
							num + 1,
							obj2
						});
						psreference.Value = obj2;
						goto IL_92;
					}
					goto IL_92;
				}
			}
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x0004D5C4 File Offset: 0x0004B7C4
		internal static MethodInformation GetBestMethodAndArguments(string methodName, MethodInformation[] methods, object[] arguments, out object[] newArguments)
		{
			return Adapter.GetBestMethodAndArguments(methodName, methods, null, arguments, out newArguments);
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x0004D5D0 File Offset: 0x0004B7D0
		internal static MethodInformation GetBestMethodAndArguments(string methodName, MethodInformation[] methods, PSMethodInvocationConstraints invocationConstraints, object[] arguments, out object[] newArguments)
		{
			string errorId = null;
			string resourceString = null;
			bool expandParamsOnBest;
			bool flag;
			MethodInformation methodInformation = Adapter.FindBestMethod(methods, invocationConstraints, arguments, ref errorId, ref resourceString, out expandParamsOnBest, out flag);
			if (methodInformation == null)
			{
				throw new MethodException(errorId, null, resourceString, new object[]
				{
					methodName,
					arguments.Length
				});
			}
			newArguments = Adapter.GetMethodArgumentsBase(methodName, methodInformation.parameters, arguments, expandParamsOnBest);
			return methodInformation;
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x0004D630 File Offset: 0x0004B830
		internal static object[] GetMethodArgumentsBase(string methodName, ParameterInformation[] parameters, object[] arguments, bool expandParamsOnBest)
		{
			int num = parameters.Length;
			if (num == 0)
			{
				return new object[0];
			}
			object[] array = new object[num];
			for (int i = 0; i < num - 1; i++)
			{
				ParameterInformation parameter = parameters[i];
				Adapter.SetNewArgument(methodName, arguments, array, parameter, i);
			}
			ParameterInformation parameterInformation = parameters[num - 1];
			if (!expandParamsOnBest)
			{
				Adapter.SetNewArgument(methodName, arguments, array, parameterInformation, num - 1);
				return array;
			}
			if (arguments.Length < num)
			{
				object[] array2 = array;
				int num2 = num - 1;
				Type elementType = parameterInformation.parameterType.GetElementType();
				int[] lengths = new int[1];
				array2[num2] = Array.CreateInstance(elementType, lengths);
				return array;
			}
			int num3 = arguments.Length - num + 1;
			if (num3 == 1 && arguments[arguments.Length - 1] == null)
			{
				array[num - 1] = null;
			}
			else
			{
				object[] array3 = new object[num3];
				Type elementType2 = parameterInformation.parameterType.GetElementType();
				for (int j = 0; j < num3; j++)
				{
					int num4 = j + num - 1;
					try
					{
						array3[j] = Adapter.MethodArgumentConvertTo(arguments[num4], false, num4, elementType2, CultureInfo.InvariantCulture);
					}
					catch (InvalidCastException ex)
					{
						throw new MethodException("MethodArgumentConversionInvalidCastArgument", ex, ExtendedTypeSystem.MethodArgumentConversionException, new object[]
						{
							num4,
							arguments[num4],
							methodName,
							elementType2,
							ex.Message
						});
					}
				}
				try
				{
					array[num - 1] = Adapter.MethodArgumentConvertTo(array3, parameterInformation.isByRef, num - 1, parameterInformation.parameterType, CultureInfo.InvariantCulture);
				}
				catch (InvalidCastException ex2)
				{
					throw new MethodException("MethodArgumentConversionParamsConversion", ex2, ExtendedTypeSystem.MethodArgumentConversionException, new object[]
					{
						num - 1,
						array3,
						methodName,
						parameterInformation.parameterType,
						ex2.Message
					});
				}
			}
			return array;
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x0004D7F0 File Offset: 0x0004B9F0
		internal static void SetNewArgument(string methodName, object[] arguments, object[] newArguments, ParameterInformation parameter, int index)
		{
			if (arguments.Length > index)
			{
				try
				{
					newArguments[index] = Adapter.MethodArgumentConvertTo(arguments[index], parameter.isByRef, index, parameter.parameterType, CultureInfo.InvariantCulture);
					return;
				}
				catch (InvalidCastException ex)
				{
					throw new MethodException("MethodArgumentConversionInvalidCastArgument", ex, ExtendedTypeSystem.MethodArgumentConversionException, new object[]
					{
						index,
						arguments[index],
						methodName,
						parameter.parameterType,
						ex.Message
					});
				}
			}
			newArguments[index] = parameter.defaultValue;
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x0004D884 File Offset: 0x0004BA84
		internal static object MethodArgumentConvertTo(object valueToConvert, bool isParameterByRef, int parameterIndex, Type resultType, IFormatProvider formatProvider)
		{
			object result;
			using (PSObject.memberResolution.TraceScope("Method argument conversion.", new object[0]))
			{
				if (resultType == null)
				{
					throw PSTraceSource.NewArgumentNullException("resultType");
				}
				bool flag;
				valueToConvert = Adapter.UnReference(valueToConvert, out flag);
				if (isParameterByRef && !flag)
				{
					throw new MethodException("NonRefArgumentToRefParameterMsg", null, ExtendedTypeSystem.NonRefArgumentToRefParameter, new object[]
					{
						parameterIndex + 1,
						typeof(PSReference).FullName,
						"[ref]"
					});
				}
				if (flag && !isParameterByRef)
				{
					throw new MethodException("RefArgumentToNonRefParameterMsg", null, ExtendedTypeSystem.RefArgumentToNonRefParameter, new object[]
					{
						parameterIndex + 1,
						typeof(PSReference).FullName,
						"[ref]"
					});
				}
				result = Adapter.PropertySetAndMethodArgumentConvertTo(valueToConvert, resultType, formatProvider);
			}
			return result;
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x0004D97C File Offset: 0x0004BB7C
		internal static object UnReference(object obj, out bool isArgumentByRef)
		{
			isArgumentByRef = false;
			PSReference psreference = obj as PSReference;
			if (psreference != null)
			{
				PSObject.memberResolution.WriteLine("Parameter was a reference.", new object[0]);
				isArgumentByRef = true;
				return psreference.Value;
			}
			PSObject psobject = obj as PSObject;
			if (psobject != null)
			{
				psreference = (psobject.BaseObject as PSReference);
			}
			if (psreference != null)
			{
				PSObject.memberResolution.WriteLine("Parameter was an PSObject containing a reference.", new object[0]);
				isArgumentByRef = true;
				return psreference.Value;
			}
			return obj;
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x0004D9F0 File Offset: 0x0004BBF0
		internal static object PropertySetAndMethodArgumentConvertTo(object valueToConvert, Type resultType, IFormatProvider formatProvider)
		{
			object result;
			using (PSObject.memberResolution.TraceScope("Converting parameter \"{0}\" to \"{1}\".", new object[]
			{
				valueToConvert,
				resultType
			}))
			{
				if (resultType == null)
				{
					throw PSTraceSource.NewArgumentNullException("resultType");
				}
				PSObject psobject = valueToConvert as PSObject;
				if (psobject != null && resultType == typeof(object))
				{
					PSObject.memberResolution.WriteLine("Parameter was an PSObject and will be converted to System.Object.", new object[0]);
					result = PSObject.Base(psobject);
				}
				else
				{
					result = LanguagePrimitives.ConvertTo(valueToConvert, resultType, formatProvider);
				}
			}
			return result;
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x0004DA94 File Offset: 0x0004BC94
		internal static void DoBoxingIfNecessary(ILGenerator generator, Type type)
		{
			TypeInfo typeInfo = null;
			if (type.IsByRef)
			{
				type = type.GetElementType();
				typeInfo = type.GetTypeInfo();
				if (typeInfo.IsPrimitive)
				{
					if (type == typeof(byte))
					{
						generator.Emit(OpCodes.Ldind_U1);
					}
					else if (type == typeof(ushort))
					{
						generator.Emit(OpCodes.Ldind_U2);
					}
					else if (type == typeof(uint))
					{
						generator.Emit(OpCodes.Ldind_U4);
					}
					else if (type == typeof(sbyte))
					{
						generator.Emit(OpCodes.Ldind_I8);
					}
					else if (type == typeof(short))
					{
						generator.Emit(OpCodes.Ldind_I2);
					}
					else if (type == typeof(int))
					{
						generator.Emit(OpCodes.Ldind_I4);
					}
					else if (type == typeof(long))
					{
						generator.Emit(OpCodes.Ldind_I8);
					}
					else if (type == typeof(float))
					{
						generator.Emit(OpCodes.Ldind_R4);
					}
					else if (type == typeof(double))
					{
						generator.Emit(OpCodes.Ldind_R8);
					}
				}
				else if (typeInfo.IsValueType)
				{
					generator.Emit(OpCodes.Ldobj, type);
				}
				else
				{
					generator.Emit(OpCodes.Ldind_Ref);
				}
			}
			else if (type.IsPointer)
			{
				MethodInfo method = typeof(Pointer).GetMethod("Box");
				MethodInfo method2 = typeof(Type).GetMethod("GetTypeFromHandle");
				generator.Emit(OpCodes.Ldtoken, type);
				generator.Emit(OpCodes.Call, method2);
				generator.Emit(OpCodes.Call, method);
			}
			typeInfo = (typeInfo ?? type.GetTypeInfo());
			if (typeInfo.IsValueType)
			{
				generator.Emit(OpCodes.Box, type);
			}
		}

		// Token: 0x04000641 RID: 1601
		[TraceSource("ETS", "Extended Type System")]
		protected static PSTraceSource tracer = PSTraceSource.GetTracer("ETS", "Extended Type System");

		// Token: 0x02000100 RID: 256
		[DebuggerDisplay("OverloadCandidate: {method.methodDefinition}")]
		private class OverloadCandidate
		{
			// Token: 0x06000E45 RID: 3653 RVA: 0x0004DCB1 File Offset: 0x0004BEB1
			internal OverloadCandidate(MethodInformation method, int argCount)
			{
				this.method = method;
				this.parameters = method.parameters;
				this.conversionRanks = new ConversionRank[argCount];
			}

			// Token: 0x04000646 RID: 1606
			internal MethodInformation method;

			// Token: 0x04000647 RID: 1607
			internal ParameterInformation[] parameters;

			// Token: 0x04000648 RID: 1608
			internal ParameterInformation[] expandedParameters;

			// Token: 0x04000649 RID: 1609
			internal ConversionRank[] conversionRanks;
		}
	}
}
