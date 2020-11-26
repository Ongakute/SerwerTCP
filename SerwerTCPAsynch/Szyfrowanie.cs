using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerwerTCPAsynch
{
    class Szyfrowanie
    {
    
        /// <summary>
        /// Funkcja zamienia małe litery na duże przez zamianę kodów ASCII
        /// </summary>
        private void duzeLitery(string tekst, string klucz)
        {
            tekst.ToUpper();
            klucz.ToUpper();

        }

        /// <summary>
        /// Funkcja oblicza kolejność kolumn do powstania szyfru na podstawie podanego klucza
        /// </summary>
        private List<int> numerKolumn(string klucz)
        {
            //przypisanie numerów do kolejnych liter klucza - A najblizej, Z najdalej
            List<int> kolejnoscKolumn = new List<int>();
            for (int i = 0; i < klucz.Length; i++)
            {

                int najmniejszy = klucz[i];
                int kolejnosc = 1;
                for (int j = 0; j < klucz.Length; j++)
                {
                    if (najmniejszy > klucz[j] && i != j) kolejnosc++;
                }
                kolejnoscKolumn.Add(kolejnosc);

            }


            //przypadek gdy w kluczu wystepuja takie same litery 
            for (int i = 0; i < kolejnoscKolumn.Count; i++)
            {
                int temp = kolejnoscKolumn[i];
                for (int j = 0; j < kolejnoscKolumn.Count; j++)
                {
                    if (temp == kolejnoscKolumn[j] && i != j) kolejnoscKolumn[j]++;
                }
            }

            //ustalenie kolejnych kolumn do utworzenia szyfru 
            //po odnalezieniu najmniejszego elementu jego wartość zamieniana jest na 1000
            List<int> kolumny = new List<int>();
            for (int i = 0; i < kolejnoscKolumn.Count; i++)
            {
                int numerKolumny = kolejnoscKolumn.IndexOf(kolejnoscKolumn.Min());
                kolumny.Add(numerKolumny);
                kolejnoscKolumn[numerKolumny] = 1000;
            }

            return kolumny;
        }

        /// <summary>
        /// Funkcja tworząca szyfr na podstawie klucza.
        /// </summary>
        public string tworzenieSzyfru(string tekst, string klucz)
        {
            List<int> kolumny = numerKolumn(klucz);
            duzeLitery(tekst, klucz);

            //tworzenie macierzy z tekstem do zaszyfrowania ("-" uzupełnia braki)
            List<List<char>> tablicaSzyfru = new List<List<char>>();
            double wys, szer;
            szer = klucz.Length; //szerokość macierzy szyfru
            wys = Math.Ceiling(tekst.Length / szer); //wysokość macierzy szyfru
            string wynik = null;

            int dlugoscTekstu = tekst.Length;
            int aktualnyZnak = 0;
            int dlugoscSzyfru = (int)szer * (int)wys;

            for (int i = 0; i < wys; i++)
            {
                List<char> row = new List<char>();
                for (int j = 0; j < szer; j++)
                {
                    if (aktualnyZnak < dlugoscTekstu)
                    {
                        row.Add(tekst[aktualnyZnak]);
                        aktualnyZnak++;
                    }
                    else
                        row.Add('-');

                }
                tablicaSzyfru.Add(row.ToList());
                row.Clear();
            }

            for (int i = 0; i < wys; i++)
            {
                for (int j = 0; j < szer; j++)
                {
                    Console.Write(tablicaSzyfru[i][j]);
                }
                Console.WriteLine();
            }

            //czytanie po przekątnej w celu utworzenia szyfru
            char[] szyfr = new char[dlugoscSzyfru];
            int znak = 0;
            for (int i = 0; i < szer; i++)
            {
                int wiersz = 0;
                int kolumna = kolumny[i];

                for (int j = 0; j < wys; j++)
                {
                    if (kolumna >= 0 && wiersz < wys)
                    {
                        szyfr[znak] = (tablicaSzyfru[wiersz][kolumna]);
                        znak++;
                        kolumna--;
                        wiersz++;
                    }
                    else
                    {
                        kolumna = (int)szer - 1;
                        szyfr[znak] = (tablicaSzyfru[wiersz][kolumna]);
                        znak++;
                        kolumna--;
                        wiersz++;
                    }

                }
            }

            wynik = new String(szyfr);

            return wynik;
        }

        /// <summary>
        /// Funkcja deszydrująca wiadomość za pomocą klucza;
        /// </summary>
        public string deszyfracja(string szyfr, string klucz)
        {
            List<int> kolumnyDeszyfracja = numerKolumn(klucz); //wektor kolejności kolumn do deszyfracji wiadomości

            List<List<char>> odczyt = new List<List<char>>(); //tymczasowy wektor do przechowania odszyfrowanego kodu

            double n = kolumnyDeszyfracja.Count;
            double m = szyfr.Length;
            int iloscZnakow = (int)m * (int)n;
            double wiersze = Math.Ceiling(m / n); //liczba wierszy potrzebna do przechowania szyfru w macierzy
            int wiersz = 0;  //aktualny wiersz odszyfrowywanej wiadomości
            int kolumnyy = kolumnyDeszyfracja.Count; //liczba kolumn potrzebna do przechowania szyfru w macierzy
            int akt = 0;  //aktualny indeks wektora kolumn
            int kolumna = kolumnyDeszyfracja[akt]; //aktualna kolumna deszyfrowywanej wiadomości
            int znak = 0;

            char[] wiadomosc = new char[iloscZnakow];


            //uzupełnianie macierzy 0
            for (int i = 0; i < wiersze; i++)
            {
                List<char> row = new List<char>();
                for (int j = 0; j < kolumnyy; j++)
                {
                    row.Add('0');
                }
                odczyt.Add(row);
            }

            //dwa przypadki - więcej kolumn lub wierszy
            //czytanie od prawej do lewej z góry w dół

            if (wiersze <= kolumnyy)
            {
                for (int i = 0; i < szyfr.Length; i++)
                {
                    if (kolumna >= 0 && wiersz < wiersze)
                    {
                        odczyt[wiersz][kolumna] = szyfr[i];
                        kolumna--;
                        wiersz++;
                    }
                    else if (kolumna < 0 && wiersz < wiersze)
                    {

                        kolumna = kolumnyDeszyfracja.Count - 1;
                        odczyt[wiersz][kolumna] = szyfr[i];
                        wiersz++;
                        kolumna--;
                    }
                    else
                    {
                        akt++;
                        kolumna = kolumnyDeszyfracja[akt];
                        i--;
                        wiersz = 0;

                    }

                }

            }
            else
            {
                for (int i = 0; i < szyfr.Length; i++)
                {
                    if (kolumna >= 0 && wiersz < wiersze)
                    {
                        odczyt[wiersz][kolumna] = szyfr[i];
                        kolumna--;
                        wiersz++;
                    }
                    else if (kolumna < 0 && wiersz < wiersze)
                    {
                        kolumna = kolumnyDeszyfracja.Count - 1;
                        odczyt[wiersz][kolumna] = szyfr[i];
                        kolumna--;
                        wiersz++;

                    }
                    else
                    {
                        wiersz = 0;
                        akt++;
                        kolumna = kolumnyDeszyfracja[akt];
                        i--;

                    }

                }
            }

            for (int i = 0; i < wiersze; i++)
            {
                for (int k = 0; k < kolumnyy; k++)
                {
                    wiadomosc[znak] = (odczyt[i][k]);
                    znak++;
                }
            }

            return new String(wiadomosc);
        }
    }

}
