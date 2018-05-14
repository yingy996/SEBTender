<?php
require_once("dbcontroller.php");
$db_handle = new DBController();

if (isset($_POST["deleteButton"])) {
    if (isset($_POST["username"])) {
        $username = sanitizeInput($_POST["username"]);
        
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
} elseif (isset($_POST["adminUsername"]) && isset($_POST["adminPassword"])) {
    //if request received from app, process the request
    try {
        $adminUsername = sanitizeInput($_POST["adminUsername"]);
        $adminPassword = sanitizeInput($_POST["adminPassword"]);
        
        $db_handle = new DBController();
        /* Get number of rows where user input username matches with those in database */
        $query = $db_handle->getConn()->prepare("SELECT username, password FROM administrator WHERE username = :username");

        $query->bindParam(":username", $adminUsername);
        $query->execute();
        $result = $query->fetchAll();
        $total = count($result);
        
        if ($total > 0) {
            if(password_verify($adminPassword, $result[0]["password"])){
                if (isset($_POST["username"])) {
                    $username = $_POST["username"];
                    
                    $updateQuery = $db_handle->getConn()->prepare("UPDATE administrator SET isDeleted = true WHERE username = :username");
                    $updateQuery->bindParam(":username", $username);
                    $updateResult = $updateQuery->execute();

                    if ($updateResult == true) {
                        echo "Successully deleted user";
                    } else {
                        echo "Failed to delete user";
                    }
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