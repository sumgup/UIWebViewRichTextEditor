using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace UIWebViewRichTextEditor
{
	
	public partial class ViewController : UIViewController
	{
		String Html;
		Timer timer;

		public Boolean CurrentBoldStatus
		{
			get;
			set;
		}

		public Boolean CurrentItalicStatus
		{
			get;
			set;
		}
	

		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			LoadHtmlToRichText();
			CheckSelection();

			timer = new Timer(500);
			timer.Elapsed += OnTimerElasped;
			timer.Start();

			AddGesturesToRichTextBox();

		}

		public override void ViewDidUnload()
		{
			base.ViewDidUnload();
			if (timer != null)
			{
				timer.Elapsed -= OnTimerElasped;
				timer.Stop();
			}
		}

		void OnTimerElasped(object o, EventArgs e)
		{
			CheckSelection();
		}

		private void CheckSelection()
		{
			try
			{
				InvokeOnMainThread(() =>
				{

					var boldEnabled = Convert.ToBoolean(webView.EvaluateJavascript(@"document.queryCommandState('bold')"));
					var italicEnabled = Convert.ToBoolean(webView.EvaluateJavascript(@"document.queryCommandState('italic')"));

					var listOfButtons = new List<UIBarButtonItem>();

					var boldUIBarButtonItem = new UIBarButtonItem(boldEnabled ? "[B]" : "B", UIBarButtonItemStyle.Plain,(sender, args) =>
					{
						var result = webView.EvaluateJavascript(@"document.execCommand('bold')");
						var htmlCode = webView.EvaluateJavascript(@"document.body.innerHTML");
						Console.WriteLine(htmlCode);
					});

					btnBold.SetTitle(boldEnabled ? "[B]" : "B", UIControlState.Normal);
					btnItalic.SetTitle(boldEnabled ? "[I]" : "I", UIControlState.Normal);

					listOfButtons.Add(boldUIBarButtonItem);


					/*
						We add the bar button items to the array and then if there are any changes to the status of bold,
						italic or underline since we last checked or if this is the first time then we set that array 
						as the navigation items right bar button items and update the statuses.
					 */

					if (CurrentBoldStatus != boldEnabled || CurrentItalicStatus != italicEnabled)
					{
						CurrentBoldStatus = boldEnabled;
						CurrentItalicStatus = italicEnabled;
					}

					this.NavigationItem.LeftBarButtonItems = listOfButtons.ToArray();

					listOfButtons.Clear();

				});


			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message + " " + ex.StackTrace);
			}



		}


		private void SetBold()
		{ 
			var result = webView.EvaluateJavascript(@"document.execCommand('bold')");
			var htmlCode = webView.EvaluateJavascript(@"document.body.innerHTML");
			Console.WriteLine(htmlCode);
		}


		partial void BtnBold_TouchUpInside(UIButton sender)
		{
			SetBold();

		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		private void LoadHtmlToRichText()
		{
			string contentDirectoryPath = Path.Combine(NSBundle.MainBundle.BundlePath, "Content/");

			string fileName = "Content/Index.html"; // remember case-sensitive
			string localHtmlUrl = Path.Combine(NSBundle.MainBundle.BundlePath, fileName);
			Html = File.ReadAllText(Path.Combine(NSBundle.MainBundle.BundlePath, fileName));

			webView.LoadHtmlString(Html,new NSUrl(contentDirectoryPath, true));
			webView.ScalesPageToFit = false;
		}


		void AddGesturesToRichTextBox()
		{
			var rteGestureRecognizer = new RTEGestureRecognizer();

			// NSSet *touches, UIEvent *event
			rteGestureRecognizer.TouchesDidBegin += (touches, evt) =>
			{
				var uiTouch = evt.touches.AnyObject as UITouch;
				var touchPoint = ((RTEGestureRecognizer)touches).LocationInView(this.View);

				var javascript = string.Format(@"document.elementFromPoint({0},{1})", touchPoint.X, touchPoint.Y);
				var elementAtPoint = webView.EvaluateJavascript(javascript);


				Console.WriteLine(elementAtPoint);
				System.Diagnostics.Trace.WriteLine("Element:" + elementAtPoint);
			};

			rteGestureRecognizer.TouchesDidEnd += (touches, evt) =>
			{
				var uiTouch = evt.touches.AnyObject as UITouch;
				var touchPoint = ((RTEGestureRecognizer)touches).LocationInView(this.View);
			};

			webView.ScrollView.AddGestureRecognizer(rteGestureRecognizer);
		}
	}
}

