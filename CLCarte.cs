using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace carteautresor
{
    internal class CLCarte
    {

        public int largeur { get; set; }
        public int longueur { get; set; }
        public List<int[]> montagnes { get; set; }
        public List<int[]> tresors { get; set; }
        public List<char[]> carte { get; set; }
        public List<string> carteVisuel { get; set; }

        public void setCarte()
        {
            List<char[]> newCarte = new List<char[]>();
            carteVisuel = new List<string>();
            for (int i = 0; i < longueur; i++)
            {
                string ligne = "";
                char[] chars = new char[largeur];
                for(int j = 0; j < largeur; j++)
                {
                    int[] pos = {  i+ 1 , j + 1 };
                    
                    if (getMontagnes(i + 1, j + 1))
                        chars[j] = 'M';
                    else if (getTresors(i+1,j+1))
                        chars[j] = 'T';
                    else
                        chars[j] = '-';
                    ligne += chars[j] + "\t";

                }
                carteVisuel.Add("\t" +ligne);
                newCarte.Add(chars);
            }
            carte = newCarte;
        }

        public void UpdateCarte( CLjoueur joueur)
        {
            int[] position = joueur.start;
            string orientation = joueur.orientation;
            foreach (var item in joueur.mouvement)
            {
                if (action(item, orientation, position, out string newOrientation, out int[] newPosition)) 
                    joueur.resultat++;
                position = newPosition;
                orientation = newOrientation;
            }
            joueur.start = position;
            joueur.orientation = orientation;

        }
        private bool action(char mouvement,string orientation, int[] position, out string newOrientation,out int[] newPosition)
        {
            newOrientation = orientation;
            newPosition = position;
            switch (mouvement)
            {
                case 'A':
                    if (orientation == "S" && position[0]< longueur)
                    {
                        if (getMontagnes(newPosition[0] + 1, newPosition[1]))
                            return false;
                            newPosition[0]++;
                    }
                    if (orientation == "N" && position[0] > 1)
                    {
                        if (getMontagnes(newPosition[0] - 1, newPosition[1]))
                            return false;
                        newPosition[0]--;
                    }
                    if (orientation == "E" && position[1] < largeur)
                    {
                        if (getMontagnes(newPosition[0], newPosition[1] + 1))
                            return false;
                        newPosition[1]++;
                    }
                    if (orientation == "O" && position[1] > 1)
                    {
                        if (getMontagnes(newPosition[0], newPosition[1] - 1))
                            return false;
                        newPosition[1]--;
                    }
                    if (getTresors(newPosition[0], newPosition[1]))
                    {
                        return true;
                    }
                    break;
                case 'D':
                    if (orientation == "S" )
                    {
                        newOrientation = "O";
                    }
                    if (orientation == "N")
                    {
                        newOrientation = "E";
                    }
                    if (orientation == "E")
                    {
                        newOrientation = "S";
                    }
                    if (orientation == "O")
                    {
                        newOrientation = "N";
                    }

                    break;
                case 'G':
                    if (orientation == "S")
                    {
                        newOrientation = "E";
                    }
                    if (orientation == "N")
                    {
                        newOrientation = "O";
                    }
                    if (orientation == "E")
                    {
                        newOrientation = "N";
                    }
                    if (orientation == "O")
                    {
                        newOrientation = "S";
                    }

                    break;

            }

            return false;
        }
        public bool getTresors(int i, int j)
        {
            if (tresors.Any(x => x[0] == i  && x[1] == j ))
                return true;
            return false;
        }
        public bool getMontagnes(int i,int j)
        {
            if (montagnes.Any(x => x[0] == i  && x[1] == j ))
                return true; 
            return false;
        }


    }
}
