<?php
require_once("dbcontroller.php");
$resultMsg = "";
$pollQuestion = $pollOptionNumber = "";
$pollQuestion_Error = $pollOptionNumber_Error = "";
$pollOptionNumber = 0;
$errorpresence = false;
$error_message = "";
$pollOptions = array();
$optionErrors = array();
$isDeleteButtonClicked = false;
$optionToDelete = "";
$pollID = "";

if(isset($_POST["username"]) && isset($_POST["password"])){
    try {
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
            $password = sanitizeInput($_POST["password"]);

            //takes plain text password and hashed string password from database user_account table as arguments, return success message when admin account verified
            if(password_verify($password, $result2)){
                //if authentication success, proceed to update the poll
                $resultMsg = "Login success";
                
                //inputs validation
                if(empty($_POST["pollID"])){
                    $error_message = "Poll ID not present";
                    $errorpresence = true;
                } else {
                    $pollID = sanitizeInput($_POST["pollID"]);
                }
                
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
                
                $count = 0;
                //Poll option validation
                foreach ($_POST as $name => $value) {
                    if ($name != "question" && $name != "option_number" && $name != "pollID" && $name != "username" && $name != "password") {
                        $pollOptions[$name] = sanitizeInput($value); 
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
                
                if($count != $pollOptionNumber) {
                    $errorpresence = true;
                    $error_message = "Option field(s) must not be empty";
                }
                
                //Check for error presence
                if($error_message == "" && $errorpresence == false) {
                    //If no error present, update the poll details
                    $updateQuery = $db_handle->getConn()->prepare("UPDATE poll_question SET pollQuestion = :pollQuestion, editedDate = NOW(), editedBy = :login_user WHERE pollID = :pollID");
                    $updateQuery->bindParam(":pollQuestion", $pollQuestion);
                    $updateQuery->bindParam(":login_user", $username);
                    $updateQuery->bindParam(":pollID", $pollID);

                    $updateResult = $updateQuery->execute();
                    
                    if($updateResult) {
                        //if poll question successfully updated, proceed to update options
                        for($i = 1; $i <= $pollOptionNumber; $i++) {
                            //Check if option is registered in database
                            $selectOptionQuery = $db_handle->getConn()->prepare("SELECT optionID FROM poll_option WHERE optionID = :optionID");
                            $selectOptionQuery->bindParam(":optionID", $pollOptions["optionID" . $i]);
                            $selectOptionQuery->execute();
                            $selectOptionResult = $selectOptionQuery->fetchAll();
                            $total = count($selectOptionResult);
                            if($total == 0) {
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
                            } else {
                                //Update the option
                                $updateOptionQuery = $db_handle->getConn()->prepare("UPDATE poll_option SET optionTitle = :optionTitle WHERE optionID = :optionID");
                                $updateOptionQuery->bindParam(":optionTitle", $pollOptions["option" . $i]);
                                $updateOptionQuery->bindParam(":optionID", $pollOptions["optionID" . $i]);
                                $updateOptionResult = $updateOptionQuery->execute();

                                if ($updateOptionResult) {
                                    $resultMsg = "Successfully updated the poll!";
                                } else {
                                    $resultMsg = "Error occured while updating the poll, Please try again!";
                                }
                            }
                        }
                        echo $resultMsg;
                    } else {
                        $resultMsg = "Failed to update poll. Please try again.";
                        echo $resultMsg;
                    }
                } else {
                    echo $error_message;
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
    echo "Please login to edit poll";
}

function sanitizeInput($data) {
    $data = trim($data);
    $data = stripslashes($data);
    $data = htmlspecialchars($data);

    return $data;
}
?>