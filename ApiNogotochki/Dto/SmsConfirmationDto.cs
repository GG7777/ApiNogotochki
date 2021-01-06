namespace ApiNogotochki.Dto
{
	public class SmsConfirmationDto
	{
		public string PhoneNumber { get; set; }
		public string ConfirmationType { get; set; }
		public string ConfirmationCode { get; set; }
	}
}