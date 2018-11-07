<?php

require_once("../dbcontroller.php");
$db_handle = new DBController();
//control tenders result field visibility
$showDivFlag = false;

//Retrieve all originating sources
$getSourceQuery = $db_handle->getConn()->prepare("SELECT DISTINCT originatingSource FROM scrapped_tender");
$getSourceQuery->execute();
$getSourceResults = $getSourceQuery->fetchAll();

$errorMessage = "";
$errorpresence = false;
$resultMsg = "";

$reference = "";
$title = "";
$originatingSource = "";
$closingDateFrom = "";
$closingDateTo = "";

//Check which information to return
if(isset($_POST["searchReference"]) || isset($_POST["searchTitle"]) || isset($_POST["searchOriginatingSource"]) || isset($_POST["searchClosingDateFrom"]) || isset($_POST["searchClosingDateTo"])) {
    
    $reference= $_POST["searchReference"];
    $title = $_POST["searchTitle"];
    $originatingSource = $_POST["searchOriginatingSource"];
    $closingDateFrom = $_POST["searchClosingDateFrom"];
    $closingDateTo = $_POST["searchClosingDateTo"];
    

    $initialQuery = "SELECT * FROM scrapped_tender";
    $laterQuery = "";
    
    //Query Mix and Match depending on how many search fields entered by user
    if($reference != ""){
        $laterQuery = $laterQuery . " WHERE reference LIKE :reference";
        
    }
    if($title != ""){
        if($reference == ""){
            $laterQuery = $laterQuery . " WHERE title LIKE :title";
        }else{
            $laterQuery = $laterQuery . " AND title LIKE :title";
        }
    }
    if($originatingSource != ""){
        if($originatingSource == "all"){
            if($reference == "" && $title == ""){
                $laterQuery = "";
                //reset originatingSource variable to not aversely affect the remaining if else
                $originatingSource = "";
            }else{
                $laterQuery = $laterQuery . "";
                $originatingSource = "";
            }
        }else{
            if($reference == "" && $title == ""){
                $laterQuery = $laterQuery . " WHERE originatingSource LIKE :originatingSource";
            }else{
                $laterQuery = $laterQuery . " AND originatingSource LIKE :originatingSource";
            }
        }
    }
    if($closingDateFrom != "" && $closingDateTo != ""){
        if($reference == "" && $title == "" && $originatingSource == ""){
            $laterQuery = $laterQuery . " WHERE closingDate BETWEEN :closingDateFrom AND :closingDateTo";
        }else{
            $laterQuery = $laterQuery . " AND closingDate BETWEEN :closingDateFrom AND :closingDateTo";
        }
    }else if($closingDateFrom != "" && $closingDateTo == ""){
        if($reference == "" && $title == "" && $originatingSource == ""){
            $laterQuery = $laterQuery . " WHERE closingDate > :closingDateFrom";
        }else{
            $laterQuery = $laterQuery . " AND closingDate > :closingDateFrom";
        }
    }else if($closingDateTo != "" && $closingDateFrom == ""){
        if($reference == "" && $title == "" && $originatingSource == ""){
            $laterQuery = $laterQuery . " WHERE closingDate < :closingDateTo";
        }else{
            $laterQuery = $laterQuery . " AND closingDate < :closingDateTo";
        }
    }
    
    
    
    //bind paramaters with the format %searchinput% for SQL LIKE clause
    $query = $db_handle->getConn()->prepare($initialQuery . $laterQuery);
    if($reference != ""){
        $referencequery = "%" . $reference . "%";
        $query->bindParam(":reference", $referencequery);
    }
    if($title != ""){
        $titlequery = "%" . $title . "%";
        $query->bindParam(":title", $titlequery);
    }
    if($originatingSource != ""){
        if($originatingSource != "all"){
        $originatingSourcequery = "%" . $originatingSource . "%";
        $query->bindParam(":originatingSource", $originatingSourcequery);
        }
    }
    if($closingDateFrom != "" & $closingDateTo != ""){
        $query->bindParam(":closingDateFrom", $closingDateFrom);
        $query->bindParam(":closingDateTo", $closingDateTo);
    }else if($closingDateFrom != "" && $closingDateTo == ""){
        $query->bindParam(":closingDateFrom", $closingDateFrom);
    }else if($closingDateTo != "" && $closingDateFrom == ""){
        $query->bindParam(":closingDateTo", $closingDateTo);
    }
    
    $query->execute();
    $result = $query->fetchAll();
        
        
    if($result != null) {
        /*$resultString = json_encode($result);
        echo $resultString;*/
        $resultMsg = "Search success";
        //Get the bookmark status for each tenders
        $showDivFlag = "true";
        $bookmarkQuery = $db_handle->getConn()->prepare("SELECT * FROM tender_bookmark WHERE username = :username");
        $bookmarkQuery->bindParam(":username", $login_user);
        $bookmarkQuery->execute();
        $bookmarkResult = $bookmarkQuery->fetchAll();

        foreach ($result as $key => $tender) {
            //Set default bookmark image as non-bookmark
            $result[$key]["bookmarkImg"] = "bookmark.png";
            foreach ($bookmarkResult as $bookmark) {
                if (isset($tender["reference"])) {
                    if ($tender["reference"] == $bookmark["tenderReferenceNumber"]) {
                        $result[$key]["bookmarkImg"] = "bookmarkfilled.png";
                    }
                } else {
                    if ($tender["title"] == $bookmark["tenderTitle"]) {
                        $result[$key]["bookmarkImg"] = "bookmarkfilled.png";
                    }
                }
            }
        }
        
    } else {
        $errorMessage = "No tenders available";
    }
}else if(isset($_POST["searchKeyword"])){
    $keywordSearch = $_POST["searchKeyword"];
    $keywordSearchquery = "%" . $keywordSearch . "%";
    $query = $db_handle->getConn()->prepare("SELECT * FROM scrapped_tender WHERE reference LIKE :keywordsearch OR title LIKE :keywordsearch OR category LIKE :keywordsearch OR originatingSource LIKE :keywordsearch OR agency LIKE :keywordsearch");
    
    $query->bindParam(":keywordsearch", $keywordSearchquery);
    $query->execute();
    $result = $query->fetchAll();
    
    if($result != null) {
        /*$resultString = json_encode($result);
        echo $resultString;*/
        $resultMsg = "Search success";
        //Get the bookmark status for each tenders
        $showDivFlag = "true";
        $bookmarkQuery = $db_handle->getConn()->prepare("SELECT * FROM tender_bookmark WHERE username = :username");
        $bookmarkQuery->bindParam(":username", $login_user);
        $bookmarkQuery->execute();
        $bookmarkResult = $bookmarkQuery->fetchAll();

        foreach ($result as $key => $tender) {
            //Set default bookmark image as non-bookmark
            $result[$key]["bookmarkImg"] = "bookmark.png";
            foreach ($bookmarkResult as $bookmark) {
                if (isset($tender["reference"])) {
                    if ($tender["reference"] == $bookmark["tenderReferenceNumber"]) {
                        $result[$key]["bookmarkImg"] = "bookmarkfilled.png";
                    }
                } else {
                    if ($tender["title"] == $bookmark["tenderTitle"]) {
                        $result[$key]["bookmarkImg"] = "bookmarkfilled.png";
                    }
                }
            }
        }
        
    } else {
        $errorMessage = "No tenders available";
    }
}else{
    //View all tenders
    $query = $db_handle->getConn()->prepare("SELECT * FROM scrapped_tender ORDER BY tenderSource");
    $query->execute();
    $result = $query->fetchAll();
}

?>