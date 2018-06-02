namespace Playground.Dto
{
    /// <summary>
    /// Contains diff for a property between two objects.
    /// </summary>
    public class DifferenceDto
    {
        /// <summary>
        /// Property name.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Value in the first object.
        /// </summary>
        public string Value1 { get; set; }

        /// <summary>
        /// Value in the second object.
        /// </summary>
        public string Value2 { get; set; }
    }
}
