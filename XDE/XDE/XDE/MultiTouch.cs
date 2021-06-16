using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Shapes;

namespace Microsoft.Xde.Client
{
	// Token: 0x02000021 RID: 33
	public class MultiTouch : UserControl, IComponentConnector, IStyleConnector
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060001BA RID: 442 RVA: 0x000081DC File Offset: 0x000063DC
		// (remove) Token: 0x060001BB RID: 443 RVA: 0x00008214 File Offset: 0x00006414
		public event EventHandler<MultiTouchEventArgs> PointsTouched;

		// Token: 0x060001BC RID: 444 RVA: 0x0000824C File Offset: 0x0000644C
		private void FirePointsTouched(TouchEventType type)
		{
			if (this.PointsTouched != null)
			{
				double num = (double)base.GetValue(Canvas.LeftProperty) + 5.0;
				double num2 = (double)base.GetValue(Canvas.TopProperty) + 5.0;
				Point point = new Point(num + this.model.X1, num2 + this.model.Y1);
				Point point2 = new Point(num + this.model.X2, num2 + this.model.Y2);
				MultiTouchEventArgs e = new MultiTouchEventArgs(point, point2, type);
				EventHandler<MultiTouchEventArgs> pointsTouched = this.PointsTouched;
				if (pointsTouched == null)
				{
					return;
				}
				pointsTouched(this, e);
			}
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000082F4 File Offset: 0x000064F4
		private static void Point1XCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MultiTouch.MultiTouchViewModel multiTouchViewModel = ((MultiTouch)d).model;
			multiTouchViewModel.X1 = (double)e.NewValue;
			multiTouchViewModel.X2 = -multiTouchViewModel.X1;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000831F File Offset: 0x0000651F
		private static void Point1YCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MultiTouch.MultiTouchViewModel multiTouchViewModel = ((MultiTouch)d).model;
			multiTouchViewModel.Y1 = (double)e.NewValue;
			multiTouchViewModel.Y2 = -multiTouchViewModel.Y1;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000834A File Offset: 0x0000654A
		public MultiTouch()
		{
			this.InitializeComponent();
			this.MainControl.DataContext = this.model;
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00008374 File Offset: 0x00006574
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x00008386 File Offset: 0x00006586
		public double Point1X
		{
			get
			{
				return (double)base.GetValue(MultiTouch.Point1XProperty);
			}
			set
			{
				base.SetValue(MultiTouch.Point1XProperty, value);
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x00008399 File Offset: 0x00006599
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x000083AB File Offset: 0x000065AB
		public double Point1Y
		{
			get
			{
				return (double)base.GetValue(MultiTouch.Point1YProperty);
			}
			set
			{
				base.SetValue(MultiTouch.Point1YProperty, value);
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x000083BE File Offset: 0x000065BE
		private void TargetCircle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.StartTargetCircleDrag(sender, e, true);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000083CC File Offset: 0x000065CC
		private void StartTargetCircleDrag(object sender, MouseButtonEventArgs e, bool engaged)
		{
			this.StartDrag(sender, e, engaged);
			FrameworkElement frameworkElement = sender as FrameworkElement;
			this.dragSign = ((frameworkElement.Name == "Target1") ? 1.0 : -1.0);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00008418 File Offset: 0x00006618
		private void TargetCircle_MouseMove(object sender, MouseEventArgs e)
		{
			e.Handled = true;
			if (this.model.IsDragging)
			{
				FrameworkElement relativeTo = base.Parent as FrameworkElement;
				Point position = e.GetPosition(relativeTo);
				double num = (position.X - this.lastPoint.X) * this.dragSign;
				double num2 = (position.Y - this.lastPoint.Y) * this.dragSign;
				this.lastPoint = position;
				this.model.X1 += num;
				this.model.Y1 += num2;
				this.model.X2 -= num;
				this.model.Y2 -= num2;
				if (this.model.IsEngaged)
				{
					this.FirePointsTouched(TouchEventType.Move);
				}
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000084F0 File Offset: 0x000066F0
		private void Drag_MouseButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (this.model.IsDragging)
			{
				(sender as FrameworkElement).ReleaseMouseCapture();
				this.model.IsDragging = false;
				e.Handled = true;
				if (this.model.IsEngaged)
				{
					this.model.IsEngaged = false;
					this.FirePointsTouched(TouchEventType.Up);
				}
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00008548 File Offset: 0x00006748
		private void CenterCircle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.StartDrag(sender, e, true);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00008554 File Offset: 0x00006754
		private void StartDrag(object sender, MouseButtonEventArgs e, bool engaged)
		{
			UIElement uielement = sender as FrameworkElement;
			FrameworkElement relativeTo = base.Parent as FrameworkElement;
			this.lastPoint = e.GetPosition(relativeTo);
			uielement.CaptureMouse();
			this.model.IsDragging = true;
			e.Handled = true;
			if (engaged)
			{
				this.model.IsEngaged = true;
				this.FirePointsTouched(TouchEventType.Down);
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000085B0 File Offset: 0x000067B0
		private void CenterCircle_MouseMove(object sender, MouseEventArgs e)
		{
			e.Handled = true;
			if (this.model.IsDragging)
			{
				FrameworkElement relativeTo = base.Parent as FrameworkElement;
				Point position = e.GetPosition(relativeTo);
				double num = position.X - this.lastPoint.X;
				double num2 = position.Y - this.lastPoint.Y;
				this.lastPoint = position;
				double num3 = (double)base.GetValue(Canvas.LeftProperty);
				base.SetValue(Canvas.LeftProperty, num3 + num);
				num3 = (double)base.GetValue(Canvas.TopProperty);
				base.SetValue(Canvas.TopProperty, num3 + num2);
				if (this.model.IsEngaged)
				{
					this.FirePointsTouched(TouchEventType.Move);
				}
			}
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00008677 File Offset: 0x00006877
		private void CenterCircle_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.StartDrag(sender, e, false);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00008682 File Offset: 0x00006882
		private void TargetCircle_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.StartTargetCircleDrag(sender, e, false);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00008690 File Offset: 0x00006890
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri resourceLocator = new Uri("/XDE;V10.1.0.0;component/controls/multitouch.xaml", UriKind.Relative);
			Application.LoadComponent(this, resourceLocator);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000086C0 File Offset: 0x000068C0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				this.TopControl = (MultiTouch)target;
				return;
			}
			if (connectionId != 2)
			{
				this._contentLoaded = true;
				return;
			}
			this.MainControl = (ContentControl)target;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000086F0 File Offset: 0x000068F0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 3:
				((Ellipse)target).MouseLeftButtonDown += this.CenterCircle_MouseLeftButtonDown;
				((Ellipse)target).MouseRightButtonDown += this.CenterCircle_MouseRightButtonDown;
				((Ellipse)target).MouseRightButtonUp += this.Drag_MouseButtonUp;
				((Ellipse)target).MouseMove += this.CenterCircle_MouseMove;
				((Ellipse)target).MouseLeftButtonUp += this.Drag_MouseButtonUp;
				return;
			case 4:
				((Grid)target).MouseLeftButtonDown += this.TargetCircle_MouseLeftButtonDown;
				((Grid)target).MouseRightButtonDown += this.TargetCircle_MouseRightButtonDown;
				((Grid)target).MouseRightButtonUp += this.Drag_MouseButtonUp;
				((Grid)target).MouseMove += this.TargetCircle_MouseMove;
				((Grid)target).MouseLeftButtonUp += this.Drag_MouseButtonUp;
				return;
			case 5:
				((Grid)target).MouseLeftButtonDown += this.TargetCircle_MouseLeftButtonDown;
				((Grid)target).MouseRightButtonDown += this.TargetCircle_MouseRightButtonDown;
				((Grid)target).MouseRightButtonUp += this.Drag_MouseButtonUp;
				((Grid)target).MouseMove += this.TargetCircle_MouseMove;
				((Grid)target).MouseLeftButtonUp += this.Drag_MouseButtonUp;
				return;
			default:
				return;
			}
		}

		// Token: 0x040000B5 RID: 181
		public static DependencyProperty Point1XProperty = DependencyProperty.Register("Point1X", typeof(double), typeof(MultiTouch), new FrameworkPropertyMetadata(40.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(MultiTouch.Point1XCallback)));

		// Token: 0x040000B6 RID: 182
		public static DependencyProperty Point1YProperty = DependencyProperty.Register("Point1Y", typeof(double), typeof(MultiTouch), new FrameworkPropertyMetadata(40.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(MultiTouch.Point1YCallback)));

		// Token: 0x040000B7 RID: 183
		private const double CenterPointRadius = 5.0;

		// Token: 0x040000B8 RID: 184
		private MultiTouch.MultiTouchViewModel model = new MultiTouch.MultiTouchViewModel();

		// Token: 0x040000B9 RID: 185
		private Point lastPoint;

		// Token: 0x040000BA RID: 186
		private double dragSign;

		// Token: 0x040000BC RID: 188
		internal MultiTouch TopControl;

		// Token: 0x040000BD RID: 189
		internal ContentControl MainControl;

		// Token: 0x040000BE RID: 190
		private bool _contentLoaded;

		// Token: 0x0200003F RID: 63
		private class MultiTouchViewModel : INotifyPropertyChanged
		{
			// Token: 0x14000019 RID: 25
			// (add) Token: 0x06000476 RID: 1142 RVA: 0x000113D0 File Offset: 0x0000F5D0
			// (remove) Token: 0x06000477 RID: 1143 RVA: 0x00011408 File Offset: 0x0000F608
			public event PropertyChangedEventHandler PropertyChanged;

			// Token: 0x170001BA RID: 442
			// (get) Token: 0x06000478 RID: 1144 RVA: 0x0001143D File Offset: 0x0000F63D
			public double ActualX1
			{
				get
				{
					return this.x1 - 15.0;
				}
			}

			// Token: 0x170001BB RID: 443
			// (get) Token: 0x06000479 RID: 1145 RVA: 0x0001144F File Offset: 0x0000F64F
			// (set) Token: 0x0600047A RID: 1146 RVA: 0x00011457 File Offset: 0x0000F657
			public bool IsDragging
			{
				get
				{
					return this.isDragging;
				}
				set
				{
					this.isDragging = value;
					this.OnPropertyChanged("IsDragging");
				}
			}

			// Token: 0x170001BC RID: 444
			// (get) Token: 0x0600047B RID: 1147 RVA: 0x0001146B File Offset: 0x0000F66B
			// (set) Token: 0x0600047C RID: 1148 RVA: 0x00011473 File Offset: 0x0000F673
			public bool IsEngaged
			{
				get
				{
					return this.isEngaged;
				}
				set
				{
					this.isEngaged = value;
					this.OnPropertyChanged("IsEngaged");
				}
			}

			// Token: 0x170001BD RID: 445
			// (get) Token: 0x0600047D RID: 1149 RVA: 0x00011487 File Offset: 0x0000F687
			public double ActualY1
			{
				get
				{
					return this.y1 - 15.0;
				}
			}

			// Token: 0x170001BE RID: 446
			// (get) Token: 0x0600047E RID: 1150 RVA: 0x00011499 File Offset: 0x0000F699
			public double ActualX2
			{
				get
				{
					return this.x2 - 15.0;
				}
			}

			// Token: 0x170001BF RID: 447
			// (get) Token: 0x0600047F RID: 1151 RVA: 0x000114AB File Offset: 0x0000F6AB
			public double ActualY2
			{
				get
				{
					return this.y2 - 15.0;
				}
			}

			// Token: 0x170001C0 RID: 448
			// (get) Token: 0x06000480 RID: 1152 RVA: 0x000114BD File Offset: 0x0000F6BD
			// (set) Token: 0x06000481 RID: 1153 RVA: 0x000114C5 File Offset: 0x0000F6C5
			public double X1
			{
				get
				{
					return this.x1;
				}
				set
				{
					this.x1 = value;
					this.OnPropertyChanged("X1");
					this.OnPropertyChanged("ActualX1");
				}
			}

			// Token: 0x170001C1 RID: 449
			// (get) Token: 0x06000482 RID: 1154 RVA: 0x000114E4 File Offset: 0x0000F6E4
			// (set) Token: 0x06000483 RID: 1155 RVA: 0x000114EC File Offset: 0x0000F6EC
			public double Y1
			{
				get
				{
					return this.y1;
				}
				set
				{
					this.y1 = value;
					this.OnPropertyChanged("Y1");
					this.OnPropertyChanged("ActualY1");
				}
			}

			// Token: 0x170001C2 RID: 450
			// (get) Token: 0x06000484 RID: 1156 RVA: 0x0001150B File Offset: 0x0000F70B
			// (set) Token: 0x06000485 RID: 1157 RVA: 0x00011513 File Offset: 0x0000F713
			public double X2
			{
				get
				{
					return this.x2;
				}
				set
				{
					this.x2 = value;
					this.OnPropertyChanged("X2");
					this.OnPropertyChanged("ActualX2");
				}
			}

			// Token: 0x170001C3 RID: 451
			// (get) Token: 0x06000486 RID: 1158 RVA: 0x00011532 File Offset: 0x0000F732
			// (set) Token: 0x06000487 RID: 1159 RVA: 0x0001153A File Offset: 0x0000F73A
			public double Y2
			{
				get
				{
					return this.y2;
				}
				set
				{
					this.y2 = value;
					this.OnPropertyChanged("Y2");
					this.OnPropertyChanged("ActualY2");
				}
			}

			// Token: 0x06000488 RID: 1160 RVA: 0x00011559 File Offset: 0x0000F759
			private void OnPropertyChanged(string propName)
			{
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs(propName));
			}

			// Token: 0x040001A6 RID: 422
			private const double Radius = 15.0;

			// Token: 0x040001A7 RID: 423
			private double x1;

			// Token: 0x040001A8 RID: 424
			private double y1;

			// Token: 0x040001A9 RID: 425
			private double x2;

			// Token: 0x040001AA RID: 426
			private double y2;

			// Token: 0x040001AB RID: 427
			private bool isDragging;

			// Token: 0x040001AC RID: 428
			private bool isEngaged;
		}
	}
}
