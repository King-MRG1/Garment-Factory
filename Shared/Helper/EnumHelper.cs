using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Helper
{
    public static class EnumHelper
    {
        public static List<ViewEnumDto> GetEnumList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(e => new ViewEnumDto
                {
                    Id = (int)(object)e,
                    Name = e.ToString()
                })
                .ToList();
        }
    }
}
