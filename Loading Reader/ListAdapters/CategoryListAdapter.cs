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
    public class CategoryListAdapter : ArrayAdapter<CategoryListItem> {
        public CategoryListAdapter(Context context, int textViewResourceId, List<CategoryListItem> objects) : base(context, textViewResourceId, objects) {
        }

        public override View GetView(int position, View convertView, ViewGroup parent) {
            var view = convertView;
            CategoryListItem item = GetItem(position);

            if (item is CategoryListCategory) {
                if (view == null || view.Id != Resource.Layout.categoryListCategory) {
                    LayoutInflater vi;
                    vi = LayoutInflater.From(this.Context);
                    view = vi.Inflate(Resource.Layout.categoryListCategory, null);
                }
            } else {
                if (view == null || view.Id != Resource.Layout.categoryListForum) {
                    LayoutInflater vi;
                    vi = LayoutInflater.From(this.Context);
                    view = vi.Inflate(Resource.Layout.categoryListForum, null);
                }
            }





            if (item != null) {
                if (item is CategoryListCategory) {
                    var category = item as CategoryListCategory;
                    var categoryTitle = view.FindViewById<TextView>(Resource.Id.categoryName);
                    categoryTitle.Text = category.Title ?? string.Empty;
                } else {
                    var forum = item as CategoryListForum;
                    var forumTitle = view.FindViewById<TextView>(Resource.Id.forumTitle);
                    var statusImage = view.FindViewById<ImageView>(Resource.Id.threadStatusImage);
                    var lastThreadTitle = view.FindViewById<TextView>(Resource.Id.lastThreadTitle);
                    var lastChanged = view.FindViewById<TextView>(Resource.Id.lastChangedByText);

                    statusImage.SetImageBitmap(BitmapFactory.DecodeByteArray(forum.StatusIcon.ImageData, 0, forum.StatusIcon.ImageData.Length));
                    forumTitle.Text = forum.Title ?? string.Empty;
                    lastThreadTitle.Text = forum.LastThreadTitle ?? string.Empty;
                    lastChanged.Text = string.Format("Ändrad: {0} av {1}", forum.LastChangeAt ?? string.Empty, forum.LastChangeBy ?? string.Empty);
                }
            }
            return view;
        }
    }
}