<?php
require_once("../dbcontroller.php");

if (isset($_SESSION["normaluser_login"])) {
    //Retrieve tender bookmarks from database
    $user = sanitizeInput($_SESSION["normaluser_login"]);
    
	$db_handle = new DBController();
    $query = $db_handle->getConn()->prepare("SELECT * FROM tender_bookmark WHERE username = :username");
    $query->bindParam(":username", $user);
    $query->execute();
    $result = $query->fetchAll();
    
    if (count($result) > 0) {
        $tenderArr = array();
        //Retrieve tender details for each of the bookmarked tenders
        foreach($result as $tenderBookmark) {
            if(isset($tenderBookmark["tenderReferenceNumber"]) && $tenderBookmark["tenderReferenceNumber"] != "null") {
                $tenderQuery = $db_handle->getConn()->prepare("SELECT * FROM scrapped_tender WHERE reference = :reference");
                $tenderQuery->bindParam(":reference", $tenderBookmark["tenderReferenceNumber"]);
            } else {
                $tenderQuery = $db_handle->getConn()->prepare("SELECT * FROM scrapped_tender WHERE title = :title");
                $tenderQuery->bindParam(":title", $tenderBookmark["tenderTitle"]);
            }
            
            $tenderQuery->execute();
            $tenderResult = $tenderQuery->fetchAll();
            
            if (count($tenderResult) > 0) {
                $tenderArr[] = $tenderResult[0];
            } else {
                //Special Checking for &amp; & in tender reference
                $escapedReference = preg_replace("/\b&amp;\b/", "&", $tenderBookmark["tenderReferenceNumber"]);
                $tenderQuery2 = $db_handle->getConn()->prepare("SELECT * FROM scrapped_tender WHERE reference = :reference");
                $tenderQuery2->bindParam(":reference", $escapedReference);
                
                $tenderQuery2->execute();
                $tenderResult2 = $tenderQuery2->fetchAll();
                
                if (count($tenderResult2) > 0) {
                    $tenderArr[] = $tenderResult2[0];
                }
            }
        }
    }
} else {
    header("location: login.php");
    exit();
}
?>