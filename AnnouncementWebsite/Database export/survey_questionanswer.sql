-- phpMyAdmin SQL Dump
-- version 4.7.7
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Oct 19, 2018 at 08:15 AM
-- Server version: 10.1.31-MariaDB
-- PHP Version: 7.0.26

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `id5453709_sarawaktenderapplication_db`
--

-- --------------------------------------------------------

--
-- Table structure for table `survey_questionanswer`
--

CREATE TABLE `survey_questionanswer` (
  `answerID` int(7) NOT NULL,
  `answerTitle` text NOT NULL,
  `questionID` int(7) NOT NULL,
  `surveyID` int(7) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `survey_questionanswer`
--

INSERT INTO `survey_questionanswer` (`answerID`, `answerTitle`, `questionID`, `surveyID`) VALUES
(2341647, '3', 1276369, 1229037),
(2851555, '4', 3886204, 1229037),
(3295750, 'Does the app crash, hang or freeze? If so at which point(s)?', 4677497, 1229037),
(4113307, '5', 1276369, 1229037),
(5165153, '1', 1276369, 1229037),
(7423264, 'What do you think can be improved in the application?', 8041364, 1229037),
(7489502, '5', 3886204, 1229037),
(8739512, '2', 3886204, 1229037),
(8801806, '2', 1276369, 1229037),
(8919887, '1', 3886204, 1229037),
(9190270, '3', 3886204, 1229037),
(9957955, '4', 1276369, 1229037);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `survey_questionanswer`
--
ALTER TABLE `survey_questionanswer`
  ADD PRIMARY KEY (`answerID`),
  ADD KEY `questionID` (`questionID`),
  ADD KEY `surveyID` (`surveyID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
