using System;
using System.Runtime.InteropServices;
using dotnet_compat_marshal_bug.Libraries.Symlink;

namespace dotnet_compat_marshal_bug
{
    class Program
    {
        static void Main(string[] args)
        {
            var symLinkLib = new Symlink();

            if (symLinkLib.Environment.Create("./symlink_example", "C:/Users/"))
            {
                Console.WriteLine("The symlink was successful");
                Environment.Exit(0);
            }
            else
            {
                var errorVal = Marshal.GetLastWin32Error();

                Console.WriteLine("The symlink has failed.");
                Console.WriteLine("Marshal Error: {0} ({1})", errorVal, MarshalDecoder(errorVal));
                Environment.Exit(1);
            }
        }

        static string MarshalDecoder(int marshal)
        {
            switch (marshal)
            {
                case 0:
                    return "ERROR_SUCCESS";
                case 1:
                    return "ERROR_INVALID_FUNCTION";
                case 2:
                    return "ERROR_FILE_NOT_FOUND";
                case 3:
                    return "ERROR_PATH_NOT_FOUND";
                default:
                    return marshal.ToString();
            }
        }
    }
}