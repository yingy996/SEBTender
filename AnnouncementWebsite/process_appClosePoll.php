<?php
require_once("dbcontroller.php");
$errorpresence = false;
$error_message = "";
$resultMsg = "";

if(isset($_POST["username"]) && isset($_POST["password"])) {
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
                if(empty($_POST["pollID"])) {
                    $errorpresence = true;
                    $error_message = "Poll ID not present";
                } else {
                    $pollID = sanitizeInput($_POST["pollID"]);
                }
                
                if ($error_message == "" && $errorpresence == false) {
                    $query = $db_handle->getConn()->prepare("SELECT * FROM poll_question WHERE pollID = :pollID");
                    $query->bindParam(":pollID", $pollID);
                    $query->execute();
                    $pollResult = $query->fetchAll();

                    if ($pollResult != null) {
                        //Close the selected poll
                        $updateQuery = $db_handle->getConn()->prepare("UPDATE poll_question SET isEnded = 1, editedDate = NOW(), editedBy = :login_user, endDate = NOW() WHERE pollID = :pollID");
                        $updateQuery->bindParam(":pollID", $pollID);
                        $updateQuery->bindParam(":login_user", $username);

                        $result = $updateQuery->execute();

                        if ($result) {
                            $resultMsg = "Successfully closed poll!";
                        } else {
                            $resultMsg = "Error occured while trying to close the poll. Please try again!";
                        }
                    } else {
                        $resultMsg = "Poll not found! Please try again.";
                    }
                    echo $resultMsg;
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
    echo "Please login to close poll";
}

function sanitizeInput($data) {
    $data = trim($data);
    $data = stripslashes($data);
    $data = htmlspecialchars($data);

    return $data;
}
?>