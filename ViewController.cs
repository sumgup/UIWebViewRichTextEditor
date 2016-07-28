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
		// 	var htmlCode = webView.EvaluateJavascript(@"document.body.innerHTML");

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
			CheckSelection();

			timer = new Timer(500);
			timer.Elapsed += OnTimerElasped;
			timer.Start();
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

					var boldUIBarButtonItem = new UIBarButtonItem(boldEnabled ? "[B]" : "B", UIBarButtonItemStyle.Plain, (sender, args) =>
					 {
						 webView.Bold();
					 });

					var italicBarButtonItem = new UIBarButtonItem(italicEnabled ? "[I]" : "I", UIBarButtonItemStyle.Plain, (sender, args) =>
					 {
						webView.Italic();
					 });

					var paragraphBarButtonItem = new UIBarButtonItem("P", UIBarButtonItemStyle.Plain, (sender, args) =>
					 {
						 webView.Paragraph();
					 });


					listOfButtons.Add(boldUIBarButtonItem);
					listOfButtons.Add(italicBarButtonItem);
					listOfButtons.Add(paragraphBarButtonItem);


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



		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

	}
}

