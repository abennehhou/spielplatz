namespace Playground.Dto
{
    /// <summary>
    /// Description of an api version.
    /// </summary>
    public class VersionDescriptionDto
    {
        /// <summary>
        /// Version number.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Determines whether the api version is deprecated.
        /// </summary>
        public bool IsDeprecated { get; set; }
    }
}
