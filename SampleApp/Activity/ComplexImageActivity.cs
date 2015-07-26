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
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Java.Lang;
using Nostra13UniversalImageLoader.SampleApp.Fragment;

namespace Nostra13UniversalImageLoader.SampleApp.Activity
{
    /**
     * @author Sergey Tarasevich (nostra13[at]gmail[dot]com)
     */
    [Activity(Label = "@string/ac_name_complex")]
    public class ComplexImageActivity : FragmentActivity
    {
	    private const string STATE_POSITION = "STATE_POSITION";

	    private ViewPager pager;

	    protected override void OnCreate(Bundle savedInstanceState)
        {
		    base.OnCreate(savedInstanceState);
		    SetContentView(Resource.Layout.ac_complex);

		    int pagerPosition = savedInstanceState == null ? 0 : savedInstanceState.GetInt(STATE_POSITION);

		    pager = FindViewById<ViewPager>(Resource.Id.pager);
		    pager.Adapter = new ImagePagerAdapter(SupportFragmentManager, Resources);
		    pager.CurrentItem = pagerPosition;
	    }

	    protected override void OnSaveInstanceState(Bundle outState)
        {
		    outState.PutInt(STATE_POSITION, pager.CurrentItem);
	    }

	    private class ImagePagerAdapter : FragmentPagerAdapter
        {
            readonly Resources mResources;
            Android.Support.V4.App.Fragment listFragment;
            Android.Support.V4.App.Fragment gridFragment;
            
            public ImagePagerAdapter(Android.Support.V4.App.FragmentManager fm, Resources resources)
                : base(fm)
            {
                mResources = resources;

                listFragment = new ImageListFragment();
			    gridFragment = new ImageGridFragment();
		    }

		    public override int Count
            {
			    get { return 2; }
		    }

		    public override Android.Support.V4.App.Fragment GetItem(int position)
            {
			    switch (position)
                {
				    case 0:
					    return listFragment;
				    case 1:
					    return gridFragment;
				    default:
					    return null;
			    }
		    }

		    public override ICharSequence GetPageTitleFormatted(int position)
            {
			    switch (position)
                {
				    case 0:
					    return mResources.GetTextFormatted(Resource.String.title_list);
				    case 1:
					    return mResources.GetTextFormatted(Resource.String.title_grid);
				    default:
					    return null;
			    }
		    }
	    }
    }
}