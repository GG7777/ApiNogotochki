namespace ApiNogotochki.Services.Items
{
	public class WorkingTimeItem
	{
		public string DayOfWeek { get; set; }

		public int StartHour { get; set; }
		public int StartMinute { get; set; }
		
		public int EndHour { get; set; }
		public int EndMinute { get; set; }
	}
}