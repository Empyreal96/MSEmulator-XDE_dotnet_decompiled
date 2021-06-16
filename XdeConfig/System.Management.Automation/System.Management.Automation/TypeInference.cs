using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.Management.Automation
{
	// Token: 0x02000113 RID: 275
	internal class TypeInference
	{
		// Token: 0x06000ECD RID: 3789 RVA: 0x00051C84 File Offset: 0x0004FE84
		internal static MethodInformation Infer(MethodInformation genericMethod, Type[] argumentTypes)
		{
			MethodInfo genericMethod2 = (MethodInfo)genericMethod.method;
			MethodInfo methodInfo = TypeInference.Infer(genericMethod2, argumentTypes, genericMethod.hasVarArgs);
			if (methodInfo != null)
			{
				return new MethodInformation(methodInfo, 0);
			}
			return null;
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x00051CC8 File Offset: 0x0004FEC8
		private static MethodInfo Infer(MethodInfo genericMethod, Type[] typesOfMethodArguments, bool hasVarArgs)
		{
			if (!genericMethod.ContainsGenericParameters)
			{
				return genericMethod;
			}
			Type[] genericArguments = genericMethod.GetGenericArguments();
			Type[] array = (from p in genericMethod.GetParameters()
			select p.ParameterType).ToArray<Type>();
			MethodInfo methodInfo = TypeInference.Infer(genericMethod, genericArguments, array, typesOfMethodArguments);
			if (methodInfo == null && hasVarArgs && typesOfMethodArguments.Length >= array.Length - 1)
			{
				IEnumerable<Type> first = array.Take(array.Length - 1);
				IEnumerable<Type> second = Enumerable.Repeat<Type>(array[array.Length - 1].GetElementType(), typesOfMethodArguments.Length - array.Length + 1);
				methodInfo = TypeInference.Infer(genericMethod, genericArguments, first.Concat(second), typesOfMethodArguments);
			}
			return methodInfo;
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x00051D80 File Offset: 0x0004FF80
		private static MethodInfo Infer(MethodInfo genericMethod, ICollection<Type> typeParameters, IEnumerable<Type> typesOfMethodParameters, IEnumerable<Type> typesOfMethodArguments)
		{
			MethodInfo result;
			using (TypeInference.tracer.TraceScope("Inferring type parameters for the following method: {0}", new object[]
			{
				genericMethod
			}))
			{
				if (PSTraceSourceOptions.WriteLine == (TypeInference.tracer.Options & PSTraceSourceOptions.WriteLine))
				{
					PSTraceSource pstraceSource = TypeInference.tracer;
					string format = "Types of method arguments: {0}";
					object[] array = new object[1];
					array[0] = string.Join(", ", (from t in typesOfMethodArguments
					select t.ToString()).ToArray<string>());
					pstraceSource.WriteLine(format, array);
				}
				TypeInference typeInference = new TypeInference(typeParameters);
				if (!typeInference.UnifyMultipleTerms(typesOfMethodParameters, typesOfMethodArguments))
				{
					result = null;
				}
				else
				{
					IEnumerable<Type> source = typeParameters.Select(new Func<Type, Type>(typeInference.GetInferredType));
					if (source.Any((Type inferredType) => inferredType == null))
					{
						result = null;
					}
					else
					{
						try
						{
							MethodInfo methodInfo = genericMethod.MakeGenericMethod(source.ToArray<Type>());
							TypeInference.tracer.WriteLine("Inference succesful: {0}", new object[]
							{
								methodInfo
							});
							result = methodInfo;
						}
						catch (ArgumentException ex)
						{
							TypeInference.tracer.WriteLine("Inference failure: {0}", new object[]
							{
								ex.Message
							});
							result = null;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x00051F04 File Offset: 0x00050104
		internal TypeInference(ICollection<Type> typeParameters)
		{
			this.typeParameterIndexToSetOfInferenceCandidates = new HashSet<Type>[typeParameters.Count];
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x00051FB4 File Offset: 0x000501B4
		internal Type GetInferredType(Type typeParameter)
		{
			ICollection<Type> inferenceCandidates = this.typeParameterIndexToSetOfInferenceCandidates[typeParameter.GenericParameterPosition];
			if (inferenceCandidates != null)
			{
				if (inferenceCandidates.Any((Type t) => t == typeof(LanguagePrimitives.Null)))
				{
					Type type = inferenceCandidates.FirstOrDefault((Type t) => t.GetTypeInfo().IsValueType);
					if (type != null)
					{
						TypeInference.tracer.WriteLine("Cannot reconcile null and {0} (a value type)", new object[]
						{
							type
						});
						inferenceCandidates = null;
						this.typeParameterIndexToSetOfInferenceCandidates[typeParameter.GenericParameterPosition] = null;
					}
					else
					{
						inferenceCandidates = (from t in inferenceCandidates
						where t != typeof(LanguagePrimitives.Null)
						select t).ToList<Type>();
						if (inferenceCandidates.Count == 0)
						{
							inferenceCandidates = null;
							this.typeParameterIndexToSetOfInferenceCandidates[typeParameter.GenericParameterPosition] = null;
						}
					}
				}
			}
			if (inferenceCandidates != null && inferenceCandidates.Count > 1)
			{
				Type type2 = inferenceCandidates.FirstOrDefault((Type potentiallyCommonBaseClass) => inferenceCandidates.All((Type otherCandidate) => otherCandidate == potentiallyCommonBaseClass || potentiallyCommonBaseClass.IsAssignableFrom(otherCandidate)));
				if (type2 != null)
				{
					inferenceCandidates.Clear();
					inferenceCandidates.Add(type2);
				}
				else
				{
					TypeInference.tracer.WriteLine("Multiple unreconcilable inferences for type parameter {0}", new object[]
					{
						typeParameter
					});
					inferenceCandidates = null;
					this.typeParameterIndexToSetOfInferenceCandidates[typeParameter.GenericParameterPosition] = null;
				}
			}
			if (inferenceCandidates == null)
			{
				TypeInference.tracer.WriteLine("Couldn't infer type parameter {0}", new object[]
				{
					typeParameter
				});
				return null;
			}
			return inferenceCandidates.Single<Type>();
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x00052190 File Offset: 0x00050390
		internal bool UnifyMultipleTerms(IEnumerable<Type> parameterTypes, IEnumerable<Type> argumentTypes)
		{
			List<Type> list = parameterTypes.ToList<Type>();
			List<Type> list2 = argumentTypes.ToList<Type>();
			if (list.Count != list2.Count)
			{
				TypeInference.tracer.WriteLine("Mismatch in number of parameters and arguments", new object[0]);
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (!this.Unify(list[i], list2[i]))
				{
					TypeInference.tracer.WriteLine("Couldn't unify {0} with {1}", new object[]
					{
						list[i],
						list2[i]
					});
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x00052228 File Offset: 0x00050428
		private bool Unify(Type parameterType, Type argumentType)
		{
			TypeInfo typeInfo = parameterType.GetTypeInfo();
			if (!typeInfo.ContainsGenericParameters)
			{
				return true;
			}
			if (parameterType.IsGenericParameter)
			{
				HashSet<Type> hashSet = this.typeParameterIndexToSetOfInferenceCandidates[parameterType.GenericParameterPosition];
				if (hashSet == null)
				{
					hashSet = new HashSet<Type>();
					this.typeParameterIndexToSetOfInferenceCandidates[parameterType.GenericParameterPosition] = hashSet;
				}
				hashSet.Add(argumentType);
				TypeInference.tracer.WriteLine("Inferred {0} => {1}", new object[]
				{
					parameterType,
					argumentType
				});
				return true;
			}
			if (parameterType.IsArray)
			{
				if (argumentType == typeof(LanguagePrimitives.Null))
				{
					return true;
				}
				if (argumentType.IsArray && parameterType.GetArrayRank() == argumentType.GetArrayRank())
				{
					return this.Unify(parameterType.GetElementType(), argumentType.GetElementType());
				}
				TypeInference.tracer.WriteLine("Couldn't unify array {0} with {1}", new object[]
				{
					parameterType,
					argumentType
				});
				return false;
			}
			else if (parameterType.IsByRef)
			{
				if (argumentType.GetTypeInfo().IsGenericType && argumentType.GetGenericTypeDefinition() == typeof(PSReference<>))
				{
					Type type = argumentType.GetGenericArguments()[0];
					return type == typeof(LanguagePrimitives.Null) || this.Unify(parameterType.GetElementType(), type);
				}
				TypeInference.tracer.WriteLine("Couldn't unify reference type {0} with {1}", new object[]
				{
					parameterType,
					argumentType
				});
				return false;
			}
			else
			{
				if (typeInfo.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(Nullable<>))
				{
					return argumentType == typeof(LanguagePrimitives.Null) || this.Unify(parameterType.GetGenericArguments()[0], argumentType);
				}
				if (typeInfo.IsGenericType)
				{
					return argumentType == typeof(LanguagePrimitives.Null) || this.UnifyConstructedType(parameterType, argumentType);
				}
				TypeInference.tracer.WriteLine("Unrecognized kind of type: {0}", new object[]
				{
					parameterType
				});
				return false;
			}
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x0005240C File Offset: 0x0005060C
		private bool UnifyConstructedType(Type parameterType, Type argumentType)
		{
			if (TypeInference.IsEqualGenericTypeDefinition(parameterType, argumentType))
			{
				IEnumerable<Type> genericArguments = parameterType.GetGenericArguments();
				IEnumerable<Type> genericArguments2 = argumentType.GetGenericArguments();
				return this.UnifyMultipleTerms(genericArguments, genericArguments2);
			}
			Type[] interfaces = argumentType.GetInterfaces();
			for (int i = 0; i < interfaces.Length; i++)
			{
				if (TypeInference.IsEqualGenericTypeDefinition(parameterType, interfaces[i]))
				{
					return this.UnifyConstructedType(parameterType, interfaces[i]);
				}
			}
			Type baseType = argumentType.GetTypeInfo().BaseType;
			while (baseType != null)
			{
				if (TypeInference.IsEqualGenericTypeDefinition(parameterType, baseType))
				{
					return this.UnifyConstructedType(parameterType, baseType);
				}
				baseType = baseType.GetTypeInfo().BaseType;
			}
			TypeInference.tracer.WriteLine("Attempt to unify different constructed types: {0} and {1}", new object[]
			{
				parameterType,
				argumentType
			});
			return false;
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x000524C3 File Offset: 0x000506C3
		private static bool IsEqualGenericTypeDefinition(Type parameterType, Type argumentType)
		{
			return argumentType.GetTypeInfo().IsGenericType && parameterType.GetGenericTypeDefinition() == argumentType.GetGenericTypeDefinition();
		}

		// Token: 0x0400067A RID: 1658
		[TraceSource("ETS", "Extended Type System")]
		private static readonly PSTraceSource tracer = PSTraceSource.GetTracer("ETS", "Extended Type System");

		// Token: 0x0400067B RID: 1659
		private readonly HashSet<Type>[] typeParameterIndexToSetOfInferenceCandidates;
	}
}
