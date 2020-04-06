﻿using Android.App;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using System.Collections.Generic;
using Android.Content;
using BluetoothTestApp.Droid.Services;
using Android.Support.V4.App;
using Android;
using Android.Content.PM;
using BluetoothTestApp.Droid.Services.BluetoothLeClient;
using BluetoothTestApp.Droid.Views;
using BluetoothTestApp.Droid.Views.ViewAdapter;

namespace BluetoothTestApp.Droid
{
    [Activity(Label = "BluetoothTestApp", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        public static Context ActivityContext= null;
        private const int Request_Fine_Location = 0;
        private int employeeId;

        private BluetoothAdapter _bluetoothAdapter; 
        private BluetoothLeGattService _bluetoothLeService;
        private BluetoothLEClientService _bluetoothLeclintService;
        private EmployeeNearByListViewAdapter _employeeNearByListViewAdapter;
        private IList<EmployeeListViewItem> _employeeListViewItemList;

        private ListView _lvEmployeeNearBy;
        private TextView _tvNoData;

        private bool _isbleOperationStarted;

        private IList<BluetoothDevice> mConnectedDevices;
        private ArrayAdapter<BluetoothDevice> mConnectedDevicesAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _lvEmployeeNearBy = FindViewById<ListView>(Resource.Id.lvNearByEmployee);
            _tvNoData = FindViewById<TextView>(Resource.Id.tvNoData);

            ActivityContext = this;

            _employeeListViewItemList = new List<EmployeeListViewItem>();
            employeeId = Intent.GetIntExtra("EmployeeId", 0);
             
            _bluetoothAdapter = AndroidBluetoothServiceProvider.Instance.GetBluetoothAdapter();           
            _bluetoothLeService = BluetoothLeGattService.Instance;
            _bluetoothLeclintService = BluetoothLEClientService.Instance;

            UpdateUi();
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (!_isbleOperationStarted)
            {

                if (_bluetoothAdapter == null || !_bluetoothAdapter.IsEnabled)
                {
                    //Bluetooth is disabled
                    Intent enableBtIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
                    StartActivity(enableBtIntent);
                    Finish();
                    return;
                }

                if (!_bluetoothAdapter.IsMultipleAdvertisementSupported)
                {
                    Toast.MakeText(this, "No Advertising Support.", ToastLength.Short).Show();
                    Finish();
                    return;
                }

                _bluetoothLeService.OpenGattServer();
                _bluetoothLeService.InitGattServer();
                _bluetoothLeService.StartGattAdvertising(employeeId);

                RequestLocationPermission();

                _isbleOperationStarted = true;
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
           
            if (requestCode == Request_Fine_Location)
            {
                if (grantResults[0] == Permission.Granted)
                {
                    _bluetoothLeclintService.StartBluetoothLeScan();
                }
                else
                {
                    RequestLocationPermission();
                }
            }
        }

        private void UpdateUi()
        {
            if (_employeeListViewItemList.Count == 0)
            {
                _lvEmployeeNearBy.Visibility = Android.Views.ViewStates.Gone;
                _tvNoData.Visibility = Android.Views.ViewStates.Visible;
            }
            else
            {
                _lvEmployeeNearBy.Visibility = Android.Views.ViewStates.Visible;
                _tvNoData.Visibility = Android.Views.ViewStates.Gone;
                if (null == _employeeNearByListViewAdapter)
                {
                    _employeeNearByListViewAdapter = new EmployeeNearByListViewAdapter(this, _employeeListViewItemList);
                    _lvEmployeeNearBy.Adapter = _employeeNearByListViewAdapter;
                }
                else
                {
                    _employeeListViewItemList.Clear();
                    _employeeNearByListViewAdapter.NotifyDataSetChanged();
                }
            }
        }

        private void RequestLocationPermission()
        {
            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != (int)Permission.Granted)
            {
                // Camera permission has not been granted
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.AccessFineLocation }, Request_Fine_Location);
            }
            else
            {
                _bluetoothLeclintService.StartBluetoothLeScan();
            }
        }
    }
}

