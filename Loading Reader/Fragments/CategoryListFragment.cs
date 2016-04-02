using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using GuardianLamppost.LoadingReader.Common.Entitites;
using GuardianLamppost.LoadingReader.ListAdapters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuardianLamppost.LoadingReader.Fragments {
    public class CategoryListFragment : Fragment {
        private ListView CategoryListView { get; set; }
        private SwipeRefreshLayout RefreshLayout { get; set; }
        private Android.App.ProgressDialog ProgressDialog { get; set; }
        public List<CategoryListItem> CategoryItems { get; set; }

        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
        }

        public static CategoryListFragment NewInstance() {
            var categoryListFragment = new CategoryListFragment { Arguments = new Bundle() };
            return categoryListFragment;
        }

        public override void OnActivityCreated(Bundle savedInstanceState) {
            base.OnActivityCreated(savedInstanceState);
            CategoryListView = View.FindViewById<ListView>(Resource.Id.categoryListView);
            RefreshLayout = View.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout);
            //RegisterForContextMenu(CategoryListView);
            ProgressDialog = new Android.App.ProgressDialog(View.Context);
            if (CategoryItems == null) {
                RefreshCategories();
            } else {
                UpdateCategories();
            }
        }

        public override void OnCreateContextMenu(IContextMenu menu, View vValue, IContextMenuContextMenuInfo menuInfo) {
            base.OnCreateContextMenu(menu, vValue, menuInfo);
            /*if (vValue.Id == Resource.Id.threadListView) {
                MenuInflater inflater = Activity.MenuInflater;
                inflater.Inflate(Resource.Menu.thread_list_long_press, menu);
            }*/
        }

        private void UpdateCategories() {
            CategoryListView.Adapter = new CategoryListAdapter(View.Context, 0, CategoryItems);
        }

        private void RefreshCategories() {
            ProgressDialog.SetTitle("Hämtar kategorier...");
            ProgressDialog.Show();
            Task.Run(() => LoadingReader.LoadingClient.ForumReader.GetCategories()).ContinueWith((task) => {
                Activity.RunOnUiThread(() => {
                    ProgressDialog.Hide();
                    CategoryItems = task.Result;
                    if (CategoryItems == null) {
                        Activity.Recreate();
                    } else {
                        UpdateCategories();
                    }
                });
            });
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.categoryListFragment, null);

            return view;
        }
    }
}