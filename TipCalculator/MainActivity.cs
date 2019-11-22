using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace TipCalculator
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        EditText inputBill;
        Button calculateButton;
        Button TestCrashButton;
        TextView outputTip;
        TextView outputTotal;
        protected override void OnCreate(Bundle bundle)
        {
            AppCenter.Start("0c990751-fa61-4a50-a1c4-56f709d63749",
                   typeof(Analytics), typeof(Crashes));
            Analytics.TrackEvent("App Center service loaded.");
            base.OnCreate(bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);
            SetContentView(Resource.Layout.content_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            inputBill = FindViewById<EditText>(Resource.Id.inputBill);

            outputTip = FindViewById<TextView>(Resource.Id.outputTip);
            outputTotal = FindViewById<TextView>(Resource.Id.outputTotal);

            TestCrashButton = FindViewById<Button>(Resource.Id.TestCrashButton);
            TestCrashButton.Click += OnTestButtonClick;

            calculateButton = FindViewById<Button>(Resource.Id.calculateButton);
            calculateButton.Click += OnCalculateClick;
        }

        void OnCalculateClick(object sender, EventArgs e)
        {
            string text = inputBill.Text;
            double bill = 0;
            if (double.TryParse(text, out bill))
            {
                var tip = bill / 1.1 * 0.1;
                tip = Math.Truncate(tip * 100) / 100;
                var total = bill - tip;
                total = Math.Truncate(total * 100) / 100;

                outputTip.Text = tip.ToString();
                outputTotal.Text = total.ToString();

                Analytics.TrackEvent("Calculate button clicked");
            }
        }

        void OnTestButtonClick(object sender, EventArgs e)
        {
            Analytics.TrackEvent("Crash button clicked");
            Crashes.GenerateTestCrash();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}

