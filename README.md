# SportFogadasWPF

## Indítás

1. [SportFogadasWPF.sql](https://github.com/Hunor64/SportFogadasWPF/blob/master/SportFogadasWPF.sql) lefuttatása MySQL-ben.
2. A project futtatása Visual Studio-ban.

## UI Tervek

Az UI terveket [itt találod](https://github.com/Hunor64/SportFogadasWPF/tree/master/UI%20template).

## Szerepkörök

### Guest

- Megnézheti az eventeket, de nem fogadhat.

### User: Bettor

- Példa Felhasználónév/Jelszó: john
- Regisztrálhat, fogadásokat tehet, egyenlegét kezelheti.
- Megtekintheti saját fogadásait és azok státuszát.

### Moderator: Organizer

- Példa Felhasználónév/Jelszó: jane
- Minden jogot megkap, amit a User szerepkör is, de ezen felül:
  - Készíthet új eseményeket és törölhet régieket ha még nem fogadtak rá.

### Admin

- Példa Felhasználónév/Jelszó: admin
- Teljes hozzáférése van az összes felhasználó adatához, beleértve azok egyenlegét, státuszát, és jelszavait (hash-elve).
- Új felhasználók létrehozása, meglévők szerkesztése.

## Munkamegosztás

### Hunor

C# Backend, Adatbázis

### Laci 

XAML Frontend
