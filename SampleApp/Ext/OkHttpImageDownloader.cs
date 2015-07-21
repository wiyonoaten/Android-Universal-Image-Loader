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
using Android.Content;
using Nostra13UniversalImageLoader.Core.Download;
using Square.OkHttp;
using System.IO;

namespace Nostra13UniversalImageLoader.SampleApp.Ext
{
    /**
     * Implementation of ImageDownloader which uses {@link com.squareup.okhttp.OkHttpClient} for image stream retrieving.
     *
     * @author Sergey Tarasevich (nostra13[at]gmail[dot]com)
     */
    public class OkHttpImageDownloader : BaseImageDownloader
    {
        private OkHttpClient client;

        public OkHttpImageDownloader(Context context, OkHttpClient client)
            : base(context)
        {
            this.client = client;
        }

        /// <exception cref="IOException">This method might throw this exception.</exception>
	    protected override Stream GetStreamFromNetwork(string imageUri, Java.Lang.Object extra)
        {
            Request request = new Request.Builder().Url(imageUri).Build();
            return client.NewCall(request).Execute().Body().ByteStream();
        }
    }
}