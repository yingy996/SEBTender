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
-- Table structure for table `survey_question`
--

CREATE TABLE `survey_question` (
  `questionID` int(7) NOT NULL,
  `questionTitle` text NOT NULL,
  `surveyID` int(7) NOT NULL,
  `questionType` varchar(20) NOT NULL,
  `questionNumber` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `survey_question`
--

INSERT INTO `survey_question` (`questionID`, `questionTitle`, `surveyID`, `questionType`, `questionNumber`) VALUES
(1232964, 'Which part of the app did you found it hard to use? Why?', 1229037, 'longsentence', 4),
(1276369, 'On a scale of 1-5 with 5 meaning the easiest, how easy is it to use and figure out the app?', 1229037, 'dropdown', 0),
(3325764, 'Which feature(s) of the application do you like most? Why?', 1229037, 'longsentence', 3),
(3886204, 'On a scale of 1-5 with 5 meaning the fastest, how fast is the application to process your requests?', 1229037, 'dropdown', 1),
(4677497, 'Does the app crash, hang or freeze? If so at which point(s)?', 1229037, 'longsentence', 2),
(8041364, 'What do you think can be improved in the application?', 1229037, 'longsentence', 5);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `survey_question`
--
ALTER TABLE `survey_question`
  ADD PRIMARY KEY (`questionID`),
  ADD KEY `surveyID` (`surveyID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
