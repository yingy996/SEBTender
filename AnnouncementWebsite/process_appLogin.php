<?php
require_once("dbcontroller.php");    
$errorMsg = "";

if(isset($_POST["username"]) && isset($_POST["password"])){
    try {
        $db_handle = new DBController();
        /* Get number of rows where user input username matches with those in database */
        $query = $db_handle->getConn()->prepare("SELECT administratorID FROM administrator WHERE username = :username");

        $query->bindParam(":username", $username);

        $username = sanitizeInput($_POST["username"]);
        $query->execute();
        $result = $query->fetchAll();
        $total = count($result);
        if($total > 0) {
            /* Get the password hash of the username inputed by user */
            $query2 = $db_handle->getConn()->prepare("Select password FROM administrator WHERE username = :username");

            $query2->bindParam(":username", $username);

            $query2->execute();
            $result2 = $query2->fetchColumn();

            //plain text user input password
            $password = $_POST["password"];
            
            //takes plain text password and hashed string password from database user_account table as arguments, log in if matches
            if(password_verify($password, $result2)){
                //if admin authentication succeeded, return message
                $roleQuery = $db_handle->getConn()->prepare("SELECT role FROM administrator WHERE username = :username");

                $roleQuery->bindParam(":username", $username);

                $roleQuery->execute();
                $roleResult = $roleQuery->fetchColumn();
                
                echo $roleResult;
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