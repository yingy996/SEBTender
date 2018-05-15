<?php
$oldPassword = $newPassword = "";
$errorpresence = false;

if (isset($_POST["adminUsername"]) && isset($_POST["adminPassword"])) {
    require_once("dbcontroller.php");
    $errorMessage = "";
    
    try {
        $adminUsername = $_POST["adminUsername"];
        $adminPassword = $_POST["adminPassword"];
        
        $db_handle = new DBController();
        /* Get number of rows where user input username matches with those in database */
        $query = $db_handle->getConn()->prepare("SELECT username, password FROM administrator WHERE username = :username");

        $query->bindParam(":username", $adminUsername);
        $query->execute();
        $result = $query->fetchAll();
        $total = count($result);
        
        if ($total > 0) {
            //Validate user password
            if(password_verify($adminPassword, $result[0]["password"])){
                //if user authentication succeeded, validate the inputs              
                if(empty($_POST["oldPassword"])){
                    $errorMessage .= "Old Password must not be empty. ";
                    $errorpresence = true;
                } else {
                    $oldPassword = $_POST["oldPassword"];
                }

                if(empty($_POST["newPassword"])){
                    $errorMessage .= "New Password must not be empty. ";
                    $errorpresence = true;
                }

                if($_POST["newPassword"] != $_POST["confPassword"]){ 
                    $errorMessage .= "Confirm Password does not match. "; 
                    $errorpresence = true;
                } else {
                    $newPassword = password_hash(sanitizeInput($_POST["newPassword"]), PASSWORD_DEFAULT);
                }

                if(empty($_POST["confPassword"])){
                    $errorMessage .= "Confirm Password must not be empty. ";
                    $errorpresence = true;
                }
                                
                //Validate if old password is correct
                $db_handle = new DBController();
                $checkPasswordQuery = $db_handle->getConn()->prepare("SELECT password FROM administrator WHERE username = :username");
                $checkPasswordQuery->bindParam(":username", $adminUsername);

                $checkPasswordQuery->execute();
                $checkResult = $checkPasswordQuery->fetchColumn();
                
                if(!password_verify($oldPassword, $checkResult)){
                    $errorMessage .= "Old password is incorrect";
                    $errorpresence = true;
                }
                
                if (!$errorpresence) {
                    //if no error found, update the password
                    $updateQuery = $db_handle->getConn()->prepare("UPDATE administrator SET password = :password WHERE username = :username");
                    $updateQuery->bindParam(":username", $adminUsername);
                    $updateQuery->bindParam(":password", $newPassword);
                    $updateResult = $updateQuery->execute();

                    if ($updateResult) {
                        echo "Successfully changed password!";
                    } else {
                        echo "Error occured while trying to change password. Please try again!";
                    }
                } else {
                    echo $errorMessage;
                }
            } else {
                echo "Invalid password"; //login failed
            }
        } else {
            echo "User not found!";
        }
    } catch (PDOException $e) {
        echo "Error: " . $e->getMessage();
    }      
}

function sanitizeInput($data) {
    $data = trim($data);
    $data = stripslashes($data);
    $data = htmlspecialchars($data);

    return $data;
}
?>