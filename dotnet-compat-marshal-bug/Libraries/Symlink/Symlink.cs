/*
    Spectero Daemon - Daemon Component to the Spectero Solution
    Copyright (C)  2017 Spectero, Inc.

    Spectero Daemon is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Spectero Daemon is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.
    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://github.com/ProjectSpectero/daemon/blob/master/LICENSE>.
*/

using System;
using System.IO;
using System.Runtime.InteropServices;
using Spectero.daemon.Libraries.Symlink;

namespace dotnet_compat_marshal_bug.Libraries.Symlink
{
    public class Symlink
    {
        private ISymlinkEnvironment _environment;

        public enum SymbolicLink
        {
            File = 0,
            Directory = 1,
            WithoutEvevation = 2
        }

        public Symlink()
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) _environment = new Windows(this);
            else throw new NotImplementedException();
        }

        public SymbolicLink GetAbsolutePathType(string absolutePath)
        {
            // Check if file.
            if (File.Exists(absolutePath))
                return SymbolicLink.File;

            // Check if directory
            if (Directory.Exists(absolutePath))
                return SymbolicLink.Directory;

            // Unsure, we should never reach this point but let's decide to use a file.
            return SymbolicLink.File;
        }

        public bool IsSymlink(string linkPath)
        {
            if (Directory.Exists(linkPath) || File.Exists(linkPath))
            {
                FileInfo pathInfo = new FileInfo(linkPath);
                return pathInfo.Attributes.HasFlag(FileAttributes.ReparsePoint);
            }
            else
            {
                return false;
            }
        }

        public ISymlinkEnvironment Environment => _environment;
    }
}