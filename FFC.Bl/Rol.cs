﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FFC.Bl
{
    public class Rol
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
