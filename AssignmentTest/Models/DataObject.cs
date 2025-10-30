namespace AssignmentTest.Models
{
    /// <summary>
    /// Represents input data for processing operations
    /// </summary>
    public class DataObject
    {
        /// <summary>
        /// Unique identifier for the data object
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Input string to be processed
        /// </summary>
        public string Input { get; set; } = string.Empty;
    }
}
