using System.Collections;

namespace ObjectsComparer
{
    /// <summary>
    /// Specifies types of the differences between object members.
    /// </summary>
    public enum DifferenceTypes
    {
        /// <summary>
        /// Value of the member in first and second objects are not equal.
        /// </summary>
        ValueMismatch,

        /// <summary>
        /// Type of the member in first and second objects  are not equal.
        /// </summary>
        TypeMismatch,

        /// <summary>
        /// Member does not exists in the first object.
        /// </summary>
        MissedMemberInFirstObject,

        /// <summary>
        /// Member does not exists in the second object.
        /// </summary>
        MissedMemberInSecondObject,

        /// <summary>
        /// <see cref="IEnumerable"/>s have different number of elements.
        /// </summary>
        NumberOfElementsMismatch,

        /// <summary>
        /// <see cref="IEnumerable"/>s have been removed from the list
        /// </summary>
        ItemRemovedFromList,

        /// <summary>
        /// <see cref="IEnumerable"/>s have been added to the list
        /// </summary>
        ItemAddedToList,

        /// <summary>
        /// <see cref="IEnumerable"/>s have been moved in the list
        /// </summary>
        ItemMovedInList
    }

    /// <summary>
    /// Specifies types of the differences between object members.
    /// </summary>
    public enum DifferenceSeverity
    {
        /// <summary>
        /// Difference is negligible 
        /// </summary>
        Informational,

        /// <summary>
        /// Difference is potentially hazardous
        /// </summary>
        Warning,

        /// <summary>
        /// Difference is important, will influence some workflows
        /// </summary>
        Important,

        /// <summary>
        /// Difference is in error, not allowed, hazardous
        /// </summary>
        Error
    }
}
