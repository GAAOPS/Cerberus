namespace Cerberus.Core.Configuration
{
    using System.Collections.Generic;

    public class HelixLayerEqualityComparer : IEqualityComparer<IHelixLayer>
    {
        public bool Equals(IHelixLayer x, IHelixLayer y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return string.Equals(x.Name, y.Name);
        }

        public int GetHashCode(IHelixLayer obj)
        {
            unchecked
            {
                var hashCode = obj.Name != null ? obj.Name.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (obj.DependsOn != null ? obj.DependsOn.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.DependentLayers != null ? obj.DependentLayers.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}