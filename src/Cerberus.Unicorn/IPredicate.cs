namespace Cerberus.Unicorn
{
    using Core;
    using Rainbow;
    using Rainbow.Model;

    public interface IPredicate : IDocumentable
    {
        PredicateResult Includes(IItemData itemData);

        TreeRoot[] GetRootPaths();
    }
}