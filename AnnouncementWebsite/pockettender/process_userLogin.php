<?php
require_once("../dbcontroller.php");
$errorMsg = "";
$resultMsg = "";

if (isset($_SESSION["user_login"]) || isset($_SESSION["normaluser_login"])) {
    header("location: index.php");
}

if(isset($_POST["username"]) && isset($_POST["password"])){
    try {
        $db_handle = new DBController();
        
        $username = sanitizeInput($_POST["username"]);
        /* Get number of rows where user input username matches with those in database */
        $query = $db_handle->getConn()->prepare("SELECT username, password FROM user WHERE username = :username");

        $query->bindParam(":username", $username);
        $query->execute();
        $result = $query->fetchAll();
        $total = count($result);
        if($total > 0) {
            /* Get the password hash of the username inputed by user */
            //plain text user input password
            $password = $_POST["password"];
            
            if (password_verify($password, $result[0]["password"])) {
               
                if (session_status() == PHP_SESSION_NONE) {
                    @session_start();
                }
                $_SESSION["normaluser_login"] = $username;
                $resultMsg = "Login successfully! You will be redirected soon.";
                header("refresh:2;url=index.php");
            } else {
                $errorMsg =  "Invalid password. Please try again!";
            }
        } else {
            $errorMsg =  "User not found. Please try again!";
        }     
    } catch (PDOException $e) {
        $errorMsg =  "Error: " . $e->getMessage();
    }

}

?>