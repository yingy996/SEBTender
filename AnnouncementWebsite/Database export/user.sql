-- phpMyAdmin SQL Dump
-- version 4.7.7
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Oct 13, 2018 at 09:39 AM
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
-- Table structure for table `user`
--

CREATE TABLE `user` (
  `username` varchar(15) NOT NULL,
  `password` text NOT NULL,
  `email` varchar(100) NOT NULL,
  `fullName` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `user`
--

INSERT INTO `user` (`username`, `password`, `email`, `fullName`) VALUES
('dinosaur96', '$2y$10$7mKdVtijsvhtzRZxr8Xt8eFjiMLWwjQrlyc75e.m58Bv07A0nsRzi', 'chiputi@chiputi.com', 'chiputi'),
('koala', '$2y$10$DKR4I.CGMDycNazJC012qONVk3JCiJ.JCMFTCHTPXu.VFRFflAoAS', 'joseph_sim19@hotmail.com', 'Joseph Sim'),
('sharon96', '$2y$10$ebW6/GEYZKXHypyPuHWgbegmlCDRLhdX8wu4RXTn6u0hbsT4xlLH6', 'sharon@email.com', 'Sharon Lo'),
('slo', '$2y$10$iDudrVX0z0jitBlVbjm3HutFDCey3gGd/sHKti2/8cePAMaKrWi3y', 'slo@yahoo.com', 'Sharon Lo');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`username`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
