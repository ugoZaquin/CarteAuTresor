using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace carteautresor
{
    internal class CLjoueur
    {
        public string nom { get; set; }
        public string mouvement { get; set; }
        public int resultat { get;set; }
        public int[] start { get; set; }
        public string orientation { get; set; }

        public CLjoueur(string _nom,string _mouvement, int[] _start, string _orientation)
        {
            resultat = 0;
            nom = _nom;
            mouvement = _mouvement;
            start = _start;
            orientation = _orientation;
        }
    }
}
