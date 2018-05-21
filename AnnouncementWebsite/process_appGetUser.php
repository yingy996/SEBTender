<?php
require_once("dbcontroller.php");

if(isset($_POST["username"]) && isset($_POST["password"])) {
    try {
        //check user login status and return result
        $adminUsername = $_POST["username"];
        $adminPassword = $_POST["password"];

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
                //if user authentication succeeded, return the result    
                //get user info
                $query = $db_handle->getConn()->prepare("SELECT * FROM administrator WHERE username = :username ORDER BY administratorName ASC");
                $query->bindParam(":username", $adminUsername);
                $query->execute();

                $results = $query->fetchAll();

                if(!empty($results)) {
                    $returnResult = json_encode($results);
                    echo $returnResult;
                } else {
                    echo "No user found";
                }
            }
        } else {
            echo "User not found!";
        }
    } catch (PDOException $e) {
        echo "Error: " . $e->getMessage();
    }   
} else {
    echo "Invalid Parameters";
}
?>