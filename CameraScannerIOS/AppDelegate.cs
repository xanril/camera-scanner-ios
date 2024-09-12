using CameraScannerIOS.ViewControllers;

namespace CameraScannerIOS;

[Register("AppDelegate")]
public class AppDelegate : UIApplicationDelegate
{
    public override UIWindow? Window { get; set; }

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        // create a new window instance based on the screen size
        Window = new UIWindow(UIScreen.MainScreen.Bounds);
        
        // create a UINavigationController with MainPageViewController as the root
        var mainPageViewController = new MainPageViewController();
        var navigationController = new UINavigationController(mainPageViewController);
        Window.RootViewController = navigationController;

        // make the window visible
        Window.MakeKeyAndVisible();

        return true;
    }
}