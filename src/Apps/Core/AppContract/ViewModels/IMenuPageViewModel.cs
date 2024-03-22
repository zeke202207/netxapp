using Material.Icons;

namespace NetX.AppCore.Contract
{
    public interface IMenuPageViewModel
    {
        public int Order { get; }

        public string DisplayName { get; }

        public MaterialIconKind Icon { get; }

        //暂时不支持子菜单
        //public IEnumerable<IMenuPageViewModel> Children { get; }
    }
}
