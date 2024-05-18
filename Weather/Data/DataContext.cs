using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Data
{
    public static class DataContext
    {
        public static List<DateTemperature> DateTemperarures = new List<DateTemperature>
        {
            new DateTemperature("20.03.2024", "Какой хороший день, чтобы подарить цветов", +10),
            new DateTemperature("21.03.2024", "Скоро сессия, не забудь подгтовиться", -10),
            new DateTemperature("22.03.2024", "Сегодня тот самый день, чтобы написать докладную на Талгата", +15),
            new DateTemperature("23.03.2024", "Отличный день!", +7),
            new DateTemperature("24.03.2024", "Красивая дата! Загадай желание", -11),
            new DateTemperature("25.03.2024", " I'm a рокстар попстар", - 1),
            new DateTemperature("26.03.2024", "Сегодня можно покушать мороженое", + 11),
            new DateTemperature("27.03.2024", "Одевайтесь теплее)", -1)
        };
    }
}
