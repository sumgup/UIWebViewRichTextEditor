using System;
using Foundation;
using UIKit;

namespace UIWebViewRichTextEditor
{
	
	public class RTEGestureRecognizer : UIGestureRecognizer
	{
		public event EventHandler<CustomArgs> TouchesDidBegin;
		public event EventHandler<CustomArgs> TouchesDidEnd;

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			this.TouchesDidBegin(this, new CustomArgs(touches, evt));
		}

		public override void TouchesEnded(NSSet touches, UIEvent evt)
		{
			this.TouchesDidEnd(this, new CustomArgs(touches, evt));
		}
	}

	public class CustomArgs : EventArgs
	{
		public NSSet touches { get; set; }
		public UIEvent evt { get; set; }
		public CustomArgs(NSSet touches, UIEvent evt)
		{
			this.touches = touches;
			this.evt = evt;
		}
	}
}

