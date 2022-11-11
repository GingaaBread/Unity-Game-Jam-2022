using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Allows you to specify objects that you want asserted to not-null.
/// At Awake(), every reference will be asserted.
/// </summary>
/// <author>Gino</author>
public abstract class InspectorReferenceChecker : MonoBehaviour
{
    protected void Awake()
    {
        foreach (var reference in CheckForMissingReferences())
        {
            Assert.IsNotNull(reference);

            // Edge cases for missing inspector references
            if (reference.Equals(null))
            {
                throw new System.MissingFieldException($"Checked reference '{reference.GetType()}' is missing. " +
                    $"Make sure its's been set in the inspector or remove it from the checked references.");
            }
        }
    }

    /// <summary>
    /// Pass any number of objects that you want asserted to not-null
    /// </summary>
    /// <returns>An array of objects</returns>
    /// <example>
    /// protected override object[] CheckForMissingReferences() => new object[] 
    /// {
    ///     myAnimator, myText, ...
    /// };
    /// </example>
    protected abstract object[] CheckForMissingReferences();

}
