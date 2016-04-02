using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using GuardianLamppost.LoadingReader.Common.Entitites;
using GuardianLamppost.LoadingReader.ListAdapters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuardianLamppost.LoadingReader.Fragments {
    public class ThreadListFragment : Fragment {
        private ListView ThreadListView { get; set; }
        private SwipeRefreshLayout RefreshLayout { get; set; }
        private Android.App.ProgressDialog ProgressDialog { get; set; }
        public List<ThreadListThread> Threads { get; set; }
        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
        }

        public static ThreadListFragment NewInstance() {
            var threadListFragment = new ThreadListFragment { Arguments = new Bundle() };
            return threadListFragment;
        }

        public override void OnActivityCreated(Bundle savedInstanceState) {
            base.OnActivityCreated(savedInstanceState);
            ThreadListView = View.FindViewById<ListView>(Resource.Id.threadListView);
            RefreshLayout = View.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout);
            RegisterForContextMenu(ThreadListView);
            ProgressDialog = new Android.App.ProgressDialog(View.Context);
            if (Threads == null) {
                RefreshThreads();
            } else {
                UpdateThreads();
            }
        }

        public override void OnCreateContextMenu(IContextMenu menu, View vValue, IContextMenuContextMenuInfo menuInfo) {
            base.OnCreateContextMenu(menu, vValue, menuInfo);
            if (vValue.Id == Resource.Id.threadListView) {
                MenuInflater inflater = Activity.MenuInflater;
                inflater.Inflate(Resource.Menu.thread_list_long_press, menu);
            }
        }

        private void UpdateThreads() {
            ThreadListView.Adapter = new ThreadListAdapter(View.Context, 0, Threads);
        }

        private void RefreshThreads() {
            ProgressDialog.SetTitle("Hämtar trådar...");
            ProgressDialog.Show();
            Task.Run(() => LoadingReader.LoadingClient.ForumReader.GetThreads()).ContinueWith((task) => {
                Activity.RunOnUiThread(() => {
                    ProgressDialog.Hide();
                    Threads = task.Result;
                    UpdateThreads();
                });
            });
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.threadListFragment, null);

            return view;
        }

    }
}