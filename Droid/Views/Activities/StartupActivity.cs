using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;

using Android.Text;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Content;
using AndroidX.ViewPager.Widget;
using BluetoothTestApp.Droid.Views.SelfDiagnosis;
using BluetoothTestApp.Droid.Views.ViewAdapter;

namespace BluetoothTestApp.Droid
{
    [Activity(MainLauncher = true,Label = "Details",Theme = "@style/ActionBarTheme", Icon = "@mipmap/icon")]
    public class StartupActivity : AppCompatActivity
    {
        private ViewPager _viewPager;
        private int[] _colors;
        private int[] _images;
        private string[] _contents;
        private LinearLayout _dotLayout;
        TextView[] dot;
        private Button _buttonRegistration, _buttonLogin;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.StartupPage);
            _viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
            _dotLayout = FindViewById<LinearLayout>(Resource.Id.layout_dot);
            _buttonLogin = FindViewById<Button>(Resource.Id.buttonLogin);
            _buttonRegistration = FindViewById<Button>(Resource.Id.buttonRegistration);
            _buttonRegistration.Click += NavigateToRegistration;
            _buttonLogin.Click += NavigateToLogin;
            _colors = new int[]
            {
                Resource.Color.red,
                Resource.Color.green,
                Resource.Color.colorPrimaryDark,
                Resource.Color.colorAccent,
            };
            _contents = new string[]
            {
                "Say Hi without handshakes",
                "Avoid social gathering",
                "Keep a 6ft distance from people",
                "Join hands to keep out campus pandameic free"
            };
            _images = new int[]
            {
                Resource.Mipmap.imgViewPager1,
                Resource.Mipmap.imgViewPager2,
                Resource.Mipmap.imgViewPager3,
                Resource.Mipmap.imgViewPager4,
            };
            Covid19InfoAdaptercs adapter = new Covid19InfoAdaptercs(this, _images, _contents);
            _viewPager.Adapter = adapter;
            AddPageIndicatorViews();
            _viewPager.PageSelected += delegate
            {
                int currentPosition = _viewPager.CurrentItem;

                if (currentPosition > 0)
                    _dotLayout.GetChildAt(currentPosition - 1).Enabled = true;
                if (currentPosition < _dotLayout.ChildCount - 1)
                    _dotLayout.GetChildAt(currentPosition + 1).Enabled = true;

                _dotLayout.GetChildAt(currentPosition).Enabled = false;

            };




        }

        private void NavigateToLogin(object sender, EventArgs e)
        {
           
        }

        private void NavigateToRegistration(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(RegisterationActivity));
            StartActivity(intent);
        }

        private void AddPageIndicatorViews()
        {
            LinearLayout.LayoutParams lParams =
                new LinearLayout.LayoutParams((int)Resources.GetDimension(Resource.Dimension.PageIndicator_Radius),
                    (int)Resources.GetDimension(Resource.Dimension.PageIndicator_Radius));
            lParams.LeftMargin = 5;
            lParams.RightMargin = 5;

            for (int i = 0; i < _images.Length; i++)
            {
                View button = new View(this);
                button.LayoutParameters = lParams;

                //button.SetBackgroundDrawable(ContextCompat.GetDrawable(this, Resource.Drawable.));
                //button.SetBackgroundDrawable(ContextCompat.GetDrawable(this, Resource.Drawable.Page));
                button.Background = ContextCompat.GetDrawable(this, Resource.Drawable.PageIndicator_BgSelector);
                if (i == 0)
                    button.Enabled = false;

                _dotLayout.AddView(button);
            }
        }
    }
}
