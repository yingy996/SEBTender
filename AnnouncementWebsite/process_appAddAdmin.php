<?php
require_once("dbcontroller.php");

$errorMessage = "";
$errorpresence = false;
$name = $email = $username = $password = $confPassword = $role = "";

if(isset($_POST["adminUsername"]) && isset($_POST["adminPassword"])){
    try {
        $adminUsername = $_POST["adminUsername"];
        $adminPassword = $_POST["adminPassword"];
        
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
                //if user authentication succeeded, validate the inputs
                if(empty($_POST["name"])){
                    $errorMessage .= "Name must not be empty. ";
                    $errorpresence = true;
                }else{
                    if (!preg_match("/^[a-zA-Z ]*$/",$_POST["name"])) {
                        $errorMessage .= "Name must be alphabetic. ";
                        $errorpresence = true;
                    }else{
                        $name = sanitizeInput($_POST["name"]);
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
                }

                if($_POST["password"] != $_POST["confPassword"]){ 
                    $errorMessage .= "Confirm Password does not match. "; 
                    $errorpresence = true;
                }else{
                    $password = password_hash(sanitizeInput($_POST["password"]), PASSWORD_DEFAULT);
                }

                if(empty($_POST["confPassword"])){
                    $errorMessage .= "Confirm Password must not be empty. ";
                    $errorpresence = true;
                }

                if(empty($_POST["role"])) {
                    $errorMessage .= "Please select user role. ";
                    $errorpresence = true;
                } else {
                    $role = sanitizeInput($_POST["role"]);
                }
                
                //Register user if inputs are valid
                if (!$errorpresence) {
                    //validate uniqueness of username
                    $checkUsernameQuery = $db_handle->getConn()->prepare("SELECT username FROM administrator WHERE username= :username AND isDeleted = false");
                    $checkUsernameQuery->bindParam(":username", $username);
                    $checkUsernameQuery->execute();
                    $checkUsernameResult = count($checkUsernameQuery->fetchAll());
                    
                    if($checkUsernameResult > 0){
                        $errorMessage .= "Username already exists";
                        echo $errorMessage;
                    } else {
                        //if inputs are valid and username is unique, register user into database
                        $insertQuery = $db_handle->getConn()->prepare("INSERT INTO administrator (administratorID, username, password, administratorName, administratorEmail, role) VALUES (NULL, :username, :password, :adminname, :adminemail, :role)");

                        $insertQuery->bindParam(":username", $username);
                        $insertQuery->bindParam(":password", $password);
                        $insertQuery->bindParam(":adminname", $name);
                        $insertQuery->bindParam(":adminemail", $email);
                        $insertQuery->bindParam(":role", $role);

                        $insertResult = $insertQuery->execute();
                        
                        if ($insertResult == true) {
                            echo "Account successfully registered!";
                        } else {
                            $errorMessage = "Error occured while registering. Please try again! ";
                            echo $errorMessage;
                        }
                    }
                } else {
                    echo $errorMessage;
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
    echo "Please login to add new admin user";
}

function sanitizeInput($data) {
    $data = trim($data);
    $data = stripslashes($data);
    $data = htmlspecialchars($data);

    return $data;
}
?>