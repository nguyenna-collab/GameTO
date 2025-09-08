namespace Save
{
    /// <summary>
    /// Contain the Accept method for Visitor Pattern
    /// You can expand this interface for more saveable classes
    /// </summary>
    public interface ISaveable
    {
        public void Accept(ISaveVisitor visitor);
        public void Accept(ILoadVisitor loadVisitor);
    }
}