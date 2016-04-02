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
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Content.PM;
using GuardianLamppost.LoadingReader.Fragments;

namespace GuardianLamppost.LoadingReader {
    [Activity(Label = "MainActivity", Icon = "@drawable/Icon")]
    public class MainActivity : BaseActivity {
        protected override int LayoutResource {
            get {
                return Resource.Layout.Main;
            }
        }

        private DrawerLayout DrawerLayout { get; set; }
        public NavigationView NavigationView { get; set; }
        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            DrawerLayout = this.FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            //Set hamburger items menu
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);

            //setup navigation view
            NavigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            //handle navigation
            NavigationView.NavigationItemSelected += (sender, e) => {
                e.MenuItem.SetChecked(true);

                switch (e.MenuItem.ItemId) {
                    case Resource.Id.forum:
                        ListItemClicked(0);
                        break;
                    case Resource.Id.watched:
                        ListItemClicked(1);
                        break;
                    case Resource.Id.quotes:
                        ListItemClicked(2);
                        break;
                    case Resource.Id.pm:
                        ListItemClicked(3);
                        break;
                    case Resource.Id.news:
                        ListItemClicked(4);
                        break;
                    case Resource.Id.blogs:
                        ListItemClicked(5);
                        break;
                }

                DrawerLayout.CloseDrawers();
            };


            //if first time you will want to go ahead and click first item.
            if (savedInstanceState == null) {
                ListItemClicked(0);
            }
        }

        private void ListItemClicked(int position) {

            Android.Support.V4.App.Fragment fragment = null;
            switch (position) {
                case 0:
                    if (!LoadingReader.LoadingClient.IsCategorisedView) {
                        fragment = CategoryListFragment.NewInstance();
                    } else {
                        fragment = ThreadListFragment.NewInstance();
                    }
                    break;
            }

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .Commit();
        }



        public override bool OnOptionsItemSelected(IMenuItem item) {
            switch (item.ItemId) {
                case Android.Resource.Id.Home:
                    DrawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}