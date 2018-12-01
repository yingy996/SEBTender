<?php 
	require_once("../dbcontroller.php");
	$db_handle = new DBController();

    if(isset($_POST["username"]) && isset($_POST["password"])) {
        //if received request from application, check user login status and return result
        try {
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
                    $selectQuery = $db_handle->getConn()->prepare("SELECT username, administratorName, administratorEmail, role FROM administrator WHERE isDeleted = false ORDER BY administratorName ASC");
                    $selectQuery->execute();
                    $results = $selectQuery->fetchAll();
                    
                    if(!empty($results)) {
                        $returnResult = json_encode($results);
                        echo $returnResult;
                    } else {
                        echo "No user found";
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
    } else {
        $query = $db_handle->getConn()->prepare("SELECT username, administratorName, administratorEmail, role FROM administrator WHERE isDeleted = false ORDER BY administratorName ASC");
        $query->execute();
        $results = $query->fetchAll();
    }    
?>
