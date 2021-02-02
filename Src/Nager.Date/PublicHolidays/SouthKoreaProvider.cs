﻿using Nager.Date.Contract;
using Nager.Date.Model;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System;

namespace Nager.Date.PublicHolidays
{
    /// <summary>
    /// Korea
    /// </summary>
    public class SouthKoreaProvider : IPublicHolidayProvider
    {
        /// <summary>
        /// SouthKoreaProvider
        /// </summary>
        public SouthKoreaProvider()
        {
        }

        ///<inheritdoc/>
        public IEnumerable<PublicHoliday> Get(int year)
        {
            var countryCode = CountryCode.KR;
            var items = new List<PublicHoliday>();

            if (year > 918 && year < 2050)
            {
                //LunisolarCalendar .net implementation only valid are between 918 and 2050, inclusive.
                //https://docs.microsoft.com/en-us/dotnet/api/system.globalization.koreanlunisolarcalendar?view=net-5.0
                var koreanCalendar = new KoreanLunisolarCalendar();
                var leapMonth = koreanCalendar.GetLeapMonth(year);

                var lunarNewYear1 = koreanCalendar.ToDateTime(year, this.MoveMonth(1, leapMonth), 1, 0, 0, 0, 0);   //Has substitute holiday
                while (lunarNewYear1.DayOfWeek == DayOfWeek.Sunday)
                    lunarNewYear1.AddDays(+1);
                var lunarNewYear2 = lunarNewYear1.AddDays(+1);
                var lunarNewYear3 = lunarNewYear1.AddDays(-1);

                var buddhaBday = koreanCalendar.ToDateTime(year, this.MoveMonth(4, leapMonth), 8, 0, 0, 0, 0);

                var Chuseok1 = koreanCalendar.ToDateTime(year, this.MoveMonth(8, leapMonth), 14, 0, 0, 0, 0);   //Has substitute holiday
                while (Chuseok1.DayOfWeek == DayOfWeek.Sunday)
                    Chuseok1.AddDays(+1);
                var Chuseok2 = Chuseok1.AddDays(+1);
                var Chuseok3 = Chuseok2.AddDays(+1);

                items.Add(new PublicHoliday(lunarNewYear1, "설날", "Lunar New Year", countryCode));
                items.Add(new PublicHoliday(lunarNewYear2, "설날", "Lunar New Year", countryCode));
                items.Add(new PublicHoliday(lunarNewYear3, "설날", "Lunar New Year", countryCode));
                items.Add(new PublicHoliday(buddhaBday, "부처님 오신 날", "Buddha's Birthday", countryCode));
                items.Add(new PublicHoliday(Chuseok1, "추석", "Chuseok", countryCode));
                items.Add(new PublicHoliday(Chuseok2, "추석", "Chuseok", countryCode));
                items.Add(new PublicHoliday(Chuseok3, "추석", "Chuseok", countryCode));
            }
            items.Add(new PublicHoliday(year, 1, 1, "새해", "New Year's Day", countryCode));
            items.Add(new PublicHoliday(year, 3, 1, "3·1절", "Independence Movement Day", countryCode));

            var childrenDay = new DateTime(year, 5, 5);
            while (childrenDay.DayOfWeek == DayOfWeek.Saturday || childrenDay.DayOfWeek == DayOfWeek.Sunday)
                childrenDay.AddDays(+1);
            items.Add(new PublicHoliday(childrenDay, "어린이날", "Children's Day", countryCode));    //Has substitute holiday

            items.Add(new PublicHoliday(year, 6, 6, "현충일", "Memorial Day", countryCode));
            items.Add(new PublicHoliday(year, 8, 15, "광복절", "Liberation Day", countryCode));
            items.Add(new PublicHoliday(year, 10, 3, "개천절", "National Foundation Day", countryCode));
            items.Add(new PublicHoliday(year, 10, 9, "한글날", "Hangul Day", countryCode));
            items.Add(new PublicHoliday(year, 12, 25, "크리스마스", "Christmas", countryCode));

            return items.OrderBy(o => o.Date);
        }

        private int MoveMonth(int month, int leapMonth)
        {
            if (leapMonth == 0)
            {
                return month;
            }

            if (leapMonth < month)
            {
                return ++month;
            }

            return month;
        }

        ///<inheritdoc/>
        public IEnumerable<string> GetSources()
        {
            return new string[]
            {
                "https://en.wikipedia.org/wiki/Public_holidays_in_South_Korea",
            };
        }
    }
}