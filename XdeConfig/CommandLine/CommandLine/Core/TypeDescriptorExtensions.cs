using System;
using CSharpx;

namespace CommandLine.Core
{
	// Token: 0x02000086 RID: 134
	internal static class TypeDescriptorExtensions
	{
		// Token: 0x06000322 RID: 802 RVA: 0x0000C840 File Offset: 0x0000AA40
		public static TypeDescriptor WithNextValue(this TypeDescriptor descriptor, Maybe<TypeDescriptor> nextValue)
		{
			return TypeDescriptor.Create(descriptor.TargetType, descriptor.MaxItems, nextValue.GetValueOrDefault(default(TypeDescriptor)));
		}
	}
}
