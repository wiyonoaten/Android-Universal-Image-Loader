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
using Nostra13UniversalImageLoader.Core.Display;
using Nostra13UniversalImageLoader.Core.Listener;
using System.Collections.Generic;

namespace Nostra13UniversalImageLoader.SampleApp.Fragment
{
    /**
     * @author Sergey Tarasevich (nostra13[at]gmail[dot]com)
     */
    public class ImageListFragment : AbsListViewBaseFragment
    {
	    public const int INDEX = 0;

	    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
		    View rootView = inflater.Inflate(Resource.Layout.fr_image_list, container, false);
		    listView = rootView.FindViewById<ListView>(Android.Resource.Id.List);
		    ((ListView) listView).Adapter = new ImageAdapter(Activity);
            listView.ItemClick += (sender, args) =>
            {
                StartImagePagerActivity(args.Position);
            };
		    return rootView;
	    }

        public override void OnDestroy()
        {
		    base.OnDestroy();
		    AnimateFirstDisplayListener.DisplayedImages.Clear();
	    }

	    private class ImageAdapter : BaseAdapter
        {
		    private static readonly string[] IMAGE_URLS = Constants.IMAGES;

		    private LayoutInflater inflater;
		    private IImageLoadingListener animateFirstListener = new AnimateFirstDisplayListener();

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
					    .Displayer(new RoundedBitmapDisplayer(20))
                        .Build();
		    }

		    public override int Count
            {
			    get { return IMAGE_URLS.Length; }
		    }

		    public override Java.Lang.Object GetItem(int position)
            {
			    return position;
		    }

		    public override long GetItemId(int position)
            {
			    return position;
		    }

		    public override View GetView(int position, View convertView, ViewGroup parent)
            {
			    View view = convertView;
			    ViewHolder holder;
			    if (convertView == null)
                {
				    view = inflater.Inflate(Resource.Layout.item_list_image, parent, false);
				    holder = new ViewHolder();
				    holder.Text = view.FindViewById<TextView>(Resource.Id.text);
				    holder.Image = view.FindViewById<ImageView>(Resource.Id.image);
				    view.Tag = holder;
			    }
                else
                {
				    holder = (ViewHolder) view.Tag;
			    }

			    holder.Text.Text = "Item " + (position + 1);

			    ImageLoader.Instance.DisplayImage(IMAGE_URLS[position], holder.Image, options, animateFirstListener);

			    return view;
		    }
	    }

	    private class ViewHolder : Java.Lang.Object
        {
		    public TextView Text { get; set; }
		    public ImageView Image { get; set; }
	    }

	    private class AnimateFirstDisplayListener : SimpleImageLoadingListener
        {
		    public static readonly IList<string> DisplayedImages = new SynchronizedCollection<string>(new LinkedList<string>());

		    public override void OnLoadingComplete(string imageUri, View view, Bitmap loadedImage)
            {
			    if (loadedImage != null)
                {
				    ImageView imageView = (ImageView) view;
				    bool firstDisplay = !DisplayedImages.Contains(imageUri);
				    if (firstDisplay)
                    {
					    FadeInBitmapDisplayer.Animate(imageView, 500);
					    DisplayedImages.Add(imageUri);
				    }
			    }
		    }
	    }
    }
}