﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageStore.Dto.Response
{
    public class BaseResponse
    {
        public bool Success { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
