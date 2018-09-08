<?php
require_once("dbcontroller.php");
$errorpresence = false;
$error_message = "";
$resultMsg = "";
$pollOptionID = "";

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
                if(empty($_POST["optionID"])) {
                    $errorpresence = true;
                    $error_message = "Poll option ID not present";
                } else {
                    $pollOptionID = sanitizeInput($_POST["optionID"]);
                }
                
                if ($error_message == "" && $errorpresence == false) {
                    //Check if the option is present in database
                    $selectQuery = $db_handle->getConn()->prepare("SELECT optionID FROM poll_option WHERE optionID = :optionID");
                    $selectQuery->bindParam(":optionID", $pollOptionID);
                    $selectQuery->execute();
                    $selectResult = $selectQuery->fetchAll();
                    
                    if(count($selectResult) > 0) {
                        //Delete the option from database
                        $deleteQuery = $db_handle->getConn()->prepare("DELETE FROM poll_option WHERE optionID = :optionID");
                        $deleteQuery->bindParam(":optionID", $pollOptionID);
                        $deleteResult = $deleteQuery->execute();

                        if ($deleteResult) {
                            $resultMsg = "Poll option successfully deleted!";
                        } else {
                            $resultMsg = "Failed to delete option. Please try again!";
                        }
                    } else {
                        $resultMsg = "Poll option does not exist! Please try again.";
                    }
                    echo trim($resultMsg);
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
    echo "Please login to delete option";
}
function sanitizeInput($data) {
    $data = trim($data);
    $data = stripslashes($data);
    $data = htmlspecialchars($data);

    return $data;
}
?>