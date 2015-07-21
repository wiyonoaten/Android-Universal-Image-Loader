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
using Java.Lang;
using Nostra13UniversalImageLoader.Core;
using Nostra13UniversalImageLoader.SampleApp.Activity;

namespace Nostra13UniversalImageLoader.SampleApp.Fragment
{
    /**
     * @author Sergey Tarasevich (nostra13[at]gmail[dot]com)
     */
    public class ImageGalleryFragment : BaseFragment
    {
	    public const int INDEX = 3;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
		    View rootView = inflater.Inflate(Resource.Layout.fr_image_gallery, container, false);
#pragma warning disable CS0618 // Type or member is obsolete
            Gallery gallery = (Gallery) rootView.FindViewById(Resource.Id.gallery);
#pragma warning restore CS0618 // Type or member is obsolete
            gallery.Adapter = new ImageAdapter(Activity);
		    gallery.ItemClick += (sender, args) =>
            {
                StartImagePagerActivity(args.Position);
		    };
		    return rootView;
	    }

	    private void StartImagePagerActivity(int position)
        {
		    Intent intent = new Intent(Activity, typeof(SimpleImageActivity));
		    intent.PutExtra(Constants.Extra.FRAGMENT_INDEX, ImagePagerFragment.INDEX);
		    intent.PutExtra(Constants.Extra.IMAGE_POSITION, position);
		    StartActivity(intent);
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

		    public override Object GetItem(int position)
            {
			    return position;
		    }

		    public override long GetItemId(int position)
            {
			    return position;
		    }

		    public override View GetView(int position, View convertView, ViewGroup parent)
            {
			    ImageView imageView = (ImageView) convertView;
			    if (imageView == null)
                {
				    imageView = (ImageView) inflater.Inflate(Resource.Layout.item_gallery_image, parent, false);
			    }
			    ImageLoader.Instance.DisplayImage(IMAGE_URLS[position], imageView, options);
			    return imageView;
		    }
	    }
    }
}