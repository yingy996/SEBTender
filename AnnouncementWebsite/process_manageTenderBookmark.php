<?php
require_once("dbcontroller.php");
$db_handle = new DBController();

//add bookmark
if(isset($_POST["username"]) && isset($_POST["tenderReferenceNumber"]) && isset($_POST["tenderTitle"]) && isset($_POST["closingDate"])) {
    $username = sanitizeInput($_POST["username"]);
    $tenderRefNumber = sanitizeInput($_POST["tenderReferenceNumber"]);
    $tenderTitle = sanitizeInput($_POST["tenderTitle"]);
    $closingDate = date('Y-m-d H:i:s',strtotime($_POST["closingDate"]));
    
    try {
        $query = $db_handle->getConn()->prepare("INSERT INTO tender_bookmark (bookmarkID, username, tenderReferenceNumber, tenderTitle, isAvailable, bookmarkDate, closingDate) VALUES (NULL, :username, :tendeRefNumber, :tenderTitle, 1, NOW(), :closingDate)");
    
        $query->bindParam(":username", $username);
        $query->bindParam(":tendeRefNumber", $tenderRefNumber);
        $query->bindParam(":tenderTitle", $tenderTitle);
        $query->bindParam(":closingDate", $closingDate);
        $result = $query->execute();

        if($result == true) {
            echo "Success";	
        } else {
            echo "Failed";	
        }
    } catch (PDOException $e) {
        echo "Error: " . $e->getMessage();
    }
} elseif (isset($_POST["username"]) && isset($_POST["tenderReferenceNumber"]) && isset($_POST["isDelete"])) { 
    //remove bookmark from database
    $username = sanitizeInput($_POST["username"]);
    $tenderRefNumber = sanitizeInput($_POST["tenderReferenceNumber"]);
    $isDeleteAction = sanitizeInput($_POST["isDelete"]);
    
    try {
        $query = $db_handle->getConn()->prepare("DELETE FROM tender_bookmark WHERE tender_bookmark.username = :username AND tender_bookmark.tenderReferenceNumber = :tendeRefNumber");
    
        $query->bindParam(":username", $username);
        $query->bindParam(":tendeRefNumber", $tenderRefNumber);
        $result = $query->execute();
        
        if($result == true) {
            echo "Success";	
        } else {
            echo "Failed";	
        }
    } catch (PDOException $e) {
        echo "Error: " . $e->getMessage();
    }
} else {
    echo "Missing parameters";
}

function sanitizeInput($data) {
    $data = trim($data);
    $data = stripslashes($data);
    $data = htmlspecialchars($data);

    return $data;
}
?>