namespace GraphML.Utils
{
  /// <summary>
  /// User roles within GraphML
  /// </summary>
  public static class Roles
  {
    /// <summary>
    /// An entity using GraphML
    /// </summary>
    public const string User = "User";

    /// <summary>
    /// An entity managing a subset of data within GraphML,
    /// typically data belonging to a single organisation
    /// </summary>
    public const string UserAdmin = "UserAdmin";

    /// <summary>
    /// An entity managing all data within GraphML
    /// </summary>
    public const string Admin = "Admin";
  }
}