<?php
$announcement_title = $announcement_content = "";
$announcement_titleerr = $announcement_contenterr = "";
$errorpresence = false;
$error_message = "";
$editID = "";

$editID = $_SESSION["editID"];
$oriannouncementTitle = $oriannouncementContent = "";

$query = $db_handle->getConn()->prepare("SELECT announcementTitle, announcementContent FROM announcement WHERE announcementID = " . $editID);
$query->execute();
$oriAnnouncementResult = $query->fetchAll();

if($oriAnnouncementResult[0][0] != "")
{
    $oriannouncementTitle = $oriAnnouncementResult[0]["announcementTitle"];
    $oriannouncementContent = $oriAnnouncementResult[0]["announcementContent"];
                        
}

if(!empty($_POST["editAnnouncementbutton"])) {
	
     
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
        
        require_once("dbcontroller.php");
		$db_handle = new DBController();
        
        
		
        $query = $db_handle->getConn()->prepare("UPDATE announcement SET announcementTitle = :announcement_title, announcementContent = :announcement_content, editedDate = NOW(), editedBy = :login_user WHERE announcementID = :editID");
        $query->bindParam(":announcement_title", $announcement_title);
        $query->bindParam(":announcement_content", $announcement_content);
        $query->bindParam(":login_user", $login_user);
        $query->bindParam(":editID", $editID);


        $result = $query->execute();
        if($result = true) {
            $error_message = "";
            $success_message = "You have succesfully edited your announcement!";	
            unset($_POST);
            header("refresh:3; url=postannouncement.php");
        } else {
            $error_message = "Problem in editing announcement. Try Again!";	
        }

	} else {
        $error_message = "Failed to submit";
    }
}


?>