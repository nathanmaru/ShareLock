using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using ShareLock.Models;
using ShareLock;
using System.Collections.Generic;


namespace ShareLock.Adapter
{
    class MemberAdapter : RecyclerView.Adapter
    {
        public event EventHandler<MemberAdapterClickEventArgs> ItemClick;
        public event EventHandler<MemberAdapterClickEventArgs> ItemLongClick;
        List<Members> Items;

        public MemberAdapter(List<Members>Data)
        {
            Items = Data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.MemberItem;
            itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new MemberAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            //var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as MemberAdapterViewHolder;
            //holder.TextView.Text = items[position];
            holder.Fullname.Text = Items[position].Fullname;
            //holder.Role.Text = Items[position].Role;
        }

        public override int ItemCount => Items.Count;

        void OnClick(MemberAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(MemberAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class MemberAdapterViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }
        public TextView Fullname { get; set; }
        //public TextView Role { get; set; }

        public MemberAdapterViewHolder(View itemView, Action<MemberAdapterClickEventArgs> clickListener,
                            Action<MemberAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            Fullname = (TextView)itemView.FindViewById(Resource.Id.fullnameTxt);
            //Role = (TextView)itemView.FindViewById(Resource.Id.roleTxt);
            itemView.Click += (sender, e) => clickListener(new MemberAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new MemberAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class MemberAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}