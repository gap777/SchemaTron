
namespace SchemaTron
{
    /// <summary>
    /// Validator settings which affect its behavior.
    /// </summary>
    /// <remarks>
    /// The settings include validation phase and enabling/disabling
    /// preprocessing.
    /// </remarks>
    public sealed class ValidatorSettings
    {
        /// <summary>
        /// Creates a new instance of validator settings with default values.
        /// </summary>
        /// <remarks>
        /// Default values: all phases are enabled, preprocessing is enabled.
        /// </remarks>
        public ValidatorSettings()
        {
            this.Phase = "#ALL";
            this.Preprocessing = true;
        }

        /// <summary>
        /// Implementation of resolving included Schematron elements.
        /// In case it is set to null a default implementation is used.
        /// </summary>
        public IInclusionResolver InclusionsResolver { get; set; }

        /// <summary>
        /// Name of the validation phase to be performed.
        /// </summary>
        /// <remarks>
        /// <para>
        /// It is useful for supporting progressive validation.
        /// </para>
        /// <para>
        /// The phase has to be specified in the Schematron schema.
        /// </para>
        /// <para>
        /// Phase name can be either concrete, or symbolic. Supported symbolic
        /// identifiers are <c>#ALL</c> and <c>#DEFAULT</c>. <c>#ALL</c> means
        /// that all phases in the schema should be performed. <c>#DEFAULT</c> 
        /// means that only the phase set as default in the schema should be
        /// performed.
        /// </para>
        /// </remarks>
        public string Phase { get; set; }

        /// <summary>
        /// Indicates whether validator should perform preprocessing
        /// during its initialization.
        /// </summary>
        internal bool Preprocessing { get; set; }
    }
}
