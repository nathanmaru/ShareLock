using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ShareLock.Models;
using System;
using System.Collections.Generic;

namespace ShareLock.Adapter
{
    class SearchHomeAdapter : RecyclerView.Adapter
    {
        public event EventHandler<SearchHomeAdapterClickEventArgs> ItemClick;
        public event EventHandler<SearchHomeAdapterClickEventArgs> ItemLongClick;
        List<Home> items;

        public SearchHomeAdapter(List<Home> data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.SearchHouseItem;
            itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new SearchHomeAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as SearchHomeAdapterViewHolder;
            holder.HomeName.Text = items[position].HomeName;
            holder.HomeAddress.Text = items[position].HomeAddress;
        }

        public override int ItemCount => items.Count;

        void OnClick(SearchHomeAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(SearchHomeAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class SearchHomeAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView HomeName { get; set; }
        public TextView HomeAddress { get; set; }

        public SearchHomeAdapterViewHolder(View itemView, Action<SearchHomeAdapterClickEventArgs> clickListener,
                            Action<SearchHomeAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            HomeName = (TextView)itemView.FindViewById(Resource.Id.homeNameTxt);
            //HomeAddress = (TextView)itemView.FindViewById(Resource.Id.homeAddress);
            itemView.Click += (sender, e) => clickListener(new SearchHomeAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new SearchHomeAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class SearchHomeAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}