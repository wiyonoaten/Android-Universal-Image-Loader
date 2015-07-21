/*******************************************************************************
 * Copyright 2011-2013 Sergey Tarasevich
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
using Android.Appwidget;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Nostra13UniversalImageLoader.Core;
using Nostra13UniversalImageLoader.Core.Assist;
using Nostra13UniversalImageLoader.Core.Listener;

namespace Nostra13UniversalImageLoader.SampleApp.Widget
{
    /**
     * Example widget provider
     * 
     * @author Sergey Tarasevich (nostra13[at]gmail[dot]com)
     */
    public class UILWidgetProvider : AppWidgetProvider
    {
	    private static DisplayImageOptions displayOptions;

	    static UILWidgetProvider()
        {
		    displayOptions = DisplayImageOptions.CreateSimple();
	    }

	    public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
		    UILApplication.InitImageLoader(context);

		    int widgetCount = appWidgetIds.Length;
		    for (int i = 0; i < widgetCount; i++)
            {
			    int appWidgetId = appWidgetIds[i];
			    UpdateAppWidget(context, appWidgetManager, appWidgetId);
		    }
	    }

	    internal static void UpdateAppWidget(Context context, AppWidgetManager appWidgetManager, int appWidgetId)
        {
		    RemoteViews views = new RemoteViews(context.PackageName, Resource.Layout.widget);

		    ImageSize minImageSize = new ImageSize(70, 70); // 70 - approximate size of ImageView in widget
            ImageLoader.Instance
                    .LoadImage(Constants.IMAGES[0], minImageSize, displayOptions, new ThisSimpleImageLoadingListener(appWidgetManager, appWidgetId, views, Resource.Id.image_left));
		    ImageLoader.Instance
				    .LoadImage(Constants.IMAGES[1], minImageSize, displayOptions, new ThisSimpleImageLoadingListener(appWidgetManager, appWidgetId, views, Resource.Id.image_right));
        }

        private class ThisSimpleImageLoadingListener : SimpleImageLoadingListener
        {
            private readonly AppWidgetManager mAppWidgetManager;
            private readonly int mAppWidgetId;
            private readonly RemoteViews mRemoteViews;
            private readonly int mImageResId;

            public ThisSimpleImageLoadingListener(AppWidgetManager appWidgetManager, int appWidgetId, RemoteViews remoteViews, int imageResId)
            {
                mAppWidgetManager = appWidgetManager;
                mAppWidgetId = appWidgetId;
                mRemoteViews = remoteViews;
                mImageResId = imageResId;
            }

            public override void OnLoadingComplete(string imageUri, View view, Bitmap loadedImage)
            {
                mRemoteViews.SetImageViewBitmap(mImageResId, loadedImage);
                mAppWidgetManager.UpdateAppWidget(mAppWidgetId, mRemoteViews);
            }
        }
    }
}