namespace CameraScannerIOS.ViewControllers;

public class AutofillViewController: UIViewController
{
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        
        var label = new UILabel
        {
            Text = "Information",
            TextAlignment = UITextAlignment.Left,
            TranslatesAutoresizingMaskIntoConstraints = false
        };
        
        var addressTextField = new UITextField
        {
            BorderStyle = UITextBorderStyle.RoundedRect,
            Placeholder = "Address:",
            TranslatesAutoresizingMaskIntoConstraints = false,
            AutocorrectionType = UITextAutocorrectionType.No
        };
        
        var urlTextField = new UITextField
        {
            BorderStyle = UITextBorderStyle.RoundedRect,
            Placeholder = "URL:",
            TextContentType = UITextContentType.Url,
            TranslatesAutoresizingMaskIntoConstraints = false,
            AutocorrectionType = UITextAutocorrectionType.No
        };
        
        var phoneNumberTextField = new UITextField
        {
            BorderStyle = UITextBorderStyle.RoundedRect,
            Placeholder = "Phone:",
            TranslatesAutoresizingMaskIntoConstraints = false,
            TextContentType = UITextContentType.TelephoneNumber,
            KeyboardType = UIKeyboardType.PhonePad,
            AutocorrectionType = UITextAutocorrectionType.No
        };

        // Add the label and text field to the view
        View.AddSubview(label);
        View.AddSubview(addressTextField);
        View.AddSubview(urlTextField);
        View.AddSubview(phoneNumberTextField);

        // Set up constraints
        NSLayoutConstraint.ActivateConstraints(new[]
        {
            label.TopAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TopAnchor, 20),
            label.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 20),
            label.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, -20),
            
            addressTextField.TopAnchor.ConstraintEqualTo(label.BottomAnchor, 20),
            addressTextField.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 20),
            addressTextField.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, -20),

            urlTextField.TopAnchor.ConstraintEqualTo(addressTextField.BottomAnchor, 20),
            urlTextField.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 20),
            urlTextField.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, -20),
            
            phoneNumberTextField.TopAnchor.ConstraintEqualTo(urlTextField.BottomAnchor, 20),
            phoneNumberTextField.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 20),
            phoneNumberTextField.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, -20)
        });
    }
}