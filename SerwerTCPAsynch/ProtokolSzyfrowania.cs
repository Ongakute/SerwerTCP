using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerwerTCPAsynch
{
    public class ProtokolSzyfrowania : ProtokolKomunikacyjny
    {
        private int trybPracy; //0 - brak, 1 - szyfrowanie, 2 - deszyfrowanie
        private bool status; //nazwa chwilowa
        Szyfrowanie szyfrowanie = new Szyfrowanie();
        private int statusPracy; // 0 - prosba o podanie tekstu, 1 - odbior tekstu, 2 - prosba 0 podanie klucza, 3 - odbior klucza, 4 - praca
        private String tekst, klucz;
        private bool witaj = false;

        public ProtokolSzyfrowania() : base() {
            status = false;
            trybPracy = 0;
        }

        public String inicjalizujPrace()
        {
            String odpowiedz;
            odpowiedz = "Wybierz opcje:  a) Zaszyfruj wiadomosc  b) Odszyfruj Wiadomosc \r\n";

            return odpowiedz;
        }
        public override string utworzOdpowiedz(String wiadomosc)
        {
           String odpowiedz = null;
            if (status == false && trybPracy == 0)
            {

                if (wiadomosc == "a") trybPracy = 1;
                else if (wiadomosc == "b") trybPracy = 2;
                else
                {
                    odpowiedz = "Wybierz opcje:  a) Zaszyfruj wiadomosc  b) Odszyfruj Wiadomosc \r\n";
                    return odpowiedz;
                }
                status = true;
                statusPracy = 0;
                odpowiedz = praca(null);
                
            }
            else
            {
                odpowiedz = praca(wiadomosc);
            }
                
            return odpowiedz;
        }

        private String praca(String wiadomosc)
        {
            String odpowiedz = null;
            switch (statusPracy)
            {
                case 0:
                    {
                        odpowiedz = "Podaj tekst: ";
                        statusPracy++;
                        break;
                    }
                case 1:
                    {
                        tekst = wiadomosc;
                        odpowiedz = "Podaj klucz do szyfru.";
                        statusPracy++;
                        break;
                    }
                case 2:
                    {
                        klucz = wiadomosc;
                        if (trybPracy == 1)
                            odpowiedz = "Zaszyfrowany tekst " + szyfrowanie.tworzenieSzyfru(tekst, klucz) + " :/ \r\n";

                        else
                            odpowiedz = szyfrowanie.deszyfracja(tekst, klucz) + " :/ \r\n";
                        statusPracy = 0;
                        trybPracy = 0;
                        status = false;
                        break;

                    }
            }

            return odpowiedz;
        }

    }
}
