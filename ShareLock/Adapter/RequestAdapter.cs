using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using ShareLock.Models;

namespace ShareLock.Adapter
{
    class RequestAdapter : RecyclerView.Adapter
    {
        public event EventHandler<RequestAdapterClickEventArgs> ItemClick;
        public event EventHandler<RequestAdapterClickEventArgs> ItemLongClick;
        List<Request> items;


        public RequestAdapter(List<Request> data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.RequestItem;
            itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new RequestAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as RequestAdapterViewHolder;
            holder.VisitorName.Text = items[position].Fullname;
        }

        public override int ItemCount => items.Count;

        void OnClick(RequestAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(RequestAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class RequestAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView VisitorName { get; set; }


        public RequestAdapterViewHolder(View itemView, Action<RequestAdapterClickEventArgs> clickListener,
                            Action<RequestAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            VisitorName = (TextView)itemView.FindViewById(Resource.Id.visitorNameTxt);
            itemView.Click += (sender, e) => clickListener(new RequestAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new RequestAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class RequestAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}