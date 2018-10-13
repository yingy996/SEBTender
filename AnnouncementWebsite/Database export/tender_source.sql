-- phpMyAdmin SQL Dump
-- version 4.7.7
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Oct 13, 2018 at 05:21 AM
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
-- Table structure for table `tender_source`
--

CREATE TABLE `tender_source` (
  `id` tinyint(10) NOT NULL,
  `website` varchar(100) NOT NULL,
  `lastUpdate` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tender_source`
--

INSERT INTO `tender_source` (`id`, `website`, `lastUpdate`) VALUES
(0, 'sarawakenergy', '2018-10-10 17:37:14'),
(1, 'myprocurement', '2018-10-12 21:58:13'),
(2, 'telekom', '2018-10-12 21:58:13'),
(3, 'mbks', '2018-10-12 21:58:15');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `tender_source`
--
ALTER TABLE `tender_source`
  ADD PRIMARY KEY (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
