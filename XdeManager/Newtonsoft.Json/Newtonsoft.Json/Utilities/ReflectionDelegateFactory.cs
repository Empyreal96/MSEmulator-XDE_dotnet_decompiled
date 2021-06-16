using System;
using System.Globalization;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200005E RID: 94
	internal abstract class ReflectionDelegateFactory
	{
		// Token: 0x06000571 RID: 1393 RVA: 0x00017D68 File Offset: 0x00015F68
		public Func<T, object> CreateGet<T>(MemberInfo memberInfo)
		{
			PropertyInfo propertyInfo;
			if ((propertyInfo = (memberInfo as PropertyInfo)) != null)
			{
				if (propertyInfo.PropertyType.IsByRef)
				{
					throw new InvalidOperationException("Could not create getter for {0}. ByRef return values are not supported.".FormatWith(CultureInfo.InvariantCulture, propertyInfo));
				}
				return this.CreateGet<T>(propertyInfo);
			}
			else
			{
				FieldInfo fieldInfo;
				if ((fieldInfo = (memberInfo as FieldInfo)) != null)
				{
					return this.CreateGet<T>(fieldInfo);
				}
				throw new Exception("Could not create getter for {0}.".FormatWith(CultureInfo.InvariantCulture, memberInfo));
			}
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00017DD4 File Offset: 0x00015FD4
		public Action<T, object> CreateSet<T>(MemberInfo memberInfo)
		{
			PropertyInfo propertyInfo;
			if ((propertyInfo = (memberInfo as PropertyInfo)) != null)
			{
				return this.CreateSet<T>(propertyInfo);
			}
			FieldInfo fieldInfo;
			if ((fieldInfo = (memberInfo as FieldInfo)) != null)
			{
				return this.CreateSet<T>(fieldInfo);
			}
			throw new Exception("Could not create setter for {0}.".FormatWith(CultureInfo.InvariantCulture, memberInfo));
		}

		// Token: 0x06000573 RID: 1395
		public abstract MethodCall<T, object> CreateMethodCall<T>(MethodBase method);

		// Token: 0x06000574 RID: 1396
		public abstract ObjectConstructor<object> CreateParameterizedConstructor(MethodBase method);

		// Token: 0x06000575 RID: 1397
		public abstract Func<T> CreateDefaultConstructor<T>(Type type);

		// Token: 0x06000576 RID: 1398
		public abstract Func<T, object> CreateGet<T>(PropertyInfo propertyInfo);

		// Token: 0x06000577 RID: 1399
		public abstract Func<T, object> CreateGet<T>(FieldInfo fieldInfo);

		// Token: 0x06000578 RID: 1400
		public abstract Action<T, object> CreateSet<T>(FieldInfo fieldInfo);

		// Token: 0x06000579 RID: 1401
		public abstract Action<T, object> CreateSet<T>(PropertyInfo propertyInfo);
	}
}
