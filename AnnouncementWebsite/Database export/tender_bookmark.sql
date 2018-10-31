-- phpMyAdmin SQL Dump
-- version 4.7.7
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Oct 30, 2018 at 01:18 PM
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
-- Database: `id7317248_pockettenderdb`
--

-- --------------------------------------------------------

--
-- Table structure for table `tender_bookmark`
--

CREATE TABLE `tender_bookmark` (
  `bookmarkID` int(11) NOT NULL,
  `username` varchar(15) NOT NULL,
  `originatingSource` varchar(100) NOT NULL,
  `tenderReferenceNumber` varchar(100) DEFAULT NULL,
  `tenderTitle` text NOT NULL,
  `isAvailable` tinyint(1) NOT NULL,
  `bookmarkDate` date NOT NULL,
  `closingDate` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tender_bookmark`
--

INSERT INTO `tender_bookmark` (`bookmarkID`, `username`, `originatingSource`, `tenderReferenceNumber`, `tenderTitle`, `isAvailable`, `bookmarkDate`, `closingDate`) VALUES
(107, 'slo', 'KEMENTERIAN PENDIDIKAN MALAYSIA', 'T3/18/BM/HUSM/15', 'PROVISION &quot;CARDIAC ANGIOGRAPHY FACILITY WITH BIPLANE SYSTEM AND ESSENTIAL SUPPORTING DEVICES&quot; TO HOSPITAL UNIVERSITI SAINS MALAYSIA, KAMPUS KESIHATAN, KUBANG KERIAN, KELANTAN, ON A RENTAL BASIS - KOD BIDANG 050101 - TARIKH TAKLIMAT TENDER PADA 25 SEPTEMBER 2018 (SELASA), JAM 10.00 PAGI - BERTEMPAT DI MAKMAL KARDIOLOGI INVASIF, HOSPITAL UNIVERSITI SAINS MALAYSIA, KAMPUS KESIHATAN, KUBANG KERIAN, KELANTAN.', 1, '2018-10-14', '2020-10-15 00:00:00'),
(108, 'slo', 'Telekom', '', 'REQUEST FOR QUOTATION (RFQ) FOR LICENSE PLATE RECOGNITION SYSTEM AND VEHICLE COUNTING FOR MEDINI ISKANDAR MALAYSIA CITY-WIDE SOLUTION SPECIFICATION', 1, '2018-10-14', '1970-01-01 00:00:00'),
(109, 'slo', 'MBKS', 'MBKS/BLD(Q)-No.42/2018', 'Service &amp; Maintenance of Lifts For Dewan Bandaraya Kuching Selatan For The Year 2019', 1, '2018-10-14', '2018-10-17 00:00:00'),
(115, 'sharon96', 'Sarawak Energy Head Office', 'Opening Hours for Sale of Tender Document', 'Please be informed that tender documents are obtainable from 1st Floor Sarawak Energy Berhad, Wisma SEB, No.1, The Isthmus, 93050 Kuching, Sarawak at the following hours: MON-THU 8am-12nn, FRI 8am-1030am', 1, '2018-10-15', '2020-12-31 00:00:00'),
(116, 'slo', 'Sarawak Energy Head Office', '11-04-2018', 'CONTRACTOR BRIEFING &quot;Creating Opportunities in Sarawak Energy&quot; - Day 1', 1, '2018-10-15', '2020-04-27 00:00:00'),
(118, 'koala', 'KEMENTERIAN PERTAHANAN', 'KP/PERO-6/D/T109/2018/OE', 'PERKHIDMATAN DOBI DAN JAHITAN DI AKADEMI TENTERA UDARA IPOH, PERAK', 1, '2018-10-16', '2018-10-30 00:00:00'),
(120, 'Maxim3839', 'Sarawak Energy Bintulu', 'BTU2(A)/2018/TLI', 'RE-TENDER FOR REPLACEMENT OF 33kV OUTDOOR SWITCHGEAR AT TOWN 33/11kV &amp; HOUSING 33/11kV SUBSTATIONS', 1, '2018-10-21', '2020-11-21 00:00:00'),
(121, 'sharon96', 'Sarawak Energy Head Office', 'PLS160085', 'Serudit to Sri Aman 132kV Transmission Line Project', 1, '2018-10-21', '2020-11-28 00:00:00'),
(124, 'gene', 'Sarawak Energy Head Office', '11-04-2018', 'CONTRACTOR BRIEFING &quot;Creating Opportunities in Sarawak Energy&quot; - Day 1', 1, '2018-10-30', '2020-04-27 00:00:00');

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
  MODIFY `bookmarkID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=125;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
