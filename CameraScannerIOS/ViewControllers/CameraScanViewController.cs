using VisionKit;

namespace CameraScannerIOS.ViewControllers;

public class CameraScanViewController : UIViewController, IVNDocumentCameraViewControllerDelegate
{
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        // Create and configure the button
        var button = new UIButton(UIButtonType.System)
        {
            TranslatesAutoresizingMaskIntoConstraints = false
        };
        button.SetTitle("Click Me", UIControlState.Normal);

        // Add the button to the view
        View.AddSubview(button);

        // Set up constraints
        NSLayoutConstraint.ActivateConstraints(new[]
        {
            button.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor),
            button.CenterYAnchor.ConstraintEqualTo(View.CenterYAnchor)
        });

        // Connect the button's click event to the handler method
        button.TouchUpInside += Button_Clicked;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        // navigate to DataScanner
        if (VNDocumentCameraViewController.Supported && UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
        {
            var documentCameraViewController = new VNDocumentCameraViewController();
            documentCameraViewController.Delegate = this;
            PresentViewController(documentCameraViewController, true, null);
        }
        else
        {
            // show an alert to notify the user that document scanning is not supported
            var alert = UIAlertController.Create("Error", "Document scanning is not supported on this device.", UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            PresentViewController(alert, true, null);
        }
    }
    
    // Method to handle document scanning completion
    [Export("documentCameraViewController:didFinishWithScan:")]
    public void DidFinish(VNDocumentCameraViewController controller, VNDocumentCameraScan scan)
    {
        Console.WriteLine("Document scanning completed successfully.");
        // Process the scanned document
        // Extract images and text from the scan
    }
}