using System.Collections.Generic;

namespace Playground.Dto
{
    /// <summary>
    /// Represents a resource of the api.
    /// A resource contains a list of hyperlinks.
    /// </summary>
    public abstract class ResourceBase
    {
        /// <summary>
        /// Link name to access to the resource itself.
        /// </summary>
        public const string RelationNameSelf = "self";

        /// <summary>
        /// Link name to access to the previous list of resources in a paged result.
        /// </summary>
        public const string RelationNamePrevious = "previous";

        /// <summary>
        /// Link name to access to the next list of resources in a paged result.
        /// </summary>
        public const string RelationNameNext = "next";

        /// <summary>
        /// Hyperlinks related to the resource.
        /// </summary>
        public Dictionary<string, string> Links { get; set; }

        /// <summary>
        /// Initializes a ResourceBase.
        /// </summary>
        protected ResourceBase()
        {
            Links = new Dictionary<string, string>();
        }
    }
}
