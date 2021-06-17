using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ShareLock.Models;
using System;
using System.Collections.Generic;

namespace ShareLock.Adapter
{
    class DoorLockAdapter : RecyclerView.Adapter
    {
        public event EventHandler<DoorLockAdapterClickEventArgs> ItemClick;
        public event EventHandler<DoorLockAdapterClickEventArgs> ItemLongClick;
        List<DoorLock> Items;

        public DoorLockAdapter(List<DoorLock> data)
        {
            Items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.DoorLockItem;
            itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new DoorLockAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            //var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as DoorLockAdapterViewHolder;
            //holder.TextView.Text = items[position];
            holder.DoorLockName.Text = Items[position].DoorLockName;
        }

        public override int ItemCount => Items.Count;

        void OnClick(DoorLockAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(DoorLockAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class DoorLockAdapterViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }
        public TextView DoorLockName { get; set; }

        public DoorLockAdapterViewHolder(View itemView, Action<DoorLockAdapterClickEventArgs> clickListener,
                            Action<DoorLockAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            DoorLockName = (TextView)itemView.FindViewById(Resource.Id.doorNameTxt);

            itemView.Click += (sender, e) => clickListener(new DoorLockAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new DoorLockAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class DoorLockAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}