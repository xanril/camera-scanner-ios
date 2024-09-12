using System.Net.Mime;
using System.Text;
using CameraScannerIOS.Helpers;
using DataScannerViewControlleriOS;
using Vision;
using VisionKit;

namespace CameraScannerIOS.ViewControllers;

public class CameraScanViewController : UIViewController, IVNDocumentCameraViewControllerDelegate
{
    private UITextField _nameTextField;
    private UITextField _creditCardNumberTextField;
    
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        var label = new UILabel
        {
            Text = "Information",
            TextAlignment = UITextAlignment.Left,
            TranslatesAutoresizingMaskIntoConstraints = false,
            Font = UIFont.BoldSystemFontOfSize(20)
        };
        
        // Create and configure the text field
        _nameTextField = new UITextField
        {
            BorderStyle = UITextBorderStyle.RoundedRect,
            Placeholder = "Name:",
            TranslatesAutoresizingMaskIntoConstraints = false,
            TextContentType = UITextContentType.CreditCardName,
            AutocorrectionType = UITextAutocorrectionType.No
        };
        
        _creditCardNumberTextField = new UITextField
        {
            BorderStyle = UITextBorderStyle.RoundedRect,
            Placeholder = "Credit Card Number:",
            TranslatesAutoresizingMaskIntoConstraints = false,
            TextContentType = UITextContentType.CreditCardNumber,
            KeyboardType = UIKeyboardType.PhonePad,
            AutocorrectionType = UITextAutocorrectionType.No
        };
        
        // Create and configure the button
        var button = new UIButton(UIButtonType.System)
        {
            TranslatesAutoresizingMaskIntoConstraints = false
        };
        button.SetTitle("Scan Card", UIControlState.Normal);
        
        // Connect the button's click event to the handler method
        button.TouchUpInside += Button_Clicked;

        // Add the button to the view
        // Add the label and text field to the view
        View.AddSubview(label);
        View.AddSubview(_nameTextField);
        View.AddSubview(_creditCardNumberTextField);
        View.AddSubview(button);
        
        // Set up constraints
        NSLayoutConstraint.ActivateConstraints(new[]
        {
            label.TopAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TopAnchor, 20),
            label.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 20),
            label.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, -20),

            _nameTextField.TopAnchor.ConstraintEqualTo(label.BottomAnchor, 20),
            _nameTextField.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 20),
            _nameTextField.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, -20),
            
            _creditCardNumberTextField.TopAnchor.ConstraintEqualTo(_nameTextField.BottomAnchor, 20),
            _creditCardNumberTextField.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 20),
            _creditCardNumberTextField.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, -20),
            
            button.TopAnchor.ConstraintEqualTo(_creditCardNumberTextField.BottomAnchor, 20),
            button.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 20),
            button.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, -20)
        });
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        // Handle the button click event
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
        var scanImage = scan.GetImage(0);
        
        // Process the scanned document
        // Extract images and text from the scan
        var cardDetails = ImageRecognizerHelper.ValidateScanImage(scanImage);
        _nameTextField.Text = cardDetails.Name;
        _creditCardNumberTextField.Text = cardDetails.CreditCardNumber;
        
        DismissViewController(true, null);
    }
    
    // Method to handle document scanning cancellation
    [Export("documentCameraViewControllerDidCancel:")]
    public void DidCancel(VNDocumentCameraViewController controller)
    {
        Console.WriteLine("Document scanning was cancelled.");
        DismissViewController(true, null);
    }
    
    // Method to handle document scanning error
    [Export("documentCameraViewController:didFailWithError:")]
    public void DidFail(VNDocumentCameraViewController controller, NSError error)
    {
        Console.WriteLine("Document scanning failed with error: " + error.LocalizedDescription);
        DismissViewController(true, null);
    }
    

    // private void navigateToDataScannerViewController()
    // {
    //     var dataScannerViewController = new DataScannerViewController();
    //     dataScannerViewController.SetScanCallbackWithCallback(scanCallback);
    //     dataScannerViewController.SetScanUpdatedCallbackWithCallback(scanUpdatedCallback);
    //     
    //     PresentViewController(dataScannerViewController.ViewController, true, null);
    // }
    
    // #region DataScannerViewController Callbacks
    //
    // private void scanCallback(string[] obj)
    // {
    //     // Handle the scan callback
    //     Console.WriteLine(obj);
    // }
    //
    // private void scanUpdatedCallback(string[] obj)
    // {
    //     Console.WriteLine("Scan Updated!");
    //     Console.WriteLine(obj);
    // }
    //
    // #endregion
    
    
}