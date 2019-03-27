namespace Cerberus.Core.Data
{
    using System;

    public abstract class BaseItem : IBaseItem
    {
        public string Path { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Path: {Path}, Id: {Id}";
        }
    }
}