using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using dotnet_compat_marshal_bug.Libraries.Symlink;


namespace Spectero.daemon.Libraries.Symlink
{
    /// <summary>
    /// Environment class for Windows.
    /// Specifically here we can use kernel calls to manage the filesystem and it's symbolic links.
    /// We do so here by carefully determining what the target path is, and having conditional
    /// statements to make sure we safely handle what we want.
    /// </summary>
    public class Windows : ISymlinkEnvironment
    {
        /// <summary>
        /// The parent class is the class that will initialize this environment.
        /// </summary>
        private dotnet_compat_marshal_bug.Libraries.Symlink.Symlink _parent;

        /// <summary>
        /// Constructor
        /// (Inherits the parent class)
        /// </summary>
        /// <param name="parent"></param>
        public Windows(dotnet_compat_marshal_bug.Libraries.Symlink.Symlink parent)
        {
            _parent = parent;
        }

        [DllImport("kernel32.dll")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.I1)]
        static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, dotnet_compat_marshal_bug.Libraries.Symlink.Symlink.SymbolicLink dwFlags);

        /// <summary>
        /// Wrapper function to create symlink and determine the type from the absolute path.
        /// 0 = file
        /// 1 = directory
        /// </summary>
        /// <param name="symlink"></param>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        public bool Create(string symlink, string absolutePath)
        {
            return CreateSymbolicLink(symlink, absolutePath, _parent.GetAbsolutePathType(absolutePath));
        }

        /// <summary>
        /// Delete the symlink safely.
        /// </summary>
        /// <param name="linkPath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(string linkPath)
        {
            if (_parent.IsSymlink(linkPath))
            {
                switch (_parent.GetAbsolutePathType(linkPath))
                {
                    case dotnet_compat_marshal_bug.Libraries.Symlink.Symlink.SymbolicLink.Directory:
                        Directory.Delete(linkPath);
                        return true;

                    case dotnet_compat_marshal_bug.Libraries.Symlink.Symlink.SymbolicLink.File:
                        File.Delete(linkPath);
                        return true;

                    default:
                        return false;
                }
            }
            else
            {
                throw new Exception("Specified deletion path is not a symbolic link.");
            }
        }
    }
}