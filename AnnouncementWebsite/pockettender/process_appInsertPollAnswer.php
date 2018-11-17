<?php
require_once("../dbcontroller.php");
$errorPresence = false;
$errorMessage = "";
$pollID = $optionID = $text_answer = $userID = "";
//Validates that all the required parameters are valid and not empty
if(empty($_POST["pollID"])) {
    $errorMessage .= "Poll ID must not be empty. ";
    $errorPresence = true;
} else {
    $pollID = sanitizeInput($_POST["pollID"]);
}

if(empty($_POST["optionID"])) {
    if(empty($_POST["answerInText"])) {
        $errorMessage .= "Option or answer input must not be empty. ";
        $errorPresence = true;
    } else {
        $text_answer = sanitizeInput($_POST["answerInText"]);
    }   
} else {
    $optionID = sanitizeInput($_POST["optionID"]);
}

if(empty($_POST["userID"])) {
    $errorMessage .= "Poll ID must not be empty. ";
    $errorPresence = true;
} else {
    $userID = sanitizeInput($_POST["userID"]);
}

if (!$errorPresence) {
    //If no error present for parameters, check for the existence of poll, option, and whether user has voted for the same poll before
    
    //Validate to ensure the poll ID and option ID exist in database
    //Validate poll ID
    $db_handle = new DBController();
    $selectQuery = $db_handle->getConn()->prepare("SELECT pollID FROM poll_question WHERE pollID = :pollID");
    $selectQuery->bindParam(":pollID", $pollID);
    $selectQuery->execute();
    $selectResult = $selectQuery->fetchAll();
    $total = count($selectResult);

    if($total == 0) {
        $errorPresence = true;
        $errorMessage = "Poll does not exist. Please try again!";
    } else {
        //Validate option ID if option ID is not empty
        if ($optionID != "") {
            $db_handle = new DBController();
            $selectQuery = $db_handle->getConn()->prepare("SELECT optionID FROM poll_option WHERE optionID = :optionID AND pollID = :pollID");
            $selectQuery->bindParam(":optionID", $optionID);
            $selectQuery->bindParam(":pollID", $pollID);
            $selectQuery->execute();
            $selectOptionResult = $selectQuery->fetchAll();
            $totalResult = count($selectOptionResult);

            if($totalResult == 0) {
                $errorPresence = true;
                $errorMessage = "Poll option does not exist. Please try again!";
            }
        }
    }

    //Check if user has voted before
    $db_handle = new DBController();
    $selectQuery = $db_handle->getConn()->prepare("SELECT userID FROM poll_votes_history WHERE userID = :userID AND pollID = :pollID");
    $selectQuery->bindParam(":userID", $userID);
    $selectQuery->bindParam(":pollID", $pollID);
    $selectQuery->execute();
    $selectResult = $selectQuery->fetchAll();
    $total = count($selectResult);

    if($total > 0) {
        $errorPresence = true;
        $errorMessage = "You have voted before. Thanks for participating!";
    }
}

if(!$errorPresence) {
    //If no error present, proceed to insert poll answer into the database
    //Generate unique vote ID
    $digits = 7;
    $randomVoteID = rand(pow(10, $digits-1), pow(10, $digits)-1);
    $isVoteIDUnique = false;
    
    //Check for Vote ID uniqueness  
    do {
       $selectQuery = $db_handle->getConn()->prepare("SELECT voteID FROM poll_votes_history WHERE voteID = :voteID");
       $selectQuery->bindParam(":voteID", $randomVoteID);
       $selectQuery->execute();
       $selectResult = $selectQuery->fetchAll();
       $total = count($selectResult);
        
       if($total == 0) {
           $isVoteIDUnique = true;
       } else {
           $randomVoteID = rand(pow(10, $digits-1), pow(10, $digits)-1);
       }
    } while(!$isVoteIDUnique);  
    
    //Insert information into database
    $insertAnswerQuery = $db_handle->getConn()->prepare("INSERT INTO poll_votes_history (voteID, pollID, optionID, text_answer, userID, dateVoted) VALUES
    (:voteID, :pollID, :optionID, :text_answer, :user, NOW())");

    $insertAnswerQuery->bindParam(":voteID", $randomVoteID);
    $insertAnswerQuery->bindParam(":pollID", $pollID);
    $insertAnswerQuery->bindParam(":optionID", $optionID);
    $insertAnswerQuery->bindParam(":text_answer", $text_answer);
    $insertAnswerQuery->bindParam(":user", $userID); 
    $insertAnswerResult = $insertAnswerQuery->execute();
    
    if($insertAnswerResult == true) {
        echo "Answer has been successfully submitted. Thank you for participating!";
    } else {
        echo "Failed to submit answer. Please try again!";
    }
} else {
    echo $errorMessage;
}

function sanitizeInput($data) {
    $data = trim($data);
    $data = stripslashes($data);
    $data = htmlspecialchars($data);

    return $data;
}
?>