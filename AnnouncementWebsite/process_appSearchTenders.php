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
            $laterQuery = $laterQuery . " WHERE closingDate BETWEEN STR_TO_DATE(:closingDateFrom, '%m/%d/%Y') AND STR_TO_DATE(:closingDateTo, '%m/%d/%Y')";
        }else{
            $laterQuery = $laterQuery . " AND closingDate BETWEEN STR_TO_DATE(:closingDateFrom, '%m/%d/%Y') AND STR_TO_DATE(:closingDateTo, '%m/%d/%Y')";
        }
    }else if($closingDateFrom != "" && $closingDateTo == ""){
        if($reference == "" && $title == "" && $originatingSource == ""){
            $laterQuery = $laterQuery . " WHERE closingDate > STR_TO_DATE(:closingDateFrom, '%m/%d/%Y')";
        }else{
            $laterQuery = $laterQuery . " AND closingDate > STR_TO_DATE(:closingDateFrom, '%m/%d/%Y')";
        }
    }else if($closingDateTo != "" && $closingDateFrom == ""){
        if($reference == "" && $title == "" && $originatingSource == ""){
            $laterQuery = $laterQuery . " WHERE closingDate < STR_TO_DATE(:closingDateTo, '%m/%d/%Y')";
        }else{
            $laterQuery = $laterQuery . " AND closingDate < STR_TO_DATE(:closingDateTo, '%m/%d/%Y')";
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
    $query = $db_handle->getConn()->prepare("SELECT DISTINCT originatingSource FROM scrapped_tender");
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