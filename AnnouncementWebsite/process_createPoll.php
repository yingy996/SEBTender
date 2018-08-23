<?php
if (isset($_SESSION["user_login"])) {
    $pollQuestion = $pollOptionNumber = "";
    $pollQuestion_Error = $pollOptionNumber_Error = "";
    $errorpresence = false;
    $error_message = "";
    $pollOptions = array();
    $optionErrors = array();
      
    if(!empty($_POST["question"])){
        $pollQuestion = sanitizeInput($_POST["question"]);
    }

    if(!empty($_POST["option_number"])){
        $pollOptionNumber = sanitizeInput($_POST["option_number"]);
    }

    //Poll options validation
    foreach ($_POST as $name => $value) {
        if ($name != "question" && $name != "option_number" && $name != "publishPollButton") {
            $pollOptions[$name] = $value;
        }
    }
    
    if(!empty($_POST["publishPollButton"])) {
        //Poll question input validation
        if(empty($_POST["question"])){
            $pollQuestion_Error = "Please enter poll question";
            $errorpresence = true;
        }
        
        if(empty($_POST["option_number"])){
            $pollOptionNumber_Error = "Please enter number of option";
            $errorpresence = true;
        }
        
        //Poll options validation
        foreach ($_POST as $name => $value) {
            if ($name != "question" && $name != "option_number" && $name != "publishPollButton") {
                if (empty($value)) {
                    $optionErrors[$name] = "Option must not be empty";
                    $errorpresence = true;
                    $error_message = "Option field(s) must not be empty";
                }
            }
        }
        
        if($error_message == "" && $errorpresence == false) {
            //If inputs are valid, save the poll details into the database and publish the poll
            require_once("dbcontroller.php");
            $db_handle = new DBController();
            
            //Generate Unique ID for poll question
            $digits = 7;
            $randomPollID = rand(pow(10, $digits-1), pow(10, $digits)-1);
            $isPollIDUnique = false;
            
            do {
               $selectQuery = $db_handle->getConn()->prepare("SELECT pollID FROM poll_question WHERE pollID = :pollID");
               $selectQuery->bindParam(":pollID", $randomPollID);
               $selectQuery->execute();
               $selectResult = $selectQuery->fetchAll();
               $total = count($selectResult);
               if($total == 0) {
                   $isPollIDUnique = true;
               } else {
                   $randomPollID = rand(pow(10, $digits-1), pow(10, $digits)-1);
               }
            } while(!$isPollIDUnique);

            $insertPollQuery = $db_handle->getConn()->prepare("INSERT INTO poll_question (pollID, pollQuestion, postedBy, publishedDate, editedDate, editedBy, endDate, isEnded) VALUES
            (:pollID, :pollQuestion, :login_user, NOW(), NULL, NULL, NULL, 0)");
            
            $insertPollQuery->bindParam(":pollID", $randomPollID);
            $insertPollQuery->bindParam(":pollQuestion", $pollQuestion);
            $insertPollQuery->bindParam(":login_user", $login_user); 
            $insertPollResult = $insertPollQuery->execute();
            
            if($insertPollResult == true) {
                /*require_once("process_sendNotification.php");
                sendNotification("Poll: " . $pollQuestion);
                $error_message = "";
                $success_message = "You have succesfully published a poll!";*/
                
                //Insert poll options into the database
                foreach($pollOptions as $name => $value) {
                    $isInsertSuccess = false;
                    do {
                        //Generate Unique ID for poll question
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
                        $insertOptionQuery->bindParam(":optionTitle", $value);
                        $insertOptionQuery->bindParam(":pollID", $randomPollID); 
                        $insertOptionResult = $insertOptionQuery->execute();
                        
                        $isInsertSuccess = $insertOptionResult;
                    } while (!$isInsertSuccess);     
                }
                
                $error_message = "";
                $success_message = "You have succesfully published a poll!";	
                unset($_POST);
                header("refresh:2; url=index.php");
            } else {
                $error_message = "Problem in publishing poll. Please try again!";	
            }
        } else {
            $error_message = "Failed to submit: " . $error_message;
        }
    } 
} else {
    header("location: login.php");
    exit();
}
?>