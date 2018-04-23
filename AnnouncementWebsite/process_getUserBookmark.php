<?php
require_once("dbcontroller.php");
$db_handle = new DBController();

if(isset($_POST["username"])) {
    $username = $_POST["username"];
    
    //query database to get bookmark of the user
    $query = $db_handle->getConn()->prepare("SELECT * FROM tender_bookmark WHERE username = :username ORDER BY bookmarkDate DESC");
    
    $username = sanitizeInput($_POST["username"]);
    $query->bindParam(":username", $username);

    $query->execute();
    $results = $query->fetchAll();
    
    if(!empty($results)) {
        $resultString = json_encode($results);
        echo $resultString;
    } else {
        echo "No bookmark found";
    }
}

function sanitizeInput($data) {
    $data = trim($data);
    $data = stripslashes($data);
    $data = htmlspecialchars($data);

    return $data;
}
?>