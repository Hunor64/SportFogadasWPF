CREATE TABLE Bets ( BetsID INT AUTO_INCREMENT PRIMARY KEY,
BetDate DATE NOT NULL,
Odds FLOAT NOT NULL,
Amount INT NOT NULL,
BettorsID INT NOT NULL,
EventID INT NOT NULL,
Status BOOLEAN NOT NULL,
FOREIGN KEY (BettorsID) REFERENCES Bettors(BettorsID), FOREIGN KEY (EventID) REFERENCES Events(EventID) );

CREATE TABLE Bettors ( BettorsID INT AUTO_INCREMENT PRIMARY KEY,
Username VARCHAR(50) NOT NULL,
Balance INT NOT NULL,
Email VARCHAR(100) NOT NULL,
JoinDate DATE NOT NULL,
IsActive BOOLEAN NOT NULL DEFAULT 1
);

CREATE TABLE Events ( EventID INT AUTO_INCREMENT PRIMARY KEY,
EventName VARCHAR(100) NOT NULL,
EventDate DATE NOT NULL,
Category VARCHAR(50) NOT NULL,
Location VARCHAR(100) NOT NULL
);

Szerepkörök és jogosultságok tervezete:

User: Bettor

Regisztrálhat, fogadásokat tehet, egyenlegét kezelheti. Megtekintheti saját fogadásait és azok státuszát.

Nem fér hozzá adminisztrációs funkciókhoz (pl. új események létrehozása, felhasználók kezelése).

Moderator: Organizer

Minden jogot megkap, amit a User szerepkör is, de ezen felül: Láthatja más felhasználók fogadásait. Adminisztrálhatja a felhasználókat (pl. letilthatja őket, vagy aktiválhatja a fiókjukat). Felülvizsgálhatja és módosíthatja a fogadások státuszát. Új eseményeket hozhat létre vagy törölhet. Módosíthatja az események adatait (pl. időpont, helyszín).

Admin:

Teljes hozzáférése van az összes felhasználó adatához, beleértve azok egyenlegét, státuszát, és jelszavait (hash-elve). Új felhasználó, meglévő törlése, jelszó alaphelyzetbe hozása,
