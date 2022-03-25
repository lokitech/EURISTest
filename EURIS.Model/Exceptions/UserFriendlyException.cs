using System;

namespace EURIS.Model.Exceptions
{
    public sealed class UserFriendlyException : Exception
    {
        public UserFriendlyException(string message)
            : base(message)
        {

        }
    }
}
