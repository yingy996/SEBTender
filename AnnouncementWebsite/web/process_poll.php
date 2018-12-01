<?php
require_once("../dbcontroller.php");
$pollID = $pollQuestion = "";
$error_message = "";
$optionResults = "";

//Obtain poll information from database
$db_handle = new DBController();
$query = $db_handle->getConn()->prepare("SELECT * FROM poll_question WHERE isEnded = 0");
$query->execute();

$result = $query->fetchAll();

//If there are poll present, obtain the poll details, else, display error message
if($result != null) {
    $poll = $result[0];
    $pollID = $poll["pollID"];
    $pollQuestion = $poll["pollQuestion"];
    $questionType = $poll["questionType"];
    $isOtherAllowed = $poll["isOtherAllowed"];
    
    //Obtain poll options from database
    $optionQuery = $db_handle->getConn()->prepare("SELECT * FROM poll_option WHERE pollID = :pollID ORDER BY optionTitle ASC");
    $optionQuery->bindParam(":pollID", $pollID);
    $optionQuery->execute();
    
    $optionResults = $optionQuery->fetchAll();
} else {
    $error_message = "There is no poll for now. ";
    if (isset($_SESSION["user_login"])) {
        $error_message .= "Create a new poll?";
    }
}
?>