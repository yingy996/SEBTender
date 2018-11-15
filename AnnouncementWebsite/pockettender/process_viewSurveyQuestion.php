<?php 
require_once("../dbcontroller.php");
$db_handle = new DBController();

$query = $db_handle->getConn()->prepare("SELECT surveyTitle FROM survey WHERE surveyID = :surveyID");
$query->bindParam(":surveyID", $_GET['surveyID']);
$query->execute();
$survey = $query->fetchAll(); 

$query = $db_handle->getConn()->prepare("SELECT * FROM survey_question WHERE surveyID = :surveyID ORDER BY questionNumber ASC");
$query->bindParam(":surveyID", $_GET['surveyID']);
$query->execute();
$questions = $query->fetchAll(); 

?>
