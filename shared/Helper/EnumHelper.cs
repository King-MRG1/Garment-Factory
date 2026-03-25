using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Helper
{
    public static class EnumHelper
    {
        public static List<object> GetEnumList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(e => new
                {
                    Id = (int)(object)e,
                    Name = e.ToString()
                })
                .Cast<object>()
                .ToList();
        }
    }
}
