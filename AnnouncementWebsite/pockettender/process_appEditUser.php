<?php
require_once("../dbcontroller.php");
$errorpresence = false;
$error_message = "";
$name = $email = $role = $username = "";

if(isset($_POST["adminUsername"]) && isset($_POST["adminPassword"])){
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
            if(password_verify($adminPassword, $result[0]["password"])){
                //if user authentication succeeded, validate the inputs
                if(isset($_POST["name"]) && isset($_POST["email"]) && isset($_POST["role"]) && isset($_POST["username"])) {
                    //Inputs validation
                    if(empty($_POST["name"])){
                        $error_message .= "Name must not be empty";
                        $errorpresence = true;
                    }else{
                        if (!preg_match("/^[a-zA-Z ]*$/",$_POST["name"])) {
                            $error_message .= "Name must be alphabetic";
                            $errorpresence = true;
                        }else{
                            $name = sanitizeInput($_POST["name"]);
                        }            
                    }

                    if(empty($_POST["email"])){
                        $error_message .= "Email must not be empty";
                        $errorpresence = true;
                    }else{
                        $email = sanitizeInput($_POST["email"]);
                        if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
                            $error_message .= "Invalid email format.";
                            $errorpresence = true;
                        }
                    }

                    if(empty($_POST["role"])) {
                        $errorpresence = true;
                    } else {
                        $role = sanitizeInput($_POST["role"]);
                    }

                    if(empty($_POST["username"])){
                        $error_message .= "Username must not be empty";
                        $errorpresence = true;
                    } else {
                        $username = sanitizeInput($_POST["username"]);
                    }

                    //If no error presents, update the information
                    if(!$errorpresence) {
                        $db_handle = new DBController();

                        $updateQuery = $db_handle->getConn()->prepare("UPDATE administrator SET administratorName = :name, administratorEmail = :email, role = :role WHERE username = :username");
                        $updateQuery->bindParam(":username", $username);
                        $updateQuery->bindParam(":name", $name);
                        $updateQuery->bindParam(":email", $email);
                        $updateQuery->bindParam(":role", $role);

                        $updateResult = $updateQuery->execute();

                        if ($updateResult == true) {
                            echo "Successfully updated user information!";
                        } else {
                            echo "Error while updating user information. Please try again.";
                        }
                    } else {
                        echo $error_message;
                    }
                } else {
                    echo "Invalid parameters";
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