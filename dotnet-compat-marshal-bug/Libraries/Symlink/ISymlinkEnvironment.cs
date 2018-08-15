namespace dotnet_compat_marshal_bug.Libraries.Symlink
{
    
    /// <summary>
    /// This interface file defines a basic layout that each environment should be implemented to.
    /// </summary>
    public interface ISymlinkEnvironment
    {
        bool Create(string symlink, string absolutePath);
        bool Delete(string symlink);
    }
}