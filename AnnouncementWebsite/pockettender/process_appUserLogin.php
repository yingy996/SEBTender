<?php
require_once("../dbcontroller.php");

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
                echo "Login successful!";
            } else {
                echo "Invalid password. Please try again!";
            }
        } else {
            echo "User not found. Please try again!";
        }     
    } catch (PDOException $e) {
        echo "Error: " . $e->getMessage();
    }
} else {
    echo "Invalid parameters";
}

function sanitizeInput($data) {
    $data = trim($data);
    $data = stripslashes($data);
    $data = htmlspecialchars($data);

    return $data;
}
?>