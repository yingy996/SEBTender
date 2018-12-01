<?php 
require_once("../dbcontroller.php");
$db_handle = new DBController();
$appGetID = "";

if (isset($_GET["announcementid"])) {
    
    $appGetID = $_GET["announcementid"];
    
    $query = $db_handle->getConn()->prepare("SELECT announcementID, announcementTitle, announcementContent, publishedDate, editedDate, editedBy, postedBy FROM announcement WHERE postDeleted = 0 AND announcementID = ". $appGetID);

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
} else {
    echo "anouncementid no found";
}

?>