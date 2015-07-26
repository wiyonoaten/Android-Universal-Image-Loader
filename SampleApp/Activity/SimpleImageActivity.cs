/*******************************************************************************
 * Copyright 2014 Sergey Tarasevich
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
using Android.App;
using Android.OS;
using Android.Support.V4.App;
using Nostra13UniversalImageLoader.SampleApp.Fragment;

namespace Nostra13UniversalImageLoader.SampleApp.Activity
{
    /**
     * @author Sergey Tarasevich (nostra13[at]gmail[dot]com)
     */
    [Activity(Label = "@string/ac_name_image_list")]
    public class SimpleImageActivity : FragmentActivity
    {
	    protected override void OnCreate(Bundle savedInstanceState)
        {
		    base.OnCreate(savedInstanceState);

		    int frIndex = Intent.GetIntExtra(Constants.Extra.FRAGMENT_INDEX, 0);
		    Android.Support.V4.App.Fragment fr;
		    string tag;
		    int titleRes;
		    switch (frIndex)
            {
			    default:
			    case ImageListFragment.INDEX:
				    tag = typeof(ImageListFragment).Name;
				    fr = SupportFragmentManager.FindFragmentByTag(tag);
				    if (fr == null)
                    {
					    fr = new ImageListFragment();
				    }
				    titleRes = Resource.String.ac_name_image_list;
				    break;
			    case ImageGridFragment.INDEX:
				    tag = typeof(ImageGridFragment).Name;
                    fr = SupportFragmentManager.FindFragmentByTag(tag);
				    if (fr == null)
                    {
					    fr = new ImageGridFragment();
				    }
				    titleRes = Resource.String.ac_name_image_grid;
				    break;
			    case ImagePagerFragment.INDEX:
				    tag = typeof(ImagePagerFragment).Name;
                    fr = SupportFragmentManager.FindFragmentByTag(tag);
				    if (fr == null)
                    {
					    fr = new ImagePagerFragment();
					    fr.Arguments = Intent.Extras;
				    }
				    titleRes = Resource.String.ac_name_image_pager;
				    break;
			    case ImageGalleryFragment.INDEX:
				    tag = typeof(ImageGalleryFragment).Name;
                    fr = SupportFragmentManager.FindFragmentByTag(tag);
				    if (fr == null)
                    {
					    fr = new ImageGalleryFragment();
				    }
				    titleRes = Resource.String.ac_name_image_gallery;
				    break;
		    }

		    SetTitle(titleRes);
		    SupportFragmentManager.BeginTransaction().Replace(Android.Resource.Id.Content, fr, tag).Commit();
	    }
    }
}