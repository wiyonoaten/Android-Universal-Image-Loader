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
using Android.Content;
using Android.OS;
using Android.Views;
using Java.IO;
using Java.Lang;
using Nostra13UniversalImageLoader.Core;
using Nostra13UniversalImageLoader.SampleApp.Fragment;
using Nostra13UniversalImageLoader.Utils;
using System.IO;

namespace Nostra13UniversalImageLoader.SampleApp.Activity
{
    /**
     * @author Sergey Tarasevich (nostra13[at]gmail[dot]com)
     */
    public class HomeActivity : Android.App.Activity
    {
	    private const string TEST_FILE_NAME = "Universal Image Loader @#&=+-_.,!()~'%20.png";

	    protected override void OnCreate(Bundle savedInstanceState)
        {
		    base.OnCreate(savedInstanceState);
		    SetContentView(Resource.Layout.ac_home);

            Java.IO.File testImageOnSdCard = new Java.IO.File("/mnt/sdcard", TEST_FILE_NAME);
		    if (!testImageOnSdCard.Exists())
            {
			    CopyTestImageToSdCard(testImageOnSdCard);
		    }
	    }

	    public void OnImageListClick(View view)
        {
		    Intent intent = new Intent(this, typeof(SimpleImageActivity));
		    intent.PutExtra(Constants.Extra.FRAGMENT_INDEX, ImageListFragment.INDEX);
		    StartActivity(intent);
	    }

	    public void OnImageGridClick(View view)
        {
		    Intent intent = new Intent(this, typeof(SimpleImageActivity));
		    intent.PutExtra(Constants.Extra.FRAGMENT_INDEX, ImageGridFragment.INDEX);
		    StartActivity(intent);
	    }

	    public void OnImagePagerClick(View view)
        {
		    Intent intent = new Intent(this, typeof(SimpleImageActivity));
		    intent.PutExtra(Constants.Extra.FRAGMENT_INDEX, ImagePagerFragment.INDEX);
		    StartActivity(intent);
	    }

	    public void OnImageGalleryClick(View view)
        {
		    Intent intent = new Intent(this, typeof(SimpleImageActivity));
		    intent.PutExtra(Constants.Extra.FRAGMENT_INDEX, ImageGalleryFragment.INDEX);
		    StartActivity(intent);
	    }

	    public void OnFragmentsClick(View view)
        {
		    Intent intent = new Intent(this, typeof(ComplexImageActivity));
		    StartActivity(intent);
	    }

	    public override void OnBackPressed()
        {
		    ImageLoader.Instance.Stop();
		    base.OnBackPressed();
	    }

	    public override bool OnCreateOptionsMenu(IMenu menu)
        {
		    MenuInflater.Inflate(Resource.Menu.main_menu, menu);
		    return true;
	    }

	    public override bool OnOptionsItemSelected(IMenuItem item)
        {
		    switch (item.ItemId)
            {
			    case Resource.Id.item_clear_memory_cache:
				    ImageLoader.Instance.ClearMemoryCache();
				    return true;
			    case Resource.Id.item_clear_disc_cache:
				    ImageLoader.Instance.ClearDiskCache();
				    return true;
			    default:
				    return false;
		    }
	    }

	    private void CopyTestImageToSdCard(Java.IO.File testImageOnSdCard)
        {
		    new Thread(new Runnable(() =>
            {
			    try
                {
					Stream is_ = Assets.Open(TEST_FILE_NAME);
					FileOutputStream fos = new FileOutputStream(testImageOnSdCard);
					byte[] buffer = new byte[8192];
					int read;
					try
                    {
						while ((read = is_.Read(buffer, 0, buffer.Length)) != -1)
                        {
							fos.Write(buffer, 0, read);
						}
					}
                    finally
                    {
						fos.Flush();
						fos.Close();
						is_.Close();
					}
				}
                catch (Java.IO.IOException e)
                {
					L.W("Can't copy test image onto SD card");
				}
		    })).Start();
	    }
    }
}