-- phpMyAdmin SQL Dump
-- version 4.7.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Oct 07, 2018 at 05:38 PM
-- Server version: 10.1.25-MariaDB
-- PHP Version: 7.1.7

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
-- Table structure for table `scrapped_tender`
--

CREATE TABLE `scrapped_tender` (
  `id` int(11) NOT NULL,
  `reference` varchar(100) DEFAULT NULL,
  `title` text NOT NULL,
  `category` varchar(100) DEFAULT NULL,
  `originatingSource` varchar(100) DEFAULT NULL,
  `tenderSource` tinyint(10) DEFAULT NULL,
  `agency` varchar(100) DEFAULT NULL,
  `closingDate` varchar(50) DEFAULT NULL,
  `startDate` varchar(50) DEFAULT NULL,
  `docInfoJson` text,
  `originatorJson` text,
  `fileLinks` text
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `scrapped_tender`
--
ALTER TABLE `scrapped_tender`
  ADD PRIMARY KEY (`id`),
  ADD KEY `tenderSource` (`tenderSource`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `scrapped_tender`
--
ALTER TABLE `scrapped_tender`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- Constraints for dumped tables
--

--
-- Constraints for table `scrapped_tender`
--
ALTER TABLE `scrapped_tender`
  ADD CONSTRAINT `scrapped_tender_ibfk_1` FOREIGN KEY (`tenderSource`) REFERENCES `tender_source` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
