-- phpMyAdmin SQL Dump
-- version 4.7.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Oct 13, 2018 at 10:15 AM
-- Server version: 10.1.26-MariaDB
-- PHP Version: 7.1.8

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
-- Table structure for table `tender_bookmark`
--

CREATE TABLE `tender_bookmark` (
  `bookmarkID` int(11) NOT NULL,
  `username` varchar(15) NOT NULL,
  `tenderReferenceNumber` varchar(100) DEFAULT NULL,
  `tenderTitle` text NOT NULL,
  `isAvailable` tinyint(1) NOT NULL,
  `bookmarkDate` date NOT NULL,
  `closingDate` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tender_bookmark`
--

INSERT INTO `tender_bookmark` (`bookmarkID`, `username`, `tenderReferenceNumber`, `tenderTitle`, `isAvailable`, `bookmarkDate`, `closingDate`) VALUES
(3, 'test', NULL, '321', 1, '2018-04-22', NULL),
(59, 'dinosaur96', 'PLS-160128', 'Medamit to Lawas Town 275kV Transmission Line Project', 1, '2018-05-03', '2018-06-13 15:00:00'),
(60, 'crocodile96', 'TLO1/18', 'SUPPLY &amp; DELIVERY OF PERSONAL PROTECTIVE EQUIPMENT FOR TRANSMISSION LINES LINESMEN', 1, '2018-05-03', '2018-05-16 15:00:00'),
(61, 'dinosaur96', 'LIM03/2018/MFS', 'Tender Sale of Scrap Materials at Limbang Electrical Store, \r\nSyarikat SESCO Berhad (Northern Region)', 1, '2018-05-03', '2018-05-16 15:00:00'),
(63, 'koala96', 'TLO1/18', 'SUPPLY &amp; DELIVERY OF PERSONAL PROTECTIVE EQUIPMENT FOR TRANSMISSION LINES LINESMEN', 1, '2018-05-03', '2018-05-16 15:00:00'),
(71, 'dinosaur96', 'TLO1/18', 'SUPPLY &amp; DELIVERY OF PERSONAL PROTECTIVE EQUIPMENT FOR TRANSMISSION LINES LINESMEN', 1, '2018-05-05', '2018-05-16 15:00:00'),
(75, 'dinosaur96', 'Opening Hours for Sale of Tender Document', 'Please be informed that tender documents are obtainable from 1st Floor Sarawak Energy Berhad, Wisma SEB, No.1, The Isthmus, 93050 Kuching, Sarawak at the following hours: MON-THU 8am-12nn, FRI 8am-1030am', 1, '2018-05-06', '2029-12-31 15:00:00'),
(76, 'dinosaur96', 'DPC36/18', 'Tender for Supply &amp; Delivery of 33kV XLPE Underground Cable', 1, '2018-05-06', '2018-05-16 15:00:00'),
(77, 'dinosaur96', 'PMS136/17A', 'Tender for installation of additional VAV and recommissioning of chilled water system and air distribution system at Menara Sarawak Energy', 1, '2018-05-07', '2018-05-10 15:00:00'),
(81, 'dinosaur96', 'DPC34/18', 'Tender for Annual Supply &amp; Delivery of 7/4.65 Aluminium Alloy Stranded Conductor', 1, '2018-05-13', '2018-05-23 15:00:00'),
(83, 'koala96', 'SA04/18', 'Supply And Delivery Of D20MX RTU Parts', 1, '2018-05-15', '2018-06-06 15:00:00'),
(90, 'dinosaur96', 'SA04/18', 'Supply And Delivery Of D20MX RTU Parts', 1, '2018-05-29', '2018-06-06 15:00:00'),
(92, 'dinosaur96', '11-04-2018', 'CONTRACTOR BRIEFING &quot;Creating Opportunities in Sarawak Energy&quot; - Day 1', 1, '2018-06-03', '2020-04-27 15:00:00'),
(94, 'dinosaur96', 'BPS021/18', 'SUPPLY, DELIVERY, INSTALLATION AND COMMISSIONING OF FOUR (4) SETS HIGH PRESSURE STEAM DRUM LEVEL GAUGES C/W LED ILLUMINATOR AT TG. KIDURONG POWER STATION, BINTULU', 1, '2018-06-03', '2018-06-20 15:00:00'),
(96, 'dinosaur96', 'SA03/18', 'Supply And Delivery Of RTUs', 1, '2018-09-05', '2018-10-03 15:00:00'),
(100, 'slo', '11-04-2018', 'CONTRACTOR BRIEFING &quot;Creating Opportunities in Sarawak Energy&quot; - Day 1', 1, '2018-09-26', '2020-04-27 15:00:00'),
(101, 'slo', 'Sri(NW)10(a)/2018/JN/la', 'Re-tender for the Servicing And Maintenance Of Fire Protection System And Fire Extinguishers For Sri Aman Division (Western Region)', 1, '2018-09-26', '2018-10-31 15:00:00');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `tender_bookmark`
--
ALTER TABLE `tender_bookmark`
  ADD PRIMARY KEY (`bookmarkID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `tender_bookmark`
--
ALTER TABLE `tender_bookmark`
  MODIFY `bookmarkID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=102;COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
