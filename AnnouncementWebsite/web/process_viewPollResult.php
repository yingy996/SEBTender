<?php 
require_once("../dbcontroller.php");
$db_handle = new DBController();

// Get poll question based on pollID
$query = $db_handle->getConn()->prepare("SELECT pollQuestion FROM poll_question WHERE pollID = :pollID");
$query->bindParam(":pollID", $_GET['pollID']);
$query->execute();
$question = $query->fetchAll(); 

// Get list of poll options based on pollID
$query = $db_handle->getConn()->prepare("SELECT optionTitle, optionID FROM poll_option WHERE pollID = :pollID ORDER BY optionTitle ASC");
$query->bindParam(":pollID", $_GET['pollID']);
$query->execute();
$options = $query->fetchAll(); 

// Get list of poll response based on pollID
$query = $db_handle->getConn()->prepare("SELECT poll_votes_history.optionID, poll_option.optionTitle AS option FROM poll_votes_history JOIN poll_option ON poll_votes_history.pollID = poll_option.pollID AND poll_votes_history.optionID = poll_option.optionID WHERE poll_votes_history.pollID = :pollID ORDER BY poll_option.optionTitle ASC");
$query->bindParam(":pollID", $_GET['pollID']);
$query->execute();
$response_answers = $query->fetchAll(); 
$total_response = count($response_answers);

?>
