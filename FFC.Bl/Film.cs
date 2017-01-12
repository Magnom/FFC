using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FFC.Bl
{
    public partial class Film
    {
        #region PublicProperties

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string UrlImagen { get; set; }

        [DataMember]
        public string UrlFilmaffinity { get; set; }

        [DataMember]
        public List<Person> Directors { get; set; }

        [DataMember]
        public List<Person> Actors { get; set; }

        [DataMember]
        public List<Person> Musicians { get; set; }

        [DataMember]
        public List<Person> Guionist { get; set; }

        [DataMember]
        public List<Person> Fotografy { get; set; }

        [DataMember]
        public string Productora { get; set; }

        [DataMember]
        public List<Genre> Genero { get; set; }

        [DataMember]
        public string Sinopsi { get; set; }

        [DataMember]
        public double UserRatting { get; set; }

        [DataMember]
        public double GlobalRatting { get; set; }

        [DataMember]
        public string Year { get; set; }

        [DataMember]
        public Nullable<int> Duration { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string OriginalTitle { get; set; }

        [DataMember]
        public string AwardsText { get; set; }

        [DataMember]
        public Nullable<System.DateTime> LastCrawl { get; set; }

        [DataMember]
        public Nullable<System.DateTime> LastRead { get; set; }

        [DataMember]
        public virtual ICollection<Film_Person> Persons { get; set; }

        [DataMember]
        public virtual ICollection<Country> Countries { get; set; }


        [DataMember]
        public virtual int? TorrentLinksCount { get; set; }


        [DataMember]
        public virtual int? ELinksCount { get; set; }


        [DataMember]
        public virtual List<Object> TorrentLinks { get; set; }

        [DataMember]
        public virtual List<Object> ELinks { get; set; }


        #endregion

        public string GetFilmUrl()
        {
            return "";
        }


        public enum Campos
        {
            musica,
            fotografia,
            actors,
            genero,
            productora,
            tituloOriginal,
            premios,
            puntuacion,
            director,
            guion,
            pais,
            duracion
        }

        public enum LinkTypes
        {
            torrent,
            mejorTorrent
        }


        public Film() {
            Genero = new List<Genre>();
            Fotografy = new List<Person>();
            Guionist = new List<Person>();
            Musicians = new List<Person>();
            Actors = new List<Person>();
            Directors = new List<Person>();
            TorrentLinks = new List<Object>();
            ELinks = new List<Object>();
        }
        
        public void AddCountry(string value, int IdRole)
        {
        }
        
        //El title conte l'any al final entre parentesi
        public void ParseTitle(string sTitle)        
        {
            var final =sTitle.LastIndexOf(')');
            var inici =sTitle.LastIndexOf('(');

            Title = sTitle.Substring(0, inici);
            Year = sTitle.Substring(inici + 1, final - inici -1);
        }

        public void SetFieldValue(Campos c, string value, string html)
        {
            switch (c)
            {
                case Campos.tituloOriginal: OriginalTitle = value; break;
                case Campos.director:
                    Directors.Add(new Person(value));
                    //AddPersonToRole(value, 1);
                    break;
                case Campos.guion:
                    Guionist.Add(new Person(value));
                    break;
                case Campos.fotografia:
                    Fotografy.Add(new Person(value));
                    break;
                case Campos.genero: Genero.Add(Genre.GetById(value)); break;
                //case Campos.pais: = value; break;
                case Campos.duracion: Duration = Int32.Parse(value); break;
                case Campos.premios: AwardsText = value.Length > 1000 ? value.Substring(0, 1000) : value; break;
                case Campos.productora: Productora = value.Length > 255 ? value.Substring(0, 255) : value; break;
                case Campos.musica: Musicians.Add(new Person(value)); break;
            }
        }

        public string GetDirectorName()
        {
            if (Directors.Count > 0)
            {
                return Directors.First<Person>().Name;
            }
            else
                return "";
        }

        public static Film GetById(List<Film> lst, int itemId)
        {
            return lst.FirstOrDefault(p => p.Id == itemId);
        }
    }
}
