-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2024. Okt 03. 14:42
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `bets`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `bettors`
--

CREATE TABLE `bettors` (
  `BettorsID` int(11) NOT NULL,
  `Username` varchar(50) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `Balance` int(11) NOT NULL DEFAULT 0,
  `Email` varchar(100) NOT NULL,
  `JoinDate` date NOT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT 1,
  `Privilage` enum('user','organiser','admin') NOT NULL DEFAULT 'user'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- A tábla adatainak kiíratása `bettors`
--

INSERT INTO `bettors` (`BettorsID`, `Username`, `Password`, `Balance`, `Email`, `JoinDate`, `IsActive`, `Privilage`) VALUES
(1, 'john_doe', '96d9632f363564cc3032521409cf22a852f2032eec099ed5967c0d000cec607a', 1000, 'john@example.com', '2022-01-15', 1, 'user'),
(2, 'jane_smith', '', 1500, 'jane@example.com', '2022-02-20', 1, 'organiser'),
(3, 'admin_user', '', 5000, 'admin@example.com', '2021-12-01', 1, 'admin'),
(10, 'a', 'ca978112ca1bbdcafac231b39a23dc4da786eff8147c4e72b9807785afee48bb', 0, 'a', '2024-10-03', 1, 'user'),
(11, 'aa', '961b6dd3ede3cb8ecbaacbd68de040cd78eb2ed5889130cceb4c49268ea4d506', 0, 'aa', '2024-10-03', 1, 'user');

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `bettors`
--
ALTER TABLE `bettors`
  ADD PRIMARY KEY (`BettorsID`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `bettors`
--
ALTER TABLE `bettors`
  MODIFY `BettorsID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
