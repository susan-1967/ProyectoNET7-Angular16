﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageStore.Dto.Response
{
    public class BaseResponseGeneric<T> : BaseResponse
    {
        public T Data { get; set; } = default!;
    }
}
