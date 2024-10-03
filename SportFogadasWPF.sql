CREATE TABLE `Bets` (
  `BetID` INT PRIMARY KEY AUTO_INCREMENT,
  `BetDate` DATE NOT NULL,
  `Odds` FLOAT NOT NULL,
  `Amount` INT NOT NULL,
  `BettorsID` INT NOT NULL,
  `EventID` INT NOT NULL,
  `Status` BOOLEAN NOT NULL
);

CREATE TABLE `Bettors` (
  `BettorsID` INT PRIMARY KEY AUTO_INCREMENT,
  `Username` VARCHAR(50) NOT NULL,
  `Password` VARCHAR(255) NOT NULL,
  `Balance` INT NOT NULL DEFAULT 0,
  `Email` VARCHAR(100) NOT NULL,
  `JoinDate` DATE NOT NULL,
  `IsActive` BOOLEAN NOT NULL DEFAULT 1,
  `Privilage` ENUM ('user', 'organiser', 'admin') NOT NULL DEFAULT 'user'
);

CREATE TABLE `Events` (
  `EventID` INT PRIMARY KEY AUTO_INCREMENT,
  `EventName` VARCHAR(100) NOT NULL,
  `EventDate` DATE NOT NULL,
  `Category` VARCHAR(50) NOT NULL,
  `Location` VARCHAR(100) NOT NULL
);

ALTER TABLE `Bets` ADD FOREIGN KEY (`BettorsID`) REFERENCES `Bettors` (`BettorsID`);

ALTER TABLE `Bets` ADD FOREIGN KEY (`EventID`) REFERENCES `Events` (`EventID`);

INSERT INTO `Bettors` (`Username`, `Balance`, `Email`, `JoinDate`, `IsActive`, `Privilage`) VALUES
('john_doe', 1000, 'john@example.com', '2022-01-15', 1, 'user'),
('jane_smith', 1500, 'jane@example.com', '2022-02-20', 1, 'organiser'),
('admin_user', 5000, 'admin@example.com', '2021-12-01', 1, 'admin');

INSERT INTO `Events` (`EventName`, `EventDate`, `Category`, `Location`) VALUES
('Football Match', '2023-05-10', 'Sports', 'Stadium A'),
('Basketball Game', '2023-06-15', 'Sports', 'Arena B'),
('Concert', '2023-07-20', 'Entertainment', 'Concert Hall C');

INSERT INTO `Bets` (`BetDate`, `Odds`, `Amount`, `BettorsID`, `EventID`, `Status`) VALUES
('2023-04-01', 2.5, 200, 1, 1, 1),
('2023-04-02', 1.8, 150, 2, 2, 0),
('2023-04-03', 3.0, 300, 1, 3, 1);
