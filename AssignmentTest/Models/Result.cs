namespace AssignmentTest.Models
{
    /// <summary>
    /// Represents the result of a processing operation
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Identifier matching the input DataObject Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Processed output string
        /// </summary>
        public string Output { get; set; } = string.Empty;
    }
}
