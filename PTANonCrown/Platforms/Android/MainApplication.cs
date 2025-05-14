using Android.App;
using Android.Runtime;
using PTANonCrown.Services;
namespace PTANonCrown
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp()
        {

            try
            {
                return MauiProgram.CreateMauiApp();
            } catch (Exception e) {
                AppLogger.Log($"{e}", "MauiApplication");
                throw;
            }
        } 

    }
}