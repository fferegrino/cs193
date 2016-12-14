using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.iOS;
using Xamarin.UITest.Queries;

namespace CalculatorTests
{
	[TestFixture]
	public class Tests
	{
		iOSApp app;

		[SetUp]
		public void BeforeEachTest()
		{
			// TODO: If the iOS app being tested is included in the solution then open
			// the Unit Tests window, right click Test Apps, select Add App Project
			// and select the app projects that should be tested.
			//
			// The iOS project should have the Xamarin.TestCloud.Agent NuGet package
			// installed. To start the Test Cloud Agent the following code should be
			// added to the FinishedLaunching method of the AppDelegate:
			//
			//    #if ENABLE_TEST_CLOUD
			//    Xamarin.Calabash.Start();
			//    #endif
			app = ConfigureApp
				.iOS
				// TODO: Update this path to point to your iOS app and uncomment the
				// code if the app is not included in the solution.
				//.AppBundle ("../../../iOS/bin/iPhoneSimulator/Debug/Calculator.app")

				.StartApp();
		}

		[Test]
		public void SimpleCalculation()
		{
			app.PressButton(7);
			app.PressButton("+");
			app.PressButton(8);
			app.PressButton("=");
			var result = app.GetDisplayResult();
			Assert.AreEqual(result, 15d);
		}

		[Test]
		public void MemoryButtonTest()
		{
			app.PressButton(9);
			app.PressButton("+");
			app.PressButton("M");
			app.PressButton("=");
			app.PressButton("√");

			var result = app.GetDisplayResult();
			Assert.AreEqual(3d,result);


			app.PressButton(7);
			app.PressButton("→M");

			result = app.GetDisplayResult();
			Assert.AreEqual(4d,result);


			app.PressButton("+");
			app.PressButton(1);
			app.PressButton(4);
			app.PressButton("=");

			result = app.GetDisplayResult();
			Assert.AreEqual(18d, result);

		}

	}

	public static class CalculatorAppExtensions
	{
		public static void PressButton(this iOSApp app, int digit)
		{
			app.Tap(b => b.Button(digit.ToString()));
		}

		public static void PressButton(this iOSApp app, string button)
		{
			app.Tap(b => b.Button(button));
		}

		public static double GetDisplayResult(this iOSApp app)
		{
			var result = app.Query(view => view.Id("display")).FirstOrDefault()?.Text;
			return double.Parse(result);
		}
	}


}
