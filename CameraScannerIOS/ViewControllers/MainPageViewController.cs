namespace CameraScannerIOS.ViewControllers;

public class MainPageViewController: UIViewController, IUITableViewDataSource, IUITableViewDelegate
{
    #region Fields

    private readonly UITableView _tableView = new UITableView();
    private readonly string[] _items = ["Demo 1", "Demo 2 (not working)", "Demo 3"];

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
            navigateToDemo1();
        }
        else if (indexPath.Row == 2)
        {
            navigateToDemo3();
        }
    }
    
    #endregion
    
    private void navigateToDemo1()
    {
        var autofillViewController = new AutofillViewController();
        NavigationController.PushViewController(autofillViewController, true);
    }
    
    private void navigateToDemo3()
    {
        var autofillViewController = new CameraScanViewController();
        NavigationController.PushViewController(autofillViewController, true);
    }

    #endregion
}