<?php 
require_once("dbcontroller.php");
$db_handle = new DBController();

// Get survey title based on surveyID
$query = $db_handle->getConn()->prepare("SELECT surveyTitle FROM survey WHERE surveyID = :surveyID");
$query->bindParam(":surveyID", $_GET['surveyID']);
$query->execute();
$survey = $query->fetchAll(); 

// Get respondent's userID 
$query = $db_handle->getConn()->prepare("SELECT * FROM survey_response WHERE surveyID = :surveyID ORDER BY dateSubmitted ASC");
$query->bindParam(":surveyID", $_GET['surveyID']);
$query->execute();
$user = $query->fetchAll(); 

// Get list of questions based on surveyID
$query = $db_handle->getConn()->prepare("SELECT * FROM survey_question WHERE surveyID = :surveyID ORDER BY questionNumber ASC");
$query->bindParam(":surveyID", $_GET['surveyID']);
$query->execute();
$questions = $query->fetchAll(); 

// Get list of response answers based on userID (TEXT)
$query = $db_handle->getConn()->prepare("SELECT survey_response_answer.text_answer as text_answer, survey_response_answer.answerID as answerID FROM survey_response_answer JOIN survey_response ON survey_response_answer.responseID = survey_response.responseID WHERE survey_response.userID = :userID ORDER BY survey_response_answer.text_answer");
$query->bindParam(":userID", $_GET['userID']);
$query->execute();
$responses = $query->fetchAll(); 

// Get list of response answers based on userID (SELECTION)
$query = $db_handle->getConn()->prepare("SELECT survey_questionanswer.answerTitle as answerTitle FROM survey_response_answer JOIN survey_response ON survey_response_answer.responseID = survey_response.responseID JOIN survey_questionanswer ON survey_response_answer.answerID = survey_questionanswer.answerID WHERE survey_response.userID = :userID");
$query->bindParam(":userID", $_GET['userID']);
$query->execute();
$selection = $query->fetchAll(); 

?>
