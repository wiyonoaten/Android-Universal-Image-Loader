/*******************************************************************************
 * Copyright 2011-2014 Sergey Tarasevich
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *******************************************************************************/
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Nostra13UniversalImageLoader.Core;
using Nostra13UniversalImageLoader.Core.Assist;
using Nostra13UniversalImageLoader.Core.Listener;
using System;

namespace Nostra13UniversalImageLoader.SampleApp.Fragment
{
    /**
     * @author Sergey Tarasevich (nostra13[at]gmail[dot]com)
     */
    public class ImageGridFragment : AbsListViewBaseFragment
    {
	    public const int INDEX = 1;

	    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
		    View rootView = inflater.Inflate(Resource.Layout.fr_image_grid, container, false);
		    listView = rootView.FindViewById<GridView>(Resource.Id.grid);
		    ((GridView) listView).Adapter = new ImageAdapter(Activity);
            listView.ItemClick += (sender, args) =>
            {
                StartImagePagerActivity(args.Position);
            };
		    return rootView;
	    }

	    private class ImageAdapter : BaseAdapter
        {
		    private static readonly string[] IMAGE_URLS = Constants.IMAGES;

		    private LayoutInflater inflater;

		    private DisplayImageOptions options;

		    public ImageAdapter(Context context)
            {
			    inflater = LayoutInflater.From(context);

			    options = new DisplayImageOptions.Builder()
					    .ShowImageOnLoading(Resource.Drawable.ic_stub)
					    .ShowImageForEmptyUri(Resource.Drawable.ic_empty)
					    .ShowImageOnFail(Resource.Drawable.ic_error)
					    .CacheInMemory(true)
					    .CacheOnDisk(true)
					    .ConsiderExifParams(true)
					    .BitmapConfig(Bitmap.Config.Rgb565)
					    .Build();
		    }

		    public override int Count
            {
                get { return IMAGE_URLS.Length; }
		    }

		    public override Java.Lang.Object GetItem(int position)
            {
			    return null;
		    }

		    public override long GetItemId(int position)
            {
			    return position;
		    }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
			    ViewHolder holder;
			    View view = convertView;
			    if (view == null)
                {
				    view = inflater.Inflate(Resource.Layout.item_grid_image, parent, false);
				    holder = new ViewHolder();
				    System.Diagnostics.Debug.Assert(view != null);
				    holder.ImageView = view.FindViewById<ImageView>(Resource.Id.image);
				    holder.ProgressBar = view.FindViewById<ProgressBar>(Resource.Id.progress);
				    view.Tag = holder;
			    }
                else
                {
				    holder = (ViewHolder) view.Tag;
			    }

			    ImageLoader.Instance
					    .DisplayImage(IMAGE_URLS[position], holder.ImageView, options, 
                            new ThisSimpleImageLoadingListener(holder), new ImageLoadingProgressListener(holder));

			    return view;
		    }

            private class ThisSimpleImageLoadingListener : SimpleImageLoadingListener
            {
                private readonly ViewHolder mHolder;

                public ThisSimpleImageLoadingListener(ViewHolder holder)
                {
                    mHolder = holder;
                }

                public override void OnLoadingStarted(string imageUri, View view)
                {
                    mHolder.ProgressBar.Progress = 0;
                    mHolder.ProgressBar.Visibility = ViewStates.Visible;
                }

                public override void OnLoadingFailed(string imageUri, View view, FailReason failReason)
                {
                    mHolder.ProgressBar.Visibility = ViewStates.Gone;
                }

                public override void OnLoadingComplete(string imageUri, View view, Bitmap loadedImage)
                {
                    mHolder.ProgressBar.Visibility = ViewStates.Gone;
                }
            }

            private class ImageLoadingProgressListener : Java.Lang.Object, IImageLoadingProgressListener
            {
                private readonly ViewHolder mHolder;

                public ImageLoadingProgressListener(ViewHolder holder)
                {
                    mHolder = holder;
                }

                public void OnProgressUpdate(string imageUri, View view, int current, int total)
                {
                    mHolder.ProgressBar.Progress = (int) Math.Round(100.0f * current / total);
                }
            }
	    }

	    private class ViewHolder : Java.Lang.Object
        {
		    public ImageView ImageView { get; set; }
		    public ProgressBar ProgressBar { get; set; }
	    }
    }
}