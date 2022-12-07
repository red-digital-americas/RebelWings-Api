namespace api.rebel_wings.Extensions;

public static class DateTimeExtensions
{
    public static DateTime AbsoluteStart(this DateTime dateTime)
    {
        return dateTime.Date;
    }
    
    public static DateTime AbsoluteEnd(this DateTime dateTime)
    {
        return AbsoluteStart(dateTime).AddDays(1).AddTicks(-1);
    }
}