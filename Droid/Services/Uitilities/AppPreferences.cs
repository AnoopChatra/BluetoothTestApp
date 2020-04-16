using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BluetoothTestApp.Droid.Services.Uitilities
{
    class AppPreferences
    {
        private ISharedPreferences _sharedPreferences;
        private ISharedPreferencesEditor _sharedPreferencesEditor;
        private Context _context;

        public AppPreferences(Context context)
        {
            _context = context;
            _sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(context);
            _sharedPreferencesEditor = _sharedPreferences.Edit();
        }

        public void SaveAccessKey(string key,string value)
        {
            _sharedPreferencesEditor.PutString(key, value);
            _sharedPreferencesEditor.Commit();
        }

        public string GetAccessKey(string key)
        {
            return _sharedPreferences.GetString(key, "");
        }

    }
}