using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Management.Automation.Language
{
	// Token: 0x02000611 RID: 1553
	internal class PSAttributeGenerator : CreateInstanceBinder
	{
		// Token: 0x0600437F RID: 17279 RVA: 0x00163408 File Offset: 0x00161608
		internal static PSAttributeGenerator Get(CallInfo callInfo)
		{
			PSAttributeGenerator result;
			lock (PSAttributeGenerator._binderCache)
			{
				PSAttributeGenerator psattributeGenerator;
				if (!PSAttributeGenerator._binderCache.TryGetValue(callInfo, out psattributeGenerator))
				{
					psattributeGenerator = new PSAttributeGenerator(callInfo);
					PSAttributeGenerator._binderCache.Add(callInfo, psattributeGenerator);
				}
				result = psattributeGenerator;
			}
			return result;
		}

		// Token: 0x06004380 RID: 17280 RVA: 0x00163468 File Offset: 0x00161668
		private PSAttributeGenerator(CallInfo callInfo) : base(callInfo)
		{
		}

		// Token: 0x06004381 RID: 17281 RVA: 0x0016347C File Offset: 0x0016167C
		public override DynamicMetaObject FallbackCreateInstance(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
		{
			Type type = (Type)target.Value;
			ConstructorInfo[] constructors = type.GetConstructors();
			MethodInformation[] methodInformationArray = DotNetAdapter.GetMethodInformationArray(constructors);
			target = new DynamicMetaObject(target.Expression, BindingRestrictions.GetInstanceRestriction(target.Expression, target.Value), target.Value);
			string value = null;
			string value2 = null;
			int num = base.CallInfo.ArgumentCount - base.CallInfo.ArgumentNames.Count;
			bool flag;
			bool flag2;
			MethodInformation methodInformation = Adapter.FindBestMethod(methodInformationArray, null, (from arg in args.Take(num)
			select arg.Value).ToArray<object>(), ref value, ref value2, out flag, out flag2);
			if (methodInformation == null)
			{
				return errorSuggestion ?? new DynamicMetaObject(Expression.Throw(Expression.New(CachedReflectionInfo.MethodException_ctor, new Expression[]
				{
					Expression.Constant(value),
					Expression.Constant(null, typeof(Exception)),
					Expression.Constant(value2),
					Expression.NewArrayInit(typeof(object), new Expression[]
					{
						Expression.Constant(".ctor").Cast(typeof(object)),
						ExpressionCache.Constant(num).Cast(typeof(object))
					})
				}), this.ReturnType), target.CombineRestrictions(args));
			}
			ConstructorInfo constructorInfo = (ConstructorInfo)methodInformation.method;
			ParameterInfo[] parameters = constructorInfo.GetParameters();
			Expression[] array = new Expression[parameters.Length];
			int i;
			for (i = 0; i < parameters.Length; i++)
			{
				Type parameterType = parameters[i].ParameterType;
				object[] customAttributes = parameters[i].GetCustomAttributes(typeof(ParamArrayAttribute), true);
				if (customAttributes != null && customAttributes.Any<object>() && flag)
				{
					Type elementType = parameters[i].ParameterType.GetElementType();
					List<Expression> list = new List<Expression>();
					int num2 = i;
					int j = i;
					while (j < num)
					{
						bool debase;
						LanguagePrimitives.ConversionData conversion = LanguagePrimitives.FigureConversion(args[i].Value, elementType, out debase);
						list.Add(PSConvertBinder.InvokeConverter(conversion, args[j].Expression, elementType, debase, ExpressionCache.InvariantCulture));
						j++;
						i++;
					}
					array[num2] = Expression.NewArrayInit(elementType, list);
					break;
				}
				bool debase2;
				LanguagePrimitives.ConversionData conversion2 = LanguagePrimitives.FigureConversion(args[i].Value, parameterType, out debase2);
				array[i] = PSConvertBinder.InvokeConverter(conversion2, args[i].Expression, parameterType, debase2, ExpressionCache.InvariantCulture);
			}
			Expression expression = Expression.New(constructorInfo, array);
			if (base.CallInfo.ArgumentNames.Any<string>())
			{
				ParameterExpression parameterExpression = Expression.Parameter(expression.Type);
				List<Expression> list2 = new List<Expression>();
				foreach (string text in base.CallInfo.ArgumentNames)
				{
					MemberInfo[] member = type.GetMember(text, MemberTypes.Field | MemberTypes.Property, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
					if (member.Length != 1 || (!(member[0] is PropertyInfo) && !(member[0] is FieldInfo)))
					{
						return target.ThrowRuntimeError(args, BindingRestrictions.Empty, "PropertyNotFoundForType", ParserStrings.PropertyNotFoundForType, new Expression[]
						{
							Expression.Constant(text),
							Expression.Constant(type, typeof(Type))
						});
					}
					MemberInfo memberInfo = member[0];
					PropertyInfo propertyInfo = memberInfo as PropertyInfo;
					Type type2;
					Expression left;
					if (propertyInfo != null)
					{
						if (propertyInfo.GetSetMethod() == null)
						{
							return target.ThrowRuntimeError(args, BindingRestrictions.Empty, "PropertyIsReadOnly", ParserStrings.PropertyIsReadOnly, new Expression[]
							{
								Expression.Constant(text)
							});
						}
						type2 = propertyInfo.PropertyType;
						left = Expression.Property(parameterExpression.Cast(propertyInfo.DeclaringType), propertyInfo);
					}
					else
					{
						type2 = ((FieldInfo)memberInfo).FieldType;
						left = Expression.Field(parameterExpression.Cast(memberInfo.DeclaringType), (FieldInfo)memberInfo);
					}
					bool debase3;
					LanguagePrimitives.ConversionData conversionData = LanguagePrimitives.FigureConversion(args[i].Value, type2, out debase3);
					if (conversionData.Rank == ConversionRank.None)
					{
						return PSConvertBinder.ThrowNoConversion(args[i], type2, this, -1, args.Except(new DynamicMetaObject[]
						{
							args[i]
						}).Prepend(target).ToArray<DynamicMetaObject>());
					}
					list2.Add(Expression.Assign(left, PSConvertBinder.InvokeConverter(conversionData, args[i].Expression, type2, debase3, ExpressionCache.InvariantCulture)));
					i++;
				}
				ParameterExpression parameterExpression2 = Expression.Parameter(typeof(Exception));
				BlockExpression blockExpression = Expression.Block(Expression.Assign(parameterExpression, expression), Expression.TryCatch(Expression.Block(typeof(void), list2), new CatchBlock[]
				{
					Expression.Catch(parameterExpression2, Expression.Block(Expression.Call(CachedReflectionInfo.CommandProcessorBase_CheckForSevereException, parameterExpression2), Compiler.ThrowRuntimeErrorWithInnerException("PropertyAssignmentException", Expression.Property(parameterExpression2, "Message"), parameterExpression2, typeof(void), new Expression[0])))
				}), parameterExpression);
				expression = Expression.Block(new ParameterExpression[]
				{
					parameterExpression
				}, new Expression[]
				{
					blockExpression
				});
			}
			return new DynamicMetaObject(expression, target.CombineRestrictions(args));
		}

		// Token: 0x040021AA RID: 8618
		private static readonly Dictionary<CallInfo, PSAttributeGenerator> _binderCache = new Dictionary<CallInfo, PSAttributeGenerator>();
	}
}
