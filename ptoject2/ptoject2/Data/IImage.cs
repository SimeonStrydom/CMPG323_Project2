﻿using ptoject2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptoject2.Data
{
    public interface IImage
    {
        IEnumerable<Photo> GetAll();
        Photo GetById();
        IEnumerable<Photo> GetWithTag(string tag);
    }
}
