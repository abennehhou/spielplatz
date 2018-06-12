using System.Collections.Generic;

namespace Playground.Dto
{
    /// <summary>
    /// Information about the version of the api.
    /// </summary>
    public class VersionDto
    {
        /// <summary>
        /// Assembly version.
        /// </summary>
        public string AssemblyVersion { get; set; }

        /// <summary>
        /// Collection of supported api versions.
        /// </summary>
        public List<VersionDescriptionDto> SupportedApiVersions { get; set; }
    }
}
