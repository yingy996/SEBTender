<?php
require_once("dbcontroller.php");
$db_handle = new DBController();

if (isset($_POST["tenderReferenceNo"]) && isset($_POST["tenderTitle"])) {
    $tenderRef = sanitizeInput($_POST["tenderReferenceNo"]);
    $tenderTitle = sanitizeInput($_POST["tenderTitle"]);
    
    //Retrieve tender item from database
    if ($tenderRef == "None") {
        $query = $db_handle->getConn()->prepare("SELECT * FROM scrapped_tender WHERE title = :title");
        $query->bindParam(":title", $tenderTitle);
    } else {
        $query = $db_handle->getConn()->prepare("SELECT * FROM scrapped_tender WHERE reference = :reference");
        $query->bindParam(":reference", $tenderRef);
    }
    
    $query->execute();
    $result = $query->fetchAll();
    
    if ($result != null) {
        $resultString = json_encode($result[0]);
        echo $resultString;
    } else {
        echo "Tender not found";
    }
} else {
    echo "Missing parameter(s)";
}

function sanitizeInput($data) {
    $data = trim($data);
    $data = stripslashes($data);
    $data = htmlspecialchars($data);

    return $data;
}
?>