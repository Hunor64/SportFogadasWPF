Indítás:
1. <a>https://github.com/Hunor64/SportFogadasWPF/blob/master/SportFogadasWPF.sql</a> Lefuttatása mysql-ben
2. project futtatása visual studióban

UI Tervek:
<a>https://github.com/Hunor64/SportFogadasWPF/tree/master/UI%20template</a>

Szerepkörök:

Guest:

Megnézheti az eventeket. Nem fogadhat.

User: Bettor

Regisztrálhat, fogadásokat tehet, egyenlegét kezelheti. Megtekintheti saját fogadásait és azok státuszát.

Nem fér hozzá adminisztrációs funkciókhoz (pl. új események létrehozása, felhasználók kezelése).

Moderator: Organizer

Minden jogot megkap, amit a User szerepkör is, de ezen felül: Láthatja más felhasználók fogadásait. Adminisztrálhatja a felhasználókat (pl. letilthatja őket, vagy aktiválhatja a fiókjukat). Felülvizsgálhatja és módosíthatja a fogadások státuszát. Új eseményeket hozhat létre vagy törölhet. Módosíthatja az események adatait (pl. időpont, helyszín).

Admin:

Teljes hozzáférése van az összes felhasználó adatához, beleértve azok egyenlegét, státuszát, és jelszavait (hash-elve). Új felhasználó, meglévő törlése, jelszó alaphelyzetbe hozása,
