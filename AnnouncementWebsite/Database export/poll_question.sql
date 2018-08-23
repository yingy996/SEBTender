-- phpMyAdmin SQL Dump
-- version 4.7.4
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Aug 22, 2018 at 11:33 AM
-- Server version: 10.1.29-MariaDB
-- PHP Version: 7.2.0

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `sarawaktenderapplication_db`
--

-- --------------------------------------------------------

--
-- Table structure for table `poll_question`
--

CREATE TABLE `poll_question` (
  `pollID` int(7) NOT NULL,
  `pollQuestion` text NOT NULL,
  `postedBy` varchar(15) NOT NULL,
  `publishedDate` datetime NOT NULL,
  `editedDate` datetime DEFAULT NULL,
  `editedBy` varchar(15) DEFAULT NULL,
  `endDate` datetime DEFAULT NULL,
  `isEnded` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `poll_question`
--

INSERT INTO `poll_question` (`pollID`, `pollQuestion`, `postedBy`, `publishedDate`, `editedDate`, `editedBy`, `endDate`, `isEnded`) VALUES
(8895217, 'What is your favourite food cuisine?', 'admintest123', '2018-08-21 19:29:00', NULL, NULL, NULL, 0);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `poll_question`
--
ALTER TABLE `poll_question`
  ADD PRIMARY KEY (`pollID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
