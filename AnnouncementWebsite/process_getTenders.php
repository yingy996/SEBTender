<?php
require_once("dbcontroller.php");
$db_handle = new DBController();

$query = $db_handle->getConn()->prepare("SELECT * FROM scrapped_tender ORDER BY tenderSource ASC");

$query->execute();
$results = $query->fetchAll();

if (!empty($results)) {
    $resultString = json_encode($results);
    echo $resultString;
} else {
    echo "No tender found";
}
?>