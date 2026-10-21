Raamatukogu infosüsteemi arendamine ORM-i abil
1. Projekti kirjeldus
Selle projekti eesmärk on luua raamatukogu infosüsteem C# ja Entity Framework Core'i abil. Rakendus võimaldab lisada, otsida, muuta ja kustutada raamatuid ning vaadata lugejate laenutusi.
Andmebaasiga suhtlemiseks kasutasin Entity Framework Core'i (EF Core), mis teeb andmebaasi kasutamise lihtsamaks.

2. Andmemudelid
Projektis on kolm klassi: Book, Reader ja Loan.

Book – sisaldab raamatu ID-d, pealkirja, autorit, ilmumisaastat ja saadavuse infot.
Reader – sisaldab lugeja ID-d, nime ja e-posti aadressi.
Loan – sisaldab laenutuse ID-d, raamatu ja lugeja ID-d ning laenutamise ja tagastamise kuupäevi.
Ühel lugejal võib olla mitu laenutust. Üks raamat võib olla seotud mitme laenutuse kirjega.

3. DbContext ja andmebaas
Kasutasin DbContext klassi, et hallata andmebaasiga suhtlemist. DbSet abil määrasin tabelid, kuhu salvestatakse raamatud, lugejad ja laenutused.
Andmebaasi ühenduse seadistus asub rakenduse konfiguratsioonifailis. Ühenduse string määrab, millise andmebaasiga rakendus ühendub.
Andmebaasiga suhtlemine on eraldatud ülejäänud rakenduse loogikast, et kood oleks lihtsamini loetav ja hallatav.

4. Andmete lisamine ja pärimine
Rakenduses saab lisada uusi raamatuid. Uue raamatu lisamisel määratakse selle saadavuseks true.
LINQ-i ja EF Core'i meetodite abil saab:
leida raamatuid ilmumisaasta järgi;
otsida raamatut pealkirja järgi;
vaadata raamatuid, mis ei ole hetkel saadaval;
vaadata konkreetse lugeja laenutusi.

6. Andmete muutmine ja kustutamine
Rakenduses saab muuta raamatu pealkirja ja kustutada raamatu andmebaasist.
Raamatu tagastamisel määratakse raamatu saadavuseks true ning salvestatakse tagastamise kuupäev.
Enne raamatu kustutamist tuleb kontrollida, kas see on seotud laenutustega. Kui seos on olemas, tuleb otsustada, kuidas laenutuste andmeid säilitada või kustutamist takistada, et andmebaasi seosed jääksid õigeks.

6. Arhitektuur ja põhjendused
Milline on DbContext-i ülesanne?
DbContext ühendab rakenduse andmemudelid andmebaasiga. Selle abil saab andmeid lugeda, lisada, muuta ja kustutada. Muudatused salvestatakse andmebaasi meetodiga SaveChanges() või SaveChangesAsync().
Miks ei ole hea panna kõiki päringuid kasutajaliidese koodi?
Kui kõik andmebaasipäringud on kasutajaliidese koodis, muutub programm keerulisemaks ja raskemini hooldatavaks. Kui andmebaasiga suhtlemine on eraldi kihis, on koodi lihtsam muuta, testida ja taaskasutada.
Millisesse kihti paigutaksin andmebaasiga suhtlemise MVC- või VIPER-arhitektuuris?
MVC-arhitektuuris paigutaksin andmebaasiga suhtlemise eraldi andmekihti ehk Repository-kihti. Mudelid kirjeldavad andmeid, kontroller töötleb kasutaja tegevusi ja vaade näitab infot kasutajale.
VIPER-arhitektuuris paigutaksin andmebaasiga suhtlemise Interactor'i kaudu eraldi Repository- või teenusekihti. Nii on rakenduse osad üksteisest paremini eraldatud.

7. Kokkuvõte
Selle töö käigus õppisin kasutama Entity Framework Core'i ja LINQ-i andmebaasiga suhtlemiseks. Samuti õppisin looma andmemudeleid, kasutama DbContext-i ning lisama, otsima, muutma ja kustutama andmeid.
Minu arvates muudab selline lahendus koodi arusaadavamaks ja aitab rakendust edaspidi arendada.
