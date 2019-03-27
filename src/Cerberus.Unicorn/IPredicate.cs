namespace Sitecore.Helix.Validator.Unicorn
{
    using Common;
    using Rainbow;
    using Rainbow.Model;

    public interface IPredicate : IDocumentable
    {
        PredicateResult Includes(IItemData itemData);

        TreeRoot[] GetRootPaths();
    }
}