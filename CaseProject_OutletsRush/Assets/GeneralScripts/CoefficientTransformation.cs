using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoefficientTransformation
{
    public static string Converter(int number)
    {
        int integerPart;
        int decimalPart;
        string result = number.ToString();
        if (number >= 1000)
        {
            integerPart = number / 1000;
            decimalPart = number % 1000;
            if (decimalPart >= 100)
            {
                decimalPart /= 10;
                result = integerPart.ToString() + "." + decimalPart.ToString() + "K";
            }
            else
            {
                decimalPart /= 10;
                result = integerPart.ToString() + ".0" + decimalPart.ToString() + "K";
            }
        }
        return result;
    }

    public static float SecondFormat(int second)
    {
        int hour = second / 3600;
        int minute = (second % 3600) / 60;
        int remainingTime = second % 60;

        int time = hour * 10000 + minute * 100 + remainingTime;
        return time;
    }
}
