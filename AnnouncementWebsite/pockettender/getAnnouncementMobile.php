<?php 
require_once("../dbcontroller.php");
	$db_handle = new DBController();

    $query = $db_handle->getConn()->prepare("SELECT announcementID, announcementTitle, announcementContent, publishedDate, editedDate, editedBy, postedBy FROM announcement WHERE postDeleted = 0 ORDER BY publishedDate DESC");
    
    $query->execute();
    $results = $query->fetchAll();
    $resultstring = "";

    if(!empty($results)){
        
            $resultstring = json_encode($results);
            echo $resultstring;
        
        
    }
    else
    {
        echo "noanouncement";
    }
?>