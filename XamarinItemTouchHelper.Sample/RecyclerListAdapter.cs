﻿using System;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.Linq;
using Java.Util;
using System.Collections.ObjectModel;
using Android.Support.V4.View;

namespace XamarinItemTouchHelper.Sample
{
    public class RecyclerListAdapter : RecyclerView.Adapter, IItemTouchHelperAdapter
    {
        /**
     * Listener for manual initiation of a drag.
     */
        public interface IOnStartDragListener {

            /**
         * Called when a view is requesting a start of a drag.
         *
         * @param viewHolder The holder of the view to drag.
         */
            void OnStartDrag(RecyclerView.ViewHolder viewHolder);
        }

        private static string[] STRINGS = new String[]{
            "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten"
        };

        private ObservableCollection<string> mItems = new ObservableCollection<string>();

        private IOnStartDragListener mDragStartListener;

        public RecyclerListAdapter(IOnStartDragListener dragStartListener) {
            mDragStartListener = dragStartListener;

            foreach (var item in STRINGS) {
                mItems.Add(item);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_main, parent, false);
            ItemViewHolder itemViewHolder = new ItemViewHolder(view);
            return itemViewHolder;
        }

        public class TouchListener : View.IOnTouchListener
        {
            private ItemViewHolder itemHolder;

            public TouchListener(ItemViewHolder holder)
            {
                itemHolder = holder;
            }

            public bool OnTouch (View v, MotionEvent e)
            {
                
                if (MotionEventCompat.GetActionMasked(e) == MotionEventCompat.ActionPointerDown) {
                    //TODO: Fix this
                    //mDragStartListener.OnStartDrag(itemHolder);
                }
                return false;
            }

            public void Dispose ()
            {
                throw new NotImplementedException ();
            }

            public IntPtr Handle {
                get {
                    throw new NotImplementedException ();
                }
            }
        }

        public override void OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
        {
            var itemHolder = (ItemViewHolder)holder;

            itemHolder.textView.Text = mItems.ElementAt(position);
            itemHolder.handleView.SetOnTouchListener (new TouchListener((ItemViewHolder)holder));
        }

        public void OnItemMove (int fromPosition, int toPosition)
        {
            mItems.Move(fromPosition, toPosition);
            NotifyItemMoved(fromPosition, toPosition);
        }
        public void OnItemDismiss (int position)
        {
            mItems.Remove(mItems.ElementAt(position));
            NotifyItemRemoved(position);
        }

        public override int ItemCount {
            get {
                return mItems.Count;
            }
        }

        /**
     * Simple example of a view holder that implements {@link ItemTouchHelperViewHolder} and has a
     * "handle" view that initiates a drag event when touched.
     */
        public class ItemViewHolder : RecyclerView.ViewHolder, IItemTouchHelperViewHolder {

            public TextView textView;
            public ImageView handleView;
            public View _itemView;

            public ItemViewHolder (View itemView) : base (itemView)
            {
                _itemView = itemView;
                textView = (TextView) itemView.FindViewById(Resource.Id.textView1);
                handleView = (ImageView) itemView.FindViewById(Resource.Id.imageView1);
            }

            public void OnItemSelected ()
            {
                _itemView.SetBackgroundColor(Color.LightGray);
            }

            public void OnItemClear ()
            {
                _itemView.SetBackgroundColor(Color.Transparent);
            }
        }
    }
}

