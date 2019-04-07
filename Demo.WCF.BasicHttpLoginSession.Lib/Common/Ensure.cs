using System;

namespace Demo.WCF.BasicHttpLoginSession.Lib.Common
{
    public static class Ensure
    {
        public static void ArgumentNotNull<T>(T argument, string argumentName) where T : class
        {
            if (argument == null)
                throw new ArgumentNullException(argumentName);
        }

        public static void ArgumentNotNull<T>(Nullable<T> argument, string argumentName) where T : struct
        {
            if (argument == null)
                throw new ArgumentNullException(argumentName);
        }

        public static void StringArgumentNotNullAndNotEmpty(string argument, string argumentName)
        {
            if (argument == null)
                throw new ArgumentNullException(argumentName);

            if (argument.Length == 0)
                throw new ArgumentException("argument cannot be empty", argumentName);
        }

        public static void ArrayArgumentNotNullAndNotEmpty<T>(T[] argument, string argumentName)
        {
            if (argument == null)
                throw new ArgumentNullException(argumentName);

            if (argument.Length == 0)
                throw new ArgumentException("argument cannot be empty", argumentName);
        }
    }
}
