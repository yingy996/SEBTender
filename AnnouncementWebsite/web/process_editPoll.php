<?php
@ob_start();
require_once("../dbcontroller.php");
$pollQuestion = $pollOptionNumber = "";
$pollQuestion_Error = $pollOptionNumber_Error = "";
$pollOptionNumber = 0;
$errorpresence = false;
$error_message = "";
$pollOptions = array();
$optionErrors = array();
$isDeleteButtonClicked = false;
$optionToDelete = "";
$isOtherAllowed = 0;

if (!isset($_SESSION["user_login"])) {
    header("location: login.php");
}
if(!isset($_GET["edit_pollID"])) {
    header("location: login.php");
}

$pollID =  sanitizeInput($_GET["edit_pollID"]);

//Check if any of the delete button is clicked
foreach ($_POST as $name => $value) {
    if (substr($name, 0, 18) == "deleteOptionButton") {
        $isDeleteButtonClicked = true;
        $optionToDelete = sanitizeInput($value);
    }
}
//Get the information from the database when the user first load the page
if(empty($_POST["updatePollButton"]) && empty($_POST["addOptionButton"]) && $isDeleteButtonClicked == false) { 
   //Obtain poll information from database
    $db_handle = new DBController();
    $query = $db_handle->getConn()->prepare("SELECT * FROM poll_question WHERE pollID = :pollID");
    $query->bindParam(":pollID", $pollID);
    $query->execute();

    $result = $query->fetchAll();

    //If there are poll present, obtain the poll details, else, display error message
    if($result[0][0] != "") {
        $poll = $result[0];
        $pollID = $poll["pollID"];
        $pollQuestion = $poll["pollQuestion"];
        $isOtherAllowed = $poll["isOtherAllowed"];

        //Obtain poll options from database
        $optionQuery = $db_handle->getConn()->prepare("SELECT * FROM poll_option WHERE pollID = :pollID ORDER BY optionTitle ASC");
        $optionQuery->bindParam(":pollID", $pollID);
        $optionQuery->execute();

        $optionResults = $optionQuery->fetchAll();
        $pollOptionNumber = count($optionResults);
        if ($isOtherAllowed == 1) {
            $pollOptionNumber++;
        }
        
        $count = 1;
        foreach($optionResults as $option) {
            $pollOptions["option" . $count] = $option["optionTitle"];
            $pollOptions["optionID" . $count] = $option["optionID"];
            $count++;
        }
        //Generate input fields for options
        $_POST["option_number"] = $pollOptionNumber;
    } else {
        header("Location: index.php");
        /*$error_message = "There is no poll for now.";
        if (isset($_SESSION["user_login"])) {
            $error_message += "Create a new poll?";
        }*/
    } 
} else {
    //Get the information filled in the form when user clicked on any of the button in the page
    if(!empty($_POST["question"])){
        $pollQuestion = sanitizeInput($_POST["question"]);
    }

    if(!empty($_POST["option_number"])){
        $pollOptionNumber = sanitizeInput($_POST["option_number"]);
    }

    foreach ($_POST as $name => $value) {
        if ($name != "question" && $name != "option_number" && $name != "updatePollButton" && $name != "addOptionButton" && $name != "isOther" && $name != "other") {
            $pollOptions[$name] = sanitizeInput($value);
        }
    }
    
    if (!empty($_POST["updatePollButton"])) {
        //Poll question input validation
        if(empty($_POST["question"])){
            $pollQuestion_Error = "Please enter poll question";
            $errorpresence = true;
        }

        if(empty($_POST["option_number"])){
            $pollOptionNumber_Error = "Please enter number of option";
            $errorpresence = true;
        }

        $count = 0;
        //Poll options validation
        foreach ($_POST as $name => $value) {
            if ($name != "question" && $name != "option_number" && $name != "updatePollButton" && $name != "addOptionButton" && $name != "isOther" && $name != "other") {
                if(substr($name, 0, 8) != "optionID") {
                    if (empty($value)) {
                        $optionErrors[$name] = "Option must not be empty";
                        $errorpresence = true;
                        $error_message = "Option field(s) must not be empty";
                    }
                    $count++;
                }                
            }
        }
        if (!empty($_POST["isOther"]) || !empty($_POST["other"])) {
            $count++;
        }
        if($count != $pollOptionNumber) {
            $errorpresence = true;
            $error_message = "Option field(s) must not be empty";
        }

        //Check for error presence
        if($error_message == "" && $errorpresence == false) {
            //if no error present, update the poll details
            //$db_handle = new DBController();
            $updateQuery = $db_handle->getConn()->prepare("UPDATE poll_question SET pollQuestion = :pollQuestion, isOtherAllowed = :isOtherAllowed, editedDate = NOW(), editedBy = :login_user WHERE pollID = :pollID");
            $updateQuery->bindParam(":pollQuestion", $pollQuestion);
            $isOther = 0;
            if (!empty($_POST["other"]) || !empty($_POST["isOther"])) {
                $isOther = 1;
            } 
            $updateQuery->bindParam(":isOtherAllowed", $isOther);
            $updateQuery->bindParam(":login_user", $login_user);
            $updateQuery->bindParam(":pollID", $pollID);

            $updateResult = $updateQuery->execute();

            if ($updateResult) {        
                //if poll question successfully updated, proceed to update options
                for($i = 1; $i <= $pollOptionNumber; $i++) {
                    //Check if option is registered in database
                    $selectOptionQuery = $db_handle->getConn()->prepare("SELECT optionID FROM poll_option WHERE optionID = :optionID");
                    $selectOptionQuery->bindParam(":optionID", $pollOptions["optionID" . $i]);
                    $selectOptionQuery->execute();
                    $selectOptionResult = $selectOptionQuery->fetchAll();
                    $total = count($selectOptionResult);
                    if($total == 0) {
                        if ($i != $pollOptionNumber || ($i == $pollOptionNumber && empty($_POST["isOther"]) && empty($_POST["other"]))) {
                            //Insert the new option into the database
                            $isInsertSuccess = false;
                            do {
                               //Generate Unique ID for poll option
                                $digits = 7;
                                $randomOptionID = rand(pow(10, $digits-1), pow(10, $digits)-1);
                                $isOptionIDUnique = false;

                                do {
                                   $selectOptionQuery = $db_handle->getConn()->prepare("SELECT optionID FROM poll_option WHERE optionID = :optionID");
                                   $selectOptionQuery->bindParam(":optionID", $randomOptionID);
                                   $selectOptionQuery->execute();
                                   $selectOptionResult = $selectOptionQuery->fetchAll();
                                   $total = count($selectOptionResult);
                                   if($total == 0) {
                                       $isOptionIDUnique = true;
                                   } else {
                                       $randomOptionID = rand(pow(10, $digits-1), pow(10, $digits)-1);
                                   }
                                } while(!$isOptionIDUnique);

                                $insertOptionQuery = $db_handle->getConn()->prepare("INSERT INTO poll_option (optionID, optionTitle, pollID, votesCount) VALUES
                                (:optionID, :optionTitle, :pollID, 0)");

                                $insertOptionQuery->bindParam(":optionID", $randomOptionID);
                                $insertOptionQuery->bindParam(":optionTitle", $pollOptions["option" . $i]);
                                $insertOptionQuery->bindParam(":pollID", $pollID); 
                                $insertOptionResult = $insertOptionQuery->execute();

                                $isInsertSuccess = $insertOptionResult;
                            } while (!$isInsertSuccess);
                        }
                    } else {
                        //Update the option
                        $updateOptionQuery = $db_handle->getConn()->prepare("UPDATE poll_option SET optionTitle = :optionTitle WHERE optionID = :optionID");
                        $updateOptionQuery->bindParam(":optionTitle", $pollOptions["option" . $i]);
                        $updateOptionQuery->bindParam(":optionID", $pollOptions["optionID" . $i]);
                        $updateOptionResult = $updateOptionQuery->execute();
                        
                        if ($updateOptionResult) {
                            $success_message = "Successfully updated the poll!";
                            header("Refresh:1");
                        } else {
                            $error_message = "Error occured while updating the poll, Please try again!";
                        }
                    }   
                }
            } else {
                $error_message = "Failed to update poll. Please try again.";
            }
        } else {
            $error_message = "Failed to submit: " . $error_message;
        }
    }
    
    //When user click on 'Delete' button on option, delete the option from database
    //$_POST["deleteOptionButton"] is set above in foreach by getting the delete button clicked
    if($isDeleteButtonClicked == true && $optionToDelete != "") {
        if ($optionToDelete != "other"){
            //Check if the option is present in database
            $selectQuery = $db_handle->getConn()->prepare("SELECT optionID FROM poll_option WHERE optionID = :optionID");
            $selectQuery->bindParam(":optionID", $optionToDelete);
            $selectQuery->execute();
            $selectResult = $selectQuery->fetchAll();

            if(count($selectResult) > 0) {
                //Delete the option from database
                $deleteQuery = $db_handle->getConn()->prepare("DELETE FROM poll_option WHERE optionID = :optionID");
                $deleteQuery->bindParam(":optionID", $optionToDelete);
                $deleteResult = $deleteQuery->execute();

                if ($deleteResult) {
                    $success_message = "Poll option successfully deleted!";
                    header("Refresh:1");
                }
            } else {
                $error_message = "Poll option does not exist! Please try again.";
            }
        } else {
            $updateQuery = $db_handle->getConn()->prepare("UPDATE poll_question SET isOtherAllowed = 0, editedDate = NOW(), editedBy = :login_user WHERE pollID = :pollID");
            $updateQuery->bindParam(":login_user", $login_user);
            $updateQuery->bindParam(":pollID", $pollID);
            $updateResult = $updateQuery->execute();

            if ($updateResult) {
                $success_message = "Poll option successfully deleted!";
                header("Refresh:1");
            }
        }
    }
} 
?>