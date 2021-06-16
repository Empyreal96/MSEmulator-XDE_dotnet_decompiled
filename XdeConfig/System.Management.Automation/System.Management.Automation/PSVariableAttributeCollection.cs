using System;
using System.Collections.ObjectModel;
using System.Management.Automation.Runspaces;

namespace System.Management.Automation
{
	// Token: 0x02000845 RID: 2117
	internal class PSVariableAttributeCollection : Collection<Attribute>
	{
		// Token: 0x0600517B RID: 20859 RVA: 0x001B234D File Offset: 0x001B054D
		internal PSVariableAttributeCollection(PSVariable variable)
		{
			if (variable == null)
			{
				throw PSTraceSource.NewArgumentNullException("variable");
			}
			this.variable = variable;
		}

		// Token: 0x0600517C RID: 20860 RVA: 0x001B236C File Offset: 0x001B056C
		protected override void InsertItem(int index, Attribute item)
		{
			object newValue = this.VerifyNewAttribute(item);
			base.InsertItem(index, item);
			this.variable.SetValueRaw(newValue, true);
		}

		// Token: 0x0600517D RID: 20861 RVA: 0x001B2398 File Offset: 0x001B0598
		protected override void SetItem(int index, Attribute item)
		{
			object newValue = this.VerifyNewAttribute(item);
			base.SetItem(index, item);
			this.variable.SetValueRaw(newValue, true);
		}

		// Token: 0x0600517E RID: 20862 RVA: 0x001B23C2 File Offset: 0x001B05C2
		internal void AddAttributeNoCheck(Attribute item)
		{
			base.InsertItem(base.Count, item);
		}

		// Token: 0x0600517F RID: 20863 RVA: 0x001B23D4 File Offset: 0x001B05D4
		private object VerifyNewAttribute(Attribute item)
		{
			object obj = this.variable.Value;
			ArgumentTransformationAttribute argumentTransformationAttribute = item as ArgumentTransformationAttribute;
			if (argumentTransformationAttribute != null)
			{
				ExecutionContext executionContextFromTLS = LocalPipeline.GetExecutionContextFromTLS();
				EngineIntrinsics engineIntrinsics = null;
				if (executionContextFromTLS != null)
				{
					engineIntrinsics = executionContextFromTLS.EngineIntrinsics;
				}
				obj = argumentTransformationAttribute.Transform(engineIntrinsics, obj);
			}
			if (!PSVariable.IsValidValue(obj, item))
			{
				ValidationMetadataException ex = new ValidationMetadataException("ValidateSetFailure", null, Metadata.InvalidMetadataForCurrentValue, new object[]
				{
					this.variable.Name,
					(this.variable.Value != null) ? this.variable.Value.ToString() : ""
				});
				throw ex;
			}
			return obj;
		}

		// Token: 0x040029CA RID: 10698
		private PSVariable variable;
	}
}
