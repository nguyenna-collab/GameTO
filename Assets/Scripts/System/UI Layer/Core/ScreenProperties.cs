using System;
using UnityEngine;

/// <summary>
/// Base class for all screen properties
/// Provides common functionality for data passing to UI screens
/// </summary>
[Serializable]
public abstract class ScreenProperties
{
    [Header("Screen Behavior")]
    public bool blockInput = false;
    public bool showLoadingOverlay = false;
    
    [Header("Data")]
    public string sourceScreenId = "";
    public DateTime timestamp = DateTime.Now;
    
    public ScreenProperties()
    {
        timestamp = DateTime.Now;
    }
    
    /// <summary>
    /// Validate the properties before showing the screen
    /// </summary>
    public virtual bool Validate()
    {
        return true;
    }
    
    /// <summary>
    /// Clone the properties for reuse
    /// </summary>
    public virtual ScreenProperties Clone()
    {
        return (ScreenProperties)MemberwiseClone();
    }
    
    /// <summary>
    /// Get a summary of the properties for debugging
    /// </summary>
    public virtual string GetSummary()
    {
        return $"ScreenProperties: {GetType().Name} | BlockInput: {blockInput}";
    }
}

/// <summary>
/// Empty properties for screens that don't need data
/// </summary>
[Serializable]
public class EmptyScreenProperties : ScreenProperties
{
    public EmptyScreenProperties() : base() { }
}