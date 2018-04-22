<?php 
require_once("dbcontroller.php");
$db_handle = new DBController();

if (!empty($_POST["announcementid"]) && !empty($_POST["username"])) {

    $deleteID = $_POST["announcementid"];
    $login_user = $_POST["username"];
    $query = $db_handle->getConn()->prepare("SELECT * FROM announcement WHERE announcementID = " . $deleteID);
    $query->execute();
    $oriAnnouncementResult = $query->fetchAll();

    if($oriAnnouncementResult[0][0] != "")
    {
        
        $query = $db_handle->getConn()->prepare("UPDATE announcement SET postDeleted = true, editedDate = NOW(), editedBy = :username WHERE announcementID = :deleteID);
        $query->bindParam(":username", $login_user);
        $query->bindParam(":deleteID", $deleteID);
        $result = $query->execute();

        if($result = true) {
            //unset($_POST);
            echo "deletesuccess";
        } else {
            echo "deletefailure";
            //$error_message = "Problem in editing announcement. Try Again!";	
        }

    } else {
        echo "CANNOT FIND ".$_POST["delete_postID"];
    }
}

?>