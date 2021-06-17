using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ShareLock.Models;
using System;
using System.Collections.Generic;

namespace ShareLock.Adapter
{
    class SavedHomeAdapter : RecyclerView.Adapter
    {
        public event EventHandler<SavedHomeAdapterClickEventArgs> ItemClick;
        public event EventHandler<SavedHomeAdapterClickEventArgs> ItemLongClick;
        List<Home> items;

        public SavedHomeAdapter(List<Home> data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.HomeItem;
            itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new SavedHomeAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as SavedHomeAdapterViewHolder;
            holder.HomeName.Text = items[position].HomeName;
        }

        public override int ItemCount => items.Count;

        void OnClick(SavedHomeAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(SavedHomeAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class SavedHomeAdapterViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }
        public TextView HomeName { get; set; }

        public SavedHomeAdapterViewHolder(View itemView, Action<SavedHomeAdapterClickEventArgs> clickListener,
                            Action<SavedHomeAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            HomeName = (TextView)itemView.FindViewById(Resource.Id.homeNameTxt);
            itemView.Click += (sender, e) => clickListener(new SavedHomeAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new SavedHomeAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class SavedHomeAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}