<?php
require_once("dbcontroller.php");

$announcement_title = $announcement_content = "";
$announcement_titleerr = $announcement_contenterr = "";
$errorpresence = false;
$error_message = "";
$editID = "";
$oriannouncementTitle = $oriannouncementContent = "";

if (isset($_GET["edit_postID"])) {
    $editID = $_GET["edit_postID"];
} else {
    header("location: announcementPage.php");
}
 echo $_SESSION["user_login"];
if (!empty($_GET["edit_postID"])) {

    echo $_GET["edit_postID"];
    $editID = $_GET["edit_postID"];

    //$editID = $_SESSION["editID"];

    $query = $db_handle->getConn()->prepare("SELECT announcementTitle, announcementContent FROM announcement WHERE announcementID = " . $editID);
    $query->execute();
    $oriAnnouncementResult = $query->fetchAll();

    if($oriAnnouncementResult[0][0] != "")
    {
        $oriannouncementTitle = $oriAnnouncementResult[0]["announcementTitle"];
        $oriannouncementContent = $oriAnnouncementResult[0]["announcementContent"];

    } else {
        echo "CANNOT FIND ".$_GET["edit_postID"];
    }

    if(!empty($_POST["submitEdit"])) {

        //Announcement title Validation
        if(empty($_POST["title"])){
            $announcement_titleerr = "Please enter an announcement title";
            $errorpresence = true;
        }

        //Announcement content Validation 
        if(empty($_POST["content"])){
            $announcement_contenterr = "Please enter an announcement content";
            $errorpresence = true;
        }

        echo $announcement_titleerr." ".$announcement_contenterr;

        if($error_message == "" && $errorpresence == false) {

            //$announcement_title = sanitizeInput($_POST["title"]);
            //$announcement_content = sanitizeInput($_POST["content"]);

            //require_once("dbcontroller.php");
            //$db_handle = new DBController();

            $announcement_title = sanitizeInput($_POST["title"]);
            $announcement_content = sanitizeInput($_POST["content"]);
            $login_user = $_SESSION["user_login"];
            $editID = $_GET["edit_postID"];
            
            echo $announcement_title." ".$announcement_content." ".$login_user." ".$editID;
            
            $query = $db_handle->getConn()->prepare("UPDATE announcement SET announcementTitle = '$announcement_title', announcementContent = '$announcement_content', editedDate = NOW(), editedBy = '$login_user' WHERE announcementID = $editID");
            //$query->bindParam(":announcement_title", $announcement_title);
            //$query->bindParam(":announcement_content", $announcement_content);
            //$query->bindParam(":login_user", $login_user);
            //$query->bindParam(":editID", $editID);

            

            $result = $query->execute();
            if($result = true) {
                $error_message = "";
                $success_message = "You have succesfully edited your announcement!";	
                unset($_POST);
                header("refresh:3");
            } else {
                $error_message = "Problem in editing announcement. Try Again!";	
            }

        } else {
            $error_message = "Failed to submit";
        }

    } 
} else {
    header("Location: announcementPage.php");
}   
?>