<?php
require_once("dbcontroller.php");
$db_handle = new DBController();

if (isset($_POST["deleteButton"])) {
    echo "first if";
    if (isset($_POST["username"])) {
        echo "second if" . $_POST["username"];
        $username = $_POST["username"];
        
        
        //Ensure admin is logged in
        session_start();
        
        if (isset($_SESSION["user_login"]) && isset($_SESSION["user_role"])) {
            if ($_SESSION["user_role"] == "admin") {
                $query = $db_handle->getConn()->prepare("UPDATE administrator SET isDeleted = true WHERE username = :username");
                $query->bindParam(":username", $username);
                $result = $query->execute();
                
                if ($result == true) {
                    echo "<script type='text/javascript'>alert('User has been successfully deleted.');</script>";
                    header("refresh:0, manage_users.php");
                } else {
                    echo "<script type='text/javascript'>alert('Error occured while trying to delete user. Please try again.');</script>";
                    header("refresh:0, manage_users.php");
                }
            }               
        }
    }
}
?>