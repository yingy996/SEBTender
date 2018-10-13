<?php
require_once("dbcontroller.php");
$db_handle = new DBController();

//Check which information to return
if(isset($_POST["searchReference"]) || isset($_POST["searchTitle"]) || isset($_POST["searchOriginatingSource"]) || isset($_POST["searchClosingDateFrom"]) || isset($_POST["searchClosingDateTo"])) {
    $reference = $_POST["searchReference"];
    $title = $_POST["searchTitle"];
    $originatingSource = $_POST["searchOriginatingSource"];
    $closingDateFrom = $_POST["searchClosingDateFrom"];
    $closingDateTo = $_POST["searchClosingDateTo"];
    
    $initialQuery = "SELECT * FROM scrapped_tender WHERE";
    $laterQuery = "";
    
    if($reference != ""){
        //$laterQuery = $laterQuery . " reference = " . $reference;
        $laterQuery = $laterQuery . " reference LIKE :reference";
        
    }
    if($title != ""){
        if($reference == ""){
            //$laterQuery = $laterQuery . " title = " . $title;
            $laterQuery = $laterQuery . " title LIKE :title";
        }else{
            //$laterQuery = $laterQuery . " AND title = " . $title;
            $laterQuery = $laterQuery . " AND title LIKE :title";
        }
    }
    if($originatingSource != ""){
        if($reference == "" && $title == ""){
            //$laterQuery = $laterQuery . " originatingSource = " . $originatingSource;
            $laterQuery = $laterQuery . " originatingSource LIKE :originatingSource";
        }else{
            //$laterQuery = $laterQuery . " AND originatingSource = " . $originatingSource;
            $laterQuery = $laterQuery . " AND originatingSource LIKE :originatingSource";
        }
    }
    if($closingDateFrom != "" && $closingDateTo != ""){
        if($reference == "" && $title == "" && $originatingSource == ""){
            //$laterQuery = $laterQuery . " closingDate BETWEEN " . $closingDateFrom . " AND " . $closingDateTo;
            $laterQuery = $laterQuery . " closingDate BETWEEN :closingDateFrom AND :closingDateTo";
        }else{
            //$laterQuery = $laterQuery . " AND closingDate BETWEEN " . $closingDateFrom . " AND " . $closingDateTo;
            $laterQuery = $laterQuery . " AND closingDate BETWEEN :closingDateFrom AND :closingDateTo";
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
        $originatingSourcequery = "%" . $originatingSource . "%";
        $query->bindParam(":originatingSource", $originatingSourcequery);
    }
    if($closingDateFrom != "" & $closingDateTo != ""){
        $query->bindParam(":closingDateFrom", $closingDateFrom);
        $query->bindParam(":closingDateTo", $closingDateTo);
    }
    
    
    //$query->bindParam(":closingDate", $pollID);
    
    
    
    $query->execute();
    $result = $query->fetchAll();
        
        
    if($result != null) {
        $resultString = json_encode($result);
        echo $resultString;
    } else {
        $error_message = "No tenders available";
        
        echo $error_message;
    }
}else if(isset($_POST["retrieveOriginatingSource"])){
    $query = $db_handle->getConn()->prepare("SELECT DISTINCT originatingSource FROM scrapped_tenders");
    $query->execute();
    $result = $query->fetchAll();
    if($result[0][0] != ""){
        $resultString = json_encode($result);
        echo $resultString;
    }
}else if(isset($_POST["tenderKeywordSearch"])){
    $keywordSearch = $_POST["tenderKeywordSearch"];
    $keywordSearchquery = "%" . $keywordSearch . "%";
    $query = $db_handle->getConn()->prepare("SELECT * FROM scrapped_tender WHERE reference LIKE :keywordsearch OR title LIKE :keywordsearch OR category LIKE :keywordsearch OR originatingSource LIKE :keywordsearch OR agency LIKE :keywordsearch");
    
    //SELECT * FROM `scrapped_tender` WHERE `reference` LIKE '%kontrak%' OR `title` LIKE '%kontrak%' OR `category` LIKE '%kontrak%' OR `originatingSource` LIKE '%kontrak%' OR `agency` LIKE '%kontrak%'
    $query->bindParam(":keywordsearch", $keywordSearchquery);
    $query->execute();
    $result = $query->fetchAll();
    
    if($result != null){
        $resultString = json_encode($result);
        echo $resultString;
    }else{
        $error_message = "No tenders found with the keyword";
        echo $error_message;
    }
    
}else{
    $error_message = "Problems in post parameters";
    echo $error_message;
}



?>