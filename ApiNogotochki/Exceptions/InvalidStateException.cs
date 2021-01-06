using System;

#nullable enable

namespace ApiNogotochki.Exceptions
{
	public class InvalidStateException : Exception
	{
		public InvalidStateException(string? error = null) : base(error)
		{
		}
	}
}