namespace SubjectRegister.Common
{
    public static class TimeHandle
    {
        public static string TimeFormat(DateTime inputTime)
        {
            DateTime today = DateTime.Today;
            DateTime yesterday = today.AddDays(-1);

            if (inputTime.Date == today)
            {
                return "Hôm nay";
            }
            else if (inputTime.Date == yesterday)
            {
                return "Hôm qua";
            }
            else
            {
                return inputTime.ToString("dd/MM/yyyy");
            }
        }

    }
}
