using System;
using UIKit;

namespace UIWebViewRichTextEditor
{
	// delegate methods for UITapGestureRecognizer
	public class GestureDelegate : UIGestureRecognizerDelegate
	{
		public override bool ShouldReceiveTouch(UIGestureRecognizer recognizer, UITouch touch)
		{
			return true;
		}

		public override bool ShouldBegin(UIGestureRecognizer recognizer)
		{
			return true;
		}

		public override bool ShouldRecognizeSimultaneously(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
		{
			return true;
		}
	}

}

