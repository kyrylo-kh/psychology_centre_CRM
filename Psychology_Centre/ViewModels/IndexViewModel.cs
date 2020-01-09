﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Psychology_Centre.Models;

namespace Psychology_Centre.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Client> Clients { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
    }
}