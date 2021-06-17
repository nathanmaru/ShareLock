using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using ShareLock.Models;

namespace ShareLock.Adapter
{
    class SubscriptionAdapter : RecyclerView.Adapter
    {
        public event EventHandler<SubscriptionAdapterClickEventArgs> ItemClick;
        public event EventHandler<SubscriptionAdapterClickEventArgs> ItemLongClick;
        List<SubscriptionPlan> items;

        public SubscriptionAdapter(List<SubscriptionPlan> data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.SubcriptionItem;
            itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new SubscriptionAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as SubscriptionAdapterViewHolder;
            holder.PlanName.Text = items[position].PlanName;
            holder.Price.Text = items[position].Price;
        }

        public override int ItemCount => items.Count;

        void OnClick(SubscriptionAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(SubscriptionAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class SubscriptionAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView PlanName{ get; set; }
        public TextView Price { get; set; }


        public SubscriptionAdapterViewHolder(View itemView, Action<SubscriptionAdapterClickEventArgs> clickListener,
                            Action<SubscriptionAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            PlanName = (TextView)itemView.FindViewById(Resource.Id.planNameTxt);
            Price = (TextView)itemView.FindViewById(Resource.Id.priceTxt);
            itemView.Click += (sender, e) => clickListener(new SubscriptionAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new SubscriptionAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class SubscriptionAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}