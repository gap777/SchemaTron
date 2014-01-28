using System;
using System.Collections.Generic;

namespace SchemaTron
{
    /// <summary>
    /// Represents one or more schema syntax errors and contains user
    /// information about them.
    /// </summary>
    [Serializable]
    public sealed class SyntaxException : Exception
    {
        /// <summary>
        /// Creates an instance of a SyntaxException containing one or more
        /// user messages about the syntax errors.
        /// </summary>
        /// <param name="messages">user messages on the syntax errors</param>
        internal SyntaxException(IEnumerable<string> messages)
        {
            this.UserMessages = messages;
        }

        /// <summary>
        /// Gets or sets user messages concerned with each of the syntax
        /// errors.
        /// </summary>
        public IEnumerable<string> UserMessages { get; private set; }
    }
}
