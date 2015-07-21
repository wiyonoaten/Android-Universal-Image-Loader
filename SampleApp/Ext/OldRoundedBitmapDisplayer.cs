/*******************************************************************************
 * Copyright 2011-2015 Sergey Tarasevich
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
using Android.Graphics;
using Android.Widget;
using Nostra13UniversalImageLoader.Core.Assist;
using Nostra13UniversalImageLoader.Core.Display;
using Nostra13UniversalImageLoader.Core.Imageaware;
using Nostra13UniversalImageLoader.Utils;
using System;

namespace Nostra13UniversalImageLoader.SampleApp.Ext
{
    /**
     * Displays bitmap with rounded corners. This implementation works only with ImageViews wrapped in ImageViewAware.<br />
     * <b>NOTE:</b> It's strongly recommended your {@link ImageView} has defined width (<i>layout_width</i>) and height
     * (<i>layout_height</i>) .<br />
     * <b>NOTE:</b> New {@link Bitmap} object is created for displaying. So this class needs more memory and can cause
     * {@link OutOfMemoryError}.
     *
     * @author Sergey Tarasevich (nostra13[at]gmail[dot]com)
     */
    public class OldRoundedBitmapDisplayer : Java.Lang.Object, IBitmapDisplayer
    {
        private readonly int roundPixels;

        public OldRoundedBitmapDisplayer(int roundPixels)
        {
            this.roundPixels = roundPixels;
        }

        public void Display(Bitmap bitmap, IImageAware imageAware, LoadedFrom loadedFrom)
        {
            if (!(imageAware is ImageViewAware))
            {
                throw new ArgumentException("ImageAware should wrap ImageView. ImageViewAware is expected.");
            }
            Bitmap roundedBitmap = RoundCorners(bitmap, (ImageViewAware)imageAware, roundPixels);
            imageAware.SetImageBitmap(roundedBitmap);
        }

        /**
	     * Process incoming {@linkplain Bitmap} to make rounded corners according to target
	     * {@link com.nostra13.universalimageloader.core.imageaware.ImageViewAware}.<br />
	     * This method <b>doesn't display</b> result bitmap in {@link ImageView}
	     *
	     * @param bitmap      Incoming Bitmap to process
	     * @param imageAware  Target {@link com.nostra13.universalimageloader.core.imageaware.ImageAware ImageAware} to
	     *                    display bitmap in
	     * @param roundPixels Rounded pixels of corner
	     * @return Result bitmap with rounded corners
	     */
        public static Bitmap RoundCorners(Bitmap bitmap, ImageViewAware imageAware, int roundPixels)
        {
            ImageView imageView = imageAware.WrappedView;
            if (imageView == null)
            {
                L.W("View is collected probably. Can't round bitmap corners without view properties.");
                return bitmap;
            }

            Bitmap roundBitmap;

            int bw = bitmap.Width;
            int bh = bitmap.Height;
            int vw = imageAware.Width;
            int vh = imageAware.Height;
            if (vw <= 0) vw = bw;
            if (vh <= 0) vh = bh;

            ImageView.ScaleType scaleType = imageView.GetScaleType();
            if (scaleType == null)
            {
                return bitmap;
            }

            int width, height;
            Rect srcRect;
            Rect destRect;
            if (scaleType == ImageView.ScaleType.CenterInside)
            {
                float vRation = (float)vw / vh;
                float bRation = (float)bw / bh;
                int destWidth;
                int destHeight;
                if (vRation > bRation)
                {
                    destHeight = Math.Min(vh, bh);
                    destWidth = (int)(bw / ((float)bh / destHeight));
                }
                else
                {
                    destWidth = Math.Min(vw, bw);
                    destHeight = (int)(bh / ((float)bw / destWidth));
                }
                int x = (vw - destWidth) / 2;
                int y = (vh - destHeight) / 2;
                srcRect = new Rect(0, 0, bw, bh);
                destRect = new Rect(x, y, x + destWidth, y + destHeight);
                width = vw;
                height = vh;
            }
            else if (scaleType == ImageView.ScaleType.CenterCrop)
            {
                float vRation = (float)vw / vh;
                float bRation = (float)bw / bh;
                int srcWidth;
                int srcHeight;
                int x;
                int y;
                if (vRation > bRation)
                {
                    srcWidth = bw;
                    srcHeight = (int)(vh * ((float)bw / vw));
                    x = 0;
                    y = (bh - srcHeight) / 2;
                }
                else
                {
                    srcWidth = (int)(vw * ((float)bh / vh));
                    srcHeight = bh;
                    x = (bw - srcWidth) / 2;
                    y = 0;
                }
                width = srcWidth;// Math.min(vw, bw);
                height = srcHeight;//Math.min(vh, bh);
                srcRect = new Rect(x, y, x + srcWidth, y + srcHeight);
                destRect = new Rect(0, 0, width, height);
            }
            else if (scaleType == ImageView.ScaleType.FitXy)
            {
                width = vw;
                height = vh;
                srcRect = new Rect(0, 0, bw, bh);
                destRect = new Rect(0, 0, width, height);
            }
            else if (scaleType == ImageView.ScaleType.Center
                || scaleType == ImageView.ScaleType.Matrix)
            {
                width = Math.Min(vw, bw);
                height = Math.Min(vh, bh);
                int x = (bw - width) / 2;
                int y = (bh - height) / 2;
                srcRect = new Rect(x, y, x + width, y + height);
                destRect = new Rect(0, 0, width, height);
            }
            else
            //if (scaleType == ImageView.ScaleType.FitCenter
            //|| scaleType == ImageView.ScaleType.FitStart
            //|| scaleType == ImageView.ScaleType.FitEnd)
            //|| default
            {
                float vRation = (float)vw / vh;
                float bRation = (float)bw / bh;
                if (vRation > bRation)
                {
                    width = (int)(bw / ((float)bh / vh));
                    height = vh;
                }
                else
                {
                    width = vw;
                    height = (int)(bh / ((float)bw / vw));
                }
                srcRect = new Rect(0, 0, bw, bh);
                destRect = new Rect(0, 0, width, height);
            }

            try
            {
                roundBitmap = GetRoundedCornerBitmap(bitmap, roundPixels, srcRect, destRect, width, height);
            }
            catch (Java.Lang.OutOfMemoryError e)
            {
                L.E(e, "Can't create bitmap with rounded corners. Not enough memory.");
                roundBitmap = bitmap;
            }

            return roundBitmap;
        }

        private static Bitmap GetRoundedCornerBitmap(Bitmap bitmap, int roundPixels, Rect srcRect, Rect destRect, int width,
			    int height)
        {
            Bitmap output = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(output);

            Paint paint = new Paint();
            RectF destRectF = new RectF(destRect);

            paint.AntiAlias = true;
            canvas.DrawARGB(0, 0, 0, 0);
            paint.Color = Color.Argb(0xFF, 0x00, 0x00, 0x00);
            canvas.DrawRoundRect(destRectF, roundPixels, roundPixels, paint);

            paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
            canvas.DrawBitmap(bitmap, srcRect, destRectF, paint);

            return output;
        }
    }
}