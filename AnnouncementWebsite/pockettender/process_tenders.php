<?php
require_once("../dbcontroller.php");
$db_handle = new DBController();

$query = $db_handle->getConn()->prepare("SELECT * FROM scrapped_tender ORDER BY tenderSource");
$query->execute();
$results = $query->fetchAll();

//Get the bookmark status for each tenders
if (count($results) > 0) {
    $bookmarkQuery = $db_handle->getConn()->prepare("SELECT * FROM tender_bookmark WHERE username = :username");
    $bookmarkQuery->bindParam(":username", $login_user);
    $bookmarkQuery->execute();
    $bookmarkResult = $bookmarkQuery->fetchAll();
    
    foreach ($results as $key => $tender) {
        //Set default bookmark image as non-bookmark
        $results[$key]["bookmarkImg"] = "bookmark.png";
        foreach ($bookmarkResult as $bookmark) {
            if (isset($tender["reference"])) {
                if ($tender["reference"] == $bookmark["tenderReferenceNumber"]) {
                    $results[$key]["bookmarkImg"] = "bookmarkfilled.png";
                } else {
                    $escapedReference = preg_replace("/\b&amp;\b/", "&", $bookmark["tenderReferenceNumber"]);
                    
                    if ($tender["reference"] == $escapedReference) {
                        $results[$key]["bookmarkImg"] = "bookmarkfilled.png";
                    }
                }
            } else {
                if ($tender["title"] == $bookmark["tenderTitle"]) {
                    $results[$key]["bookmarkImg"] = "bookmarkfilled.png";
                }
            }
            
        }
    }
}
?>