using BookCollector.Data;
using BookCollector.Resources.Localization;

namespace BookCollector.Views.Support;

public partial class PermissionsView : ContentPage
{
    public List<PermissionsModel> Permissions { get; set; }

    public PermissionsView()
    {
        this.Permissions = new List<PermissionsModel>();
        this.CreatePermissions();
        this.InitializeComponent();
        this.BindingContext = this;
    }

    private void CreatePermissions()
    {
        this.Permissions.Add(
        new PermissionsModel()
        {
            Permission = AppStringResources.Camera,
            Description = AppStringResources.Camera_Description,
        });

        this.Permissions.Add(
        new PermissionsModel()
        {
            Permission = AppStringResources.Internet,
            Description = AppStringResources.Internet_Description,
        });

        this.Permissions.Add(
        new PermissionsModel()
        {
            Permission = AppStringResources.Files_Photos,
            Description = AppStringResources.Files_Photos_Description,
        });
    }
}