namespace CameraScannerIOS.ViewControllers;

public class MainPageViewController: UIViewController, IUITableViewDataSource, IUITableViewDelegate
{
    #region Fields

    private readonly UITableView _tableView = new UITableView();
    private readonly string[] _items = ["Autofill Demo", "Item 2", "Item 3", "Item 4", "Item 5"];

    #endregion

    #region Methods

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        
        // Set up the table view
        _tableView.Frame = View.Bounds;
        _tableView.DataSource = this;
        _tableView.Delegate = this;
        View.AddSubview(_tableView);
    }

    #region UITableViewDataSource Methods
    
    public IntPtr RowsInSection(UITableView tableView, IntPtr section)
    {
        return _items.Length;
    }

    public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
    {
        var cell = tableView.DequeueReusableCell("cell") ?? new UITableViewCell(UITableViewCellStyle.Default, "cell");
        
        // Configure the cell using UIListContentConfiguration
        var contentConfig = UIListContentConfiguration.CellConfiguration;
        contentConfig.Text = _items[indexPath.Row];
        cell.ContentConfiguration = contentConfig;
        
        return cell;
    }
    
    #endregion
    
    #region UITableViewDelegate Method
    
    [Export("tableView:didSelectRowAtIndexPath:")]
    public void RowSelected(UITableView tableView, NSIndexPath indexPath)
    {
        tableView.DeselectRow(indexPath, true);
        var selectedItem = _items[indexPath.Row];
        Console.WriteLine($"Selected item: {selectedItem}");
        // Perform action on item click
        
        if (indexPath.Row == 0)
        {
            navigateToAutofillViewController();
        }
    }
    
    #endregion
    
    private void navigateToAutofillViewController()
    {
        var autofillViewController = new AutofillViewController();
        NavigationController.PushViewController(autofillViewController, true);
    }

    #endregion
}