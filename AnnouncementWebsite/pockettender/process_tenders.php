<?php
require_once("../dbcontroller.php");
$db_handle = new DBController();

if (isset($_POST["sortOrder"]) && isset($_POST["sortField"])) {
    $sortArr = array("Closing Date" => "closingDate", "Originating Source" => "originatingSource");
    $sortOrder = sanitizeInput($_POST["sortOrder"]);
    $sortOrder = strtoupper($sortOrder);
    $sortField = sanitizeInput($_POST["sortField"]);
    
    $query = $db_handle->getConn()->prepare("SELECT * FROM scrapped_tender ORDER BY $sortArr[$sortField] $sortOrder");
} else {
    $query = $db_handle->getConn()->prepare("SELECT * FROM scrapped_tender ORDER BY tenderSource");
}

$query->execute();
$results = $query->fetchAll();

$tenderSourceQuery = $db_handle->getConn()->prepare("SELECT originatingSource FROM scrapped_tender GROUP BY originatingSource");
$tenderSourceQuery->execute();
$sourceResults = $tenderSourceQuery->fetchAll();

if (isset($_POST["filterJSON"]) && $_POST["filterJSON"] != "") {
    //Get the tenders based on the filter
    $filterJson = $_POST["filterJSON"];
    $filterArr = json_decode($filterJson);
    $filteredTenders = array();
    foreach ($results as $tender) {
        foreach ($filterArr as $sourceFilter) {
            if ($tender["originatingSource"] == $sourceFilter) {
                $filteredTenders[] = $tender;
                break;
            }
        }
    }
    $results = $filteredTenders;
}

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