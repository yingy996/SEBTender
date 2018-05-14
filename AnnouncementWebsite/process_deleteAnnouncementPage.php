<?php
require_once("dbcontroller.php");
$db_handle = new DBController();

if (isset($_POST["delete_postID"])) {
    $deleteID = $_POST["delete_postID"];
    //echo $deleteID;
} else {
    header("location: index.php");
}

if (!isset($_POST["login_user"])) {
    header("location: login.php");
} else {
    $login_user = $_POST["login_user"];
}

if (!empty($_POST["delete_postID"])) {

    $deleteID = $_POST["delete_postID"];

    $query = $db_handle->getConn()->prepare("SELECT * FROM announcement WHERE announcementID = " . $deleteID);
    $query->execute();
    $oriAnnouncementResult = $query->fetchAll();

    if($oriAnnouncementResult[0][0] != "")
    {
        
        $query = $db_handle->getConn()->prepare("UPDATE announcement SET postDeleted = true, editedDate = NOW(), editedBy = '$login_user' WHERE announcementID = ". $deleteID);
        $result = $query->execute();

        if($result == true) {
            //unset($_POST);
            echo "<script type='text/javascript'>alert('Announcement post has been successfully deleted.');</script>";
            header("refresh:0, index.php");
        } else {
            echo "<script type='text/javascript'>alert('Error occured while trying to delete announcement post. Please try again.');</script>";
            header("refresh:0, index.php");
            //$error_message = "Problem in editing announcement. Try Again!";	
        }

    } else {
        echo "CANNOT FIND ".$_POST["delete_postID"];
    }
}

    /*
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

        //echo $announcement_titleerr." ".$announcement_contenterr;

        if($error_message == "" && $errorpresence == false) {

            //$announcement_title = sanitizeInput($_POST["title"]);
            //$announcement_content = sanitizeInput($_POST["content"]);

            //require_once("dbcontroller.php");
            //$db_handle = new DBController();

            $announcement_title = sanitizeInput($_POST["title"]);
            $announcement_content = sanitizeInput($_POST["content"]);
            $login_user = $_SESSION["user_login"];
            $deleteID = $_POST["delete_postID"];

            //echo $announcement_title." ".$announcement_content." ".$login_user." ".$deleteID;

            $query = $db_handle->getConn()->prepare("UPDATE announcement SET announcementTitle = '$announcement_title', announcementContent = '$announcement_content', editedDate = NOW(), editedBy = '$login_user' WHERE announcementID = $deleteID");
            //$query->bindParam(":announcement_title", $announcement_title);
            //$query->bindParam(":announcement_content", $announcement_content);
            //$query->bindParam(":login_user", $login_user);
            //$query->bindParam(":deleteID", $deleteID);



            $result = $query->execute();
            if($result = true) {
                $error_message = "";
                $success_message = "You have succesfully edited your announcement!";	
                unset($_POST);
                header("refresh:3, index.php");
            } else {
                $error_message = "Problem in editing announcement. Try Again!";	
            }

        } else {
            $error_message = "Failed to submit";
        }

    } 
} else {
    header("Location: index.php");
}*/

?>