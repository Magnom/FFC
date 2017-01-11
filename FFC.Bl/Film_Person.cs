using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FFC.Bl
{
    public class Film_Person
    {
        [DataMember]
        public int IdFilm { get; set; }
        [DataMember]
        public int IdPerson { get; set; }
        [DataMember]
        public int IdRole { get; set; }
        [DataMember]
        public string Personaje { get; set; }

        [DataMember]
        public virtual Person FilmPerson { get; set; }
        [DataMember]
        public virtual Rol FilmRol { get; set; }
    }
}
