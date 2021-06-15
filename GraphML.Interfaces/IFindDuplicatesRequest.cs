namespace GraphML.Interfaces
{
  public interface IFindDuplicatesRequest : IGraphRequest
  {
    /// <summary>
    /// Minimum number of characters in double metaphone key
    /// for it to be considered a duplicate.
    /// <remarks>
    /// <para>
    /// A lower number (1-3) will give more matches
    /// ie looser matching; whereas a higher number (6-12) will
    /// give less matches ie tighter matching.
    /// </para>
    /// <para>
    /// If you know your data will have mostly unique names  ie targetted subset,
    /// then a higher number should be used.
    /// Conversely, if you know your data will have random names ie untargetted subset,
    /// then a lower number should be used.
    /// </para>
    /// </remarks>
    /// </summary>
    int MinMatchingKeyLength { get; set; }
  }
}
