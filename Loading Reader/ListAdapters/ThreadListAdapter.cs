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
using GuardianLamppost.LoadingReader.Common.Entitites;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace GuardianLamppost.LoadingReader.ListAdapters {
    public class ThreadListAdapter : ArrayAdapter<ThreadListThread> {
        public ThreadListAdapter(Context context, int textViewResourceId, List<ThreadListThread> objects) : base(context, textViewResourceId, objects) {
        }

        public override View GetView(int position, View convertView, ViewGroup parent) {
            var view = convertView;

            if (view == null) {
                LayoutInflater vi;
                vi = LayoutInflater.From(this.Context);
                view = vi.Inflate(Resource.Layout.threadListItem, null);
            }

            ThreadListThread thread = GetItem(position);

            if (thread != null) {
                ImageView statusImage = view.FindViewById<ImageView>(Resource.Id.threadStatusImage);
                statusImage.SetImageBitmap(BitmapFactory.DecodeByteArray(thread.StatusIcon.ImageData, 0, thread.StatusIcon.ImageData.Length));
                TextView threadTitle = view.FindViewById<TextView>(Resource.Id.threadTitle);
                TextView createdBy = view.FindViewById<TextView>(Resource.Id.createdByText);
                TextView lastChangedBy = view.FindViewById<TextView>(Resource.Id.lastChangedByText);

                threadTitle.Text = thread.Subject;
                createdBy.Text = String.Format("Skapad: {0} av {1}", thread.CreatedAt, thread.CreatedBy);
                lastChangedBy.Text = String.Format("Ändrad: {0} av {1}", thread.LastPostAt, thread.LastPostBy);
            }
            return view;
        }
    }
}