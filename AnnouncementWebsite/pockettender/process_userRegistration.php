<?php
require_once("../dbcontroller.php");

$errorMessage = "";
$errorpresence = false;
$resultMsg = "";
$name = $email = $username = $password = "";

if(isset($_POST["username"]) && isset($_POST["password"]) && isset($_POST["confirmpassword"]) && isset($_POST["fullname"]) && isset($_POST["email"])){
    //Validate inputs
    if (empty($_POST["fullname"])) {
        $errorMessage .= "Full name must not be empty. ";
        $errorpresence = true;
    } else {
        if (!preg_match("/^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$/", $_POST["fullname"])) {
            $errorMessage .= "Wrong format for name. e.g. Anthony Tan";
            $errorpresence = true;
        } else {
            $name = sanitizeInput($_POST["fullname"]);
        }
    }

    if(empty($_POST["email"])){
        $errorMessage .= "Email must not be empty. ";
        $errorpresence = true;
    }else{
        $email = sanitizeInput($_POST["email"]);
        if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
            $errorMessage .= "Invalid email format. ";
            $errorpresence = true;
        }
    }

    if(empty($_POST["username"])){
        $errorMessage .= "Username must not be empty. ";
        $errorpresence = true;
    }else{
        if (!preg_match("/^[a-zA-Z0-9]*$/",$_POST["username"])) {
            $errorMessage .= "Only alphanumeric is allowed for username. ";
            $errorpresence = true;
        }else{
            $username = sanitizeInput($_POST["username"]);
        }           
    }

    if(empty($_POST["password"])){
        $errorMessage .= "Password must not be empty. ";
        $errorpresence = true;
    } else {
        if (!preg_match("/^[a-zA-Z0-9]*$/",$_POST["password"])) {
            $errorMessage .= "Only alphanumeric is allowed for password. ";
            $errorpresence = true;
        }
    }

    if(empty($_POST["confirmpassword"])){
        $errorMessage .= "Confirm Password must not be empty. ";
        $errorpresence = true;
    }

    if($_POST["password"] != $_POST["confirmpassword"]){ 
        $errorMessage .= "Confirm Password does not match. "; 
        $errorpresence = true;
    }else{
        $password = password_hash(sanitizeInput($_POST["password"]), PASSWORD_DEFAULT);
    }


    //Register user if inputs are valid
    if (!$errorpresence) {
        $db_handle = new DBController();
        //validate uniqueness of username
        $checkUsernameQuery = $db_handle->getConn()->prepare("SELECT username FROM user WHERE username= :username");
        $checkUsernameQuery->bindParam(":username", $username);
        $checkUsernameQuery->execute();
        $checkUsernameResult = count($checkUsernameQuery->fetchAll());

        if($checkUsernameResult > 0){
            $errorMessage .= "Username already exists";
            echo $errorMessage;
        } else {
            //if inputs are valid and username is unique, register user into database
            $insertQuery = $db_handle->getConn()->prepare("INSERT INTO user (username, password, email, fullName, dateSubmitted, registeredFrom) VALUES (:username, :password, :email, :name, NOW(), :fromWhere)");

            $insertQuery->bindParam(":username", $username);
            $insertQuery->bindParam(":password", $password);
            $insertQuery->bindParam(":name", $name);
            $insertQuery->bindParam(":email", $email);
            $fromWhere = "Web";
            $insertQuery->bindParam(":fromWhere", $fromWhere);

            $insertResult = $insertQuery->execute();

            if ($insertResult) {
                $resultMsg = "Registration successful";
            } else {
                $errorMessage = "Error occured while registering. Please try again! ";
            }
        }
    }
}
?>