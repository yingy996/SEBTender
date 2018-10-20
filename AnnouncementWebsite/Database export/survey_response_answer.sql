-- phpMyAdmin SQL Dump
-- version 4.7.7
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Oct 19, 2018 at 07:06 AM
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
-- Table structure for table `survey_response_answer`
--

CREATE TABLE `survey_response_answer` (
  `resp_answerID` int(7) NOT NULL,
  `responseID` int(7) NOT NULL,
  `questionID` int(7) NOT NULL,
  `answerID` int(7) DEFAULT NULL,
  `text_answer` text
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `survey_response_answer`
--

INSERT INTO `survey_response_answer` (`resp_answerID`, `responseID`, `questionID`, `answerID`, `text_answer`) VALUES
(1965426, 4204359, 4677497, 0, 'no issue'),
(2095429, 9920080, 3325764, 0, 'answer 4'),
(4204581, 9920080, 1276369, 5165153, ''),
(5124101, 9920080, 1232964, 0, 'answer 5'),
(5389353, 4204359, 1276369, 4113307, ''),
(6196256, 4204359, 8041364, 0, 'Updating tender list notice should close automatically after update finishes.'),
(6587302, 9920080, 3886204, 8739512, ''),
(6770275, 4204359, 3325764, 0, 'Simple with the colors, no headache.'),
(8190463, 9920080, 8041364, 0, 'answer 6'),
(9489883, 4204359, 3886204, 7489502, ''),
(9522346, 4204359, 1232964, 0, 'none'),
(9952439, 9920080, 4677497, 0, 'answer 3');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `survey_response_answer`
--
ALTER TABLE `survey_response_answer`
  ADD PRIMARY KEY (`resp_answerID`),
  ADD KEY `responseID` (`responseID`),
  ADD KEY `questionID` (`questionID`),
  ADD KEY `answerID` (`answerID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
