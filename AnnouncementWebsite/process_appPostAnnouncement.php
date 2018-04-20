<?php
require_once("dbcontroller.php");
$announcement_title = $announcement_content = "";
$announcement_titleerr = $announcement_contenterr = "";
$randomID = "";
$errorpresence = false;
$resultMsg = $errorMsg = ""; //login message
$error_message = "";

if(isset($_POST["username"]) && isset($_POST["password"])){

    //Authentication for admin login 
    try{
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

            //takes plain text password and hashed string password from database user_account table as arguments, return success message when admin account verified
            if(password_verify($password, $result2)){
                //if authentication success
                $resultMsg = "Login success";

                /* Announcement title Validation */
                if(empty($_POST["title"])){
                    $announcement_titleerr = "Please enter an announcement title";
                    $errorpresence = true;
                }else{
                    $announcement_title = sanitizeInput($_POST["title"]);
                }

                /* Announcement content Validation */
                if(empty($_POST["content"])){
                    $announcement_contenterr = "Please enter an announcement content";
                    $errorpresence = true;
                }else{
                    $announcement_content = sanitizeInput($_POST["content"]);
                }

                if($error_message == "" && $errorpresence == false) {
                    $db_handle = new DBController();

                    $digits = 7;
                    $randomID = rand(pow(10, $digits-1), pow(10, $digits)-1);

                    //insert announcement into database
                    $query = $db_handle->getConn()->prepare("INSERT INTO announcement (announcementID, announcementTitle, announcementContent, publishedDate, editedDate, editedBy, postedBy) VALUES
                    (:randomID, :announcement_title, :announcement_content, NOW(), NULL, NULL, :login_user)");
                    $query->bindParam(":randomID", $randomID);
                    $query->bindParam(":announcement_title", $announcement_title);
                    $query->bindParam(":announcement_content", $announcement_content);
                    $query->bindParam(":login_user", $username);

                    $result = $query->execute();
                    if($result == true) {
                        echo "You have succesfully posted your announcement!";	
                    } else {
                        echo "Problem in posting announcement. Please try again!";	
                    }
                } else {
                    echo "Failed to post announcement. Please ensure all the fields are entered.";
                }
            } else {
                echo "Invalid password"; //login failed
            }

        } else {
            echo "Invalid username"; //login failed
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