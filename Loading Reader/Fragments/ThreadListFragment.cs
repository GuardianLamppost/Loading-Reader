using Android.OS;
using Android.Support.V4.App;
using Android.Views;

namespace GuardianLamppost.LoadingReader.Fragments {
    public class ThreadListFragment : Fragment {
        public override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static ThreadListFragment NewInstance() {
            var frag1 = new ThreadListFragment { Arguments = new Bundle() };
            return frag1;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return inflater.Inflate(Resource.Layout.threadListFragment, null);
        }
    }
}