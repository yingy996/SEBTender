<?php
if (isset($_SESSION["user_login"])) {
    $announcement_title = $announcement_content = "";
    $announcement_titleerr = $announcement_contenterr = "";
    $randomID = "";
    $errorpresence = false;
    $error_message = "";
    
    if(!empty($_POST["postAnnouncementbutton"])) {
    
    
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
    
            require_once("../dbcontroller.php");
            $db_handle = new DBController();
    
            $digits = 7;
            $randomID = rand(pow(10, $digits-1), pow(10, $digits)-1);
    
            date_default_timezone_set("Asia/Kuching");
            $currentDate = date("Y-m-d G:i:s");
            $query = $db_handle->getConn()->prepare("INSERT INTO announcement (announcementID, announcementTitle, announcementContent, publishedDate, editedDate, editedBy, postedBy) VALUES
            (:randomID, :announcement_title, :announcement_content, :date, NULL, NULL, :login_user)");
            $query->bindParam(":randomID", $randomID);
            $query->bindParam(":announcement_title", $announcement_title);
            $query->bindParam(":announcement_content", $announcement_content);
            $query->bindParam(":login_user", $login_user);
            $query->bindParam(":date", $currentDate);
    
            $result = $query->execute();
            if($result = true) {
                require_once("process_sendNotification.php");
                sendNotification("Announcement: " . $announcement_title);
                $error_message = "";
                $success_message = "You have succesfully posted your announcement!";	
                unset($_POST);
                header("refresh:3; index.php");
            } else {
                $error_message = "Problem in posting announcement. Try Again!";	
            }
    
        } else {
            $error_message = "Failed to submit";
        }
    }
} else {
    header("location: login.php");
    exit();
}
?>