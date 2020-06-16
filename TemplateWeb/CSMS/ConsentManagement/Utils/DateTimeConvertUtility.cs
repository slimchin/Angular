using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
namespace ConsentManagement
{
    public class DateTimeConvertUtility
    {
        public static string CnvToYYYYMMDDEng(DateTime argDateTime)
        {
            string returnValue;
            returnValue = argDateTime.ToString("yyyyMMdd",new CultureInfo("en-US"));
            return returnValue;
        }
        public static string CnvDateShowToDb(string ShowDate)
        {
            string DateToDb, d, m, y;
            if (ShowDate == "")
            {
                if (DateTime.Now.Day < 10)
                {
                    d = '0' + DateTime.Now.Day.ToString();
                }
                else
                {
                    d = DateTime.Now.Day.ToString();
                }
                if (DateTime.Now.Month < 10)
                {
                    m = '0' + DateTime.Now.Month.ToString();
                }
                else
                {
                    m = DateTime.Now.Month.ToString();
                }
                y = DateTime.Now.Year.ToString();
                DateToDb = y + m + d;
            }
            else
            {
                DateToDb = ShowDate.Substring(6, 4) + ShowDate.Substring(3, 2) + ShowDate.Substring(0, 2);
            }
            return DateToDb;
        }
        public static string CnvToYYYYMMDDEng(string argDateTime)
        {
            string returnValue = "";
            int yearEng; int Months; int Days;
            if (String.IsNullOrEmpty(argDateTime))
            {
                returnValue = "";
            }
            if (argDateTime.Length == 8)
            {

                if (int.TryParse(argDateTime.Substring(0,4),out yearEng))
                {
                    if (yearEng > 2500)
                    {
                        yearEng = yearEng - 543;
                    }
                } else
                {
                    return "";
                }
                if (int.TryParse(argDateTime.Substring(4,2),out Months))
                {
                    if (Months > 12)
                    {
                        return "";
                    }
                } else
                {
                    return "";
                }
                if (int.TryParse(argDateTime.Substring(6,2),out Days))
                {
                    if (Days > 31)
                    {
                        return "";
                    }
                } else
                {
                    return "";
                }
                returnValue = yearEng.ToString("0000") + Months.ToString("0000") + Days.ToString("0000");
            }
            if (argDateTime.Length == 10)
            { 
                if (argDateTime.Contains("/"))
                { 
                    if(int.TryParse(argDateTime.Substring(0,2),out Days))
                    {
                        if (Days>31)
                        {
                            return "";
                        }
                    } else
                    {
                        return "";
                    }

                    if(int.TryParse(argDateTime.Substring(3,2),out Months))
                    {
                        if (Months>12)
                        {
                            return "";
                        }
                    }else
                    {
                        return "";
                    }

                    if (int.TryParse(argDateTime.Substring(6,4),out yearEng))
                    {
                        if (yearEng > 2500)
                        {
                            yearEng = yearEng + 543;
                        }
                    }else
                    {
                        return "";
                    }
                    returnValue = yearEng.ToString("0000") + Months.ToString("0000") + Days.ToString("0000");
                }
                if(argDateTime.Contains("-"))
                {
                    argDateTime = argDateTime.Replace("-", "");
                    if (argDateTime.Length == 8)
                    {
                        if (int.TryParse(argDateTime.Substring(0, 4), out yearEng))
                        {
                            if (yearEng > 2500)
                            {
                                yearEng = yearEng - 543;
                            }
                        }
                        else
                        {
                            return "";
                        }
                        if (int.TryParse(argDateTime.Substring(4, 2), out Months))
                        {
                            if (Months > 12)
                            {
                                return "";
                            }
                        }
                        else
                        {
                            return "";
                        }
                        if (int.TryParse(argDateTime.Substring(6, 2), out Days))
                        {
                            if (Days > 31)
                            {
                                return "";
                            }
                        }
                        else
                        {
                            return "";
                        }
                        returnValue = yearEng.ToString("0000") + Months.ToString("0000") + Days.ToString("0000");
                    }
                }
            }
            return returnValue;
        }

    }
}