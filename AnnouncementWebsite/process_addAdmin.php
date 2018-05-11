<?php
if (isset($_SESSION["user_login"]) && isset($_SESSION["user_role"])) {
    if ($_SESSION["user_role"] != "admin") {
        header("location: index.php");
        exit();
    }
    
    $nameError = "";
    $emailError = "";
    $usernameError = "";
    $passwordError = "";
    $confPasswordError = "";
    $errorpresence = false;
    $name = $email = $username = $password = $confPassword = $role = "";
    $success_message = "";
    $error_message = "";
    
    if(!empty($_POST["registerAdminBtn"])) {
        //Inputs validation
        if(empty($_POST["name"])){
            $nameError = "Name must not be empty";
            $errorpresence = true;
        }else{
            if (!preg_match("/^[a-zA-Z ]*$/",$_POST["name"])) {
                $nameError = "Name must be alphabetic";
                $errorpresence = true;
            }else{
                $name = sanitizeInput($_POST["name"]);
            }
            
        }
        
        if(empty($_POST["email"])){
            $emailError = "Email must not be empty";
            $errorpresence = true;
        }else{
            $email = sanitizeInput($_POST["email"]);
            if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
                $emailError = "Invalid email format.";
                $errorpresence = true;
            }
        }
        
        if(empty($_POST["username"])){
            $usernameError = "Username must not be empty";
            $errorpresence = true;
        }else{
            if (!preg_match("/^[a-zA-Z0-9]*$/",$_POST["username"])) {
                $usernameError = "Only alphanumeric is allowed for username";
                $errorpresence = true;
            }else{
                $username = sanitizeInput($_POST["username"]);
            }           
        }
        
        if(empty($_POST["password"])){
            $passwordError = "Password must not be empty";
            $errorpresence = true;
        }
        
        if($_POST["password"] != $_POST["confPassword"]){ 
            $confPasswordError = "Confirm Password does not match"; 
            $errorpresence = true;
        }else{
            $password = password_hash(sanitizeInput($_POST["password"]), PASSWORD_DEFAULT);
        }
            
        if(empty($_POST["confPassword"])){
            $confPasswordError = "Confirm Password must not be empty";
            $errorpresence = true;
        }
        
        if(empty($_POST["role"])) {
            $errorpresence = true;
        } else {
            $role = sanitizeInput($_POST["role"]);
        }
        
        //Register user if inputs are valid
        if (!$errorpresence) {
            require_once("dbcontroller.php");
            $db_handle = new DBController();
            
            //validate uniqueness of username
            $checkUsernameQuery = $db_handle->getConn()->prepare("SELECT username FROM administrator WHERE username= :username AND isDeleted = false");
            $checkUsernameQuery->bindParam(":username", $username);
            $checkUsernameQuery->execute();
            $checkUsernameResult = count($checkUsernameQuery->fetchAll());
            
            if($checkUsernameResult > 0){
                $usernameError = "Username already exists";
            } else {
                //if inputs are valid and username is unique, register user into database
                $query = $db_handle->getConn()->prepare("INSERT INTO administrator (administratorID, username, password, administratorName, administratorEmail, role) VALUES (NULL, :username, :password, :adminname, :adminemail, :role)");
    
                $query->bindParam(":username", $username);
                $query->bindParam(":password", $password);
                $query->bindParam(":adminname", $name);
                $query->bindParam(":adminemail", $email);
                $query->bindParam(":role", $role);
                
                $result = $query->execute();
                
                if ($result == true) {
                    $error_message = ""; 
                    $success_message = "Account successfully registered!";
                    unset($_POST);
                    header("refresh:3; url=login.php");
                } else {
                    $error_message = "Error occured while registering. Please try again!";
                }
            }
            
        } else {
            $error_message = "Invalid input(s)! Please ensure the input(s) entered are valid!";
        }
    }
} else {
    header("location: login.php");
    exit();
}


?>