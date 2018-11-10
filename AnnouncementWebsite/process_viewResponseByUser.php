<?php 
require_once("dbcontroller.php");
$db_handle = new DBController();

// Get survey title based on surveyID
$query = $db_handle->getConn()->prepare("SELECT surveyTitle FROM survey WHERE surveyID = :surveyID");
$query->bindParam(":surveyID", $_GET['surveyID']);
$query->execute();
$survey = $query->fetchAll(); 

// Get list of respondents' userID based on surveyID
$query = $db_handle->getConn()->prepare("SELECT * FROM survey_response WHERE surveyID = :surveyID ORDER BY dateSubmitted DESC");
$query->bindParam(":surveyID", $_GET['surveyID']);
$query->execute();
$users = $query->fetchAll(); 

?>
