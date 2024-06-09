Dokumentacja projektu Coookie.NET (aplikacja do wyszukiwania przepisów) 

Lista funkcjonalności:
✅ znajdowanie przepisów na podstawie składników użytkownika
✅ Rejestracja i logowanie
✅ aktywacja konta za pomocą e-maila
✅ odzyskiwanie hasła za pomocą e-maila
✅ wyświetlanie danych zalogowanego użytkownika
✅ zmiana hasła e-maila i usuwanie konta użytkownika
✅ baza danych z przepisami i składnikami
✅ scrapowanie przepisów
✅ zapisywanie i wyświetlanie ulubionych przepisów użytkownika
✅ analiza wykorzystania składników
✅ analiza spożytych kalorii
✅ analiza finansowa
✅ dodawanie znajomych
✅ ocenianie aplikacji
✅ rekomendacja aplikacji
✅ dark mode
✅ dodawanie własnych przepisów
✅ ranking użytkowników z odznakami
✅ Transport danych do excela
✅ Ranking przepisów

Przewodnik użytkownika

Użytkownik po wejściu na stronę i kliknięciu przycisku Let’s Start Cooking przechodzi do menu aplikacji gdzie może:  
Korzystać z aplikacji jako gość - wybierając tą opcję użytkownik ma dostęp do podstawowej funkcjonalności aplikacji czyli wyszukiwania przepisów na podstawie podanych składników
Wyświetlić aktualny ranking przepisów (ranking generowany jest na podstawie ilości zapisów danego przepisu przez różnych użytkowników - przykład Przepis nr1 zapisał Użytkownik1 i Użytkownik2 więc przepis nr1 ma dwa punkty - jeżeli z pośród pozostałych przepisów ten przepis ma najwięcej punktów jest wyświetlany jako pierwszy w rangigu jeżeli istnieje jeden przepis który ma więcej punktów niż nas przepis nr1 to przepis nr1 jest na drugiej pozycji w rangingu etc.) 
Wyświetlić aktualny ranking użytkowników ranking użytkowników generowany jest na podstawie punktów uzyskanych przez użytkowników punkty przyznawane za odpowiedni: 
Wykorzystanie kodu polecającego (5 punktów dla użytkownika który wpisał kod, 10 punktów dla użytkownika którego był kod)
Ocenienie Aplikacji (5 puntków)
Dodanie własnego przepisu (20 puntków) 

UWAGA: PUNKTY ZDOBYWAĆ MOGĄ JEDYNIE ZALOGOWANI UŻYTKOWNICY 

Zarejestrować się - użytkownik może założyć konto na stronie podając email, imię, nazwisko, hasło oraz opcjonalnie podać kod polecający od innego użytkownika. Po dokonaniu rejestracji użytkownik proszony jest o aktywację swojego konta poprzez link przesłany na podany adres email. Po pomyślnym aktywowaniu konta użytkownik może się zalogować oraz korzystać z opcji dostępnych wyłącznie dla zarejestrowanych użytkowników.

Zalogować się - jeżeli użytkownik poprawie dokonał rejestracji może zalogować się na swoje konto, jednakże jeżeli użytkownik zapomniał hasła może je zresetować poprzez kliknięcie przycisku Reset Password które przekieruje go do formularza w którym musi podać email na który zarejestrował swoje konto, po wprowadzeniu emaila na konto email zostaje przesłany link do zmiany hasła użytkownika gdzie może zresetować swoje hasło. 

Dodatkowo użytkownik niezależnie od tego czy jest zalogowany czy nie w każdym momencie może włączyć i wyłączyć tryb dark mode. 

Funkcje dostępne dla zalogowanego użytkownika
Użytkownik po poprawnym zalogowaniu przechodzi do okna w którym może wyszukiwać przepisy na podstawie wprowadzonych składników, po wyszukiwaniu przepisu w odróżnieniu od niezalogowanego użytkownika może zapisać swój przepis, swoje zapisane przepisy użytkownik może przeglądać i usuwać przechodząc do options -> Saved Recipes 

Oprócz wyszukiwania przepisów zalogowany użytkownik może także przejść do wyżej wspomnianego widoku opcji który oferuje następujące możliwości: 
My Account - po przejściu do tego okna użytkownik uzyskuje wgląd w swoje dane personalne takie jak: 
Id 
Username
Email

Użytkownik w tym oknie może także zmienić hasło oraz email (UWAGA przy zmianie emaila użytkownik zostanie poproszony o ponowną aktywację konta poprzez email i ponowne zalogowanie na konto) 

2) Add your Recipe - po przejściu do tego okna użytkownik ma następujące możliwości: 
Add your own Recipe - użytkownik może stworzyć swój własny autorski przepis 
Add a Recipe Based on an Existing One - użytkownik może stworzyć przepis na podstawie istniejącego już przepisu modyfikując go w dowolny sposób

3) Saved Recipes - po przejściu do tego okna użytkownik uzyskuje wgląd do swoich zapisanych przepisów 

4) Friends - po przejściu do tego okna użytkownik może:
Wyświetlić swoich znajomych, przeglądać ich ulubione przepisy oraz wedle uznania usunąć znajomego ze swojej listy 
Dodawać znajomych - po przejściu do tej sekcji użytkownik może dodać znajomego podając jego email, po dodaniu znajomego zostaje przesłane mu zaproszenie które w przypadku akceptacji spowoduje wyświetlenie znajomego na naszej wcześniej wspomnianej liście 
Przeglądać otrzymane zaproszenia - w tej sekcji użytkownik może zobaczyć zaproszenia do znajomych, akceptować je lub odrzucać w przypadku akceptacji zaproszenia przyjęty znajomy zostanie wyświetlony na liście znajomych

5) Your Analysis - po przejściu do tej sekcji użytkownik ma wgląd do swoich analiz:
Food Analysis - Analiza ilości spożycia danego produktu ogólnie
Financial Analysis - Przybliżone wartości wydane na przepisy w danym dniu
Caloric - ilość spożytych kalorii w danym dniu 

Każdą z wyżej wspomnianych analiz użytkownik może pobrać w formie pliku excel

6) Rate App - w tej sekcji użytkownik może ocenić aplikację w skali 1-5 gdzie
 	
7) Recommend App - w tej sekcji użytkownik może uzyskać swój unikalny kod polecający 

8) Sign out - kliknięcie tego przycisku spowoduje wylogowanie 

9) Go back - powrót do widoku z wyszukiwaniem przepisów

Zmienne 

 
"ConnectionStrings": {
 "MyAzureDb": "Server=tcp:coookiedb.database.windows.net,1433;Initial Catalog=coookiedb;Persist Security Info=False;User ID=coookieadmin;Password=truten123@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
},
Ta sekcja dodaje połączenie do bazy danych Azure SQL. Dzięki temu aplikacja może łączyć się z bazą danych o nazwie “cookiedb” na serwerze “coookiedb.database.windows.net” przy użyciu podanych danych uwierzytelniających.

  
 "SmtpSettings": {
 "Server": "smtp.office365.com",
 "Port": 587,
 "SenderName": "Coookie App",
 "SenderEmail": "coookieapp@outlook.com",
 "Username": "coookieapp@outlook.com",
 "Password": "Truten123"
}

Ta sekcja dodaje ustawienia serwera SMTP potrzebne do wysyłania wiadomości e-mail. 
