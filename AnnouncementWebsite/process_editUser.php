<?php
if (isset($_SESSION["user_login"]) && isset($_SESSION["user_role"])) {
    if ($_SESSION["user_role"] != "admin") {
        header("location: index.php");
        exit();
    }

    require_once("dbcontroller.php");
    $nameError = "";
    $emailError = "";
    $usernameError = "";
    $passwordError = "";
    $confPasswordError = "";
    $errorpresence = false;
    $name = $email = $username = $password = $confPassword = $role = "";
    $success_message = "";
    $error_message = "";
    $name = $email = $role = $username = "";

    //Display user info for edit
    if (isset($_POST["editButton"]) && isset($_POST["username"])) {
        $db_handle = new DBController();
        $username = $_POST["username"];

        //get user info
        $query = $db_handle->getConn()->prepare("SELECT * FROM administrator WHERE username = :username ORDER BY administratorName ASC");
        $query->bindParam(":username", $username);
        $query->execute();

        $results = $query->fetchAll();

        //Get the result to display it in input fields
        if($results[0][0] != ""){
            $user = $results[0];
            $name = $user["administratorName"];
            $email = $user["administratorEmail"];
            $role = $user["role"];
            $username = $user["username"];
        } else {
            $error_message = "User not found. Please try again.";
        }

    } else {
        header("manage_users.php");
    }
    
    //If user clicked "Update" button, validate the inputs and update the user info
    if (isset($_POST["editAdminBtn"])){
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
        
        if(empty($_POST["role"])) {
            $errorpresence = true;
        } else {
            $role = sanitizeInput($_POST["role"]);
        }
        
        if(empty($_POST["username"])){
            $usernameError = "Username must not be empty";
            $errorpresence = true;
        } else {
            $username = sanitizeInput($_POST["username"]);
        }
        
        //If no error presents, update the information
        if (!$errorpresence) {
            $db_handle = new DBController();
            
            $query = $db_handle->getConn()->prepare("UPDATE administrator SET administratorName = :name, administratorEmail = :email, role = :role WHERE username = :username");
            $query->bindParam(":username", $username);
            $query->bindParam(":name", $name);
            $query->bindParam(":email", $email);
            $query->bindParam(":role", $role);
            
            $result = $query->execute();

            if ($result == true) {
                $success_message = "Successfully updated user information!";
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