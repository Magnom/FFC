using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FFC.Bl
{
    public class Genre
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }

        public static Genre GetById(string id) {
            return new Genre() { Id = 0, Description =id};
        }
    }
}
