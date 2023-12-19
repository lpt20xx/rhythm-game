using System;

public static class TimeUtils
{
    public static string ToString(double num)
    {
        string result = string.Empty;

        TimeSpan timeSpan = TimeSpan.FromSeconds(num);
        if (timeSpan.Days > 0)
        {
            result = string.Format("{0:00}d:{1:00}h:{2:00}m:{3:00}s", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
        else if(timeSpan.Hours > 0)
        {
            result = string.Format("{0:00}h:{1:00}m:{2:00}s", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
        else if (timeSpan.Minutes > 0)
        {
            result = string.Format("{0:00}m:{1:00}s", timeSpan.Minutes, timeSpan.Seconds);
        }
        else if (timeSpan.Seconds > 0)
        {
            result = string.Format("{0:00}s", timeSpan.Seconds);
        }

        return result;
    }

    public static string ToSimpleString(double num)
    {
        string result = string.Empty;

        TimeSpan timeSpan = TimeSpan.FromSeconds(num);
        if (timeSpan.Days > 0)
        {
            result = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
        else if (timeSpan.Hours > 0)
        {
            result = string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
        else if (timeSpan.Minutes > 0)
        {
            result = string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
        }
        else if (timeSpan.Seconds > 0)
        {
            result = string.Format("{0:00}", timeSpan.Seconds);
        }

        return result;
    }
}
