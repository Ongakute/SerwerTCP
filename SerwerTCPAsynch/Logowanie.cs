using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;


namespace SerwerTCPAsynch
{
    public class Logowanie
    {

        BazaUzytkownikow baza;
        int ziarno;
        public bool statusLogowania;
        bool czyLogin;
        bool czyHaslo;
        bool czyMenu;
        bool czyLogowanie;
        bool czyRejestracja;
        bool czyPonowneHaslo;
        String login = null;
        String haslo = null;
        String nowyLogin = null;
        String noweHaslo = null;


        /// <summary>
        /// Konstruktor klasy logowanie
        /// </summary>
        public Logowanie(BazaUzytkownikow baza, int ziarno)
        {
            this.baza = baza;
            this.ziarno = ziarno;
            statusLogowania = false;
            czyLogin = false;
            czyHaslo = false;
            czyMenu = false;
            czyLogowanie = true;
            czyPonowneHaslo = false;
            czyRejestracja = true;
        }

        public String utworzOdpowiedz(String wiadomosc)
        {
            String odpowiedz = null;
          
            if(!czyMenu)
            {
                if(wiadomosc == "1")
                {
                    odpowiedz = "Wybrano 1 - Logowanie:";
                    czyMenu = true;
                    czyLogowanie = false;
                }
                else if(wiadomosc == "2")
                {
                    odpowiedz = "Wybrano 2 - Rejestracja:";
                    czyMenu = true;
                    czyRejestracja = false;
                }
                else
                {
                    odpowiedz = "Dokonano blednego wyboru - sprobuj raz jeszcze \n s1. Zaloguj, 2. zarejestruj nowego uzytkownika\n";
                    czyMenu = false;
                }
            }
            else if(!czyLogowanie)
            {
                if (!czyLogin)
                {
                    odpowiedz = "Podaj login: ";
                    czyLogin = true;
                }
                else if (!czyHaslo)
                {
                    login = wiadomosc;
                    odpowiedz = "Podaj haslo: ";
                    czyHaslo = true;
                }
                else if (czyLogin && czyHaslo)
                {
                    haslo = wiadomosc;
                    statusLogowania = zaloguj(login, haslo);
                    if (zaloguj(login, haslo))
                    {
                        odpowiedz = "Poprawnie zalogowano. \r\n";
                    }
                    else
                    {
                        odpowiedz = "Niepoprawny login lub haslo, sprobuj ponownie.";
                        czyLogin = false;
                        czyHaslo = false;
                    }

                }
            }
            else if(!czyRejestracja)
            {
                if (!czyLogin)
                {
                    odpowiedz = "Podaj login: ";
                    czyLogin = true;
                }
                else if (!czyHaslo)
                {
                    nowyLogin = wiadomosc;
                    odpowiedz = "Podaj haslo: ";
                    czyHaslo = true;
                }
                else if (!czyPonowneHaslo)
                {
                    noweHaslo = wiadomosc;
                    odpowiedz = "Powtorz haslo: ";
                    czyPonowneHaslo = true;
                }
                else
                {
                    if (noweHaslo == wiadomosc)
                    {
                        if (zarejestruj(nowyLogin,noweHaslo))
                        {
                            odpowiedz = "Poprawnie zarejestrowano nowego uzytkownika (ENTER) \n";
                            statusLogowania = true;

                        }
                        else
                        {
                            odpowiedz = "Problem z dodaniem uzytkownika, sprobuj ponownie \n";
                            czyMenu = false;
                            czyLogin = false;
                            czyHaslo = false;
                            czyPonowneHaslo = false;
                            nowyLogin = null;
                            noweHaslo = null;

                        }
                        czyRejestracja = true;

                    }
                    else
                    {
                        odpowiedz = "Hasla nie sa tozsame - sprobuj ponownie. \n";
                        czyLogin = false;
                        czyHaslo = false;
                        czyPonowneHaslo = false;
                        noweHaslo = null;
                    }
                }
            }

            return odpowiedz;
        }

        /// <summary>
        /// Wywoływana przez rozpocznijTransmisję, po poprawnym zalogowaniu, powraca do funkcji wywołującej.
        /// </summary>
        /// <param name="stream"></param>
        protected bool zaloguj(string login, string haslo)
        {
            if (baza.czyIstnieje(login))
            {
                string hasloTajne = baza.pobieraczHasla(login);
                //char[] tymczasowy = hasloTajne.ToCharArray();
                List<int> haslo_zahaszowane = new List<int>();
                int wartosc_znaku;
                for (int i = 0; i < hasloTajne.Length; i++)
                {
                    wartosc_znaku = Char.ConvertToUtf32(hasloTajne, i) * ziarno;
                    haslo_zahaszowane.Add(wartosc_znaku);
                    //tymczasowy[i] = Convert.ToChar(Char.ConvertToUtf32(hasloTajne, i) * ziarno);
                }
                //hasloTajne = tymczasowy.ToString();
                hasloTajne = string.Join("|", haslo_zahaszowane);
                if (hasloTajne == haslo)
                {
                    statusLogowania = true;
                    return true;
                }
                else return false;
            }
            else return false;

        }

        public bool zarejestruj(String login, String haslo)
        {
            String[] subs = haslo.Split('|');
            String haslo_jawne = "";
            int a = 0;
            for(int i=0; i < subs.Length; i++)
            {
                //haslo_jawne += subs[i];
                a = Int32.Parse(subs[i]);
                a = a / ziarno;
                haslo_jawne += (char)a;
                //Console.WriteLine((char)a);
            }
            Console.WriteLine(haslo_jawne);

            if (baza.czyIstnieje(login))
            {
                Console.WriteLine("błąd - uzytkownik istnieje");
                return false;
            } else
            {
                Console.WriteLine("bangla");
                baza.dodajUzytkownika(login, haslo_jawne);
                return true;
            }
            

            
        }

    }
}
