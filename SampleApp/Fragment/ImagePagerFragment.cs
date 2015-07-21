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
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Nostra13UniversalImageLoader.Core;
using Nostra13UniversalImageLoader.Core.Assist;
using Nostra13UniversalImageLoader.Core.Display;
using Nostra13UniversalImageLoader.Core.Listener;

namespace Nostra13UniversalImageLoader.SampleApp.Fragment
{
    /**
     * @author Sergey Tarasevich (nostra13[at]gmail[dot]com)
     */
    public class ImagePagerFragment : BaseFragment
    {
	    public const int INDEX = 2;

	    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
		    View rootView = inflater.Inflate(Resource.Layout.fr_image_pager, container, false);
		    ViewPager pager = rootView.FindViewById<ViewPager>(Resource.Id.pager);
		    pager.Adapter = new ImageAdapter(Activity);
		    pager.CurrentItem = Arguments.GetInt(Constants.Extra.IMAGE_POSITION, 0);
		    return rootView;
	    }

        private class ImageAdapter : PagerAdapter
        {
            private static readonly string[] IMAGE_URLS = Constants.IMAGES;

            private LayoutInflater inflater;
            private DisplayImageOptions options;

            public ImageAdapter(Context context)
            {
                inflater = LayoutInflater.From(context);

                options = new DisplayImageOptions.Builder()
                        .ShowImageForEmptyUri(Resource.Drawable.ic_empty)
                        .ShowImageOnFail(Resource.Drawable.ic_error)
                        .ResetViewBeforeLoading(true)
                        .CacheOnDisk(true)
                        .ImageScaleType(ImageScaleType.Exactly)
                        .BitmapConfig(Bitmap.Config.Rgb565)
                        .ConsiderExifParams(true)
                        .Displayer(new FadeInBitmapDisplayer(300))
                        .Build();
            }

            public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object obj)
            {
                container.RemoveView((View)obj);
            }

            public override int Count
            {
                get { return IMAGE_URLS.Length; }
            }

            public override Java.Lang.Object InstantiateItem(ViewGroup view, int position)
            {
                View imageLayout = inflater.Inflate(Resource.Layout.item_pager_image, view, false);
                System.Diagnostics.Debug.Assert(imageLayout != null);
                ImageView imageView = imageLayout.FindViewById<ImageView>(Resource.Id.image);
                ProgressBar spinner = imageLayout.FindViewById<ProgressBar>(Resource.Id.loading);

                ImageLoader.Instance.DisplayImage(IMAGE_URLS[position], imageView, options, new ThisSimpleImageLoadingListener(spinner));

                view.AddView(imageLayout, 0);
                return imageLayout;
            }

            public override bool IsViewFromObject(View view, Java.Lang.Object obj)
            {
                return view.Equals(obj);
            }

            public override void RestoreState(IParcelable state, Java.Lang.ClassLoader loader)
            {
            }

            public override IParcelable SaveState()
            {
                return null;
            }

            private class ThisSimpleImageLoadingListener : SimpleImageLoadingListener
            {
                private readonly ProgressBar mSpinner;

                public ThisSimpleImageLoadingListener(ProgressBar spinner)
                {
                    mSpinner = spinner;
                }

                public override void OnLoadingStarted(string imageUri, View view)
                {
                    mSpinner.Visibility = ViewStates.Visible;
                }

                public override void OnLoadingFailed(string imageUri, View view, FailReason failReason)
                {
                    string message = null;
                    if (failReason.Type == FailReason.FailType.IoError)
                    {
                        message = "Input/Output error";
                    }
                    else if (failReason.Type == FailReason.FailType.DecodingError)
                    {
                        message = "Image can't be decoded";
                    }
                    else if (failReason.Type == FailReason.FailType.NetworkDenied)
                    {
                        message = "Downloads are denied";
                    }
                    else if (failReason.Type == FailReason.FailType.OutOfMemory)
                    {
                        message = "Out Of Memory error";
                    }
                    else if (failReason.Type == FailReason.FailType.Unknown)
                    {
                        message = "Unknown error";
                    }
                    Toast.MakeText(view.Context, message, ToastLength.Short).Show();

                    mSpinner.Visibility = ViewStates.Gone;
                }

                public override void OnLoadingComplete(string imageUri, View view, Bitmap loadedImage)
                {
                    mSpinner.Visibility = ViewStates.Gone;
                }
            }
        }
    }
}