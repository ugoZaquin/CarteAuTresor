using carteautresor.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace carteautresor
{
    internal class ContextMainMenu : ObservableObject
    {
        #region public
        public CLCarte carte
        {
            get
            {
                return _carte;
            }
            set
            {
                _carte = value;
                NotifyPropertyChanged();
            }
        }
        public List<CLjoueur> ljoueurs
        {
            get
            {
                return _ljoueurs;
            }
            set
            {
                _ljoueurs = value;
                NotifyPropertyChanged();
            }
        }
        public string definition
        {
            get
            {
                return _definition;
            }
            set
            {
                _definition = value;
                NotifyPropertyChanged();
            }
        }
        public string commentaire
        {
            get
            {
                return _commentaire;
            }
            set
            {
                _commentaire = value;
                NotifyPropertyChanged();
            }
        }
        public string button
        {
            get
            {
                return _button;
            }
            set
            {
                _button = value;
                NotifyPropertyChanged();
            }
        }
        public string entre
        {
            get
            {
                return _entre;
            }
            set
            {
                _entre = value;
                NotifyPropertyChanged();
            }
        }
        public ICommand suivant
        {
            get
            {
                return _suivant;
            }
            set
            {
                _suivant = value;
                NotifyPropertyChanged();
            }
        }

        #endregion
        private int etape;

        private void ActionSuivant(object obj)
        {
            bool Ok = false;
            switch (etape)
            {
                case 0:
                    premiereEtape();
                    break;
                case 1:
                    secondeEtape();
                    break;
                case 2:
                    troisiemeEtape();
                    break;
                case 3:
                    quatriemeEtape();
                    break;
                case 4:
                    cinquiemeEtape();
                    break;
            }
        }

        private bool Verification(string[] entreSplit, string lettre,string modelCommentaire, out int largeurGet,out int longueurGet, int lenght = 3,bool isAventurier = false)
        {
             largeurGet = -1;
            longueurGet = -1;
            int startLenght = isAventurier ? 1 : 0;
            if (entreSplit.Length != lenght)
            {
                commentaire = "# la serie rentrée est incorect, \n " + modelCommentaire;
                return false;
            }
            if (!entreSplit[0].Trim().StartsWith(lettre))
            {
                commentaire = "# la serie rentrée ne commence pas par \""+ lettre +"\", \n" + modelCommentaire;
                return false;
            }
            if (!Int32.TryParse(entreSplit[startLenght +1].Trim(), out int largeurEntre))
            {
                commentaire = "# la largeur rentrée est incorrect, \n" + modelCommentaire;
                return false;
            }
            if (!Int32.TryParse(entreSplit[startLenght+2].Trim(), out int longueurEntre))
            {
                commentaire = "# la longueur rentrée est incorrect, \n" + modelCommentaire;
                return false;
            }
            largeurGet = largeurEntre;
            longueurGet = longueurEntre;
            return true;
        }
        private void cinquiemeEtape()
        {
            commentaire = _modelCommentaireEtape5;
            if (!entre.Contains("-"))
            {
                commentaire = "# la serie rentrée est incorect, \n " + _modelCommentaireEtape4;
                return;
            }
            string[] listEntreSplit = entre.Split("\n");
            foreach (var item in listEntreSplit)
            {
                int[] Tresors = new int[2];
                string[] entreSplit = item.Split('-');
                if (!Verification(entreSplit, "A", _modelCommentaireEtape4, out int largeurEntre, out int longueurEntre , 6,true))
                    return;
                if (largeurEntre > carte.largeur)
                {
                    commentaire = "# une largeur " + largeurEntre + " est superieur a la largeur de la carte (" + carte.largeur + "), \n " + _modelCommentaireEtape3;
                    return;
                }
                if (longueurEntre > carte.longueur)
                {
                    commentaire = "# une largeur " + longueurEntre + " est superieur a la longueur de la carte (" + carte.longueur + "), \n " + _modelCommentaireEtape3;
                    return;
                }
                if (carte.getMontagnes(largeurEntre, longueurEntre))
                {
                    commentaire = "# la postion [ " + largeurEntre + " , " + longueurEntre + "] est deja occupée par une montagne , \n " + _modelCommentaireEtape3;
                    return;

                }
                string orientation = entreSplit[4].Trim();
                int[] start = { largeurEntre, longueurEntre };
                ljoueurs.Add(new CLjoueur(entreSplit[1].Trim(), entreSplit[5].Trim(), start, orientation));
                  
            }

            string resultat = "# {A comme Aventurier} - {Nom de l’aventurier} - {Axe horizontal} - {Axe\r\nvertical} - {Orientation} - {Nb. trésors ramassés}";

            foreach (var joueur in ljoueurs)
            {
                carte.UpdateCarte(joueur);
                resultat += "\nA - " + joueur.nom + " - " + joueur.start[0] + " - " + joueur.start[1] + " - " + joueur.orientation + " - " + joueur.resultat;
            }
            entre = _script + "\n" + resultat;
            commentaire += "";
            definition = "fini";
        }
        private void quatriemeEtape()
        {
            entre = "";
            definition = "a vous de joué";
            commentaire = "";
            etape = 4;
        }
        private void secondeEtape()
        {
            if (!entre.Contains("-"))
            {
                commentaire = "# la serie rentrée est incorect, \n " + _modelCommentaireEtape2;
                return;
            }
            string[] listEntreSplit = entre.Split("\n");
            List<int[]> ListMontagnes = new List<int[]>();
            foreach (var item in listEntreSplit)
            {
                int[] Montagne = new int[2];
                string[] entreSplit = item.Split('-');
                if (!Verification(entreSplit, "M", _modelCommentaireEtape2,out int largeurEntre,out int longueurEntre))
                    return;
                if (largeurEntre > carte.largeur)
                {
                    commentaire = "# une largeur " + largeurEntre +" est superieur a la largeur de la carte ("+ carte.largeur+"), \n " + _modelCommentaireEtape2;
                    return;
                }
                if (longueurEntre > carte.longueur)
                {

                    commentaire = "# une largeur " + longueurEntre + " est superieur a la longueur de la carte (" + carte.longueur + "), \n " + _modelCommentaireEtape2;
                    return;
                }
                Montagne[0] = largeurEntre; 
                Montagne[1] = longueurEntre;
                ListMontagnes.Add( Montagne );
            }
            _carte.montagnes = ListMontagnes;
            etape = 2;
            commentaire = _modelCommentaireEtape3;
            definition = "definition de la position des tresors";
            _script += entre;
            entre = "";
        }
        private void troisiemeEtape()
        {
            if (!entre.Contains("-"))
            {
                commentaire = "# la serie rentrée est incorect, \n " + _modelCommentaireEtape2;
                return;
            }
            string[] listEntreSplit = entre.Split("\n");
            List<int[]> ListTresors = new List<int[]>();
            foreach (var item in listEntreSplit)
            {
                int[] Tresors = new int[2];
                string[] entreSplit = item.Split('-');
                if (!Verification(entreSplit, "T", _modelCommentaireEtape3, out int largeurEntre, out int longueurEntre))
                    return;
                if (largeurEntre > carte.largeur)
                {
                    commentaire = "# une largeur " + largeurEntre + " est superieur a la largeur de la carte (" + carte.largeur + "), \n " + _modelCommentaireEtape3;
                    return;
                }
                if (longueurEntre > carte.longueur)
                {
                    commentaire = "# une largeur " + longueurEntre + " est superieur a la longueur de la carte (" + carte.longueur + "), \n " + _modelCommentaireEtape3;
                    return;
                }
                if(carte.getMontagnes(largeurEntre, longueurEntre))
                {
                    commentaire = "# la postion [ " + largeurEntre + " , " + longueurEntre + "] est deja occupée par une montagne , \n " + _modelCommentaireEtape3;
                    return;

                }
                Tresors[0] = largeurEntre;
                Tresors[1] = longueurEntre;
                ListTresors.Add(Tresors);
            }
            _carte.tresors = ListTresors;
            etape = 3;
            commentaire = _modelCommentaireEtape4;
            definition = "validation";
            button = "validé";

            carte.setCarte();
            _script += entre;
            entre = "\n" +string.Join("\n", carte.carteVisuel);
        }
        private void premiereEtape()
        {

            if (!entre.Contains("-"))
            {
                commentaire = "# la serie rentrée est incorect, \n " + _modelCommentaireEtape1;
                return ;
            }
            string[] entreSplit = entre.Split('-');
            if (!Verification(entreSplit, "C", _modelCommentaireEtape1, out int largeurEntre, out int longueurEntre))
                return;

            _carte.largeur = largeurEntre;
            _carte.longueur = longueurEntre;
            etape = 1;
            commentaire = _modelCommentaireEtape2;
            definition = "definition de la position des montagne";
            _script += entre;
            entre = "";

        }
        public ContextMainMenu()
        {
            etape = 0;
            entre = "";
            _definition = " definition de la largeur et la longueur de la carte";
            commentaire = _modelCommentaireEtape1;
            _button = "suivant";
            _carte = new CLCarte();
            _ljoueurs = new List<CLjoueur>();
            _suivant = new MainCommand(ActionSuivant, param => true);
        }

        private CLCarte _carte;
        private List<CLjoueur> _ljoueurs;
        private string _definition;
        private string _commentaire;
        private string _button;
        private string _entre;
        private string _script;
        private ICommand _suivant;
        private string _modelCommentaireEtape1 = "Le Modele est le suivant (C commme Carte) - (Nb.de case en largeur) _ (Nb. de case en hauteur) \n exemple : C - 3 - 4";
        private string _modelCommentaireEtape2 = "# {M comme Montagne} - {Axe horizontal} - {Axe vertical}\n{M comme Montagne} - {Axe horizontal} - {Axe vertical}"+
                "exemple : \n M - 4 - 4\nM - 3 - 4";
        private string _modelCommentaireEtape3 = "# {T comme Trésor} - {Axe horizontal} - {Axe vertical} - {Nb. de trésors}" +
                "exemple : \n T - 2 - 4\nT - 2 - 4";
        private string _modelCommentaireEtape4 = "# Voici ce que donne votre carte";
        private string _modelCommentaireEtape5 = "# {A comme Aventurier} - {Nom de l’aventurier} - {Axe horizontal} - {Axe\\r\\nvertical} - {Orientation} - {Séquence de mouvement}";
    }


}
