using Android.Widget;

namespace Nostra13UniversalImageLoader.Core.Imageaware
{
    public partial class ImageViewAware
    {
        public virtual ImageView WrappedView
        {
            get { return base.WrappedView as ImageView; }
        }
    }
}
