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
using Android.OS;
using Android.Views;
using Nostra13UniversalImageLoader.Core;

namespace Nostra13UniversalImageLoader.SampleApp.Fragment
{
    /**
     * @author Sergey Tarasevich (nostra13[at]gmail[dot]com)
     */
    public abstract class BaseFragment : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            HasOptionsMenu = true;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.main_menu, menu);
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
    }
}