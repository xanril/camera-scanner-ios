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

        // Create and configure the text field
        var nameTextField = new UITextField
        {
            BorderStyle = UITextBorderStyle.RoundedRect,
            Placeholder = "Name:",
            TranslatesAutoresizingMaskIntoConstraints = false
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
        View.AddSubview(nameTextField);
        View.AddSubview(phoneNumberTextField);

        // Set up constraints
        NSLayoutConstraint.ActivateConstraints(new[]
        {
            label.TopAnchor.ConstraintEqualTo(View.SafeAreaLayoutGuide.TopAnchor, 20),
            label.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 20),
            label.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, -20),

            nameTextField.TopAnchor.ConstraintEqualTo(label.BottomAnchor, 20),
            nameTextField.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 20),
            nameTextField.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, -20),
            
            phoneNumberTextField.TopAnchor.ConstraintEqualTo(nameTextField.BottomAnchor, 20),
            phoneNumberTextField.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 20),
            phoneNumberTextField.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, -20)
        });
    }
}