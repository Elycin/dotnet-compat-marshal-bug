using System;
using System.Runtime.InteropServices;
using dotnet_compat_marshal_bug.Libraries.Symlink;

namespace dotnet_compat_marshal_bug
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize the wrapper library.
            var symLinkLib = new Symlink();

            // Here I'm using an example path.
            if (symLinkLib.Environment.Create("C:/ExampleSymlinkPath", "C:/Users/Public"))
            {
                // Success.
                Console.WriteLine("The symlink was successful");
                Environment.Exit(0);
            }
            else
            {
                // Get the error code
                var errorVal = Marshal.GetLastWin32Error();

                // Failure.
                Console.WriteLine("The symlink has failed.");
                Console.WriteLine("Marshal Error: {0} ({1})", errorVal, MarshalDecoder(errorVal));
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Example function to return string from the error code.
        /// </summary>
        /// <param name="marshal"></param>
        /// <returns></returns>
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