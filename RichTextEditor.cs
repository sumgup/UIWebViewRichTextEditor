using Foundation;
using System;
using UIKit;
using System.IO;

namespace UIWebViewRichTextEditor
{
    public partial class RichTextEditor : UIWebView
    {
        public RichTextEditor (IntPtr handle) : base (handle)
		{
			this.LoadHtml();
        }

		public void LoadHtml()
		{ 
			string contentDirectoryPath = Path.Combine(NSBundle.MainBundle.BundlePath, "Content/");
			string fileName = "Content/Index.html";
			var html = File.ReadAllText(Path.Combine(NSBundle.MainBundle.BundlePath, fileName));
			base.LoadHtmlString(html, new NSUrl(contentDirectoryPath, true));
		}

    }
}