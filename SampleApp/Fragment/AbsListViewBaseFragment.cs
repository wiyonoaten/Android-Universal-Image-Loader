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
using Android.Views;
using Android.Widget;
using Nostra13UniversalImageLoader.Core;
using Nostra13UniversalImageLoader.Core.Listener;
using Nostra13UniversalImageLoader.SampleApp.Activity;

namespace Nostra13UniversalImageLoader.SampleApp.Fragment
{
    /**
     * @author Sergey Tarasevich (nostra13[at]gmail[dot]com)
     */
    public abstract class AbsListViewBaseFragment : BaseFragment
    {
	    protected const string STATE_PAUSE_ON_SCROLL = "STATE_PAUSE_ON_SCROLL";
	    protected const string STATE_PAUSE_ON_FLING = "STATE_PAUSE_ON_FLING";

	    protected AbsListView listView;

	    protected bool pauseOnScroll = false;
	    protected bool pauseOnFling = true;

	    public override void OnResume()
        {
		    base.OnResume();
		    ApplyScrollListener();
	    }

	    public override void OnPrepareOptionsMenu(IMenu menu)
        {
		    IMenuItem pauseOnScrollItem = menu.FindItem(Resource.Id.item_pause_on_scroll);
		    pauseOnScrollItem.SetVisible(true);
		    pauseOnScrollItem.SetChecked(pauseOnScroll);

		    IMenuItem pauseOnFlingItem = menu.FindItem(Resource.Id.item_pause_on_fling);
		    pauseOnFlingItem.SetVisible(true);
		    pauseOnFlingItem.SetChecked(pauseOnFling);
	    }

	    public override bool OnOptionsItemSelected(IMenuItem item)
        {
		    switch (item.ItemId)
            {
			    case Resource.Id.item_pause_on_scroll:
				    pauseOnScroll = !pauseOnScroll;
				    item.SetChecked(pauseOnScroll);
				    ApplyScrollListener();
				    return true;
			    case Resource.Id.item_pause_on_fling:
				    pauseOnFling = !pauseOnFling;
				    item.SetChecked(pauseOnFling);
				    ApplyScrollListener();
				    return true;
			    default:
				    return base.OnOptionsItemSelected(item);
		    }
	    }

	    protected void StartImagePagerActivity(int position)
        {
		    Intent intent = new Intent(Activity, typeof(SimpleImageActivity));
		    intent.PutExtra(Constants.Extra.FRAGMENT_INDEX, ImagePagerFragment.INDEX);
		    intent.PutExtra(Constants.Extra.IMAGE_POSITION, position);
		    StartActivity(intent);
	    }

	    private void ApplyScrollListener()
        {
		    listView.SetOnScrollListener(new PauseOnScrollListener(ImageLoader.Instance, pauseOnScroll, pauseOnFling));
	    }
    }
}