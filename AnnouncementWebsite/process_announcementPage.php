<?php 
    $editID = "";
	require_once("dbcontroller.php");
	$db_handle = new DBController();

    $query = $db_handle->getConn()->prepare("SELECT announcementID, announcementTitle, announcementContent, publishedDate, editedDate, editedBy, postedBy FROM announcement WHERE postDeleted = false ORDER BY publishedDate DESC");
    $query->execute();
    $results = $query->fetchAll();

    if(!empty($_POST["editButton"])){
        $editID = $_POST["edit_postID"];
        //$_SESSION["editID"] = "$editID";
    }


?>
