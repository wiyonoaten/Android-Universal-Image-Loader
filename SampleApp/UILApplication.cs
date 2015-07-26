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
using Android.Annotation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Nostra13UniversalImageLoader.Cache.Disc.Naming;
using Nostra13UniversalImageLoader.Core;
using Nostra13UniversalImageLoader.Core.Assist;
using System;

namespace Nostra13UniversalImageLoader.SampleApp
{
    /**
     * @author Sergey Tarasevich (nostra13[at]gmail[dot]com)
     */
    [Application(Label = "@string/app_name", Icon = "@drawable/ic_launcher", AllowBackup = false)]
    public class UILApplication : Application
    {
        public UILApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        [TargetApi(Value = (int)BuildVersionCodes.Gingerbread)]
	    public override void OnCreate()
        {
            if (Constants.Config.DEVELOPER_MODE && Build.VERSION.SdkInt >= BuildVersionCodes.Gingerbread)
            {
                StrictMode.SetThreadPolicy(new StrictMode.ThreadPolicy.Builder().DetectAll().PenaltyDialog().Build());
                StrictMode.SetVmPolicy(new StrictMode.VmPolicy.Builder().DetectAll().PenaltyDeath().Build());
            }

            base.OnCreate();

            InitImageLoader(ApplicationContext);
        }

        public static void InitImageLoader(Context context)
        {
            // This configuration tuning is custom. You can tune every option, you may tune some of them,
            // or you can create default configuration by
            //  ImageLoaderConfiguration.createDefault(this);
            // method.
            ImageLoaderConfiguration.Builder config = new ImageLoaderConfiguration.Builder(context);
            config.ThreadPriority(Java.Lang.Thread.NormPriority - 2);
            config.DenyCacheImageMultipleSizesInMemory();
            config.DiskCacheFileNameGenerator(new Md5FileNameGenerator());
            config.DiskCacheSize(50 * 1024 * 1024); // 50 MiB
            config.TasksProcessingOrder(QueueProcessingType.Lifo);
            config.WriteDebugLogs(); // Remove for release app

            // Initialize ImageLoader with configuration.
            ImageLoader.Instance.Init(config.Build());
        }
    }
}