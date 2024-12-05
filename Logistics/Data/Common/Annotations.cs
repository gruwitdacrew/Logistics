using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Org.BouncyCastle.Bcpg.Sig;
using System.ComponentModel.DataAnnotations;

namespace Logistics.Data.Common
{
    public static class Annotations
    {
        internal class PasswordAttribute : RegularExpressionAttribute
        {
            public PasswordAttribute() : base("^.{8,}$")
            {
                ErrorMessage = "Пароль должен состоять из минимум 8 символов";
            }
        }

        internal class INNAttribute : RegularExpressionAttribute
        {
            public INNAttribute() : base("^\\d{12}|\\d{10}$")
            {
                ErrorMessage = "ИНН состоит из 10 или 12 цифр";
            }
        }

        internal class FullNameAttribute : RegularExpressionAttribute
        {
            public FullNameAttribute() : base("^([A-Za-zА-Яа-я]+\\s){1,2}[A-Za-zА-Яа-я]+$")
            {
                ErrorMessage = "Полное имя должно состоять как минимум из фамилии и имени";
            }
        }

        internal class PhoneAttribute : RegularExpressionAttribute
        {
            public PhoneAttribute() : base("^\\+\\d{11}$")
            {
                ErrorMessage = "Телефон должен быть по образцу: +X XXX XXX XX XX";
            }
        }

        internal class EmailAttribute : RegularExpressionAttribute
        {
            public EmailAttribute() : base("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.(?:[a-zA-Z]{2,})(?:\\.[a-zA-Z]{2,})*$")
            {
                ErrorMessage = "Неверная электронная почта";
            }
        }

        internal class SeriesAttribute : RegularExpressionAttribute
        {
            public SeriesAttribute() : base("^\\d{4}$")
            {
                ErrorMessage = "Серия должна состоять из 4 цифр";
            }
        }

        internal class PassportNumberAttribute : RegularExpressionAttribute
        {
            public PassportNumberAttribute() : base("^\\d{6}$")
            {
                ErrorMessage = "Номер должен состоять из 6 цифр";
            }
        }

        internal class CodeAttribute : RegularExpressionAttribute
        {
            public CodeAttribute() : base("^\\d{3}\\-\\d{3}$")
            {
                ErrorMessage = "Код имеет маску XXX-XXX";
            }
        }

        internal class WeightInTonsAttribute : RangeAttribute
        {
            public WeightInTonsAttribute() : base(0, 20)
            {
                ErrorMessage = "Вес должен быть до 20 тонн";
            }
        }

        internal class VolumeInCubicMetersAttribute : RangeAttribute
        {
            public VolumeInCubicMetersAttribute() : base(0, 96)
            {
                ErrorMessage = "Объем должен быть до 96 кубометров";
            }
        }

        internal class CarNumberAttribute : RegularExpressionAttribute
        {
            public CarNumberAttribute() : base("^[А-Я]{2}\\d{3}[А-Я]$")
            {
                ErrorMessage = "Автомобильный номер имеет маску АА000А";
            }
        }

        internal class DateAttribute : RegularExpressionAttribute
        {
            public DateAttribute() : base("^(0[1-9]|[12][0-9]|3[01])\\.(0[1-9]|1[0-2])\\.(19|20)\\d{2}$")
            {
                ErrorMessage = "Дата должна быть в формате дд.мм.гггг";
            }
        }
    }
}
