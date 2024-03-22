namespace NetX.AppCore.Models
{
    public class AppAddoneConfig
    {
        public string[] AddoneAssembly { get; set; }
        public StartupConfig[] StartupConfig { get; set; }
        public NavigationMenuConfig[] NavigationMenuConfig { get; set; }
    }

    public class StartupConfig
    {
        public string Id { get; set; }
        public int Order { get; set; }
        public string Des { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class NavigationMenuConfig
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public string Tooltip { get; set; }
        public string Icon { get; set; }
        public string ViewModel { get; set; }
        public bool TriggerInvoked { get; set; }
        public NavigationMenuConfig[] Childrens { get; set; }
    }
}
