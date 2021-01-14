using System.Linq;

#nullable enable

namespace ApiNogotochki.Validators
{
	public class PhoneNumberValidator
	{
		public string? Validate(string phoneNumber)
		{
			if (phoneNumber.Any(x => !char.IsDigit(x)))
				return "Phone number should contain only digits";

			if (phoneNumber.Length != 11)
				return "Phone number should contain only 11 digits";

			if (!phoneNumber.StartsWith("79"))
				return "Phone number should start with '79'";

			return null;
		}
	}
}