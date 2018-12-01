<?php
require_once("../dbcontroller.php");
$pollQuestion = $pollOptionNumber = "";
$pollQuestion_Error = $pollOptionNumber_Error = "";
$errorpresence = false;
$error_message = "";
$pollOptions = array();
$optionErrors = array();
$resultMsg = $errorMsg = ""; //login message

if(isset($_POST["username"]) && isset($_POST["password"])){
    //Authentication for admin login 
    try{
        $db_handle = new DBController();
        /* Get number of rows where user input username matches with those in database */
        $query = $db_handle->getConn()->prepare("SELECT administratorID FROM administrator WHERE username = :username AND isDeleted = false");

        $query->bindParam(":username", $username);

        $username = sanitizeInput($_POST["username"]);
        $query->execute();
        $result = $query->fetchAll();
        $total = count($result);

        if($total > 0) {
            /* Get the password hash of the username inputed by user */
            $query2 = $db_handle->getConn()->prepare("SELECT password FROM administrator WHERE username = :username");

            $query2->bindParam(":username", $username);

            $query2->execute();
            $result2 = $query2->fetchColumn();

            //plain text user input password
            $password = $_POST["password"];

            //takes plain text password and hashed string password from database user_account table as arguments, return success message when admin account verified
            if(password_verify($password, $result2)){
                //if authentication success
                $resultMsg = "Login success";
                
                //Inputs validation
                if(empty($_POST["question"])){
                    $pollQuestion_Error = "Please enter poll question";
                    $errorpresence = true;
                } else {
                    $pollQuestion = sanitizeInput($_POST["question"]);
                }
                
                if(empty($_POST["option_number"])){
                    $pollOptionNumber_Error = "Please enter number of option";
                    $errorpresence = true;
                } else {
                    $pollOptionNumber = sanitizeInput($_POST["option_number"]);
                }
                
                //Poll options validation
                foreach ($_POST as $name => $value) {
                    if ($name != "question" && $name != "option_number" && $name != "username" && $name != "password") {
                        if (empty($value)) {
                            $optionErrors[$name] = "Option must not be empty";
                            $errorpresence = true;
                            $error_message = "Option field(s) must not be empty";
                        } else {
                            $pollOptions[$name] = $value;
                        }
                    }
                }
                
                if($error_message == "" && $errorpresence == false) {
                    /* Insert information into database */
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
                    $insertPollQuery->bindParam(":login_user", $username); 
                    $insertPollResult = $insertPollQuery->execute();
                    
                    if($insertPollResult == true) {
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

                        echo "You have succesfully published a poll!";	
                    } else {
                        echo "Problem in publishing poll. Please try again!";	
                    }
                } else {
                    echo "Failed to publish poll. Please ensure all the fields are entered.";
                }
            } else {
                echo "Invalid password"; //login failed
            }

        } else {
            echo "Invalid username"; //login failed
        }  
    } catch (PDOException $e) {
        echo "Error: " . $e->getMessage();
    }
} else {
    echo "Please login to create a poll";
}

function sanitizeInput($data) {
    $data = trim($data);
    $data = stripslashes($data);
    $data = htmlspecialchars($data);

    return $data;
}
?>