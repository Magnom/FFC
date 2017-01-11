using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FFC.Bl
{
    public class Person
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public Nullable<int> IdMigrado { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DateBorn { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DateDead { get; set; }
        [DataMember]
        public string LinkBiography { get; set; }
        [DataMember]
        public string Name { get; set; }


        [DataMember]
        public virtual ICollection<Film_Person> FILMS_PERSONS { get; set; }


        public Person (string name)
        {
            Name = name;
        }
    }
}
