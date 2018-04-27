-- phpMyAdmin SQL Dump
-- version 4.7.7
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Apr 27, 2018 at 04:22 PM
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
-- Table structure for table `administrator`
--

CREATE TABLE `administrator` (
  `administratorID` int(10) NOT NULL,
  `username` varchar(15) NOT NULL,
  `password` text NOT NULL,
  `administratorName` varchar(50) NOT NULL,
  `administratorEmail` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `administrator`
--

INSERT INTO `administrator` (`administratorID`, `username`, `password`, `administratorName`, `administratorEmail`) VALUES
(1, 'admintest123', '$2y$10$4kZSMW0o7A.1BK.zvIp6.eqchwz0nxjSb1q6Rob70ns6Q.Lc5Q46W', 'Upshur', 'upshur@yahoo.com');

-- --------------------------------------------------------

--
-- Table structure for table `announcement`
--

CREATE TABLE `announcement` (
  `announcementID` int(7) NOT NULL,
  `announcementTitle` varchar(50) NOT NULL,
  `announcementContent` text NOT NULL,
  `publishedDate` datetime NOT NULL,
  `editedDate` datetime DEFAULT NULL,
  `editedBy` varchar(15) DEFAULT NULL,
  `postedBy` text NOT NULL,
  `postDeleted` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `announcement`
--

INSERT INTO `announcement` (`announcementID`, `announcementTitle`, `announcementContent`, `publishedDate`, `editedDate`, `editedBy`, `postedBy`, `postDeleted`) VALUES
(1318775, 'Ios testing e', 'iOS testing edi', '2018-04-23 12:10:25', '2018-04-23 12:31:25', 'admintest123', 'Admintest123', 1),
(1378742, 'Editting something1', 'aaabbbccc a2', '2018-04-23 07:24:35', '2018-04-23 14:34:59', 'admintest123', 'admintest123', 0),
(1760040, 'post33', 'qqqq3', '2018-04-19 13:32:04', '2018-04-27 10:26:10', 'admintest123', 'admintest123', 1),
(1810468, 'dddddd', 'qqqqq', '2018-04-19 13:32:48', '2018-04-19 14:26:24', 'admintest123', 'admintest123', 1),
(2023830, 'testing 231', 'This is a content', '2018-04-21 05:30:53', '2018-04-21 15:26:54', 'admintest123', 'admintest123', 1),
(2490165, 'I\'m hurl it yes yes', 'This is a content of an announcement. It means nothing at all.', '2018-04-20 16:18:39', NULL, NULL, 'admintest123', 0),
(3183046, 'post2', 'aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa', '2018-04-12 14:27:27', '2018-04-19 13:29:39', 'admintest123', 'admintest123', 0),
(3607912, 'urs', 'im', '2018-04-21 08:19:18', '2018-04-21 09:22:55', '', 'admintest123', 1),
(4103855, 'star platinum', 'za warudo', '2018-04-23 10:57:42', '2018-04-25 16:36:07', 'admintest123', 'admintest123', 0),
(4847620, 'adasd', 'dasdad dasdad  fsdf f fe w w f d d g gd sgds g ef wef ewg weg weg we gwe f  df ds fds fsd fs g we gwe ge he r ws fa f gweg w ga f sd gsd g dfg df g d e g erh er her her g dsg   sf sd fds gsd gdr hg er e rge rh erh erg sd f s df sd fsd f wt w g  sdg sd g r g gdfg', '2018-04-20 16:07:57', '2018-04-27 10:24:41', 'admintest123', 'admintest123', 0),
(4936341, 'gasgasgas eurobeat intensifies', 'aaaaaaaaa kansei dorifuto', '2018-04-19 14:26:41', '2018-04-22 16:06:47', 'admintest123', 'admintest123', 0),
(5029862, '123', '4321a', '2018-04-21 08:38:33', '2018-04-21 15:15:33', '', 'admintest123', 1),
(5146621, 'dd', '555', '2018-04-19 13:34:53', '2018-04-19 13:35:27', 'admintest123', 'admintest123', 1),
(5318315, 'post33', 'qqqq3', '2018-04-19 13:32:40', '2018-04-23 11:14:59', 'admintest123', 'admintest123', 1),
(5421344, '12', '12', '2018-04-21 08:10:26', '2018-04-21 15:26:31', 'admintest123', 'admintest123', 1),
(6030033, 'Test post', '123asas', '2018-04-21 07:46:53', '2018-04-21 15:39:34', 'admintest123', 'admintest123', 1),
(6059320, 'adasd', 'dasdad', '2018-04-20 16:07:09', '2018-04-21 12:31:35', 'admintest123', 'admintest123', 1),
(6566261, 'test', 'asdasdasdasd', '2018-04-12 12:30:05', '2018-04-23 13:09:53', 'admintest123', 'admintest123', 0),
(7186645, 'post3', 'qqqq', '2018-04-19 13:30:45', NULL, NULL, 'admintest123', 0),
(7199499, 'ccc', 'cqcqcqc', '2018-04-19 14:27:27', '2018-04-19 14:27:35', 'admintest123', 'admintest123', 1),
(7598837, '123', 'Posting from app', '2018-04-21 07:51:32', '2018-04-21 15:33:44', 'admintest123', 'admintest123', 1),
(7793467, 'kansei dorifuto', '*eurobeat intensifies*', '2018-04-23 11:05:04', '2018-04-25 11:42:02', 'admintest123', 'admintest123', 1),
(8813427, 'Test', '123', '2018-04-20 14:27:59', '2018-04-23 13:51:20', 'admintest123', 'admintest123', 1),
(9109473, 'qqqqq', 'qqq', '2018-04-22 17:55:27', '2018-04-25 16:36:26', 'admintest123', 'admintest123', 1),
(9415322, 'Adding new announcement', '1234 new new', '2018-04-23 11:48:12', '2018-04-23 14:33:25', 'admintest123', 'admintest123', 1);

-- --------------------------------------------------------

--
-- Table structure for table `custom_search`
--

CREATE TABLE `custom_search` (
  `searchID` int(7) NOT NULL,
  `tenderReference` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `tenderTitle` varchar(300) COLLATE utf8_unicode_ci DEFAULT NULL,
  `originatingStation` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `closingDateFrom` datetime DEFAULT NULL,
  `closingDateTo` datetime DEFAULT NULL,
  `biddingclosingDateFrom` datetime DEFAULT NULL,
  `biddingclosingDateTo` datetime DEFAULT NULL,
  `username` varchar(15) COLLATE utf8_unicode_ci DEFAULT NULL,
  `identifier` varchar(100) COLLATE utf8_unicode_ci NOT NULL,
  `savedDate` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `custom_search`
--

INSERT INTO `custom_search` (`searchID`, `tenderReference`, `tenderTitle`, `originatingStation`, `closingDateFrom`, `closingDateTo`, `biddingclosingDateFrom`, `biddingclosingDateTo`, `username`, `identifier`, `savedDate`) VALUES
(560585419, '12345', 'doge', 'NULL', NULL, NULL, NULL, NULL, 'dinosaur96', 'kelsodp', '2018-04-25 14:21:34'),
(840954814, 'ehey', 'ehehey', 'Miri Power Station', '2018-04-20 00:00:00', '2018-04-02 00:00:00', NULL, NULL, 'dinosaur96', 'ehey', '2018-04-25 12:39:04'),
(1684378828, 'testing123', 'testing123', 'Head Office', '2018-04-26 00:00:00', '2018-04-30 00:00:00', '2018-04-26 00:00:00', '2018-04-30 00:00:00', 'dinosaur96', 'testing123', '2018-04-25 16:10:32');

-- --------------------------------------------------------

--
-- Table structure for table `tender_bookmark`
--

CREATE TABLE `tender_bookmark` (
  `bookmarkID` int(11) NOT NULL,
  `username` varchar(15) NOT NULL,
  `tenderReferenceNumber` varchar(100) NOT NULL,
  `tenderTitle` text NOT NULL,
  `isAvailable` tinyint(1) NOT NULL,
  `bookmarkDate` date NOT NULL,
  `closingDate` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tender_bookmark`
--

INSERT INTO `tender_bookmark` (`bookmarkID`, `username`, `tenderReferenceNumber`, `tenderTitle`, `isAvailable`, `bookmarkDate`, `closingDate`) VALUES
(3, 'test', '123', '321', 1, '2018-04-22', '2018-05-30 00:00:00'),
(47, 'dinosaur96', 'PLS-120058-KSL-CSR', 'RENOVATION WORKS AND RENTAL OF SHOPLOT (1ST AND 2ND FLOOR) IN KAPIT TOWN FOR SEB\'S CSR OFFICE', 1, '2018-04-27', '2018-05-02 15:00:00'),
(48, 'dinosaur96', 'TLO1/18', 'SUPPLY &amp; DELIVERY OF PERSONAL PROTECTIVE EQUIPMENT FOR TRANSMISSION LINES LINESMEN', 1, '2018-04-27', '2018-05-16 15:00:00'),
(49, 'dinosaur96', 'PMS136/17A', 'Tender for installation of additional VAV and recommissioning of chilled water system and air distribution system at Menara Sarawak Energy', 1, '2018-04-27', '2018-05-09 15:00:00');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `administrator`
--
ALTER TABLE `administrator`
  ADD PRIMARY KEY (`administratorID`);

--
-- Indexes for table `announcement`
--
ALTER TABLE `announcement`
  ADD PRIMARY KEY (`announcementID`);

--
-- Indexes for table `custom_search`
--
ALTER TABLE `custom_search`
  ADD PRIMARY KEY (`searchID`);

--
-- Indexes for table `tender_bookmark`
--
ALTER TABLE `tender_bookmark`
  ADD PRIMARY KEY (`bookmarkID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `administrator`
--
ALTER TABLE `administrator`
  MODIFY `administratorID` int(10) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `tender_bookmark`
--
ALTER TABLE `tender_bookmark`
  MODIFY `bookmarkID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=50;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
