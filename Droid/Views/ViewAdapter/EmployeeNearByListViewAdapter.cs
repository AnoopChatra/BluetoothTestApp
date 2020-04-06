using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using BluetoothTestApp.Droid.Views.ViewAdapter;
using Object = Java.Lang.Object;

namespace BluetoothTestApp.Droid.Views
{
    public class EmployeeNearByListViewAdapter : BaseAdapter<EmployeeListViewItem>
    {
        private readonly Activity _context;
        private readonly IList<EmployeeListViewItem> _listData;

        public EmployeeNearByListViewAdapter(Activity context, IList<EmployeeListViewItem> empLoyeeList)
        {
            _context = context;
            _listData = empLoyeeList;
        }

        public override int Count => _listData.Count;

        public override EmployeeListViewItem this[int position] => _listData[position];

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            ListViewHolder holder = null;

            if(null != view)
            {
                holder = view.Tag as ListViewHolder;
            }

            if(null == holder)
            {
                holder = new ListViewHolder();
                view = _context.LayoutInflater.Inflate(Resource.Layout.NearByEmployeeListItem, null);
                holder.EmployeeId = view.FindViewById<TextView>(Resource.Id.tvEmployeeId);
                holder.TimeStamp = view.FindViewById<TextView>(Resource.Id.tvTime);
                view.Tag = holder;

            }
            holder.EmployeeId.Text = _listData[position].EmployeeId;
            holder.TimeStamp.Text = _listData[position].TimeStamp;

            return view;
        }
    }

    public class ListViewHolder : Object
    {
        public TextView EmployeeId { get; set; }
        public TextView TimeStamp { get; set; }

    }
}
