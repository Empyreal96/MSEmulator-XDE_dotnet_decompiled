using System;
using System.ComponentModel;

namespace System.Management.Automation
{
	// Token: 0x0200018F RID: 399
	public class PSObjectPropertyDescriptor : PropertyDescriptor
	{
		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06001355 RID: 4949 RVA: 0x000783A4 File Offset: 0x000765A4
		// (remove) Token: 0x06001356 RID: 4950 RVA: 0x000783DC File Offset: 0x000765DC
		internal event EventHandler<SettingValueExceptionEventArgs> SettingValueException;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06001357 RID: 4951 RVA: 0x00078414 File Offset: 0x00076614
		// (remove) Token: 0x06001358 RID: 4952 RVA: 0x0007844C File Offset: 0x0007664C
		internal event EventHandler<GettingValueExceptionEventArgs> GettingValueException;

		// Token: 0x06001359 RID: 4953 RVA: 0x00078481 File Offset: 0x00076681
		internal PSObjectPropertyDescriptor(string propertyName, Type propertyType, bool isReadOnly, AttributeCollection propertyAttributes) : base(propertyName, new Attribute[0])
		{
			this._isReadOnly = isReadOnly;
			this._propertyAttributes = propertyAttributes;
			this._propertyType = propertyType;
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x0600135A RID: 4954 RVA: 0x000784A6 File Offset: 0x000766A6
		public override AttributeCollection Attributes
		{
			get
			{
				return this._propertyAttributes;
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x0600135B RID: 4955 RVA: 0x000784AE File Offset: 0x000766AE
		public override bool IsReadOnly
		{
			get
			{
				return this._isReadOnly;
			}
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x000784B6 File Offset: 0x000766B6
		public override void ResetValue(object component)
		{
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x000784B8 File Offset: 0x000766B8
		public override bool CanResetValue(object component)
		{
			return false;
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x000784BB File Offset: 0x000766BB
		public override bool ShouldSerializeValue(object component)
		{
			return true;
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x0600135F RID: 4959 RVA: 0x000784BE File Offset: 0x000766BE
		public override Type ComponentType
		{
			get
			{
				return typeof(PSObject);
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06001360 RID: 4960 RVA: 0x000784CA File Offset: 0x000766CA
		public override Type PropertyType
		{
			get
			{
				return this._propertyType;
			}
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x000784D4 File Offset: 0x000766D4
		public override object GetValue(object component)
		{
			if (component == null)
			{
				throw PSTraceSource.NewArgumentNullException("component");
			}
			PSObject componentPSObject = PSObjectPropertyDescriptor.GetComponentPSObject(component);
			object result;
			try
			{
				PSPropertyInfo pspropertyInfo = componentPSObject.Properties[this.Name];
				if (pspropertyInfo == null)
				{
					PSObjectTypeDescriptor.typeDescriptor.WriteLine("Could not find property \"{0}\" to get its value.", new object[]
					{
						this.Name
					});
					ExtendedTypeSystemException ex = new ExtendedTypeSystemException("PropertyNotFoundInPropertyDescriptorGetValue", null, ExtendedTypeSystem.PropertyNotFoundInTypeDescriptor, new object[]
					{
						this.Name
					});
					bool flag;
					object obj = this.DealWithGetValueException(ex, out flag);
					if (flag)
					{
						throw ex;
					}
					result = obj;
				}
				else
				{
					result = pspropertyInfo.Value;
				}
			}
			catch (ExtendedTypeSystemException ex2)
			{
				PSObjectTypeDescriptor.typeDescriptor.WriteLine("Exception getting the value of the property \"{0}\": \"{1}\".", new object[]
				{
					this.Name,
					ex2.Message
				});
				bool flag2;
				object obj2 = this.DealWithGetValueException(ex2, out flag2);
				if (flag2)
				{
					throw;
				}
				result = obj2;
			}
			return result;
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x000785D0 File Offset: 0x000767D0
		private static PSObject GetComponentPSObject(object component)
		{
			PSObject psobject = component as PSObject;
			if (psobject == null)
			{
				PSObjectTypeDescriptor psobjectTypeDescriptor = component as PSObjectTypeDescriptor;
				if (psobjectTypeDescriptor == null)
				{
					throw PSTraceSource.NewArgumentException("component", ExtendedTypeSystem.InvalidComponent, new object[]
					{
						"component",
						typeof(PSObject).Name,
						typeof(PSObjectTypeDescriptor).Name
					});
				}
				psobject = psobjectTypeDescriptor.Instance;
			}
			return psobject;
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x00078640 File Offset: 0x00076840
		private object DealWithGetValueException(ExtendedTypeSystemException e, out bool shouldThrow)
		{
			GettingValueExceptionEventArgs gettingValueExceptionEventArgs = new GettingValueExceptionEventArgs(e);
			if (this.GettingValueException != null)
			{
				this.GettingValueException.SafeInvoke(this, gettingValueExceptionEventArgs);
				PSObjectTypeDescriptor.typeDescriptor.WriteLine("GettingValueException event has been triggered resulting in ValueReplacement:\"{0}\".", new object[]
				{
					gettingValueExceptionEventArgs.ValueReplacement
				});
			}
			shouldThrow = gettingValueExceptionEventArgs.ShouldThrow;
			return gettingValueExceptionEventArgs.ValueReplacement;
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x00078698 File Offset: 0x00076898
		public override void SetValue(object component, object value)
		{
			if (component == null)
			{
				throw PSTraceSource.NewArgumentNullException("component");
			}
			PSObject componentPSObject = PSObjectPropertyDescriptor.GetComponentPSObject(component);
			try
			{
				PSPropertyInfo pspropertyInfo = componentPSObject.Properties[this.Name];
				if (pspropertyInfo == null)
				{
					PSObjectTypeDescriptor.typeDescriptor.WriteLine("Could not find property \"{0}\" to set its value.", new object[]
					{
						this.Name
					});
					ExtendedTypeSystemException ex = new ExtendedTypeSystemException("PropertyNotFoundInPropertyDescriptorSetValue", null, ExtendedTypeSystem.PropertyNotFoundInTypeDescriptor, new object[]
					{
						this.Name
					});
					bool flag;
					this.DealWithSetValueException(ex, out flag);
					if (flag)
					{
						throw ex;
					}
					return;
				}
				else
				{
					pspropertyInfo.Value = value;
				}
			}
			catch (ExtendedTypeSystemException ex2)
			{
				PSObjectTypeDescriptor.typeDescriptor.WriteLine("Exception setting the value of the property \"{0}\": \"{1}\".", new object[]
				{
					this.Name,
					ex2.Message
				});
				bool flag2;
				this.DealWithSetValueException(ex2, out flag2);
				if (flag2)
				{
					throw;
				}
			}
			this.OnValueChanged(component, EventArgs.Empty);
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x00078790 File Offset: 0x00076990
		private void DealWithSetValueException(ExtendedTypeSystemException e, out bool shouldThrow)
		{
			SettingValueExceptionEventArgs settingValueExceptionEventArgs = new SettingValueExceptionEventArgs(e);
			if (this.SettingValueException != null)
			{
				this.SettingValueException.SafeInvoke(this, settingValueExceptionEventArgs);
				PSObjectTypeDescriptor.typeDescriptor.WriteLine("SettingValueException event has been triggered resulting in ShouldThrow:\"{0}\".", new object[]
				{
					settingValueExceptionEventArgs.ShouldThrow
				});
			}
			shouldThrow = settingValueExceptionEventArgs.ShouldThrow;
		}

		// Token: 0x04000845 RID: 2117
		private bool _isReadOnly;

		// Token: 0x04000846 RID: 2118
		private AttributeCollection _propertyAttributes;

		// Token: 0x04000847 RID: 2119
		private Type _propertyType;
	}
}
