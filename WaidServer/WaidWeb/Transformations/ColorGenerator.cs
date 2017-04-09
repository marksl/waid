using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Caching;
using WaidWeb.Models;

namespace WaidWeb.Transformations
{
    public class ColorGenerator
    {
        private const int RandomSeed = 2;

        private static readonly Random _random = new Random(RandomSeed);

        private static string GetRandomColor()
        {
            // to create lighter colours:
            // take a random integer between 0 & 128 (rather than between 0 and 255)
            // and then add 127 to make the colour lighter
            var colorBytes = new byte[3];
            colorBytes[0] = (byte)(_random.Next(128) + 127);
            colorBytes[1] = (byte)(_random.Next(128) + 127);
            colorBytes[2] = (byte)(_random.Next(128) + 127);

            var color = Color.FromArgb(255, colorBytes[0], colorBytes[1], colorBytes[2]);

            return ColorTranslator.ToHtml(color);

            //string color = String.Format("#{0:X6}", _random.Next(0x1000000)); // = "#A197B9"
            //return color;
        }

        public static string GetColor(uint hash)
        {
            // 1. Check if it's in cache
            object cachedValue = HttpContext.Current.Cache[hash.ToString(CultureInfo.InvariantCulture)];
            if (cachedValue != null)
            {
                return (string) cachedValue;
            }

            // 2. Check if it's in DB.
            string s;
            if (GetColorFromDBAndCache(hash, out s)) return s;

            // 3. Insert into DB.
            try
            {
                using (var db = new UsersContext())
                {
                    db.Database.ExecuteSqlCommand(
                        string.Format("INSERT INTO AppColors(AppId, Color) VALUES({0},'{1}')", hash, GetRandomColor()));
                }
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch {}
            // ReSharper restore EmptyGeneralCatchClause

            // 4. Requery from DB and insert into Cache.
            if (GetColorFromDBAndCache(hash, out s)) return s;

            return GetRandomColor();
        }

        private static bool GetColorFromDBAndCache(uint hash, out string s)
        {
            List< AppColor> colors;
            using (var db = new UsersContext())
            {
                colors = db.AppColors.ToList();
            }

            // Cache all colors
            foreach (AppColor color in colors)
            {
                HttpContext.Current.Cache.Add(color.AppId.ToString(CultureInfo.InvariantCulture), color.Color,
                                              null,
                                              DateTime.Now.AddDays(7),
                                              Cache.NoSlidingExpiration,
                                              CacheItemPriority.High, null);
            }

            var user = colors.SingleOrDefault(u => u.AppId == hash);
            if (user != null)
            {
                s = user.Color;
                return true;
            }

            s = null;
            return false;
        }
    }
}