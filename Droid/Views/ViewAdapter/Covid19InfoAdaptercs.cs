using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.ViewPager.Widget;
using Object = Java.Lang.Object;

namespace BluetoothTestApp.Droid.Views.ViewAdapter
{
   public  class Covid19InfoAdaptercs : PagerAdapter
    {
        private int[] _imageIds;
        private string[] _contents;
        private Context _context;
        public Covid19InfoAdaptercs(Context context, int[] imageId, string[] content)
        {
            _imageIds = imageId;
            _contents = content;
            _context = context;

        }
        public override bool IsViewFromObject(View view, Object @object)
        {
            return view == @object;
        }

        public override Object InstantiateItem(ViewGroup container, int position)
        {

            View view = LayoutInflater.From(_context).Inflate(Resource.Layout.Pager_Item, container, false);
            ImageView imageView = view.FindViewById<ImageView>(Resource.Id.image);
            TextView textContent = view.FindViewById<TextView>(Resource.Id.text);
            imageView.SetBackgroundResource(_imageIds[position]);
            textContent.Text = _contents[position];
            container.AddView(view);
            return view;
        }

        public override int Count
        {
            get { return _imageIds.Length; }
        }

        public override void DestroyItem(ViewGroup container, int position, Object @object)
        {
            container.RemoveView((View)@object);
        }

        public override int GetItemPosition(Object @object)
        {
            return base.GetItemPosition(@object);
        }
    }
}